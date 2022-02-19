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
    public partial class TransverseSphericalPlot : Form
    {
        Lens lens;
        double Refocus;
        List<PointD> pts = new List<PointD>();

        public TransverseSphericalPlot(Lens lensin, double refocus)
        {
            InitializeComponent();
            lens = lensin;
            Refocus = refocus;

            GenerateData();
            GeneratChart();
        }

        private void GenerateData()
        {
            int nopts = Properties.Settings.Default.TransversDataPoints;
            double dy = 2 * lens.ap / (nopts - 1);
            for (double y = -lens.ap; y < lens.ap + 0.001; y += dy)
            {
                var ve = RTM.Trace_3D_Plus(new Vector3D(0, y, 0), new Vector3D(0, 0, 1), lens, Refocus);
                pts.Add(new PointD(y, ve.Vout.Y));
                var wfe = (new Vector3D(0, y, 0)).Y.CalcRayWFE(lens, Refocus);
            }
        }

        private void GeneratChart ( )
        { 

            string units = "mm";
            double scale = 1.0;
            var maxy = pts.Max(point => Math.Abs(point.Y));
            if (Math.Abs(maxy) <= 0.01)
            {
                scale = 1000;
                units = "μm";
            }

            List<PointD> traall = new List<PointD>();
            foreach(PointD p in pts)
            {
                traall.Add(p);
            }


            foreach (PointD p in traall)
                chart1.Series[0].Points.AddXY(p.X, p.Y*scale);
            chart1.Series[0].BorderWidth = 2;

            double xsc = traall[traall.Count() - 1].X;
            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 3;

            chart1.Titles[0].Text = "Transverse Ray Intercept Curve";
            chart1.Titles[1].Text = "λ: " + lens.WL.ToString("f3") + " μm,  EFL:  " + lens.EFL.ToString("F2") + " mm";

            chart1.ChartAreas[0].AxisY.Title = "Y Ray Intercept (" + units + ")";   //  + units;
            chart1.ChartAreas[0].AxisX.Title = "Input Ray Height (mm)";   // + units;
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);

            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 2;

            // anchored to a point but shifted down
            AddAnnotation((traall[0].Y * scale).ToString("f4") + " " + units);
        }

        private void AddAnnotation(string anotetext)
        {
            chart1.Annotations.Clear();

            //TextAnnotation anote = new TextAnnotation();
            //RectangleAnnotation anote = new RectangleAnnotation();
            CalloutAnnotation anote = new CalloutAnnotation();

            anote.Text = anotetext;
            anote.ForeColor = Color.Black;
            anote.Font = new Font("Arial", 10);

            anote.AnchorDataPoint = chart1.Series[0].Points[0];

            anote.BackColor = Color.FromArgb(200, Color.White);
            anote.LineColor = Color.FromArgb(200, Color.LightBlue);
            anote.AnchorAlignment = ContentAlignment.TopLeft;
            anote.AllowMoving = true;
            chart1.Annotations.Add(anote);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e) 
        {
            cms.Show(this, new Point(chart1.Location.X, chart1.Location.Y));
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
        }

        private void copyDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            List<PointD> pts = new List<PointD>();
            sb.AppendLine("Yin, Yend, Zend, AOI, LSA, OPD-Obs");

            Vector3D Inormal = new Vector3D(0, 0, 1);

            int nopts = Properties.Settings.Default.TransversDataPoints;
            double dy = 2 * lens.ap / (nopts - 1);

            for (double y = -lens.ap; y < lens.ap + 0.001; y += dy)
            {
                var Vmain = new Vector3D(0, y, 0);
                var Vsqr = new Vector3D(0, (y / Math.Sqrt(2)), 0);

                var (P_m, E_m, AOI_m, LSA_m) = RTM.Trace_3D_Extra(Vmain, Inormal, lens, 0);
                var (P_z, E_z, AOI_z, LSA_z) = RTM.Trace_3D_Extra(Vsqr, Inormal, lens, Refocus);
                var (P_f, E_f, AOI_f, LSA_f) = RTM.Trace_3D_Extra(Vmain, Inormal, lens, Refocus);

                double a = (4 * LSA_z - LSA_m) / Math.Pow(y, 2);
                double b = (2 * LSA_m - 4 * LSA_z) / Math.Pow(y, 4);
                double opd = 0.0;
                if (Math.Abs(y) > 1e-6)
                    opd = 1000 * (Math.Sin(AOI_f) * Math.Sin(AOI_f) / 2) *
                                     (Refocus - a * Math.Pow(y, 2) / 2 - b * Math.Pow(y, 4) / 3) / lens.WL;   // convert to waves /WL

                var x = y.CalcRayWFE(lens, Refocus);

                sb.AppendLine(y.ToString("f3") + ",  " + P_f.Y.ToString("f6") + ", " +
                P_f.Z.ToString("f4") + ", " + AOI_f.RadToDeg().ToString("f4") + ", " +
                LSA_f.ToString("f6") + ", " + opd.ToString("f3"));
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Data copied to Clipboard!");
        }

        private void copyToClipboardAsPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
                MessageBox.Show("Panel copied to Clipboard!");
            }
        }

        private void saveToPNGFileToolStripMenuItem_Click(object sender, EventArgs e)
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

        public void autoScaleButton_Click(object sender, EventArgs e)
        {
            LocalAutoScaleChart( );
        }

        /*
        private void setVerticalScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.SetAxisScale("Set Vertical Axis Parameters");
            //SetAxisScale("Set Vertical Axis Parameters", chart1.ChartAreas[0].AxisY);
        }

        private void setHorizontalScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.SetAxisScale("Set Horizontal Axis Parameters");
            //SetAxisScale("Set Horizontal Axis Parameters", chart1.ChartAreas[0].AxisX);
        }
        */
        int whichaxis = 0;
        int caller = 0;
        private void setHorizontalScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            whichaxis = 0;
            //.ChartAreas[0].AxisX.SetAxisScale("Set Horizontal Axis Parameters");
            SetAxisScalewithNotify(chart1.ChartAreas[0].AxisX, "Set Horizontal Axis Parameters");
        }

        private void setVerticalScaleToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

    }
}
