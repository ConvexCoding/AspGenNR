using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using gClass;
using gExtensions;
using gFFTExtensions;
using System.Numerics;
using System.Linq;

namespace AspGen
{
    class Misc
    {
        static public List<double> GenerateRayList(int type)
        {
            List<double> rays;
            switch (type)
            {
                case 1:
                    rays = new List<double> { 0, 0.15, 0.30, 0.45, 0.60, 0.7, 0.8, 0.875, 0.925, 0.975, 1.0 };
                    break;
                case 2:
                    rays = new List<double> { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
                    break;
                default:
                    rays = new List<double> { 0, 0.2, 0.4, 0.6, 0.8, 1.0 };
                    break;
            }
            return rays;
        }

        static public double CalcEFL(double r1, double r2, double n, double ct)
        {
            double C1 = 0, C2 = 0;

            if (Math.Abs(r1) > 0.1)
                C1 = 1 / r1;
            if (Math.Abs(r2) > 0.1)
                C2 = 1 / r2;
            double Phi = (n - 1) * (C1 - C2 + (n - 1) * ct * C1 * C2 / n);
            double EFL = 1 / Phi;
            return EFL;
        }


        static public (double R1, double R2, double EFLp, StringBuilder results) 
                        DesignPosMenLens(double n, double ct, double targetEFL)
        {
            #region BestFitMath
            /*  
             *  outline of math used to create a best form lens solution
            //a = -2 * (n * n - 1) / (n + 2);
            //
            // r1 = ratio * r2
            //r2 = r1 / ratio
            //
            // R2 = R1 * (a + 1) / (a - 1)
            //
            //qp = (a - 1) / (a  + 1)
            //q = 1 / qp = (a + 1)/ (a - 1)
            //
            // R2 = qp * R1
            // C2 = q * C1
            //
            // cf = (n - 1) * (C1 - C2 + (n - 1) * ct * C1 * C2 / n)
            //
            // cf / (n - 1) = C1 - C1 * q + (n - 1) * ct * C1 * C1 * q / n
            //
            // cf / (n - 1) = C1 (1 - q)  + ((n - 1) * ct * q / n) *  C1 * C1
            //
            // ((n - 1) * ct * q / n) *  C1 * C1 + C1 (1 - q) - cf / (n - 1)
            //
            // A = ((n - 1) * ct * q / n)
            // B = (1 - q)
            // C = -cf / (n - 1)
            //
            // C1 = (-B +/- sqrt(B^2 - 4AC))/2A
            //
            */
            #endregion

            StringBuilder sb = new StringBuilder();
            string rfmt = "f2";
            double a = -2 * (n * n - 1) / (n + 2);
            double q = (a + 1) / (a - 1);
            double C = -1 / (targetEFL * (n - 1));
            double B = 1 - q;
            double A = ((n - 1) * ct * q / n);

            double POS = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
            double NEG = (-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A);

            double R1 = double.Parse((1 / POS).ToString(rfmt));
            double R2 = double.Parse(CalcR2(R1, n, ct, targetEFL).ToString(rfmt)); ;
            //double R2 = double.Parse((R1 / q).ToString(rfmt));

            double EFLp = Misc.CalcEFL(R1, R2, n, ct);

            sb.AppendLine("************************************************");
            sb.AppendLine("*******Positive Meniscus Design Parameters******");
            sb.AppendLine("a:  " + a.ToString("f6"));
            sb.AppendLine("q:  " + q.ToString("f6"));
            sb.AppendLine(" ");
            sb.AppendLine("A:  " + A.ToString("f6"));
            sb.AppendLine("B:  " + B.ToString("f6"));
            sb.AppendLine("C:  " + C.ToString("f6"));
            sb.AppendLine("\n");
            sb.AppendLine("POS:  " + POS.ToString("f6"));
            sb.AppendLine("NEG:  " + NEG.ToString("f6"));
            sb.AppendLine("\n");
            sb.AppendLine("R1:  " + R1.ToString("f6"));
            sb.AppendLine("R2:  " + R2.ToString("f6"));
            sb.AppendLine("\n");
            sb.AppendLine("EFL:  " + EFLp.ToString("f6"));
            sb.AppendLine("************************************************");

            return (R1, R2, EFLp, sb);
        }

        static public double CalcR2(double r1, double n, double ct, double efl)
        {

            double C1 = 0, C2 = 0, Cf = 0; ;

            if (Math.Abs(r1) > 0.1)
                C1 = 1 / r1;
            if (Math.Abs(efl) > 0.1)
                Cf = 1 / efl;

            // Cf = (n - 1) * (C1 - C2 + (n - 1) * ct * C1 * C2 / n);
            // CF / (n -1) = C1 - C2 + (n - 1) * ct * C1 * C2 / n;
            // CF / (n -1) = C1  - (1 - (n - 1) * ct * C1 / n) * C2;
            // (C1 - CF / (n -1)) /  (1 - (n - 1) * ct * C1 / n) = C2

            C2 = (C1 - Cf / (n - 1)) / (1 - (n - 1) * ct * C1 / n);
            return (1 / C2);
        }

        static public string GenerateLensType(double R1, double R2)
        {
            string type1 = "Po";
            if (R1 > 0.01)
                type1 = "CX";
            if (R1 < -0.01)
                type1 = "CC";

            string type2 = "Po";
            if (R2 > 0.01)
                type2 = "CC";
            if (R2 < -0.01)
                type2 = "CX";

            if ((type1 == "CC") && (type2 == "CC"))
                return "Bi-CC";

            if ((type1 == "CX") && (type2 == "CX"))
                return "Bi-CX";

            return type1 + "/" + type2;
        }

        static public double GenMaxPSF(Lens lens, double refocus, int gridsize, int totalgrid )
        {
            int s = gridsize;
            int totalsize = totalgrid;

            // generate amplitude mask &  generate wavefront map
            var datamask = GenAmplitudeMask(lens, gridsize);
            var datawfe = GenWFEMap(lens, refocus, gridsize);

            //Clipboard.SetText(datamask.ConvertDoublesToStringBuilder().ToString());
            var rows = datawfe.GetLength(0);
            var cols = datawfe.GetLength(1);

            Complex[,] data = new Complex[datawfe.GetLength(0), datawfe.GetLength(1)];
            Complex[,] datadl = new Complex[datawfe.GetLength(0), datawfe.GetLength(1)];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    data[r, c] = datamask[r, c] * Complex.Exp(new Complex(0, datawfe[r, c]));
                    datadl[r, c] = datamask[r, c];
                }

            var datapad = data.PadComplex(totalsize);
            var datapaddl = datadl.PadComplex(totalsize);

            AForge.Math.FourierTransform.FFT2(datapad, AForge.Math.FourierTransform.Direction.Forward);
            AForge.Math.FourierTransform.FFT2(datapaddl, AForge.Math.FourierTransform.Direction.Forward);

            // do circular shift on matrix
            var datashift = datapad.FFT2Shift();
            var datapaddlshift = datapaddl.FFT2Shift();

            // multiple matrix times it's conjugate
            var psf = datashift.MultiConjugate();
            var psfdl = datapaddlshift.MultiConjugate();

            // remove real portion from complex matrix to get intensity data
            var psfreal = psf.Real();
            var psfrealdl = psfdl.Real();

            var psfline = psfreal.PSFLine().SliceMid(s * 2);
            var psflinedl = psfrealdl.PSFLine().SliceMid(s * 2);

            // normalize data to peak of diff limited psf
            var maxdl = psflinedl.Max();
            psfline = psfline.ScaleData(maxdl);

            return psfline.Max();
        }

        public static double[,] GenWFEMap(Lens lens, double refocus, int beamgridsize = 0)
        {
            int s;
            if (beamgridsize == 0)
                s = Properties.Settings.Default.PSFBeamGridSize;
            else
                s = beamgridsize;

            int center = s / 2;
            var main = new double[s, s];
            double hwidth = lens.ap;

            double scale = (double)(s / 2) / lens.ap;

            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                {
                    main[r, c] = 0;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                    {
                        var wfe = hypot.CalcRayWFE(lens, refocus);
                        main[r, c] = wfe.OPD * 2 * Math.PI;
                    }
                }
            return main;
        }

        public static double[,] GenAmplitudeMask(Lens lens, int beamgridsize = 0)
        {
            int s;
            if (beamgridsize == 0)
                s = Properties.Settings.Default.PSFBeamGridSize;
            else
                s = beamgridsize;

            int center = s / 2;
            var mask = new double[s, s];
            double scale = (double)(s / 2) / lens.ap;

            for (int r = 0; r < s; r++)
                for (int c = 0; c < s; c++)
                {
                    mask[r, c] = double.NaN;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                        mask[r, c] = 1.0;
                    else
                        mask[r, c] = 0.0;
                }
            return mask;
        }

    }

}
