using System;
using System.Windows.Forms;
using gClass;

namespace AspGen
{
    public partial class SetChartAxis : Form
    {
        public bool isOk = false;
        public bool autoScaleOn = false;
        public double promptValue = 0;
        public ChartAxis ca;
        public ChartAxis CAOut;
        public SetChartAxis(string title, ChartAxis cain)
        {
            InitializeComponent();
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
            }

            try
            {
                CAOut.AxisMax = double.Parse(axisMax.Text);
            }
            catch
            {
                MessageBox.Show("Max Value: \"" + axisMax.Text + "\" is not a valid number!", "Error in Data Entry");
                isValid = false;
            }

            try
            {
                CAOut.MajorInterval = double.Parse(axisInterval.Text);
            }
            catch
            {
                MessageBox.Show("Interval Value: \"" + axisInterval.Text + "\" is not a valid number!", "Error in Data Entry");
                isValid = false;
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
            this.Close();
        }
    }
}
