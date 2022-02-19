using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    public partial class SpotDiagram2D : Form
    {
        Size basesize = new Size(1, 1);
        int raycount = 0;
        Lens lensglobal;
        double refocusglobal;
        string pointtype = "Hex";
        double maxscalefactor;
        List<Vector3D> rays;
        double offangle = 0.0;
        Vector3D InitDir = new Vector3D(0, 0, 1);
        //bool IsOffAngle = false;
        string units = "mm";

        public SpotDiagram2D(Lens lensp, double Refocus, bool OffAxis = false, double OffAngleIn = 0.0)
        {
            InitializeComponent();
            lensglobal = lensp;
            refocusglobal = Refocus;
            if (OffAxis)
            {
                //IsOffAngle = true;
                offangle = Math.Tan(OffAngleIn * Math.PI / 180.0);
                InitDir = new Vector3D(0, offangle, Math.Sqrt(1 - offangle * offangle));
            }
            rays = new List<Vector3D>();

            var vlist = GenUniformPositions(lensp.ap);
            //var vlist = GenFibonacciPositions(lensp.ap);
            //var vlist = GenHexPositions(lensp.ap);

            foreach (Vector3D v in vlist)
            {
                var P = RTM.Trace_3D(v, InitDir, lensp, Refocus);
                rays.Add(P);
            }
            pointtype = "Uniform";
            DisplayChart(lensp, rays);
            this.Width = this.Width + 5;  // do this to initialize resize event
        }

        private void DisplayChart(Lens lensp, List<Vector3D> rays)
        {
            var mm = rays.MinMaxVectorList();
            double axiscalefactor = 1;
            units = "mm";
            if (mm.MaxY < 0.001e-6)
            {
                axiscalefactor = 1000;
                units = "μm";
            }

            FormatChart();
            AddTitles(units);
            AddWavelengthPoints(lensglobal.WL, axiscalefactor);
            AddRayPoints(rays, axiscalefactor);

            // *****************************************
            var xsc = (mm.MaxY.YscaleValue() * axiscalefactor);
            if (xsc < (lensp.WL * axiscalefactor / 2000))
                xsc = (((lensp.WL / 2000) * axiscalefactor).YscaleValue());

            maxscalefactor = xsc;
            SetChartScale(xsc);

            chart1.Titles[0].Text = "Spot Diagram - λ: " + lensp.WL.ToString("f3") + " μm,  EFL:  " + lensp.EFL.ToString("F2") + " mm";
            UpdateAnnotations(mm.MaxX - mm.MinX, -xsc, xsc);
        }

        private void UpdateAnnotations(double pv, double ymin, double ymax)
        {
            this.chart1.Annotations.Clear();
            this.chart1.ChartAreas[0].RecalculateAxesScale();
            var anote = "Geometric Spot Size: " + pv.ToString("g3") + " " + units;
            AddTextAnnotation(0, ymax - 0.02 * (ymax - ymin), anote, ContentAlignment.TopCenter);
            var anote2 = pointtype + " Grid Ray Count: " + raycount.ToString();
            AddTextAnnotation(0, ymin + 0.02 * (ymax - ymin), anote2, ContentAlignment.BottomCenter);
        }

        private void SetChartScale(double scale)
        {
            string lformat = "g" + Properties.Settings.Default.NoOfDigitsSpotDiagLabels.ToString();
            double xsc = double.Parse(scale.ToString("E3"));  // use this hack to make sure that the labels are rounded properly

            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 2;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = lformat;

            chart1.ChartAreas[0].AxisY.Minimum = -xsc;
            chart1.ChartAreas[0].AxisY.Maximum = xsc;
            chart1.ChartAreas[0].AxisY.Interval = xsc / 2;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = lformat;
        }


        #region  // Generate source ray arrangement: Uniform, Hex, Fibonacci

        private List<Vector3D> GenFibonacciPositions(double aperture)
        {
            List<Vector3D> vlist = new List<Vector3D>();
            var num = (double)Properties.Settings.Default.SpotDiagramRadialSamples;

            int nrays = (int)Math.Round(Math.PI * num * num);   // use this to estimate total number of rays in circle
                                                                // when sampled as a uniform grid of 50 div per aperture

            /* math needed to calc fibonacci positions
            from numpy import arange, pi, sin, cos, arccos
            n = 50
            i = arange(0, n, dtype = float) + 0.5
            phi = arccos(1 - 2 * i / n)
            goldenRatio = (1 + 5 * *0.5) / 2
            theta = 2 pi* i / goldenRatio
            x, y, z = cos(theta) * sin(phi), sin(theta) * sin(phi), cos(phi);
            */

            double goldenratio = (1 + Math.Sqrt(5)) / 2;
            for (int i = 0; i < nrays; i++)
            {
                double x = (double)i / goldenratio - Math.Truncate((double)i / goldenratio);
                double y = (double)i / nrays;
                List<double> theta = new List<double>();
                List<double> rad = new List<double>();
                if (Math.Sqrt(x * x + y * y) <= 10)
                {
                    theta.Add(2 * Math.PI * x);
                    rad.Add(Math.Sqrt(y));
                    double xs = (aperture * 0.1 * 10 * Math.Sqrt(y) * Math.Cos(2 * Math.PI * x));
                    double ys = (aperture * 0.1 * 10 * Math.Sqrt(y) * Math.Sin(2 * Math.PI * x));
                    vlist.Add(new Vector3D(xs, ys, 0));
                }
            }
            raycount = vlist.Count;
            return vlist;
        }

        private List<Vector3D> GenUniformPositions (double aperture)
        {
            List<Vector3D> vlist = new List<Vector3D>();
            int num = Properties.Settings.Default.SpotDiagramRadialSamples;
            int num2 = Properties.Settings.Default.SpotDiagramSpokesInc;

            for (double y = -aperture; y <= aperture * 1.001; y += aperture / num)
                for (double x = -aperture; x <= aperture * 1.001; x += aperture / num)
                {
                    var h = Math.Sqrt(x * x + y * y);
                    if (h <= aperture)
                    {
                        vlist.Add(new Vector3D(x, y, 0));
                    }
                }
            raycount = vlist.Count;
            return vlist;
        }

        private List<Vector3D> GenHexPositions(double aperture)
        {
            List<Vector3D> vlist = new List<Vector3D>();
            int hexrays = Properties.Settings.Default.SpotDiagramRadialSamples;
            double deltaxy = aperture / (double)hexrays;
            int rowcount = 0;
            for (double y = -aperture - deltaxy * 2; y <= (aperture + deltaxy * 2) * 1.001; y += deltaxy)
            {
                for (double x = -aperture - deltaxy * 2; x <= (aperture + deltaxy * 2) * 1.001; x += deltaxy)
                {
                    double delta = 0;
                    if ((rowcount % 2) != 0)
                        delta = deltaxy / 2;
                    var h = Math.Sqrt((delta + x) * (delta + x) + y * y);
                    if (h <= aperture)
                    {
                        vlist.Add(new Vector3D(x + delta, y, 0));
                    }
                }
                rowcount++;
            }
            raycount = vlist.Count;
            return vlist;
        }

        private void useFibonacciPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vlist = GenFibonacciPositions(lensglobal.ap);
            List<Vector3D> rays = new List<Vector3D>();
            foreach (Vector3D v in vlist)
            {
                var P = RTM.Trace_3D(v, new Vector3D(0, offangle, 1), lensglobal, refocusglobal);
                rays.Add(P);
            }
            pointtype = "Fibonacci";

            DisplayChart(lensglobal, rays);
        }

        private void useHexPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vlist = GenHexPositions(lensglobal.ap);
            List<Vector3D> rays = new List<Vector3D>();
            foreach (Vector3D v in vlist)
            {
                var P = RTM.Trace_3D(v, new Vector3D(0, offangle, 1), lensglobal, refocusglobal);
                rays.Add(P);
            }
            pointtype = "Hexagonal";
            DisplayChart(lensglobal, rays);
        }
        private void useUniformtGridPtsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vlist = GenUniformPositions(lensglobal.ap);
            List<Vector3D> rays = new List<Vector3D>();
            foreach (Vector3D v in vlist)
            {
                var P = RTM.Trace_3D(v, new Vector3D(0, offangle, 1), lensglobal, refocusglobal);
                rays.Add(P);
            }
            pointtype = "Uniform";
            DisplayChart(lensglobal, rays);
        }

        #endregion
 

        private void AddWavelengthPoints(double wl, double axiscalefactor)
        {
            // *************************************************
            // plot circle showing wavelength size

            List<double> angles = new List<double>();
            int ainc = Properties.Settings.Default.SpotDiagramSpokesInc;
            for (int i = 0; i <= 360; i += ainc)
                angles.Add((double)i);

            double yr2 = (wl * 0.001) / 2;

            List<PointD> pds2 = new List<PointD>();
            for (int i = 0; i < angles.Count(); i++)
                pds2.Add(new PointD(0, yr2 * axiscalefactor).RotatePointD(angles[i]));

            chart1.Series[1].Points.Clear();
            foreach (PointD p in pds2)
                chart1.Series[1].Points.AddXY(p.X, p.Y);
        }

        private void AddRayPoints(List<Vector3D> rays, double axiscalefactor)
        {
            chart1.Series[0].Points.Clear();
            foreach (Vector3D r in rays)
            {
                chart1.Series[0].Points.AddXY(r.X * axiscalefactor, r.Y * axiscalefactor);
            }
        }

        private void FormatChart( )
        {
            chart1.Series[0].ChartType = SeriesChartType.Point;
            chart1.Series[0].MarkerStyle = MarkerStyle.Star4;
            chart1.Series[0].MarkerColor = Color.Red;
            chart1.Series[0].MarkerSize = 2;

            chart1.Series[1].Color = Color.DarkBlue;
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[1].BorderDashStyle = ChartDashStyle.Dash;
        }

        private void AddTitles (string units)
        {
            chart1.ChartAreas[0].AxisY.Title = "Vertical Spread" + " (" + units + ")";
            chart1.ChartAreas[0].AxisX.Title = "Horizontal Spread" + " (" + units + ")";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);
        }

        private void AddTextAnnotation(double x0, double y0, string anotestring, ContentAlignment ca)
        {
            RectangleAnnotation anote = new RectangleAnnotation();

            anote.AxisX = chart1.ChartAreas[0].AxisX;
            anote.AxisY = chart1.ChartAreas[0].AxisY;
            anote.AnchorX = x0;
            anote.AnchorY = y0;
            
            anote.AllowMoving = true;

            anote.Text = anotestring;
            anote.ForeColor = Color.Black;
            anote.LineColor = Color.LightGray;
            anote.Font = new Font("Arial", 10);

           anote.BackColor = Color.FromArgb(200, Color.White);

            anote.AnchorAlignment = ca;
            anote.Alignment = ca;
            chart1.Annotations.Add(anote);
        }

        private void SpotDiagram2D_Resize(object sender, EventArgs e)
        {
            int offset = 40; // offset used to compensate for title and form banner

            Control control = (Control)sender;
            if (control.Size.Height != (control.Size.Width + offset))
            {
                control.Size = new Size(control.Size.Width, control.Size.Width + offset);
            }
        }


        #region // Right Mouse Click Options

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
        }

        private void copyToClipboard_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
                MessageBox.Show("Chart copied to Clipboard!");
            }
        }

        private void saveToFile_Click(object sender, EventArgs e)
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

        private void setPlotScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var maxstr = chart1.ChartAreas[0].AxisX.Maximum.ToString("f3");
            Prompt pt = new Prompt("Set Max Scale Value", "Enter maximum scale for X & Y axis:", maxstr);
            pt.ShowDialog();
            if (pt.isOk == true)
            {
                double scale = (double)pt.promptValue;
                SetChartScale(scale);
            }
        }

        private void autoScalePlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var chartpts = chart1.Series[0].Points;
            List<PointD> pts = new List<PointD>();
            foreach (DataPoint p in chartpts)
            {
                pts.Add(new PointD(p.XValue, p.YValues[0]));
            }

            int i = 1;
            if (i == 0)
                SetChartScale(maxscalefactor);
            else
            {
                double maxX = pts.Max(p => p.X);
                double minX = pts.Min(p => p.X);
                double maxY = pts.Max(p => p.Y);
                double minY = pts.Min(p => p.Y);
                double diffx = maxX - minX;
                double diffy = maxY - minY;

                double zerolimit = 0.001;

                //double ctrX = (maxX + minX) / 2;
                double ctrX = pts.Sum(p => p.X) / pts.Count;
                double ctrY = pts.Sum(p => p.Y) / pts.Count;
                if (Math.Abs(ctrX) < zerolimit)
                    ctrX = 0.0;
                else
                    ctrX = Math.Round(ctrX, 3);

                //double ctrY = (maxY + minY) / 2;
                if (Math.Abs(ctrY) < zerolimit)
                    ctrY = 0.0;
                else
                    ctrY = Math.Round(ctrY, 3);

                double maxscale = 0;
                maxscale = diffx >= diffy ? diffx : diffy;
                maxscale = maxscale.YscaleValue();

                chart1.ChartAreas[0].AxisX.Minimum = ctrX - maxscale;
                chart1.ChartAreas[0].AxisX.Maximum = ctrX + maxscale;
                chart1.ChartAreas[0].AxisX.Interval = maxscale / 2;

                chart1.ChartAreas[0].AxisY.Minimum = ctrY - maxscale;
                chart1.ChartAreas[0].AxisY.Maximum = ctrY + maxscale;
                chart1.ChartAreas[0].AxisY.Interval = maxscale / 2;

                UpdateAnnotations(diffx >= diffy ? diffx : diffy, ctrY - maxscale, ctrY + maxscale);
            }
        }

        #endregion







    }
}
