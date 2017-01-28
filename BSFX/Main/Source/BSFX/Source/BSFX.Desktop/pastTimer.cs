using System;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// Price Timer for "Past Price"
		private void pastTimer_Tick(object timer, EventArgs timing)
		{
			intervalLeft = pastTimer.Interval / 1000;
			// AUD/CAD
			if (intervalBox.SelectedIndex == 16 && pastTimer.Interval > 0)
			{
				try
				{
					pastTimer.Stop();
					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("PAST TIMER STOPPED"); }));
				}
				catch (Exception zeroErr)
				{
					Console.WriteLine(zeroErr);
				}			
			}
			else
			{
				try
				{
					audcadReEnter = false;
					if (audcadAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { audcadPastBox.Text = audcadAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { audcadPast = Convert.ToDouble(audcadAsk.Text); }));
					}
				}
				catch (Exception pastError)
				{
					Console.WriteLine(pastError);
				}
				// AUD/JPY
				try
				{
					audjpyReEnter = false;
					if (audjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { audjpyPastBox.Text = audjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { audjpyPast = Convert.ToDouble(audjpyAsk.Text); }));
					}
				}
				catch (Exception pastError)
				{
					Console.WriteLine(pastError);
				}
				// AUD/USD
				try
				{
					audusdReEnter = false;
					if (audusdAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { audusdPastBox.Text = audusdAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { audusdPast = Convert.ToDouble(audusdAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// CAD/JPY
				try
				{
					cadjpyReEnter = false;
					if (cadjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { cadjpyPastBox.Text = cadjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { cadjpyPast = Convert.ToDouble(cadjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// CHF/JPY
				try
				{
					chfjpyReEnter = false;
					if (chfjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { chfjpyPastBox.Text = chfjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { chfjpyPast = Convert.ToDouble(chfjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// EUR/AUD
				try
				{
					euraudReEnter = false;
					if (euraudAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { euraudPastBox.Text = euraudAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { euraudPast = Convert.ToDouble(euraudAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// EUR/CAD
				try
				{
					eurcadReEnter = false;
					if (eurcadAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { eurcadPastBox.Text = eurcadAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { eurcadPast = Convert.ToDouble(eurcadAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// EUR/CHF
				try
				{
					eurchfReEnter = false;
					if (eurchfAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { eurchfPastBox.Text = eurchfAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { eurchfPast = Convert.ToDouble(eurchfAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// EUR/JPY
				try
				{
					eurjpyReEnter = false;
					if (eurjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { eurjpyPastBox.Text = eurjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { eurjpyPast = Convert.ToDouble(eurjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// EUR/USD
				try
				{
					eurusdReEnter = false;
					if (eurusdAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { eurusdPastBox.Text = eurusdAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { eurusdPast = Convert.ToDouble(eurusdAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// GBP/AUD
				try
				{
					gbpaudReEnter = false;
					if (gbpaudAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { gbpaudPastBox.Text = gbpaudAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { gbpaudPast = Convert.ToDouble(gbpaudAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// GBP/CHF
				try
				{
					gbpchfReEnter = false;
					if (gbpchfAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { gbpchfPastBox.Text = gbpchfAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { gbpchfPast = Convert.ToDouble(gbpchfAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// GBP/JPY
				try
				{
					gbpjpyReEnter = false;
					if (gbpjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { gbpjpyPastBox.Text = gbpjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { gbpjpyPast = Convert.ToDouble(gbpjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// GBP/NZD
				try
				{
					gbpnzdReEnter = false;
					if (gbpnzdAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { gbpnzdPastBox.Text = gbpnzdAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { gbpnzdPast = Convert.ToDouble(gbpnzdAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// GBP/USD
				try
				{
					gbpusdReEnter = false;
					if (gbpusdAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { gbpusdPastBox.Text = gbpusdAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { gbpusdPast = Convert.ToDouble(gbpusdAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// NZD/JPY
				try
				{
					nzdjpyReEnter = false;
					if (nzdjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { nzdjpyPastBox.Text = nzdjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { nzdjpyPast = Convert.ToDouble(nzdjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// NZD/USD
				try
				{
					nzdusdReEnter = false;
					if (nzdusdAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { nzdusdPastBox.Text = nzdusdAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { nzdusdPast = Convert.ToDouble(nzdusdAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// USD/CAD
				try
				{
					usdcadReEnter = false;
					if (usdcadAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { usdcadPastBox.Text = usdcadAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { usdcadPast = Convert.ToDouble(usdcadAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// USD/CHF
				try
				{
					usdchfReEnter = false;
					if (usdchfAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { usdchfPastBox.Text = usdchfAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { usdchfPast = Convert.ToDouble(usdchfAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
				// USD/JPY
				try
				{
					usdjpyReEnter = false;
					if (usdjpyAsk.Text != "--")
					{
						this.Invoke(new MethodInvoker(delegate { usdjpyPastBox.Text = usdjpyAsk.Text; }));
						this.Invoke(new MethodInvoker(delegate { usdjpyPast = Convert.ToDouble(usdjpyAsk.Text); }));
					}
				}
				catch (Exception spreadError)
				{
					Console.WriteLine(spreadError);
				}
			}
			
		}
	}
}

