using BSFX.Properties;
using fxcore2;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// Values and Parameter changes events
		// Change Move
		private void moveBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (moveBox.Text != "" && e.KeyCode == Keys.Enter)
			{
				try
				{
					buyLong = Convert.ToDecimal(moveBox.Text) / 10000;
					sellShort = (buyLong * -1);
					Settings.Default.buyLong = buyLong;
					Settings.Default.sellShort = sellShort;
					Settings.Default.Save();
					highestMove = buyLong;
					actiBox.AppendText("Buy Long changed: " + buyLong + Environment.NewLine);
					actiBox.AppendText("Sell Short changed: " + sellShort + Environment.NewLine);
					actiBox.AppendText("MOVE: " + Convert.ToString(buyLong) + " Pips" + Environment.NewLine);
				}
				catch (Exception moveErr)
				{
					MessageBox.Show("Unable to change move parameter.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine(moveErr);
				}
			}
		}

		// Goal Parameter
		private void goalBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (goalBox.Text != "" && e.KeyCode == Keys.Enter)
			{
				try
				{
					goalLong = Convert.ToDecimal(goalBox.Text) / 10000;
					goalShort = (goalLong * -1);
					Settings.Default.goalLong = goalLong;
					Settings.Default.goalShort = goalShort;
					Settings.Default.Save();
					actiBox.AppendText("goalLong changed: " + goalLong + Environment.NewLine);
					actiBox.AppendText("goalShort changed: " + goalShort + Environment.NewLine);
					actiBox.AppendText("GOAL: " + goalLong + " Pips" + Environment.NewLine);
				}
				catch (Exception goalErr)
				{
					MessageBox.Show("Unable to change goal parameter.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine(goalErr);
					actiBox.AppendText("ERROR: " + goalErr + Environment.NewLine);
				}
			}
		}

		private void stopLossBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (stopLossBox.Text != "" && e.KeyCode == Keys.Enter)
			{
				try
				{
					stopLoss = Convert.ToDecimal(stopLossBox.Text) / 10000;
					Settings.Default.stopLoss = stopLoss;
					Settings.Default.Save();
					actiBox.AppendText(" stopLoss changed: " + stopLoss + Environment.NewLine);
					actiBox.AppendText("STOP LOSS: " + stopLossBox.Text + " Pips" + Environment.NewLine);
				}
				catch (Exception paramErr)
				{
					actiBox.AppendText("ERROR: " + paramErr + Environment.NewLine);
					MessageBox.Show("Unable to change stop loss parameter.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine(paramErr);
				}
			}

		}

		// Max Spread Parameter
		private void maxSpreadBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (maxSpreadBox.Text != "" && e.KeyCode == Keys.Enter)
			{
				try
				{
					maxSpread = Convert.ToDecimal(maxSpreadBox.Text) / 10000;
					Settings.Default.maxSpread = maxSpread;
					Settings.Default.Save();
					actiBox.AppendText("maxSpread changed: " + maxSpread + Environment.NewLine);
					actiBox.AppendText("Max Spread: " + maxSpread + Environment.NewLine + Environment.NewLine);
				}
				catch (Exception spreadErr)
				{
					MessageBox.Show("Unable to change max spread parameter.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine(spreadErr);
				}
			}
		}

		// Lot Size Parameter
		private void lotSizeBox_KeyDown(object sender, KeyEventArgs e)
		{

		}

		// Time Interval Parameter Changed Event
		private void intervalBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Time Interval Parameter

			try
			{
				// Check the interval int and change the text box based on that interval
				if (intervalBox.SelectedIndex == 0)
				{					
					pastTimer.Interval = 5000;
					Properties.Settings.Default.interval = 0;
					actiBox.AppendText("Interval Changed: " + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 1)
				{
					pastTimer.Interval = 15000;
					Properties.Settings.Default.interval = 1;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 2)
				{
					pastTimer.Interval = 30000;
					Properties.Settings.Default.interval = 2;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 3)
				{
					pastTimer.Interval = 45000;
					Properties.Settings.Default.interval = 3;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 4)
				{
					pastTimer.Interval = 60000;
					Properties.Settings.Default.interval = 4;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 5)
				{
					pastTimer.Interval = 120000;
					Properties.Settings.Default.interval = 5;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 6)
				{
					pastTimer.Interval = 180000;
					Properties.Settings.Default.interval = 6;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 7)
				{
					pastTimer.Interval = 240000;
					Properties.Settings.Default.interval = 7;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 8)
				{
					pastTimer.Interval = 300000;
					Properties.Settings.Default.interval = 8;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 9)
				{
					pastTimer.Interval = 600000;
					Properties.Settings.Default.interval = 9;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 10)
				{
					pastTimer.Interval = 900000;
					Properties.Settings.Default.interval = 10;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 11)
				{
					pastTimer.Interval = 1800000;
					Properties.Settings.Default.interval = 11;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 12)
				{
					pastTimer.Interval = 3600000;
					Properties.Settings.Default.interval = 12;
					actiBox.AppendText("Interval Changed: " + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 13)
				{
					pastTimer.Interval = 7200000;
					Properties.Settings.Default.interval = 13;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 14)
				{
					pastTimer.Interval = 10800000;
					Properties.Settings.Default.interval = 14;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 15)
				{
					pastTimer.Interval = 14400000;
					Properties.Settings.Default.interval = 15;
					actiBox.AppendText("Interval Changed: "  + pastTimer.Interval + "ms" + Environment.NewLine);
					Properties.Settings.Default.Save();
					pastTimer.Start();
					oneSecTimer.Start();
				}
				else if (intervalBox.SelectedIndex == 16)
				{
					Properties.Settings.Default.interval = 16;
					oneSecTimer.Stop();
					pastTimer.Stop();
					intervalLeftBox.Text = "00h:00m:00s";
					actiBox.AppendText("PAST TIMER STOPPED" + Environment.NewLine);
					Properties.Settings.Default.Save();
				}
				else
				{
					pastTimer.Interval = 60000;
					actiBox.AppendText("CAUTION: Could not read Time Interval. Setting default to 1 minute." + Environment.NewLine);
					actiBox.AppendText("Could not read Time Interval. Setting default to 1 minute." + Environment.NewLine);
				}
				intervalLeft = pastTimer.Interval / 1000;
			}
			catch (Exception spreadInterval)
			{
				Console.WriteLine(spreadInterval);
			}
			actiBox.AppendText("Interval: " + Settings.Default.interval + Environment.NewLine);
		}

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

		// Main form references
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			pastTimer.Stop();
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
				// ENTRY Column
				audcadSide.Text = "--";
				audjpySide.Text = "--";
				audusdSide.Text = "--";
				cadjpySide.Text = "--";
				chfjpySide.Text = "--";
				euraudSide.Text = "--";
				eurcadSide.Text = "--";
				eurchfSide.Text = "--";
				eurjpySide.Text = "--";
				eurusdSide.Text = "--";
				gbpaudSide.Text = "--";
				gbpchfSide.Text = "--";
				gbpjpySide.Text = "--";
				gbpnzdSide.Text = "--";
				gbpusdSide.Text = "--";
				nzdjpySide.Text = "--";
				nzdusdSide.Text = "--";
				usdcadSide.Text = "--";
				usdchfSide.Text = "--";
				usdjpySide.Text = "--";

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

				// SIDE Column
				audcadEntry.Text = "--";
				audjpyEntry.Text = "--";
				audusdEntry.Text = "--";
				cadjpyEntry.Text = "--";
				chfjpyEntry.Text = "--";
				euraudEntry.Text = "--";
				eurcadEntry.Text = "--";
				eurchfEntry.Text = "--";
				eurjpyEntry.Text = "--";
				eurusdEntry.Text = "--";
				gbpaudEntry.Text = "--";
				gbpchfEntry.Text = "--";
				gbpjpyEntry.Text = "--";
				gbpnzdEntry.Text = "--";
				gbpusdEntry.Text = "--";
				nzdjpyEntry.Text = "--";
				nzdusdEntry.Text = "--";
				usdcadEntry.Text = "--";
				usdchfEntry.Text = "--";
				usdjpyEntry.Text = "--";

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

		private void autoButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (availLev == 0)
				{
					MessageBox.Show("Your account balance is $0. An account balance is required to determine lot size automatically.", "Auto Lot Size", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else if (availLev > 0 && availLev < 2000)
				{
					lotSize = 1000;
				}
				else if (availLev > 2000 && availLev < 3000)
				{
					lotSize = 2000;
				}
				else if (availLev > 3000 && availLev < 5000)
				{
					lotSize = 5000;
				}
				else if (availLev > 5000 && availLev < 15000)
				{
					lotSize = 10000;
				}
				else if (availLev > 15000 && availLev < 20000)
				{
					lotSize = 20000;
				}
				else if (availLev > 20000 && availLev < 30000)
				{
					lotSize = 30000;
				}
				else if (availLev > 30000 && availLev < 40000)
				{
					lotSize = 40000;
				}
				else if (availLev > 40000 && availLev < 50000)
				{
					lotSize = 60000;
				}
				else if (availLev > 50000 && availLev < 60000)
				{
					lotSize = 70000;
				}
				else if (availLev > 50000 && availLev < 60000)
				{
					lotSize = 70000;
				}
				else if (availLev > 60000 && availLev < 70000)
				{
					lotSize = 80000;
				}
				else if (availLev > 70000 && availLev < 80000)
				{
					lotSize = 100000;
				}
				else if (availLev > 80000 && availLev < 90000)
				{
					lotSize = 110000;
				}
				else if (availLev > 90000 && availLev < 100000)
				{
					lotSize = 120000;
				}
				// 9/12/14 If balance is above 100k, just set the lot size to 200k.
				// Need to expand this in the future.
				else if (availLev > 100000)
				{
					lotSize = 200000;
				}
			}
			catch (Exception autoErr)
			{
				Console.WriteLine(autoErr);
			}
			finally
			{
				if (availLev > 0)
				{
					try
					{
						DialogResult autoLot = MessageBox.Show("Your account balance is " + availLev + ". Based on these available funds, BSFX recommends your lot size be:" + Environment.NewLine + Environment.NewLine + lotSize + Environment.NewLine + Environment.NewLine + "Would you like to set this as your lot size now?", "Auto Lot Size", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

						if (autoLot == DialogResult.Yes)
						{
							lotSizeBox.Text = Convert.ToString(lotSize);
							actiBox.AppendText("AUTO LOT: " + Convert.ToString(lotSize));
							Settings.Default.lotSize = lotSize;
							Settings.Default.Save();
						}
						else if (autoLot == DialogResult.No)
						{
							lotSize = Settings.Default.lotSize;
						}
					}
					catch (Exception autoErr)
					{
						Console.WriteLine(autoErr);
					}
				}
			}
		}

		public O2GResponse response;
		public DateTime today;

		//private void weekPastToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (weekPastToolStripMenuItem.CheckState == CheckState.Checked)
		//	{
		//		try
		//		{
		//			DateTime todaysDate = DateTime.Today;
		//			DateTime dt = new DateTime();
		//			string dayOfTheWeek = dt.DayOfWeek.ToString();

		//			switch (dayOfTheWeek)
		//			{
		//				case "Sunday":
		//					today = DateTime.Today;
		//					break;
		//				case "Monday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(1));
		//					break;
		//				case "Tuesday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(2));
		//					break;
		//				case "Wednesday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(3));
		//					break;
		//				case "Thursday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(4));
		//					break;
		//				case "Friday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(5));
		//					break;
		//				case "Saturday":
		//					today = DateTime.Today.Subtract(TimeSpan.FromDays(6));
		//					break;
		//			}
		//			Console.WriteLine(today);
		//			Console.WriteLine(todaysDate);
		//			GetHistoryPrices(today, todaysDate);
		//		}
		//		catch (Exception weekErr)
		//		{
		//			Console.WriteLine(weekErr);
		//		}
		//	}
		//}

		private void lotSizeBox_TextChanged(object sender, EventArgs e)
		{
			if (lotSizeBox.Text != "")
			{
				try
				{
					lotSize = Convert.ToInt64(lotSizeBox.Text);
					Settings.Default.lotSize = lotSize;
					Settings.Default.Save();
					actiBox.AppendText(" lotSize: " + lotSize + Environment.NewLine);
					actiBox.AppendText("Lot Size: " + lotSizeBox.Text + Environment.NewLine + Environment.NewLine);

					audcadInitial = shortLot * audcadMMR;
					audjpyInitial = shortLot * audjpyMMR;
					audusdInitial = shortLot * audusdMMR;
					cadjpyInitial = shortLot * cadjpyMMR;
					chfjpyInitial = shortLot * chfjpyMMR;
					euraudInitial = shortLot * euraudMMR;
					eurcadInitial = shortLot * eurcadMMR;
					eurchfInitial = shortLot * eurchfMMR;
					eurjpyInitial = shortLot * eurjpyMMR;
					eurusdInitial = shortLot * eurusdMMR;
					gbpaudInitial = shortLot * gbpaudMMR;
					gbpchfInitial = shortLot * gbpchfMMR;
					gbpjpyInitial = shortLot * gbpjpyMMR;
					gbpnzdInitial = shortLot * gbpnzdMMR;
					gbpusdInitial = shortLot * gbpusdMMR;
					nzdjpyInitial = shortLot * nzdjpyMMR;
					nzdusdInitial = shortLot * nzdusdMMR;
					usdcadInitial = shortLot * usdcadMMR;
					usdchfInitial = shortLot * usdchfMMR;
					usdjpyInitial = shortLot * usdjpyMMR;
					actiBox.AppendText(" Pair MMR's Stored...." + Environment.NewLine);
				}
				catch (Exception lotErr)
				{
					Console.WriteLine(lotErr);
					MessageBox.Show("Unable to change lot size parameter.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					actiBox.AppendText("ERROR: " + lotErr + Environment.NewLine);
				}
			}
		}

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

		// 100 Move | 50 Goal | 4 Hours
		private void move50Goal4HoursToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				actiBox.AppendText(" PRESETS: 100 Move | 50 Goal | 4 Hours" + Environment.NewLine);
				// Move
				moveBox.Text = "100";
				buyLong = (decimal)0.0100;
				sellShort = (decimal)(-0.0100);
				// Goal
				goalBox.Text = "50";
				goalLong = (decimal)0.0050;
				goalShort = (decimal)(-0.0050);
				// Hours
				pastTimer.Interval = 14400000;
				intervalBox.Text = "4 Hours";
			}
			catch (Exception oneHundyErr)
			{
				Console.WriteLine(oneHundyErr);
			}
		}

		// 250 Move | 100 Goal | None
		private void move5Goal3MinutesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				actiBox.AppendText(" PRESETS: 250 Move | 100 Goal | None" + Environment.NewLine);
				// Move
				moveBox.Text = "250";
				buyLong = (decimal)0.0250;
				sellShort = (decimal)(-0.0250);
				// Goal
				goalBox.Text = "5";
				goalLong = (decimal)0.0100;
				goalShort = (decimal)(-0.0100);
				// Minutes
				pastTimer.Stop();
				intervalBox.Text = "NONE";
				intervalBox.SelectedIndex = 16;
			}
			catch (Exception snap15Err)
			{
				Console.WriteLine(snap15Err);
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

		private void enableStopLossToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			if (enableStopLossToolStripMenuItem.CheckState == CheckState.Checked)
			{
				try
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Stop Loss Enabled!" + Environment.NewLine); }));
					this.Invoke(new MethodInvoker(delegate { stopLossBox.Enabled = true; }));
					this.Invoke(new MethodInvoker(delegate { stopLoss = 1; }));
					this.Invoke(new MethodInvoker(delegate { stopLossBox.Text = Convert.ToString(stopLoss/100); }));
				}
				catch (Exception eStopLossErr)
				{
					Console.WriteLine(eStopLossErr);		
				}						
			}
			else if (enableStopLossToolStripMenuItem.CheckState == CheckState.Unchecked)
			{
				try
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Stop Loss Disabled!" + Environment.NewLine); }));
					this.Invoke(new MethodInvoker(delegate { stopLossBox.Enabled = false; }));
					this.Invoke(new MethodInvoker(delegate { stopLoss = 1; }));
					this.Invoke(new MethodInvoker(delegate { stopLossBox.Text = "--"; }));
				}
				catch (Exception)
				{
					
					throw;
				}
			}
		}

		
	}
}