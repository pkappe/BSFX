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
			// Log setter
			try
			{				
				intervalLeft = pastTimer.Interval / 1000;				
				
				actiBox.AppendText("BSFX INITIALIZED..." + Environment.NewLine);				
			}
			catch (Exception logErr)
			{
				Console.WriteLine(logErr);

				actiBox.AppendText("ERROR: " + logErr + Environment.NewLine);
			}
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

			// Move Parameter
			try
			{				
				int moveInt = Convert.ToInt16(Properties.Settings.Default.buyLong * 10000);
				actiBox.AppendText("moveInt: " + moveInt + Environment.NewLine);
				moveBox.Text = Convert.ToString(moveInt);
				buyLong = Settings.Default.buyLong;
				actiBox.AppendText("buyLong: " + buyLong + Environment.NewLine);
				sellShort = Settings.Default.sellShort;
				actiBox.AppendText("sellShort: " + sellShort + Environment.NewLine);
			}
			catch (Exception errMove)
			{
				Console.WriteLine(errMove);
				actiBox.AppendText("ERROR: " + errMove + Environment.NewLine);
			}

			// Goal Parameter
			try
			{
				int goalInt = Convert.ToInt16(Properties.Settings.Default.goalLong * 10000);
				actiBox.AppendText("goalInt: " + goalInt + Environment.NewLine);
				goalBox.Text = Convert.ToString(goalInt);
				goalLong = Settings.Default.goalLong;
				actiBox.AppendText("goalLong: " + goalLong + Environment.NewLine);
				goalShort = Settings.Default.goalShort;
				actiBox.AppendText("goalShort: " + goalShort + Environment.NewLine);
			}
			catch (Exception errGoal)
			{
				Console.WriteLine(errGoal);
				actiBox.AppendText("ERROR: " + errGoal + Environment.NewLine);
			}

			// Max Spread Parameter
			try
			{
				int spreadInt = Convert.ToInt16(Settings.Default.maxSpread * 10000);
				actiBox.AppendText("spreadInt: " + spreadInt + Environment.NewLine);
				maxSpreadBox.Text = Convert.ToString(spreadInt);
				actiBox.AppendText("maxSpreadBox: " + maxSpreadBox + Environment.NewLine);
				maxSpread = Settings.Default.maxSpread;
				actiBox.AppendText("maxSpread: " + maxSpread + Environment.NewLine);
			}
			catch (Exception errmaxSpread)
			{
				Console.WriteLine(errmaxSpread);
				actiBox.AppendText("ERROR: " + errmaxSpread + Environment.NewLine);
			}			

			// Lot Size Parameter
			try
			{
				lotSizeBox.Text = Convert.ToString(Settings.Default.lotSize);
				lotSize = Settings.Default.lotSize;
				actiBox.AppendText("lotSize: " + lotSize + Environment.NewLine);
				shortLot = lotSize / 1000;
				actiBox.AppendText("shortLot: " + shortLot + Environment.NewLine);
			}
			catch (Exception lotErr)
			{
				Console.WriteLine(lotErr);
			}

			// Stop Loss Parameter
			try
			{
				int stopInt = Convert.ToInt32(Settings.Default.stopLoss * 10000);
				stopLossBox.Text = Convert.ToString(stopInt);
				actiBox.AppendText("stopInt: " + stopInt + Environment.NewLine);
				stopLoss = Settings.Default.stopLoss;
				actiBox.AppendText("stopLoss: " + stopLoss + Environment.NewLine);
			}
			catch (Exception paramErr)
			{
				Console.WriteLine(paramErr);
				actiBox.AppendText("ERROR: " + paramErr + Environment.NewLine);
			}

			// Interval Setter
			try
			{
				intervalBox.SelectedIndex = Settings.Default.interval;
			}
			catch (Exception tickErr)
			{
				Console.WriteLine(tickErr);
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
				pastTimer.Stop();
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

		// One second timer for Interval Remaining
		private void oneSecTimer_Tick_1(object sender, EventArgs e)
		{
			try
			{
				intervalLeft = intervalLeft - 1;
				TimeSpan t = TimeSpan.FromSeconds(intervalLeft);
				string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
								t.Hours,
								t.Minutes,
								t.Seconds);

				intervalLeftBox.Text = answer;
			}
			catch (Exception oneErr)
			{
				Console.WriteLine(oneErr);
			}
		}

		private void BSFX_Load(object sender, EventArgs e)
		{
			try
			{
				audcadJustSold = false;
				audjpyJustSold = false;
				audusdJustSold = false;
				cadjpyJustSold = false;
				chfjpyJustSold = false;
				euraudJustSold = false;
				eurcadJustSold = false;
				eurchfJustSold = false;
				eurjpyJustSold = false;
				eurusdJustSold = false;
				gbpaudJustSold = false;
				gbpchfJustSold = false;
				gbpjpyJustSold = false;
				gbpnzdJustSold = false;
				gbpusdJustSold = false;
				nzdjpyJustSold = false;
				nzdusdJustSold = false;
				usdcadJustSold = false;
				usdjpyJustSold = false;
			}
			catch (Exception justErr)
			{
				Console.WriteLine(justErr);
			}
		}

				
	}
}
