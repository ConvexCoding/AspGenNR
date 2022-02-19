using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using gClass;
using gExtensions;
using gGraphExt;

namespace AspGen
{
    public partial class ExtendedSourcePlot : Form
    {
        double[,] data2d;
        double[] ydata;
        double[] ytheory;
        double[] xdata;
        double xmin, xmax;
        double maxdata;
        readonly double[] maxs = new double[] { 0.005, 0.006, 0.01, 0.015, 0.02, 0.025, 0.03, 0.04, 0.05, 0.06, 0.075, 0.08, 0.09, 0.100, 0.15, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.75, 0.8, 0.9, 1.00, 1.25, 1.5, 2.0, 2.5, 3.0, 5.0 };
        readonly double[] incs = new double[] { 0.001, 0.001, 0.002, 0.005, 0.005, 0.05, 0.01, 0.01, 0.01, 0.02, 0.025, 0.02, 0.03, 0.025, 0.05, 0.05, 0.5, 0.1, 0.1, 0.1, 0.2, 0.25, 0.2, 0.3, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1.0 };
        long totalrays;
        double esize;

        public ExtendedSourcePlot(Lens lens, double Refocus, double fiber_radius, double[,] indata = null)
        {
            InitializeComponent();

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            esize = fiber_radius;
            var angin = (fiber_radius / Math.Abs(lens.EFL));
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            if (indata == null)
            {
                //vlist = GenRayData(lens, Refocus, fiber_radius);
                var vxyz = lens.ap.GenerateFibonacciRayVectors(Properties.Settings.Default.ExtSrcNoofBaseRays);
                var vlist = RTM.TraceWithAngles(lens, Refocus, vxyz, angin);
                data2d = RTM.ProcessRayData(vlist, lens, Refocus, fiber_radius);
            }
            else
            {
                data2d = indata;
            }

            long rayct = Properties.Settings.Default.ExtSrcNoofAngles * Properties.Settings.Default.ExtSrcNoofBaseRays;
            ydata = data2d.SliceDataMidPt(border: Properties.Settings.Default.ExtScrVerticalAverage1D);
            double xbins = (double)((ydata.Length - 1) / 4);
            double ppr = (double)rayct / (Math.PI * xbins * xbins);
            ydata.DivideDoubles(ppr);

            //xdata = GenerateXArray();
            xdata = ydata.Length.GenerateLinearArray(esize * 2);
            maxdata = ydata.Max();

            if (ydata != null)
                ChartData(lens, Refocus, xdata, ydata);

            chart1.Series[0].Name = "Raw Data";
            watch.Stop();
            timerLabel.Text = "Gen && Chart Time: " + watch.ElapsedMilliseconds + " ms for " + rayct.ToString("n0") + " rays";
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private double[] GenerateXArray( )
        {
            int s = ydata.Length;
            double[] x = new double[s];
            xmin = -2 * esize;
            xmax = 2 * esize;
            double stepsize = (xmax - xmin) / (s - 1);
            for (int i = 0; i < s; i++)
                x[i] = xmin + i * stepsize;

            return x;
        }

        private void ChartData(Lens lens, double Refocus, double[] xs, double[] ys)
        {
            int s = ys.Length;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            totalrays = Properties.Settings.Default.ExtSrcNoofAngles *
                Properties.Settings.Default.ExtSrcNoofBaseRays;
            chart1.Series[0].Points.DataBindXY(xs, ys);
            SetHorizontalScaleMax(double.Parse(xs.Max().ToString("f2")));
            //SetVecticalScaleMax(1.0);

            var color = Color.LightGray;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = color;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = color;

            this.chart1.ChartAreas[0].AxisX.Title = "Vertical (Y-Axis) Cross Section (micron)";
            this.chart1.ChartAreas[0].AxisY.Title = "Relative Intensity";

            this.chart1.Titles[0].Text = "Image Cross Section for Extended Source";

            string t1 = lens.Material + " " + lens.Type + " Lens, " +
                        lens.Diameter.ToString("f1") + " mm x " +
                        lens.EFL.ToString("f2") + " mm EFL";
            this.chart1.Titles[1].Text = t1;

            chart1.ChartAreas[0].RecalculateAxesScale();

            string rayctstr = "Ray Count = " + totalrays.ToString("n0");
            AddRayCountAnnotation(rayctstr);
        }

        private void AddRayCountAnnotation(string anote)
        {
            chart1.Annotations.Clear();
            var ca = this.chart1.ChartAreas[0];
            var deltax = 0.01 * (ca.AxisX.Maximum - ca.AxisX.Minimum);
            var deltay = 0.01 * (ca.AxisY.Maximum - ca.AxisY.Minimum);

            //TextAnnotation rayct = new TextAnnotation();
            RectangleAnnotation rayct = new RectangleAnnotation();
            rayct.Text = anote;
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

            RectangleAnnotation expsize = new RectangleAnnotation();
            expsize.Text = "Exp Focus Dia:  " + (2*esize).ToString("F3") + " mm";
            expsize.ForeColor = Color.Black;
            expsize.Font = new Font("Arial", 10);
            expsize.AnchorDataPoint = chart1.Series[0].Points[0];
            expsize.AnchorY = ca.AxisY.Maximum - deltay;
            expsize.AnchorX = ca.AxisX.Maximum - deltax;
            expsize.AnchorAlignment = ContentAlignment.TopRight;
            expsize.AllowMoving = true;
            expsize.LineColor = Color.FromArgb(200, Color.LightBlue);
            expsize.BackColor = Color.FromArgb(200, Color.White);
            this.chart1.Annotations.Add(expsize);
        }

        private void SetHorizontalScaleMax(double xsc)
        {
            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 4;
            //chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";
        }

        private void SetVecticalScaleMax(double ysc)
        {
            int index = SetInterval(ysc);
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = ysc;
            chart1.ChartAreas[0].AxisY.Interval = incs[index];
            //chart1.ChartAreas[0].AxisY.Interval = SetTicInterval(ysc, 10);
            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.00e0";
        }

        private int SetInterval(double range)
        {
            int i;
            for (i = 0; i < maxs.Count(); i++)
                if (maxs[i] >= range)
                    return i;
            return (maxs.Count() - 1);
        }


        // Context Menu Activation and Option Clicks
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));

        }

        private void copyDataToClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x, y");
            for (int i = 0; i < ydata.Length; i++)
            {
                sb.AppendLine(xdata[i].ToString("f5") + ", " + ydata[i].ToString());
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Data copied to Clipboard!");
        }

        private void graphToBitmap_Click(object sender, EventArgs e)
        {
            int width = panel1.Size.Width;
            int height = panel1.Size.Height;

            Bitmap bm = new Bitmap(width, height);
            panel1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));
            Clipboard.SetImage(bm as Image);
            MessageBox.Show("Panel copied to Clipboard!");
        }

        public void autoScaleButton_Click(object sender, EventArgs e)
        {
            chart1.AutoScaleChart();
        }

        int whichaxis = 0;
        int caller = 2;
        private void setHorizontalAxis_Click(object sender, EventArgs e)
        {
            whichaxis = 0;
            //.ChartAreas[0].AxisX.SetAxisScale("Set Horizontal Axis Parameters");
            SetAxisScalewithNotify(chart1.ChartAreas[0].AxisX, "Set Horizontal Axis Parameters");
        }

        private void setVerticalAxisButton_Click(object sender, EventArgs e)
        {
            whichaxis = 1;
            //chart1.ChartAreas[0].AxisY.SetAxisScale("Set Vertical Axis Parameters");
            SetAxisScalewithNotify(chart1.ChartAreas[0].AxisY, "Set Vertical Axis Parameters");
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
            string rayctstr = "Ray Count = " + (totalrays / 2).ToString("n0");
            AddRayCountAnnotation(rayctstr);
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
            this.chart1.Update();
        }

        public void LocalAutoScaleChart(string callertitle)
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
            string rayctstr = "Ray Count = " + totalrays.ToString("n0");
            AddRayCountAnnotation(rayctstr);
        }

        private void copyFormToClip_Click(object sender, EventArgs e)
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

        /*
            *double getfermipwr ( double rin, double rhalf )
            {
                double outpwr;
                double x = 3;	
                outpwr = rin/rhalf - log(1+exp(BETA*(rin/rhalf-1)))/BETA;	
                return ( outpwr*outpwr );
            }
         */

        private void smoothNoise_Click(object sender, EventArgs e)
        {            
            List<double> xsmooth = new List<double>();
            List<double> ysmooth = new List<double>();

            int nopts = 3;
            for(int i = nopts - 1; i < xdata.Length; i++)
            {
                double x = 0;
                double y = 0;
                for(int k = 0; k < nopts; k++)
                {
                    x += xdata[i - k];
                    y += ydata[i - k];
                }
                xsmooth.Add(x / nopts);
                ysmooth.Add(y / nopts);
            }
            xdata = xsmooth.ToArray();
            ydata = ysmooth.ToArray();

            FolderandAverageData();
            chart1.Series[0].Name = "Smoothed";
            UpdateChartWithRevisedData();
        }

        private void FolderandAverageData()
        {
            // search for zero X
            int ctr = xdata.Length / 2;
            int nopts = xdata.Length;
            var xzeroest = Properties.Settings.Default.ExtScrBinSize / 10;

            double[] yfold = new double[nopts];
            for (int i = 0; i < ctr; i++)
            { 
                yfold[i] = (ydata[i] + ydata[nopts - 1 - i]) / 2;
                yfold[nopts - 1 - i] = yfold[i];
            }
            yfold[ctr] = (ydata[ctr] + ydata[ctr + 1] + ydata[ctr - 1]) / 3 ;
            ydata = yfold;
        }

        private void resetRawData_Click(object sender, EventArgs e)
        {
            long rayct = Properties.Settings.Default.ExtSrcNoofAngles * Properties.Settings.Default.ExtSrcNoofBaseRays;
            ydata = data2d.SliceDataMidPt(border: Properties.Settings.Default.ExtScrVerticalAverage1D);
            double xbins = (double)((ydata.Length - 1) / 4);
            double ppr = (double)rayct / (Math.PI * xbins * xbins);
            ydata.DivideDoubles(ppr);
            //xdata = GenerateXArray();
            xdata = ydata.Length.GenerateLinearArray(esize * 2);
            chart1.Series[0].Name = "Raw Data";
            UpdateChartWithRevisedData();
        }

        private void fermiButton_Click(object sender, EventArgs e)
        {
            ydata = FDF.FittoFermiDirac(xdata, ydata, 20, 0.1, 1.0);
            UpdateChartWithRevisedData();
            chart1.Series[0].Name = "Fermi Dirac Fit";
        }

        private void theoryButton_Click(object sender, EventArgs e)
        {
            ytheory = new double[xdata.Length];
            for (int i = 0; i < ytheory.Length; i++)
            {
                ydata[i] = 0;
                if ((xdata[i] <= 0) && (xdata[i] >= -esize))
                    ytheory[i] = 1;
                if ((xdata[i] >= 0) && (xdata[i] <= esize))
                    ytheory[i] = 1;
            }

            chart1.Series.Add("Theory");
            chart1.Series[1].Points.DataBindXY(xdata, ytheory);
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[1].Color = Color.DarkBlue;
            chart1.Series[1].BorderDashStyle = ChartDashStyle.Dot;

            chart1.Legends.Add("Main");
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;

        }

        private void UpdateChartWithRevisedData ( )
        {
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[0].Points.DataBindXY(xdata, ydata);
            int s = ydata.Length;
            double binsize = Properties.Settings.Default.ExtScrBinSize;
            double minbin = (double)((s - 1) / 2) * binsize;

            chart1.ChartAreas[0].RecalculateAxesScale();

            string rayctstr = "Ray Count = " + totalrays.ToString("n0");
            AddRayCountAnnotation(rayctstr);
        }
    }
}
