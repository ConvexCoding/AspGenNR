using System;

using System.Data;
using System.Drawing;
using System.Collections.Generic;

namespace AspGen
{
    // Fermi Dirac Methods here
    public static class FDF
    {
        public static Func<double, double, double, double, double> FermiDirac = (xp, beta, r50p, peak) => (peak / (1 + Math.Exp(beta * (Math.Abs(xp) / r50p - 1.0))));

        static public double[] FittoFermiDirac(double[] xs, double[] ys, double betaest = 20, double radiusest = 0.1, double peakest = 1.0)
        {
            double err, delta;
            var b = betaest;
            var r = radiusest;
            var p = peakest;
            var startb = b;

            for (int k = 0; k < 15; k++)
            {
                for (int j = 0; j < 15; j++)
                {
                    double db = 1;
                    double dr = 0.01;
                    double dp = 1;
                    double mintweakstep = 1e-5;
                    for (int i = 0; i < 15; i++)
                    {
                        (delta, err) = FindDirectionBeta(xs, ys, b, r, p, db);
                        b += delta;
                        if (delta < mintweakstep)
                            db /= 10.0;

                        (delta, err) = FindDirectionRadius(xs, ys, b, r, p, dr);
                        r += delta;
                        if (delta < mintweakstep)
                            dr /= 10.0;

                        (delta, err) = FindDirectionPeak(xs, ys, b, r, p, dp);
                        p += delta;
                        if (delta < mintweakstep)
                            dp /= 10.0;

                    }
                }
            }

            double[] yfermi = new double[ys.Length];
            for (int i = 0; i < ys.Length; i++)
                yfermi[i] = FermiDirac(xs[i], b, r, p);

            return yfermi;
        }

        static public (double, double, double) CalcFermiDiracCoef(double[] xs, double[] ys, double betaest = 20, double radiusest = 0.1, double peakest = 1.0)
        {
            double err, delta;
            var b = betaest;
            var r = radiusest;
            var p = peakest;
            var startb = b;

            for (int k = 0; k < 15; k++)
            {
                for (int j = 0; j < 15; j++)
                {
                    double db = 1;
                    double dr = 0.01;
                    double dp = 1;
                    double mintweakstep = 1e-5;
                    for (int i = 0; i < 15; i++)
                    {
                        (delta, err) = FindDirectionBeta(xs, ys, b, r, p, db);
                        b += delta;
                        if (delta < mintweakstep)
                            db /= 10.0;

                        (delta, err) = FindDirectionRadius(xs, ys, b, r, p, dr);
                        r += delta;
                        if (delta < mintweakstep)
                            dr /= 10.0;

                        (delta, err) = FindDirectionPeak(xs, ys, b, r, p, dp);
                        p += delta;
                        if (delta < mintweakstep)
                            dp /= 10.0;

                    }
                }
            }

            return (b, r, p);
        }

        private static double[] GenFermiDirac(double beta, double r50p, double peak, double[] xs)
        {
            double[] ys = new double[xs.Length];
            for (int i = 0; i < xs.Length; i++)
            {
                //ys[i] = peak / (1 + Math.Exp(beta * (Math.Abs(xs[i]) / r50p - 1.0)));
                ys[i] = FermiDirac(xs[i], beta, r50p, peak);
            }
            return ys;

        }

        private static (double delta, double err) FindDirectionBeta(double[] xs, double[] y0, double b, double r, double p, double db)
        {
            var betabase = b;
            var center = CalcErrorFunc(y0, GenFermiDirac(betabase, r, p, xs));

            betabase = b + db;
            var right = CalcErrorFunc(y0, GenFermiDirac(betabase, r, p, xs));

            betabase = b - db;
            var left = CalcErrorFunc(y0, GenFermiDirac(betabase, r, p, xs));

            if (left < center)
                return (-db, left);
            if (right < center)
                return (db, right);
            return (0.0, center);

        }

        private static (double delta, double err) FindDirectionRadius(double[] xs, double[] y0, double b, double r, double p, double dr)
        {
            var radiusbase = r;
            var center = CalcErrorFunc(y0, GenFermiDirac(b, radiusbase, p, xs));

            radiusbase = r + dr;
            var right = CalcErrorFunc(y0, GenFermiDirac(b, radiusbase, p, xs));

            radiusbase = r - dr;
            var left = CalcErrorFunc(y0, GenFermiDirac(b, radiusbase, p, xs));

            if (left < center)
                return (-dr, left);
            if (right < center)
                return (dr, right);
            return (0.0, center);
        }

        private static (double delta, double err) FindDirectionPeak(double[] xs, double[] y0, double b, double r, double p, double dp)
        {
            var peakbase = p;
            var center = CalcErrorFunc(y0, GenFermiDirac(b, r, peakbase, xs));

            peakbase = p + dp;
            var right = CalcErrorFunc(y0, GenFermiDirac(b, r, peakbase, xs));

            peakbase = p - dp;
            var left = CalcErrorFunc(y0, GenFermiDirac(b, r, peakbase, xs));

            if (left < center)
                return (-dp, left);
            if (right < center)
                return (dp, right);
            return (0.0, center);
        }

        private static double CalcErrorFunc(double[] y0, double[] y1)
        {
            double sum = 0;
            for (int i = 0; i < y0.Length; i++)
                sum += Math.Pow(y1[i] - y0[i], 2);
            return Math.Sqrt(sum);
        }
    }
}