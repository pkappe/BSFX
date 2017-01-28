using BSFX.Properties;
using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using fxcore2;

namespace BSFX
{
	/// <summary>
	/// This is the main form thread. 
	/// - Initialize form.
	/// - Run through stored parameters and set them. 
	/// - Form closing protocol.
	/// </summary>
	public partial class BSFX : Form
	{
		public BSFX()
		{
			InitializeComponent();
			paramSetter();			
		}
		
		// Parameter setting class from stored settings
		public void paramSetter()
		{			
			// Sound settter
			try
			{
				openSound = new SoundPlayer("Sounds\\PosOpened.wav");
				closedSound = new SoundPlayer("Sounds\\PosClosed.wav");
			}
			catch (Exception soundErr)
			{
				Console.WriteLine(soundErr);
				actiBox.AppendText("ERROR: " + soundErr + Environment.NewLine);
			}
			// UserID and Password Parameter
			try
			{
				// Change user name and password box text based on settings file
				userIDbox.Text = Properties.Settings.Default.sUserID;
				passwordBox.Text = Properties.Settings.Default.sPassword;
			}
			catch (Exception userErr)
			{
				Console.WriteLine(userErr);
				actiBox.AppendText("ERROR: " + userErr + Environment.NewLine);
			}
			try
			{
				if (Settings.Default.hedgeLong == true)
				{
					longCheckBox.Checked = true;
					hedgingLong = true;
				}
				if (Settings.Default.hedgeShort == true)
				{
					shortCheckBox.Checked = true;
					hedgingShort = true;
				}
			}
			catch (Exception hedgeErr)
			{
				Console.WriteLine(hedgeErr);
			}
			this.Activate();
		}

		private void BSFX_FormClosing(object sender, FormClosingEventArgs e)
		{			
			SessionStatusListener statusListener = new SessionStatusListener(mSession);
			// Check for connected session and take action before application close
			try
			{				
				priceBW.CancelAsync();				
				// Confirm Connected
				if (statusListener.Connected)
				{
					sessionTextBox.Text = "Disconnecting";
					mSession.logout();
					Thread.Sleep(5000);
					mSession.Dispose();
					Application.Exit();
				}

				// Any other status than connected, close the application
				else
				{
					actiBox.AppendText("CAUTION: NO SESSION DETECTED.. CLOSING BSFX" + Environment.NewLine);
					Application.Exit();
				}
			}
			catch (Exception closeErr)
			{
				Console.WriteLine(closeErr);
			}
		}

		private void BSFX_Load(object sender, EventArgs e)
		{
			try
			{
				audcadJustTraded = false;
				audjpyJustTraded = false;
				audusdJustTraded = false;
				cadjpyJustTraded = false;
				chfjpyJustTraded = false;
				euraudJustTraded = false;
				eurcadJustTraded = false;
				eurchfJustTraded = false;
				eurjpyJustTraded = false;
				eurusdJustTraded = false;
				gbpaudJustTraded = false;
				gbpchfJustTraded = false;
				gbpjpyJustTraded = false;
				gbpnzdJustTraded = false;
				gbpusdJustTraded = false;
				nzdjpyJustTraded = false;
				nzdusdJustTraded = false;
				usdcadJustTraded = false;
				usdjpyJustTraded = false;
			}
			catch (Exception justErr)
			{
				Console.WriteLine(justErr);
			}
		}

		private void marginLevRestrictBar_Scroll(object sender, EventArgs e)
		{
			if (marginLevRestrictBar.Value == 0)
			{
				
			}
		}				
	}
}
