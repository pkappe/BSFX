using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ForxtraSetup
{	
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		public void installButton_Click(object sender, EventArgs e)
		{
			try
			{
				actiBox.AppendText("Install Button Clicked..." + Environment.NewLine);
				actiBox.AppendText("Triggering background worker..." + Environment.NewLine);
				installBw.RunWorkerAsync();				
			}
			catch (Exception installErr)
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(installErr + Environment.NewLine); }));
			}			
		}

		public void installBw_DoWork(object sender, DoWorkEventArgs e)
		{
			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Triggering Installation Procedure." + Environment.NewLine); }));
			doInstall();			
		}
		
		public void doInstall()
		{
			//Disable Windows Update auto reboot
			try
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Adjusting Updates..." + Environment.NewLine); }));

				Process update = new Process();
				update.StartInfo.Verb = "runas";
				update.StartInfo.Arguments = "/C reg.exe ADD HKEY_LOCAL_MACHINE\\Software\\Policies\\Microsoft\\Windows\\WindowsUpdate\\AU /v NoAutoRebootWithLoggedOnUsers /t REG_DWORD /d 1 /f";
				update.StartInfo.CreateNoWindow = true;
				update.StartInfo.UseShellExecute = false;
				update.StartInfo.FileName = "cmd.exe";
				update.Start();
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Disabling Windows Update Restart Started..." + Environment.NewLine); }));
				update.WaitForExit();
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Disabling Windows Update Restart Finished!" + Environment.NewLine); }));
			}
			catch (Exception eupdate)
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(" Disabling Windows Update Restart Failed - " + eupdate + Environment.NewLine); }));
			}
			try
			{
				// Transfer Metaquotes folder
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Checking for MetaQuotes directory..." + Environment.NewLine); }));
				if (Directory.Exists(Environment.CurrentDirectory + "\\MetaQuotes"))
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("MetaQuotes Directory found!" + Environment.NewLine); }));
					Process settings = new Process();
					settings.StartInfo.Verb = "runas";
					settings.StartInfo.Arguments = "/C xcopy \"" + Environment.CurrentDirectory + "\\MetaQuotes\" \"C:\\Documents and Settings\\Administrator\\Application Data\\MetaQuotes\" /E /I /R /Y";
					settings.StartInfo.CreateNoWindow = true;
					settings.StartInfo.UseShellExecute = false;
					settings.StartInfo.FileName = "cmd.exe";
					settings.Start();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Transferring settings files..." + Environment.NewLine); }));
					settings.WaitForExit();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Settings Transfer Complete!" + Environment.NewLine); }));
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("ERROR: SETTINGS FILE NOT FOUND!!" + Environment.NewLine); }));
				}
				// USG Metatrader installation
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Checking for usgfx4setup.exe file..." + Environment.NewLine); }));
				if (File.Exists(Environment.CurrentDirectory + "\\usgfx4setup.exe"))
				{
					
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("usgfx4setup.exe found!" + Environment.NewLine); }));
					
					Process usgInstall = new Process();
					usgInstall.StartInfo.Verb = "runas";
					usgInstall.StartInfo.Arguments = "/C \"" + Environment.CurrentDirectory + "\\usgfx4setup.exe\"";
					usgInstall.StartInfo.CreateNoWindow = true;
					usgInstall.StartInfo.UseShellExecute = false;
					usgInstall.StartInfo.FileName = "cmd.exe";
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Launching USG MT4 Setup..." + Environment.NewLine); }));
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Follow the instructions to complete the setup..." + Environment.NewLine); }));
					usgInstall.Start();
					usgInstall.WaitForExit();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("USG setup closed..." + Environment.NewLine + Environment.NewLine); }));
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("**SETUP COMPLETE**" + Environment.NewLine); }));
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("ERROR: USG SETUP NOT FOUND!!" + Environment.NewLine); }));
				}			
			}
			catch (Exception bwErr)
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(bwErr + Environment.NewLine); }));
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Checking for previous MetaQuotes directory..." + Environment.NewLine); }));
				if (Directory.Exists("C:\\Documents and Settings\\Administrator\\Application Data\\MetaQuotes"))
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Deleting old MetaQuotes directory..." + Environment.NewLine); }));
					Directory.Delete("C:\\Documents and Settings\\Administrator\\Application Data\\MetaQuotes",true);
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("MetaQuotes directory deleted!" + Environment.NewLine); }));
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("No previous MetaQuotes directory found!" + Environment.NewLine); }));
				}			
			}
			catch (Exception delErr)
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(delErr + Environment.NewLine); }));
			}

			try
			{
				// Transfer Metaquotes folder
				if (Directory.Exists(Environment.CurrentDirectory + "\\MetaQuotes"))
				{
					Process settings = new Process();
					settings.StartInfo.Verb = "runas";
					settings.StartInfo.Arguments = "/C xcopy \"" + Environment.CurrentDirectory + "\\MetaQuotes\" \"C:\\Documents and Settings\\Administrator\\Application Data\\MetaQuotes\" /E /I /R /Y";
					settings.StartInfo.CreateNoWindow = true;
					settings.StartInfo.UseShellExecute = false;
					settings.StartInfo.FileName = "cmd.exe";
					settings.Start();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Transferring settings files..." + Environment.NewLine); }));
					settings.WaitForExit();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Settings Transfer Complete!" + Environment.NewLine); }));
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("ERROR: SETTINGS FILE NOT FOUND!!" + Environment.NewLine); }));
				}
			}
			catch (Exception upErr)
			{
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(upErr + Environment.NewLine); }));
			}
		}
	}
}
