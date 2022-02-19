using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AspGen
{
    public partial class HelpOnSettings : Form
    {
        public HelpOnSettings()
        {
            InitializeComponent();

            Assembly _assembly;
            StreamReader reader;
            try
            {
                _assembly = Assembly.GetExecutingAssembly();
                //var resourceNames = _assembly.GetManifestResourceNames();
                var helpfilename = "AspGen.Resources.SettingsHelp.txt";
                reader = new StreamReader(_assembly.GetManifestResourceStream(helpfilename));
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] ss = line.Split('\t');
                    if (ss.Count() == 2)
                    {
                        dgv.Rows.Add(ss[0], ss[1]);
                    }
                    else
                    {
                        dgv.Rows.Add(" ", " ");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error accessing resources!");
            }
        }
    }
}
