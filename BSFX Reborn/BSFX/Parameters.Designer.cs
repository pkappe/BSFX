namespace BSFX
{
	partial class Parameters
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Parameters));
			this.paramOK = new System.Windows.Forms.Button();
			this.paramCancel = new System.Windows.Forms.Button();
			this.lotGroup = new System.Windows.Forms.GroupBox();
			this.lotSizeBoxParam = new System.Windows.Forms.TextBox();
			this.maxPosGroup = new System.Windows.Forms.GroupBox();
			this.maxPosBoxParam = new System.Windows.Forms.TextBox();
			this.goalGroup = new System.Windows.Forms.GroupBox();
			this.goalBoxParam = new System.Windows.Forms.TextBox();
			this.stopGroup = new System.Windows.Forms.GroupBox();
			this.stopLossBoxParam = new System.Windows.Forms.TextBox();
			this.maxSpreadGroup = new System.Windows.Forms.GroupBox();
			this.maxSpreadBoxParam = new System.Windows.Forms.TextBox();
			this.moveGroup = new System.Windows.Forms.GroupBox();
			this.moveBoxParam = new System.Windows.Forms.TextBox();
			this.intervalGroup = new System.Windows.Forms.GroupBox();
			this.intervalBoxParam = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lotGroup.SuspendLayout();
			this.maxPosGroup.SuspendLayout();
			this.goalGroup.SuspendLayout();
			this.stopGroup.SuspendLayout();
			this.maxSpreadGroup.SuspendLayout();
			this.moveGroup.SuspendLayout();
			this.intervalGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// paramOK
			// 
			this.paramOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.paramOK.Location = new System.Drawing.Point(38, 230);
			this.paramOK.Name = "paramOK";
			this.paramOK.Size = new System.Drawing.Size(75, 23);
			this.paramOK.TabIndex = 0;
			this.paramOK.Text = "OK";
			this.paramOK.UseVisualStyleBackColor = true;
			this.paramOK.Click += new System.EventHandler(this.paramOK_Click);
			// 
			// paramCancel
			// 
			this.paramCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.paramCancel.Location = new System.Drawing.Point(119, 230);
			this.paramCancel.Name = "paramCancel";
			this.paramCancel.Size = new System.Drawing.Size(75, 23);
			this.paramCancel.TabIndex = 1;
			this.paramCancel.Text = "Cancel";
			this.paramCancel.UseVisualStyleBackColor = true;
			this.paramCancel.Click += new System.EventHandler(this.paramCancel_Click);
			// 
			// lotGroup
			// 
			this.lotGroup.Controls.Add(this.lotSizeBoxParam);
			this.lotGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lotGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.lotGroup.Location = new System.Drawing.Point(12, 83);
			this.lotGroup.Name = "lotGroup";
			this.lotGroup.Size = new System.Drawing.Size(101, 28);
			this.lotGroup.TabIndex = 10017;
			this.lotGroup.TabStop = false;
			this.lotGroup.Text = "LOT SIZE";
			// 
			// lotSizeBoxParam
			// 
			this.lotSizeBoxParam.AutoCompleteCustomSource.AddRange(new string[] {
            "1000",
            "10000",
            "20000",
            "30000",
            "40000",
            "50000",
            "100000",
            "150000",
            "200000"});
			this.lotSizeBoxParam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.lotSizeBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.lotSizeBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lotSizeBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lotSizeBoxParam.ForeColor = System.Drawing.Color.White;
			this.lotSizeBoxParam.Location = new System.Drawing.Point(3, 14);
			this.lotSizeBoxParam.Name = "lotSizeBoxParam";
			this.lotSizeBoxParam.Size = new System.Drawing.Size(95, 11);
			this.lotSizeBoxParam.TabIndex = 0;
			this.lotSizeBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.lotSizeBoxParam.TextChanged += new System.EventHandler(this.lotSizeBoxParam_TextChanged);
			// 
			// maxPosGroup
			// 
			this.maxPosGroup.Controls.Add(this.maxPosBoxParam);
			this.maxPosGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.maxPosGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.maxPosGroup.Location = new System.Drawing.Point(12, 117);
			this.maxPosGroup.Name = "maxPosGroup";
			this.maxPosGroup.Size = new System.Drawing.Size(101, 28);
			this.maxPosGroup.TabIndex = 10020;
			this.maxPosGroup.TabStop = false;
			this.maxPosGroup.Text = "MAX POSITIONS";
			// 
			// maxPosBoxParam
			// 
			this.maxPosBoxParam.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
			this.maxPosBoxParam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.maxPosBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.maxPosBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.maxPosBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maxPosBoxParam.ForeColor = System.Drawing.SystemColors.Window;
			this.maxPosBoxParam.Location = new System.Drawing.Point(3, 14);
			this.maxPosBoxParam.Name = "maxPosBoxParam";
			this.maxPosBoxParam.Size = new System.Drawing.Size(95, 11);
			this.maxPosBoxParam.TabIndex = 0;
			this.maxPosBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.maxPosBoxParam.TextChanged += new System.EventHandler(this.maxPosBoxParam_TextChanged);
			// 
			// goalGroup
			// 
			this.goalGroup.Controls.Add(this.goalBoxParam);
			this.goalGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.goalGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.goalGroup.Location = new System.Drawing.Point(119, 83);
			this.goalGroup.Name = "goalGroup";
			this.goalGroup.Size = new System.Drawing.Size(101, 28);
			this.goalGroup.TabIndex = 10018;
			this.goalGroup.TabStop = false;
			this.goalGroup.Text = "GOAL";
			// 
			// goalBoxParam
			// 
			this.goalBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.goalBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.goalBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.goalBoxParam.ForeColor = System.Drawing.SystemColors.Window;
			this.goalBoxParam.Location = new System.Drawing.Point(3, 14);
			this.goalBoxParam.Name = "goalBoxParam";
			this.goalBoxParam.Size = new System.Drawing.Size(95, 11);
			this.goalBoxParam.TabIndex = 0;
			this.goalBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.goalBoxParam.TextChanged += new System.EventHandler(this.goalBoxParam_TextChanged);
			// 
			// stopGroup
			// 
			this.stopGroup.Controls.Add(this.stopLossBoxParam);
			this.stopGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.stopGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.stopGroup.Location = new System.Drawing.Point(119, 117);
			this.stopGroup.Name = "stopGroup";
			this.stopGroup.Size = new System.Drawing.Size(101, 28);
			this.stopGroup.TabIndex = 10022;
			this.stopGroup.TabStop = false;
			this.stopGroup.Text = "STOP LOSS";
			// 
			// stopLossBoxParam
			// 
			this.stopLossBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.stopLossBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.stopLossBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stopLossBoxParam.ForeColor = System.Drawing.Color.White;
			this.stopLossBoxParam.Location = new System.Drawing.Point(3, 14);
			this.stopLossBoxParam.Name = "stopLossBoxParam";
			this.stopLossBoxParam.Size = new System.Drawing.Size(95, 11);
			this.stopLossBoxParam.TabIndex = 0;
			this.stopLossBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.stopLossBoxParam.TextChanged += new System.EventHandler(this.stopLossBoxParam_TextChanged);
			// 
			// maxSpreadGroup
			// 
			this.maxSpreadGroup.Controls.Add(this.maxSpreadBoxParam);
			this.maxSpreadGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.maxSpreadGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.maxSpreadGroup.Location = new System.Drawing.Point(12, 151);
			this.maxSpreadGroup.Name = "maxSpreadGroup";
			this.maxSpreadGroup.Size = new System.Drawing.Size(101, 28);
			this.maxSpreadGroup.TabIndex = 10019;
			this.maxSpreadGroup.TabStop = false;
			this.maxSpreadGroup.Text = "MAX SPREAD";
			// 
			// maxSpreadBoxParam
			// 
			this.maxSpreadBoxParam.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
			this.maxSpreadBoxParam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.maxSpreadBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.maxSpreadBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.maxSpreadBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maxSpreadBoxParam.ForeColor = System.Drawing.Color.White;
			this.maxSpreadBoxParam.Location = new System.Drawing.Point(3, 14);
			this.maxSpreadBoxParam.Name = "maxSpreadBoxParam";
			this.maxSpreadBoxParam.Size = new System.Drawing.Size(95, 11);
			this.maxSpreadBoxParam.TabIndex = 0;
			this.maxSpreadBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.maxSpreadBoxParam.TextChanged += new System.EventHandler(this.maxSpreadBoxParam_TextChanged);
			// 
			// moveGroup
			// 
			this.moveGroup.Controls.Add(this.moveBoxParam);
			this.moveGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.moveGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.moveGroup.Location = new System.Drawing.Point(12, 185);
			this.moveGroup.Name = "moveGroup";
			this.moveGroup.Size = new System.Drawing.Size(101, 28);
			this.moveGroup.TabIndex = 10016;
			this.moveGroup.TabStop = false;
			this.moveGroup.Text = "MOVE";
			// 
			// moveBoxParam
			// 
			this.moveBoxParam.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70"});
			this.moveBoxParam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.moveBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.moveBoxParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.moveBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.moveBoxParam.ForeColor = System.Drawing.Color.White;
			this.moveBoxParam.Location = new System.Drawing.Point(3, 14);
			this.moveBoxParam.Name = "moveBoxParam";
			this.moveBoxParam.Size = new System.Drawing.Size(95, 11);
			this.moveBoxParam.TabIndex = 10002;
			this.moveBoxParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.moveBoxParam.TextChanged += new System.EventHandler(this.moveBoxParam_TextChanged);
			// 
			// intervalGroup
			// 
			this.intervalGroup.Controls.Add(this.intervalBoxParam);
			this.intervalGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.intervalGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.intervalGroup.Location = new System.Drawing.Point(12, 37);
			this.intervalGroup.Name = "intervalGroup";
			this.intervalGroup.Size = new System.Drawing.Size(208, 40);
			this.intervalGroup.TabIndex = 10021;
			this.intervalGroup.TabStop = false;
			this.intervalGroup.Text = "TIME INTERVAL";
			// 
			// intervalBoxParam
			// 
			this.intervalBoxParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
			this.intervalBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.intervalBoxParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.intervalBoxParam.ForeColor = System.Drawing.Color.White;
			this.intervalBoxParam.Items.AddRange(new object[] {
            "5 Seconds",
            "15 Seconds",
            "30 Seconds",
            "45 Seconds",
            "1 Minute",
            "2 Minutes",
            "3 Minutes",
            "4 Minutes",
            "5 Minutes",
            "10 Minutes",
            "15 Minutes",
            "30 Minutes",
            "1 Hour"});
			this.intervalBoxParam.Location = new System.Drawing.Point(3, 14);
			this.intervalBoxParam.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
			this.intervalBoxParam.Name = "intervalBoxParam";
			this.intervalBoxParam.Size = new System.Drawing.Size(202, 20);
			this.intervalBoxParam.TabIndex = 0;
			this.intervalBoxParam.Text = "30 Minutes";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(187, 26);
			this.label1.TabIndex = 10023;
			this.label1.Text = "Please change the desired parameter.\r\nClick \"OK\" or \"Cancel\" when finished.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// Parameters
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.ClientSize = new System.Drawing.Size(232, 265);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lotGroup);
			this.Controls.Add(this.maxPosGroup);
			this.Controls.Add(this.goalGroup);
			this.Controls.Add(this.stopGroup);
			this.Controls.Add(this.maxSpreadGroup);
			this.Controls.Add(this.moveGroup);
			this.Controls.Add(this.intervalGroup);
			this.Controls.Add(this.paramCancel);
			this.Controls.Add(this.paramOK);
			this.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Parameters";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Parameters";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.Parameters_Load);
			this.lotGroup.ResumeLayout(false);
			this.lotGroup.PerformLayout();
			this.maxPosGroup.ResumeLayout(false);
			this.maxPosGroup.PerformLayout();
			this.goalGroup.ResumeLayout(false);
			this.goalGroup.PerformLayout();
			this.stopGroup.ResumeLayout(false);
			this.stopGroup.PerformLayout();
			this.maxSpreadGroup.ResumeLayout(false);
			this.maxSpreadGroup.PerformLayout();
			this.moveGroup.ResumeLayout(false);
			this.moveGroup.PerformLayout();
			this.intervalGroup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button paramCancel;
		private System.Windows.Forms.GroupBox lotGroup;
		private System.Windows.Forms.TextBox lotSizeBoxParam;
		private System.Windows.Forms.GroupBox maxPosGroup;
		private System.Windows.Forms.TextBox maxPosBoxParam;
		private System.Windows.Forms.GroupBox goalGroup;
		private System.Windows.Forms.TextBox goalBoxParam;
		private System.Windows.Forms.GroupBox stopGroup;
		private System.Windows.Forms.TextBox stopLossBoxParam;
		private System.Windows.Forms.GroupBox maxSpreadGroup;
		private System.Windows.Forms.TextBox maxSpreadBoxParam;
		private System.Windows.Forms.GroupBox moveGroup;
		private System.Windows.Forms.TextBox moveBoxParam;
		private System.Windows.Forms.GroupBox intervalGroup;
		private System.Windows.Forms.ComboBox intervalBoxParam;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.Button paramOK;
	}
}