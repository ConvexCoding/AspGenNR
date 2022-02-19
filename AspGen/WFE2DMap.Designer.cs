
namespace AspGen
{
    partial class WFE2DMap
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pix = new System.Windows.Forms.PictureBox();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFormButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToCSVFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.savePupilApodMapToCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCrossSectionalPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pix)).BeginInit();
            this.cms.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pix);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 496);
            this.panel1.TabIndex = 0;
            // 
            // pix
            // 
            this.pix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pix.Location = new System.Drawing.Point(0, 0);
            this.pix.Margin = new System.Windows.Forms.Padding(4);
            this.pix.Name = "pix";
            this.pix.Size = new System.Drawing.Size(832, 496);
            this.pix.TabIndex = 0;
            this.pix.TabStop = false;
            this.pix.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pix_MouseClick);
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem,
            this.copyFormButton,
            this.saveToFileToolStripMenuItem,
            this.saveDataToCSVFileButton,
            this.savePupilApodMapToCSVFileToolStripMenuItem,
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem,
            this.generateCrossSectionalPlotToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(370, 172);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(369, 24);
            this.copyToClipboardToolStripMenuItem.Text = "Copy Chart To Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // copyFormButton
            // 
            this.copyFormButton.Name = "copyFormButton";
            this.copyFormButton.Size = new System.Drawing.Size(369, 24);
            this.copyFormButton.Text = "Copy Form to Clipboad";
            this.copyFormButton.Click += new System.EventHandler(this.copyFormButton_Click);
            // 
            // saveToFileToolStripMenuItem
            // 
            this.saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            this.saveToFileToolStripMenuItem.Size = new System.Drawing.Size(369, 24);
            this.saveToFileToolStripMenuItem.Text = "Save Chart To File";
            this.saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);
            // 
            // saveDataToCSVFileButton
            // 
            this.saveDataToCSVFileButton.Name = "saveDataToCSVFileButton";
            this.saveDataToCSVFileButton.Size = new System.Drawing.Size(369, 24);
            this.saveDataToCSVFileButton.Text = "Save Data To CSV File";
            this.saveDataToCSVFileButton.Click += new System.EventHandler(this.saveDataToCSVFileButton_Click);
            // 
            // savePupilApodMapToCSVFileToolStripMenuItem
            // 
            this.savePupilApodMapToCSVFileToolStripMenuItem.Name = "savePupilApodMapToCSVFileToolStripMenuItem";
            this.savePupilApodMapToCSVFileToolStripMenuItem.Size = new System.Drawing.Size(369, 24);
            this.savePupilApodMapToCSVFileToolStripMenuItem.Text = "Save Pupil Apod Map to CSV File";
            this.savePupilApodMapToCSVFileToolStripMenuItem.Click += new System.EventHandler(this.savePupilApodMapToCSVFileToolStripMenuItem_Click);
            // 
            // savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem
            // 
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem.Name = "savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem";
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem.Size = new System.Drawing.Size(369, 24);
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem.Text = "Save Pupil Wavefront Phase Map to CSV File";
            this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem.Click += new System.EventHandler(this.savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem_Click);
            // 
            // generateCrossSectionalPlotToolStripMenuItem
            // 
            this.generateCrossSectionalPlotToolStripMenuItem.Name = "generateCrossSectionalPlotToolStripMenuItem";
            this.generateCrossSectionalPlotToolStripMenuItem.Size = new System.Drawing.Size(369, 24);
            this.generateCrossSectionalPlotToolStripMenuItem.Text = "Generate Cross Section Plot";
            this.generateCrossSectionalPlotToolStripMenuItem.Click += new System.EventHandler(this.generateCrossSectionalPlotToolStripMenuItem_Click);
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(832, 496);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(832, 553);
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
            this.statusStrip1.Size = new System.Drawing.Size(832, 26);
            this.statusStrip1.TabIndex = 0;
            // 
            // timerLabel
            // 
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(21, 20);
            this.timerLabel.Text = "**";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 31);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(271, 28);
            this.toolStripLabel1.Text = "Right Mouse Click Over Plot for Options";
            // 
            // WFE2DMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 553);
            this.Controls.Add(this.toolStripContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WFE2DMap";
            this.Text = "WFE2DMap";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pix)).EndInit();
            this.cms.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pix;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem saveDataToCSVFileButton;
        private System.Windows.Forms.ToolStripMenuItem savePupilApodMapToCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePupilWavefrontPhaseMapToCSVFileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timerLabel;
        private System.Windows.Forms.ToolStripMenuItem copyFormButton;
        private System.Windows.Forms.ToolStripMenuItem generateCrossSectionalPlotToolStripMenuItem;
    }
}