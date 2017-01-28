using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// Account table changed event
		private void accountsTable_RowChanged(object sender, RowEventArgs e)
		{
			// O2GAccountTableRow designator
			O2GAccountTableRow accountTableRow = (O2GAccountTableRow)e.RowData;
			// If the account table row dows not equal null
			if (accountTableRow != null)
			{
				// Fill in any information changed from account table RowEventArg
				try
				{
					// Day P/L
					dayPL = Math.Round(accountTableRow.DayPL, 2);
					this.Invoke(new MethodInvoker(delegate { dayPLBox.Text = "$" + Convert.ToString(dayPL); }));
					// Unrealized P/L
					unrealPL = Math.Round(accountTableRow.GrossPL, 2);
					this.Invoke(new MethodInvoker(delegate { unrealBox.Text = "$" + Convert.ToString(unrealPL); }));
					// Account Value
					accountValue = accountTableRow.Balance;
					this.Invoke(new MethodInvoker(delegate { accountValueBox.Text = "$" + Convert.ToString(accountValue); }));
					// Available Funds
					acctEq = accountTableRow.UsableMargin;
					this.Invoke(new MethodInvoker(delegate { accountLevBox.Text = "$" + Convert.ToString(acctEq); }));
					this.Invoke(new MethodInvoker(delegate { accountEquityBox.Text = "$" + Convert.ToString(acctEq); }));
					// Allocated Funds
					acctAllocated = accountTableRow.UsedMargin;
					this.Invoke(new MethodInvoker(delegate { allocatedFundsBox.Text = "$" + Convert.ToString(acctAllocated); }));
					// Account type (to determine hedging privilages)
					
					// Margin Level %
					if (acctAllocated > 0)
					{
						marginLev = Math.Round((acctEq / acctAllocated) * 100, 2);
						string marginLevString = Convert.ToString(marginLev);					
						this.Invoke(new MethodInvoker(delegate { marginLevBox.Text = marginLevString; }));
						this.Invoke(new MethodInvoker(delegate { marginLevBoxTop.Text = marginLevString + "%"; }));
					}										
				}
				catch (Exception accountError)
				{
					Console.WriteLine(accountError);
				}
			}
		}
	}
}
