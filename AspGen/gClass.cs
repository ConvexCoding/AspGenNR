using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace gClass
{
    public struct Side
    {
        public double R;
        public double C;
        public double K;
        public double AD;
        public double AE;
        public int Type; // 0 = plane, 1 = sphere with or wo conic, 2 = poly asphere
    }

    public struct Lens
    {
        public double Diameter;
        public string Material;
        public string Type;
        public double ap;
        public double n;
        public double WL;
        public double CT;
        public Side Side1;
        public Side Side2;
        public double EFL;
        public double BFL;

        public Lens LensKTweak(double tweak, int side)
        {
            if (side == 1)
                this.Side1.K += tweak;
            else
                this.Side2.K += tweak;
            return this;
        }
        public Lens LensADTweak(double tweak, int side)
        {
            if (side == 1)
                this.Side1.AD += tweak;
            else
                this.Side2.AD += tweak;
            return this;
        }
        public Lens LensAETweak(double tweak, int side)
        {
            if (side == 1)
                this.Side1.AE += tweak;
            else
                this.Side2.AE += tweak;
            return this;
        }
    }

    public struct LensWOStrings
    {
        public double Diameter;
        public double ap;
        public double n;
        public double WL;
        public double CT;
        public Side Side1;
        public Side Side2;
        public double EFL;
        public double BFL;

        public LensWOStrings(Lens lens)
        {
            this.Diameter = lens.Diameter;
            this.ap = lens.ap;
            this.n = lens.n;
            this.WL = lens.WL;
            this.CT = lens.CT;
            this.Side1 = lens.Side1;
            this.Side2 = lens.Side2;
            this.EFL = lens.EFL;
            this.BFL = lens.BFL;
        }
    }

    public struct WFE_Ray
    {

        public Ray rstart { get; set; }
        public Ray rend { get; set; }
        public double opd { get; set; }
        public double lsa { get; set; }
        public int ix { get; set; }
        public int iy { get; set; }
        public int isvalid { get; set; }
        public WFE_Ray(Vector3D p, Vector3D e)
        {
            this.rstart = new Ray(p, e);
            this.rend = new Ray();
            this.opd = 0.0;
            this.lsa = 0.0;
            this.ix = 0;
            this.iy = 0;
            this.isvalid = 0;
        }
    }
    public struct WFERay
    {
        public double Yin { get; set; }
        public double Yend { get; set; }
        public double Zend { get; set; }
        public double AOI { get; set; }
        public double Slope { get; set; }
        public double LSA { get; set; }
        public double OPD { get; set; }

        // WFE(yin, yend, zend, aoi, slope eflr, lsa, opd)
        public WFERay (double yin, double yend, double zend, double aoi, double slope, double lsa, double opd)
        {
            this.Yin = yin;
            this.Yend = yend;
            this.Zend = zend;
            this.AOI = aoi;
            this.Slope = slope;
            this.LSA = lsa;
            this.OPD = opd;
        }
    }

    public class RayVector
    {
        PointD Pbegin { get; set; }
        PointD Pend { get; set; }
        double AOI { get; set; }
        double AOR { get; set; }
        double Index { get; set; }

        public RayVector(double x0, double y0, double x1, double y1, double index, double aoi, double aor)
        {
            this.Pbegin = new PointD(x0, y0);
            this.Pend = new PointD(x1, y1);
            this.Index = index;
            this.AOI = aoi;
            this.AOI = aor;
        }

        public RayVector(PointD p0, PointD p1, double index, double aoi, double aor)
        {
            this.Pbegin = new PointD(p0.X, p0.Y);
            this.Pend = new PointD(p1.X, p1.Y);
            this.Index = index;
            this.AOI = aoi;
            this.AOI = aor;
        }

        public double OPD()
        {
            double l = Math.Sqrt((this.Pend.X - this.Pbegin.X) * (this.Pend.X - this.Pbegin.X) + 
                                 (this.Pend.Y - this.Pbegin.Y) * (this.Pend.Y - this.Pbegin.Y));
            return l * this.Index;
        }        
    }

    public class PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD()
        {
            this.X = 0;
            this.Y = 0;
        }

        public PointD(double xin, double yin)
        {
            this.X = xin;
            this.Y = yin;
        }

        public static PointD operator +(PointD a, PointD b)
        {
            return new PointD(a.X + b.X, a.Y + b.Y);
        }

        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }

        public static PointD operator *(PointD a, PointD b)
        {
            return new PointD(a.X * b.X, a.Y * b.Y);
        }

        public static PointD operator /(PointD a, PointD b)
        {
            return new PointD(a.X / b.X, a.Y / b.Y);
        }

        public void SetPointD_X(double xin)
        {
            this.X = xin;
        }

        public void SetPointD_Y(double yin)
        {
            this.Y = yin;
        }

        public void SetPointD(double xin, double yin)
        {
            this.X = xin;
            this.Y = yin;
        }

        public bool IsAlmostZero()
        {
            if (Math.Sqrt(this.X * this.X + this.Y * this.Y) < 0.05)
                return true;
            else
                return false;
        }

        public string ToString(string format = null)
        {
            return "{X= " + this.X.ToString(format) + ",   Y= " + this.Y.ToString(format) + "}";
        }

        public Point ToIntegerPoint()
        {
            return new Point((int)Math.Round(this.X), (int)Math.Round(this.Y));
        }

        public PointF ToFloatPoint()
        {
            return new PointF((float)this.X, (float)this.Y);
        }
    }

    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public Point3D(double xin, double yin, double zin)
        {
            this.X = xin;
            this.Y = yin;
            this.Z = zin;
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Point3D operator *(Point3D a, Point3D b)
        {
            return new Point3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Point3D operator /(Point3D a, Point3D b)
        {
            return new Point3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public void SetPoint3D_X(double xin)
        {
            this.X = xin;
        }

        public void SetPoint3D_Y(double yin)
        {
            this.Y = yin;
        }

        public void SetPoint3D_Z(double zin)
        {
            this.Z = zin;
        }

        public void SetPoint3D(double xin, double yin, double zin)
        {
            this.X = xin;
            this.Y = yin;
            this.Z = zin;
        }

        public bool IsAlmostZero()
        {
            if (Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z) < 0.05)
                return true;
            else
                return false;
        }

        public string ToString(string format = "f4")
        {
            return "{X= " + this.X.ToString(format) +
                ",   Y= " + this.Y.ToString(format) +
                ",   Z= " + this.Z.ToString(format) + "}";
        }
    }

    public struct DirectionVector
    {
        public double DirX { get; set; }
        public double DirY { get; set; }
        public double DirZ { get; set; }

        public DirectionVector(double dirx, double diry, double dirz)
        {
            this.DirX = dirx;
            this.DirY = diry;
            this.DirZ = dirz;
        }
        public double Magnitude()
        {
            return Math.Sqrt(this.DirX * this.DirX + this.DirY * this.DirY + this.DirZ * this.DirZ);
        }

        public DirectionVector Normalize()
        {
            var mag = this.Magnitude();
            this.DirX /= mag;
            this.DirY /= mag;
            this.DirZ /= mag;

            return this;
        }

    }

    // added for 3d analysis

    public struct Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3D(double xin, double yin, double zin)
        {
            this.X = xin;
            this.Y = yin;
            this.Z = zin;
        }

        public static double DotProduct(Vector3D a, Vector3D b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        public double Length()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public double LengthSquared()
        {
            return (this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3D operator -(Vector3D a)
        {
            return new Vector3D(-a.X, -a.Y, -a.Z);
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3D operator *(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Vector3D operator *(double b, Vector3D a)
        {
            return new Vector3D(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3D operator /(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Vector3D operator /(Vector3D a, double b)
        {
            return new Vector3D(a.X / b, a.Y / b, a.Z / b);
        }

        public void SetVector3D_X(double xin)
        {
            this.X = xin;
        }

        public void SetVector3D_Y(double yin)
        {
            this.Y = yin;
        }

        public void SetVector3D_Z(double zin)
        {
            this.Z = zin;
        }

        public void SetVector3D(double xin, double yin, double zin)
        {
            this.X = xin;
            this.Y = yin;
            this.Z = zin;
        }

        public bool IsAlmostZero()
        {
            if (Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z) < 0.05)
                return true;
            else
                return false;
        }

        public string ToString(string format = "f4")
        {
            return "{X= " + this.X.ToString(format) +
                ",   Y= " + this.Y.ToString(format) +
                ",   Z= " + this.Z.ToString(format) + "}";
        }

        public string ToString2(string format = "f4")
        {
            return (this.X.ToString(format) + ", " + this.Y.ToString(format) + ", " + this.Z.ToString(format));
        }
    }

    public struct Ray
    {
        public Vector3D pvector { get; set; }
        public Vector3D edir { get; set; }

        public Ray(Vector3D position, Vector3D direction)
        {
            this.pvector = position;
            this.edir = direction;
        }
    }

    public struct GenRays
    {
        public uint baserays { get; set; }
        public uint angles { get; set; }
        public double half_ap { get; set; }
        public double half_ang { get; set; }

        public GenRays(uint _baserays, uint _angles, double _half_ap, double _half_angle)
        {
            this.baserays = _baserays;
            this.angles = _angles;
            this.half_ap = _half_ap;
            this.half_ang = _half_angle;
        }
    }

    public struct MicroGauss
    {
        public Vector3D Loc { get; set; }

        public Vector3D Slopes { get; set; }
        public double W0_Y { get; set; }
        public double W0_X { get; set; }

        public MicroGauss(Vector3D loc, Vector3D slopes, double w0x, double w0y)
        {
            this.Loc = new Vector3D(loc.X, loc.Y, loc.Z);
            this.Slopes = new Vector3D(slopes.X, slopes.Y, slopes.Z);
            this.W0_X = w0x;
            this.W0_Y = w0y;
        }

        public MicroGauss(double x, double y, double z, Vector3D slopes, double w0x, double w0y)
        {
            this.Loc = new Vector3D(x, y, z);
            this.Slopes = new Vector3D(slopes.X, slopes.Y, slopes.Z);
            this.W0_X = w0x;
            this.W0_Y = w0y;
        }

    }

    public struct ChartAxis
    {
        public double AxisMin { get; set; }
        public double AxisMax { get; set; }        
        public int NumberTics { get; set; }
        public double MajorInterval { get; set; }

        public ChartAxis(double min, double max, double interval)
        {
            this.AxisMax = max;
            this.AxisMin = min;
            this.MajorInterval = interval;
            this.NumberTics = (int)Math.Round((max - min) / interval);
        }

        public ChartAxis(double min, double max, int tics)
        {
            this.AxisMax = max;
            this.AxisMin = min;
            this.NumberTics = tics;
            this.MajorInterval = (max - min) / (double)tics;
        }
    }


}
