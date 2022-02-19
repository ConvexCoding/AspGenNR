using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;


namespace gFFTExtensions
{
    public static class gFFTExtensions
    {
        public static double[,] Real(this Complex[,] q)
        {
            var rows = q.GetLength(0);
            var cols = q.GetLength(1);
            var t = new double[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    t[r, c] = q[r, c].Real;

            return t;
        }

        public static Complex[,] PadComplex(this Complex[,] q, int size)
        {
            var t = GenerateZeroArray(size);
            var rows = q.GetLength(0);
            var cols = q.GetLength(1);

            int start = (size - rows) / 2;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    t[start + r, start + c] = q[r, c];

            return t;
        }

        public static Complex[,] GenerateZeroArray(this int size)
        {
            Complex[,] t = new Complex[size, size];
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    t[r, c] = new Complex(0, 0);

            return t;
        }

        public static Complex[,] MultiConjugate(this Complex[,] q)
        {
            var rows = q.GetLength(0);
            var cols = q.GetLength(1);

            Complex[,] t = new Complex[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < rows; c++)
                    t[r, c] = q[r, c] * Complex.Conjugate(q[r, c]);

            return t;
        }

        public static Complex[,] FFT2Shift(this Complex[,] q)
        {
            var rows = q.GetLength(0);
            var cols = q.GetLength(1);

            // shift columns
            Complex[,] s = new Complex[rows, cols];
            int mid = cols / 2;
            for (int c = mid; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    s[r, c - mid] = q[r, c];
                    s[r, c] = q[r, c - mid];
                }
            }

            Complex[,] t = new Complex[rows, cols];
            mid = rows / 2;
            for (int r = mid; r < rows; r++)
            {
                for (int c = 0; c < rows; c++)
                {
                    t[r - mid, c] = s[r, c];
                    t[r, c] = s[r - mid, c];
                }
            }

            return t;
        }

        public static double[] PSFLine(this double[,] q)
        {
            return Enumerable.Range(0, q.GetLength(0)).Select(x => q[x, q.GetLength(1) / 2]).ToArray();
        }

        public static double[] SliceMid(this double[] q, int slicelength)
        {
            int start = (int)((q.Count() - slicelength) / 2);
            return new ArraySegment<double>(q, start, slicelength).ToArray();
        }

        public static int[,] CircShift(this int[,] q)
        {
            var rows = q.GetLength(0);
            var cols = q.GetLength(1);

            // shift columns
            int[,] s = new int[rows, cols];
            int mid = cols / 2;
            for (int c = mid; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    s[r, c - mid] = q[r, c];
                    s[r, c] = q[r, c - mid];
                }
            }

            int[,] t = new int[rows, cols];
            mid = rows / 2;
            for (int r = mid; r < rows; r++)
            {
                for (int c = 0; c < rows; c++)
                {
                    t[r - mid, c] = s[r, c];
                    t[r, c] = s[r - mid, c];
                }
            }

            return t;
        }
    }

}