namespace BSFX
{
	partial class Reports
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
			this.fromDatePicker = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.todayCheck = new System.Windows.Forms.CheckBox();
			this.toDatePicker = new System.Windows.Forms.DateTimePicker();
			this.generateButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// fromDatePicker
			// 
			this.fromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.fromDatePicker.Location = new System.Drawing.Point(51, 41);
			this.fromDatePicker.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
			this.fromDatePicker.Name = "fromDatePicker";
			this.fromDatePicker.Size = new System.Drawing.Size(201, 20);
			this.fromDatePicker.TabIndex = 0;
			this.fromDatePicker.Value = new System.DateTime(2014, 7, 1, 0, 0, 0, 0);
			this.fromDatePicker.ValueChanged += new System.EventHandler(this.fromDatePicker_ValueChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label1.Location = new System.Drawing.Point(63, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(142, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select Time Frame of Report";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "From:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label3.Location = new System.Drawing.Point(10, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "To:";
			// 
			// todayCheck
			// 
			this.todayCheck.AutoSize = true;
			this.todayCheck.Checked = true;
			this.todayCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.todayCheck.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.todayCheck.Location = new System.Drawing.Point(196, 79);
			this.todayCheck.Name = "todayCheck";
			this.todayCheck.Size = new System.Drawing.Size(56, 17);
			this.todayCheck.TabIndex = 4;
			this.todayCheck.Text = "Today";
			this.todayCheck.UseVisualStyleBackColor = true;
			this.todayCheck.CheckedChanged += new System.EventHandler(this.todayCheck_CheckedChanged);
			// 
			// toDatePicker
			// 
			this.toDatePicker.Enabled = false;
			this.toDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.toDatePicker.Location = new System.Drawing.Point(51, 74);
			this.toDatePicker.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
			this.toDatePicker.Name = "toDatePicker";
			this.toDatePicker.Size = new System.Drawing.Size(139, 20);
			this.toDatePicker.TabIndex = 5;
			// 
			// generateButton
			// 
			this.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.generateButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.generateButton.Location = new System.Drawing.Point(51, 125);
			this.generateButton.Name = "generateButton";
			this.generateButton.Size = new System.Drawing.Size(75, 23);
			this.generateButton.TabIndex = 6;
			this.generateButton.Text = "Generate";
			this.generateButton.UseVisualStyleBackColor = true;
			this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.cancelButton.Location = new System.Drawing.Point(144, 125);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// Reports
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.ClientSize = new System.Drawing.Size(264, 160);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.generateButton);
			this.Controls.Add(this.toDatePicker);
			this.Controls.Add(this.todayCheck);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.fromDatePicker);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Reports";
			this.Text = "Reports";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker fromDatePicker;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox todayCheck;
		private System.Windows.Forms.DateTimePicker toDatePicker;
		private System.Windows.Forms.Button generateButton;
		private System.Windows.Forms.Button cancelButton;
	}
}