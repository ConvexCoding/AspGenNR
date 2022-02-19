using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using gGraphExt;
using gExtensions;
using System.IO;
using gClass;
using System.Runtime.InteropServices;

namespace AspGen
{
    public partial class ExtSource3DMap : Form
    {
        Color[] cp;
        double[,] indata, yfit;
        Lens inlens;
        double inRefocus, inFiberRadius;

        //[DllImport(@"C:\Users\glher\source\repos\RustTestOne\RustTestOne\hellocargo\target\release\lib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        //private unsafe static extern bool gen_trace_rays(GenRays gr, Ray* ptr1, Ray* ptr2, UIntPtr numpts, LensWOStrings lens, double refocus);
  
        public ExtSource3DMap(Lens lens, double Refocus, double fiber_radius)
        {
            InitializeComponent();
            inlens = lens;
            inRefocus = Refocus;
            inFiberRadius = fiber_radius;
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            var angin = (fiber_radius / Math.Abs(lens.EFL));

            var watch = new System.Diagnostics.Stopwatch();
            cp = Properties.Resources.rainbow.LoadColorPallete2();
            cp[cp.Length - 1] = cp[cp.Length - 2];  // get rid of white pixels

            //int nrays = (int)Math.Round(Math.PI * num * num);
            //var trays = Properties.Settings.Default.ExtSrcNoofBaseRays;
            //var num = (int)Math.Sqrt((double)trays / Math.PI);
            //var vxyz = lens.ap.GenerateUniformRayVectors(num);

            watch.Start();

            var noofangles = Properties.Settings.Default.ExtSrcNoofAngles;
            var baserays = Properties.Settings.Default.ExtSrcNoofBaseRays;
            var npts = (ulong)(baserays * noofangles);


            
            var vxyz = lens.ap.GenerateRandomRayVectors(Properties.Settings.Default.ExtSrcNoofBaseRays);
            var vlist = RTM.TraceWithAngles(lens, Refocus, vxyz, angin);
            indata = RTM.ProcessRayData(vlist, lens, Refocus, fiber_radius);

            /*
            Ray[] rrout = new Ray[npts];
            Ray[] rrin = new Ray[npts];
            LensWOStrings lenswo = new LensWOStrings(lens);
            var gr = new GenRays((uint)baserays, (uint)noofangles, lens.ap, angin);
            unsafe
            {
                fixed (Ray* ptr = rrin)
                fixed (Ray* ptr2 = rrout)
                {
                    var good = gen_trace_rays(gr, ptr, ptr2, (UIntPtr)rrin.Length, lenswo, Refocus);
                }
            }
            indata = RTM.ProcessRustRayData(rrout, lens, Refocus, fiber_radius);
            */

            watch.Stop();
 
            UpdatePixBox(indata);

            timerLabel.Text = "Gen && Map Time: " + watch.ElapsedMilliseconds + " ms for " + npts.ToString("n0") + " rays";
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void UpdatePixBox(double[,] data)
        {
            Bitmap b = gGraphExt.gGraphExt.DoubleToBitmap(data, cp);
            pix.Image = b as Image;
        }

        private void pix_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
        }


        private void copyToClip_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bm = pix.Image as Bitmap;
                Clipboard.SetImage(bm);
                MessageBox.Show("Color Map copied to Clipboard!");
            }
        }

        private void saveToFile_Click(object sender, EventArgs e)
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

        private void condenseData2X_Click(object sender, EventArgs e)
        {
            int r = indata.GetLength(0);
            int c = indata.GetLength(1);
            int rp = r / 2;
            int cp = c / 2;
            double[,] dout = rp.Gen2DZeroArray();

            for (int col = 0; col < cp; col++)
                for (int row = 0; row < rp; row++)
                    dout[row, col] = indata[row * 2, col * 2] + indata[row * 2 + 1, col * 2] +
                                     indata[row * 2, col * 2 + 1] + indata[row * 2 + 1, col * 2 + 1];
            indata = dout;
            UpdatePixBox(indata);
        }

        private void fermiDiracFitTButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            long rayct = Properties.Settings.Default.ExtSrcNoofAngles * Properties.Settings.Default.ExtSrcNoofBaseRays;
            var ydata = indata.SliceDataMidPt(border: Properties.Settings.Default.ExtScrVerticalAverage1D);
            double xbins = (double)((ydata.Length - 1) / 4);
            double ppr = (double)rayct / (Math.PI * xbins * xbins);
            ydata.DivideDoubles(ppr);

            var xdata = ydata.Length.GenerateLinearArray(2 * inFiberRadius);
            /*
            var yfermi = FDF.FittoFermiDirac(xdata, ydata, 20, inFiberRadius, 1.0);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("X, Y Raw, Y Fermi");
            for (int i = 0; i < ydata.Length; i++)
                sb.AppendLine(xdata[i] + ", " + ydata[i] + ", " + yfermi[i]);
            Clipboard.SetText(sb.ToString());
            */
            var (beta, radius, peak) = FDF.CalcFermiDiracCoef(xdata, ydata, 20, inFiberRadius, 1.0);

            var rows = indata.GetLength(0);
            var cols = indata.GetLength(1);

            var xctr = rows / 2;
            var yctr = cols / 2;

            double stepsize = 2 * inFiberRadius / xctr;
            yfit = new double[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    double x = (r - xctr) * stepsize;
                    double y = (c - yctr) * stepsize;
                    double rad = Math.Sqrt(x * x + y * y);
                    yfit[r, c] = FDF.FermiDirac(rad, beta, radius, peak);
                }
            UpdatePixBox(yfit);
            watch.Stop();
            //var sb = yfit.ConvertDoublesToStringBuilder();
            //Clipboard.SetText(sb.ToString());
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            timerLabel.Text += ", Fermi Fit Time: " + watch.ElapsedMilliseconds + " ms";
        }

        private void copyFitDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yfit != null)
            {
                var sb = indata.ConvertDoublesToStringBuilder();
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("Fit Data Copied to Clipboard.", "Data Copy Success");
            }
            else
            {
                MessageBox.Show("No fit data available yet! Trying fitting data to Fermi-Dirac function.", "Copy Fit Error");
            }
        }

        private void crossSectionPlot_Click(object sender, EventArgs e)
        {
            //(Lens lens, double Refocus, double fiber_radius)
            ExtendedSourcePlot esp = new ExtendedSourcePlot(inlens, inRefocus, inFiberRadius, indata);
            esp.Show();
        }

        private void copyDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = indata.ConvertDoublesToStringBuilder();
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Raw Data Copied to Clipboard.", "Data Copy Success");
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
    }
}
