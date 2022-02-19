using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using gClass;
using gExtensions;
using System.Numerics;

namespace AspGen
{
    public partial class DiffSpotSizeMap : Form
    {
        public DiffSpotSizeMap(List<MicroGauss> mglist, Lens lens)
        {
            InitializeComponent();

            List<double> xs = new List<double>();
            List<double> amps = new List<double>();
            double s = 0.025;
            double sinterval = s / 5;
            StringBuilder sb = new StringBuilder();

            double z = 0.0;
            double wl = lens.WL/ 1000;
            double  phase = 0.0;
            double k = 2 * Math.PI / wl;
            
            for (double x = -s; x <= s + 0.000001; x += 0.0002)
            {
                xs.Add(x);
                Vector3D axispt = new Vector3D(x, 0, 0);
                var amp = 0.0;
                foreach (MicroGauss mg in mglist)
                {
                    var r = mg.Loc.VDistance(new Vector3D(x, 0, 0));
                    var w0 = (mg.W0_X + mg.W0_Y) / 2;
                    var zr = Math.PI * w0 * w0 / wl;
                    phase = Math.Atan(mg.Loc.Z / zr);
                    var wz = w0 * Math.Sqrt(1 + z * z / (zr * zr));
                    if (Math.Abs(z) < 1e-20)
                        z = 1e-20;
                    var Rz = z * (1 + (zr * zr) / (z * z));
                    var itemp = new Complex(0, -(k * z + k * r * r / (2 * Rz) - phase));
                    Complex E = (w0 / wz) * Math.Exp(-(r * r) / (wz * wz)); // * Complex.Exp(itemp);
                    amp += E.Real;
                }
                amps.Add(amp);
            }
            
            /*
            for (double x = -s; x < s + 0.000001; x += 0.0002)
            {
                xs.Add(x);
                Vector3D axispt = new Vector3D(x, 0, 0);
                var amp = 0.0;
                foreach (MicroGauss mg in mglist)
                {
                    var r = mg.Loc.VDistance(new Vector3D(x, 0, 0));
                    var w = (mg.W0_X + mg.W0_Y) / 2;
                    amp += (2 / (Math.PI * w * w)) * Math.Exp(-2 * (r * r) / (w * w));
                }
                amps.Add(amp);
            }
            */
            var normamps = amps.NormalizeList();
            string fmt = "e8";
            for (int i = 0; i < amps.Count(); i++)
            {
                sb.AppendLine(xs[i].ToString(fmt) + ", " + amps[i].ToString(fmt) + ", " + normamps[i].ToString(fmt));
            }
            chart1.Series[0].Points.DataBindXY(xs, normamps);

            double q = 0.135335;
            double negx = 0, posx = 0;
            for (int i = 0; i < (int)(normamps.Count() / 2); i++)
                if ( (normamps[i] < q) && (normamps[i+1] >= q) )
                {
                    negx = xs[i + 1];
                    break;
                }
            for (int i = (int)(normamps.Count() / 2); i < normamps.Count()-2; i++)
                if ((normamps[i] > q) && (normamps[i + 1] <= q))
                {
                    posx = xs[i + 1];
                    break;
                }

            chart1.ChartAreas[0].AxisX.Minimum = -s;
            chart1.ChartAreas[0].AxisX.Maximum = s;
            chart1.ChartAreas[0].AxisX.Interval = sinterval;

            RectangleAnnotation annotation = new RectangleAnnotation();
            annotation.AnchorDataPoint = chart1.Series[0].Points[0];
            annotation.AnchorY = chart1.ChartAreas[0].AxisY.Maximum * 0.95;
            annotation.AnchorX = chart1.ChartAreas[0].AxisX.Minimum;
            annotation.Text = "Gauss 1/2 Dia:   " + ((posx - negx) / 2).ToString("f3") + " mm";
            annotation.ForeColor = Color.Black;
            annotation.Font = new Font("Arial", 8);
            annotation.LineWidth = 1;
            chart1.Annotations.Add(annotation);


            Clipboard.SetText(sb.ToString());
        }
    }
}
