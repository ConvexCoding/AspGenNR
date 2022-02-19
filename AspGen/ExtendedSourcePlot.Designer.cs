
namespace AspGen
{
    partial class ExtendedSourcePlot
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
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyClipButton = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFormToClip = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.theoryButton = new System.Windows.Forms.ToolStripMenuItem();
            this.smoothNoise = new System.Windows.Forms.ToolStripMenuItem();
            this.fermiButton = new System.Windows.Forms.ToolStripMenuItem();
            this.resetRawData = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScaleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.setHorizontalAxis = new System.Windows.Forms.ToolStripMenuItem();
            this.setVerticalAxisButton = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(682, 549);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title1.Text = "Title";
            title2.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "Title2";
            title2.Text = "Subtitle";
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
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
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(682, 549);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(682, 606);
            this.toolStripContainer1.TabIndex = 1;
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
            this.statusStrip1.Size = new System.Drawing.Size(682, 26);
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
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 549);
            this.panel1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(171, 31);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(158, 28);
            this.toolStripLabel1.Text = "Right Click for Options";
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyClipButton,
            this.copyFormToClip,
            this.copyDataButton,
            this.theoryButton,
            this.smoothNoise,
            this.fermiButton,
            this.resetRawData,
            this.autoScaleButton,
            this.setHorizontalAxis,
            this.setVerticalAxisButton});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(282, 244);
            // 
            // copyClipButton
            // 
            this.copyClipButton.Name = "copyClipButton";
            this.copyClipButton.Size = new System.Drawing.Size(281, 24);
            this.copyClipButton.Text = "Copy Graph To Clipboard";
            this.copyClipButton.Click += new System.EventHandler(this.graphToBitmap_Click);
            // 
            // copyFormToClip
            // 
            this.copyFormToClip.Name = "copyFormToClip";
            this.copyFormToClip.Size = new System.Drawing.Size(281, 24);
            this.copyFormToClip.Text = "Copy Form To Clipboard";
            this.copyFormToClip.Click += new System.EventHandler(this.copyFormToClip_Click);
            // 
            // copyDataButton
            // 
            this.copyDataButton.Name = "copyDataButton";
            this.copyDataButton.Size = new System.Drawing.Size(281, 24);
            this.copyDataButton.Text = "Copy Data to Clipboard";
            this.copyDataButton.Click += new System.EventHandler(this.copyDataToClipboard_Click);
            // 
            // theoryButton
            // 
            this.theoryButton.Name = "theoryButton";
            this.theoryButton.Size = new System.Drawing.Size(281, 24);
            this.theoryButton.Text = "Show Theory";
            this.theoryButton.Click += new System.EventHandler(this.theoryButton_Click);
            // 
            // smoothNoise
            // 
            this.smoothNoise.Name = "smoothNoise";
            this.smoothNoise.Size = new System.Drawing.Size(281, 24);
            this.smoothNoise.Text = "Smooth Noise";
            this.smoothNoise.Click += new System.EventHandler(this.smoothNoise_Click);
            // 
            // fermiButton
            // 
            this.fermiButton.Name = "fermiButton";
            this.fermiButton.Size = new System.Drawing.Size(281, 24);
            this.fermiButton.Text = "Fit to Fermi-Dirac";
            this.fermiButton.Click += new System.EventHandler(this.fermiButton_Click);
            // 
            // resetRawData
            // 
            this.resetRawData.Name = "resetRawData";
            this.resetRawData.Size = new System.Drawing.Size(281, 24);
            this.resetRawData.Text = "Reset Data";
            this.resetRawData.Click += new System.EventHandler(this.resetRawData_Click);
            // 
            // autoScaleButton
            // 
            this.autoScaleButton.Name = "autoScaleButton";
            this.autoScaleButton.Size = new System.Drawing.Size(281, 24);
            this.autoScaleButton.Text = "Auto Scale";
            this.autoScaleButton.Click += new System.EventHandler(this.autoScaleButton_Click);
            // 
            // setHorizontalAxis
            // 
            this.setHorizontalAxis.Name = "setHorizontalAxis";
            this.setHorizontalAxis.Size = new System.Drawing.Size(281, 24);
            this.setHorizontalAxis.Text = "Set Horizontal Axis Parameters";
            this.setHorizontalAxis.Click += new System.EventHandler(this.setHorizontalAxis_Click);
            // 
            // setVerticalAxisButton
            // 
            this.setVerticalAxisButton.Name = "setVerticalAxisButton";
            this.setVerticalAxisButton.Size = new System.Drawing.Size(281, 24);
            this.setVerticalAxisButton.Text = "Set Vertical Axis Parameters";
            this.setVerticalAxisButton.Click += new System.EventHandler(this.setVerticalAxisButton_Click);
            // 
            // ExtendedSourcePlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 606);
            this.Controls.Add(this.toolStripContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ExtendedSourcePlot";
            this.Text = "ExtendedSourcePlot";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timerLabel;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem copyClipButton;
        private System.Windows.Forms.ToolStripMenuItem copyDataButton;
        private System.Windows.Forms.ToolStripMenuItem setVerticalAxisButton;
        private System.Windows.Forms.ToolStripMenuItem setHorizontalAxis;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem autoScaleButton;
        private System.Windows.Forms.ToolStripMenuItem copyFormToClip;
        private System.Windows.Forms.ToolStripMenuItem smoothNoise;
        private System.Windows.Forms.ToolStripMenuItem resetRawData;
        private System.Windows.Forms.ToolStripMenuItem fermiButton;
        private System.Windows.Forms.ToolStripMenuItem theoryButton;
    }
}