using System;
using System.Windows.Forms;
using gClass;

namespace AspGen
{
    public partial class SetChartAxisNotify : Form
    {
        public bool isOk = false;
        public bool autoScaleOn = false;
        public double promptValue = 0;
        public ChartAxis ca;
        public ChartAxis CAOut;
        public int caller;
        public string title;

        public SetChartAxisNotify(int callerin, string titlein, ChartAxis cain)
        {
            InitializeComponent();
            caller = callerin;
            title = titlein;
            label4.Text = title;
            ca = cain;
            axisMin.Text = ca.AxisMin.ToString();
            axisMax.Text = ca.AxisMax.ToString();
            axisInterval.Text = ca.MajorInterval.ToString();
            axisMin.Focus();
        }

        private bool LoadChartParameters ( )
        {
            bool isValid = true;
            CAOut = new ChartAxis();
            try
            {
                CAOut.AxisMin = double.Parse(axisMin.Text);
            }
            catch
            {
                MessageBox.Show("Min Value: \"" + axisMin.Text + "\" is not a valid number!", "Error in Data Entry");
                isValid = false;
                axisMin.Focus();
            }

            try
            {
                CAOut.AxisMax = double.Parse(axisMax.Text);
            }
            catch
            {
                MessageBox.Show("Max Value: \"" + axisMax.Text + "\" is not a valid number!", "Error in Data Entry");
                isValid = false;
                axisMax.Focus();
            }

            try
            {
                CAOut.MajorInterval = double.Parse(axisInterval.Text);
            }
            catch
            {
                MessageBox.Show("Interval Value: \"" + axisInterval.Text + "\" is not a valid number!", "Error in Data Entry");
                isValid = false;
                axisInterval.Focus();
            }
            return isValid;
        }

        private void okButton_Click(object sender, EventArgs e)
        {

            isOk = true;
            if (LoadChartParameters())
                this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }

        private void autoScaleButton_Click(object sender, EventArgs e)
        {
            isOk = true;
            autoScaleOn = true;
            DoAutoScaleNotification();
            isOk = false;
            this.Close();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            DoNotification();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
                DoNotification();
        }

        private void DoNotification ( )
        {
            autoScaleOn = false;

            if (!LoadChartParameters())
                return;

            switch (caller)
            {
                case 0:
                    var pt0 = (TransverseSphericalPlot)this.Owner;
                    pt0.NotifyMe(CAOut);
                    break;

                case 1:
                    var pt1 = (WaveFrontErrorPlot)this.Owner;
                    pt1.NotifyMe(CAOut);
                    break;

                case 2:
                    var pt2 = (ExtendedSourcePlot)this.Owner;
                    pt2.NotifyMe(CAOut);
                    break;

                case 3:
                    var pt3 = (PSF_DiffractionBased)this.Owner;
                    pt3.NotifyMe(CAOut);
                    break;
            }            
        }

        private void DoAutoScaleNotification()
        {
            switch (caller)
            {
                case 0:
                    var pt0 = (TransverseSphericalPlot)this.Owner;
                    pt0.LocalAutoScaleChart(title);
                    break;

                case 1:
                    var pt1 = (WaveFrontErrorPlot)this.Owner;
                    pt1.LocalAutoScaleChart(title);
                    break;

                case 2:
                    var pt2 = (ExtendedSourcePlot)this.Owner;
                    pt2.LocalAutoScaleChart(title);
                    break;

                case 3:
                    var pt3 = (PSF_DiffractionBased)this.Owner;
                    pt3.LocalAutoScaleChart(title);
                    break;
            }
        }

    }
}
