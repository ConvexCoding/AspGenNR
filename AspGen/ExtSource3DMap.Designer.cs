
namespace AspGen
{
    partial class ExtSource3DMap
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pix = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.condenseData2X = new System.Windows.Forms.ToolStripMenuItem();
            this.fermiDiracFitTButton = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDataToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClip = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFormToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.crossSectionPlot = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.copyFitDataToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pix)).BeginInit();
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(630, 453);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(630, 504);
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
            this.statusStrip1.Size = new System.Drawing.Size(630, 26);
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
            this.panel1.Controls.Add(this.pix);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20);
            this.panel1.Size = new System.Drawing.Size(630, 453);
            this.panel1.TabIndex = 0;
            // 
            // pix
            // 
            this.pix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pix.Location = new System.Drawing.Point(20, 20);
            this.pix.Name = "pix";
            this.pix.Size = new System.Drawing.Size(590, 413);
            this.pix.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pix.TabIndex = 0;
            this.pix.TabStop = false;
            this.pix.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pix_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(271, 22);
            this.toolStripLabel1.Text = "Right Mouse Click Over Plot for Options";
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.condenseData2X,
            this.fermiDiracFitTButton,
            this.copyDataToClipboard,
            this.copyFitDataToClipboardToolStripMenuItem,
            this.copyToClip,
            this.copyFormToClipboard,
            this.saveToFile,
            this.crossSectionPlot});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(269, 224);
            // 
            // condenseData2X
            // 
            this.condenseData2X.Name = "condenseData2X";
            this.condenseData2X.Size = new System.Drawing.Size(268, 24);
            this.condenseData2X.Text = "Condense 2X";
            this.condenseData2X.Click += new System.EventHandler(this.condenseData2X_Click);
            // 
            // fermiDiracFitTButton
            // 
            this.fermiDiracFitTButton.Name = "fermiDiracFitTButton";
            this.fermiDiracFitTButton.Size = new System.Drawing.Size(268, 24);
            this.fermiDiracFitTButton.Text = "Fermi Dirac Fit";
            this.fermiDiracFitTButton.Click += new System.EventHandler(this.fermiDiracFitTButton_Click);
            // 
            // copyDataToClipboard
            // 
            this.copyDataToClipboard.Name = "copyDataToClipboard";
            this.copyDataToClipboard.Size = new System.Drawing.Size(268, 24);
            this.copyDataToClipboard.Text = "Copy Raw Data to Clipboard";
            this.copyDataToClipboard.Click += new System.EventHandler(this.copyDataToClipboardToolStripMenuItem_Click);
            // 
            // copyToClip
            // 
            this.copyToClip.Name = "copyToClip";
            this.copyToClip.Size = new System.Drawing.Size(268, 24);
            this.copyToClip.Text = "Copy Graphic to Clipboard";
            this.copyToClip.Click += new System.EventHandler(this.copyToClip_Click);
            // 
            // copyFormToClipboard
            // 
            this.copyFormToClipboard.Name = "copyFormToClipboard";
            this.copyFormToClipboard.Size = new System.Drawing.Size(268, 24);
            this.copyFormToClipboard.Text = "Copy Form to Clipboard";
            this.copyFormToClipboard.Click += new System.EventHandler(this.copyFormClipboard_Click);
            // 
            // saveToFile
            // 
            this.saveToFile.Name = "saveToFile";
            this.saveToFile.Size = new System.Drawing.Size(268, 24);
            this.saveToFile.Text = "Save BitmapTo File";
            this.saveToFile.Click += new System.EventHandler(this.saveToFile_Click);
            // 
            // crossSectionPlot
            // 
            this.crossSectionPlot.Name = "crossSectionPlot";
            this.crossSectionPlot.Size = new System.Drawing.Size(268, 24);
            this.crossSectionPlot.Text = "Generate Cross Section Plot";
            this.crossSectionPlot.Click += new System.EventHandler(this.crossSectionPlot_Click);
            // 
            // copyFitDataToClipboardToolStripMenuItem
            // 
            this.copyFitDataToClipboardToolStripMenuItem.Name = "copyFitDataToClipboardToolStripMenuItem";
            this.copyFitDataToClipboardToolStripMenuItem.Size = new System.Drawing.Size(268, 24);
            this.copyFitDataToClipboardToolStripMenuItem.Text = "Copy Fit Data to Clipboard";
            this.copyFitDataToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyFitDataToClipboardToolStripMenuItem_Click);
            // 
            // ExtSource3DMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 504);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ExtSource3DMap";
            this.Text = "ExtSource3DMap";
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
            ((System.ComponentModel.ISupportInitialize)(this.pix)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pix;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem copyToClip;
        private System.Windows.Forms.ToolStripMenuItem saveToFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem condenseData2X;
        private System.Windows.Forms.ToolStripMenuItem copyDataToClipboard;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timerLabel;
        private System.Windows.Forms.ToolStripMenuItem copyFormToClipboard;
        private System.Windows.Forms.ToolStripMenuItem crossSectionPlot;
        private System.Windows.Forms.ToolStripMenuItem fermiDiracFitTButton;
        private System.Windows.Forms.ToolStripMenuItem copyFitDataToClipboardToolStripMenuItem;
    }
}