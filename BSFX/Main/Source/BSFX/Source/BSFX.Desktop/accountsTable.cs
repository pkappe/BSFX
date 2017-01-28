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
					dayPL = accountTableRow.DayPL;
					this.Invoke(new MethodInvoker(delegate { netPL.Text = "$" + Convert.ToString(dayPL); }));
					// Unrealized P/L
					unrealPL = accountTableRow.GrossPL;
					this.Invoke(new MethodInvoker(delegate { unrealBox.Text = "$" + Convert.ToString(unrealPL); }));
					// Account Value
					accountValue = accountTableRow.Balance;
					this.Invoke(new MethodInvoker(delegate { accountValueBox.Text = "$" + Convert.ToString(accountValue); }));
					// Available Funds
					availLev = accountTableRow.UsableMargin;
					this.Invoke(new MethodInvoker(delegate { accountLevBox.Text = "$" + Convert.ToString(availLev); }));
					// Allocated Funds
					acctAllocated = accountTableRow.UsedMargin;
					this.Invoke(new MethodInvoker(delegate { allocatedFundsBox.Text = "$" + Convert.ToString(acctAllocated); }));					
					if (sAccountID == null)
					{
						sAccountID = accountTableRow.AccountID;
						Console.WriteLine(sAccountID);
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
