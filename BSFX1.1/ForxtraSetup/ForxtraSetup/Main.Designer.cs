namespace ForxtraSetup
{
	partial class Main
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.actiBox = new System.Windows.Forms.TextBox();
			this.installBw = new System.ComponentModel.BackgroundWorker();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.installButton = new System.Windows.Forms.Button();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// actiBox
			// 
			this.actiBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
			this.actiBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.actiBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.actiBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.actiBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.actiBox.Location = new System.Drawing.Point(0, 59);
			this.actiBox.Multiline = true;
			this.actiBox.Name = "actiBox";
			this.actiBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.actiBox.Size = new System.Drawing.Size(348, 306);
			this.actiBox.TabIndex = 2;
			this.actiBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// installBw
			// 
			this.installBw.WorkerReportsProgress = true;
			this.installBw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.installBw_DoWork);
			// 
			// button1
			// 
			this.button1.BackgroundImage = global::ForxtraSetup.Properties.Resources.UpdateButton;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.Dock = System.Windows.Forms.DockStyle.Right;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.ForeColor = System.Drawing.Color.Transparent;
			this.button1.Location = new System.Drawing.Point(248, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 59);
			this.button1.TabIndex = 4;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.BackgroundImage = global::ForxtraSetup.Properties.Resources.USG;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(102, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(146, 59);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// installButton
			// 
			this.installButton.BackgroundImage = global::ForxtraSetup.Properties.Resources.InstallButton2;
			this.installButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.installButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.installButton.Dock = System.Windows.Forms.DockStyle.Left;
			this.installButton.FlatAppearance.BorderSize = 0;
			this.installButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.installButton.ForeColor = System.Drawing.Color.Transparent;
			this.installButton.Location = new System.Drawing.Point(0, 0);
			this.installButton.Name = "installButton";
			this.installButton.Size = new System.Drawing.Size(102, 59);
			this.installButton.TabIndex = 0;
			this.installButton.UseVisualStyleBackColor = true;
			this.installButton.Click += new System.EventHandler(this.installButton_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.ClientSize = new System.Drawing.Size(348, 365);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.installButton);
			this.Controls.Add(this.actiBox);
			this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ForXtra Installer";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox actiBox;
		public System.ComponentModel.BackgroundWorker installBw;
		public System.Windows.Forms.Button installButton;
		private System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
	}
}

