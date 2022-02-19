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

namespace AspGen
{
    public partial class InputRayPlot : Form
    {
        public InputRayPlot(Lens lens)
        {
            InitializeComponent();
            var rays = Misc.GenerateRayList(Properties.Settings.Default.InputRaySet);
            
            foreach(double ray in rays)
            {
                chart1.Series[0].Points.AddXY(1e-6*ray, ray * lens.ap);
            }
            chart1.Series[0].MarkerStyle = MarkerStyle.Star5;
            chart1.Series[0].MarkerColor = Color.Red;
            chart1.Series[0].MarkerSize = 10;

            double xsc = lens.Diameter / 2;
            chart1.ChartAreas[0].AxisX.Minimum = -xsc;
            chart1.ChartAreas[0].AxisX.Maximum = xsc;
            chart1.ChartAreas[0].AxisX.Interval = xsc / 5;
            chart1.ChartAreas[0].AxisY.Minimum = -xsc;
            chart1.ChartAreas[0].AxisY.Maximum = xsc;
            chart1.ChartAreas[0].AxisY.Interval = xsc / 5;
            chart1.Series[0].Name = "Ray Points";

            chart1.Series.Add("Clear Aperture");
            var lensboundary1 = chart1.Series[1];
            for (int i = 0; i <= 360; i += 10)
            {
                double angle = i * Math.PI / 180.0;
                lensboundary1.Points.AddXY(lens.ap * Math.Cos(angle), lens.ap * Math.Sin(angle));
            }
            lensboundary1.ChartType = SeriesChartType.Line;
            lensboundary1.BorderColor = Color.Black;
            lensboundary1.Color = Color.Black;
            lensboundary1.BorderDashStyle = ChartDashStyle.Dash;

            chart1.Series.Add("Lens Diameter");
            var lensboundary2 = chart1.Series[2];
            for (int i = 0; i <= 360; i += 5)
            {
                double angle = i * Math.PI / 180.0;
                lensboundary2.Points.AddXY((lens.Diameter / 2) * Math.Cos(angle), (lens.Diameter / 2) * Math.Sin(angle));
            }
            lensboundary2.ChartType = SeriesChartType.Line;
            lensboundary2.BorderColor = Color.Black;
            lensboundary2.Color = Color.Black;
            lensboundary2.BorderDashStyle = ChartDashStyle.Solid;
            lensboundary2.BorderWidth = 2;

            chart1.Titles[0].Text = "Input Ray Distrubtion";

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

        private void InputRayPlot_Resize(object sender, EventArgs e)
        {
            int offset = 25; // offset used to compensate for title and form banner

            Control control = (Control)sender;
            if (control.Size.Height != (control.Size.Width + offset))
            {
                control.Size = new Size(control.Size.Width, control.Size.Width + offset);
            }
        }
    }
}
