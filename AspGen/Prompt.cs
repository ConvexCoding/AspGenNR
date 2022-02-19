using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AspGen
{
    public partial class Prompt : Form
    {
        public bool isOk = false;
        public double promptValue = 0;
        public Prompt(string formtitle, string datapromptstring, string Valuestring)
        {
            InitializeComponent();
            this.Text = formtitle;
            promptBoxLabel.Text = datapromptstring;
            valueTextBox.Text = Valuestring;
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            isOk = false;
            promptValue = 0;
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            isOk = true;
            try
            {
                promptValue = double.Parse(valueTextBox.Text);
            }
            catch
            {
                MessageBox.Show("\"" + valueTextBox.Text + "\" is not a valid number!", "Error in Data Entry");
                isOk = false;

            }
            this.Close();
        }
    }
}
