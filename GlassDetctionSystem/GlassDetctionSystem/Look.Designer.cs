namespace GlassDetctionSystem
{
    partial class Look
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Look));
            this.LookPicture = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.LookPicture)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LookPicture
            // 
            this.LookPicture.Location = new System.Drawing.Point(23, 14);
            this.LookPicture.Name = "LookPicture";
            this.LookPicture.Size = new System.Drawing.Size(1091, 596);
            this.LookPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LookPicture.TabIndex = 0;
            this.LookPicture.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LookPicture);
            this.panel1.Location = new System.Drawing.Point(3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1143, 610);
            this.panel1.TabIndex = 1;
            // 
            // Look
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1168, 638);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Look";
            this.Text = "图片预览";
            ((System.ComponentModel.ISupportInitialize)(this.LookPicture)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox LookPicture;
        private System.Windows.Forms.Panel panel1;
    }
}