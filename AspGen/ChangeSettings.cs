using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Drawing;

namespace AspGen
{
    public partial class ChangeSettings : Form
    {
        List<BackUpSetting> StartValues = new List<BackUpSetting>();

        List<WithIn> withList = new List<WithIn>();
        Dictionary<string, WithIn> skeys = new Dictionary<string, WithIn>();

        public ChangeSettings()
        {
            InitializeComponent();
            LoadSettingsIntoList();
            InitializeDataGridView();
            //Properties.Settings.Default.SettingChanging += new SettingChangingEventHandler(Validate_SettingChanged);

            int height = 0;
            height += dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < dataGridView1.RowCount; i++)
                if (dataGridView1.Rows[i].Visible)
                    height += dataGridView1.Rows[i].Height;

            int sh = splitContainer1.Size.Height - splitContainer1.SplitterDistance;
            this.Height = height + sh + 150;
            InitializeSettingsRangeData();
        }

        #region Inits methods
        public void InitializeSettingsRangeData ( )
        {
            withList.Add(new WithIn("DrawLensDrawCL", true, false, false));
            withList.Add(new WithIn("DrawLensPreRayDistance", 0.0, 1000.0, 10.0));
            withList.Add(new WithIn("NumberOfDrawLensRays", 1, 101, 10));
            withList.Add(new WithIn("InputRaySet", 1, 3, 1));
            withList.Add(new WithIn("ExtSrcNoofBaseRays", 1000, 5000000, 100000));
            withList.Add(new WithIn("ExtSrcNoofAngles", 1, 10, 3));
            withList.Add(new WithIn("ExtScrBinSize", 0.00025, 1.0, 0.001));
            withList.Add(new WithIn("NoOfDigitsSpotDiagLabels", 0, 9, 3));
            withList.Add(new WithIn("WFE2DMapSize", 50, 1000, 200));
            withList.Add(new WithIn("PSFTotalGridSize", 128, 2048, 1024));
            withList.Add(new WithIn("PSFBeamGridSize", 32, 512, 100));
            withList.Add(new WithIn("SpotDiagramSpokesInc", 1, 180, 2));
            withList.Add(new WithIn("SpotDiagramRadialSamples", 1, 200, 50));
            withList.Add(new WithIn("ExtScrVerticalAverage1D", 0, 5, 0));
            withList.Add(new WithIn("TransversDataPoints", 3, 101, 21));

            foreach (WithIn w in withList)
            {
                skeys.Add(w.SettingStr, w);
            }

            // quick check to make sure that no new user settings are added without also 
            // adding a WithIn range element
            foreach(BackUpSetting bus in StartValues)
            {
                if (!withList.Exists(x => x.SettingStr == bus.Name))
                {
                    MessageBox.Show("No \"WithIn\" range element found for settings \"" + bus.Name + "\"\n", "Error in Range Dictionary");
                }
            }            
        }

        public void InitializeDataGridView ( )
        {
            dataGridView1.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            var valuetypes = Properties.Settings.Default.PropertyValues;
            foreach (SettingsPropertyValue v in valuetypes)
            {
                dataGridView1.Rows.Add(v.Name, v.Property.PropertyType, v.PropertyValue);
            }
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Ascending);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
        }

        private void LoadSettingsIntoList()
        {
            var valuetypes = Properties.Settings.Default.PropertyValues;
            foreach (SettingsPropertyValue v in valuetypes)
            {
                if (v.Name != null)
                    StartValues.Add(new BackUpSetting(v.Name.ToString(),
                                                      v.Property.PropertyType.FullName.ToString(),
                                                      v.PropertyValue.ToString()));
            }
        }

        #endregion

        #region Main Menu Buttons

        private void saveSettings_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        
        private void ResetValuesToOriginals( )
        {
            foreach(BackUpSetting b in StartValues)
            {
                Type type = Type.GetType(b.DataType);
                TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
                Properties.Settings.Default[b.Name] = typeConverter.ConvertFromString(b.Value);
            }
            dataGridView1.Rows.Clear();
            InitializeDataGridView();
        }

        private void resetValues_Click(object sender, EventArgs e)
        {
            ResetValuesToOriginals();
        }

        private void quickScan_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            InitializeDataGridView();
            this.Refresh();
        }

        private void longScanSettings_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            HelpOnSettings help = new HelpOnSettings();
            help.Show();
        }

        private void exitNoSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 2) && (e.RowIndex >= 0) && (e.RowIndex < dataGridView1.Rows.Count - 1))
            {
                string name = dataGridView1[0, e.RowIndex].Value.ToString();
                var datatype = (Type)dataGridView1[1, e.RowIndex].Value;
                var value = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                WithIn ckey = skeys[name];
                if (ckey.IsWithIn(value))
                    Properties.Settings.Default[name] = ckey.Value;
                else
                {
                    MessageBox.Show("Improper Settings Format for " + name + "!\n" +
                                    "Enter a type " + ckey.SType.ToString() + " value" + "\n" +
                                    "between " + ckey.Lower + " and " + ckey.Upper + "\n" +
                                    "***Resetting value to default.***",
                                    "Error in dataGridView1_CellValueChanged!");
                    dataGridView1[2, e.RowIndex].Value = ckey.Value;
                    Properties.Settings.Default[name] = ckey.Value;
                }
            }
        }

        // This function is used to copy the Settings Name and types to the clipboard. 
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                StringBuilder sb = new StringBuilder();
                List<SettingsPropertyValue> splist = new List<SettingsPropertyValue>();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Value", typeof(string));
                foreach (SettingsPropertyValue v in Properties.Settings.Default.PropertyValues)
                {

                    string t = v.Property.PropertyType.ToString().Substring(7);
                    //sb.AppendLine(v.Name + "(" + t + ")\t");
                    sb.AppendLine("Properties.Settings.Default." + v.Name + " = " + v.PropertyValue.ToString() + ";");
                    dt.Rows.Add(v.Name, t, v.PropertyValue.ToString());
                }
                sb.AppendLine("\n\nSorted Shorthand\n");
                DataView dv = dt.DefaultView;
                dv.Sort = "Name";
                DataTable sortedDT = dv.ToTable();

                foreach (DataRow r in sortedDT.Rows)
                {
                    sb.AppendLine(r[0].ToString() + ", " + r[1].ToString() + ", " + r[2].ToString());
                }
                Clipboard.Clear();
                Clipboard.SetText(sb.ToString());

                MessageBox.Show("User Settings copied to clipboard!", "Settings copy to clip");
            }
        }

        // this was initially a double check on setting validation, later I built this check into
        // the datagridview cell validation since both datagridview and Settings needs validated.
        // note that installation of this event is commented out in the main method
        private void Validate_SettingChanged(Object o, SettingChangingEventArgs e)
        {
            if (!skeys.Keys.Contains(e.SettingName))  // check to make sure that name is in dictionary
                return;

            WithIn ckey = skeys[e.SettingName];
            var value = e.NewValue;

            if (!ckey.IsWithIn(value))
            {
                MessageBox.Show("Improper Settings Format for " + e.SettingName + "!\n" +
                                "Enter a type " + ckey.SType.ToString() + " value" + "\n" +
                                "between " + ckey.Lower + " and " + ckey.Upper + "\n" +
                                "***Resetting value to default.***",
                                "Error in dataGridView1_CellValueChanged!");

            }
            Properties.Settings.Default[e.SettingName] = ckey.Value;
            e.Cancel = true;
        }

        private void saveFormToClip_Click(object sender, EventArgs e)
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
        }
    }

}
