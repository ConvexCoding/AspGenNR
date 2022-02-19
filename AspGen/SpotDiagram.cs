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
using gExtensions;
using gClass;
using System.IO;

namespace AspGen
{
    public partial class SpotDiagram : Form
    {
        Size basesize = new Size(1, 1);

        public SpotDiagram(List<double> y, double wl, double EFL, double BFL)
        {
            InitializeComponent();

            var junk = y.Min();
            double axiscalefactor = 1;
            string units = " (mm)";
            if (y.MaxDAbs() < 0.001)
            {
                axiscalefactor = 1000;
                units = " (μm)";
            }

            List<double> angles = new List<double>();
            for (int i = 0; i <= 360; i += 10)
                angles.Add((double)i);

            int ct = 0;
            chart1.Series.Clear();
            foreach (double yr in y)
            {
                List<PointD> pds = new List<PointD>();
                for (int i = 0; i < angles.Count(); i++)
                    pds.Add(new PointD(0, yr*axiscalefactor).RotatePointD(angles[i]));

                chart1.Series.Add(ct.ToString());
                foreach (PointD p in pds)
                    chart1.Series[ct].Points.AddXY(p.X, p.Y);
                chart1.Series[ct].ChartType = SeriesChartType.Point;
                chart1.Series[ct].MarkerStyle = MarkerStyle.Star4;
                chart1.Series[ct].MarkerColor = Color.Red;
                chart1.Series[ct].MarkerSize = 10;
                ct++;
            }

            angles.Clear();

            for (int i = 0; i <= 360; i += 2)
                angles.Add((double)i);

            double yr2 = (wl * 0.001) / 2;

            List<PointD> pds2 = new List<PointD>();
            for (int i = 0; i < angles.Count(); i++)
                pds2.Add(new PointD(0, yr2 * axiscalefactor).RotatePointD(angles[i]));

            chart1.Series.Add(ct.ToString());
            foreach (PointD p in pds2)
                chart1.Series[ct].Points.AddXY(p.X, p.Y);
            chart1.Series[ct].ChartType = SeriesChartType.Line;
            chart1.Series[ct].BorderColor = Color.Gray;
            chart1.Series[ct].BorderDashStyle = ChartDashStyle.Dash;

            var xymax = y.MaxDAbs();

            var xsc = xymax.YscaleValue() * axiscalefactor;

            if (xsc < wl * axiscalefactor / 2000)
                xsc = ((wl / 2000) * axiscalefactor).YscaleValue();

            string lformat = "f" + Properties.Settings.Default.NoOfDigitsSpotDiagLabels.ToString();
            xsc = double.Parse(xsc.ToString("E3"));  // use this hack to make sure that the labels are rounded properly

            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 2;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = lformat;

            chart1.ChartAreas[0].AxisY.Minimum = -xsc;
            chart1.ChartAreas[0].AxisY.Maximum = xsc;
            chart1.ChartAreas[0].AxisY.Interval = xsc / 2;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = lformat;

            chart1.Titles[0].Text = "Spot Diagram - λ: " + wl.ToString("f3") + " μm,  EFL:  " + EFL.ToString("F2") + " mm";

            chart1.ChartAreas[0].AxisY.Title = "Vertical Spread" + units;
            chart1.ChartAreas[0].AxisX.Title = "Horizontal Spread" + units;
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);

            var r = y.minmaxD();
            var anote = "P-to-V: " + r.Diff.ToString("g5") + " mm,  RMS:  " + r.RMS.ToString("g5");
            AddTextAnnotation(0, xsc * 0.95, anote, ContentAlignment.MiddleCenter);
        }

        private void AddTextAnnotation(double x0, double y0, string s, ContentAlignment ca)
        {
            TextAnnotation txtone = new TextAnnotation();
            txtone.IsSizeAlwaysRelative = false;
            txtone.AxisX = chart1.ChartAreas[0].AxisX;
            txtone.AxisY = chart1.ChartAreas[0].AxisY;
            txtone.AllowAnchorMoving = true;
            txtone.AnchorX = x0;
            txtone.AnchorY = y0;
            txtone.Text = s;
            txtone.ForeColor = Color.Black;
            txtone.Font = new Font("Arial", 10);
            txtone.LineWidth = 2;

            txtone.AnchorAlignment = ca;
            txtone.Alignment = ca;

            txtone.AllowAnchorMoving = true;
            txtone.AllowMoving = true;
            txtone.AllowPathEditing = true;
            txtone.AllowResizing = true;
            txtone.AllowSelecting = true;

            chart1.Annotations.Add(txtone);
        }



        // form event handlers
        private void SpotDiagram_Resize(object sender, EventArgs e)
        {
            int offset = 50; // offset used to compensate for title and form banner

            Control control = (Control)sender;
            if (control.Size.Height != (control.Size.Width + offset))
            {
                control.Size = new Size(control.Size.Width, control.Size.Width + offset);
            }
        }



        // context menu pop and menu clicks
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));

        }

        private void copyChartToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
                MessageBox.Show("Chart copied to Clipboard!");
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

    }
}
