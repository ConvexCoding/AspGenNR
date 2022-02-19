
namespace AspGen
{
    partial class SpotDiagram2D
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.useUniformtGridPtsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useFibonacciPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useHexPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScalePlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPlotScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(582, 542);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(582, 567);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 542);
            this.panel1.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(582, 542);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            this.chart1.Titles.Add(title1);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.toolStripSeparator7});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(222, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(197, 22);
            this.toolStripLabel1.Text = "Right Click Chart for Options";
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboard,
            this.saveToFile,
            this.useUniformtGridPtsToolStripMenuItem,
            this.useFibonacciPointsToolStripMenuItem,
            this.useHexPointsToolStripMenuItem,
            this.autoScalePlotToolStripMenuItem,
            this.setPlotScaleToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(240, 172);
            // 
            // copyToClipboard
            // 
            this.copyToClipboard.Name = "copyToClipboard";
            this.copyToClipboard.Size = new System.Drawing.Size(239, 24);
            this.copyToClipboard.Text = "Copy Chart to Clipboard";
            this.copyToClipboard.Click += new System.EventHandler(this.copyToClipboard_Click);
            // 
            // saveToFile
            // 
            this.saveToFile.Name = "saveToFile";
            this.saveToFile.Size = new System.Drawing.Size(239, 24);
            this.saveToFile.Text = "Save Chart to File";
            this.saveToFile.Click += new System.EventHandler(this.saveToFile_Click);
            // 
            // useUniformtGridPtsToolStripMenuItem
            // 
            this.useUniformtGridPtsToolStripMenuItem.Name = "useUniformtGridPtsToolStripMenuItem";
            this.useUniformtGridPtsToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.useUniformtGridPtsToolStripMenuItem.Text = "Use Uniformt Grid Pts";
            this.useUniformtGridPtsToolStripMenuItem.Click += new System.EventHandler(this.useUniformtGridPtsToolStripMenuItem_Click);
            // 
            // useFibonacciPointsToolStripMenuItem
            // 
            this.useFibonacciPointsToolStripMenuItem.Name = "useFibonacciPointsToolStripMenuItem";
            this.useFibonacciPointsToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.useFibonacciPointsToolStripMenuItem.Text = "Use Fibonacci Points";
            this.useFibonacciPointsToolStripMenuItem.Click += new System.EventHandler(this.useFibonacciPointsToolStripMenuItem_Click);
            // 
            // useHexPointsToolStripMenuItem
            // 
            this.useHexPointsToolStripMenuItem.Name = "useHexPointsToolStripMenuItem";
            this.useHexPointsToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.useHexPointsToolStripMenuItem.Text = "Use Hex Points";
            this.useHexPointsToolStripMenuItem.Click += new System.EventHandler(this.useHexPointsToolStripMenuItem_Click);
            // 
            // autoScalePlotToolStripMenuItem
            // 
            this.autoScalePlotToolStripMenuItem.Name = "autoScalePlotToolStripMenuItem";
            this.autoScalePlotToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.autoScalePlotToolStripMenuItem.Text = "Auto Scale Plot";
            this.autoScalePlotToolStripMenuItem.Click += new System.EventHandler(this.autoScalePlotToolStripMenuItem_Click);
            // 
            // setPlotScaleToolStripMenuItem
            // 
            this.setPlotScaleToolStripMenuItem.Name = "setPlotScaleToolStripMenuItem";
            this.setPlotScaleToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.setPlotScaleToolStripMenuItem.Text = "Set Plot Scale";
            this.setPlotScaleToolStripMenuItem.Click += new System.EventHandler(this.setPlotScaleToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // SpotDiagram2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 567);
            this.Controls.Add(this.toolStripContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SpotDiagram2D";
            this.Text = "SpotDiagram2D";
            this.Resize += new System.EventHandler(this.SpotDiagram2D_Resize);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboard;
        private System.Windows.Forms.ToolStripMenuItem saveToFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem autoScalePlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPlotScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useFibonacciPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useHexPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useUniformtGridPtsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    }
}