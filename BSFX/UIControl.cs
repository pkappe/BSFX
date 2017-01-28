using BSFX.Properties;
using fxcore2;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class  BSFX : Form
	{
		// Practice account radio button
		private void accountPractice_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				actiBox.AppendText("Demo account radio button changed." + Environment.NewLine);
				if (accountPractice.Checked == true)
				{
					accountLive.Checked = false;
					sConnection = "Demo";
				}
				else
				{
					accountLive.Checked = true;
					sConnection = "Real";
				}
			}
			catch (Exception pracErr)
			{
				Console.WriteLine(pracErr);
			}
		}

		// Live account radio button
		private void accountLive_CheckedChanged(object sender, EventArgs e)
		{
			// Check which button is active and store the bool, while deactivating the other radio button
			try
			{
				actiBox.AppendText("Real account radio button changed." + Environment.NewLine);
				if (accountLive.Checked == true)
				{
					accountPractice.Checked = false;
					sConnection = "Real";
				}
				else
				{
					accountPractice.Checked = true;
					sConnection = "Demo";
				}
			}
			catch (Exception liveErr)
			{
				Console.WriteLine(liveErr);
			}
		}	

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SessionStatusListener statusListener = new SessionStatusListener(mSession);
			// Check for connected session and take action before application close
			try
			{
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
					actiBox.AppendText("CAUTION: NO SESSION DETECTED" + Environment.NewLine);
					actiBox.AppendText("NO SESSION DETECTED" + Environment.NewLine);
					Application.Exit();
				}
			}
			catch (Exception closeErr)
			{
				Console.WriteLine(closeErr);
			}
		}

		private void refreshTheGridToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// LOT SIZE Column
				audcadPosSize.Text = "--";
				audjpyPosSize.Text = "--";
				audusdPosSize.Text = "--";
				cadjpyPosSize.Text = "--";
				chfjpyPosSize.Text = "--";
				euraudPosSize.Text = "--";
				eurcadPosSize.Text = "--";
				eurchfPosSize.Text = "--";
				eurjpyPosSize.Text = "--";
				eurusdPosSize.Text = "--";
				gbpaudPosSize.Text = "--";
				gbpchfPosSize.Text = "--";
				gbpjpyPosSize.Text = "--";
				gbpnzdPosSize.Text = "--";
				gbpusdPosSize.Text = "--";
				nzdjpyPosSize.Text = "--";
				nzdusdPosSize.Text = "--";
				usdcadPosSize.Text = "--";
				usdchfPosSize.Text = "--";
				usdjpyPosSize.Text = "--";				

				// P/L Column
				audcadPL.Text = "--";
				audjpyPL.Text = "--";
				audusdPL.Text = "--";
				cadjpyPL.Text = "--";
				chfjpyPL.Text = "--";
				euraudPL.Text = "--";
				eurcadPL.Text = "--";
				eurchfPL.Text = "--";
				eurjpyPL.Text = "--";
				eurusdPL.Text = "--";
				gbpaudPL.Text = "--";
				gbpchfPL.Text = "--";
				gbpjpyPL.Text = "--";
				gbpnzdPL.Text = "--";
				gbpusdPL.Text = "--";
				nzdjpyPL.Text = "--";
				nzdusdPL.Text = "--";
				usdcadPL.Text = "--";
				usdchfPL.Text = "--";
				usdjpyPL.Text = "--";
			}
			catch (Exception refErr)
			{
				Console.WriteLine(refErr);
			}
		}		

		public O2GResponse response;
		public DateTime today;		

		private void passwordBox_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Enter)
				{
					fxcmLogin.PerformClick();
				}
			}
			catch (Exception keyErr)
			{
				Console.WriteLine(keyErr);
			}
		}		

		// Subscribe to all pairs
		private void subscribeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			actiBox.AppendText("Subscribing Pairs..." + Environment.NewLine);
			try
			{
				sOfferID = "1";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "2";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "3";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "4";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "5";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "6";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "7";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "8";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "10";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "11";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "12";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "13";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "14";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "15";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "16";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "17";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "18";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "19";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "21";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				Thread.Sleep(500);
				sOfferID = "22";
				sStatus = "T";
				CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
				actiBox.AppendText("Subscribing Complete.");
			}
			catch (Exception subErr)
			{
				Console.WriteLine(subErr);
			}

			actiBox.AppendText("Subscription Successful!" + Environment.NewLine);
		}

		// Unsubscribe from all pairs
		private void unsubscribeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				actiBox.AppendText("Unsubscribing all pairs..." + Environment.NewLine);
				//this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(""); }));
				int unSubInt = 1;
				for (int i = 0; i < 30; i++)
				{
					sOfferID = Convert.ToString(unSubInt);
					sStatus = "D";
					actiBox.AppendText(sOfferID + ", ");
					CreateSetSubscriptionStatusRequest(sOfferID, sStatus);
					unSubInt++;
				}
				Thread.Sleep(500);
			}
			catch (Exception subErr)
			{
				Console.WriteLine(subErr);
			}
			actiBox.AppendText("Unsub Successful!" + Environment.NewLine);
		}

		// Margin Level Percentage Slider
		private void marginLevRestrictBar_Scroll(object sender, EventArgs e)
		{
			switch (marginLevRestrictBar.Value)
			{
				case 0:
					restrictBuy = 10;
					break;
				case 1:
					restrictBuy = 12.5;
					break;
				case 2:
					restrictBuy = 15;
					break;
				case 3:
					restrictBuy = 17.5;
					break;
				case 4:
					restrictBuy = 20;
					break;
				case 5:
					restrictBuy = 22.5;
					break;
				case 6:
					restrictBuy = 25;
					break;
				case 7:
					restrictBuy = 27.5;
					break;
				case 8:
					restrictBuy = 30;
					break;
				case 9:
					restrictBuy = 32.5;
					break;
				case 10:
					restrictBuy = 35;
					break;
				default:
					restrictBuy = 10;
					break;
			}
			Console.WriteLine("Restricting Buy Percentage: " + restrictBuy);
		}
	}
}