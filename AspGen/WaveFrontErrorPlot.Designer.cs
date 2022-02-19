
namespace AspGen
{
    partial class WaveFrontErrorPlot
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
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveFrontErrorPlot));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.informationButton = new System.Windows.Forms.ToolStripButton();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToBitmap = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDataToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePNGFile = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScaleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.setHorizontalScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setVerticalScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 522);
            this.panel1.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(832, 522);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "Title2";
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(832, 522);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(832, 553);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.informationButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(832, 31);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabel1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(313, 28);
            this.toolStripLabel1.Text = "Right Mouse Click Over Plot to Bring Up Menu";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // informationButton
            // 
            this.informationButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.informationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.informationButton.Image = ((System.Drawing.Image)(resources.GetObject("informationButton.Image")));
            this.informationButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.informationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.informationButton.Name = "informationButton";
            this.informationButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.informationButton.Size = new System.Drawing.Size(144, 28);
            this.informationButton.Text = "Info About This Plot";
            this.informationButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToBitmap,
            this.copyDataToClipboardToolStripMenuItem,
            this.savePNGFile,
            this.autoScaleButton,
            this.setHorizontalScaleToolStripMenuItem,
            this.setVerticalScaleToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(271, 148);
            // 
            // copyToBitmap
            // 
            this.copyToBitmap.Name = "copyToBitmap";
            this.copyToBitmap.Size = new System.Drawing.Size(270, 24);
            this.copyToBitmap.Text = "Copy as Bitmap to Clipboard";
            this.copyToBitmap.Click += new System.EventHandler(this.copyToBitmap_Click);
            // 
            // copyDataToClipboardToolStripMenuItem
            // 
            this.copyDataToClipboardToolStripMenuItem.Name = "copyDataToClipboardToolStripMenuItem";
            this.copyDataToClipboardToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.copyDataToClipboardToolStripMenuItem.Text = "Copy Data to Clipboard";
            this.copyDataToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyDataToClipboardToolStripMenuItem_Click);
            // 
            // savePNGFile
            // 
            this.savePNGFile.Name = "savePNGFile";
            this.savePNGFile.Size = new System.Drawing.Size(270, 24);
            this.savePNGFile.Text = "Save PNG File";
            this.savePNGFile.Click += new System.EventHandler(this.savePNGFile_Click);
            // 
            // autoScaleButton
            // 
            this.autoScaleButton.Name = "autoScaleButton";
            this.autoScaleButton.Size = new System.Drawing.Size(270, 24);
            this.autoScaleButton.Text = "Auto Scale";
            this.autoScaleButton.Click += new System.EventHandler(this.autoScaleButton_Click);
            // 
            // setHorizontalScaleToolStripMenuItem
            // 
            this.setHorizontalScaleToolStripMenuItem.Name = "setHorizontalScaleToolStripMenuItem";
            this.setHorizontalScaleToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.setHorizontalScaleToolStripMenuItem.Text = "Set Horizontal Scale";
            this.setHorizontalScaleToolStripMenuItem.Click += new System.EventHandler(this.setHorizontalScaleToolStripMenuItem_Click);
            // 
            // setVerticalScaleToolStripMenuItem
            // 
            this.setVerticalScaleToolStripMenuItem.Name = "setVerticalScaleToolStripMenuItem";
            this.setVerticalScaleToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.setVerticalScaleToolStripMenuItem.Text = "Set Vertical Scale";
            this.setVerticalScaleToolStripMenuItem.Click += new System.EventHandler(this.setVerticalScaleToolStripMenuItem_Click);
            // 
            // WaveFrontErrorPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 553);
            this.Controls.Add(this.toolStripContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "WaveFrontErrorPlot";
            this.Text = "WaveFrontErrorPlot";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem copyToBitmap;
        private System.Windows.Forms.ToolStripMenuItem savePNGFile;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton informationButton;
        private System.Windows.Forms.ToolStripMenuItem copyDataToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem setVerticalScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setHorizontalScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoScaleButton;
    }
}