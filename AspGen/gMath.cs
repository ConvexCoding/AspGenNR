using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using gClass;
using gExtensions;

using gGraphExt;
using System.IO;

namespace AspGen
{
    public partial class Form1
    {
        public double FindDirection2(Lens lensp, double deltaae, int whichvar)
        {
            double left, right, center, basevalue;
            unsafe
            {
                double* ptr;
                switch (whichvar)
                {
                    case 0:
                        ptr = &lensp.Side1.K;
                        break;
                    case 1:
                        ptr = &lensp.Side1.AD;
                        break;
                    case 2:
                        ptr = &lensp.Side1.AE;
                        break;
                    case 3:
                        ptr = &lensp.Side2.K;
                        break;
                    case 4:
                        ptr = &lensp.Side2.AD;
                        break;
                    case 5:
                        ptr = &lensp.Side2.AE;
                        break;
                    default:
                        ptr = null;
                        break;
                }

                basevalue = *ptr;
                center = CalcErrorFunc(lensp, rays);
                *ptr += deltaae;
                right = CalcErrorFunc(lensp, rays);
                *ptr -= 2 * deltaae;
                left = CalcErrorFunc(lensp, rays);
                *ptr = basevalue;
            }

            if (left < center)
                return -deltaae;
            if (right < center)
                return deltaae;
            return 0.0;
        }


        private double FindRadius1Direction(Lens lens, double tweak)
        {
            double left, right, center;
            double R1 = lens.Side1.R;
            double R2 = lens.Side2.R;
            double EFL = lens.EFL;

            center = CalcErrorFunc(lens, rays, false);

            lens.Side1.R = R1 + tweak;
            lens.Side2.R = Misc.CalcR2(lens.Side1.R, lens.Side2.R, lens.CT, EFL);
            right = CalcErrorFunc(lens, rays, false);

            lens.Side1.R = R1 - tweak;
            lens.Side2.R = Misc.CalcR2(lens.Side1.R, lens.Side2.R, lens.CT, EFL);
            left = CalcErrorFunc(lens, rays, false);

            lens.Side1.R = R1;
            lens.Side2.R = R2;
            lens.EFL = EFL;

            if (left < center)
                return -tweak;
            if (right < center)
                return tweak;
            return 0.0;
        }
    }
    public class gMath
    {
        static public double[,] GenerateOPDMap(Lens lens, double Refocus, int size = 100)
        {
            int center = size / 2;
            double[,] map = new double[size, size];

            double scale = (double)(size / 2) / lens.ap;
            double min = 1e20;
            double max = -1e20;

            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                {
                    map[r, c] = double.NaN;
                    double row = (double)(r - center) / scale;
                    double col = (double)(c - center) / scale;
                    double hypot = Math.Sqrt(row * row + col * col);
                    if (hypot <= lens.ap)
                    {
                        var wfe = hypot.CalcRayWFE(lens, Refocus);
                        map[r, c] = wfe.OPD;
                        if (wfe.OPD > max)
                            max = wfe.OPD;
                        if (wfe.OPD < min)
                            min = wfe.OPD;
                    }
                }
            return map;
        }

        static public double CalcRMSfromMap(double[,] map)
        {
            if (map == null)
                return 0;
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            int cts = 0;
            double total = 0;
            double ave = CalcAverforMap(map);
            for(int r = 0; r < rows; r++)
                for(int c = 0; c < cols; c++)
                {
                    if (!double.IsNaN(map[r,c]))
                    {
                        cts++;
                        total += (map[r, c] - ave) * (map[r, c] - ave);
                    }
                }
            return Math.Sqrt(total/(double)(cts-1));
        }

        static public double CalcAverforMap(double[,] map)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            int cts = 0;
            double total = 0;

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (!double.IsNaN(map[r, c]))
                    {
                        cts++;
                        total += map[r, c];
                    }
                }
            return (total / (double)cts);
        }

        static public double CalcWFEPV(Lens lens, double Refocus, int iterations = 10)
        {
            double dr = lens.ap / (double)iterations;
            List<double> wfe = new List<double>();

            for (double r = 0; r < lens.ap + 0.001; r += dr)
            {
                wfe.Add(r.CalcRayWFE(lens, Refocus).OPD);
            }
            return (wfe.Max() - wfe.Min());
        }

        static public double CalcWFERMS(Lens lens, double Refocus, int iterations = 10)
        {
            //double dr = lens.ap / (double)iterations;
            var map = GenerateOPDMap(lens, Refocus);
            var rms = CalcRMSfromMap(map);
            return rms;
        }

        static public double CalcTSAPV(Lens lens, double Refocus, int iterations = 10)
        {
            List<double> ylist = new List<double>();
            for(double y = -lens.ap; y < lens.ap + 0.001; y += lens.ap/iterations)
            {
                ylist.Add(y.CalcLSA(lens, Refocus).Y3);
            }
            return (ylist.Max() - ylist.Min());
        }

        static public double FindRMSRefocusDirection(Lens lens, double Refocus, double tweak)
        {
            double left, right, center;

            center = CalcWFERMS(lens, Refocus);
            right = CalcWFERMS(lens, Refocus + tweak);
            left = CalcWFERMS(lens, Refocus - tweak);

            if (left < center)
                return -tweak;
            if (right < center)
                return tweak;
            return 0.0;
        }

        static public double FindPVRefocusDirection(Lens lens, double Refocus, double tweak)
        {
            double left, right, center;

            center = CalcWFEPV(lens, Refocus);
            right = CalcWFEPV(lens, Refocus + tweak);
            left = CalcWFEPV(lens, Refocus - tweak);

            if (left < center)
                return -tweak;
            if (right < center)
                return tweak;
            return 0.0;
        }

        static public double FindPSFDirection(Lens lens, double Refocus, double tweak)
        {
            double left, right, center;

            center = Misc.GenMaxPSF(lens, Refocus, 64, 128);
            right = Misc.GenMaxPSF(lens, Refocus + tweak, 64, 128);
            left = Misc.GenMaxPSF(lens, Refocus - tweak, 64, 128);

            if (left > center)
                return -tweak;
            if (right > center)
                return tweak;
            return 0.0;
        }


        static public double FindTSARefocusDirection(Lens lens, double Refocus, double tweak)
        {
            double left, right, center;

            center = CalcTSAPV(lens, Refocus);
            right = CalcTSAPV(lens, Refocus + tweak);
            left = CalcTSAPV(lens, Refocus - tweak);

            if (left < center)
                return -tweak;
            if (right < center)
                return tweak;
            return 0.0;
        }

        static public double[] GenArray(double xbegin, double xinc, int steps)
        {
            double[] xs = new double[steps];
            for (int w = 0; w < steps; w++)
                xs[w] = xbegin + xinc * w;

            return xs;
        }

    }
}