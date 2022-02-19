using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using gClass;
using gGraphExt;
using gExtensions;
using System.IO;

using gFFTExtensions;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace AspGen
{
    public partial class PSF_DiffractionBased : Form
    {
        readonly double[] maxs = new double[] { 0.005,  0.01, 0.015,  0.02, 0.025, 0.03, 0.04, 0.05, 0.06, 0.075, 0.08, 0.09, 0.100, 0.15,  0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.75, 0.8, 0.9, 1.00, 1.25, 1.5, 2.0, 2.5, 3.0, 5.0 };
        readonly double[] incs = new double[] { 0.001, 0.002, 0.005, 0.005,  0.05, 0.01, 0.01, 0.01, 0.02, 0.025, 0.02, 0.03, 0.025, 0.05, 0.05, 0.05,  0.1, 0.1, 0.1, 0.2, 0.25, 0.2, 0.3, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1.0  };

        double[] xpts;
        double[] ypts;
        double[] yptstheory;
        double maxxtarget;
        double maxpsf;
        public PSF_DiffractionBased(Lens lens, double Refocus)
        {
            InitializeComponent();

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            GenerateData(lens, Refocus);
            ChartInitialData( );

            watch.Stop();
            long rayct = Properties.Settings.Default.PSFTotalGridSize;
            timerLabel.Text = "Gen && Chart Time: " + watch.ElapsedMilliseconds + " ms for " + rayct.ToString("n0") + " x " + rayct.ToString("n0") + " Grid Size";
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void GenerateData(Lens lens, double Refocus)
        {
            // Powers of 2
            //0   1	  2	  3	  4	  5	  6	   7	8	 9	 10	     11	     12
            //1   2   4   8   16  32  64  128  256  512  1024    2048    4096

            int s = Properties.Settings.Default.PSFBeamGridSize;
            int totalsize = Properties.Settings.Default.PSFTotalGridSize;


            // generate amplitude mask &  generate wavefront map
            var datamask = GenerateAmplitudeMask(lens);
            var datawfe = GenerateWFEMap(lens, Refocus);

            //Clipboard.SetText(datamask.ConvertDoublesToStringBuilder().ToString());
            var rows = datawfe.GetLength(0);
            var cols = datawfe.GetLength(1);

            Complex[,] data = new Complex[datawfe.GetLength(0), datawfe.GetLength(1)];
            Complex[,] datadl = new Complex[datawfe.GetLength(0), datawfe.GetLength(1)];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    data[r, c] = datamask[r, c] * Complex.Exp(new Complex(0, datawfe[r, c]));
                    datadl[r, c] = datamask[r, c];
                }

            var datapad = data.PadComplex(totalsize);
            var datapaddl = datadl.PadComplex(totalsize);

            AForge.Math.FourierTransform.FFT2(datapad, AForge.Math.FourierTransform.Direction.Forward);
            AForge.Math.FourierTransform.FFT2(datapaddl, AForge.Math.FourierTransform.Direction.Forward);

            // do circular shift on matrix
            var datashift = datapad.FFT2Shift();
            var datapaddlshift = datapaddl.FFT2Shift();

            // multiple matrix times it's conjugate
            var psf = datashift.MultiConjugate();
            var psfdl = datapaddlshift.MultiConjugate();

            // remove real portion from complex matrix to get intensity data
            var psfreal = psf.Real();
            var psfrealdl = psfdl.Real();

            // int s = Properties.Settings.Default.PSFBeamGridSize;
            //
            // grab the central 200 pixels or whatever user has selected for input psf beam grid size. 
            // this is just a guess and might not work in all situations.
            // a more elegant way of determining slice length is needed.
            //
            var psfline = psfreal.PSFLine().SliceMid(s * 2);
            var psflinedl = psfrealdl.PSFLine().SliceMid(s * 2);

           // Clipboard.SetText("totalreal, " + (psfreal.NormalizeData(1.0)).SumData().ToString() + "\n" +
           //                     "totaldl, " + (psfrealdl.NormalizeData(1.0)).SumData().ToString());

            // normalize data to peak of diff limited psf
            var maxdl = psflinedl.Max();
            psflinedl = psflinedl.ScaleData(maxdl);
            psfline = psfline.ScaleData(maxdl);

            // scale factor for image pixels 
            // sf = wl * F# / Q
            // F# is the F-number of the lens = EFL/Diameter
            // Q is the fill factor ratio which is just total pixel grid / portion of grid containing beam
            //

            double pixelscale = (lens.WL / 1000.0) * (lens.EFL / (2 * lens.ap)) / ((double)totalsize / (double)s);

            int width = psfline.Count();
            double[] xs = new double[width];
            double xends = -pixelscale * width / 2;
            for (int w = 0; w < width; w++)
                xs[w] = xends + pixelscale * w;
            //var xss = gMath.GenArray(-pixelscale * width / 2, pixelscale, psfline.Count());

            xpts = xs;
            ypts = psfline;
            yptstheory = psflinedl;


            // diameter of airy ring = 2.44 * wl * EFL / (2 * ap)
            // factor 0.003 = 2.5 * 2.44 / 1000 (for wl) / 2 (for ap)
            //
            // theoretical spot size times 2.5 to make sure that we 
            // plot data over large enough interval
            //
            maxxtarget = 0.003 * lens.WL * Math.Abs(lens.EFL) / lens.ap;
        }

        private void ChartInitialData( )
        {
            chart1.Series[0].Points.DataBindXY(xpts, ypts);
            chart1.Series[0].BorderWidth = 3;

            var n1 = GetPosition(maxxtarget);
            chart1.ChartAreas[0].AxisX.Minimum = -maxs[n1];
            chart1.ChartAreas[0].AxisX.Maximum = maxs[n1];
            chart1.ChartAreas[0].AxisX.Interval = incs[n1];
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            //var n2 = GetPosition(ypts.Max());
            //if (n2 > maxs.Count() - 1)
            //    n2 = maxs.Count() - 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1.0;
            chart1.ChartAreas[0].AxisY.Interval = 0.2;

            chart1.Series[0].Name = "PSF of Designed Lens";

            maxpsf = ypts.Max();
            AddPeaktAnnotation(maxpsf);
        }

        private void AddPeaktAnnotation(double maxpsf)
        {
            chart1.Annotations.Clear();
            var ca = this.chart1.ChartAreas[0];
            var deltax = 0.01 * (ca.AxisX.Maximum - ca.AxisX.Minimum);
            var deltay = 0.01 * (ca.AxisY.Maximum - ca.AxisY.Minimum);

            //TextAnnotation rayct = new TextAnnotation();
            RectangleAnnotation rayct = new RectangleAnnotation();
            rayct.Text = "Peak PSF:  " + maxpsf.ToString("f4");
            rayct.ForeColor = Color.Black;
            rayct.Font = new Font("Arial", 10);
            rayct.AnchorDataPoint = chart1.Series[0].Points[0];
            rayct.AnchorY = ca.AxisY.Maximum - deltay;
            rayct.AnchorX = ca.AxisX.Minimum + deltax;
            rayct.AnchorAlignment = ContentAlignment.TopLeft;
            rayct.AllowMoving = true;
            rayct.LineColor = Color.FromArgb(200, Color.LightBlue);
            rayct.BackColor = Color.FromArgb(200, Color.White);
            this.chart1.Annotations.Add(rayct);
        }

        private int GetPosition(double q)
        {
            for (int i = 0; i < maxs.Count(); i++)
                if (maxs[i] > Math.Abs(q))
                    return (i);
            return maxs.Count() - 1;
        }

        private double[,] GenerateWFEMap(Lens lens, double refocus, int beamgridsize = 0)
        {
            int s;
            if (beamgridsize == 0)
                s = Properties.Settings.Default.PSFBeamGridSize;
            else
                s = beamgridsize;

            int center = s / 2;
            var main = new double[s, s];
            double hwidth = lens.ap;

            double scale = (double)(s / 2) / lens.ap;

            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                {
                    main[r, c] = 0;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                    {
                        var wfe = hypot.CalcRayWFE(lens, refocus);
                        main[r, c] = wfe.OPD * 2 * Math.PI;
                    }
                }
            return main;
        }

        private double[,] GenerateAmplitudeMask(Lens lens, int beamgridsize = 0)
        {
            int s;
            if (beamgridsize == 0)
                s = Properties.Settings.Default.PSFBeamGridSize;
            else
                s = beamgridsize;

            int center = s / 2;
            var mask = new double[s, s];
            double scale = (double)(s / 2) / lens.ap;

            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                {
                    mask[r, c] = double.NaN;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                        mask[r, c] = 1.0;
                    else
                        mask[r, c] = 0.0;
                }
            return mask;
        }

        private void copyChartToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
                MessageBox.Show("Panel copied to Clipboard!");
            }
        }

        private void saveChartToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    bm.Save(saveFile.FileName);
                }
            }
        }

        private void saveDataToCSVFile_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                StringBuilder sb = DataToStrings();

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter swriter = new StreamWriter(saveFile.FileName))
                    {
                        swriter.Write(sb.ToString());
                    }
                }
            }
        }

        private StringBuilder DataToStrings( )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("X(mm), Irradiance, Irradiance Theory");

            for (int i = 0; i < xpts.Count(); i++)
            {
                sb.AppendLine(xpts[i].ToString() + ", " + ypts[i].ToString() + ", " + yptstheory[i].ToString());
            }
            return sb;
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
        }

        private void saveDataToClipBoard_Click(object sender, EventArgs e)
        {
            StringBuilder sb = DataToStrings();
            Clipboard.SetText(sb.ToString());
        }

        private void showTheory_Click(object sender, EventArgs e)
        {
            if (chart1.Series.Count == 2)
            {
                return;
            }
            else
            {
                chart1.Series.Add("PSF of Perfect Lens");
                chart1.Series[1].ChartType = SeriesChartType.Line;
                chart1.Series[1].Points.DataBindXY(xpts, yptstheory);
                chart1.Series[1].BorderWidth = 3;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 1.0;
                chart1.ChartAreas[0].AxisY.Interval = 0.2;

                chart1.Legends.Add(new Legend("Legend"));

                // Set Docking of the Legend chart to the Default Chart Area.
                chart1.Legends["Legend"].Docking = Docking.Top;
                chart1.Legends["Legend"].Alignment = StringAlignment.Center;
                chart1.Series[0].Legend = "Legend";
                chart1.Series[0].IsVisibleInLegend = true;
            }
            AddPeaktAnnotation(maxpsf);
        }

        private void copyFormClipboard_Click(object sender, EventArgs e)
        {
            var tb = QueryFormDimensions.GetWindowRectangle(this.Handle);
            using (Bitmap bitmap = new Bitmap(tb.RSize.Width, tb.RSize.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(tb.P0, Point.Empty, tb.RSize);

                }
                Clipboard.SetImage(bitmap);
            }
            MessageBox.Show("Form copied to Clipboard!");
        }

        public void autoScaleButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.DataBindXY(xpts, ypts);

            var n1 = GetPosition(maxxtarget);
            chart1.ChartAreas[0].AxisX.Minimum = -maxs[n1];
            chart1.ChartAreas[0].AxisX.Maximum = maxs[n1];
            chart1.ChartAreas[0].AxisX.Interval = incs[n1];

            var n2 = GetPosition(ypts.Max());
            if (n2 > maxs.Count() - 1)
                n2 = maxs.Count() - 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = maxs[n2];
            chart1.ChartAreas[0].AxisY.Interval = incs[n2];

            chart1.Series[0].Name = "PSF of Designed Lens";

            AddPeaktAnnotation(maxpsf);
        }

        int whichaxis = 0;
        int caller = 3;
        private void setHorizontalScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            whichaxis = 0;
            //.ChartAreas[0].AxisX.SetAxisScale("Set Horizontal Axis Parameters");
            SetAxisScalewithNotify(chart1.ChartAreas[0].AxisX, "Set Horizontal Axis Parameters");
            AddPeaktAnnotation(maxpsf);
        }

        private void setVerticalScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            whichaxis = 1;
            //chart1.ChartAreas[0].AxisY.SetAxisScale("Set Vertical Axis Parameters");
            SetAxisScalewithNotify(chart1.ChartAreas[0].AxisY, "Set Vertical Axis Parameters");
            AddPeaktAnnotation(maxpsf);
        }

        public void SetAxisScalewithNotify(Axis axis, string title)
        {
            ChartAxis ca = new ChartAxis(axis.Minimum, axis.Maximum, axis.Interval);
            SetChartAxisNotify scaxis = new SetChartAxisNotify(caller, title, ca);
            scaxis.ShowDialog(this);
            if (scaxis.isOk == true)
            {
                if (scaxis.autoScaleOn == false)
                {
                    ChartAxis ctemp = scaxis.CAOut;
                    axis.Minimum = ctemp.AxisMin;
                    axis.Maximum = ctemp.AxisMax;
                    axis.Interval = ctemp.MajorInterval;
                }
                else
                {
                    axis.Minimum = double.NaN;
                    axis.Maximum = double.NaN;
                    axis.Interval = double.NaN;
                }
            }
            chart1.ChartAreas[0].RecalculateAxesScale();
            AddPeaktAnnotation(maxpsf);
        }

        public void NotifyMe(ChartAxis ca)
        {
            if (whichaxis == 0)
            {
                chart1.ChartAreas[0].AxisX.Minimum = ca.AxisMin;
                chart1.ChartAreas[0].AxisX.Maximum = ca.AxisMax;
                chart1.ChartAreas[0].AxisX.Interval = ca.MajorInterval;
            }

            if (whichaxis == 1)
            {
                chart1.ChartAreas[0].AxisY.Minimum = ca.AxisMin;
                chart1.ChartAreas[0].AxisY.Maximum = ca.AxisMax;
                chart1.ChartAreas[0].AxisY.Interval = ca.MajorInterval;
            }
            AddPeaktAnnotation(maxpsf);
            this.chart1.Update();
        }

        public void LocalAutoScaleChart(string callertitle = "Horizontal and Vertical")
        {
            if (callertitle.Contains("Horizontal"))
            {
                chart1.ChartAreas[0].AxisX.Minimum = double.NaN;
                chart1.ChartAreas[0].AxisX.Maximum = double.NaN;
                chart1.ChartAreas[0].AxisX.Interval = double.NaN;
            }

            if (callertitle.Contains("Vertical"))
            {
                chart1.ChartAreas[0].AxisY.Minimum = double.NaN;
                chart1.ChartAreas[0].AxisY.Maximum = double.NaN;
                chart1.ChartAreas[0].AxisY.Interval = double.NaN;
            }
            AddPeaktAnnotation(maxpsf);
        }

    }
}
