namespace BSFX
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.userIDbox = new System.Windows.Forms.TextBox();
			this.passwordBox = new System.Windows.Forms.TextBox();
			this.actiBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.sessionTextBox = new System.Windows.Forms.TextBox();
			this.logIn = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.textBox7 = new System.Windows.Forms.TextBox();
			this.oneSecTimer = new System.Windows.Forms.Timer(this.components);
			this.textBox8 = new System.Windows.Forms.TextBox();
			this.textBox9 = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.clockBW = new System.ComponentModel.BackgroundWorker();
			this.priceTimerBW = new System.ComponentModel.BackgroundWorker();
			this.CentralClock = new System.Windows.Forms.TextBox();
			this.oneMinTimer = new System.Windows.Forms.Timer(this.components);
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// userIDbox
			// 
			this.userIDbox.Location = new System.Drawing.Point(6, 20);
			this.userIDbox.Name = "userIDbox";
			this.userIDbox.Size = new System.Drawing.Size(100, 20);
			this.userIDbox.TabIndex = 1;
			this.userIDbox.Text = "D172210763001";
			this.userIDbox.TextChanged += new System.EventHandler(this.userIDbox_TextChanged);
			// 
			// passwordBox
			// 
			this.passwordBox.Location = new System.Drawing.Point(6, 46);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.PasswordChar = '*';
			this.passwordBox.Size = new System.Drawing.Size(100, 20);
			this.passwordBox.TabIndex = 2;
			this.passwordBox.Text = "9015";
			this.passwordBox.TextChanged += new System.EventHandler(this.password_TextChanged);
			// 
			// actiBox
			// 
			this.actiBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.actiBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.actiBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.actiBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.actiBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.actiBox.Font = new System.Drawing.Font("Consolas", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.actiBox.ForeColor = System.Drawing.SystemColors.Info;
			this.actiBox.Location = new System.Drawing.Point(0, 313);
			this.actiBox.Multiline = true;
			this.actiBox.Name = "actiBox";
			this.actiBox.ReadOnly = true;
			this.actiBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.actiBox.Size = new System.Drawing.Size(415, 120);
			this.actiBox.TabIndex = 9999;
			this.actiBox.TabStop = false;
			this.actiBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.actiBox.TextChanged += new System.EventHandler(this.actiBox_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.sessionTextBox);
			this.groupBox1.Controls.Add(this.logIn);
			this.groupBox1.Controls.Add(this.userIDbox);
			this.groupBox1.Controls.Add(this.passwordBox);
			this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox1.Location = new System.Drawing.Point(11, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(113, 120);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Session Status";
			// 
			// sessionTextBox
			// 
			this.sessionTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.sessionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.sessionTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.sessionTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sessionTextBox.ForeColor = System.Drawing.Color.White;
			this.sessionTextBox.Location = new System.Drawing.Point(3, 101);
			this.sessionTextBox.Name = "sessionTextBox";
			this.sessionTextBox.Size = new System.Drawing.Size(107, 16);
			this.sessionTextBox.TabIndex = 4;
			this.sessionTextBox.Text = "NOT SIGNED IN";
			this.sessionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.sessionTextBox.TextChanged += new System.EventHandler(this.sessionTextBox_TextChanged);
			// 
			// logIn
			// 
			this.logIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.logIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logIn.Location = new System.Drawing.Point(6, 72);
			this.logIn.Name = "logIn";
			this.logIn.Size = new System.Drawing.Size(100, 23);
			this.logIn.TabIndex = 3;
			this.logIn.Text = "Log In";
			this.logIn.UseVisualStyleBackColor = true;
			this.logIn.Click += new System.EventHandler(this.logIn_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button2);
			this.groupBox2.Controls.Add(this.textBox5);
			this.groupBox2.Controls.Add(this.textBox4);
			this.groupBox2.Controls.Add(this.textBox3);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.comboBox1);
			this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox2.Location = new System.Drawing.Point(132, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(145, 132);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Current Pair";
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(48, 102);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(49, 24);
			this.button2.TabIndex = 5;
			this.button2.Text = "Update";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox5
			// 
			this.textBox5.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox5.Location = new System.Drawing.Point(103, 75);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.ReadOnly = true;
			this.textBox5.Size = new System.Drawing.Size(35, 45);
			this.textBox5.TabIndex = 7;
			this.textBox5.TabStop = false;
			// 
			// textBox4
			// 
			this.textBox4.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox4.Location = new System.Drawing.Point(50, 79);
			this.textBox4.Margin = new System.Windows.Forms.Padding(1);
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			this.textBox4.Size = new System.Drawing.Size(45, 20);
			this.textBox4.TabIndex = 6;
			this.textBox4.TabStop = false;
			// 
			// textBox3
			// 
			this.textBox3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox3.Location = new System.Drawing.Point(7, 75);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(35, 45);
			this.textBox3.TabIndex = 5;
			this.textBox3.TabStop = false;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(51, 59);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(43, 13);
			this.label13.TabIndex = 4;
			this.label13.Text = "Spread";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(108, 59);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(25, 13);
			this.label12.TabIndex = 3;
			this.label12.Text = "Ask";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(11, 59);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(25, 13);
			this.label11.TabIndex = 2;
			this.label11.Text = "Bid";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "GBP/JPY"});
			this.comboBox1.Location = new System.Drawing.Point(7, 20);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(131, 21);
			this.comboBox1.TabIndex = 4;
			this.comboBox1.Text = "GBP/JPY";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(37, 18);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(31, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "12am";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(40, 57);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(25, 13);
			this.label7.TabIndex = 3;
			this.label7.Text = "1am";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(40, 96);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(25, 13);
			this.label8.TabIndex = 4;
			this.label8.Text = "2am";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(40, 135);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(25, 13);
			this.label9.TabIndex = 5;
			this.label9.Text = "3am";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(40, 174);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(25, 13);
			this.label10.TabIndex = 6;
			this.label10.Text = "4am";
			// 
			// textBox6
			// 
			this.textBox6.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox6.Location = new System.Drawing.Point(6, 34);
			this.textBox6.Name = "textBox6";
			this.textBox6.ReadOnly = true;
			this.textBox6.Size = new System.Drawing.Size(100, 20);
			this.textBox6.TabIndex = 25;
			this.textBox6.TabStop = false;
			// 
			// textBox7
			// 
			this.textBox7.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox7.Location = new System.Drawing.Point(6, 73);
			this.textBox7.Name = "textBox7";
			this.textBox7.ReadOnly = true;
			this.textBox7.Size = new System.Drawing.Size(100, 20);
			this.textBox7.TabIndex = 26;
			this.textBox7.TabStop = false;
			// 
			// oneSecTimer
			// 
			this.oneSecTimer.Enabled = true;
			this.oneSecTimer.Interval = 1000;
			this.oneSecTimer.Tick += new System.EventHandler(this.oneSecTimer_Tick);
			// 
			// textBox8
			// 
			this.textBox8.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox8.Location = new System.Drawing.Point(6, 112);
			this.textBox8.Name = "textBox8";
			this.textBox8.ReadOnly = true;
			this.textBox8.Size = new System.Drawing.Size(100, 20);
			this.textBox8.TabIndex = 27;
			this.textBox8.TabStop = false;
			// 
			// textBox9
			// 
			this.textBox9.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox9.Location = new System.Drawing.Point(6, 151);
			this.textBox9.Name = "textBox9";
			this.textBox9.ReadOnly = true;
			this.textBox9.Size = new System.Drawing.Size(100, 20);
			this.textBox9.TabIndex = 28;
			this.textBox9.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.textBox9);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.textBox8);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.textBox7);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.textBox6);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox3.Location = new System.Drawing.Point(283, 40);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(119, 199);
			this.groupBox3.TabIndex = 29;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Time Triggers";
			// 
			// clockBW
			// 
			this.clockBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.clockBW_DoWork);
			// 
			// priceTimerBW
			// 
			this.priceTimerBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.priceTimerBW_DoWork);
			// 
			// CentralClock
			// 
			this.CentralClock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.CentralClock.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CentralClock.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CentralClock.ForeColor = System.Drawing.Color.White;
			this.CentralClock.Location = new System.Drawing.Point(139, 196);
			this.CentralClock.Name = "CentralClock";
			this.CentralClock.Size = new System.Drawing.Size(131, 23);
			this.CentralClock.TabIndex = 30;
			this.CentralClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.CentralClock.TextChanged += new System.EventHandler(this.CentralClock_TextChanged);
			// 
			// oneMinTimer
			// 
			this.oneMinTimer.Tick += new System.EventHandler(this.oneMinTimer_Tick);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.Transparent;
			this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
			this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button3.FlatAppearance.BorderSize = 0;
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.ForeColor = System.Drawing.Color.Transparent;
			this.button3.Location = new System.Drawing.Point(395, 1);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(20, 20);
			this.button3.TabIndex = 10000;
			this.button3.UseVisualStyleBackColor = false;
			// 
			// button4
			// 
			this.button4.BackColor = System.Drawing.Color.Transparent;
			this.button4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button4.BackgroundImage")));
			this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.button4.FlatAppearance.BorderSize = 0;
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.ForeColor = System.Drawing.Color.Transparent;
			this.button4.Location = new System.Drawing.Point(17, 188);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(40, 40);
			this.button4.TabIndex = 10001;
			this.button4.UseVisualStyleBackColor = false;
			// 
			// button5
			// 
			this.button5.BackColor = System.Drawing.Color.Transparent;
			this.button5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button5.BackgroundImage")));
			this.button5.Location = new System.Drawing.Point(78, 196);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 10002;
			this.button5.Text = "button5";
			this.button5.UseVisualStyleBackColor = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.ClientSize = new System.Drawing.Size(415, 433);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.CentralClock);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.actiBox);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "BSFX 1.0";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox userIDbox;
		private System.Windows.Forms.TextBox passwordBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button logIn;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.TextBox textBox7;
		private System.Windows.Forms.Timer oneSecTimer;
		private System.Windows.Forms.TextBox textBox8;
		private System.Windows.Forms.TextBox textBox9;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.ComponentModel.BackgroundWorker priceTimerBW;
		private System.Windows.Forms.TextBox CentralClock;
		private System.Windows.Forms.Timer oneMinTimer;
		private System.Windows.Forms.Button button2;
		public System.ComponentModel.BackgroundWorker clockBW;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.TextBox sessionTextBox;
		public System.Windows.Forms.TextBox actiBox;
	}
}

