using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BSFX;
using fxcore2;

namespace BSFX
{
	public partial class Reports : Form
	{
		public Reports()
		{
			InitializeComponent();
		}

		// Sessions/DateTime's		
		public DateTime toDate { get; set; }
		public DateTime fromDate { get; set; }

		// Generate Button
		// Grab values, and generate report via HTML
		private void generateButton_Click(object sender, EventArgs e)
		{
			BSFX bsfx = new BSFX();
			try
			{
				if (todayCheck.Checked == true)
				{
					toDate = DateTime.Today.Date;
				}
				else
				{
					toDatePicker.Select();
					toDate = toDatePicker.Value;
				}
			}
			catch (Exception todayErr)
			{
				Console.WriteLine(todayErr);
			}

			fromDate = fromDatePicker.Value;

			bsfx.generateReports();
			this.Close();
		}

		// From Date Setter
		private void fromDatePicker_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				fromDate = fromDatePicker.Value;
			}
			catch (Exception fromErr)
			{
				Console.WriteLine(fromErr);
			}
		}

		// Today Check Box Event
		private void todayCheck_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (todayCheck.Checked == true)
				{
					toDatePicker.Enabled = false;
					toDate = DateTime.Today;
				}
				else
				{
					toDatePicker.Enabled = true;
					toDate = fromDatePicker.Value;
				}
			}
			catch (Exception todayErr)
			{
				Console.WriteLine(todayErr);
			}			
		}

		// To Date Setter
		private void toDatePicker_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (todayCheck.Checked == true)
				{					
					toDate = DateTime.Today;
				}
				else
				{
					toDate = fromDatePicker.Value;
				}
			}
			catch (Exception todayErr)
			{
				Console.WriteLine(todayErr);
			}
		}

		// Cancel Button
		private void cancelButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception closeErr)
			{
				Console.WriteLine(closeErr);
			}
		}
	}
}
