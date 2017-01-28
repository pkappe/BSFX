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
			//try
			//{				
			//	intervalLeft = pastTimer.Interval / 1000;

			//	time = DateTime.Now.ToString("hh:mm:ss tt");				
			//	date = DateTime.Now.ToString("M-d");				
			//	appLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);				
			//	if (appLocation.StartsWith(@"file:\"))
			//	{
			//		appLocation = appLocation.Substring(6);
			//	}
			//	logLoc = appLocation + @"\" + @"Logs\" + date + ".txt";
			//	File.AppendAllText(logLoc, time + ":" + " BSFX INITIALIZED..." + Environment.NewLine);
			//	File.AppendAllText(logLoc, time + ":" + " Time String Set" + Environment.NewLine);
			//	File.AppendAllText(logLoc, time + ":" + " Date String Set" + Environment.NewLine);
			//	File.AppendAllText(logLoc, time + ":" + " BSFX Directory: " + appLocation + Environment.NewLine);
			//	File.AppendAllText(logLoc, time + ":" + " Log Directory: " + logLoc + Environment.NewLine);
				
			////}
			//catch (Exception logErr)
			//{
			//	Console.WriteLine(logErr);
				
			//}
			// Sound settter
			try
			{
				openSound = new SoundPlayer("Sounds\\PosOpened.wav");
				closedSound = new SoundPlayer("Sounds\\PosClosed.wav");
			}
			catch (Exception soundErr)
			{
				Console.WriteLine(soundErr);				
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
			}

			// Move Parameter
			try
			{
				int moveInt = Convert.ToInt16(Properties.Settings.Default.buyLong * 10000);				
				moveBox.Text = Convert.ToString(moveInt);
				buyLong = Settings.Default.buyLong;				
				sellShort = Settings.Default.sellShort;				
			}
			catch (Exception errMove)
			{
				Console.WriteLine(errMove);				
			}

			// Goal Parameter
			try
			{
				int goalInt = Convert.ToInt16(Properties.Settings.Default.goalLong * 10000);				
				goalBox.Text = Convert.ToString(goalInt);
				goalLong = Settings.Default.goalLong;				
				goalShort = Settings.Default.goalShort;				
			}
			catch (Exception errGoal)
			{
				Console.WriteLine(errGoal);
			}

			// Max Spread Parameter
			try
			{
				int spreadInt = Convert.ToInt16(Settings.Default.maxSpread * 10000);				
				maxSpreadBox.Text = Convert.ToString(spreadInt);
				maxSpread = Settings.Default.maxSpread;
			}
			catch (Exception errmaxSpread)
			{
				Console.WriteLine(errmaxSpread);
			}			

			// Lot Size Parameter
			try
			{
				lotSizeBox.Text = Convert.ToString(Settings.Default.lotSize);
				lotSize = Settings.Default.lotSize;
				shortLot = lotSize / 1000;
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
				stopLoss = Settings.Default.stopLoss;
			}
			catch (Exception paramErr)
			{
				Console.WriteLine(paramErr);
			}

			//// Volatility Restriction
			//try
			//{
			//	volRestrictBox.Text = Settings.Default.volValue.ToString(); ;
			//}
			//catch (Exception volErr)
			//{
			//	Console.WriteLine(volErr);
			//	File.AppendAllText(logLoc, time + ":" + " ERROR: " + volErr + Environment.NewLine);
			//}

			try
			{
				intervalBox.Text = Settings.Default.interval;
				if (Properties.Settings.Default.interval == "5 Seconds")
				{
					pastTimer.Interval = 5000;
					intervalBox.SelectedIndex = 0;
				}
				else if (Properties.Settings.Default.interval == "15 Seconds")
				{
					pastTimer.Interval = 15000;
					intervalBox.SelectedIndex = 1;
				}
				else if (Properties.Settings.Default.interval == "30 Seconds")
				{
					pastTimer.Interval = 30000;
					intervalBox.SelectedIndex = 2;
				}
				else if (Properties.Settings.Default.interval == "45 Seconds")
				{
					pastTimer.Interval = 45000;
					intervalBox.SelectedIndex = 3;
				}
				else if (Properties.Settings.Default.interval == "1 Minute")
				{
					pastTimer.Interval = 60000;
					intervalBox.SelectedIndex = 4;
				}
				else if (Properties.Settings.Default.interval == "2 Minutes")
				{
					pastTimer.Interval = 120000;
					intervalBox.SelectedIndex = 5;
				}
				else if (Properties.Settings.Default.interval == "3 Minutes")
				{
					pastTimer.Interval = 180000;
					intervalBox.SelectedIndex = 6;
				}
				else if (Properties.Settings.Default.interval == "4 Minutes")
				{
					pastTimer.Interval = 240000;
					intervalBox.SelectedIndex = 7;
				}
				else if (Properties.Settings.Default.interval == "5 Minutes")
				{
					pastTimer.Interval = 300000;
					intervalBox.SelectedIndex = 8;
				}
				else if (Properties.Settings.Default.interval == "10 Minutes")
				{
					pastTimer.Interval = 600000;
					intervalBox.SelectedIndex = 9;					
				}
				else if (Properties.Settings.Default.interval == "15 Minutes")
				{
					pastTimer.Interval = 900000;
					intervalBox.SelectedIndex = 10;					
				}
				else if (Properties.Settings.Default.interval == "30 Minutes")
				{
					pastTimer.Interval = 1800000;
					intervalBox.SelectedIndex = 11;					
				}
				else if (Properties.Settings.Default.interval == "1 Hour")
				{
					pastTimer.Interval = 3600000;
					intervalBox.SelectedIndex = 12;					
				}
				else if (Properties.Settings.Default.interval == "2 Hours")
				{
					pastTimer.Interval = 7200000;
					intervalBox.SelectedIndex = 13;					
				}
				else if (Properties.Settings.Default.interval == "3 Hours")
				{
					pastTimer.Interval = 10800000;
					intervalBox.SelectedIndex = 14;					
				}
				else if (Properties.Settings.Default.interval == "4 Hours")
				{
					pastTimer.Interval = 14400000;
					intervalBox.SelectedIndex = 15;
				}
				else if (Properties.Settings.Default.interval == "NONE")
				{
					oneSecTimer.Stop();
					pastTimer.Stop();
					intervalLeftBox.Text = "00h:00m:00s";
					actiBox.AppendText("PAST TIMER STOPPED" + Environment.NewLine);
				}
				else
				{
					pastTimer.Interval = 60000;
					Properties.Settings.Default.interval = "1 Minute";
					intervalBox.Text = "1 Minute";
				}
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
					this.Dispose();
				}

				// Any other status than connected, close the application
				else
				{
					actiBox.AppendText("NO SESSION DETECTED" + Environment.NewLine);
					Application.Exit();
					this.Dispose();
				}
			}
			catch (Exception closeErr)
			{
				Console.WriteLine(closeErr);
				Application.Exit();				
			}
		}		
	}
}
