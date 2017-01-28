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
		/// <summary> 11-3-2014
		/// Auto Parameters is a theory related to dynamically changing the following:
		/// 
		/// - Move
		/// - Goal
		/// - Stop Loss
		/// 
		/// The theory is based on the highest move achieved by any of the 20 pairs.
		/// That highest move is then used to calculate the parameters above.
		/// Dividing the highest move by an integer can potentially show amazing results.
		/// For future builds, the automation parameters, should be a form, in an of itself.
		/// </summary>
		
		public decimal autoMoveCalc { get; set; }
		public decimal autoGoalCalc { get; set; }

		// Check status change event
		private void autoPrametersToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			try
			{
				if (autoPrametersToolStripMenuItem.CheckState == CheckState.Checked)
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Auto Parameters Enabled!" + Environment.NewLine); }));
					highestMove = 0;
					autoParam();
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Auto Parameters Disabled!" + Environment.NewLine); }));
					highestMove = 100;
				}
			}
			catch (Exception autoErr)
			{
				Console.WriteLine(autoErr);
			}
		}

		// Automate this shit...
		private void autoParam()
		{
			// Check if the tool is enabled
			if (autoPrametersToolStripMenuItem.CheckState == CheckState.Checked)
			{
				try
				{
					// If the current move parameter is less than the highest move achieved...
					if (buyLong < highestMove)
					{
						// BUYLONG						
						// Set the new move to the highest move so far
						buyLong = highestMove;
						// Convert to pips...
						decimal blTenK = buyLong * 10000;
						// COnvert to string...
						string blString = Convert.ToString(blTenK);
						// Declare the completed buyLong string in pips...
						string blStringComplete;
						// Trim it...
						if (blString.Contains("."))
						{
							int index = blString.IndexOf(".") + 2;
							blStringComplete = blString.Substring(0, index);
						}
						else
						{
							blStringComplete = blString;
						}
						// Place it into the text box.
						this.Invoke(new MethodInvoker(delegate { moveBox.Text = blStringComplete; }));

						// SELL SHORT
						// Convert highest move into a negative number for selling short trigger
						sellShort = (highestMove) * -1;

						// GOAL LONG
						// Divide sellShort by 4 to get the move
						goalLong = highestMove / 4;
						// Convert GoalLong to pips
						decimal glTenK = goalLong * 10000;
						// Convert the GoalLong decimal into a string
						string glString = Convert.ToString(glTenK);
						// Declare the completed goalLong
						string glStringComplete;
						// Trim it
						if (glString.Contains("."))
						{
							int index = glString.IndexOf(".") + 2;
							glStringComplete = glString.Substring(0, index);
						}
						else
						{
							glStringComplete = glString;
						}
						// Place it into the text box
						this.Invoke(new MethodInvoker(delegate { goalBox.Text = glStringComplete; }));

						// GOAL SHORT
						goalShort = (highestMove / 4) * -1;

						// STOP LOSS
						// If stop loss is enabled
						if (enableStopLossToolStripMenuItem.CheckState == CheckState.Checked)
						{
							// Divide highest move by 2
							stopLoss = highestMove / 2;
							// Convert stopLoss to pips
							decimal slTenK = stopLoss * 10000;
							// Convert new pip value to a string
							string slString = Convert.ToString(slTenK);
							// Declare completed stopLoss
							string slStringComplete;
							if (slString.Contains("."))
							{
								int index = slString.IndexOf(".") + 2;
								slStringComplete = slString.Substring(0, index);
							}
							else
							{
								slStringComplete = slString;
							}
							// Place it into the text box
							this.Invoke(new MethodInvoker(delegate { stopLossBox.Text = slStringComplete; }));
						}
							// If stop loss is not enabled
						else if (enableStopLossToolStripMenuItem.CheckState == CheckState.Unchecked)
						{
							this.Invoke(new MethodInvoker(delegate { stopLossBox.Enabled = false; }));
							this.Invoke(new MethodInvoker(delegate { stopLoss = 1; }));
							this.Invoke(new MethodInvoker(delegate { stopLossBox.Text = "--"; }));
						}
					}
				}
				catch (Exception highErr)
				{
					Console.WriteLine(highErr);
				}
			}
			else
			{
				highestMove = 1;
			}
		}


	}
}
