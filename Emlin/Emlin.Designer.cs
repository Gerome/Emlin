namespace Emlin
{
    partial class Emlin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Emlin));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.recordingEnabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // recordingEnabled
            // 
            this.recordingEnabled.AutoSize = true;
            this.recordingEnabled.Checked = true;
            this.recordingEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recordingEnabled.Location = new System.Drawing.Point(12, 12);
            this.recordingEnabled.Name = "recordingEnabled";
            this.recordingEnabled.Size = new System.Drawing.Size(117, 17);
            this.recordingEnabled.TabIndex = 0;
            this.recordingEnabled.Text = "Recording Enabled";
            this.recordingEnabled.UseVisualStyleBackColor = true;
            // 
            // Emlin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.recordingEnabled);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 400);
            this.Name = "Emlin";
            this.Text = "Emlin";
            this.Resize += new System.EventHandler(this.EmlinView_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox recordingEnabled;
    }
}

