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
    public partial class WaveFrontErrorPlot : Form
    {
        Lens lens;
        double Refocus;
        double ptspacing = 0.2;
        public WaveFrontErrorPlot(Lens lensin, double refocus)
        {
            InitializeComponent();

            informationButton.Alignment = ToolStripItemAlignment.Right;

            lens = lensin;
            Refocus = refocus;

            List<PointD> pts = new List<PointD>();
            for (double y = 0; y < lens.ap * 1.001; y += ptspacing)
            {
                var wfe = y.CalcRayWFE(lens, Refocus);
                pts.Add(new PointD(y, wfe.OPD));
            }

            string units = "waves";
            double scale = 1.0;
            var maxy = pts.Max(point => Math.Abs(point.Y));

            List<PointD> traall = new List<PointD>();
            foreach (PointD p in pts)
            {
                traall.Add(p);
                if (Math.Abs(p.X) > 0.0001)
                    traall.Add(new PointD(-p.X, p.Y));
            }
            traall = traall.OrderBy(p => p.X).ToList();

            double pvopd = traall.Max(p => p.Y) - traall.Min(p => p.Y);
            string pvfmt = "f3";
            if (Math.Abs(pvopd) < 0.009999)
                pvfmt = "0.00E00";

            foreach (PointD p in traall)
                chart1.Series[0].Points.AddXY(p.X, p.Y * scale);

            chart1.Series[0].BorderWidth = 2;

            double xsc = traall[traall.Count() - 1].X;
            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 4;
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.Title = "Input Ray Height (mm)";   // + units;

            chart1.ChartAreas[0].AxisY.Title = "Wavefront Error (waves)";   //  + units;
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);

            chart1.Titles[0].Text = "Wavefront Error";
            chart1.Titles[1].Text = "λ: " + lens.WL.ToString("f3") + " μm,  EFL:  " +
                                    lensin.EFL.ToString("F2") + " mm";

            AddRectAnnotation("P-V OPD: " + (traall[0].Y * scale).ToString(pvfmt) + " " + units);
        }

        private void AddRectAnnotation(string anotetext)
        {
            chart1.Annotations.Clear();
            chart1.ChartAreas[0].RecalculateAxesScale();

            RectangleAnnotation anote = new RectangleAnnotation();

            anote.Text = anotetext;
            anote.ForeColor = Color.Black;
            anote.Font = new Font("Arial", 10);

            anote.AnchorDataPoint = chart1.Series[0].Points[0];
            //anote.AnchorY = chart1.ChartAreas[0].AxisY.Maximum;
            //anote.AnchorX = chart1.ChartAreas[0].AxisX.Minimum;

            anote.AnchorY = (chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum)/2;
            anote.AnchorX = 0;

            anote.LineColor = Color.FromArgb(200, Color.LightBlue);
            anote.BackColor = Color.FromArgb(200, Color.White);

            anote.AnchorAlignment = ContentAlignment.MiddleCenter;
            anote.AllowMoving = true;
            this.chart1.Annotations.Add(anote);
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
            sb.AppendLine("Yin, Yend, Zend, AOI, LSA, OPD");
            for (double y = 0; y < lens.ap + 0.001; y += ptspacing)
            {
                var wfe = y.CalcRayWFE(lens, Refocus);
                sb.AppendLine(y.ToString("f3") + ",  " + wfe.Yend.ToString("f6") + ", " +
                wfe.Zend.ToString("f4") + ", " + (wfe.AOI).RadToDeg().ToString("f4") + ", " +
                wfe.LSA.ToString("f6") + ", " + wfe.OPD.ToString("f3"));
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Data copied to Clipboard!");
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            cms.Show(this, new Point(chart1.Location.X, chart1.Location.Y));
        }

        private void copyToBitmap_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
                MessageBox.Show("Panel copied to Clipboard!");
            }
        }

        private void savePNGFile_Click(object sender, EventArgs e)
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
            chart1.AutoScaleChart();
        }

        int whichaxis = 0;
        int caller = 1;
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
