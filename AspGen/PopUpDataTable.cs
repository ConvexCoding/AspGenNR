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
    public partial class PopUpDataTable : Form
    {
        public PopUpDataTable(DataTable dt, string title = "Ray Data Table - main")
        {
            InitializeComponent();
            dgv.DataSource = dt;
            adjustDGVFormat(dgv);
            this.Text = title;
        }

        private void adjustDGVFormat(DataGridView dgv)
        {
            dgv.Columns[1].DefaultCellStyle.Format = "0.###0";

            dgv.Columns[2].DefaultCellStyle.Format = "0.###0";
            dgv.Columns[3].DefaultCellStyle.Format = "0.###0";
            dgv.Columns[4].DefaultCellStyle.Format = "0.###0";

            dgv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns[6].DefaultCellStyle.Format = "0.###0";
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.RowHeadersVisible = false;
            this.Width = dgv.Width - 10;
            this.Height = dgv.Height + 20;
        }

        private void copyTableButton_Click(object sender, EventArgs e)
        {
            var b = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(b, new Rectangle(0, 0, panel1.Width, panel1.Height));
            Clipboard.SetImage(b);
            MessageBox.Show("Data Table copied to Clipboard as Bitmap!");
        }

        private void copyFormButton_Click(object sender, EventArgs e)
        {
            var tb = QueryFormDimensions.GetWindowRectangle(this.Handle);
            using (Bitmap bitmap = new Bitmap(tb.RSize.Width, tb.RSize.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(tb.P0, Point.Empty, tb.RSize);

                }
                Clipboard.SetImage(bitmap);
            }
            MessageBox.Show("Form copied to Clipboard!");
        }
    }
}
