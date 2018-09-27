namespace AutoSegment
{
    partial class ZoomWindow
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
            this.pictureBox_Zoom = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Zoom)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Zoom
            // 
            this.pictureBox_Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Zoom.Location = new System.Drawing.Point(2, 2);
            this.pictureBox_Zoom.Name = "pictureBox_Zoom";
            this.pictureBox_Zoom.Size = new System.Drawing.Size(266, 237);
            this.pictureBox_Zoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Zoom.TabIndex = 0;
            this.pictureBox_Zoom.TabStop = false;
            // 
            // ZoomWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 242);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox_Zoom);
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "ZoomWindow";
            this.Text = "Zoom Window   ( 1:1 )";
            this.SizeChanged += new System.EventHandler(this.ZoomWindow_SizeChanged);
            this.MouseLeave += new System.EventHandler(this.ZoomWindow_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Zoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox_Zoom;
    }
}