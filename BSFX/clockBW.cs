using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		//Clock in the middle of the window
		private void clockBW_DoWork(object clock, DoWorkEventArgs tTick)
		{
			try
			{
				oneSecTimer.Start();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
			}
		}

		public DateTime localPubTime { get; set; }
		public DateTime nysePubTime { get; set; }
		public DateTime lsePubTime { get; set; }
		public DateTime sixPubTime { get; set; }
		public DateTime nikkeiPubTime { get; set; }
		public DateTime asxPubTime { get; set; }

		public DayOfWeek localPubDay { get; set; }
		public DayOfWeek nysePubDay { get; set; }
		public DayOfWeek lsePubDay { get; set; }
		public DayOfWeek sixPubDay { get; set; }
		public DayOfWeek nikkeiPubDay { get; set; }
		public DayOfWeek asxPubDay { get; set; }

		// World Clocks on left
		private void oneSecTimer_Tick(object clock, EventArgs tTick)
		{
			try
			{				
				// NYSE - New York, U.S.A.
				nyseTime.Text = DateTime.UtcNow.AddHours(-4).ToShortTimeString();
				nysePubTime = DateTime.UtcNow.AddHours(-4);
				nysePubDay = DateTime.UtcNow.AddHours(-4).DayOfWeek;

				// LSE - London, England
				lseTime.Text = DateTime.UtcNow.AddHours(1).ToShortTimeString();
				lsePubTime = DateTime.UtcNow.AddHours(1);
				lsePubDay = DateTime.UtcNow.AddHours(1).DayOfWeek;

				// SIX - Zurich, Switzerland
				sixTime.Text = DateTime.UtcNow.AddHours(2).ToShortTimeString();
				sixPubTime = DateTime.UtcNow.AddHours(2);
				sixPubDay = DateTime.UtcNow.AddHours(2).DayOfWeek;			

				//NIKKEI - Tokyo, Japan
				nikkeiTime.Text = DateTime.UtcNow.AddHours(9).ToShortTimeString();
				nikkeiPubTime = DateTime.UtcNow.AddHours(9);
				nikkeiPubDay = DateTime.UtcNow.AddHours(9).DayOfWeek;

				// ASX - Sydney, Australia
				asxTime.Text = DateTime.UtcNow.AddHours(10).ToShortTimeString();
				asxPubTime = DateTime.UtcNow.AddHours(10);
				asxPubDay = DateTime.UtcNow.AddHours(10).DayOfWeek;
			}
			catch (Exception timeErr)
			{
				Console.WriteLine(timeErr);
			}
			
		}
	}
}
