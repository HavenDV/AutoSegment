namespace AutoSegment
{
    partial class Learn
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
            this.button_Close = new System.Windows.Forms.Button();
            this.groupBox_Teacher = new System.Windows.Forms.GroupBox();
            this.label_NbofTeachers = new System.Windows.Forms.Label();
            this.label_NbofFiles = new System.Windows.Forms.Label();
            this.button_Learn = new System.Windows.Forms.Button();
            this.label_Processed = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_Clear = new System.Windows.Forms.Button();
            this.groupBox_Errors = new System.Windows.Forms.GroupBox();
            this.label_MaxError = new System.Windows.Forms.Label();
            this.label_OverallError = new System.Windows.Forms.Label();
            this.groupBox_Learning = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_MaxError = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Convergence = new System.Windows.Forms.NumericUpDown();
            this.label_Convergence = new System.Windows.Forms.Label();
            this.groupBox_Teacher.SuspendLayout();
            this.groupBox_Errors.SuspendLayout();
            this.groupBox_Learning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Convergence)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(217, 303);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(63, 23);
            this.button_Close.TabIndex = 0;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // groupBox_Teacher
            // 
            this.groupBox_Teacher.Controls.Add(this.label_NbofTeachers);
            this.groupBox_Teacher.Controls.Add(this.label_NbofFiles);
            this.groupBox_Teacher.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Teacher.Name = "groupBox_Teacher";
            this.groupBox_Teacher.Size = new System.Drawing.Size(268, 62);
            this.groupBox_Teacher.TabIndex = 1;
            this.groupBox_Teacher.TabStop = false;
            this.groupBox_Teacher.Text = "Teacher";
            // 
            // label_NbofTeachers
            // 
            this.label_NbofTeachers.AutoSize = true;
            this.label_NbofTeachers.Location = new System.Drawing.Point(40, 38);
            this.label_NbofTeachers.Name = "label_NbofTeachers";
            this.label_NbofTeachers.Size = new System.Drawing.Size(116, 13);
            this.label_NbofTeachers.TabIndex = 8;
            this.label_NbofTeachers.Text = "<Number of Teachers>";
            // 
            // label_NbofFiles
            // 
            this.label_NbofFiles.AutoSize = true;
            this.label_NbofFiles.Location = new System.Drawing.Point(40, 19);
            this.label_NbofFiles.Name = "label_NbofFiles";
            this.label_NbofFiles.Size = new System.Drawing.Size(92, 13);
            this.label_NbofFiles.TabIndex = 7;
            this.label_NbofFiles.Text = "<Number of Files>";
            // 
            // button_Learn
            // 
            this.button_Learn.BackColor = System.Drawing.Color.PaleGreen;
            this.button_Learn.Location = new System.Drawing.Point(188, 96);
            this.button_Learn.Name = "button_Learn";
            this.button_Learn.Size = new System.Drawing.Size(63, 23);
            this.button_Learn.TabIndex = 2;
            this.button_Learn.Text = "Learn!";
            this.button_Learn.UseVisualStyleBackColor = false;
            this.button_Learn.Click += new System.EventHandler(this.button_Learn_Click);
            // 
            // label_Processed
            // 
            this.label_Processed.AutoSize = true;
            this.label_Processed.Location = new System.Drawing.Point(37, 311);
            this.label_Processed.Name = "label_Processed";
            this.label_Processed.Size = new System.Drawing.Size(60, 13);
            this.label_Processed.TabIndex = 3;
            this.label_Processed.Text = "Processed:";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(111, 96);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(63, 23);
            this.button_Clear.TabIndex = 4;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // groupBox_Errors
            // 
            this.groupBox_Errors.Controls.Add(this.label_MaxError);
            this.groupBox_Errors.Controls.Add(this.label_OverallError);
            this.groupBox_Errors.Location = new System.Drawing.Point(12, 83);
            this.groupBox_Errors.Name = "groupBox_Errors";
            this.groupBox_Errors.Size = new System.Drawing.Size(268, 62);
            this.groupBox_Errors.TabIndex = 5;
            this.groupBox_Errors.TabStop = false;
            this.groupBox_Errors.Text = "Errors";
            // 
            // label_MaxError
            // 
            this.label_MaxError.AutoSize = true;
            this.label_MaxError.Location = new System.Drawing.Point(143, 28);
            this.label_MaxError.Name = "label_MaxError";
            this.label_MaxError.Size = new System.Drawing.Size(54, 13);
            this.label_MaxError.TabIndex = 5;
            this.label_MaxError.Text = "Maximum:";
            // 
            // label_OverallError
            // 
            this.label_OverallError.AutoSize = true;
            this.label_OverallError.Location = new System.Drawing.Point(23, 28);
            this.label_OverallError.Name = "label_OverallError";
            this.label_OverallError.Size = new System.Drawing.Size(43, 13);
            this.label_OverallError.TabIndex = 4;
            this.label_OverallError.Text = "Overall:";
            // 
            // groupBox_Learning
            // 
            this.groupBox_Learning.Controls.Add(this.label1);
            this.groupBox_Learning.Controls.Add(this.numericUpDown_MaxError);
            this.groupBox_Learning.Controls.Add(this.numericUpDown_Convergence);
            this.groupBox_Learning.Controls.Add(this.label_Convergence);
            this.groupBox_Learning.Controls.Add(this.button_Learn);
            this.groupBox_Learning.Controls.Add(this.button_Clear);
            this.groupBox_Learning.Location = new System.Drawing.Point(15, 155);
            this.groupBox_Learning.Name = "groupBox_Learning";
            this.groupBox_Learning.Size = new System.Drawing.Size(265, 136);
            this.groupBox_Learning.TabIndex = 6;
            this.groupBox_Learning.TabStop = false;
            this.groupBox_Learning.Text = "Learning";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Learn, if the maximum of error  >";
            // 
            // numericUpDown_MaxError
            // 
            this.numericUpDown_MaxError.DecimalPlaces = 2;
            this.numericUpDown_MaxError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_MaxError.Location = new System.Drawing.Point(202, 28);
            this.numericUpDown_MaxError.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.numericUpDown_MaxError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_MaxError.Name = "numericUpDown_MaxError";
            this.numericUpDown_MaxError.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown_MaxError.TabIndex = 8;
            this.numericUpDown_MaxError.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            // 
            // numericUpDown_Convergence
            // 
            this.numericUpDown_Convergence.DecimalPlaces = 2;
            this.numericUpDown_Convergence.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_Convergence.Location = new System.Drawing.Point(202, 60);
            this.numericUpDown_Convergence.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.numericUpDown_Convergence.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_Convergence.Name = "numericUpDown_Convergence";
            this.numericUpDown_Convergence.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown_Convergence.TabIndex = 6;
            this.numericUpDown_Convergence.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.numericUpDown_Convergence.ValueChanged += new System.EventHandler(this.numericUpDown_Convergence_ValueChanged);
            // 
            // label_Convergence
            // 
            this.label_Convergence.AutoSize = true;
            this.label_Convergence.Location = new System.Drawing.Point(39, 63);
            this.label_Convergence.Name = "label_Convergence";
            this.label_Convergence.Size = new System.Drawing.Size(161, 13);
            this.label_Convergence.TabIndex = 5;
            this.label_Convergence.Text = "Convergence (:precise >>> fast):";
            // 
            // Learn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 341);
            this.Controls.Add(this.groupBox_Learning);
            this.Controls.Add(this.groupBox_Errors);
            this.Controls.Add(this.groupBox_Teacher);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.label_Processed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Learn";
            this.Text = "Learn";
            this.Shown += new System.EventHandler(this.Learn_Shown);
            this.groupBox_Teacher.ResumeLayout(false);
            this.groupBox_Teacher.PerformLayout();
            this.groupBox_Errors.ResumeLayout(false);
            this.groupBox_Errors.PerformLayout();
            this.groupBox_Learning.ResumeLayout(false);
            this.groupBox_Learning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Convergence)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.GroupBox groupBox_Teacher;
        private System.Windows.Forms.Button button_Learn;
        private System.Windows.Forms.Label label_Processed;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.GroupBox groupBox_Errors;
        private System.Windows.Forms.Label label_MaxError;
        private System.Windows.Forms.Label label_OverallError;
        private System.Windows.Forms.GroupBox groupBox_Learning;
        private System.Windows.Forms.Label label_Convergence;
        private System.Windows.Forms.NumericUpDown numericUpDown_Convergence;
        private System.Windows.Forms.NumericUpDown numericUpDown_MaxError;
        private System.Windows.Forms.Label label_NbofFiles;
        private System.Windows.Forms.Label label_NbofTeachers;
        private System.Windows.Forms.Label label1;
    }
}