using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using gClass;
using AspGen;

namespace gExtensions
{
    public static class gExtensions
    {
        static public List<Vector3D> GenerateRandomRayVectors(this double ap, int baseraysct)
        {
            List<Vector3D> Vlist = new List<Vector3D>();
            double x, y;
            Random rd = new Random();
            for (int i = 0; i < baseraysct; i++)
            {
                x = ap * (rd.NextDouble() * 2.0 - 1.0);
                y = ap * (rd.NextDouble() * 2.0 - 1.0);
                while (RTM.Hypo(x, y) > ap)
                {
                    x = ap * (rd.NextDouble() * 2.0 - 1.0);
                    y = ap * (rd.NextDouble() * 2.0 - 1.0);
                }
                Vlist.Add(new Vector3D(x, y, 0));
            }
            return Vlist;
        }

        static public List<Vector3D> GenerateUniformRayVectors(this double ap, int numrays)
        {
            List<Vector3D> vlist = new List<Vector3D>();
            for (double y = -ap; y <= ap * 1.001; y += ap / numrays)
                for (double x = -ap; x <= ap * 1.001; x += ap / numrays)
                {
                    var h = Math.Sqrt(x * x + y * y);
                    if (h <= ap)
                        vlist.Add(new Vector3D(x, y, 0));
                }
            return vlist;
        }

        static public List<Vector3D> GenerateFibonacciRayVectors(this double ap, int baseraysct)
        {
            List<Vector3D> vlist = new List<Vector3D>();

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
            for (int i = 0; i < baseraysct; i++)
            {
                double x = (double)i / goldenratio - Math.Truncate((double)i / goldenratio);
                double y = (double)i / baseraysct;
                List<double> theta = new List<double>();
                List<double> rad = new List<double>();
                if (Math.Sqrt(x * x + y * y) <= 10)
                {
                    theta.Add(2 * Math.PI * x);
                    rad.Add(Math.Sqrt(y));
                    double xs = (ap * 0.1 * 10 * Math.Sqrt(y) * Math.Cos(2 * Math.PI * x));
                    double ys = (ap * 0.1 * 10 * Math.Sqrt(y) * Math.Sin(2 * Math.PI * x));
                    vlist.Add(new Vector3D(xs, ys, 0));
                }
            }
            return vlist;
        }

        static public double VDistance(this Vector3D v0, Vector3D v1)
        {
            return Math.Sqrt( (v0.X - v1.X) * (v0.X - v1.X) +
                              (v0.Y - v1.Y) * (v0.Y - v1.Y) +
                              (v0.Z - v1.Z) * (v0.Z - v1.Z) );
        }

        static public int SetSurfaceType(this Side side)
        {
            if ((Math.Abs(side.R) < 0.01) &&
                 (Math.Abs(side.K) < 1e-8) &&
                 (Math.Abs(side.AD) < 1e-20) &&
                 (Math.Abs(side.AE) < 1e-20))
                return 0;

            if ((Math.Abs(side.AD) < 1e-20) &&
                 (Math.Abs(side.AE) < 1e-20))
                return 1;

            return 2;
        }

        //
        //
        static public double SetCurvature(this double R, double RtolforZero = 0.01)
        {
            if (Math.Abs(R) > RtolforZero)
                return 1 / R;
            else
                return 0;
        }

        static public (DirectionVector Norms, double F) SurfNorm(this Point3D pt, Side s)
        {
            var p = pt.X * pt.X + pt.Y * pt.Y;
            var zpartial = (pt.Z - s.AD * p * p - s.AE * p * p * p);
            var F = -(s.C / 2) * p * (s.K + 1) - (s.C / 2) * (s.K + 1) * zpartial * zpartial + zpartial;
            var t2 = (s.C * (1 - 2 * (s.K + 1) * (2 * s.AD * p + 3 * s.AE * p * p) * zpartial) + (4 * s.AD * p + 6 * s.AE * p * p));

            var norms = new DirectionVector(-pt.X * t2, -pt.Y * t2, 1 - s.C * (s.K + 1) * zpartial).Normalize();
            return (norms, F);
        }

        static public (DirectionVector Norms, double F) SurfNormShort(this Point3D pt, Side s)
        {
            var p = pt.X * pt.X + pt.Y * pt.Y;

            var F = pt.Z - (s.C / 2) * p - (s.C / 2) * (s.K + 1) * pt.Z * pt.Z;

            var dFdz = 1 - s.C * (s.K + 1) * pt.Z;

            var norms = new DirectionVector(-pt.X * s.C, -pt.Y * s.C, 1 - s.C * (s.K + 1) * pt.Z).Normalize();
            return (norms, F);
        }
        //
        //
        static public double[] GenZeroArray(this int size)
        {
            double[] array = new double[size];
            for (int i = 0; i < size; i++)
                array[i] = 0;
            return array;
        }
        static public double[,] Gen2DZeroArray(this int size)
        {
            var array = new double[size, size];
            for (int c = 0; c < size; c++)
                for (int r = 0; r < size; r++)
                    array[r,c] = 0;
            return array;
        }
        static public double MaxDAbs(this List<double> xs)
        {
            double max = 0;
            foreach (double x in xs)
                if (Math.Abs(x) > max)
                    max = Math.Abs(x);
            return max;
        }
        static public double RMS(this List<double> ld)
        {
            double sumsq = 0;
            double ave = ld.Average();
            foreach(double x in ld)
                sumsq += (x - ave) * (x - ave);
            return Math.Sqrt(sumsq / (ld.Count() - 1));
        }


        static public (float MinX, float MinY, float MaxX, float MaxY) MinMaxFloatListPt(this List<PointF> list)
        {
            float minx = 1e8F;
            float miny = 1e8F;
            float maxx = -1e8F;
            float maxy = -1e8F;

            foreach (PointF p in list)
            {
                if (p.X < minx)
                    minx = p.X;
                if (p.Y < miny)
                    miny = p.Y;
                if (p.X > maxx)
                    maxx = p.X;
                if (p.Y > maxy)
                    maxy = p.Y;
            }
            return (minx, miny, maxx, maxy);
        }

        static public (float MinX, float MinY, float MaxX, float MaxY) MinMaxFloatListPt(this PointF[] list)
        {
            float minx = 1e8F;
            float miny = 1e8F;
            float maxx = -1e8F;
            float maxy = -1e8F;

            foreach (PointF p in list)
            {
                if (p.X < minx)
                    minx = p.X;
                if (p.Y < miny)
                    miny = p.Y;
                if (p.X > maxx)
                    maxx = p.X;
                if (p.Y > maxy)
                    maxy = p.Y;
            }
            return (minx, miny, maxx, maxy);
        }

        static public (double MinX, double MinY, double MaxX, double MaxY) MinMaxVectorList(this List<Vector3D> list)
        {
            double minx = 1e8F;
            double miny = 1e8F;
            double maxx = -1e8F;
            double maxy = -1e8F;

            foreach (Vector3D p in list)
            {
                if (p.X < minx)
                    minx = p.X;
                if (p.Y < miny)
                    miny = p.Y;
                if (p.X > maxx)
                    maxx = p.X;
                if (p.Y > maxy)
                    maxy = p.Y;
            }
            return (minx, miny, maxx, maxy);
        }

        static public (float MinX, float MinY, float MaxX, float MaxY) MinMaxFloatPts(this PointF[] pts)
        {
            float minx = 1e8F;
            float miny = 1e8F;
            float maxx = -1e8F;
            float maxy = -1e8F;

            //foreach (PointF p in list)
            for (int i = 0; i < pts.Count(); i++)
            {
                if (pts[i].X < minx)
                    minx = pts[i].X;
                if (pts[i].Y < miny)
                    miny = pts[i].Y;
                if (pts[i].X > maxx)
                    maxx = pts[i].X;
                if (pts[i].Y > maxy)
                    maxy = pts[i].Y;
            }
            return (minx, miny, maxx, maxy);
        }


        static public PointD TraceRayThruLens(double x1, double y1, double m, Lens lensp)
        {
            // first condition is if Surface 2 is flat or infinity
            if (Math.Abs(lensp.Side2.R) < 0.01)
            {
                return new PointD(lensp.CT, y1 + m * (lensp.CT - x1));
            }

            // If radius is > 1 then
            //
            double xtest1 = lensp.CT;
            double xtest2 = y1.Sag(lensp.Side2) + lensp.CT;
            double xstart = lensp.CT;
            double y2 = m * (xstart - x1) + y1;

            for (int i = 0; i < 10; i++)             // i in range(10) :
            {
                xstart += (xtest2 - xtest1);
                xtest1 = xstart;
                y2 = m * (xstart - x1) + y1;
                xtest2 = y2.Sag(lensp.Side2) + lensp.CT;
                if (Math.Abs(xstart - xtest2) < 0.0000001)
                    break;
            }
            
            double x2 = y2.Sag(lensp.Side2);

            return new PointD(x2 + lensp.CT, y2);
        }

        static public WFERay CalcRayWFE(this double y0, Lens lensp, double Refocus)
        {
            // ******  Note that this function returns wfe opd in waves ******** //
            // check for axis ray - return 0 if it is
            if (Math.Abs(y0) < 0.0001)
            {
                var Y0 = y0.CalcLSA(lensp, 0);
                return new WFERay(0, 0, Y0.Zend, 0, 0, 0, 0);
            }

            // note that during wavefront error calculations refocus must be set to zero. Refocus
            // is used in final opd calculation below.

            var Ym = y0.CalcLSA(lensp, 0);
            var Yz = (y0 / Math.Sqrt(2)).CalcLSA(lensp, 0);

            double a = (4 * Yz.LSA - Ym.LSA) / Math.Pow(y0, 2);
            double b = (2 * Ym.LSA - 4 * Yz.LSA) / Math.Pow(y0, 4);
            double opd = 1000 * (Math.Sin(Ym.AOI3) * Math.Sin(Ym.AOI3) / 2) *
                                 (Refocus - a * Math.Pow(y0, 2) / 2 - b * Math.Pow(y0, 4) / 3) / lensp.WL;   // convert to waves /WL

            var yfinal = y0.CalcLSA(lensp, Refocus);
            
            // ******  Note that this function returns wfe opd in waves ******** //
            return new WFERay(y0, yfinal.Y3, yfinal.Zend, yfinal.AOI3, Math.Tan(yfinal.AOI3), yfinal.LSA, opd);
        }

        static public WFERay CalcRayWFE_Plus(Vector3D Vin, Vector3D Ein, Lens lensp, double Refocus)
        {
            // ******  Note that this function returns wfe opd in waves ******** //
            // check for axis ray - return 0 if it is
            if (Math.Abs(Vin.Y) < 0.0001)
            {
                var (Vout, Eout, aoi, lsa) = RTM.Trace_3D_Extra(Vin, Ein, lensp, Refocus);
                double slope = 0;
                double opd6 = 0;
                return new WFERay(Vin.Y, Vout.Y, Vout.Z, aoi, slope, lsa, opd6);
            }

            // note that during wavefront error calculations refocus must be set to zero. Refocus
            // is used in final opd calculation below.

            var Ym = Vin.Y.CalcLSA(lensp, 0);
            var Yz = (Vin.Y / Math.Sqrt(2)).CalcLSA(lensp, 0);

            double a = (4 * Yz.LSA - Ym.LSA) / Math.Pow(Vin.Y, 2);
            double b = (2 * Ym.LSA - 4 * Yz.LSA) / Math.Pow(Vin.Y, 4);
            double opd = 1000 * (Math.Sin(Ym.AOI3) * Math.Sin(Ym.AOI3) / 2) *
                                 (Refocus - a * Math.Pow(Vin.Y, 2) / 2 - b * Math.Pow(Vin.Y, 4) / 3) / lensp.WL;   // convert to waves /WL

            var yfinal = Vin.Y.CalcLSA(lensp, Refocus);

            // ******  Note that this function returns wfe opd in waves ******** //
            return new WFERay(Vin.Y, yfinal.Y3, yfinal.Zend, yfinal.AOI3, Math.Tan(yfinal.AOI3), yfinal.LSA, opd);
        }

        static public double SagShort(this double R, double r)
        {
            double value = R * R - r * r;
            if (value < 0)
                return 0;
            return R - Math.Sqrt(value);
        }

        static public double Sag(this double y, Side side, double RtolforZero = 0.001)
        {

            double sqrtvalue = 1 - (1 + side.K) * side.C * side.C * y * y;
            if (sqrtvalue < 0)
                return 0;
            else
                return (side.C * y * y / (1 + Math.Sqrt(sqrtvalue))) + side.AD * Math.Pow(y, 4) + side.AE * Math.Pow(y, 6);
        }

        static public double SagB(this double y, double R, double k = 0, double ad = 0, double ae = 0, double RtolforZero = 0.001)
        {
            double C = R.SetCurvature();
            double sqrtvalue = 1 - (1 + k) * C * C * y * y;
            if (sqrtvalue < 0)
                return 0;
            else
                return C * y * y / (1 + Math.Sqrt(sqrtvalue)) + ad * Math.Pow(y, 4) + ad * Math.Pow(y, 6);
        }

        static public double Sag2D(this PointD p, Side side, double RtolforZero = 0.001)
        {
            double C = 0;
            if (Math.Abs(side.R) > RtolforZero)
                C = 1 / side.R;

            double r2 = (p.X * p.X + p.Y * p.Y);
            double sqrtvalue = 1 - (1 + side.K) * C * C * r2;

            if (sqrtvalue < 0)
                return 0;
            else
                //return (C * (p.X * p.X + p.Y * p.Y) / (1 + Math.Sqrt(sqrtvalue))) + side.AD * Math.Pow((p.X * p.X + p.Y * p.Y), 2) + side.AE * Math.Pow((p.X * p.X + p.Y * p.Y), 3);
                return (C * r2 / (1 + Math.Sqrt(sqrtvalue))) + side.AD * r2 * r2 + side.AE * r2 * r2 * r2;
        }

        static public (Vector3D, double) Slope3D(this Vector3D P, Side s)
        {
            double p = P.X * P.X + P.Y * P.Y;
            double q0 = (P.Z - s.AD * p * p - s.AE * p * p * p);
            double q1 = (-4 * s.AD * p - 6 * s.AE * p * p);

            double dx = P.X * (-s.C - s.C * (s.K + 1) * q1 * q0 + q1);
            double dy = P.Y * (-s.C - s.C * (s.K + 1) * q1 * q0 + q1);
            double dz = 1 - s.C * (s.K + 1) * q0;

            var N = new Vector3D(dx, dy, dz);
            var n = N / N.Length();
            double F = -(s.C / 2) * p - (s.C / 2) * (s.K + 1) * q0 * q0 + q0;
            // n is surface normal
            // F Functional form of aspheric equation
            return (n, F);
        }

        static public double Slope(this double y, Side s, double delta = 5e-6)
        {
            double sp = (y + delta).Sag(s);
            double sm = (y - delta).Sag(s);
            double slope = (sp - sm) / (2 * delta);
            return Math.Atan(slope);
        }

        static public double SlopeC(this double y, Side side)
        {
            if (Math.Abs(side.R) < 0.001)  // flat surface
                return 0.0;

            double divs = Math.Sqrt(1 - (1 + side.K) * y * y * side.C * side.C);

            double slope = -(side.C * (side.C * side.C * Math.Pow(y, 3) * (-1 - side.K) + 2 * y * divs + 2 * y) /
                               (Math.Pow((1 + divs), 2) * divs) + 
                               4 * side.AD * Math.Pow(y, 3) + 
                               6 * side.AE * Math.Pow(y, 5));

            return slope;

        }

        static public double SlopeB(this double y, double R, double k = 0, double A4 = 0, double A6 = 0, double delta = 5e-6)
        {
            double sp = (y + delta).SagB(R, k, A4, A6);
            double sm = (y - delta).SagB(R, k, A4, A6);
            double slope = (sp - sm) / (2 * delta);
            return Math.Atan(slope);
        }


        static public PointF ToPointF(this Point p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        static public PointD RotatePointD(this PointD pt, double theta)
        {
            PointD o = new PointD(0, 0);
            PointD rpt = new PointD(0, 0);
            double angle = theta * Math.PI / 180;
            rpt.X = o.X + Math.Cos(angle) * (pt.X - o.X) - Math.Sin(angle) * (pt.Y - o.Y);
            rpt.Y = o.Y + Math.Sin(angle) * (pt.X - o.X) + Math.Cos(angle) * (pt.Y - o.Y);
            return rpt;
        }

        static public double RadToDeg(this double xrad)
        {
            return (xrad * 180 / Math.PI);
        }

        static public double DegToRad(this double xrad)
        {
            return (xrad * Math.PI / 180);
        }

        static public (double Vmin, double Vmax, double Diff, double RMS) minmaxD(this List<double> xs)
        {
            double min = xs.Min();
            double max = xs.Max();
            double xtotalsq = 0;
            foreach (double x in xs)
                xtotalsq += x * x;
            double rms = Math.Sqrt(xtotalsq / xs.Count);
            return (min, max, max - min, rms);
        }

        static public double YscaleValue(this double y)
        {
            var ylog = Math.Log10(y); //-0.25884840114821
            int i = (int)ylog;  // 0
            var yt0 = ylog - i;  //-0.25884840114821
            var yt1 = Math.Pow(10, yt0) * 10;  // 5.51
            var yt2 = Math.Ceiling(yt1);  // 6.0
            var yt3 = yt2 / 10;
            var yt4 = Math.Log10(yt3) + i;
            var ysc = Math.Pow(10, yt4);  // 0.6
            return ysc;
        }

        static public StringBuilder ConvertDoublesToStringBuilder(this double[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            StringBuilder sb = new StringBuilder();
            for (int w = 0; w < width; w++)
            {
                StringBuilder oneline = new StringBuilder();
                for (int h = 0; h < height - 1; h++)
                    if (!double.IsNaN(data[h, w]))
                        oneline.Append(data[h, w].ToString() + ", ");
                    else
                        oneline.Append("0, ");
                if (!double.IsNaN(data[height - 1, w]))
                    sb.Append(oneline + data[height - 1, w].ToString() + "\n");
                else
                    sb.Append(oneline + "0\n");
            }
            return sb;
        }

        static public StringBuilder ConvertDoubleGAussApodToStringBuilder(this double[,] data, double hwidth)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            StringBuilder sb = new StringBuilder();
            double scale = 2 * hwidth / width;
            double w0 = 5;
            double w0_2 = w0 * w0;
            for (int w = 0; w < width; w++)
            {
                StringBuilder oneline = new StringBuilder();
                for (int h = 0; h < height - 1; h++)
                {
                    if (!double.IsNaN(data[h, w]))
                    {
                        var x = scale * (width / 2 - w);
                        var y = scale * (height / 2 - h);
                        var r_2 = (x * x + y * y);
                        var p = Math.Exp(-2 * r_2 / w0_2);
                        oneline.Append(p.ToString() + ", ");
                    }
                    else
                        oneline.Append("0, ");
                }
                if (!double.IsNaN(data[height - 1, w]))
                {
                    var r_2 = scale * (height * height + w * w);
                    var p = Math.Exp(-2 * r_2 / w0_2);
                    sb.Append(oneline + p.ToString() + "\n");
                }
                else
                    sb.Append(oneline + "0\n");
            }
            return sb;
        }

        static public StringBuilder ConvertDoubleUnitApodToStringBuilder(this double[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            StringBuilder sb = new StringBuilder();

            for (int w = 0; w < width; w++)
            {
                StringBuilder oneline = new StringBuilder();
                for (int h = 0; h < height - 1; h++)
                    if (!double.IsNaN(data[h, w]))
                        oneline.Append("1, ");
                    else
                        oneline.Append("0, ");
                if (!double.IsNaN(data[height - 1, w]))
                    sb.Append(oneline + "1\n");
                else
                    sb.Append(oneline + "0\n");
            }
            return sb;
        }

        static public StringBuilder ConvertDoublePhasesToStringBuilder(this double[,] data, double wavelength, double ap)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            StringBuilder sb = new StringBuilder();
            double factor = 2* Math.PI;

            int centerwidth = width / 2;
            int centerheight = height / 2;
            double pixelscale = 1.0 / centerwidth;

            for (int w = 0; w < width; w++)
            {
                StringBuilder oneline = new StringBuilder();
                for (int h = 0; h < height - 1; h++)
                    if (!double.IsNaN(data[h, w]))
                    {
                        var rhox = (w - centerwidth) * pixelscale;
                        var rhoy = (h - centerheight) * pixelscale;
                        var rho = rhox * rhox + rhoy * rhoy;
                        oneline.Append((data[h, w] * factor).ToString() + ", ");
                    }
                    else
                        oneline.Append("0, ");
                if (!double.IsNaN(data[height - 1, w]))
                {
                    var rhox = (w - centerwidth) * pixelscale;
                    var rhoy = (height - 1 - centerheight) * pixelscale;
                    var rho = rhox * rhox + rhoy * rhoy;
                    sb.Append(oneline + (data[height - 1, w] * factor).ToString() + "\n");
                }
                else
                    sb.Append(oneline + "0\n");
            }
            return sb;
        }

        public static double[,] NormalizeData(this double[,] data, double nf = 100.0)
        {
            int rmax = data.GetLength(0);
            int cmax = data.GetLength(1);
            var (Min, Max) = data.FindMinMax();
            double[,] outdata = new double[rmax, cmax];
            for (int r = 0; r < rmax; r++)
                for (int c = 0; c < cmax; c++)
                    outdata[r, c] = nf * (data[r, c] - Min) / (Max - Min);
            return outdata;
        }

        public static double[] NormalizeData(this double[] data, double nf = 100.0)
        {
            int rmax = data.GetLength(0);
            var (Min, Max) = data.FindMinMax();
            for (int r = 0; r < rmax; r++)
                    data[r] = nf * (data[r] - Min) / (Max - Min);
            return data;
        }

        public static double[] ScaleData(this double[] data, double scale = 1.0)
        {
            int rmax = data.GetLength(0);
            for (int r = 0; r < rmax; r++)
                data[r] = data[r] / scale;
            return data;
        }

        public static double[,] ScaleData(this double[,] data, double scale)
        {
            int rmax = data.GetLength(0);
            int cmax = data.GetLength(1);
            double[,] outdata = new double[rmax, cmax];
            for (int r = 0; r < rmax; r++)
                for (int c = 0; c < cmax; c++)
                    outdata[r, c] = data[r, c] / scale;
            return outdata;
        }

        public static double SumData(this double[,] data)
        {
            int rmax = data.GetLength(0);
            int cmax = data.GetLength(1);
            double TotalSum = 0;
            for (int r = 0; r < rmax; r++)
                for (int c = 0; c < cmax; c++)
                    TotalSum += data[r, c];
            return TotalSum;
        }

        public static double[] SliceDataMidPt(this double[,] data, int border = 0)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            int midrow = rows / 2;
            double[] odata = new double[cols];

            for (int c = 0; c < cols; c++)
            {
                double sum = 0;
                for (int r = midrow - border; r <= midrow + border; r++)
                    sum += data[r, c];
                odata[c] = sum / (1 + 2 * border);
            }

            return odata;           
        }

        public static double[] DivideDoubles(this double[] data, double divisor)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] /= divisor;
            return data;
        }
        
        public static double[] GenerateLinearArray(this int asize, double start)
        {
            double[] x = new double[asize];
            var xmin = -start;
            var xmax = start;
            double stepsize = (xmax - xmin) / (asize - 1);
            for (int i = 0; i < asize; i++)
                x[i] = xmin + i * stepsize;

            return x;
        }
        
        public static (double Min, double Max) FindMinMax(this double[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            double Min = 1e20;
            double Max = -1e20;

            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                {
                    if (data[w, h] < Min)
                        Min = data[w, h];
                    if (data[w, h] > Max)
                        Max = data[w, h];
                }

            return (Min, Max);
        }

        public static (double Min, double Max) FindMinMax(this double[] data)
        {
            int width = data.Count();

            double Min = 1e20;
            double Max = -1e20;

            for (int w = 0; w < width; w++)
            {
                if (data[w] < Min)
                    Min = data[w];
                if (data[w] > Max)
                    Max = data[w];
            }

            return (Min, Max);
        }

        public static (double Min, double Max) MinMax1D(this double[] data)
        {
            int width = data.Count();

            double Min = 1e20;
            double Max = -1e20;

            for (int w = 0; w < width; w++)
                {
                    if (data[w] < Min)
                        Min = data[w];
                    if (data[w] > Max)
                        Max = data[w];
                }

            return (Min, Max);
        }

        public static List<double> NormalizeList(this List<double> l)
        {
            List<double> olist = new List<double>();
            (double min, double max) = (l.ToArray()).MinMax1D();
            for(int i = 0; i < l.Count(); i++)
                olist.Add(l[i] / (max - min));
            return olist;
        }

    }

}