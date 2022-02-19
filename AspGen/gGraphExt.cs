using System;
using System.Collections.Generic;
using System.Drawing;
using gExtensions;
using gClass;
using System.Windows.Forms.DataVisualization.Charting;
using AspGen;

namespace gGraphExt
{
    public static class gGraphExt
    {
        static public void SetAxisScale(this Axis axis, string title)
        {
            ChartAxis ca = new ChartAxis(axis.Minimum, axis.Maximum, axis.Interval);
            SetChartAxis scaxis = new SetChartAxis(title, ca);
            scaxis.ShowDialog();
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

        static public void AutoScaleChart(this Chart chart)
        {
            chart.ChartAreas[0].AxisX.Minimum = double.NaN;
            chart.ChartAreas[0].AxisX.Maximum = double.NaN;
            chart.ChartAreas[0].AxisX.Interval = double.NaN;

            chart.ChartAreas[0].AxisY.Minimum = double.NaN;
            chart.ChartAreas[0].AxisY.Maximum = double.NaN;
            chart.ChartAreas[0].AxisY.Interval = double.NaN;
        }

        static public List<PointF> TraceLensShape(this Lens lens)
        {
            List<PointF> pts1plus = new List<PointF>();
            List<PointF> pts1neg = new List<PointF>();
            List<PointF> pts2plus = new List<PointF>();
            List<PointF> pts2neg = new List<PointF>();

            double radius = lens.Diameter / 2;
            double steps = 0.1;

            // define lens arc
            for (double r = 0; r <= radius * 1.001; r += steps)
            {
                pts1plus.Add(new PointF((float)r.Sag(lens.Side1) - (float)lens.CT / 2F, (float)r));
                pts2plus.Add(new PointF((float)r.Sag(lens.Side2) + (float)lens.CT / 2F, (float)r));

                pts1neg.Add(new PointF((float)r.Sag(lens.Side1) - (float)lens.CT / 2F, -(float)r));
                pts2neg.Add(new PointF((float)r.Sag(lens.Side2) + (float)lens.CT / 2F, -(float)r));
            }
            pts2plus.Reverse();
            pts1plus.AddRange(pts2plus);
            pts1plus.AddRange(pts2neg);
            pts1neg.Reverse();
            pts1plus.AddRange(pts1neg);

            return pts1plus;
        }

        static public PointF[] MultiTransform(this PointF[] pts, float scx, float scy, float dx, float dy)
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

        static public PointF[] MultiTransform(this PointF[] pts, PointF scale, PointF offset)
        {
            List<PointF> plist = new List<PointF>();
            foreach (PointF p in pts)
            {
                float x = p.X * scale.X;
                float y = p.Y * scale.Y;
                plist.Add(new PointF(x + offset.X, y + offset.Y));
            }
            return plist.ToArray();
        }

        public static Color[] LoadColorPallete2(this Bitmap b)
        {
            List<Color> pal = new List<Color>();
            for (int ii = 0; ii < b.Width; ii++)
            {
                Color c = b.GetPixel(ii, 0);
                if (!pal.Contains(c))
                    pal.Add(c);
            }
            pal.Add(Color.White);
            //pal[0] = Color.White;
            return pal.ToArray();
        }

        public static Bitmap ConvertDoubleToBitmap(this double[,] indata, Color[] cp)
        {
            int bsize = indata.GetLength(0);

            int width = indata.GetLength(0);
            int height = indata.GetLength(1);

            double[,] tempdata = new double[width, height];
            Bitmap bout = new Bitmap(width, height);


            int offset = bsize - 1;
            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                    if (Convert.ToInt16(tempdata[w, h]) > 200)
                        bout.SetPixel(w, h, cp[cp.Length - 1]);
                    else
                        if (Convert.ToInt16(tempdata[w, h]) < 0)
                        bout.SetPixel(w, h, cp[Convert.ToInt16(0)]);
                    else
                        bout.SetPixel(w, h, cp[Convert.ToInt16(tempdata[w, h])]);

            //return bout.flipImage();
            return bout;
        }

        public static Bitmap DoubleToBitmap(this double[,] indata, Color[] cp)
        {
            int bsize = indata.GetLength(0);

            int width = indata.GetLength(0);
            int height = indata.GetLength(1);

            double[,] tempdata = new double[width, height];
            Bitmap bout = new Bitmap(width, height);

            var (Min, Max) = indata.FindMinMax();

            int idatasize = cp.Length - 1;

            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                {
                    if ((Max - Min) != 0.0)
                        tempdata[w, h] = (indata[w, h] - Min) * idatasize / (Max - Min);
                    else
                        tempdata[w, h] = 0.0;
                }

            int offset = bsize - 1;
            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                    if (Convert.ToInt16(tempdata[w, h]) > idatasize)
                        bout.SetPixel(w, h, cp[cp.Length - 1]);
                    else
                        if (Convert.ToInt16(tempdata[w, h]) < 0)
                        bout.SetPixel(w, h, cp[Convert.ToInt16(0)]);
                    else
                        bout.SetPixel(w, h, cp[Convert.ToInt16(tempdata[w, h])]);

            return bout;
        }

        public static void DrawBitmapWithBorder(Bitmap bmp, Point pos, Graphics g)
        {
            const int borderSize = 20;

            using (Brush border = new SolidBrush(Color.White /* Change it to whichever color you want. */))
            {
                g.FillRectangle(border, pos.X - borderSize, pos.Y - borderSize,
                    bmp.Width + borderSize, bmp.Height + borderSize);
            }

            g.DrawImage(bmp, pos);
        }

        public static Bitmap AddBitmapsTopAndBottom(Bitmap b1, Bitmap b2)
        {
            if (b1 == null)
                return b2;

            Bitmap bout = new Bitmap(b1.Width, b1.Height + b2.Height);
            using (Graphics g = Graphics.FromImage(bout))
            {
                g.DrawImage(b1, 0, 0);
                g.DrawImage(b2, 0, b1.Height);
            }
            return bout;
        }

        public static Bitmap AddBitmapsSidebySide(Bitmap b1, Bitmap b2)
        {
            if (b1 == null)
                return b2;

            Bitmap bout = new Bitmap(b1.Width + b2.Width, b1.Height);
            using (Graphics g = Graphics.FromImage(bout))
            {
                g.DrawImage(b1, 0, 0);
                g.DrawImage(b2, b1.Width, 0);
            }
            return bout;
        }

        public static Bitmap ResizeBitmap(this Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        public static Bitmap ResizeBitmap(this Bitmap bmp, double scale)
        {
            int width = (int)((double)bmp.Width * scale);
            int height = (int)((double)bmp.Height * scale);
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

    }
}
