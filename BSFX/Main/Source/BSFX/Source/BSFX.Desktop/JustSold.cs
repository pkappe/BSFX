using System;
using System.IO;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// Pair just sold resets
		// Based on 1-minute intervals to avoid additional monitoring of open positions from trades table update.
		// Will save settings of user information on same interval
		private void justSoldTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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

			// Set property values and save settings
			try
			{
				
				Properties.Settings.Default.buyLong = buyLong;
				Properties.Settings.Default.goalLong = goalLong;
				Properties.Settings.Default.lotSize = lotSize;
				Properties.Settings.Default.maxSpread = maxSpread;
				Properties.Settings.Default.sUserID = sUserID;
				Properties.Settings.Default.sPassword = sPassword;
				Properties.Settings.Default.sConnection = sConnection;
				Properties.Settings.Default.interval = intervalBox.SelectedIndex;
				Properties.Settings.Default.Save();
				
			}
			catch (Exception settingsErr)
			{
				Console.WriteLine(settingsErr);				
			}
		}
	}
}
