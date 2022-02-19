
namespace AspGen
{
    partial class Prompt
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
            this.okButton = new System.Windows.Forms.Button();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.promptBoxLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(45, 116);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(87, 32);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(224, 49);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(100, 22);
            this.valueTextBox.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(268, 116);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 32);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // promptBoxLabel
            // 
            this.promptBoxLabel.BackColor = System.Drawing.SystemColors.Control;
            this.promptBoxLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.promptBoxLabel.Location = new System.Drawing.Point(12, 52);
            this.promptBoxLabel.Name = "promptBoxLabel";
            this.promptBoxLabel.Size = new System.Drawing.Size(206, 15);
            this.promptBoxLabel.TabIndex = 3;
            this.promptBoxLabel.Text = "prompt here";
            this.promptBoxLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Prompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 185);
            this.Controls.Add(this.promptBoxLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.okButton);
            this.Name = "Prompt";
            this.RightToLeftLayout = true;
            this.Text = "Prompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.TextBox promptBoxLabel;
    }
}