using fxcore2;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{

	public partial class BSFX : Form
	{

		#region Pre-Setup
		/// <summary> ---------------------------------------------------------------------------  		
		///	- The offersTable class is triggered by the offerTable updated event, via ForexConnect API, and the TableListener.
		///	- Depending on the pair, or "Instrument", the data following the event is filtered below.
		///	- All bid, ask, spread, and move parameters are calculated here.
		///	- Order execution is also placed in this class, based on parameters and total move.
		///	- When an order is closed, it will be closed through here.
		///	- Open orders are confirmed and maintained in the "tradesTable" class.
		/// </summary> -------------------------------------------------------------------------
		/// 

		// EVENT TRIGGERED FROM OFFERS TABLE LISTENER
		private void offersTable_RowChanged(object sender, RowEventArgs e)
		{
			// FxCore2 OfferTable Row Data Event
			O2GOfferTableRow offerTableRow = (O2GOfferTableRow)e.RowData;

			// Parameters
			double fiftyMoveLong = 0.001;
			double fiftyMoveShort = -0.001;
			double hundredMoveLong = 0.001;
			double hundredMoveShort = -0.001;
			double goal = 0.0001;
			lotSize = (Int32)Math.Round(acctEq /1000, 0) * 1000;
			if (lotSize < 1000)
			{
				lotSize = 1000;
			}
			// Data status indicators
			try
			{
				pair = offerTableRow.Instrument;
				try
				{
					// For JPY pairs
					if (offerTableRow.Instrument.Contains("JPY"))
					{
						bid = offerTableRow.Bid / 100;
						ask = offerTableRow.Ask / 100;
					}
					// For non-JPY pairs
					else
					{
						bid = offerTableRow.Bid;
						ask = offerTableRow.Ask;
					}
				}
				catch (Exception jpyErr)
				{
					Console.WriteLine(jpyErr);
				}

				// Data Stream progress bars
				if (dataProgress.Value < 100)
				{
					this.Invoke(new MethodInvoker(delegate { dataProgress.PerformStep(); }));
					this.Invoke(new MethodInvoker(delegate { dataProgress2.PerformStep(); }));
				}
				else if (dataProgress.Value == 100)
				{
					this.Invoke(new MethodInvoker(delegate { dataProgress.Value = 0; }));
					this.Invoke(new MethodInvoker(delegate { dataProgress.Update(); }));
					this.Invoke(new MethodInvoker(delegate { dataProgress2.Value = 0; }));
					this.Invoke(new MethodInvoker(delegate { dataProgress2.Update(); }));
				}
			}
			catch (Exception appedErr)
			{
				Console.WriteLine(appedErr);
			}
			
			// Spread
			try
			{
				double orginSpread = (ask - bid);
				decimal startSpread = Convert.ToDecimal(orginSpread);
				if (startSpread > 0 || startSpread < 0)
				{
					double multiSpread = orginSpread * 10000;
					string spreadConvert = Convert.ToString(multiSpread);

					if (spreadConvert.Contains("."))
					{
						int index = spreadConvert.IndexOf(".") + 2;
						spread = spreadConvert.Substring(0, index);
					}
					else
					{
						spread = spreadConvert;
					}
				}
			}
			catch (Exception spreadErr)
			{
				Console.WriteLine(spreadErr);
				actiBox.AppendText(spreadErr + Environment.NewLine);
			}
		#endregion
			// **SET TABLE DATA**
			if (offerTableRow != null)
			{
				// Grab the table update that triggered the event
				try
				{
					// If triggered pair is "AUD/CAD"
					if (offerTableRow.Instrument == "AUD/CAD")
					{
						this.Invoke(new MethodInvoker(delegate { audcadBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audcadAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audcadSpread.Text = spread; }));
						// Fill in past price if ask price has a value
						if (audcadPastBox.Text == "--" && audcadAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { audcadPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { audcadPast = ask; }));
						}
						if (audcadPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - audcadPast);
							double moveConvert = moveCalc * 10000;
							audcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (audcadMove > 0)
							{
								audcadMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audcadMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && audcadJustTraded == false && hedgingLong == true)
								{
									sOfferID = "16";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									audcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (audcadMove < 0)
							{
								audcadMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audcadMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && audcadJustTraded == false && hedgingShort == true)
								{
									sOfferID = "16";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									audcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "AUD/JPY"
					if (offerTableRow.Instrument == "AUD/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { audjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (audjpyPastBox.Text == "--" && audjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { audjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { audjpyPast = ask; }));
						}
						if (audjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - audjpyPast);
							double moveConvert = moveCalc * 10000;
							audjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (audjpyMove > 0)
							{
								audjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.Text = move; }));
								if (moveCalc > fiftyMoveLong && audjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "17";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									audjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (audjpyMove < 0)
							{
								audjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < fiftyMoveShort && audjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "17";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									audjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "AUD/USD"
					if (offerTableRow.Instrument == "AUD/USD")
					{
						this.Invoke(new MethodInvoker(delegate { audusdBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audusdAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { audusdSpread.Text = spread; }));
						if (audusdPastBox.Text == "--" && audusdAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { audusdPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { audusdPast = ask; }));
						}
						if (audusdPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - audusdPast);
							double moveConvert = moveCalc * 10000;
							audusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (audusdMove > 0)
							{
								audusdMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { audusdMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && audusdJustTraded == false && hedgingLong == true)
								{
									sOfferID = "6";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									audusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (audusdMove < 0 && audusdJustTraded == false)
							{
								audusdMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { audusdMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && audusdJustTraded == false && hedgingShort == true)
								{
									sOfferID = "6";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									audusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "CAD/JPY"
					if (offerTableRow.Instrument == "CAD/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { cadjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { cadjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { cadjpySpread.Text = spread; }));
						if (cadjpyPastBox.Text == "--" && cadjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { cadjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { cadjpyPast = ask; }));
						}
						if (cadjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - cadjpyPast);
							double moveConvert = moveCalc * 10000;
							cadjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (cadjpyMove > 0)
							{
								cadjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && cadjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "18";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									cadjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (cadjpyMove < 0)
							{
								cadjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && cadjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "18";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									cadjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "CHF/JPY"
					if (offerTableRow.Instrument == "CHF/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { chfjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { chfjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { chfjpySpread.Text = spread; }));
						if (chfjpyPastBox.Text == "--" && chfjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { chfjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { chfjpyPast = ask; }));
						}
						if (chfjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - chfjpyPast);
							double moveConvert = moveCalc * 10000;
							chfjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (chfjpyMove > 0)
							{
								chfjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.Text = move; }));
								if (moveCalc > fiftyMoveLong && chfjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "12";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									chfjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (chfjpyMove < 0)
							{
								chfjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < fiftyMoveShort && chfjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "12";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									chfjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "EUR/AUD"
					if (offerTableRow.Instrument == "EUR/AUD")
					{
						this.Invoke(new MethodInvoker(delegate { euraudBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { euraudAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { euraudSpread.Text = spread; }));
						if (euraudPastBox.Text == "--" && euraudAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { euraudPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { euraudPast = ask; }));
						}
						if (euraudPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - euraudPast);
							double moveConvert = moveCalc * 10000;
							euraudMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (euraudMove > 0)
							{
								euraudMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { euraudMoveBox.Text = move; }));
								if (moveCalc > fiftyMoveLong && euraudJustTraded == false && hedgingLong == true)
								{
									sOfferID = "14";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									euraudJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (euraudMove < 0)
							{
								euraudMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { euraudMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < fiftyMoveShort && euraudJustTraded == false && hedgingShort == true)
								{
									sOfferID = "14";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									euraudJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "EUR/CAD"
					if (offerTableRow.Instrument == "EUR/CAD")
					{
						this.Invoke(new MethodInvoker(delegate { eurcadBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurcadAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurcadSpread.Text = spread; }));
						if (eurcadPastBox.Text == "--" && eurcadAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { eurcadPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { eurcadPast = ask; }));
						}
						if (eurcadPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - eurcadPast);
							double moveConvert = moveCalc * 10000;
							eurcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (eurcadMove > 0)
							{
								eurcadMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && eurcadJustTraded == false && hedgingLong == true)
								{
									sOfferID = "15";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									eurcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (eurcadMove < 0)
							{
								eurcadMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && eurcadJustTraded == false && hedgingShort == true)
								{
									sOfferID = "15";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									eurcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "EUR/CHF"
					if (offerTableRow.Instrument == "EUR/CHF")
					{
						this.Invoke(new MethodInvoker(delegate { eurchfBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurchfAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurchfSpread.Text = spread; }));
						if (eurchfPastBox.Text == "--" && eurchfAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { eurchfPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { eurchfPast = ask; }));
						}
						if (eurchfPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - eurchfPast);
							double moveConvert = moveCalc * 10000;
							eurchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (eurchfMove > 0)
							{
								eurchfMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && eurchfJustTraded == false && hedgingLong == true)
								{
									sOfferID = "5";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									eurchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (eurchfMove < 0)
							{
								eurchfMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && eurchfJustTraded == false && hedgingShort == true)
								{
									sOfferID = "5";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									eurchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "EUR/JPY"
					if (offerTableRow.Instrument == "EUR/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { eurjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurjpySpread.Text = spread; }));
						if (eurjpyPastBox.Text == "--" && eurjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { eurjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { eurjpyPast = ask; }));
						}
						if (eurjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - eurjpyPast);
							double moveConvert = moveCalc * 10000;
							eurjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (eurjpyMove > 0)
							{
								eurjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && eurjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "10";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									eurjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (eurjpyMove < 0)
							{
								eurjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && eurjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "10";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									eurjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "EUR/USD"
					if (offerTableRow.Instrument == "EUR/USD")
					{
						this.Invoke(new MethodInvoker(delegate { eurusdBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurusdAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { eurusdSpread.Text = spread; }));
						if (eurusdPastBox.Text == "--" && eurusdAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { eurusdPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { eurusdPast = ask; }));
						}
						if (eurusdPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - eurusdPast);
							double moveConvert = moveCalc * 10000;
							eurusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (eurusdMove > 0)
							{
								eurusdMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && eurusdJustTraded == false && hedgingLong == true)
								{
									sOfferID = "1";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									eurusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (eurusdMove < 0)
							{
								eurusdMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && eurusdJustTraded == false && hedgingShort == true)
								{
									sOfferID = "1";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									eurusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "GBP/AUD"
					if (offerTableRow.Instrument == "GBP/AUD")
					{
						this.Invoke(new MethodInvoker(delegate { gbpaudBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpaudAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpaudSpread.Text = spread; }));
						if (gbpaudPastBox.Text == "--" && gbpaudAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpaudPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { gbpaudPast = ask; }));
						}
						if (gbpaudPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - gbpaudPast);
							double moveConvert = moveCalc * 10000;
							gbpaudMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (gbpaudMove > 0)
							{
								gbpaudMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && gbpaudJustTraded == false && hedgingLong == true)
								{
									sOfferID = "22";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									gbpaudJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (gbpaudMove < 0)
							{
								gbpaudMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && gbpaudJustTraded == false && hedgingShort == true)
								{
									sOfferID = "22";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									gbpaudJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "GBP/CHF"
					if (offerTableRow.Instrument == "GBP/CHF")
					{
						this.Invoke(new MethodInvoker(delegate { gbpchfBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpchfAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpchfSpread.Text = spread; }));
						if (gbpchfPastBox.Text == "--" && gbpchfAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpchfPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { gbpchfPast = ask; }));
						}
						if (gbpchfPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - gbpchfPast);
							double moveConvert = moveCalc * 10000;
							gbpchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (gbpchfMove > 0)
							{
								gbpchfMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && gbpchfJustTraded == false && hedgingLong == true)
								{
									sOfferID = "13";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									gbpchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (gbpchfMove < 0)
							{
								gbpchfMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && gbpchfJustTraded == false && hedgingShort == true)
								{
									sOfferID = "13";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									gbpchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "GBP/JPY"
					if (offerTableRow.Instrument == "GBP/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { gbpjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpjpySpread.Text = spread; }));
						if (gbpjpyPastBox.Text == "--" && gbpjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { gbpjpyPast = ask; }));
						}
						if (gbpjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - gbpjpyPast);
							double moveConvert = moveCalc * 10000;
							gbpjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (gbpjpyMove > 0)
							{
								gbpjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && gbpjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "11";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									gbpjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (gbpjpyMove < 0)
							{
								gbpjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && gbpjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "11";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									gbpjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "GBP/NZD"
					if (offerTableRow.Instrument == "GBP/NZD")
					{
						this.Invoke(new MethodInvoker(delegate { gbpnzdBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpnzdAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpnzdSpread.Text = spread; }));
						if (gbpnzdPastBox.Text == "--" && gbpnzdAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpnzdPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { gbpnzdPast = ask; }));
						}
						if (gbpnzdPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - gbpnzdPast);
							double moveConvert = moveCalc * 10000;
							gbpnzdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (gbpnzdMove > 0)
							{
								gbpnzdMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && gbpnzdJustTraded == false && hedgingLong == true)
								{
									sOfferID = "21";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									gbpnzdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (gbpnzdMove < 0)
							{
								gbpnzdMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && gbpnzdJustTraded == false && hedgingShort == true)
								{
									sOfferID = "21";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									gbpnzdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "GBP/USD"
					if (offerTableRow.Instrument == "GBP/USD")
					{
						this.Invoke(new MethodInvoker(delegate { gbpusdBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpusdAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { gbpusdSpread.Text = spread; }));
						if (gbpusdPastBox.Text == "--" && gbpusdAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpusdPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { gbpusdPast = ask; }));
						}
						if (gbpusdPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - gbpusdPast);
							double moveConvert = moveCalc * 10000;
							gbpusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (gbpusdMove > 0)
							{
								gbpusdMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && gbpusdJustTraded == false && hedgingLong == true)
								{
									sOfferID = "3";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									gbpusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (gbpusdMove < 0)
							{
								gbpusdMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && gbpusdJustTraded == false && hedgingShort == true)
								{
									sOfferID = "3";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									gbpusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "NZD/JPY"
					if (offerTableRow.Instrument == "NZD/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { nzdjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { nzdjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { nzdjpySpread.Text = spread; }));
						if (nzdjpyPastBox.Text == "--" && nzdjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { nzdjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { nzdjpyPast = ask; }));
						}
						if (nzdjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - nzdjpyPast);
							double moveConvert = moveCalc * 10000;
							nzdjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (nzdjpyMove > 0)
							{
								nzdjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.Text = move; }));
								if (moveCalc > fiftyMoveLong && nzdjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "19";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									nzdjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (nzdjpyMove < 0)
							{
								nzdjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < fiftyMoveShort && nzdjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "19";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									nzdjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "NZD/USD"
					if (offerTableRow.Instrument == "NZD/USD")
					{
						this.Invoke(new MethodInvoker(delegate { nzdusdBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { nzdusdAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { nzdusdSpread.Text = spread; }));
						if (nzdusdPastBox.Text == "--" && nzdusdAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { nzdusdPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { nzdusdPast = ask; }));
						}
						if (nzdusdPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - nzdusdPast);
							double moveConvert = moveCalc * 10000;
							nzdusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (nzdusdMove > 0)
							{
								nzdusdMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && nzdusdJustTraded == false && hedgingLong == true)
								{
									sOfferID = "8";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									nzdusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (nzdusdMove < 0)
							{
								nzdusdMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && nzdusdJustTraded == false && hedgingShort == true)
								{
									sOfferID = "8";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									nzdusdJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "USD/CAD"
					if (offerTableRow.Instrument == "USD/CAD")
					{
						this.Invoke(new MethodInvoker(delegate { usdcadBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdcadAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdcadSpread.Text = spread; }));
						if (usdcadPastBox.Text == "--" && usdcadAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { usdcadPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { usdcadPast = ask; }));
						}
						if (usdcadPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - usdcadPast);
							double moveConvert = moveCalc * 10000;
							usdcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (usdcadMove > 0)
							{
								usdcadMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && usdcadJustTraded == false && hedgingLong == true)
								{
									sOfferID = "7";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									usdcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (usdcadMove < 0)
							{
								usdcadMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && usdcadJustTraded == false && hedgingShort == true)
								{
									sOfferID = "7";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									usdcadJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "USD/CHF"
					if (offerTableRow.Instrument == "USD/CHF")
					{
						this.Invoke(new MethodInvoker(delegate { usdchfBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdchfAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdchfSpread.Text = spread; }));
						if (usdchfPastBox.Text == "--" && usdchfAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { usdchfPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { usdchfPast = ask; }));
						}
						if (usdchfPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - usdchfPast);
							double moveConvert = moveCalc * 10000;
							usdchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (usdchfMove > 0)
							{
								usdchfMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && usdchfJustTraded == false && hedgingLong == true)
								{
									sOfferID = "4";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = ask + goal;
									usdchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (usdchfMove < 0)
							{
								usdchfMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && usdchfJustTraded == false && hedgingShort == true)
								{
									sOfferID = "4";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = bid - goal;
									usdchfJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}

					// If triggered pair is "USD/JPY"
					if (offerTableRow.Instrument == "USD/JPY")
					{
						this.Invoke(new MethodInvoker(delegate { usdjpyBid.Text = bid.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdjpyAsk.Text = ask.ToString(); }));
						this.Invoke(new MethodInvoker(delegate { usdjpySpread.Text = spread; }));
						if (usdjpyPastBox.Text == "--" && usdjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { usdjpyPastBox.Text = ask.ToString(); }));
							this.Invoke(new MethodInvoker(delegate { usdjpyPast = ask; }));
						}
						if (usdjpyPast != 0)
						{
							// Move calculations
							double moveCalc = (ask - usdjpyPast);
							double moveConvert = moveCalc * 10000;
							usdjpyMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);

							string move;
							int index;
							if (moveString.Contains("."))
							{
								index = moveString.IndexOf(".") + 2;
								move = moveString.Substring(0, index);
							}
							else
							{
								move = moveString;
							}
							if (usdjpyMove > 0)
							{
								usdjpyMoveBox.BackColor = System.Drawing.Color.Blue;
								this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.Text = move; }));
								if (moveCalc > hundredMoveLong && usdjpyJustTraded == false && hedgingLong == true)
								{
									sOfferID = "2";
									iAmount = lotSize;
									sBuySell = "B";
									dRateLimit = (ask * 100) + goal;
									usdjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
							else if (usdjpyMove < 0)
							{
								usdjpyMoveBox.BackColor = System.Drawing.Color.Red;
								this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.Text = "(" + move + ")"; }));
								if (moveCalc < hundredMoveShort && usdjpyJustTraded == false && hedgingShort == true)
								{
									sOfferID = "2";
									iAmount = lotSize;
									sBuySell = "S";
									dRateLimit = (bid * 100) - goal;
									usdjpyJustTraded = true;
									CreateTrueMarketOrder(sOfferID, sAccountID, iAmount, sBuySell, dRateLimit);
								}
							}
						}
					}
					Thread.Sleep(50);
				}
				catch (Exception offerErr)
				{
					Console.WriteLine(offerErr);
				}
			}
		}
	}
}
