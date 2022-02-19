using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using gClass;
using gExtensions;

namespace AspGen
{
    public partial class DrawRayTrace : Form
    {
        StringBuilder sb = new StringBuilder();
        Lens lens;
        double EFL, BFL;
        string mater = null;
        List<PointF[]> pts = new List<PointF[]>();
        PointF[] lenspts;
        //List<PointF> lenspts;
        int titleheight = 40;
        float xoffset = 10F;
        int ixoffset = 20;
        (float MinX, float MinY, float MaxX, float MaxY) limits; // = new Tuple<float, float, float, float>();
        string pageTitle;
        int zoomfactor = 1;
        int horizpt = 0;   // 690 for focus
        float scalex, scaley;
        float refocus;

        public DrawRayTrace(Lens lensin, double Refocus, string material)
        {
            InitializeComponent();
            lens = lensin;
            refocus = (float)Refocus;
            mater = material;
            pageTitle = "Ray Trace for " + mater + ", " + (lens.ap * 2).ToString("f1") + " x " + EFL.ToString("f1") + " mm EFL";

            if (Math.Abs(lensin.EFL) > 2000)
            {
                double postlensopl = 20;
                EFL = postlensopl;
                lens.EFL = postlensopl;
                BFL = postlensopl;
                lens.BFL = postlensopl;
                pageTitle = "Ray Trace for " + mater + ", " + (lens.ap * 2).ToString("f1") + 
                                " mm APERT x ∞ mm EFL";

            }
            else
            {
                EFL = lensin.EFL;
                BFL = lensin.BFL;
                pageTitle = "Ray Trace for " + mater + ", " + (lens.ap * 2).ToString("f1") + 
                            " mm APERT x " + lensin.EFL.ToString("f1") + " EFL";
            }

            int norays = Properties.Settings.Default.NumberOfDrawLensRays;
            xoffset = (float)Properties.Settings.Default.DrawLensPreRayDistance;
            for (double ypt = 0; ypt <= lens.ap * 1.001; ypt += lens.ap / norays)
            {
                pts.Add(ypt.TraceRayPath(lens, refocus, xoffset));
            }
            var rayslimits = pts[0].MinMaxFloatPts();
            lenspts = TraceLensShape(lens, xoffset);
            //var fpts = lenspts.ToArray();
            var shapelimits = lenspts.MinMaxFloatListPt();

            if (rayslimits.MinX < -5)
            {
                foreach(PointF[] parray in pts)
                    for(int i = 0; i < parray.GetLength(0); i++)
                        parray[i].X -= rayslimits.MinX;
                for (int i = 0; i < lenspts.GetLength(0); i++)
                {
                    lenspts[i].X -= rayslimits.MinX;
                }
            }            

            limits = Compare(rayslimits, shapelimits);

            xoffset = (limits.MaxX - limits.MinX) * 0.1F;
            ixoffset = (int)xoffset;

            panel1.MouseWheel += new MouseEventHandler(Panel1_MouseWheel);
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            int d = e.Delta / 120;
            mwheeldata.Text = d.ToString();
            zoomfactor += d;
            if (zoomfactor < 1)
                zoomfactor = 1;
            this.Refresh();
            this.Text = "Lens Drawing at " + zoomfactor.ToString() + " scale.";
        }



        // form event handlers
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int w = panel1.Width;
            int h = panel1.Height;
            int cline = (h + titleheight) / 2;

            // calculate scale factor
            scalex = (float)zoomfactor * ((float)w / (limits.MaxX - limits.MinX));
            scaley = (float)zoomfactor * ((float)(h - titleheight) / (limits.MaxY - limits.MinY));
            mwheeldata.Text = "Scale X: " + scalex.ToString("f1");

            hSBar.Minimum = 0;
            hSBar.Maximum = w;

            float scale = (scalex < scaley ? scalex : scaley) * 0.90F;
            ixoffset = (int)((double)w * 0.05) - horizpt * zoomfactor;

            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
            g.DrawRectangle(new Pen(Color.Black, 2F), 2, 3, w-3, h-4);
            using (Font font1 = new Font("Arial", 18, FontStyle.Regular, GraphicsUnit.Point))
            {
                Rectangle titlerect = new Rectangle(2, 3, w - 3, titleheight);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(pageTitle, font1, Brushes.Blue, titlerect, stringFormat);
                g.DrawRectangle(new Pen(Color.Black, 1F), titlerect);
            }

            // draw ray set both positivve and negative
            foreach (PointF[] p in pts)
            {
                g.DrawLines(new Pen(Color.Red, .2F), MultiTransform(p, scale, scale, ixoffset, cline));
            }

            foreach (PointF[] p in pts)
            {
                g.DrawLines(new Pen(Color.Red, .2F), MultiTransform(p, scale, -scale, ixoffset, cline));
            }

            // draw lens
            SolidBrush fl = new SolidBrush(Color.FromArgb(125, 102, 178, 255));
            g.FillPolygon(fl, MultiTransform(lenspts, scale, -scale, ixoffset, cline));

            // center line draw
            if (Properties.Settings.Default.DrawLensDrawCL)
            {
                var cpts = MultiTransform((0.0).TraceRayPath(lens, refocus, xoffset), scale, -scale, ixoffset, cline);
                PointF[] clinepts = new PointF[]
                {
                new PointF(15, cline),
                new PointF(w-15, cline)
                };

                Pen clinepen = new Pen(Color.Black, 0.1F);
                clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawLines(clinepen, clinepts);


            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }



        // supporting functions
        private PointF[] MultiTransform(PointF[] pts, float scx, float scy, float dx, float dy)
        {
            List<PointF> plist = new List<PointF>();
            foreach (PointF p in pts)
            {
                float x = p.X * scx;
                float y = p.Y * scy;
                plist.Add(new PointF(x + dx, y + dy));
            }
            return plist.ToArray();
        }

        private PointF[] TraceLensShape(Lens lens, float xoffset)
        {
            List<PointF> pts1plus = new List<PointF>();
            List<PointF> pts1neg  = new List<PointF>();
            List<PointF> pts2plus = new List<PointF>();
            List<PointF> pts2neg  = new List<PointF>();

            double radius = lens.ap / 0.9;
            double steps = 0.2;

            // define lens arc
            for (double r = 0; r <= radius*1.001; r += steps)
            {
                pts1plus.Add(new PointF((float)r.Sag(lens.Side1) + xoffset, (float)r));
                pts2plus.Add(new PointF((float)r.Sag(lens.Side2) + (float)lens.CT + xoffset, (float)r));

                pts1neg.Add(new PointF((float)r.Sag(lens.Side1) + xoffset, -(float)r));
                pts2neg.Add(new PointF((float)r.Sag(lens.Side2) + (float)lens.CT + xoffset, -(float)r));
            }
            pts2plus.Reverse();
            pts1plus.AddRange(pts2plus);
            pts1plus.AddRange(pts2neg);
            pts1neg.Reverse();
            pts1plus.AddRange(pts1neg);

            return pts1plus.ToArray();
        }

        private (float MinX, float MinY, float MaxX, float MaxY) Compare((float MinX, float MinY, float MaxX, float MaxY) p1, (float MinX, float MinY, float MaxX, float MaxY) p2)
        {
            float minx = 1e8F;
            float miny = 1e8F;
            float maxx = -1e8F;
            float maxy = -1e8F;

            if (p1.MinX < p2.MinX)
                minx = p1.MinX;
            else
                minx = p2.MinX;

            if (p1.MinY < p2.MinY)
                miny = p1.MinY;
            else
                miny = p2.MinY;

            if (p1.MaxX > p2.MaxX)
                maxx = p1.MaxX;
            else
                maxx = p2.MaxX;

            if (p1.MaxY > p2.MaxY)
                maxy = p1.MaxY;
            else
                maxy = p2.MaxY;

            return (minx, miny, maxx, maxy);
        }



        // context menu pop click events
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cms.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
            if (e.Button == MouseButtons.Left)
            {
                var realx = (int)((float)e.X / scalex);
                mwheeldata.Text = mwheeldata.Text + ",   Up: " + e.Location.ToString() + "   Real X: " + realx;

            }
        }



        private void resetButton_Click(object sender, EventArgs e)
        {
            zoomfactor = 1;
            horizpt = 0;
            this.Refresh();
        }

        //int downpt = 0;
        //int uppt = 0;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //downpt = e.X;
            //mwheeldata.Text = "Down: " + e.Location.ToString();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            //uppt = e.X;
            //mwheeldata.Text = mwheeldata.Text + ",   Up: " + e.Location.ToString();
            //horizpt = uppt;
            //this.Refresh();
        }

        private void hSBar_ValueChanged(object sender, EventArgs e)
        {
            horizpt = (int)((float)hSBar.Value);   // * scalex);
            mwheeldata.Text = "new x point: " + horizpt;
            this.Refresh();
        }



        private void SaveImageClick_Click(object sender, EventArgs e)
        {
            int width = panel1.Size.Width;
            int height = panel1.Size.Height;

            Bitmap bmp = new Bitmap(width, height);
            panel1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));

            saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFile.FileName);
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = panel1.Size.Width;
            int height = panel1.Size.Height;

            Bitmap bm = new Bitmap(width, height);
            panel1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));
            Clipboard.SetImage(bm as Image);
            MessageBox.Show("Panel copied to Clipboard!");
        }

    }
}
