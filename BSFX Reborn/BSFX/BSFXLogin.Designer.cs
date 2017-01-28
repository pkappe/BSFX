namespace BSFX
{
	partial class BSFXLogin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BSFXLogin));
			this.userNameBox = new System.Windows.Forms.TextBox();
			this.userPwdBox = new System.Windows.Forms.TextBox();
			this.logIn = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// userNameBox
			// 
			this.userNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.userNameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.userNameBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.userNameBox.ForeColor = System.Drawing.Color.White;
			this.userNameBox.Location = new System.Drawing.Point(76, 95);
			this.userNameBox.Margin = new System.Windows.Forms.Padding(5);
			this.userNameBox.Name = "userNameBox";
			this.userNameBox.Size = new System.Drawing.Size(173, 16);
			this.userNameBox.TabIndex = 1;
			this.userNameBox.Text = "pkappe";
			this.userNameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// userPwdBox
			// 
			this.userPwdBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.userPwdBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.userPwdBox.Font = new System.Drawing.Font("Consolas", 10F);
			this.userPwdBox.ForeColor = System.Drawing.Color.White;
			this.userPwdBox.Location = new System.Drawing.Point(76, 119);
			this.userPwdBox.Name = "userPwdBox";
			this.userPwdBox.PasswordChar = '*';
			this.userPwdBox.Size = new System.Drawing.Size(173, 16);
			this.userPwdBox.TabIndex = 2;
			this.userPwdBox.Text = "061185pk";
			this.userPwdBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// logIn
			// 
			this.logIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.logIn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.logIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.logIn.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logIn.ForeColor = System.Drawing.Color.White;
			this.logIn.Location = new System.Drawing.Point(168, 150);
			this.logIn.Name = "logIn";
			this.logIn.Size = new System.Drawing.Size(81, 35);
			this.logIn.TabIndex = 3;
			this.logIn.Text = "Login";
			this.logIn.UseVisualStyleBackColor = false;
			this.logIn.Click += new System.EventHandler(this.logIn_Click);
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.White;
			this.button1.Location = new System.Drawing.Point(86, 163);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(66, 22);
			this.button1.TabIndex = 4;
			this.button1.Text = "Exit";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(86, 141);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(77, 17);
			this.checkBox1.TabIndex = 10056;
			this.checkBox1.Text = "Remember";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBox1.Image = global::BSFX.Properties.Resources.BSFX1;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(264, 82);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::BSFX.Properties.Resources.computer_diagnostic;
			this.pictureBox2.Location = new System.Drawing.Point(4, 99);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(68, 79);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 10057;
			this.pictureBox2.TabStop = false;
			// 
			// BSFXLogin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.ClientSize = new System.Drawing.Size(264, 195);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.logIn);
			this.Controls.Add(this.userPwdBox);
			this.Controls.Add(this.userNameBox);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BSFXLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BSFXLogin";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BSFXLogin_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox userNameBox;
		private System.Windows.Forms.TextBox userPwdBox;
		private System.Windows.Forms.Button logIn;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
	}
}