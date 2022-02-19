using System;

using gClass;
using gExtensions;
using System.Data;
using System.Drawing;
using System.Collections.Generic;

namespace AspGen
{
    public static class RTM
    {
        public static string formstr(int i, double y, double x, double z, double m, double aoi, double aor, double norm)
        {
            string deform = "f6";
            string line = i.ToString() + ", " +
                          y.ToString(deform) + ", " +
                          x.ToString(deform) + ", " +
                          z.ToString(deform) + ", " +
                          m.ToString(deform) + ", " +
                          aoi.RadToDeg().ToString(deform) + ", " +
                          aor.RadToDeg().ToString(deform) + ", " +
                          norm.ToString(deform) + "\n";
            return line;
        }

        public static Func<double, double, double> Hypo = (x, y) => (Math.Sqrt(x * x + y * y));

        public static Func<double, double, double> Hypo2 = (x, y) => (x * x + y * y);

        public static Func<Vector3D, Vector3D, double> F_AOI = (x, y) => Math.Cos(Vector3D.DotProduct(x, y));

        public static Func<double, double, double> F_LSA = (x, y) => x / Math.Tan(y);

        // double aoi = Math.Acos(Vector3D.DotProduct(Eout / Eout.Length(), new Vector3D(0, 0, 1)));
        // double lsa = Vout.Y / Math.Tan(aoi);

        // ray trace methods - 3D 
        public static Vector3D Trace_3D(double y, Lens lens_in, double Refocus)
        {
            // D or P are position vectors
            // C center points of surfaces 
            // E are ray direction cosine vectors
            // N are surface normals
            // Z is the zero vector used as place holder in data table

            Vector3D P0 = new Vector3D(0, y, 0);
            Vector3D E0 = new Vector3D(0, 0, 1);

            // P1 reserved for later expansion by placing entrance pupil before lens to mimic S.L.'s
            // Trace to Side 1 after Refraction
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lens_in.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var E3= calc_dir_sines(E2, N3, lens_in.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + Refocus);
            //var E4 = E3;
            //var N4 = new Vector3D(0, 0, 1);
            //var aoi4 = Math.Acos(Vector3D.DotProduct(E3, N4)).RadToDeg();
            return P4;
        }

        public static Vector3D Trace_3D(Vector3D P0, Vector3D E0, Lens lens_in, double Refocus)
        {
            // D or P are position vectors
            // C center points of surfaces 
            // E are ray direction cosine vectors
            // N are surface normals
            // Z is the zero vector used as place holder in data table

            //var P2 = TraceRayToSide1_A(P0, E0, lens);
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lens_in.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var E3 = calc_dir_sines(E2, N3, lens_in.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + Refocus);
            //var E5 = E3;
            //var N5 = new Vector3D(0, 0, 1);
            //var aoi5 = Math.Acos(Vector3D.DotProduct(E3, N5)).RadToDeg();
            return P4;
        }

        public static (Vector3D Vout, Vector3D Cout) Trace_3D_Plus(Vector3D P0, Vector3D E0, Lens lens_in, double Refocus)
        {
            // D or P are position vectors
            // C center points of surfaces 
            // E are ray direction cosine vectors
            // N are surface normals
            // Z is the zero vector used as place holder in data table

            //var P2 = TraceRayToSide1_A(P0, E0, lens);
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lens_in.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var E3 = calc_dir_sines(E2, N3, lens_in.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + Refocus);
            //var E5 = E3;
            //var N5 = new Vector3D(0, 0, 1);
            //var aoi5 = Math.Acos(Vector3D.DotProduct(E3, N5)).RadToDeg();
            return (P4, E3);
        }

        public static (Vector3D Vout, Vector3D Cout, double, double) Trace_3D_Extra(Vector3D P0, Vector3D E0, Lens lens_in, double Refocus)
        {
            // D or P are position vectors
            // C center points of surfaces 
            // E are ray direction cosine vectors
            // N are surface normals
            // Z is the zero vector used as place holder in data table

            //var P2 = TraceRayToSide1_A(P0, E0, lens);
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lens_in.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var E3 = calc_dir_sines(E2, N3, lens_in.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + Refocus);
            var E4 = E3;
            var N5 = new Vector3D(0, 0, 1);
            var aoi4 = Math.Acos(Vector3D.DotProduct(E4 / E4.Length(), N5));
            double lsa = (Math.Abs(aoi4) < 1e-8) ? 0.0 : P4.Y / Math.Tan(aoi4);

            return (P4, E3, aoi4, lsa);
        }

        public static DataTable Trace_ToDataTable(Vector3D P0, Vector3D E0, Lens lens_in, double Refocus)
        {
            // D or P are position vectors
            // E are ray direction cosine vectors
            // N are surface normals

            DataTable dt = new DataTable();
            dt.Columns.Add("P", typeof(Vector3D));
            dt.Columns.Add("E", typeof(Vector3D));
            dt.Columns.Add("N", typeof(Vector3D));
            dt.Columns.Add("Ninc", typeof(double));
            dt.Columns.Add("Nref", typeof(double));
            dt.Columns.Add("AOI", typeof(double));
            dt.Columns.Add("AOR", typeof(double));

            dt.Rows.Add(P0, E0, new Vector3D(0, 0, 0), 1.0, 1.0, 0, 0);

            //Trace to Side 1 after refraction
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var (E2, aoi2, aor2) = CalcDirectionSines(E0, N2, 1.0, lens_in.n);  // after refraction
            dt.Rows.Add(P2, E2, N2, 1.0, lens_in.n, aoi2, aor2);

            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var (E3, aoi3, aor3) = CalcDirectionSines(E2, N3, lens_in.n, 1);
            dt.Rows.Add(P3, E3, N3, lens_in.n, 1, aoi3, aor3);

            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + Refocus);
            var N4 = new Vector3D(0, 0, 1);
            var E4 = E3;
            var aoi4 = Math.Acos(Vector3D.DotProduct(E3, N4)).RadToDeg();

            dt.Rows.Add(P4, E4, N4, 1, 1, aoi4, aoi4);
            return dt;
        }

        public static List<Vector3D> TraceWithAngles(Lens lens, double Refocus, List<Vector3D> vin, double angin)
        {
            List<Vector3D> Vlist = new List<Vector3D>();
            double dirx, diry, dirz;
            Random rd = new Random();
            Vector3D Pout;
            int baseraysct = Properties.Settings.Default.ExtSrcNoofBaseRays;
            int noofangles = Properties.Settings.Default.ExtSrcNoofAngles;
            for (int i = 0; i < vin.Count; i++)
            {
                for (int j = 0; j < noofangles; j++)
                {
                    dirx = rd.NextDouble() * 2 * angin - angin;
                    diry = rd.NextDouble() * 2 * angin - angin;
                    while (RTM.Hypo(dirx, diry) > angin)
                    {
                        dirx = rd.NextDouble() * 2 * angin - angin;
                        diry = rd.NextDouble() * 2 * angin - angin;
                    }
                    dirz = Math.Sqrt(1 - dirx * dirx - diry * diry);
                    Pout = RTM.Trace_3D(vin[i], new Vector3D(dirx, diry, dirz), lens, Refocus);
                    Vlist.Add(Pout);
                }
            }
            return Vlist;
        }

        public static List<Vector3D> TraceDoubleVector(Lens lens, double Refocus, List<(Vector3D Vpos, Vector3D Vdir)> Vin)
        {
            Vector3D Pout;
            List<Vector3D> Vlist = new List<Vector3D>();
            foreach((Vector3D Vpos, Vector3D Vdir) in Vin)
            {
                Pout = RTM.Trace_3D(Vpos, Vdir, lens, Refocus);
                Vlist.Add(Pout);                
            }
            return Vlist;
        }

        public static List<(Vector3D VPosition, Vector3D VDirection)> AddAnglesToVectors(Lens lens, double Refocus, List<Vector3D> vin, double angin)
        {
            List<(Vector3D, Vector3D)> Vlist = new List<(Vector3D, Vector3D)>();
            double dirx, diry, dirz;
            Random rd = new Random();
            int baseraysct = Properties.Settings.Default.ExtSrcNoofBaseRays;
            int noofangles = Properties.Settings.Default.ExtSrcNoofAngles;
            for (int i = 0; i < vin.Count; i++)
            {
                for (int j = 0; j < noofangles; j++)
                {
                    dirx = rd.NextDouble() * 2 * angin - angin;
                    diry = rd.NextDouble() * 2 * angin - angin;
                    while (RTM.Hypo(dirx, diry) > angin)
                    {
                        dirx = rd.NextDouble() * 2 * angin - angin;
                        diry = rd.NextDouble() * 2 * angin - angin;
                    }
                    dirz = Math.Sqrt(1 - dirx * dirx - diry * diry);
                    Vlist.Add((vin[i], new Vector3D(dirx, diry, dirz)));
                }
            }
            return Vlist;
        }

        public static double[,] ProcessRayData(List<Vector3D> Vlist, Lens lens, double Refocus, double fiber_radius)
        {
            double binsize = Properties.Settings.Default.ExtScrBinSize;
            double binsPermm = 1 / binsize;
            double minbin = -2 * fiber_radius;
            double maxbin = 2 * fiber_radius;

            int sbin = (int)(((maxbin - minbin) + 0.00005) / binsize) + 1;
            var indata = sbin.Gen2DZeroArray();
            int errors = 0;
            foreach (Vector3D P in Vlist)
            {
                int row = (int)Math.Round((P.X - minbin) / binsize);
                int col = (int)Math.Round((P.Y - minbin) / binsize);
                if ((row >= 0) && (row < sbin) && (col >= 0) && (col < sbin))
                {
                    indata[row, col] += 1;
                }
                else
                    errors++;
            }
            return indata;
        }

        public static double[,] ProcessRustRayData(Ray[] Raysin, Lens lens, double Refocus, double fiber_radius)
        {
            double binsize = Properties.Settings.Default.ExtScrBinSize;
            double binsPermm = 1 / binsize;
            double minbin = -2 * fiber_radius;
            double maxbin = 2 * fiber_radius;

            int sbin = (int)(((maxbin - minbin) + 0.00005) / binsize) + 1;
            var indata = sbin.Gen2DZeroArray();
            int errors = 0;
            for(int i = 0; i < Raysin.Length; i++)
            {
                int row = (int)Math.Round((Raysin[i].pvector.X - minbin) / binsize);
                int col = (int)Math.Round((Raysin[i].pvector.Y - minbin) / binsize);
                if ((row >= 0) && (row < sbin) && (col >= 0) && (col < sbin))
                {
                    indata[row, col] += 1;
                }
                else
                    errors++;
            }
            return indata;
        }

        public static PointF[] TraceRayPath(this double y0, Lens lensp, float refocus, float xoffset)
        {
            // check for axis ray - return 0 if it is
            if (Math.Abs(y0) < 0.001)
            {
                PointF[] zerofpts =
                {
                    new PointF(0, 0F),
                    (new PointF((float)lensp.CT + xoffset, 0.0F)),
                    (new PointF((float)lensp.CT + (float)lensp.BFL + xoffset + refocus, 0.0F))

                };
                return zerofpts;
            }

            Vector3D P0 = new Vector3D(0, y0, 0);
            Vector3D E0 = new Vector3D(0, 0, 1);

            //Surf 2 before refraction
            var P2 = TraceRayToSurface(P0, E0, lensp.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lensp.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lensp.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lensp.Side2, lensp.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lensp.CT).Slope3D(lensp.Side2);  // adjust z for CT of lens
            var E3 = calc_dir_sines(E2, N3, lensp.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lensp.CT + lensp.BFL + refocus);

            PointF[] fpts =
            {
                new PointF(0, (float)P0.Y),
                new PointF((float)P0.Z + xoffset, (float)P0.Y),
                new PointF((float)P2.Z + xoffset, (float)P2.Y),
                new PointF((float)P3.Z + xoffset, (float)P3.Y),
                new PointF((float)P4.Z + xoffset, (float)P4.Y)
            };

            return fpts;
        }

        static public (double Y3, double Zend, double LSA, double AOI3) CalcLSA(this double y0, Lens lens_in, double refocus)
        {
            // this methods should be optimized as much as possible since mapping WFE in 2D requires a little HP
            // 
            Vector3D P0 = new Vector3D(0, y0, 0);
            Vector3D E0 = new Vector3D(0, 0, 1);

            // P1 reserved for later expansion by placing entrance pupil before lens to mimic S.L.'s
            // Trace to Side 1 after Refraction
            var P2 = TraceRayToSurface(P0, E0, lens_in.Side1, 0.0);
            var (N2, F) = P2.Slope3D(lens_in.Side1);
            var E2 = calc_dir_sines(E0, N2, 1.0, lens_in.n);  // after refraction


            // Trace to Surface 2 after refraction
            var P3 = TraceRayToSurface(P2, E2, lens_in.Side2, lens_in.CT);
            var (N3, F3) = new Vector3D(P3.X, P3.Y, P3.Z - lens_in.CT).Slope3D(lens_in.Side2);  // adjust z for CT of lens
            var E3 = calc_dir_sines(E2, N3, lens_in.n, 1);


            // transfer ray to image plane
            var P4 = TranslateZ_Flat(P3, E3, lens_in.CT + lens_in.BFL + refocus);
            var aoi4 = Math.Acos(Vector3D.DotProduct(E3, new Vector3D(0, 0, 1)));
            double lsa2 = P4.Y / Math.Tan(aoi4);

            return (P4.Y, P4.Z, lsa2, aoi4);  // calls to this function expect radians
        }

        static public double CalcRayOPD(this double y0, Lens lens_in, double refocus)
        {
            var dt = Trace_ToDataTable(new Vector3D(0, y0, 0), new Vector3D(0, 0, 1), lens_in, refocus);
            double opd = 0;
            for (int i = 1; i < 4; i++)
            {
                var Pa = dt.Rows[i - 1].Field<Vector3D>("P");
                var Pb = dt.Rows[i].Field<Vector3D>("P");
                var n = dt.Rows[i].Field<double>("Ninc");
                opd += (Pb - Pa).Length() * n;
            }
            return opd;
        }

        // **********
        static public DataTable GenerateRayTable(this double y0, Lens lensp, double Refocus)
        {
            DataTable dt = new DataTable("Ray Path");
            dt.Columns.Add("Surf", typeof(int));
            dt.Columns.Add("Y (mm)", typeof(double));
            dt.Columns.Add("Z (mm)", typeof(double));

            dt.Columns.Add("AOI (degree)", typeof(double));
            dt.Columns.Add("AOR (degree)", typeof(double));
            dt.Columns.Add("Index", typeof(double));
            dt.Columns.Add("OPD (mm)", typeof(double));

            var rdata = Trace_ToDataTable(new Vector3D(0, y0, 0), new Vector3D(0, 0, 1), lensp, Refocus);

            var opd = 0.0;
            for (int i = 0; i < rdata.Rows.Count; i++)
            {
                var y = rdata.Rows[i].Field<Vector3D>("P").Y;
                var z = rdata.Rows[i].Field<Vector3D>("P").Z;
                var aoi = rdata.Rows[i].Field<double>("AOI");
                var aor = rdata.Rows[i].Field<double>("AOR");
                var n = rdata.Rows[i].Field<double>("Ninc");
                if (i > 0)
                {
                    Vector3D pa = rdata.Rows[i - 1].Field<Vector3D>("P");
                    Vector3D pb = rdata.Rows[i].Field<Vector3D>("P");
                    opd += (pb - pa).Length() * n;
                }
                dt.Rows.Add(i, y, z, aoi, aor, n, opd);
            }

            return dt;
        }

        // ***********
        static public MicroGauss TraceMicroGauss(Lens lens, double Refocus, double xpt, double ypt, double w0)
        {
            List<Vector3D> Vlist = new List<Vector3D>();

            // Generate Base Ray
            //
            // far field beam divergence 
            //
            // Ɵ = λ / (π w0)
            //
            double theta = (lens.WL / 1000) / (Math.PI * 0.5);

            double w0_inc = w0;
            double xbase = xpt, ybase = ypt, zbase = 0;

            var Vin = new Vector3D(xbase, ybase, zbase);
            var Cin = new Vector3D(0, 0, 1);

            var (V0, C0) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // +Y 
            Vin.Y = ybase + w0_inc;
            var (V1a, C1a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // +Y Theta
            Vin.Y = ybase;
            Cin.Y = Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V1b, C1b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // *********************************

            // -Y Theta
            Vin.Y = ybase;
            Cin.Y = -Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V2b, C2b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // -Y
            Vin.Y = ybase - w0_inc;
            Cin.Y = 0;
            Cin.Z = 1;
            var (V2a, C2a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // *********************************

            Vin.Y = ybase;

            // +X 
            Vin.X = xbase + w0_inc;
            var (V3a, C3a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);


            // +X Theta
            Vin.X = xbase;
            Cin.X = Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V3b, C3b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);

            // *********************************

            // -X Theta
            Cin.X = -Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V4b, C4b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);


            // -X
            Vin.X = xbase - w0_inc;
            Cin.X = 0;
            Cin.Z = 1;
            var (V4a, C4a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);


            // *********************************


            var Ɵya = Math.Abs(C0.Y - C1a.Y);
            var Ɵyb = Math.Abs(C0.Y - C2a.Y);
            var Ɵxa = Math.Abs(C0.X - C3a.X);
            var Ɵxb = Math.Abs(C0.X - C4a.X);

            //
            // w0 = λ / (π Ɵ)  - in this case average Ɵ
            //

            //var w0x = (lens.WL / 1000) / (Math.PI * (Ɵxa + Ɵxb) / 2);
            //var w0y = (lens.WL / 1000) / (Math.PI * (Ɵya + Ɵyb) / 2);
            var w0y = Math.Sqrt(V1a.Y * V1a.Y + V1b.Y * V1b.Y);
            var w0x = Math.Sqrt(V3a.Y * V3a.Y + V3b.Y * V3b.Y);
            //var w0y = (V1a.Y * V1a.Y + V1b.Y * V1b.Y);
            //var w0x = (V3a.Y * V3a.Y + V3b.Y * V3b.Y);

            var div = Math.Sqrt(C0.X * C0.X + C0.Y * C0.Y);
            var z = 0.0;
            if (div > 1e-6)
                z = -2 * Math.Sqrt(V0.X * V0.X + V0.Y * V0.Y) / div;

            return new MicroGauss(-V0.X, -V0.Y, Refocus + z, C0, w0x*w0x, w0y*w0y);
        }



        // Tracing support
        public static (Vector3D, double, double) CalcDirectionSines(Vector3D E, Vector3D N, double nin, double nout)
        {
            var alpha = Vector3D.DotProduct(E, N);
            //var aoi = Math.Acos(alpha).RadToDeg();
            //var aor = Math.Asin(Math.Sin(Math.Acos(alpha)) * nin / nout).RadToDeg();

            double a = 1.0;
            double b = 2 * alpha;
            double c = (1 - (nout * nout) / (nin * nin));
            var sol2 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            var Ep = E + sol2 * N;
            Ep /= Ep.Length();
            //return (Ep, aoi, aor);
            return (Ep, 0, 0);
        }

        public static Vector3D calc_dir_sines(Vector3D E, Vector3D N, double nin, double nout)
        {
            var alpha = Vector3D.DotProduct(E, N);
            //var aoi = Math.Acos(alpha).RadToDeg();
            //var aor = Math.Asin(Math.Sin(Math.Acos(alpha)) * nin / nout).RadToDeg();

            double a = 1.0;
            double b = 2 * alpha;
            double c = (1 - (nout * nout) / (nin * nin));
            var sol2 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            var Ep = E + sol2 * N;
            Ep /= Ep.Length();
            return Ep;
        }

        public static Vector3D TraceRayToSurface(Vector3D D, Vector3D E, Side side, double plane = 0.0)
        {
            if (side.Type == 0)
            {
                return TranslateZ_Flat(D, E, plane);
            }

            double zest1 = (new PointD(D.X, D.Y)).Sag2D(side) + plane;
            double u = (zest1 - D.Z) / E.Z;
            var P1 = D;
            var P2 = D + u * E;

            for (int i = 0; i < 10; i++)
            {
                if ((P1 - P2).Length() > 1e-4)
                {
                    P1 = P2;
                    zest1 = (new PointD(P1.X, P1.Y)).Sag2D(side) + plane;
                    u = (zest1 - D.Z) / E.Z;
                    P2 = D + u * E;
                }
                else
                    break;
            }

            return P2;
        }

        private static Vector3D TranslateZ_Sphere(Vector3D D, Vector3D E, Side s)
        {
            Vector3D C = new Vector3D(0, 0, s.R);

            var a = E.LengthSquared();
            var b = Vector3D.DotProduct((2 * E), (D - C));
            var c = (D - C).LengthSquared() - s.R * s.R;

            var temp = b * b - 4 * a * c;

            var sol1 = (-b - Math.Sqrt(temp)) / (2 * a);
            //var sol2 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

            Vector3D Dp = D + sol1 * E;
            return Dp;
        }

        public static Vector3D TranslateZ_Flat(Vector3D P, Vector3D E, double zplane)
        {
            var u = (zplane - P.Z) / E.Z;
            Vector3D Pp = P + u * E;
            return Pp;
        }


        /*
         P = P1 + ((zplane - P1.Z) / E1.Z) * E1 = P1 + (zplane/E1.Z - P1.Z/E1.Z)  * E1 = P1 + (zplane * E1)/E1.Z - (P1.Z * E1)/E1.Z


        P1 + (zplane * E1)/E1.Z - (P1.Z * E1)/E1.Z = P2 + (zplane * E2)/E2.Z - (P2.Z * E2)/E2.Z

        (zplane * E1)/E1.Z - (zplane * E2)/E2.Z = (P2 - P1) - (P2.Z * E2)/E2.Z + (P1.Z * E1)/E1.Z
        zplane * (E1/E1.Z - E2/E2.Z) = (P2 - P1) - (P2.Z * E2)/E2.Z + (P1.Z * E1)/E1.Z

        zplane = (P2 - P1 - (P2.Z/E2.Z)*E2 + (P1.Z/E1.Z)*E1) / (E1/E1.Z - E2/E2.Z)

         P = P2 + ((zplane - P2.Z) / E2.Z) * E2 


         */
    }
}