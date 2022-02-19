
namespace AspGen
{
    partial class PSF_DiffractionBased
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSF_DiffractionBased));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyChartToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFormClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.saveChartToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToClipBoard = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToCSVFile = new System.Windows.Forms.ToolStripMenuItem();
            this.showTheory = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScaleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.setHorizontalScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setVerticalScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(632, 500);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(632, 553);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timerLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(632, 26);
            this.statusStrip1.TabIndex = 0;
            // 
            // timerLabel
            // 
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(21, 20);
            this.timerLabel.Text = "**";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 490);
            this.panel1.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.AxisX.Title = "Focus Cross Section (mm)";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.Title = "Relative Intensity";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(622, 490);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title1.Text = "Diffraction Based Point Spread Function (PSF)";
            this.chart1.Titles.Add(title1);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(268, 27);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(255, 24);
            this.toolStripButton1.Text = "Right Mouse Click Brings Up Options";
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyChartToClipboardToolStripMenuItem,
            this.copyFormClipboard,
            this.saveChartToFileToolStripMenuItem,
            this.saveDataToClipBoard,
            this.saveDataToCSVFile,
            this.showTheory,
            this.autoScaleButton,
            this.setHorizontalScaleToolStripMenuItem,
            this.setVerticalScaleToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(242, 220);
            // 
            // copyChartToClipboardToolStripMenuItem
            // 
            this.copyChartToClipboardToolStripMenuItem.Name = "copyChartToClipboardToolStripMenuItem";
            this.copyChartToClipboardToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.copyChartToClipboardToolStripMenuItem.Text = "Copy Chart To Clipboard";
            this.copyChartToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyChartToClipboardToolStripMenuItem_Click);
            // 
            // copyFormClipboard
            // 
            this.copyFormClipboard.Name = "copyFormClipboard";
            this.copyFormClipboard.Size = new System.Drawing.Size(241, 24);
            this.copyFormClipboard.Text = "Copy Form To Clipboard";
            this.copyFormClipboard.Click += new System.EventHandler(this.copyFormClipboard_Click);
            // 
            // saveChartToFileToolStripMenuItem
            // 
            this.saveChartToFileToolStripMenuItem.Name = "saveChartToFileToolStripMenuItem";
            this.saveChartToFileToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.saveChartToFileToolStripMenuItem.Text = "Save Chart To File";
            this.saveChartToFileToolStripMenuItem.Click += new System.EventHandler(this.saveChartToFileToolStripMenuItem_Click);
            // 
            // saveDataToClipBoard
            // 
            this.saveDataToClipBoard.Name = "saveDataToClipBoard";
            this.saveDataToClipBoard.Size = new System.Drawing.Size(241, 24);
            this.saveDataToClipBoard.Text = "Save Data To Clipboard";
            this.saveDataToClipBoard.Click += new System.EventHandler(this.saveDataToClipBoard_Click);
            // 
            // saveDataToCSVFile
            // 
            this.saveDataToCSVFile.Name = "saveDataToCSVFile";
            this.saveDataToCSVFile.Size = new System.Drawing.Size(241, 24);
            this.saveDataToCSVFile.Text = "Save Data To CSV File";
            this.saveDataToCSVFile.Click += new System.EventHandler(this.saveDataToCSVFile_Click);
            // 
            // showTheory
            // 
            this.showTheory.Name = "showTheory";
            this.showTheory.Size = new System.Drawing.Size(241, 24);
            this.showTheory.Text = "Show Best Case Theory";
            this.showTheory.Click += new System.EventHandler(this.showTheory_Click);
            // 
            // autoScaleButton
            // 
            this.autoScaleButton.Name = "autoScaleButton";
            this.autoScaleButton.Size = new System.Drawing.Size(241, 24);
            this.autoScaleButton.Text = "Auto Scale";
            this.autoScaleButton.Click += new System.EventHandler(this.autoScaleButton_Click);
            // 
            // setHorizontalScaleToolStripMenuItem
            // 
            this.setHorizontalScaleToolStripMenuItem.Name = "setHorizontalScaleToolStripMenuItem";
            this.setHorizontalScaleToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.setHorizontalScaleToolStripMenuItem.Text = "Set Horizontal Scale";
            this.setHorizontalScaleToolStripMenuItem.Click += new System.EventHandler(this.setHorizontalScaleToolStripMenuItem_Click);
            // 
            // setVerticalScaleToolStripMenuItem
            // 
            this.setVerticalScaleToolStripMenuItem.Name = "setVerticalScaleToolStripMenuItem";
            this.setVerticalScaleToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.setVerticalScaleToolStripMenuItem.Text = "Set Vertical Scale";
            this.setVerticalScaleToolStripMenuItem.Click += new System.EventHandler(this.setVerticalScaleToolStripMenuItem_Click);
            // 
            // PSF_DiffractionBased
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 553);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "PSF_DiffractionBased";
            this.Text = "PSF_DiffractionBased";
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem copyChartToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChartToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDataToCSVFile;
        private System.Windows.Forms.ToolStripMenuItem saveDataToClipBoard;
        private System.Windows.Forms.ToolStripMenuItem showTheory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timerLabel;
        private System.Windows.Forms.ToolStripMenuItem copyFormClipboard;
        private System.Windows.Forms.ToolStripMenuItem autoScaleButton;
        private System.Windows.Forms.ToolStripMenuItem setHorizontalScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setVerticalScaleToolStripMenuItem;
    }
}