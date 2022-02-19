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
    public partial class PopUpRayTable : Form
    {
        public PopUpRayTable(DataTable dt)
        {
            InitializeComponent();
            dgv.DataSource = dt;
            adjustDGVFormat(dgv);
        }

        private void adjustDGVFormat(DataGridView dgv)
        {
            //dt.Rows.Add(2, x2, y2, z2, Math.Atan(m2), aoi2, aor2, norm2);
            string fmtdl = "f10";
            dgv.Columns[1].DefaultCellStyle.Format = fmtdl;
            dgv.Columns[2].DefaultCellStyle.Format = fmtdl;
            dgv.Columns[3].DefaultCellStyle.Format = fmtdl;

            dgv.Columns[4].DefaultCellStyle.Format = fmtdl;
            dgv.Columns[5].DefaultCellStyle.Format = fmtdl;
            dgv.Columns[6].DefaultCellStyle.Format = fmtdl;
            dgv.Columns[7].DefaultCellStyle.Format = fmtdl;

            //dgv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
        }
    }
}
