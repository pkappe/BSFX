using fxcore2;
using System;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{

		/// <summary>
		/// This class is triggered by the TRADES TABLE in ForexConnect-API via FXCM. 
		/// - This class is triggered, via updates, for positions that are currently owned on the account. 
		/// - This class will keep track of all trades owned at the broker, and update as needed. 
		/// - No ordering is done through this class. It is strictly a listener. 
		/// - Pairs are filtered by OfferID, listed below.
		/// - If a position order is placed from the OFFERS TABLE class, it will be updates here, and a OrderID will be stored for future ordering.
		/// 
		/// PAIRS BY OFFER ID
		/// 1 	EUR/USD
		/// 2 	USD/JPY
		/// 3 	GBP/USD
		/// 4	USD/CHF
		/// 5	EUR/CHF
		/// 6	AUD/USD
		/// 7	USD/CAD
		/// 8	NZD/USD
		/// 10	EUR/JPY
		/// 11	GBP/JPY
		/// 12	CHF/JPY
		/// 13	GBP/CHF
		/// 14	EUR/AUD
		/// 15	EUR/CAD
		/// 16	AUD/CAD
		/// 17	AUD/JPY
		/// 18	CAD/JPY
		/// 19	NZD/JPY
		/// 21	GBP/NZD
		/// 22	GBP/AUD
		/// 
		/// </summary>

		void tradesTable_RowChanged(object sender, RowEventArgs e)
		{
			O2GTradeTableRow trade = (O2GTradeTableRow)e.RowData;

			// AUD/CAD
			if (trade.OfferID == "16")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					audcadPosPL = trade.GrossPL;
					string moveString = Convert.ToString(audcadPosPL);
					int index;
					if (audcadJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { audcadPL.Text = "$" + moveString; }));
						}					
					}
					else if (audcadJustSold == false)
					{
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
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (audcadJustSold == false)
				{
					// TradeID
					try
					{
						audcadOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							audcadSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { audcadSide.Text = audcadSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							audcadSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { audcadSide.Text = audcadSideString; }));
						}
						else
						{
							audcadSideString = "";
							this.Invoke(new MethodInvoker(delegate { audcadSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							audcadPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = Convert.ToString(audcadPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							audcadEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { audcadEntry.Text = Convert.ToString(audcadEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// AUD/JPY
			else if (trade.OfferID == "17")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					audjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(audjpyPosPL);
					int index;
					if (audjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { audjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (audjpyJustSold == false)
					{

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
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (audjpyJustSold == false)
				{
					// TradeID
					try
					{
						audjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							audjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { audjpySide.Text = audjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							audjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { audjpySide.Text = audjpySideString; }));
						}
						else
						{
							audjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							audjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = Convert.ToString(audjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							audjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = Convert.ToString(audjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						audjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// AUD/USD
			else if (trade.OfferID == "6")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					audusdPosPL = trade.GrossPL;
					string moveString = Convert.ToString(audusdPosPL);
					int index;
					if (audusdJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { audusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString; }));
						}
					}
					else if (audusdJustSold == false)
					{

						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							// Changes to P/L
							if (audusdPosPL > 0)
							{
								audusdPL.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString.Substring(0, index); }));
							}
							else if (audusdPosPL < 0)
							{
								audusdPL.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString.Substring(0, index); }));
							}
						}
						else
						{
							// Changes to P/L
							if (audusdPosPL > 0)
							{
								audusdPL.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString; }));

							}
							else if (audusdPosPL < 0)
							{
								audusdPL.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + moveString; }));
							}
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (audusdJustSold == false)
				{
					// TradeID
					try
					{
						audusdOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							audusdSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { audusdSide.Text = audusdSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							audusdSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { audusdSide.Text = audusdSideString; }));
						}
						else
						{
							audusdSideString = "";
							this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							audusdPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { audusdPosSize.Text = Convert.ToString(audusdPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							audusdEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = Convert.ToString(audusdEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// CAD/JPY
			else if (trade.OfferID == "18")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					cadjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(cadjpyPosPL);
					int index;
					if (cadjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { cadjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (cadjpyJustSold == false)
					{

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
						}
					}
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (cadjpyJustSold == false)
				{
					// TradeID
					try
					{
						cadjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							cadjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = cadjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							cadjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = cadjpySideString; }));
						}
						else
						{
							cadjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							cadjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = Convert.ToString(cadjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							cadjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = Convert.ToString(cadjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						cadjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// CHF/JPY
			else if (trade.OfferID == "12")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					chfjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(chfjpyPosPL);
					int index;
					if (chfjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { chfjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (chfjpyJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (chfjpyJustSold == false)
				{
					// TradeID
					try
					{
						chfjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							chfjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = chfjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							chfjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = chfjpySideString; }));
						}
						else
						{
							chfjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							chfjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = Convert.ToString(chfjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							chfjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = Convert.ToString(chfjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						chfjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// EUR/AUD
			else if (trade.OfferID == "14")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					euraudPosPL = trade.GrossPL;
					string moveString = Convert.ToString(euraudPosPL);
					int index;
					if (euraudJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { euraudPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + moveString; }));
						}
					}
					else if (euraudJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (euraudJustSold == false)
				{
					// TradeID
					try
					{
						euraudOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							euraudSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { euraudSide.Text = euraudSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							euraudSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { euraudSide.Text = euraudSideString; }));
						}
						else
						{
							euraudSideString = "";
							this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							euraudPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = Convert.ToString(euraudPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							euraudEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = Convert.ToString(euraudEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// EUR/CAD
			else if (trade.OfferID == "15")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurcadPosPL = trade.GrossPL;
					string moveString = Convert.ToString(eurcadPosPL);
					int index;
					if (eurcadJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { eurcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + moveString; }));
						}
					}
					else if (eurcadJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (eurcadJustSold == false)
				{
					// TradeID
					try
					{
						eurcadOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							eurcadSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = eurcadSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							eurcadSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = eurcadSideString; }));
						}
						else
						{
							eurcadSideString = "";
							this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							eurcadPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = Convert.ToString(eurcadPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							eurcadEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = Convert.ToString(eurcadEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// EUR/CHF
			else if (trade.OfferID == "5")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurchfPosPL = trade.GrossPL;
					string moveString = Convert.ToString(eurchfPosPL);
					int index;
					if (eurchfJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { eurchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + moveString; }));
						}
					}
					else if (eurchfJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (eurchfJustSold == false)
				{
					// TradeID
					try
					{
						eurchfOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							eurchfSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = eurchfSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							eurchfSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = eurchfSideString; }));
						}
						else
						{
							eurchfSideString = "";
							this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							eurchfPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = Convert.ToString(eurchfPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							eurchfEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = Convert.ToString(eurchfEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// EUR/JPY
			else if (trade.OfferID == "10")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(eurjpyPosPL);
					int index;
					if (eurjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { eurjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (eurjpyJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (eurjpyJustSold == false)
				{
					// TradeID
					try
					{
						eurjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							eurjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = eurjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							eurjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = eurjpySideString; }));
						}
						else
						{
							eurjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							eurjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = Convert.ToString(eurjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							eurjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = Convert.ToString(eurjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						eurjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// EUR/USD
			else if (trade.OfferID == "1")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					eurusdPosPL = trade.GrossPL;
					string moveString = Convert.ToString(eurusdPosPL);
					int index;
					if (eurusdJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { eurusdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { eurusdPL.Text = "$" + moveString; }));
						}
					}
					else if (eurusdJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (eurusdJustSold == false)
				{
					// TradeID
					try
					{
						eurusdOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							eurusdSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = eurusdSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							eurusdSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = eurusdSideString; }));
						}
						else
						{
							eurusdSideString = "";
							this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							eurusdPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = Convert.ToString(eurusdPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							eurusdEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { eurusdEntry.Text = Convert.ToString(eurusdEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// GBP/AUD
			else if (trade.OfferID == "22")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpaudPosPL = trade.GrossPL;
					string moveString = Convert.ToString(gbpaudPosPL);
					int index;
					if (gbpaudJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { gbpaudPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { gbpaudPL.Text = "$" + moveString; }));
						}
					}
					else if (gbpaudJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (gbpaudJustSold == false)
				{
					// TradeID
					try
					{
						gbpaudOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							gbpaudSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = gbpaudSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							gbpaudSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = gbpaudSideString; }));
						}
						else
						{
							gbpaudSideString = "";
							this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							gbpaudPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = Convert.ToString(gbpaudPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							gbpaudEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { gbpaudEntry.Text = Convert.ToString(gbpaudEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						gbpaudOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// GBP/CHF
			else if (trade.OfferID == "13")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpchfPosPL = trade.GrossPL;
					string moveString = Convert.ToString(gbpchfPosPL);
					int index;
					if (gbpchfJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { gbpchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { gbpchfPL.Text = "$" + moveString; }));
						}
					}
					else if (gbpchfJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (gbpchfJustSold == false)
				{
					// TradeID
					try
					{
						gbpchfOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							gbpchfSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = gbpchfSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							gbpchfSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = gbpchfSideString; }));
						}
						else
						{
							gbpchfSideString = "";
							this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							gbpchfPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = Convert.ToString(gbpchfPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							gbpchfEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { gbpchfEntry.Text = Convert.ToString(gbpchfEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						gbpchfOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// GBP/JPY
			else if (trade.OfferID == "11")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(gbpjpyPosPL);
					int index;
					if (gbpjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { gbpjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { gbpjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (gbpjpyJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (gbpjpyJustSold == false)
				{
					// TradeID
					try
					{
						gbpjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							gbpjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = gbpjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							gbpjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = gbpjpySideString; }));
						}
						else
						{
							gbpjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							gbpjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = Convert.ToString(gbpjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							gbpjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { gbpjpyEntry.Text = Convert.ToString(gbpjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						gbpjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// GBP/NZD
			else if (trade.OfferID == "21")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpnzdPosPL = trade.GrossPL;
					string moveString = Convert.ToString(gbpnzdPosPL);
					int index;
					if (gbpnzdJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { gbpnzdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { gbpnzdPL.Text = "$" + moveString; }));
						}
					}
					else if (gbpnzdJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (gbpnzdJustSold == false)
				{
					// TradeID
					try
					{
						gbpnzdOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							gbpnzdSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = gbpnzdSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							gbpnzdSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = gbpnzdSideString; }));
						}
						else
						{
							gbpnzdSideString = "";
							this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							gbpnzdPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = Convert.ToString(gbpnzdPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							gbpnzdEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { gbpnzdEntry.Text = Convert.ToString(gbpnzdEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						gbpnzdOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// GBP/USD
			else if (trade.OfferID == "3")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					gbpusdPosPL = trade.GrossPL;
					string moveString = Convert.ToString(gbpusdPosPL);
					int index;
					if (gbpusdJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { gbpusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { gbpusdPL.Text = "$" + moveString; }));
						}
					}
					else if (gbpusdJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (gbpusdJustSold == false)
				{
					// TradeID
					try
					{
						gbpusdOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							gbpusdSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = gbpusdSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							gbpusdSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = gbpusdSideString; }));
						}
						else
						{
							gbpusdSideString = "";
							this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							gbpusdPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = Convert.ToString(gbpusdPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							gbpusdEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { gbpusdEntry.Text = Convert.ToString(gbpusdEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						gbpusdOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// NZD/JPY
			else if (trade.OfferID == "19")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					nzdjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(nzdjpyPosPL);
					int index;
					if (nzdjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { nzdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { nzdjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (nzdjpyJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (nzdjpyJustSold == false)
				{
					// TradeID
					try
					{
						nzdjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							nzdjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = nzdjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							nzdjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = nzdjpySideString; }));
						}
						else
						{
							nzdjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							nzdjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = Convert.ToString(nzdjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							nzdjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { nzdjpyEntry.Text = Convert.ToString(nzdjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						nzdjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// NZD/USD
			else if (trade.OfferID == "8")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					nzdusdPosPL = trade.GrossPL;
					string moveString = Convert.ToString(nzdusdPosPL);
					int index;
					if (nzdusdJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { nzdusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { nzdusdPL.Text = "$" + moveString; }));
						}
					}
					else if (nzdusdJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (nzdusdJustSold == false)
				{
					// TradeID
					try
					{
						nzdusdOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							nzdusdSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = nzdusdSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							nzdusdSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = nzdusdSideString; }));
						}
						else
						{
							nzdusdSideString = "";
							this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							nzdusdPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = Convert.ToString(nzdusdPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							nzdusdEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { nzdusdEntry.Text = Convert.ToString(nzdusdEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						nzdusdOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}


			// USD/CAD
			else if (trade.OfferID == "7")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdcadPosPL = trade.GrossPL;
					string moveString = Convert.ToString(usdcadPosPL);
					int index;
					if (usdcadJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { usdcadPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { usdcadPL.Text = "$" + moveString; }));
						}
					}
					else if (usdcadJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (usdcadJustSold == false)
				{
					// TradeID
					try
					{
						usdcadOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							usdcadSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = usdcadSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							usdcadSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = usdcadSideString; }));
						}
						else
						{
							usdcadSideString = "";
							this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							usdcadPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = Convert.ToString(usdcadPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							usdcadEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { usdcadEntry.Text = Convert.ToString(usdcadEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						usdcadOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// USD/CHF
			else if (trade.OfferID == "4")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdchfPosPL = trade.GrossPL;
					string moveString = Convert.ToString(usdchfPosPL);
					int index;
					if (usdchfJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { usdchfPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { usdchfPL.Text = "$" + moveString; }));
						}
					}
					else if (usdchfJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (usdchfJustSold == false)
				{
					// TradeID
					try
					{
						usdchfOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							usdchfSideString = "Long";
							this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = usdchfSideString; }));
						}
						else if (trade.BuySell == "S")
						{
							usdchfSideString = "Short";
							this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = usdchfSideString; }));
						}
						else
						{
							usdchfSideString = "";
							this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							usdchfPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = Convert.ToString(usdchfPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							usdchfEntryPrice = Convert.ToDecimal(trade.OpenRate);
							this.Invoke(new MethodInvoker(delegate { usdchfEntry.Text = Convert.ToString(usdchfEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						usdchfOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}

			// USD/JPY
			else if (trade.OfferID == "2")
			{
				// P&L
				try
				{
					// P/L or Position move string conversion
					usdjpyPosPL = trade.GrossPL;
					string moveString = Convert.ToString(usdjpyPosPL);
					int index;
					if (usdjpyJustSold == true)
					{
						this.Invoke(new MethodInvoker(delegate { usdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
						if (moveString.Contains("."))
						{
							index = moveString.IndexOf(".") + 3;
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString.Substring(0, index); }));
						}
						else
						{
							this.Invoke(new MethodInvoker(delegate { usdjpyPL.Text = "$" + moveString; }));
						}
					}
					else if (usdjpyJustSold == false)
					{

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
				}
				catch (Exception plErr)
				{
					Console.WriteLine(plErr);
				}

				if (usdjpyJustSold == false)
				{
					// TradeID
					try
					{
						usdjpyOrderID = trade.TradeID;
					}
					catch (Exception idErr)
					{
						Console.WriteLine(idErr);
					}

					// Long or Short
					try
					{
						if (trade.BuySell == "B")
						{
							usdjpySideString = "Long";
							this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = usdjpySideString; }));
						}
						else if (trade.BuySell == "S")
						{
							usdjpySideString = "Short";
							this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = usdjpySideString; }));
						}
						else
						{
							usdjpySideString = "";
							this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = "--"; }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Lot Size
					try
					{
						if (trade.Amount != 0)
						{
							usdjpyPosLot = trade.Amount;
							this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = Convert.ToString(usdjpyPosLot); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Entry Price
					try
					{
						if (trade.OpenRate != 0)
						{
							usdjpyEntryPrice = Convert.ToDecimal(trade.OpenRate) / 100;
							this.Invoke(new MethodInvoker(delegate { usdjpyEntry.Text = Convert.ToString(usdjpyEntryPrice); }));
						}
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}

					// Open Order ID
					try
					{
						usdjpyOrderID = trade.OpenOrderID;
					}
					catch (Exception bsErr)
					{
						Console.WriteLine(bsErr);
					}
				}
			}
		}
	}
}
