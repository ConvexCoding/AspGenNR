using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using gClass;
using gExtensions;
using gGraphExt;
using System.IO;

namespace AspGen
{
    public partial class WFE2DMap : Form
    {
        double[,] main;
        double hwidth = 1;
        int s = 200;
        double scale, min, max;
        Lens lensp;
        double refocus;
        public WFE2DMap(Lens lens, double Refocus)
        {
            InitializeComponent();

            lensp = lens;
            refocus = Refocus;

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            GenData(lens, Refocus);
            DisplayWFEMap();

            watch.Stop();
            timerLabel.Text = "Gen && Map Time: " + watch.ElapsedMilliseconds + " ms for " + s.ToString("n0") + " rays";
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void GenData (Lens lens, double Refocus)
        {
            s = Properties.Settings.Default.WFE2DMapSize;
            int center = s / 2;
            main = new double[s, s];
            hwidth = lens.ap;

            scale = (double)(s / 2) / lens.ap;
            min = 1e20;
            max = -1e20;

            // Generate rays and trace
            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                {
                    main[r, c] = double.NaN;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                    {
                        var wfe = hypot.CalcRayWFE(lens, Refocus);
                        main[r, c] = wfe.OPD;
                        if (wfe.OPD > max)
                            max = wfe.OPD;
                        if (wfe.OPD < min)
                            min = wfe.OPD;
                    }
                }

        }

        private void DisplayWFEMap()
        {
            // Convert WFE to scaled int[,] array 
            int[,] imain = new int[s, s];
            var cp = Properties.Resources.rainbow.LoadColorPallete2();
            double maxcp = (double)(cp.Count() - 1);
            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                    if (double.IsNaN(main[r, c]))
                        imain[r, c] = -1;
                    else
                        imain[r, c] = (int)(maxcp * (main[r, c] - min) / (max - min));

            // create the color map bitmap
            Bitmap b = new Bitmap(s, s);
            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                    if (double.IsNaN(main[r, c]))
                        b.SetPixel(r, c, Color.White);
                    else
                        b.SetPixel(r, c, cp[imain[r, c]]);

            // resize so that it always fits properly on page with color bar
            b = b.ResizeBitmap(500, 500);

            int cmapwidth = 40;
            Bitmap cmap = new Bitmap(cmapwidth, 2 * cp.Count());
            for (int i = 0; i < cp.Count(); i++)
            {
                for (int c = 0; c < cmapwidth; c++)
                {
                    cmap.SetPixel(c, 2 * i, cp[cp.Count() - 1 - i]);
                    cmap.SetPixel(c, 2 * i + 1, cp[cp.Count() - 1 - i]);
                }
            }

            int base_size = 500;
            string maxstr = max.ToString("f3") + " waves";
            string minstr = min.ToString("f3") + " waves";
            Font font1 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
            Font font1a = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font font2 = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

            Bitmap rside = new Bitmap(100, base_size, b.PixelFormat);
            using (Graphics g = Graphics.FromImage(rside))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Far;
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, 100, base_size));

                g.DrawImage(cmap, (rside.Width - cmap.Width) / 2, (rside.Height - cmap.Height) / 2);
                Rectangle maxrect = new Rectangle(0, 0, 100, 75);
                g.DrawString(maxstr, font1, Brushes.Black, maxrect, stringFormat);

                stringFormat.LineAlignment = StringAlignment.Center;
                g.DrawString("Scale", font1a, Brushes.Black, maxrect, stringFormat);

                stringFormat.LineAlignment = StringAlignment.Near;
                Rectangle minrect = new Rectangle(0, base_size - 75, 100, base_size);
                g.DrawString(minstr, font1, Brushes.Black, minrect, stringFormat);
            }


            // Add outside white border and frame wavefront map with black border
            Bitmap b2 = gGraphExt.gGraphExt.AddBitmapsSidebySide(b, rside);
            Bitmap b3 = new Bitmap(b2.Width + 100, b2.Height + 100);
            using (Graphics g = Graphics.FromImage(b3))
            {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, b3.Width, b3.Height));
                g.DrawImage(b2, 50, 50);
                g.DrawRectangle(new Pen(Color.Black, 2f), 50, 50, base_size, base_size);
                g.DrawRectangle(new Pen(Color.Black, 2f), 0, 0, b3.Width, b3.Height);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                Rectangle rect = new Rectangle(0, 0, b2.Width, 50);
                g.DrawString("Wavefront Error Map", font2, Brushes.Black, rect, stringFormat);
            }

            pix.Image = b3 as Image;
            pix.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bm = pix.Image as Bitmap;
                Clipboard.SetImage(bm);
                MessageBox.Show("Panel copied to Clipboard!");
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bm = pix.Image as Bitmap;
                saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    bm.Save(saveFile.FileName);
                }
            }
        }

        private void pix_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
        }

        private void saveDataToCSVFileButton_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                StringBuilder sb = main.ConvertDoublesToStringBuilder();

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter swriter = new StreamWriter(saveFile.FileName))
                    {
                        swriter.Write(sb.ToString());
                    }
                }
            }
        }

        private void copyFormButton_Click(object sender, EventArgs e)
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

        private void generateCrossSectionalPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveFrontErrorPlot wfplot = new WaveFrontErrorPlot(lensp, refocus);
            wfplot.Show();
        }

        private void savePupilApodMapToCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                StringBuilder sb = main.ConvertDoubleUnitApodToStringBuilder();

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter swriter = new StreamWriter(saveFile.FileName))
                    {
                        swriter.Write(sb.ToString());
                    }
                }
            }
        }

        private void savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;

                StringBuilder sb = main.ConvertDoublePhasesToStringBuilder(lensp.WL, lensp.ap);

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter swriter = new StreamWriter(saveFile.FileName))
                    {
                        swriter.Write(sb.ToString());
                    }
                }
            }
        }
    }
}
