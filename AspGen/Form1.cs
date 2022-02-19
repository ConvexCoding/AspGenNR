using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using gClass;
using gExtensions;
using gGraphExt;

using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace AspGen
{
    public partial class Form1 : Form
    {
        public double Refocus;
        
        List<double> rays;

        public double RtolforZero = 0.001;
        bool Debug = true;

        Lens lens;

        bool autoSolve = false;
        string autoEFLstr;
        double autoEFL;
        double offaxisangle = 0.0;

        int thisWidth;
        List<TextBox> tblist;

        public Form1()
        {
            InitializeComponent();
            thisWidth = 750;

            DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
            string bdate = " (last build " + buildDate.ToShortDateString() + ")";
            string s = typeof(Form1).Assembly.GetName().Version.ToString();
            this.Text = this.Text + ", Vers. " + s + ", User: " + Environment.UserName + bdate;

            rays = Misc.GenerateRayList(Properties.Settings.Default.InputRaySet);


            MaterialIndex nm = new MaterialIndex();
            MatNames.DataSource = MaterialIndex.GetMatNameList();
            //MatNames.SelectedIndex = MatNames.Items.Count - 1;
            MatNames.SelectedIndex = MatNames.Items.IndexOf("Fused Silica");
            string mat = MatNames.SelectedItem.ToString();
            if (mat != "Custom")
            {
                double index = MaterialIndex.GetIndex(mat, lens.WL);
                IndexTextBox.Text = index.ToString("f5");

            }
            {
                IndexTextBox.Text = "1.5";
            }

            //AD1TextBox.Text = "-5.284E-07";
            //AE1TextBox.Text = "-1.184E-10";

            setuptest();

            //reload lens data in case material and index weren't set properly to begin program.

            if (LoadLensData())
            {
                lens = FirstOrderCalcs(lens);
            }
            else
                ErrTextBox.Text = "NA";

            Auto.BackColor = Color.Red;
            this.Width = 700;  // trigger form resizing to 

            splitContainer3.SplitterDistance = splitContainer3.Width / 2;
            splitContainer4.SplitterDistance = splitContainer4.Width / 2;

            tblist = new List<TextBox>();
            tblist.Add(R1TextBox);
            tblist.Add(K1TextBox);
            tblist.Add(AD1TextBox);
            tblist.Add(AE1TextBox);
            tblist.Add(R2TextBox);
            tblist.Add(K2TextBox);
            tblist.Add(AD2TextBox);
            tblist.Add(AE2TextBox);
            tblist.Add(CTTextBox);
            tblist.Add(ApTextBox);
            tblist.Add(WLTextBox);
            tblist.Add(Diameter);
            tblist.Add(IndexTextBox);
            tblist.Add(refocusTextBox);
        }

        private void setuptest()
        {
            MatNames.SelectedIndex = MatNames.Items.Count - 1;
            MatNames.SelectedIndex = 0;
            string mat = MatNames.SelectedItem.ToString();
            if (mat != "Custom")
            {
                double index = MaterialIndex.GetIndex(mat, lens.WL);
                IndexTextBox.Text = index.ToString("f5");
            }
            {
                IndexTextBox.Text = "1.5";
            }
            if (MatNames.SelectedIndex == MatNames.Items.Count - 1)
            {
                WLTextBox.Text = "10.6";
                R1TextBox.Text = "140.27";
                R2TextBox.Text = "0";
                //AD1TextBox.Text = "-4.7E-07";
            }
            if (MatNames.SelectedIndex == 0)
            {
                WLTextBox.Text = "1.07";
                IndexTextBox.Text = "1.44966";
                R1TextBox.Text = "44.966";
                R2TextBox.Text = "-1000";
                //AD1TextBox.Text = "-4.7E-07";
            }

        }

        private bool LoadLensData()
        {
            try
            {
                lens.Side1.R = double.Parse(R1TextBox.Text);
                lens.Side1.C = lens.Side1.R.SetCurvature();
                lens.Side1.K = double.Parse(K1TextBox.Text);
                lens.Side1.AD = double.Parse(AD1TextBox.Text);
                lens.Side1.AE = double.Parse(AE1TextBox.Text);
                lens.Side1.Type = lens.Side1.SetSurfaceType();


                lens.Side2.R = double.Parse(R2TextBox.Text);
                lens.Side2.C = lens.Side2.R.SetCurvature();
                lens.Side2.K = double.Parse(K2TextBox.Text);
                lens.Side2.AD = double.Parse(AD2TextBox.Text);
                lens.Side2.AE = double.Parse(AE2TextBox.Text);
                lens.Side2.Type = lens.Side2.SetSurfaceType();

                lens.CT = double.Parse(CTTextBox.Text);
                lens.ap = double.Parse(ApTextBox.Text) / 2;
                lens.WL = double.Parse(WLTextBox.Text);
                lens.Diameter = double.Parse(Diameter.Text);

                string mat = MatNames.SelectedItem.ToString();
                if (mat != "Custom")
                {
                    double index = MaterialIndex.GetIndex(mat, lens.WL);
                    IndexTextBox.Text = index.ToString("f5");
                }
                lens.Material = mat;
                lens.Type = Misc.GenerateLensType(lens.Side1.R, lens.Side2.R);

                lens.n = double.Parse(IndexTextBox.Text);

                if (EFLTextBox.Text == "")
                {
                    lens.EFL = 0;
                    EFLTextBox.Text = "0";
                }
                else
                    lens.EFL = double.Parse(EFLTextBox.Text);

                if (BFLTextBox.Text == "")
                {
                    lens.BFL = 0;
                    BFLTextBox.Text = "0";
                }
                else
                    lens.BFL = double.Parse(BFLTextBox.Text);

                if (refocusTextBox.Text == "")
                {
                    Refocus = 0;
                    refocusTextBox.Text = "0";
                }
                else
                    Refocus = double.Parse(refocusTextBox.Text);

                lens = FirstOrderCalcs(lens);

                var errfunc = CalcErrorFunc(lens, rays, false);

                if (errfunc < 0.020)
                {
                    errFuncUnits.Text = "μm"; 
                    ErrTextBox.Text = (errfunc*1000).ToString("f5");
                }
                else
                {
                    errFuncUnits.Text = "mm";
                    ErrTextBox.Text = errfunc.ToString("f5");
                }

                wfeErrorTextBox.Text = gMath.CalcWFEPV(lens, Refocus).ToString("f5");
                Bitmap b = GenerateLensDrawing(lens);
                lpix.Image = b as Image;

                return true;
            }
            catch
            {
                // There is a problem in one of the text boxes!
                // check each text box for an alpha numeric value

                foreach (TextBox tb in tblist)
                {
                    try
                    {
                        var x = double.Parse(tb.Text);
                    }
                    catch
                    {
                        var tname = tb.Name;
                        var subt = tname.Substring(0, tname.IndexOf("Text"));                        
                        MessageBox.Show("Error in number entry for " + subt + " (maybe be blank)!", 
                                        "Textbox Entry Error - Check 1");
                        tb.Text = "0";
                        tb.Focus();
                    }
                }

                return false;
            }
        }


        //Calculations
        private Lens FirstOrderCalcs(Lens lensp)
        {
            double Phi = (lensp.n - 1) * (lensp.Side1.C - lensp.Side2.C + (lensp.n - 1) * lensp.CT * lensp.Side1.C * lensp.Side2.C / lensp.n);
            lensp.EFL = 1 / Phi;
            double PrincPlane = ((lensp.n - 1) * lensp.Side1.C * lensp.EFL) * (1 / lensp.n) * lensp.CT;
            lensp.BFL = lensp.EFL - PrincPlane;
            EFLTextBox.Text = lensp.EFL.ToString("f3");
            BFLTextBox.Text = lensp.BFL.ToString("f6");
            return lensp;
        }

        private double CalcErrorFunc(Lens lensp, List<double> rays, bool PrtAll = false)
        {
            Debug = false;
            double ave = 0;
            double rms = 0;
            if (PrtAll)
                rtb.AppendText("\nYrel,   Yreal (mm),   Yend (mm)\n");
            foreach (double ytemp in rays)
            {
                //yp = CalcRayError(ytemp * lensp.ap, lensp);
                var P = RTM.Trace_3D(ytemp * lens.ap, lensp, Refocus);
                ave += P.Y;
                rms += P.Y * P.Y;
                if (PrtAll)
                    rtb.AppendText(ytemp.ToString("f3") + ",   " + (ytemp * lensp.ap).ToString("f6") + ",   " + P.Y.ToString("f10") + "\n");
            }
            ave /= rays.Count();
            rms = Math.Sqrt(rms / rays.Count());
            if (PrtAll)
                rtb.AppendText("\nAverage:  " + ave.ToString("f6") + " mm\nRMS:  " + rms.ToString("f6") + " mm\n");
            Debug = true;
            return rms;
        }

        private double FindDirection(Lens lensp, double deltaae, int whichvar)
        {
            double left, right, center, basevalue;
            unsafe
            {
                double* ptr;
                switch (whichvar)
                {
                    case 0:
                        ptr = &lensp.Side1.K;
                        break;
                    case 1:
                        ptr = &lensp.Side1.AD;
                        break;
                    case 2:
                        ptr = &lensp.Side1.AE;
                        break;
                    case 3:
                        ptr = &lensp.Side2.K;
                        break;
                    case 4:
                        ptr = &lensp.Side2.AD;
                        break;
                    case 5:
                        ptr = &lensp.Side2.AE;
                        break;
                    default:
                        ptr = null;
                        break;
                }

                basevalue = *ptr;
                center = CalcErrorFunc(lensp, rays);
                *ptr += deltaae;
                right = CalcErrorFunc(lensp, rays);
                *ptr -= 2 * deltaae;
                left = CalcErrorFunc(lensp, rays);
                *ptr = basevalue;
            }
            //rtb.AppendText("\nLeft Diff:  " + (center - left).ToString("e3") + ",   right:  " + (center - right).ToString("e3") + "\n\n");

            if (left < center)
                return -deltaae;
            if (right < center)
                return deltaae;
            return 0.0;
        }

        // Buttons


        private void CalcErrFuncButton_Click(object sender, EventArgs e)
        {
            if (LoadLensData())
            {
                CalcErrorFunc(lens, rays, true);
            }
            else
            {
                ErrTextBox.Text = "NA";
            }
        }

        private void OPDTraceButton_Click(object sender, EventArgs e)
        {
            double tempBFL = Double.Parse(BFLTextBox.Text);
            LoadLensData();
            rtb.AppendText("\nYreal(mm),    OPL(mm),      OPD(mm)\n");
            double Min = 1e6, Max = -1e6;
            var opdref = 0.0.CalcRayOPD(lens, Refocus);
            for (double relap = 0; relap < 1.001; relap += 0.1)
            {
                var yin = relap * lens.ap;
                var opl = yin.CalcRayOPD(lens, Refocus);
                var opd = yin.CalcRayOPD(lens, Refocus) - opdref;
                rtb.AppendText(yin.ToString("f6") + ",  " + opl.ToString("f6") + ",   " + opd.ToString("f6") + "\n");
                if (opd < Min)
                    Min = opd;
                if (opd > Max)
                    Max = opd;
            }
            Min *= 1000;
            Max *= 1000;
            rtb.AppendText("\nMax:  " + Max.ToString("F3") + " μm,  Min:  " + Min.ToString("f3") + " μm\n");
            rtb.AppendText("OPD:  " + (Max - Min).ToString("f3") + " μm");
        }


        private void SpotDiagramButton_Click(object sender, EventArgs e)
        {

            SpotDiagram2D spt = new SpotDiagram2D(lens, Refocus);
            spt.Show();
        }


        private void ClearRTBButton_Click(object sender, EventArgs e)
        {
            rtb.Clear();
        }

        private void SetIndexFromTableButton_Click(object sender, EventArgs e)
        {
            string mat = MatNames.SelectedItem.ToString();
            if (mat != "Custom")
            {
                double index = MaterialIndex.GetIndex(mat, lens.WL);
                IndexTextBox.Text = index.ToString("f5");
            }
        }

        private void MatNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mat = MatNames.SelectedItem.ToString();
            if (mat != "Custom")
            {
                double index = MaterialIndex.GetIndex(mat, lens.WL);
                IndexTextBox.Text = index.ToString("f5");
            }

            if (LoadLensData())
            {
                if (autoSolve)
                {
                    double autoR2 = Misc.CalcR2(lens.Side1.R, lens.n, lens.CT, autoEFL);
                    R2TextBox.Text = autoR2.ToString("f3");
                    LoadLensData();  // reload with new R2
                }
                lens = FirstOrderCalcs(lens);

            }
            else
                ErrTextBox.Text = "NA";

            displayErrorFunctionToolStripMenuItem.PerformClick();
        }

        private void clearAsphericData()
        {
            K1CheckBox.Checked = false;
            K1TextBox.Text = "0";
            AD1CheckBox.Checked = false;
            AD1TextBox.Text = "0";
            AE1CheckBox.Checked = false;
            AE1TextBox.Text = "0";

            K2CheckBox.Checked = false;
            K2TextBox.Text = "0";
            AD2CheckBox.Checked = false;
            AD2TextBox.Text = "0";
            AE2CheckBox.Checked = false;
            AE2TextBox.Text = "0";

        }

        private void drawRayTraceButton_Click(object sender, EventArgs e)
        {
            DrawRayTrace dl = new DrawRayTrace(lens, Refocus, MatNames.Text);
            dl.ShowDialog();
        }

        private void GenericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) && 
                (e.KeyChar != '.') &&
                (e.KeyChar != '-') && 
                (e.KeyChar != 'e') &&
                (e.KeyChar != 'E') )
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void GenericTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (LoadLensData())
            {
                if ((Control)sender == WLTextBox)  // do this in event the wavelength is changed
                    SetIndexFromTableButton.PerformClick();
                    
                if (autoSolve)
                {
                    double autoR2 = Misc.CalcR2(lens.Side1.R, lens.n, lens.CT, autoEFL);
                    R2TextBox.Text = autoR2.ToString("f3");
                    LoadLensData();  // reload with new R2
                }
                lens = FirstOrderCalcs(lens);

            }
            else
                ErrTextBox.Text = "NA";
        }

        private void Auto_Click(object sender, EventArgs e)
        {
            if (Auto.BackColor == Color.Red)
            {
                Auto.BackColor = Color.Green;
                autoSolve = true;
                string targefl = (double.Parse(EFLTextBox.Text)).ToString("f0");
                Prompt pt = new Prompt("AutoSolve R2 for Fixed EFL", "Enter Target EFL:", targefl);
                pt.ShowDialog();
                if (pt.isOk == true)
                {
                    autoEFL = pt.promptValue;
                    autoEFLstr = pt.valueTextBox.Text;
                }
                if (LoadLensData())
                {
                    if (autoSolve)
                    {
                        double autoR2 = Misc.CalcR2(lens.Side1.R, lens.n, lens.CT, autoEFL);
                        R2TextBox.Text = autoR2.ToString("f3");
                        LoadLensData();  // reload with new R2
                    }
                    lens = FirstOrderCalcs(lens);

                }
                else
                    ErrTextBox.Text = "NA";
            }
            else
            {
                Auto.BackColor = Color.Red;
                autoSolve = false;
            }
            
        }

        private void TraceMarginalRayButton_Click(object sender, EventArgs e)
        {
            LoadLensData();
            var margray = lens.ap.GenerateRayTable(lens, Refocus);
            PopUpDataTable pdt = new PopUpDataTable(margray, "Marginal Ray Data Table");
            pdt.Show();
        }

        private void posiMenLensDropDownButton_Click(object sender, EventArgs e)
        {
            SetIndexFromTableButton.PerformClick();
            Prompt pt = new Prompt("EFL Input Dialog", "Enter Target EFL:", EFLTextBox.Text);
            pt.ShowDialog();
            if (pt.isOk == true)
            {
                var design = Misc.DesignPosMenLens(lens.n, lens.CT, pt.promptValue);
                R1TextBox.Text = design.R1.ToString("F3");
                R2TextBox.Text = design.R2.ToString("F3");
                clearAsphericData();
                LoadLensData();
                if (Debug)
                    rtb.AppendText(design.results.ToString());
                displayErrorFunctionToolStripMenuItem.PerformClick();
            }
        }

        private void planocxLensButton_Click(object sender, EventArgs e)
        {
            SetIndexFromTableButton.PerformClick();
            Prompt pt = new Prompt("EFL Input Dialog", "Enter Target EFL:", EFLTextBox.Text);
            pt.ShowDialog();
            if (pt.isOk == true)
            {
                double index = double.Parse(IndexTextBox.Text);
                double r1 = double.Parse((pt.promptValue * (index - 1)).ToString("f2"));
                lens.Side1.R = r1;
                lens.Side2.R = 0.0;
                R1TextBox.Text = r1.ToString("f3");
                R2TextBox.Text = "0.0";
                clearAsphericData();
                if (LoadLensData())
                {
                    lens = FirstOrderCalcs(lens);
                    displayErrorFunctionToolStripMenuItem.PerformClick();
                }
            }
        }


        private void inputRaySetPlotButton_Click(object sender, EventArgs e)
        {
            InputRayPlot irp = new InputRayPlot(lens);
            irp.Show();
        }

        private void traceSingleRayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLensData();
            Prompt pt = new Prompt("Enter Ray Height Dialog", "Enter ray height:", "");
            pt.ShowDialog();
            if (pt.isOk == true)
            {
                double rayheight = (double)pt.promptValue;
                if (rayheight <= lens.ap)
                {
                    var raytrace = rayheight.GenerateRayTable(lens, Refocus);
                    PopUpDataTable pdt = new PopUpDataTable(raytrace, "Ray Table for Ray at Y = " + 
                                        rayheight.ToString("f3") + " mm");
                    pdt.Show();
                }
                else
                {
                    MessageBox.Show("Requested ray height (" + rayheight.ToString("f3") + " mm) is outside of CA.\n" + 
                                    "Max traceable ray height should be <= " +
                                    lens.ap.ToString("f3") + " mm.\nTray Again?", 
                                    "Ray Height Exceeds Clear Aperture");
                }
            }

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
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            ChangeSettings cs = new ChangeSettings();
            cs.ShowDialog();
        }


        private void transverseSphericalAbPlotButton_Click(object sender, EventArgs e)
        {
            TransverseSphericalPlot traplot = new TransverseSphericalPlot(lens, Refocus);
            traplot.Show();
        }

        private void wavefrontErrorButton_Click(object sender, EventArgs e)
        {
            Debug = false;

            LoadLensData();

            WaveFrontErrorPlot wfplot = new WaveFrontErrorPlot(lens, Refocus);
            wfplot.Show();
        }

        private void plotWavefrontError2dMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLensData();
            WFE2DMap wfemap = new WFE2DMap(lens, Refocus);
            wfemap.Show();
        }



        private void minimize3rdOrderSphericalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            refocusTextBox.Text = "0.0";
            double PV = gMath.CalcTSAPV(lens, 0.0);
            rtb.AppendText("TSA PV: " + PV.ToString("f3") + "\n");

            double refocus = 0.0;
            double err;
            double deltaref = 0.1;
            double dref;
            double mintweakstep = 1e-10;

            for (int j = 0; j < 3; j++)
            {
                deltaref = 0.1;
                for (int i = 0; i < 15; i++)
                {
                    dref = gMath.FindTSARefocusDirection(lens, refocus, deltaref);
                    refocus += dref;
                    if (Math.Abs(dref) < mintweakstep)
                        deltaref /= 10.0;
                    else
                    {
                        err = gMath.CalcTSAPV(lens, refocus);
                        rtb.AppendText(err.ToString("f5") + ",  " + refocus.ToString("f5") + "\n");
                        if (Math.Abs(deltaref) < mintweakstep)
                            deltaref /= 10.0;
                    }
                }
                rtb.AppendText("\n");
            }
            err = gMath.CalcTSAPV(lens, refocus);
            refocusTextBox.Text = refocus.ToString("f4");
            ErrTextBox.Text = err.ToString("f4");
            rtb.AppendText("Ref: " + refocusTextBox.Text + ",  err: " + ErrTextBox.Text + "\n");
            LoadLensData();
        }

        private void minimizeRMSWavefrontErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            refocusTextBox.Text = "0.0";
            double rms = gMath.CalcWFERMS(lens, 0.0);
            rtb.AppendText("RMS: " + rms.ToString("f3") + "\n");

            double refocus = 0.0;
            double err;
            double deltaref = 0.1;
            double dref;
            double mintweakstep = 1e-10;
            Cursor.Current = Cursors.WaitCursor;
            for (int j = 0; j < 5; j++)
            {
                deltaref = 0.1;
                for (int i = 0; i < 15; i++)
                {
                    dref = gMath.FindRMSRefocusDirection(lens, refocus, deltaref);
                    refocus += dref;
                    if (Math.Abs(dref) < mintweakstep)
                        deltaref /= 10.0;
                    else
                    {
                        err = gMath.CalcWFERMS(lens, refocus);
                        rtb.AppendText(err.ToString("e6") + ",  " + refocus.ToString("e6") + "\n");
                        if (Math.Abs(deltaref) < mintweakstep)
                            deltaref /= 10.0;
                    }
                }
                rtb.AppendText("\n");
            }
            err = gMath.CalcWFERMS(lens, refocus);
            refocusTextBox.Text = refocus.ToString("f4");
            ErrTextBox.Text = err.ToString("f4");
            rtb.AppendText("Ref: " + refocusTextBox.Text + ",  err: " + ErrTextBox.Text + "\n");
            rtb.AppendText("Finished.\n");
            Cursor.Current = Cursors.Arrow;
            rtb.ScrollToCaret();
            LoadLensData();
            
            //Thread t = new Thread(MiniRms);
            //t.Start(EFL);
        }

        private void minimizePVWavefrontErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            refocusTextBox.Text = "0.0";
            double PV = gMath.CalcWFEPV(lens, 0.0);
            rtb.AppendText("WFE PV: " + PV.ToString("f5") + "\n");

            double refocus = 0.0;
            double err;
            double deltaref = 0.1;
            double dref;
            double mintweakstep = 1e-10;

            for (int j = 0; j < 3; j++)
            {
                deltaref = 0.1;
                for (int i = 0; i < 15; i++)
                {
                    dref = gMath.FindPVRefocusDirection(lens, refocus, deltaref);
                    refocus += dref;
                    if (Math.Abs(dref) < mintweakstep)
                        deltaref /= 10.0;
                    else
                    {
                        err = gMath.CalcWFEPV(lens, refocus);
                        rtb.AppendText(err.ToString("e6") + ",  " + refocus.ToString("e6") + "\n");
                        if (Math.Abs(deltaref) < mintweakstep)
                            deltaref /= 10.0;
                    }
                }
                rtb.AppendText("\n");
            }
            err = gMath.CalcWFEPV(lens, refocus);
            refocusTextBox.Text = refocus.ToString("f4");
            ErrTextBox.Text = err.ToString("f4");
            rtb.AppendText("Ref: " + refocusTextBox.Text + ",  err: " + ErrTextBox.Text + "\n");
            LoadLensData();
        }

        private void MiniRms ( )
        {
            double rms = gMath.CalcWFERMS(lens, 0.0);

            double refocus = 0.0;
            double err;
            double deltaref = 0.1;
            double dref;
            double mintweakstep = 1e-10;
            Cursor.Current = Cursors.WaitCursor;
            for (int j = 0; j < 5; j++)
            {
                deltaref = 0.1;
                for (int i = 0; i < 15; i++)
                {
                    dref = gMath.FindRMSRefocusDirection(lens, refocus, deltaref);
                    refocus += dref;
                    if (Math.Abs(dref) < mintweakstep)
                        deltaref /= 10.0;
                    else
                    {
                        err = gMath.CalcWFERMS(lens, refocus);
                        if (Math.Abs(deltaref) < mintweakstep)
                            deltaref /= 10.0;
                    }
                }
            }
            err = gMath.CalcWFERMS(lens, refocus);
            Cursor.Current = Cursors.Arrow;
        }

        private void maxPSFButton_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            refocusTextBox.Text = "0.0";
            var psf = Misc.GenMaxPSF(lens, Refocus, 64, 128);
            rtb.AppendText("PSF Calc: " + psf.ToString("f4") + "\n");

            double refocus = 0.0;
            double err;
            double deltaref = 0.1;
            double dref;
            double mintweakstep = 1e-5;

            for (int j = 0; j < 3; j++)
            {
                deltaref = 0.1  ;
                for (int i = 0; i < 15; i++)
                {
                    dref = gMath.FindPSFDirection(lens, refocus, deltaref);
                    refocus += dref;
                    if (Math.Abs(dref) < mintweakstep)
                        deltaref /= 10.0;
                    else
                    {
                        err = Misc.GenMaxPSF(lens, refocus, 64, 128);
                        rtb.AppendText("PSF: " + err.ToString("f5") + ",  Refocus: " + refocus.ToString("f6") + "\n");
                        if (Math.Abs(deltaref) < mintweakstep)
                            deltaref /= 10.0;
                    }
                }
                rtb.AppendText("\n");
            }
            err = Misc.GenMaxPSF(lens, refocus, 64, 128);
            refocusTextBox.Text = refocus.ToString("f4");
            ErrTextBox.Text = err.ToString("f4");
            rtb.AppendText("Ref: " + refocusTextBox.Text + ",  err: " + ErrTextBox.Text + "\n");
            LoadLensData();
            watch.Stop();
            rtb.AppendText("Time To Max: " + watch.ElapsedMilliseconds + " ms");
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void optimizeToolStripButton_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            refocusTextBox.Text = "0.0";
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            double cc, ad, ae, err;
            for (int k = 0; k < 15; k++)
            {
                for (int j = 0; j < 15; j++)
                {
                    double deltK = 0.6;
                    double deltAD = 3.1e-7;
                    double deltAE = 1.1e-9;
                    double mintweakstep = 1e-15;
                    for (int i = 0; i < 15; i++)
                    {
                        if (K1CheckBox.Checked)
                        {
                            cc = FindDirection(lens, deltK, 0);
                            err = CalcErrorFunc(lens.LensKTweak(cc, 1), rays);
                            //rtb.AppendText("ΔK:  " + cc.ToString("f4") + ",   K:  " + lens.Side1.K.ToString("f4") + ",   err:  " + err.ToString("g5") + "\n");
                            if (Math.Abs(cc) < mintweakstep)
                                deltK /= 10.0;
                        }

                        if (AD1CheckBox.Checked)
                        {
                            ad = FindDirection(lens, deltAD, 1);
                            err = CalcErrorFunc(lens.LensADTweak(ad, 1), rays);
                            //rtb.AppendText("ΔAD:  " + ad.ToString("e5") + ",   AD:  " + lens.Side1.AD.ToString("e4") + ",   err:  " + err.ToString("f6") + "\n");
                            if (Math.Abs(ad) < mintweakstep)
                                deltAD /= 10.0;
                        }

                        if (AE1CheckBox.Checked)
                        {
                            ae = FindDirection(lens, deltAE, 2);
                            err = CalcErrorFunc(lens.LensAETweak(ae, 1), rays);
                            //rtb.AppendText("ΔAE:  " + ae.ToString("e5") + ",   AE:  " + lens.Side1.AE.ToString("e4") + ",   err:  " + err.ToString("f6") + "\n");
                            if (Math.Abs(ae) < mintweakstep)
                                deltAE /= 10.0;
                        }
                        if (K2CheckBox.Checked)
                        {
                            cc = FindDirection(lens, deltK, 3);
                            err = CalcErrorFunc(lens.LensKTweak(cc, 2), rays);
                            //rtb.AppendText("ΔK:  " + cc.ToString("f4") + ",   K:  " + lens.Side2.K.ToString("f4") + ",   err:  " + err.ToString("g5") + "\n");
                            if (Math.Abs(cc) < mintweakstep)
                                deltK /= 10.0;
                        }

                        if (AD2CheckBox.Checked)
                        {
                            ad = FindDirection(lens, deltAD, 4);
                            err = CalcErrorFunc(lens.LensADTweak(ad, 2), rays);
                            //rtb.AppendText("ΔAD:  " + ad.ToString("e5") + ",   AD:  " + lens.Side2.AD.ToString("e4") + ",   err:  " + err.ToString("f6") + "\n");
                            if (Math.Abs(ad) < mintweakstep)
                                deltAD /= 10.0;
                        }

                        if (AE2CheckBox.Checked)
                        {
                            ae = FindDirection(lens, deltAE, 5);
                            err = CalcErrorFunc(lens.LensAETweak(ae, 2), rays);
                            //rtb.AppendText("ΔAE:  " + ae.ToString("e5") + ",   AE:  " + lens.Side2.AE.ToString("e4") + ",   err:  " + err.ToString("f6") + "\n");
                            if (Math.Abs(ae) < mintweakstep)
                                deltAE /= 10.0;
                        }
                    }
                }
                err = CalcErrorFunc(lens, rays);
                rtb.AppendText("(" + k + ")");
                if (K1CheckBox.Checked)
                    rtb.AppendText(",   ΔK1: " + lens.Side1.K.ToString("f4"));
                if (AD1CheckBox.Checked)
                    rtb.AppendText(",   ΔAD1: " + lens.Side1.AD.ToString("e4"));
                if (AE1CheckBox.Checked)
                    rtb.AppendText(",   ΔAE1: " + lens.Side1.AE.ToString("e4"));
                if (K2CheckBox.Checked)
                    rtb.AppendText(",   ΔK2: " + lens.Side2.K.ToString("f4"));
                if (AD2CheckBox.Checked)
                    rtb.AppendText(",   ΔAD2: " + lens.Side2.AD.ToString("e4"));
                if (AE2CheckBox.Checked)
                    rtb.AppendText(",   ΔAE2: " + lens.Side2.AE.ToString("e4"));
                rtb.AppendText(",   err:  " + err.ToString("g5") + "\n");

            }
            string kfmt = "f5";
            string afmt = "0.000E-00";
            if (K1CheckBox.Checked)
                K1TextBox.Text = lens.Side1.K.ToString(kfmt);
            if (AD1CheckBox.Checked)
                AD1TextBox.Text = lens.Side1.AD.ToString(afmt);
            if (AE1CheckBox.Checked)
                AE1TextBox.Text = lens.Side1.AE.ToString(afmt);

            if (K2CheckBox.Checked)
                K2TextBox.Text = lens.Side2.K.ToString(kfmt);
            if (AD2CheckBox.Checked)
                AD2TextBox.Text = lens.Side2.AD.ToString(afmt);
            if (AE2CheckBox.Checked)
                AE2TextBox.Text = lens.Side2.AE.ToString(afmt);

            LoadLensData();
            err = CalcErrorFunc(lens, rays);

        }

        private void displayLensDrawngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LensDrawing ld = new LensDrawing(lens, MatNames.Text);
            ld.Show();
        }

        private void estimateFiberFocus_Click(object sender, EventArgs e)
        {
            rtb.AppendText("\nProcessing Cross Section Data......\n");

            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            Prompt pt = new Prompt("Calculate Ext. Source Focus", "Enter Size of Extended Focus: ", "0.20");
            pt.ShowDialog();
            double fiber_radius = 0.1;
            if (pt.isOk == true)
            {
                fiber_radius = pt.promptValue / 2;
            }
            else
            {
                MessageBox.Show("Error or Canceled Prompt! Canceling Command.");
                return;
            }

            ExtendedSourcePlot esp = new ExtendedSourcePlot(lens, Refocus, fiber_radius);
            esp.Show();
        }


        private void estimateFiberFocusColorMapButton_Click(object sender, EventArgs e)
        {
            rtb.AppendText("\nProcessing 3D Map Data......\n");
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            Prompt pt = new Prompt("Calculate Ext. Source Focus", "Enter Size of Extended Focus: ", "0.20");
            pt.ShowDialog();

            double fiber_radius = 0.1;
            if (pt.isOk == true)
            {
                fiber_radius = pt.promptValue / 2;
            }
            else
            {
                MessageBox.Show("Error or Canceled Prompt! Canceling Command.");
                return;
            }

            ExtSource3DMap e3d = new ExtSource3DMap(lens, Refocus, fiber_radius);
            e3d.Show();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Width = thisWidth;  // keep width the same
            splitContainer3.SplitterDistance = splitContainer3.Width / 2;
            splitContainer4.SplitterDistance = splitContainer4.Width / 2;
        }

        public static List<T> GetSubControlsOf<T>(Control parent) where T : Control
        {
            var myCtrls = new List<T>();

            foreach (Control ctrl in parent.Controls)
            {
                // if ctrl is type of T
                if (ctrl.GetType() == typeof(T) ||
                    ctrl.GetType().IsInstanceOfType(typeof(T)))
                {
                    myCtrls.Add(ctrl as T);
                }
                else if (ctrl.HasChildren)
                {
                    var childs = GetSubControlsOf<T>(ctrl);
                    if (childs.Any())
                        myCtrls.AddRange(childs);
                }
            }

            return myCtrls;
        }

        private Bitmap GenerateLensDrawing(Lens lens)
        {
            float scale;

            int wl = 500;
            Bitmap b = new Bitmap(wl, wl);

            List<PointF> lenspts = lens.TraceLensShape();
            var limits = lenspts.MinMaxFloatListPt();

            // calculate scale factor
            scale = ((float)wl / (limits.MaxY - limits.MinY)) * 0.85F;
            PointF scalept = new PointF(scale, -scale);
            PointF offsetpt = new PointF((float)(wl / 2), (float)(wl / 2));

            Pen clinepen = new Pen(Color.Black, 1F);
            clinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            using (Graphics g = Graphics.FromImage(b))
            {
                // draw lens shape and fill color
                SolidBrush fl = new SolidBrush(Color.LightBlue);
                if (lens.Material == "ZnSe")
                    fl = new SolidBrush(Color.Orange);
                if (lens.Material == "ZnS")
                    fl = new SolidBrush(Color.LightYellow);
                g.FillPolygon(fl, lenspts.ToArray().MultiTransform(scalept, offsetpt));
                g.DrawPolygon(new Pen(Color.Black, 1F), lenspts.ToArray().MultiTransform(scalept, offsetpt));

                PointF[] cl = new PointF[] { new PointF((float)-lens.CT, 0F), new PointF((float)lens.CT, 0F) };
                g.DrawPolygon(clinepen, cl.MultiTransform(scalept, offsetpt));
            }

            return b;
        }

        private void estimateDiffractionWavefrontButton_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            rtb.AppendText("\nProcessing Diffraction Analysis......\n");
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Xin, Yin, Zin, SinXin, SinYin, CosZin, Xout, Yout, Zout, SinXout, SinYout, SinZout");

            List<Vector3D> Vlist = new List<Vector3D>();

            // Generate Base Ray
            //
            // far field beam divergence 
            //
            // Ɵ = λ / (π w0)
            //
            double theta = (lens.WL / 1000) / (Math.PI * 0.5);

            double w0_inc = 0.75;
            double xbase = 0, ybase = 5, zbase = 0;

            var Vin = new Vector3D(xbase, ybase, zbase);
            var Cin = new Vector3D(0, 0, 1);

            double w0xa, w0xb, w0ya, w0yb;
            double w0x, w0y;

            var (V0, C0) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V0, C0);
            sb.AppendLine(FormatVectors4(Vin, Cin, V0, C0));
            sb.AppendLine();

            // *********************************

            // +Y 
            Vin.Y = ybase + w0_inc;
            var (V1a, C1a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V1a, C1a);
            sb.AppendLine(FormatVectors4(Vin, Cin, V1a, C1a));

            // +Y Theta
            Vin.Y = ybase;
            Cin.Y = Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V1b, C1b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V1b, C1b);
            sb.AppendLine(FormatVectors4(Vin, Cin, V1b, C1b));
            sb.AppendLine();

            // *********************************

            // -Y Theta
            Vin.Y = ybase;
            Cin.Y = -Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V2b, C2b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V2b, C2b);
            sb.AppendLine(FormatVectors4(Vin, Cin, V2b, C2b));


            // -Y
            Vin.Y = ybase - w0_inc;
            Cin.Y = 0;
            Cin.Z = 1;
            var (V2a, C2a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V2a, C2a);
            sb.AppendLine(FormatVectors4(Vin, Cin, V2a, C2a));
            sb.AppendLine();

            // *********************************

            Vin.Y = ybase;

            // +X 
            Vin.X = xbase + w0_inc;
            var (V3a, C3a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V3a, C3a);
            sb.AppendLine(FormatVectors4(Vin, Cin, V3a, C3a));

            // +X Theta
            Vin.X = xbase;
            Cin.X = Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V3b, C3b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V3b, C3b);
            sb.AppendLine(FormatVectors4(Vin, Cin, V3b, C3b));
            sb.AppendLine();

            // *********************************

            // -X Theta
            Cin.X = -Math.Sin(theta);
            Cin.Z = Math.Sqrt(1 - Cin.X * Cin.X - Cin.Y * Cin.Y);
            var (V4b, C4b) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V3b, C3b);
            sb.AppendLine(FormatVectors4(Vin, Cin, V4b, C4b));

            // -X
            Vin.X = xbase - w0_inc;
            Cin.X = 0;
            Cin.Z = 1;
            var (V4a, C4a) = RTM.Trace_3D_Plus(Vin, Cin, lens, Refocus);
            Print4V(Vin, Cin, V4a, C4a);
            sb.AppendLine(FormatVectors4(Vin, Cin, V4a, C4a));

            // *********************************

            Clipboard.SetText(sb.ToString());

            w0ya = Math.Abs(C0.Y - C1a.Y);
            w0yb = Math.Abs(C0.Y - C2a.Y);
            w0xa = Math.Abs(C0.X - C3a.X);
            w0xb = Math.Abs(C0.X - C4a.X);

            rtb.AppendText("\n" + w0ya.ToString() + ", " + w0yb.ToString() + ", " + w0xa.ToString() + ", " + w0xb.ToString());
            //
            // w0 = λ / (π Ɵ)
            //

            w0x = (lens.WL / 1000) / (Math.PI * (w0xa + w0xb) / 2);
            w0y = (lens.WL / 1000) / (Math.PI * (w0ya + w0yb) / 2);

            rtb.AppendText("\n" + w0y.ToString("f4") + ", " + w0x.ToString("f4"));
        }

        private void EstDiffWavefrontButton_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            rtb.AppendText("\nProcessing Diffraction Analysis......\n");
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);
            List<MicroGauss> mglist = new List<MicroGauss>();

            var space = .3;
            double beamrad = 10;
            double r2 = beamrad * Math.Sqrt(2);

            for (double y = -beamrad; y <= beamrad; y += space)
                for(double x = -beamrad; x <= beamrad; x += space)
                {
                    if ((y * y + x * x) <= r2)
                    {
                        var mg = RTM.TraceMicroGauss(lens, Refocus, x, y, space * 1.5 / 2);
                        mglist.Add(mg);
                        rtb.AppendText(mg.Loc.ToString2("f4") + ", " + mg.Slopes.ToString2("f4") + ", " + mg.W0_X.ToString("f4") + ", " + mg.W0_Y.ToString("f4") + "\n");
                    }
                }

            DiffSpotSizeMap dmap = new DiffSpotSizeMap(mglist, lens);
            dmap.Show();
        }

        private void Print4V(Vector3D Vin, Vector3D Cin, Vector3D Vout, Vector3D Cout)
        {
            rtb.AppendText(Vin.ToString() + " : " + Cin.ToString("F6") + "\n");
            rtb.AppendText(Vout.ToString() + " : " + Cout.ToString("F6") + "\n");
        }

        private string FormatVectors4(Vector3D Vin, Vector3D Cin, Vector3D Vout, Vector3D Cout)
        {
            return (Vin.X.ToString() + ", " + Vin.Y.ToString() + ", " + Vin.Z.ToString() + ", " +
                          Cin.X.ToString() + ", " + Cin.Y.ToString() + ", " + Cin.Z.ToString() + ", " +
                          Vout.X.ToString() + ", " + Vout.Y.ToString() + ", " + Vout.Z.ToString() + ", " +
                          Cout.X.ToString() + ", " + Cout.Y.ToString() + ", " + Cout.Z.ToString());
        }

        private void pSFDiffractionBasedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PSF_DiffractionBased psfChart = new PSF_DiffractionBased(lens, Refocus);
            psfChart.Show();
        }

        private void copyFormToClip_Click(object sender, EventArgs e)
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

        private void lensShapeButton_Click(object sender, EventArgs e)
        {
            Panel panel = splitContainer3.Panel2;

            var b = new Bitmap(panel.Width, panel.Height);
            panel.DrawToBitmap(b, new Rectangle(0, 0, panel.Width, panel.Height));

            //var b = new Bitmap(lpix.Width, lpix.Height);
            //lpix.DrawToBitmap(b, new Rectangle(0, 0, lpix.Width, lpix.Height));

            b.MakeTransparent(Color.White);

            // convert bitmap to mem stream to preserve transparency in clipboard
            // essentially this is the same as if you save bitmap to a png file
            // then retreive the png file to the clipboard for copying to another app.
            
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // convert memory stream to data object
            IDataObject dataObject = new DataObject();
            dataObject.SetData("PNG", false, ms);

            Clipboard.SetDataObject(dataObject, false);

            MessageBox.Show("Lens Drawing Copied to Clipboard");
        }

        private void GenericTextBox_Leave(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            try
            {
                var x = double.Parse(t.Text);
            }
            catch
            {
                var tname = (sender as TextBox).Name;
                var subt = tname.Substring(0, tname.IndexOf("Text"));
                MessageBox.Show("Error in number entry for " + subt + " (maybe be blank)!", 
                                "Textbox Entry Error - Check 2");
                (sender as TextBox).Text = "0";
                t.Focus();
            }
        }

        private void ExtendedSourcebyFOV_Click(object sender, EventArgs e)
        {
            rtb.AppendText("\nProcessing Field Of View Request......\n");

            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            Prompt pt = new Prompt("Field of View Source", "Enter ½ FOV in Degrees: ", "0.20");
            pt.ShowDialog();
            double hfov= 0.1;
            if (pt.isOk == true)
            {
                hfov = pt.promptValue * (Math.PI / 180.0) * lens.EFL;
            }
            else
            {
                MessageBox.Show("Error or Canceled Prompt! Canceling Command.");
                return;
            }

            ExtSource3DMap esp = new ExtSource3DMap(lens, Refocus, hfov);
            esp.Show();
        }

        private void spotSizeOffAxisMenuItem_Click(object sender, EventArgs e)
        {
            rtb.AppendText("\nProcessing Off Axis Spot Diagram......\n");
            if (!LoadLensData()) return;
            lens = FirstOrderCalcs(lens);

            Prompt pt = new Prompt("Off Axis Angle Prompt", "Enter Off Axis Angle (degrees): ", offaxisangle.ToString("F4"));
            pt.ShowDialog();

            if (pt.isOk == true)
            {
                offaxisangle = pt.promptValue;
            }
            else
            {
                MessageBox.Show("Error or Canceled Prompt! Canceling Command.");
                return;
            }

            SpotDiagram2D spt = new SpotDiagram2D(lens, Refocus, OffAxis: true, OffAngleIn: offaxisangle);
            spt.Show();
        }
    }


}
