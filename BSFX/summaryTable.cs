using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		private void summaryTable_RowChanged(object sender, RowEventArgs e)
		{
			O2GSummaryTableRow summaryTableRow = (O2GSummaryTableRow)e.RowData;
			

			// AUD/CAD
			if (summaryTableRow.OfferID == "16")
			{
				if (summaryTableRow.GrossPL != 0)
				{
					// Lot Size
					try
					{
						audcadPosLot = Convert.ToDouble(Math.Round(summaryTableRow.Amount, 2));
						this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = Convert.ToString(audcadPosLot); }));
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
					// P&L
					try
					{
						// P/L or Position move string conversion
						audcadPosPL = summaryTableRow.GrossPL;
						string moveString = Convert.ToString(audcadPosPL);
						int index;
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							// Changes to P/L
							if (audcadPosPL > 0)
							{
								audcadPL.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString.Substring(0, index); }));
							}
							else if (audcadPosPL < 0)
							{
								audcadPL.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString.Substring(0, index); }));
							}
							else
							{
								audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "--"; }));
							}
						}
						else
						{
							// Changes to P/L
							if (audcadPosPL > 0)
							{
								audcadPL.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString; }));

							}
							else if (audcadPosPL < 0)
							{
								audcadPL.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString; }));
							}
							else
							{
								audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
								this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "--"; }));
							}
						}
					}
					catch (Exception plErr)
					{
						Console.WriteLine(plErr);
					}
				}
				else
				{
					audcadPosLot = 0;
					this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = "--"; }));
					audcadPosSize.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
					audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
				}
			}

			// AUD/JPY
			else if (summaryTableRow.OfferID == "17")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						audjpyPosLot = Convert.ToDouble(summaryTableRow.Amount);
						this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = Convert.ToString(audjpyPosLot); }));
					}
					else
					{
						audjpyPosLot = 0;
						this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
						audjpyPosSize.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
						audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					audjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(audjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (audjpyPosPL > 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (audjpyPosPL < 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
						}
					}
					else
					{
						// Changes to P/L
						if (audjpyPosPL > 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString; }));

						}
						else if (audjpyPosPL < 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString; }));
						}
						else
						{
							audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// AUD/USD
			else if (summaryTableRow.OfferID == "6")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						audjpyPosLot = Convert.ToDouble(summaryTableRow.Amount);
						this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = Convert.ToString(audjpyPosLot); }));
					}
					else
					{
						audjpyPosLot = 0;
						this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
						audjpyPosSize.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
						audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					audjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(audjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (audjpyPosPL > 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (audjpyPosPL < 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
						}
					}
					else
					{
						// Changes to P/L
						if (audjpyPosPL > 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString; }));

						}
						else if (audjpyPosPL < 0)
						{
							audjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString; }));
						}
						else
						{
							audjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// CAD/JPY
			else if (summaryTableRow.OfferID == "18")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						cadjpyPosLot = Convert.ToDouble(summaryTableRow.Amount);
						this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = Convert.ToString(cadjpyPosLot); }));
					}
					else
					{
						cadjpyPosLot = 0;
						this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = "--"; }));
						cadjpyPosSize.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
						cadjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					cadjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(cadjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (cadjpyPosPL > 0)
						{
							cadjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (cadjpyPosPL < 0)
						{
							cadjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							cadjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "--"; }));
						}
					}
					else
					{
						// Changes to P/L
						if (cadjpyPosPL > 0)
						{
							cadjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString; }));

						}
						else if (cadjpyPosPL < 0)
						{
							cadjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString; }));
						}
						else
						{
							cadjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "--"; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// CHF/JPY
			else if (summaryTableRow.OfferID == "12")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						chfjpyPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = Convert.ToString(chfjpyPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					chfjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(chfjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (chfjpyPosPL > 0)
						{
							chfjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (chfjpyPosPL < 0)
						{
							chfjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (chfjpyPosPL > 0)
						{
							chfjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString; }));

						}
						else if (chfjpyPosPL < 0)
						{
							chfjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// EUR/AUD
			else if (summaryTableRow.OfferID == "14")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						euraudPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = Convert.ToString(euraudPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					euraudPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(euraudPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (euraudPosPL > 0)
						{
							euraudPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (euraudPosPL < 0)
						{
							euraudPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (euraudPosPL > 0)
						{
							euraudPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString; }));

						}
						else if (euraudPosPL < 0)
						{
							euraudPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// EUR/CAD
			else if (summaryTableRow.OfferID == "15")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						eurcadPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = Convert.ToString(eurcadPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurcadPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(eurcadPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (eurcadPosPL > 0)
						{
							eurcadPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (eurcadPosPL < 0)
						{
							eurcadPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (eurcadPosPL > 0)
						{
							eurcadPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString; }));

						}
						else if (eurcadPosPL < 0)
						{
							eurcadPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// EUR/CHF
			else if (summaryTableRow.OfferID == "5")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						eurchfPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = Convert.ToString(eurchfPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurchfPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(eurchfPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (eurchfPosPL > 0)
						{
							eurchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (eurchfPosPL < 0)
						{
							eurchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (eurchfPosPL > 0)
						{
							eurchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString; }));

						}
						else if (eurchfPosPL < 0)
						{
							eurchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// EUR/JPY
			else if (summaryTableRow.OfferID == "10")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						eurjpyPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = Convert.ToString(eurjpyPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(eurjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (eurjpyPosPL > 0)
						{
							eurjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (eurjpyPosPL < 0)
						{
							eurjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (eurjpyPosPL > 0)
						{
							eurjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString; }));

						}
						else if (eurjpyPosPL < 0)
						{
							eurjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// EUR/USD
			else if (summaryTableRow.OfferID == "1")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						eurusdPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = Convert.ToString(eurusdPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurusdPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(eurusdPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (eurusdPosPL > 0)
						{
							eurusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (eurusdPosPL < 0)
						{
							eurusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (eurusdPosPL > 0)
						{
							eurusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString; }));

						}
						else if (eurusdPosPL < 0)
						{
							eurusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// GBP/AUD
			else if (summaryTableRow.OfferID == "22")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						gbpaudPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = Convert.ToString(gbpaudPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpaudPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(gbpaudPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (gbpaudPosPL > 0)
						{
							gbpaudPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (gbpaudPosPL < 0)
						{
							gbpaudPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (gbpaudPosPL > 0)
						{
							gbpaudPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString; }));

						}
						else if (gbpaudPosPL < 0)
						{
							gbpaudPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// GBP/CHF
			else if (summaryTableRow.OfferID == "13")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						gbpchfPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = Convert.ToString(gbpchfPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpchfPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(gbpchfPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (gbpchfPosPL > 0)
						{
							gbpchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (gbpchfPosPL < 0)
						{
							gbpchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (gbpchfPosPL > 0)
						{
							gbpchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString; }));

						}
						else if (gbpchfPosPL < 0)
						{
							gbpchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// GBP/JPY
			else if (summaryTableRow.OfferID == "11")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						gbpjpyPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = Convert.ToString(gbpjpyPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(gbpjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (gbpjpyPosPL > 0)
						{
							gbpjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (gbpjpyPosPL < 0)
						{
							gbpjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (gbpjpyPosPL > 0)
						{
							gbpjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString; }));

						}
						else if (gbpjpyPosPL < 0)
						{
							gbpjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// GBP/NZD
			else if (summaryTableRow.OfferID == "21")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						gbpnzdPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = Convert.ToString(gbpnzdPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpnzdPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(gbpnzdPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (gbpnzdPosPL > 0)
						{
							gbpnzdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (gbpnzdPosPL < 0)
						{
							gbpnzdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (gbpnzdPosPL > 0)
						{
							gbpnzdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString; }));

						}
						else if (gbpnzdPosPL < 0)
						{
							gbpnzdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// GBP/USD
			else if (summaryTableRow.OfferID == "3")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						gbpusdPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = Convert.ToString(gbpusdPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpusdPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(gbpusdPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (gbpusdPosPL > 0)
						{
							gbpusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (gbpusdPosPL < 0)
						{
							gbpusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (gbpusdPosPL > 0)
						{
							gbpusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString; }));

						}
						else if (gbpusdPosPL < 0)
						{
							gbpusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// NZD/JPY
			else if (summaryTableRow.OfferID == "19")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						nzdjpyPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = Convert.ToString(nzdjpyPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					nzdjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(nzdjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (nzdjpyPosPL > 0)
						{
							nzdjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (nzdjpyPosPL < 0)
						{
							nzdjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (nzdjpyPosPL > 0)
						{
							nzdjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString; }));

						}
						else if (nzdjpyPosPL < 0)
						{
							nzdjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// NZD/USD
			else if (summaryTableRow.OfferID == "8")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						nzdusdPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = Convert.ToString(nzdusdPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					nzdusdPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(nzdusdPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (nzdusdPosPL > 0)
						{
							nzdusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (nzdusdPosPL < 0)
						{
							nzdusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (nzdusdPosPL > 0)
						{
							nzdusdPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString; }));

						}
						else if (nzdusdPosPL < 0)
						{
							nzdusdPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}


			// USD/CAD
			else if (summaryTableRow.OfferID == "7")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						usdcadPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = Convert.ToString(usdcadPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdcadPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(usdcadPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (usdcadPosPL > 0)
						{
							usdcadPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (usdcadPosPL < 0)
						{
							usdcadPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (usdcadPosPL > 0)
						{
							usdcadPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString; }));

						}
						else if (usdcadPosPL < 0)
						{
							usdcadPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// USD/CHF
			else if (summaryTableRow.OfferID == "4")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						usdchfPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = Convert.ToString(usdchfPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdchfPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(usdchfPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (usdchfPosPL > 0)
						{
							usdchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (usdchfPosPL < 0)
						{
							usdchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (usdchfPosPL > 0)
						{
							usdchfPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString; }));

						}
						else if (usdchfPosPL < 0)
						{
							usdchfPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}

			// USD/JPY
			else if (summaryTableRow.OfferID == "2")
			{
				// Lot Size
				try
				{
					if (summaryTableRow.Amount != 0)
					{
						usdjpyPosLot = summaryTableRow.Amount;
						this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = Convert.ToString(usdjpyPosLot); }));
					}
				}
				catch (Exception bsErr)
				{
					Console.WriteLine(bsErr);
				}
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdjpyPosPL = summaryTableRow.GrossPL;
					string moveString = Convert.ToString(usdjpyPosPL);
					int index;
					if (moveString.Contains("."))
					{
						index = moveString.IndexOf(".") + 3;
						// Changes to P/L
						if (usdjpyPosPL > 0)
						{
							usdjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else if (usdjpyPosPL < 0)
						{
							usdjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
					}
					else
					{
						// Changes to P/L
						if (usdjpyPosPL > 0)
						{
							usdjpyPL.BackColor = System.Drawing.Color.Blue;
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString; }));

						}
						else if (usdjpyPosPL < 0)
						{
							usdjpyPL.BackColor = System.Drawing.Color.Red;
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString; }));
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}
			}
		}
	}
}
