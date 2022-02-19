using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gExtensions;
using gClass;
using System.Drawing.Drawing2D;
using gGraphExt;
using System.Drawing.Printing;

namespace AspGen
{
    public partial class LensDrawing : Form
    {
        StringBuilder sb = new StringBuilder();
        Lens lens;
        double EFL, BFL;
        string mater = null;
        List<PointF[]> pts = new List<PointF[]>();
        List<PointF> lenspts = new List<PointF>();
        int titleheight = 60;

        float scalex, scaley;
        int clineh, clinev;
        int minylens = 0;
        double sag1, sag2, ET;
        string dsp = "\n";
        float scale;
        float hxwidthratio = 1.75f;

        (float MinX, float MinY, float MaxX, float MaxY) limits; // = new Tuple<float, float, float, float>();
        string pageTitle;

        public LensDrawing(Lens lensin, string material)
        {
            InitializeComponent();
            int h = 500;
            int dh = 64;
            int dw = 16;
            int w = (int)((float)(h) * hxwidthratio);
            this.Size = new Size(w + dw, h + dh);
            this.Refresh();

            lens = lensin;

            mater = material;
            pageTitle = "Lens Drawing for " + mater + ", " + (lens.Diameter).ToString("f1") + " x " + EFL.ToString("f1") + " mm EFL";

            if (Math.Abs(lensin.EFL) > 2000)
            {
                EFL = 40;
                BFL = 40;
                pageTitle = "Lens Drawing for " + mater + ", " + (lens.Diameter).ToString("f1") +
                                " mm APERT x ∞ mm EFL";
            }
            else
            {
                EFL = lensin.EFL;
                BFL = lensin.BFL;
                pageTitle = "Lens Drawing for " + mater + ", " + (lens.Diameter).ToString("f1") +
                            " mm APERT x " + lensin.EFL.ToString("f1") + " EFL";
            }

            lenspts = lens.TraceLensShape();
            limits = lenspts.MinMaxFloatListPt();

            sag1 = (lens.Diameter / 2).Sag(lens.Side1);
            sag2 = (lens.Diameter / 2).Sag(lens.Side2);
            ET = lens.CT - sag1 + sag2;
        }


        // form event handlers
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int w = panel1.Width;
            int h = panel1.Height;
            clineh = (int)((double)(h + titleheight) / 2.5);
            clinev = (int)((double)w / 2);

            // calculate scale factor
            scalex = ((float)w / (limits.MaxX - limits.MinX));
            scaley = ((float)(h - titleheight) / (limits.MaxY - limits.MinY));

            Pen clinepen = new Pen(Color.Black, 2F);
            clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            scale = (scalex < scaley ? scalex : scaley) * 0.5F;

            //  *************************************************************************************
            // add title block and text                        
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
            g.DrawRectangle(new Pen(Color.Black, 2F), 2, 3, w - 3, h - 4);
            GenTopDataBlock(lens, mater, g);            

            //  *************************************************************************************
            // draw lens shape and fill color
            SolidBrush fl = new SolidBrush(Color.LightBlue);
            if (mater == "ZnSe") 
                fl = new SolidBrush(Color.Orange);
            if (mater == "ZnS")
                fl = new SolidBrush(Color.LightYellow);
            PointF[] pts = lenspts.ToArray().MultiTransform(scale, -scale, clinev, clineh);
            minylens = (int)pts.MinMaxFloatPts().MinY;
            g.FillPolygon(fl, pts);
            g.DrawPolygon(new Pen(Color.Black, 1F), lenspts.ToArray().MultiTransform(scale, -scale, clinev, clineh));


            //  *************************************************************************************
            // draw lens CT & ET
            DrawCT(lens, g);
            DrawET(lens, g);


            //  *************************************************************************************
            // center line draw
            DrawCenterLines(g);


            //  *************************************************************************************
            // side 1 & 2 text
            DrawSurfaceText(lens, g);


            //  *************************************************************************************
            // draw title block lower right corner
            DrawTitleBlock(g);

            //  *************************************************************************************
            // draw aspheric equation if needed
            if ((lens.Side1.Type == 2) || (lens.Side2.Type == 2))
                DrawAsphericEquation(g);

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            // adjust width to the height

            int h = panel1.Height;
            int dh = 64;
            int dw = 16;
            int w = (int)( (float)(h) * hxwidthratio);

            this.Size = new Size(w + dw, h + dh);
            this.Refresh();
            
        }


        // graphic support functions
        private void GenTopDataBlock(Lens lens, string Material, Graphics g)
        {
            Font font1 = new Font("Arial", 10, FontStyle.Bold);
            Font font2 = new Font("Arial", 10, FontStyle.Regular);
            string[] titles = new string[] { "Material", "Wavelength", "Index", "Diameter", "EFL", "BFL" };
            string[] values = new string[] { mater,
                                            lens.WL.ToString("f3"),
                                            lens.n.ToString("f4"),
                                            lens.Diameter.ToString("f3"),
                                            lens.EFL.ToString("f3"),
                                            lens.BFL.ToString("f3") };

            int w0 = 2;
            int h0 = 2;
            int w = 90;
            int h = 35;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < titles.Count(); i++)
            {
                Rectangle r0 = new Rectangle(w0, h0, w, h);
                g.DrawString(titles[i], font1, Brushes.Black, r0, stringFormat);
                g.DrawRectangle(new Pen(Color.Black, 1F), r0);

                Rectangle r1 = new Rectangle(w0, h0 + h, w, h);
                g.DrawString(values[i], font2, Brushes.Black, r1, stringFormat);
                g.DrawRectangle(new Pen(Color.Black, 1F), r1);

                w0 += w;
            }
        }

        private void DrawCT(Lens lens, Graphics g)
        {
            Pen clinepen = new Pen(Color.Black, 1F);
            clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            PointF[] chev1 = new PointF[]
            {
                new PointF((float)(-lens.CT/2F - lens.CT/5F), (float)-lens.Diameter*0.15F/2F),
                new PointF((float)-lens.CT/2F, 0F),
                new PointF((float)lens.CT/2F, 0F),
                new PointF((float)(lens.CT/2F-lens.CT/5F), (float)-lens.Diameter*0.15F/2F)

            };
            g.DrawLines(clinepen, chev1.MultiTransform(scale, -scale, clinev, clineh));

            using (Font font1 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Near;
                g.DrawString("CT: " + lens.CT.ToString("f4") + " mm", font1, Brushes.Black,
                    T1(new PointF((float)(-lens.CT / 2F - lens.CT / 5F), (float)-lens.Diameter * 0.15F / 2F), scale, -scale, clinev, clineh),
                    stringFormat);
            }
        }

        private void DrawET(Lens lens, Graphics g)
        {
            Pen clinepen = new Pen(Color.Black, 1F);
            clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            PointF[] chev3 = new PointF[]
            {
                new PointF((float)(-lens.CT/2F + sag1), (float)-lens.Diameter * 1.03F / 2F),
                new PointF((float)(-lens.CT/2F + sag1), (float)-lens.Diameter * 1.2F / 2F)
            };
            g.DrawLines(clinepen, chev3.MultiTransform(scale, -scale, clinev, clineh));


            Pen pen = new Pen(Color.Black, 1);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            pen.CustomEndCap = bigArrow;
            var p1 = T1(new PointF((float)(-lens.CT / 2F + sag1 - 2), (float)-lens.Diameter * 1.1F / 2F),
                        scale, -scale, clinev, clineh);
            var p2 = T1(new PointF((float)(-lens.CT / 2F + sag1), (float)-lens.Diameter * 1.1F / 2F),
                        scale, -scale, clinev, clineh);
            g.DrawLine(pen, p1, p2);


            PointF[] chev4 = new PointF[]
            {
                new PointF((float)(lens.CT/2F + sag2), (float)-lens.Diameter * 1.03F / 2F),
                new PointF((float)(lens.CT/2F + sag2), (float)-lens.Diameter * 1.2F / 2F)
            };
            g.DrawLines(clinepen, chev4.MultiTransform(scale, -scale, clinev, clineh));


            var p3 = T1(new PointF((float)(lens.CT / 2F + sag2 + 2), (float)-lens.Diameter * 1.1F / 2F),
            scale, -scale, clinev, clineh);
            var p4 = T1(new PointF((float)(lens.CT / 2F + sag2), (float)-lens.Diameter * 1.1F / 2F),
                        scale, -scale, clinev, clineh);
            g.DrawLine(pen, p3, p4);


            using (Font font1 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.DrawString("ET: " + ET.ToString("f4") + " mm  ", font1, Brushes.Black, p1, stringFormat);
            }
        }

        private void DrawCenterLines(Graphics g)
        {
            var clinepen = new Pen(Color.Black, 0.1F);
            int w = panel1.Width;
            int h = panel1.Height;

            if (Properties.Settings.Default.DrawLensDrawCL)
            {
                PointF[] clineptsh = new PointF[]
                {
                new PointF(15, clineh),
                new PointF(w-15, clineh)
                };

                PointF[] clineptsv = new PointF[]
                {
                new PointF(clinev, titleheight),
                new PointF(clinev, h)
                };

                //Pen clinepen = new Pen(Color.Black, 0.1F);
                clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawLines(clinepen, clineptsh);
                g.DrawLines(clinepen, clineptsv);
            }
        }

        private void DrawSurfaceText(Lens lens, Graphics g)
        {
            int w = panel1.Width;

            int width = 200;
            int height = width;

            using (Font font1 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
            {
                var sb1 = GenerateSideText(lens.Side1, 1, lens.Diameter);
                Rectangle titlerect = new Rectangle(140, minylens + 10, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                g.DrawString(sb1.ToString(), font1, Brushes.Black, titlerect, stringFormat);
                //g.DrawRectangle(new Pen(Color.Black, 1F), titlerect);
            }

            //  *************************************************************************************
            // side 2 text
            using (Font font1 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
            {
                var sb2 = GenerateSideText(lens.Side2, 2, lens.Diameter);
                Rectangle titlerect = new Rectangle(w/2 + 100, minylens + 10, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                g.DrawString(sb2.ToString(), font1, Brushes.Black, titlerect, stringFormat);
                //g.DrawRectangle(new Pen(Color.Black, 1F), titlerect);
            }
        }

        private void DrawTitleBlock(Graphics g)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            Font font1 = new Font("Arial", 12, FontStyle.Bold);
            Font font2 = new Font("Arial", 8, FontStyle.Regular);

            Brush textcolor = Brushes.Black;

            int hend = panel1.Height - 2;
            int wend = panel1.Width - 2;
      
            int blockh = 20;
            int blockw = 90;

            // bottom right is date
            var r = new Rectangle(wend - blockw, hend - blockh, blockw, blockh);
            g.DrawString(DateTime.UtcNow.ToString("d-MMM-yy"), font2, textcolor, r, stringFormat);
            g.DrawRectangle(new Pen(Color.Black, 1F), r);

            // bottom left is user name
            r = new Rectangle(wend - blockw * 2, hend - blockh, blockw, blockh);
            g.DrawString(Environment.UserName, font2, textcolor, r, stringFormat);
            g.DrawRectangle(new Pen(Color.Black, 1F), r);

            // middle right is "Date" title
            r = new Rectangle(wend - blockw, hend - 2 * blockh, blockw, blockh);
            g.DrawString("Date", font2, textcolor, r, stringFormat);
            g.DrawRectangle(new Pen(Color.Black, 1F), r);

            // Middle left is "User" name
            r = new Rectangle(wend - blockw * 2, hend - 2 * blockh, blockw, blockh);
            g.DrawString("User", font2, textcolor, r, stringFormat);
            g.DrawRectangle(new Pen(Color.Black, 1F), r);

            // Main Title Block
            r = new Rectangle(wend - blockw * 2, hend - 3 * blockh - 5, 2 * blockw, blockh + 5);
            g.DrawString("Lens Gen", font1, textcolor, r, stringFormat);
            g.DrawRectangle(new Pen(Color.Black, 1F), r);

        }

        private void DrawAsphericEquation (Graphics g)
        {
            int hstart = 10;
            int vstart = panel1.Height - 95;
            var bmp = (new Bitmap(AspGen.Properties.Resources.AspEqSmall));
            g.DrawImage(bmp as Image, hstart, vstart);
        }


        // graphic support funtions
        private StringBuilder GenerateSideText(Side side, int sideno, double dia)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Side " + sideno.ToString() + " Parameters");
            sb.AppendLine("---------------------------" + dsp);
            if (Math.Abs(side.R) < 0.001)
            {
                sb.AppendLine("Radius:  Inf. (Plano)" + dsp);
            }
            else
            {
                if ((sideno == 1) && (side.R > 0.000))
                    sb.AppendLine("Radius: " + side.R.ToString("f4") + " mm CX" + dsp);
                if ((sideno == 1) && (side.R < 0.000))
                    sb.AppendLine("Radius: " + side.R.ToString("f4") + " mm CC" + dsp);
                if ((sideno == 2) && (side.R < 0.000))
                    sb.AppendLine("Radius: " + side.R.ToString("f4") + " mm CX" + dsp);
                if ((sideno == 2) && (side.R > 0.000))
                    sb.AppendLine("Radius: " + side.R.ToString("f4") + " mm CC" + dsp);
            }
            if (Math.Abs(side.K) > 1e-6)
            {
                sb.AppendLine("k: " + side.R.ToString("f4") + " mm" + dsp);
            }
            if (Math.Abs(side.AD) > 1e-15)
            {
                sb.AppendLine("AD: " + side.AD.ToString("e4") + " mm^-3" + dsp);
            }
            if (Math.Abs(side.AE) > 1e-18)
            {
                sb.AppendLine("AE: " + side.AE.ToString("e4") + " mm^-5" + dsp);
            }
            if (Math.Abs(side.R) > 0.001)
            {
                var sag = (dia / 2).Sag(side);
                sb.AppendLine("Sag: " + (dia / 2).Sag(side).ToString("f4") + " mm");
                sb.AppendLine("  @ " + (dia / 2).ToString("f4") + " mm" + dsp);
            }

            return sb;
        }

        


        // Menu button clicks
        private void SaveImage_Click(object sender, EventArgs e)
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

        private void copyToClipboard_Click(object sender, EventArgs e)
        {
            int width = panel1.Size.Width;
            int height = panel1.Size.Height;

            Bitmap bm = new Bitmap(width, height);
            panel1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));
            Clipboard.SetImage((bm.ResizeBitmap(width*3/2, height*3/2)) as Image);
            MessageBox.Show("Panel copied to Clipboard!");
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            PrintDocument lensPrint = new System.Drawing.Printing.PrintDocument();
            lensPrint.DefaultPageSettings.Landscape = true;
            lensPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(lensPrint_PrintPage);
            prt.Document = lensPrint;
            if (prt.ShowDialog() == DialogResult.OK)
            {
                lensPrint.Print();
            }
        }

        private void lensPrint_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap myBitmap1 = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(myBitmap1, new Rectangle(0, 0, panel1.Width, panel1.Height));
            float margin = 50F;
            float w = 0;
            float h = 0;
            float scalex = (e.PageBounds.Width - margin * 2F) / (float)panel1.Width;
            float scaley = (e.PageBounds.Height - margin * 2F) / (float)panel1.Height;
            if (scalex <= scaley)
            {
                w = panel1.Width * scalex;
                h = panel1.Height * scalex;
            }
            else
            {
                w = panel1.Width * scaley;
                h = panel1.Height * scaley;
            }

            //e.Graphics.DrawImage(myBitmap1, 0, 0, w, h);
            e.Graphics.DrawImage(myBitmap1, margin - 40, margin - 25, w, h);
            myBitmap1.Dispose();
        }


        // misc support functions

        private PointF T1(PointF p, float scx, float scy, float dx, float dy)
        {
            return new PointF(p.X * scx + dx, p.Y * scy + dy);
        }

    }
}
