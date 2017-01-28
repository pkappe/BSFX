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

		//Event handler for row change update on offers table
		private void offersTable_RowChanged(object sender, RowEventArgs e)
		{
			// FxCore2 OfferTable Row Data Event
			O2GOfferTableRow offerTableRow = (O2GOfferTableRow)e.RowData;			
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
						bidd = Convert.ToDecimal(bid);
						bids = Convert.ToString(bid);
						ask = offerTableRow.Ask / 100;
						askd = Convert.ToDecimal(ask);
						asks = Convert.ToString(ask);
					}
					// For non-JPY pairs
					else
					{
						bid = offerTableRow.Bid;							
						bidd = Convert.ToDecimal(bid);
						bids = Convert.ToString(bid);
						ask = offerTableRow.Ask;
						askd = Convert.ToDecimal(ask);
						asks = Convert.ToString(ask);
										
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
				actiBox.AppendText(appedErr + Environment.NewLine);
			}
			
			// Spread
			try
			{
				orginSpread = (ask - bid);				
				startSpread = Convert.ToDecimal(orginSpread);				
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
			
			// Volatility
			try
			{
				// Add all Volatility decimals together.				
				volatility = Convert.ToDecimal(audcadMove + audjpyMove + audusdMove + cadjpyMove + chfjpyMove + euraudMove + eurcadMove + eurchfMove + eurjpyMove + eurusdMove + gbpaudMove + gbpchfMove + gbpjpyMove + gbpnzdMove + gbpusdMove + nzdjpyMove + nzdusdMove + usdcadMove + usdchfMove + usdjpyMove);

				// Multiply by 10000
				decimal addToVol = (volatility * 10000);
				
				if (addToVol > 0 || addToVol < 0)
				{
					string convertVol = Convert.ToString(addToVol);					
					int index = convertVol.IndexOf(".") + 3;
					string finVol = convertVol.Substring(0, index);					
					if (volatility > 0)
					{
						volBox.ForeColor = System.Drawing.Color.Green;
						this.Invoke(new MethodInvoker(delegate { volBox.Text = finVol; }));
					}
					else if (volatility < 0)
					{
						volBox.ForeColor = System.Drawing.Color.Red;
						this.Invoke(new MethodInvoker(delegate { volBox.Text = "(" + finVol + ")"; }));
					}
				}
				else
				{
					volBox.ForeColor = System.Drawing.Color.White;
					this.Invoke(new MethodInvoker(delegate { volBox.Text = "0"; }));
				}				
			}
			catch (Exception volErr)
			{
				actiBox.AppendText(volErr + Environment.NewLine);
				Console.WriteLine(volErr);
			}	

				
			// Unrealized PL
			try
			{
				if (unrealPL != 0)
				{
					this.Invoke(new MethodInvoker(delegate { unrealBox.Text = Convert.ToString("$" + unrealPL); }));
				}
				else
				{
					this.Invoke(new MethodInvoker(delegate { unrealBox.Text = Convert.ToString("--"); }));
				}
			}
			catch (Exception unrealErr)
			{
				Console.WriteLine(unrealErr);
			}			

			// **SET TABLE DATA**
			if (offerTableRow != null)
			{
				
				#endregion

				// Grab the table update that triggered the event
				try
				{
					// If triggered pair is "AUD/CAD"
					if (offerTableRow.Instrument == "AUD/CAD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { audcadBid.Text = bids; }));						
						this.Invoke(new MethodInvoker(delegate { audcadAsk.Text = asks; }));						
						this.Invoke(new MethodInvoker(delegate { audcadSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (audcadPastBox.Text == "--" && audcadAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { audcadPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { audcadPast = ask; }));																
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}							
						}
						
						// If "Past" TextBox is not null
						if (audcadPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - audcadPast);
							double moveConvert = moveCalc * 10000;
							audcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (audcadMove > highestMove)
							{
								highestMove = audcadMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{								
								// LONG: Entry check
								if (audcadMove > 0)
								{									
									audcadMoveBox.BackColor = System.Drawing.Color.Blue;									
									this.Invoke(new MethodInvoker(delegate { audcadMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (audcadMove >= buyLong & startSpread < maxSpread && audcadEntry.Text == "--" && audcadReEnter == false && audcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "16";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";										
										audcadJustSold = false;
										audcadReEnter = true;
										audcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}								

								// SHORT: Entry check
								else if (audcadMove < 0)
								{
									audcadMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { audcadMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (audcadMove <= sellShort & startSpread < maxSpread && audcadEntry.Text == "--" && audcadReEnter == false && audcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "16";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										audcadJustSold = false;
										audcadReEnter = true;
										audcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { audcadMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { audcadMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}
								
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}
							 
							// LONG: POSITION MANAGER
							if (audcadSideString == "Long" && audcadJustSold == false)
							{
								// Subtract entry price from bid price
								audcadBuyLongCheck = (bidd - audcadEntryPrice);
								audcadPosMoveDec = (bidd - audcadEntryPrice) * audcadPosLot;
								
								
								// LONG: Sell Trigger
								if (audcadBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									audcadJustSold = true;
									sOfferID = "16";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = audcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + audcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (audcadBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audcadJustSold = true;
									sOfferID = "16";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = audcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (audcadSideString == "Short" && audcadJustSold == false)
							{
								// Subtract entry price from bid price
								audcadShortSaleCheck = (audcadEntryPrice - askd);
								audcadPosMoveShortDec = (audcadEntryPrice - askd) * audcadPosLot;
								
								// SHORT: Buy Trigger
								if (audcadShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									audcadJustSold = true;
									sOfferID = "16";
									iAmount = Convert.ToInt32(audcadPosLot);
									sBuySell = "B";
									sTradeID = audcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + audcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (audcadShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audcadJustSold = true;
									sOfferID = "16";
									iAmount = Convert.ToInt32(audcadPosLot);
									sBuySell = "B";
									sTradeID = audcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								audcadSideString = "";
							}
						}
					}

					// If triggered pair is "AUD/JPY"
					if (offerTableRow.Instrument == "AUD/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { audjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { audjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { audjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (audjpyPastBox.Text == "--" && audjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { audjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { audjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (audjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - audjpyPast);
							double moveConvert = moveCalc * 10000;
							audjpyMove = (decimal)(moveCalc);
							if (audjpyMove > highestMove)
							{
								highestMove = audjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (audjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									audjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (audjpyMove >= buyLong & startSpread < maxSpread && audjpyEntry.Text == "--" && audjpyReEnter == false && audjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "17";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										audjpyJustSold = false;
										audjpyReEnter = true;
										audjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (audjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									audjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (audjpyMove <= sellShort & startSpread < maxSpread && audjpyEntry.Text == "--" && audjpyReEnter == false && audjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "17";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										audjpyJustSold = false;
										audjpyReEnter = true;
										audjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { audjpyMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (audjpySideString == "Long" && audjpyJustSold == false)
							{
								// Subtract entry price from bid price
								audjpyBuyLongCheck = (bidd - audjpyEntryPrice);
								audjpyPosMoveDec = (bidd - audjpyEntryPrice) * audjpyPosLot;
								
								// LONG: Sell Trigger
								if (audjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									audjpyJustSold = true;
									audjpyReEnter = true;
									sOfferID = "17";
									iAmount = Convert.ToInt32(audjpyPosLot);
									sBuySell = "S";
									sTradeID = audjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + audjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (audjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audjpyJustSold = true;
									audjpyReEnter = true;
									sOfferID = "17";
									iAmount = Convert.ToInt32(audjpyPosLot);
									sBuySell = "S";
									sTradeID = audjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (audjpySideString == "Short" && audjpyJustSold == false)
							{
								// Subtract entry price from bid price
								audjpyShortSaleCheck = (audjpyEntryPrice - askd);
								audjpyPosMoveShortDec = (audjpyEntryPrice - askd) * audjpyPosLot;
								
								// SHORT: Buy Trigger
								if (audjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									audjpyJustSold = true;
									audjpyReEnter = true;
									sOfferID = "17";
									iAmount = Convert.ToInt32(audjpyPosLot);
									sBuySell = "B";
									sTradeID = audjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + audjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (audjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audjpyJustSold = true;
									audjpyReEnter = true;
									sOfferID = "17";
									iAmount = Convert.ToInt32(audjpyPosLot);
									sBuySell = "B";
									sTradeID = audjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { audjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								audjpySideString = "";
							}
						}
					}

					// If triggered pair is "AUD/USD"
					if (offerTableRow.Instrument == "AUD/USD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { audusdBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { audusdAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { audusdSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (audusdPastBox.Text == "--" && audusdAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { audusdPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { audusdPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (audusdPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - audusdPast);
							double moveConvert = moveCalc * 10000;
							audusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (audusdMove > highestMove)
							{
								highestMove = audusdMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (audusdMove > 0)
								{
									audusdMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { audusdMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (audusdMove >= buyLong & startSpread < maxSpread && audusdEntry.Text == "--" && audusdReEnter == false && audusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "6";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										audusdJustSold = false;
										audusdReEnter = true;
										audusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (audusdMove < 0)
								{
									audusdMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { audusdMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (audusdMove <= sellShort & startSpread < maxSpread && audusdEntry.Text == "--" && audusdReEnter == false && audusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "6";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										audusdJustSold = false;
										audusdReEnter = true;
										audusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { audusdMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { audusdMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (audusdSideString == "Long" && audusdJustSold == false)
							{
								// Subtract entry price from bid price
								audusdBuyLongCheck = (bidd - audusdEntryPrice);
								audusdPosMoveDec = (bidd - audusdEntryPrice) * audusdPosLot;


								// LONG: Sell Trigger
								if (audusdBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									audusdJustSold = true;
									audusdReEnter = true;
									sOfferID = "6";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = audusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + audusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (audusdBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audusdJustSold = true;
									audusdReEnter = true;
									sOfferID = "6";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = audusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { audusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (audusdSideString == "Short" && audusdJustSold == false)
							{
								// Subtract entry price from bid price
								audusdShortSaleCheck = (audusdEntryPrice - askd);
								audusdPosMoveShortDec = (audusdEntryPrice - askd) * audusdPosLot;

								// SHORT: Buy Trigger
								if (audusdShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									audusdJustSold = true;
									audusdReEnter = true;
									sOfferID = "6";
									iAmount = Convert.ToInt32(audusdPosLot);
									sBuySell = "B";
									sTradeID = audusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + audusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (audusdShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									audusdJustSold = true;
									audusdReEnter = true;
									sOfferID = "6";
									iAmount = Convert.ToInt32(audusdPosLot);
									sBuySell = "B";
									sTradeID = audusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { audusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { audusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + audusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								audusdSideString = "";
							}
						}
					}

					// If triggered pair is "CAD/JPY"
					if (offerTableRow.Instrument == "CAD/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { cadjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { cadjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { cadjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (cadjpyPastBox.Text == "--" && cadjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { cadjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { cadjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (cadjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - cadjpyPast);
							double moveConvert = moveCalc * 10000;
							cadjpyMove = (decimal)(moveCalc);
							if (cadjpyMove > highestMove)
							{
								highestMove = cadjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (cadjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									cadjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (cadjpyMove >= buyLong & startSpread < maxSpread && cadjpyEntry.Text == "--" && cadjpyReEnter == false && cadjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "18";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										cadjpyJustSold = false;
										cadjpyReEnter = true;
										cadjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (cadjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									cadjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (cadjpyMove <= sellShort & startSpread < maxSpread && cadjpyEntry.Text == "--" && cadjpyReEnter == false && cadjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "18";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										cadjpyJustSold = false;
										cadjpyReEnter = true;
										cadjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpyMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (cadjpySideString == "Long" && cadjpyJustSold == false)
							{
								// Subtract entry price from bid price
								cadjpyBuyLongCheck = (bidd - cadjpyEntryPrice);
								cadjpyPosMoveDec = (bidd - cadjpyEntryPrice) * cadjpyPosLot;

								// LONG: Sell Trigger
								if (cadjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									cadjpyJustSold = true;
									cadjpyReEnter = true;
									sOfferID = "18";
									iAmount = Convert.ToInt32(cadjpyPosLot);
									sBuySell = "S";
									sTradeID = cadjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { cadjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + cadjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (cadjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									cadjpyJustSold = true;
									cadjpyReEnter = true;
									sOfferID = "18";
									iAmount = Convert.ToInt32(cadjpyPosLot);
									sBuySell = "S";
									sTradeID = cadjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { cadjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + cadjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (cadjpySideString == "Short" && cadjpyJustSold == false)
							{
								// Subtract entry price from bid price
								cadjpyShortSaleCheck = (cadjpyEntryPrice - askd);
								cadjpyPosMoveShortDec = (cadjpyEntryPrice - askd) * cadjpyPosLot;

								// SHORT: Buy Trigger
								if (cadjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									cadjpyJustSold = true;
									cadjpyReEnter = true;
									sOfferID = "18";
									iAmount = Convert.ToInt32(cadjpyPosLot);
									sBuySell = "B";
									sTradeID = cadjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { cadjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + cadjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (cadjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									cadjpyJustSold = true;
									cadjpyReEnter = true;
									sOfferID = "18";
									iAmount = Convert.ToInt32(cadjpyPosLot);
									sBuySell = "B";
									sTradeID = cadjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { cadjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { cadjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + cadjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								cadjpySideString = "";
							}
						}
					}

					// If triggered pair is "CHF/JPY"
					if (offerTableRow.Instrument == "CHF/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { chfjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { chfjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { chfjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (chfjpyPastBox.Text == "--" && chfjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { chfjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { chfjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (chfjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - chfjpyPast);
							double moveConvert = moveCalc * 10000;
							chfjpyMove = (decimal)(moveCalc);
							if (chfjpyMove > highestMove)
							{
								highestMove = chfjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (chfjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									chfjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (chfjpyMove >= buyLong & startSpread < maxSpread && chfjpyEntry.Text == "--" && chfjpyReEnter == false && chfjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "12";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										chfjpyJustSold = false;
										chfjpyReEnter = true;
										chfjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (chfjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									chfjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (chfjpyMove <= sellShort & startSpread < maxSpread && chfjpyEntry.Text == "--" && chfjpyReEnter == false && chfjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "12";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										chfjpyJustSold = false;
										chfjpyReEnter = true;
										chfjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpyMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (chfjpySideString == "Long" && chfjpyJustSold == false)
							{
								// Subtract entry price from bid price
								chfjpyBuyLongCheck = (bidd - chfjpyEntryPrice);
								chfjpyPosMoveDec = (bidd - chfjpyEntryPrice) * chfjpyPosLot;

								// LONG: Sell Trigger
								if (chfjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									chfjpyJustSold = true;
									chfjpyReEnter = true;
									sOfferID = "12";
									iAmount = Convert.ToInt32(chfjpyPosLot);
									sBuySell = "S";
									sTradeID = chfjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { chfjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + chfjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (chfjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									chfjpyJustSold = true;
									chfjpyReEnter = true;
									sOfferID = "12";
									iAmount = Convert.ToInt32(chfjpyPosLot);
									sBuySell = "S";
									sTradeID = chfjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { chfjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + chfjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (chfjpySideString == "Short" && chfjpyJustSold == false)
							{
								// Subtract entry price from bid price
								chfjpyShortSaleCheck = (chfjpyEntryPrice - askd);
								chfjpyPosMoveShortDec = (chfjpyEntryPrice - askd) * chfjpyPosLot;

								// SHORT: Buy Trigger
								if (chfjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									chfjpyJustSold = true;
									chfjpyReEnter = true;
									sOfferID = "12";
									iAmount = Convert.ToInt32(chfjpyPosLot);
									sBuySell = "B";
									sTradeID = chfjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { chfjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + chfjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (chfjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									chfjpyJustSold = true;
									chfjpyReEnter = true;
									sOfferID = "12";
									iAmount = Convert.ToInt32(chfjpyPosLot);
									sBuySell = "B";
									sTradeID = chfjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { chfjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { chfjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + chfjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								chfjpySideString = "";
							}
						}
					}

					// If triggered pair is "EUR/AUD"
					if (offerTableRow.Instrument == "EUR/AUD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { euraudBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { euraudAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { euraudSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (euraudPastBox.Text == "--" && euraudAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { euraudPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { euraudPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (euraudPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - euraudPast);
							double moveConvert = moveCalc * 10000;
							euraudMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (euraudMove > highestMove)
							{
								highestMove = euraudMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (euraudMove > 0)
								{
									euraudMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { euraudMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (euraudMove >= buyLong & startSpread < maxSpread && euraudEntry.Text == "--" && euraudReEnter == false && euraudInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "14";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										euraudJustSold = false;
										euraudReEnter = true;
										euraudSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (euraudMove < 0)
								{
									euraudMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { euraudMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (euraudMove <= sellShort & startSpread < maxSpread && euraudEntry.Text == "--" && euraudReEnter == false && euraudInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "14";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										euraudJustSold = false;
										euraudSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { euraudMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { euraudMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (euraudSideString == "Long" && euraudJustSold == false)
							{
								// Subtract entry price from bid price
								euraudBuyLongCheck = (bidd - euraudEntryPrice);
								euraudPosMoveDec = (bidd - euraudEntryPrice) * euraudPosLot;


								// LONG: Sell Trigger
								if (euraudBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									euraudJustSold = true;
									sOfferID = "14";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = euraudOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { euraudPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + euraudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (euraudBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									euraudJustSold = true;
									euraudReEnter = true;
									sOfferID = "14";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = euraudOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { euraudPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + euraudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (euraudSideString == "Short" && euraudJustSold == false)
							{
								// Subtract entry price from bid price
								euraudShortSaleCheck = (euraudEntryPrice - askd);
								euraudPosMoveShortDec = (euraudEntryPrice - askd) * euraudPosLot;

								// SHORT: Buy Trigger
								if (euraudShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									euraudJustSold = true;
									euraudReEnter = true;
									sOfferID = "14";
									iAmount = Convert.ToInt32(euraudPosLot);
									sBuySell = "B";
									sTradeID = euraudOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { euraudPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + euraudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (euraudShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									euraudJustSold = true;
									euraudReEnter = true;
									sOfferID = "14";
									iAmount = Convert.ToInt32(euraudPosLot);
									sBuySell = "B";
									sTradeID = euraudOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { euraudPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { euraudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + euraudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								euraudSideString = "";
							}
						}
					}

					// If triggered pair is "EUR/CAD"
					if (offerTableRow.Instrument == "EUR/CAD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { eurcadBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { eurcadAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { eurcadSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (eurcadPastBox.Text == "--" && eurcadAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { eurcadPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { eurcadPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (eurcadPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - eurcadPast);
							double moveConvert = moveCalc * 10000;
							eurcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (eurcadMove > highestMove)
							{
								highestMove = eurcadMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (eurcadMove > 0)
								{
									eurcadMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (eurcadMove >= buyLong & startSpread < maxSpread && eurcadEntry.Text == "--" && eurcadReEnter == false && eurcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "15";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										eurcadJustSold = false;
										eurcadReEnter = true;
										eurcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (eurcadMove < 0)
								{
									eurcadMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (eurcadMove <= sellShort & startSpread < maxSpread && eurcadEntry.Text == "--" && eurcadReEnter == false && eurcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "15";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										eurcadJustSold = false;
										eurcadReEnter = true;
										eurcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (eurcadSideString == "Long" && eurcadJustSold == false)
							{
								// Subtract entry price from bid price
								eurcadBuyLongCheck = (bidd - eurcadEntryPrice);
								eurcadPosMoveDec = (bidd - eurcadEntryPrice) * eurcadPosLot;


								// LONG: Sell Trigger
								if (eurcadBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurcadJustSold = true;
									eurcadReEnter = true;
									sOfferID = "15";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + eurcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (eurcadBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurcadJustSold = true;
									eurcadReEnter = true;
									sOfferID = "15";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (eurcadSideString == "Short" && eurcadJustSold == false)
							{
								// Subtract entry price from bid price
								eurcadShortSaleCheck = (eurcadEntryPrice - askd);
								eurcadPosMoveShortDec = (eurcadEntryPrice - askd) * eurcadPosLot;

								// SHORT: Buy Trigger
								if (eurcadShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurcadJustSold = true;
									eurcadReEnter = true;
									sOfferID = "15";
									iAmount = Convert.ToInt32(eurcadPosLot);
									sBuySell = "B";
									sTradeID = eurcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + eurcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (eurcadShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurcadJustSold = true;
									eurcadReEnter = true;
									sOfferID = "15";
									iAmount = Convert.ToInt32(eurcadPosLot);
									sBuySell = "B";
									sTradeID = eurcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurcadPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								eurcadSideString = "";
							}
						}
					}

					// If triggered pair is "EUR/CHF"
					if (offerTableRow.Instrument == "EUR/CHF")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { eurchfBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { eurchfAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { eurchfSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (eurchfPastBox.Text == "--" && eurchfAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { eurchfPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { eurchfPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (eurchfPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - eurchfPast);
							double moveConvert = moveCalc * 10000;
							eurchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (eurchfMove > highestMove)
							{
								highestMove = eurchfMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (eurchfMove > 0)
								{
									eurchfMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (eurchfMove >= buyLong & startSpread < maxSpread && eurchfEntry.Text == "--" && eurchfReEnter == false && eurchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "5";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										eurchfJustSold = false;
										eurchfReEnter = true;
										eurchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (eurchfMove < 0)
								{
									eurchfMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (eurchfMove <= sellShort & startSpread < maxSpread && eurchfEntry.Text == "--" && eurchfReEnter == false && eurchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "5";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										eurchfJustSold = false;
										eurchfReEnter = true;
										eurchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (eurchfSideString == "Long" && eurchfJustSold == false)
							{
								// Subtract entry price from bid price
								eurchfBuyLongCheck = (bidd - eurchfEntryPrice);
								eurchfPosMoveDec = (bidd - eurchfEntryPrice) * eurchfPosLot;


								// LONG: Sell Trigger
								if (eurchfBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurchfJustSold = true;
									eurchfReEnter = true;
									sOfferID = "5";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + eurchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (eurchfBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurchfJustSold = true;
									eurchfReEnter = true;
									sOfferID = "5";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (eurchfSideString == "Short" && eurchfJustSold == false)
							{
								// Subtract entry price from bid price
								eurchfShortSaleCheck = (eurchfEntryPrice - askd);
								eurchfPosMoveShortDec = (eurchfEntryPrice - askd) * eurchfPosLot;

								// SHORT: Buy Trigger
								if (eurchfShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurchfJustSold = true;
									eurchfReEnter = true;
									sOfferID = "5";
									iAmount = Convert.ToInt32(eurchfPosLot);
									sBuySell = "B";
									sTradeID = eurchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + eurchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (eurchfShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurchfJustSold = true;
									eurchfReEnter = true;
									sOfferID = "5";
									iAmount = Convert.ToInt32(eurchfPosLot);
									sBuySell = "B";
									sTradeID = eurchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								eurchfSideString = "";
							}
						}
					}

					// If triggered pair is "EUR/JPY"
					if (offerTableRow.Instrument == "EUR/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { eurjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { eurjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { eurjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (eurjpyPastBox.Text == "--" && eurjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { eurjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { eurjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (eurjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - eurjpyPast);
							double moveConvert = moveCalc * 10000;
							eurjpyMove = (decimal)(moveCalc);
							if (eurjpyMove > highestMove)
							{
								highestMove = eurjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (eurjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									eurjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (eurjpyMove >= buyLong & startSpread < maxSpread && eurjpyEntry.Text == "--" && eurjpyReEnter == false && eurjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "10";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										eurjpyJustSold = false;
										eurjpyReEnter = true;
										eurjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (eurjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									eurjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (eurjpyMove <= sellShort & startSpread < maxSpread && eurjpyEntry.Text == "--" && eurjpyReEnter == false && eurjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "10";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										eurjpyJustSold = false;
										eurjpyReEnter = true;
										eurjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpyMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (eurjpySideString == "Long" && eurjpyJustSold == false)
							{
								// Subtract entry price from bid price
								eurjpyBuyLongCheck = (bidd - eurjpyEntryPrice);
								eurjpyPosMoveDec = (bidd - eurjpyEntryPrice) * eurjpyPosLot;

								// LONG: Sell Trigger
								if (eurjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurjpyJustSold = true;
									eurjpyReEnter = true;
									sOfferID = "10";
									iAmount = Convert.ToInt32(eurjpyPosLot);
									sBuySell = "S";
									sTradeID = eurjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + eurjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (eurjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurjpyJustSold = true;
									eurjpyReEnter = true;
									sOfferID = "10";
									iAmount = Convert.ToInt32(eurjpyPosLot);
									sBuySell = "S";
									sTradeID = eurjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (eurjpySideString == "Short" && eurjpyJustSold == false)
							{
								// Subtract entry price from bid price
								eurjpyShortSaleCheck = (eurjpyEntryPrice - askd);
								eurjpyPosMoveShortDec = (eurjpyEntryPrice - askd) * eurjpyPosLot;

								// SHORT: Buy Trigger
								if (eurjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurjpyJustSold = true;
									eurjpyReEnter = true;
									sOfferID = "10";
									iAmount = Convert.ToInt32(eurjpyPosLot);
									sBuySell = "B";
									sTradeID = eurjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + eurjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (eurjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurjpyJustSold = true;
									eurjpyReEnter = true;
									sOfferID = "10";
									iAmount = Convert.ToInt32(eurjpyPosLot);
									sBuySell = "B";
									sTradeID = eurjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { eurjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								eurjpySideString = "";
							}
						}
					}

					// If triggered pair is "EUR/USD"
					if (offerTableRow.Instrument == "EUR/USD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { eurusdBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { eurusdAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { eurusdSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (eurusdPastBox.Text == "--" && eurusdAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { eurusdPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { eurusdPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (eurusdPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - eurusdPast);
							double moveConvert = moveCalc * 10000;
							eurusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (eurusdMove > highestMove)
							{
								highestMove = eurusdMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (eurusdMove > 0)
								{
									eurusdMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (eurusdMove >= buyLong & startSpread < maxSpread && eurusdEntry.Text == "--" && eurusdReEnter == false && eurusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "1";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										eurusdJustSold = false;
										eurusdReEnter = true;
										eurusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (eurusdMove < 0)
								{
									eurusdMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (eurusdMove <= sellShort & startSpread < maxSpread && eurusdEntry.Text == "--" && eurusdReEnter == false && eurusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "1";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										eurusdJustSold = false;
										eurusdReEnter = true;
										eurusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (eurusdSideString == "Long" && eurusdJustSold == false)
							{
								// Subtract entry price from bid price
								eurusdBuyLongCheck = (bidd - eurusdEntryPrice);
								eurusdPosMoveDec = (bidd - eurusdEntryPrice) * eurusdPosLot;


								// LONG: Sell Trigger
								if (eurusdBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurusdJustSold = true;
									eurusdReEnter = true;
									sOfferID = "1";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurusdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + eurusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (eurusdBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurusdJustSold = true;
									eurusdReEnter = true;
									sOfferID = "1";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = eurusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { eurusdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (eurusdSideString == "Short" && eurusdJustSold == false)
							{
								// Subtract entry price from bid price
								eurusdShortSaleCheck = (eurusdEntryPrice - askd);
								eurusdPosMoveShortDec = (eurusdEntryPrice - askd) * eurusdPosLot;

								// SHORT: Buy Trigger
								if (eurusdShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									eurusdJustSold = true;
									eurusdReEnter = true;
									sOfferID = "1";
									iAmount = Convert.ToInt32(eurusdPosLot);
									sBuySell = "B";
									sTradeID = eurusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurusdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + eurusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (eurusdShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									eurusdJustSold = true;
									eurusdReEnter = true;
									sOfferID = "1";
									iAmount = Convert.ToInt32(eurusdPosLot);
									sBuySell = "B";
									sTradeID = eurusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { eurusdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { eurusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { eurusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + eurusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								eurusdSideString = "";
							}
						}
					}

					// If triggered pair is "GBP/AUD"
					if (offerTableRow.Instrument == "GBP/AUD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { gbpaudBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { gbpaudAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { gbpaudSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (gbpaudPastBox.Text == "--" && gbpaudAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { gbpaudPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { gbpaudPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (gbpaudPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - gbpaudPast);
							double moveConvert = moveCalc * 10000;
							gbpaudMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (gbpaudMove > highestMove)
							{
								highestMove = gbpaudMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (gbpaudMove > 0)
								{
									gbpaudMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (gbpaudMove >= buyLong & startSpread < maxSpread && gbpaudEntry.Text == "--" && gbpaudReEnter == false && gbpaudInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "22";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										gbpaudJustSold = false;
										gbpaudReEnter = true;
										gbpaudSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (gbpaudMove < 0)
								{
									gbpaudMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (gbpaudMove <= sellShort & startSpread < maxSpread && gbpaudEntry.Text == "--" && gbpaudReEnter == false && gbpaudInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "22";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										gbpaudJustSold = false;
										gbpaudReEnter = true;
										gbpaudSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (gbpaudSideString == "Long" && gbpaudJustSold == false)
							{
								// Subtract entry price from bid price
								gbpaudBuyLongCheck = (bidd - gbpaudEntryPrice);
								gbpaudPosMoveDec = (bidd - gbpaudEntryPrice) * gbpaudPosLot;


								// LONG: Sell Trigger
								if (gbpaudBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpaudJustSold = true;
									gbpaudReEnter = true;
									sOfferID = "22";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpaudOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpaudPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpaudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (gbpaudBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpaudJustSold = true;
									gbpaudReEnter = true;
									sOfferID = "22";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpaudOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpaudPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpaudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (gbpaudSideString == "Short" && gbpaudJustSold == false)
							{
								// Subtract entry price from bid price
								gbpaudShortSaleCheck = (gbpaudEntryPrice - askd);
								gbpaudPosMoveShortDec = (gbpaudEntryPrice - askd) * gbpaudPosLot;

								// SHORT: Buy Trigger
								if (gbpaudShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpaudJustSold = true;
									gbpaudReEnter = true;
									sOfferID = "22";
									iAmount = Convert.ToInt32(gbpaudPosLot);
									sBuySell = "B";
									sTradeID = gbpaudOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpaudPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + gbpaudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (gbpaudShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpaudJustSold = true;
									gbpaudReEnter = true;
									sOfferID = "22";
									iAmount = Convert.ToInt32(gbpaudPosLot);
									sBuySell = "B";
									sTradeID = gbpaudOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpaudPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpaudPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpaudEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpaudPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								gbpaudSideString = "";
							}
						}
					}

					// If triggered pair is "GBP/CHF"
					if (offerTableRow.Instrument == "GBP/CHF")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { gbpchfBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { gbpchfAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { gbpchfSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (gbpchfPastBox.Text == "--" && gbpchfAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { gbpchfPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { gbpchfPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (gbpchfPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - gbpchfPast);
							double moveConvert = moveCalc * 10000;
							gbpchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (gbpchfMove > highestMove)
							{
								highestMove = gbpchfMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (gbpchfMove > 0)
								{
									gbpchfMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (gbpchfMove >= buyLong & startSpread < maxSpread && gbpchfEntry.Text == "--" && gbpchfReEnter == false && gbpchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "13";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										gbpchfJustSold = false;
										gbpchfReEnter = true;
										gbpchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (gbpchfMove < 0)
								{
									gbpchfMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (gbpchfMove <= sellShort & startSpread < maxSpread && gbpchfEntry.Text == "--" && gbpchfReEnter == false && gbpchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "13";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										gbpchfJustSold = false;
										gbpchfReEnter = true;
										gbpchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (gbpchfSideString == "Long" && gbpchfJustSold == false)
							{
								// Subtract entry price from bid price
								gbpchfBuyLongCheck = (bidd - gbpchfEntryPrice);
								gbpchfPosMoveDec = (bidd - gbpchfEntryPrice) * gbpchfPosLot;


								// LONG: Sell Trigger
								if (gbpchfBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpchfJustSold = true;
									gbpchfReEnter = true;
									sOfferID = "13";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (gbpchfBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpchfJustSold = true;
									gbpchfReEnter = true;
									sOfferID = "13";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (gbpchfSideString == "Short" && gbpchfJustSold == false)
							{
								// Subtract entry price from bid price
								gbpchfShortSaleCheck = (gbpchfEntryPrice - askd);
								gbpchfPosMoveShortDec = (gbpchfEntryPrice - askd) * gbpchfPosLot;

								// SHORT: Buy Trigger
								if (gbpchfShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpchfJustSold = true;
									gbpchfReEnter = true;
									sOfferID = "13";
									iAmount = Convert.ToInt32(gbpchfPosLot);
									sBuySell = "B";
									sTradeID = gbpchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + gbpchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (gbpchfShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpchfJustSold = true;
									gbpchfReEnter = true;
									sOfferID = "13";
									iAmount = Convert.ToInt32(gbpchfPosLot);
									sBuySell = "B";
									sTradeID = gbpchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpchfPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								gbpchfSideString = "";
							}
						}
					}

					// If triggered pair is "GBP/JPY"
					if (offerTableRow.Instrument == "GBP/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { gbpjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { gbpjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { gbpjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (gbpjpyPastBox.Text == "--" && gbpjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { gbpjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { gbpjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (gbpjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - gbpjpyPast);
							double moveConvert = moveCalc * 10000;
							gbpjpyMove = (decimal)(moveCalc);
							if (gbpjpyMove > highestMove)
							{
								highestMove = gbpjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (gbpjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									gbpjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (gbpjpyMove >= buyLong & startSpread < maxSpread && gbpjpyEntry.Text == "--" && gbpjpyReEnter == false && gbpjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "11";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										gbpjpyJustSold = false;
										gbpjpyReEnter = true;
										gbpjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (gbpjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									gbpjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (gbpjpyMove <= sellShort & startSpread < maxSpread && gbpjpyEntry.Text == "--" && gbpjpyReEnter == false && gbpjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "11";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										gbpjpyJustSold = false;
										gbpjpyReEnter = true;
										gbpjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (gbpjpySideString == "Long" && gbpjpyJustSold == false)
							{
								// Subtract entry price from bid price
								gbpjpyBuyLongCheck = (bidd - gbpjpyEntryPrice);
								gbpjpyPosMoveDec = (bidd - gbpjpyEntryPrice) * gbpjpyPosLot;

								// LONG: Sell Trigger
								if (gbpjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpjpyJustSold = true;
									gbpjpyReEnter = true;
									sOfferID = "11";
									iAmount = Convert.ToInt32(gbpjpyPosLot);
									sBuySell = "S";
									sTradeID = gbpjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (gbpjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpjpyJustSold = true;
									gbpjpyReEnter = true;
									sOfferID = "11";
									iAmount = Convert.ToInt32(gbpjpyPosLot);
									sBuySell = "S";
									sTradeID = gbpjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (gbpjpySideString == "Short" && gbpjpyJustSold == false)
							{
								// Subtract entry price from bid price
								gbpjpyShortSaleCheck = (gbpjpyEntryPrice - askd);
								gbpjpyPosMoveShortDec = (gbpjpyEntryPrice - askd) * gbpjpyPosLot;

								// SHORT: Buy Trigger
								if (gbpjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpjpyJustSold = true;
									gbpjpyReEnter = true;
									sOfferID = "11";
									iAmount = Convert.ToInt32(gbpjpyPosLot);
									sBuySell = "B";
									sTradeID = gbpjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (gbpjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpjpyJustSold = true;
									gbpjpyReEnter = true;
									sOfferID = "11";
									iAmount = Convert.ToInt32(gbpjpyPosLot);
									sBuySell = "B";
									sTradeID = gbpjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpjpyPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								gbpjpySideString = "";
							}
						}
					}

					// If triggered pair is "GBP/NZD"
					if (offerTableRow.Instrument == "GBP/NZD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { gbpnzdBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { gbpnzdAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { gbpnzdSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (gbpnzdPastBox.Text == "--" && gbpnzdAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { gbpnzdPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { gbpnzdPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (gbpnzdPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - gbpnzdPast);
							double moveConvert = moveCalc * 10000;
							gbpnzdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (gbpnzdMove > highestMove)
							{
								highestMove = gbpnzdMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (gbpnzdMove > 0)
								{
									gbpnzdMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (gbpnzdMove >= buyLong & startSpread < maxSpread && gbpnzdEntry.Text == "--" && gbpnzdReEnter == false && gbpnzdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "21";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										gbpnzdJustSold = false;
										gbpnzdReEnter = true;
										gbpnzdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (gbpnzdMove < 0)
								{
									gbpnzdMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (gbpnzdMove <= sellShort & startSpread < maxSpread && gbpnzdEntry.Text == "--" && gbpnzdReEnter == false && gbpnzdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "21";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										gbpnzdJustSold = false;
										gbpnzdReEnter = true;
										gbpnzdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (gbpnzdSideString == "Long" && gbpnzdJustSold == false)
							{
								// Subtract entry price from bid price
								gbpnzdBuyLongCheck = (bidd - gbpnzdEntryPrice);
								gbpnzdPosMoveDec = (bidd - gbpnzdEntryPrice) * gbpnzdPosLot;


								// LONG: Sell Trigger
								if (gbpnzdBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpnzdJustSold = true;
									gbpnzdReEnter = true;
									sOfferID = "21";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpnzdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpnzdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpnzdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (gbpnzdBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpnzdJustSold = true;
									gbpnzdReEnter = true;
									sOfferID = "21";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpnzdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpnzdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpnzdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (gbpnzdSideString == "Short" && gbpnzdJustSold == false)
							{
								// Subtract entry price from bid price
								gbpnzdShortSaleCheck = (gbpnzdEntryPrice - askd);
								gbpnzdPosMoveShortDec = (gbpnzdEntryPrice - askd) * gbpnzdPosLot;

								// SHORT: Buy Trigger
								if (gbpnzdShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpnzdJustSold = true;
									gbpnzdReEnter = true;
									sOfferID = "21";
									iAmount = Convert.ToInt32(gbpnzdPosLot);
									sBuySell = "B";
									sTradeID = gbpnzdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpnzdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + gbpnzdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (gbpnzdShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpnzdJustSold = true;
									gbpnzdReEnter = true;
									sOfferID = "21";
									iAmount = Convert.ToInt32(gbpnzdPosLot);
									sBuySell = "B";
									sTradeID = gbpnzdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpnzdPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpnzdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpnzdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								gbpnzdSideString = "";
							}
						}
					}

					// If triggered pair is "GBP/USD"
					if (offerTableRow.Instrument == "GBP/USD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { gbpusdBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { gbpusdAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { gbpusdSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (gbpusdPastBox.Text == "--" && gbpusdAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { gbpusdPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { gbpusdPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (gbpusdPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - gbpusdPast);
							double moveConvert = moveCalc * 10000;
							gbpusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (gbpusdMove > highestMove)
							{
								highestMove = gbpusdMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (gbpusdMove > 0)
								{
									gbpusdMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (gbpusdMove >= buyLong & startSpread < maxSpread && gbpusdEntry.Text == "--" && gbpusdReEnter == false && gbpusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "3";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										gbpusdJustSold = false;
										gbpusdReEnter = true;
										gbpusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (gbpusdMove < 0)
								{
									gbpusdMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (gbpusdMove <= sellShort & startSpread < maxSpread && gbpusdEntry.Text == "--" && gbpusdReEnter == false && gbpusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "3";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										gbpusdJustSold = false;
										gbpusdReEnter = true;
										gbpusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (gbpusdSideString == "Long" && gbpusdJustSold == false)
							{
								// Subtract entry price from bid price
								gbpusdBuyLongCheck = (bidd - gbpusdEntryPrice);
								gbpusdPosMoveDec = (bidd - gbpusdEntryPrice) * gbpusdPosLot;


								// LONG: Sell Trigger
								if (gbpusdBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpusdJustSold = true;
									gbpusdReEnter = true;
									sOfferID = "3";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + gbpusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (gbpusdBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpusdJustSold = true;
									gbpusdReEnter = true;
									sOfferID = "3";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = gbpusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { gbpusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (gbpusdSideString == "Short" && gbpusdJustSold == false)
							{
								// Subtract entry price from bid price
								gbpusdShortSaleCheck = (gbpusdEntryPrice - askd);
								gbpusdPosMoveShortDec = (gbpusdEntryPrice - askd) * gbpusdPosLot;

								// SHORT: Buy Trigger
								if (gbpusdShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									gbpusdJustSold = true;
									gbpusdReEnter = true;
									sOfferID = "3";
									iAmount = Convert.ToInt32(gbpusdPosLot);
									sBuySell = "B";
									sTradeID = gbpusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + gbpusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (gbpusdShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									gbpusdJustSold = true;
									gbpusdReEnter = true;
									sOfferID = "3";
									iAmount = Convert.ToInt32(gbpusdPosLot);
									sBuySell = "B";
									sTradeID = gbpusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { gbpusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { gbpusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { gbpusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + gbpusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								gbpusdSideString = "";
							}
						}
					}

					// If triggered pair is "NZD/JPY"
					if (offerTableRow.Instrument == "NZD/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { nzdjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { nzdjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { nzdjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (nzdjpyPastBox.Text == "--" && nzdjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { nzdjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { nzdjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (nzdjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - nzdjpyPast);
							double moveConvert = moveCalc * 10000;
							nzdjpyMove = (decimal)(moveCalc);
							if (nzdjpyMove > highestMove)
							{
								highestMove = nzdjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (nzdjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									nzdjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (nzdjpyMove >= buyLong & startSpread < maxSpread && nzdjpyEntry.Text == "--" && nzdjpyReEnter == false && nzdjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "19";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										nzdjpyJustSold = false;
										nzdjpyReEnter = true;
										nzdjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (nzdjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									nzdjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (nzdjpyMove <= sellShort & startSpread < maxSpread && nzdjpyEntry.Text == "--" && nzdjpyReEnter == false && nzdjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "19";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										nzdjpyJustSold = false;
										nzdjpyReEnter = true;
										nzdjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (nzdjpySideString == "Long" && nzdjpyJustSold == false)
							{
								// Subtract entry price from bid price
								nzdjpyBuyLongCheck = (bidd - nzdjpyEntryPrice);
								nzdjpyPosMoveDec = (bidd - nzdjpyEntryPrice) * nzdjpyPosLot;

								// LONG: Sell Trigger
								if (nzdjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									nzdjpyJustSold = true;
									nzdjpyReEnter = true;
									sOfferID = "19";
									iAmount = Convert.ToInt32(nzdjpyPosLot);
									sBuySell = "S";
									sTradeID = nzdjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { nzdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + nzdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (nzdjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									nzdjpyJustSold = true;
									nzdjpyReEnter = true;
									sOfferID = "19";
									iAmount = Convert.ToInt32(nzdjpyPosLot);
									sBuySell = "S";
									sTradeID = nzdjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { nzdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + nzdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (nzdjpySideString == "Short" && nzdjpyJustSold == false)
							{
								// Subtract entry price from bid price
								nzdjpyShortSaleCheck = (nzdjpyEntryPrice - askd);
								nzdjpyPosMoveShortDec = (nzdjpyEntryPrice - askd) * nzdjpyPosLot;

								// SHORT: Buy Trigger
								if (nzdjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									nzdjpyJustSold = true;
									nzdjpyReEnter = true;
									sOfferID = "19";
									iAmount = Convert.ToInt32(nzdjpyPosLot);
									sBuySell = "B";
									sTradeID = nzdjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { nzdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + nzdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (nzdjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									nzdjpyJustSold = true;
									nzdjpyReEnter = true;
									sOfferID = "19";
									iAmount = Convert.ToInt32(nzdjpyPosLot);
									sBuySell = "B";
									sTradeID = nzdjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { nzdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { nzdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + nzdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								nzdjpySideString = "";
							}
						}
					}

					// If triggered pair is "NZD/USD"
					if (offerTableRow.Instrument == "NZD/USD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { nzdusdBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { nzdusdAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { nzdusdSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (nzdusdPastBox.Text == "--" && nzdusdAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { nzdusdPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { nzdusdPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (nzdusdPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - nzdusdPast);
							double moveConvert = moveCalc * 10000;
							nzdusdMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (nzdusdMove > highestMove)
							{
								highestMove = nzdusdMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (nzdusdMove > 0)
								{
									nzdusdMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (nzdusdMove >= buyLong & startSpread < maxSpread && nzdusdEntry.Text == "--" && nzdusdReEnter == false && nzdusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "8";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										nzdusdJustSold = false;
										nzdusdReEnter = true;
										nzdusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (nzdusdMove < 0)
								{
									nzdusdMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (nzdusdMove <= sellShort & startSpread < maxSpread && nzdusdEntry.Text == "--" && nzdusdReEnter == false && nzdusdInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "8";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										nzdusdJustSold = false;
										nzdusdReEnter = true;
										nzdusdSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (nzdusdSideString == "Long" && nzdusdJustSold == false)
							{
								// Subtract entry price from bid price
								nzdusdBuyLongCheck = (bidd - nzdusdEntryPrice);
								nzdusdPosMoveDec = (bidd - nzdusdEntryPrice) * nzdusdPosLot;


								// LONG: Sell Trigger
								if (nzdusdBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									nzdusdJustSold = true;
									nzdusdReEnter = true;
									sOfferID = "8";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = nzdusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { nzdusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + nzdusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (nzdusdBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									nzdusdJustSold = true;
									nzdusdReEnter = true;
									sOfferID = "8";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = nzdusdOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { nzdusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + nzdusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (nzdusdSideString == "Short" && nzdusdJustSold == false)
							{
								// Subtract entry price from bid price
								nzdusdShortSaleCheck = (nzdusdEntryPrice - askd);
								nzdusdPosMoveShortDec = (nzdusdEntryPrice - askd) * nzdusdPosLot;

								// SHORT: Buy Trigger
								if (nzdusdShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									nzdusdJustSold = true;
									nzdusdReEnter = true;
									sOfferID = "8";
									iAmount = Convert.ToInt32(nzdusdPosLot);
									sBuySell = "B";
									sTradeID = nzdusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { nzdusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + nzdusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (nzdusdShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									nzdusdJustSold = true;
									nzdusdReEnter = true;
									sOfferID = "8";
									iAmount = Convert.ToInt32(nzdusdPosLot);
									sBuySell = "B";
									sTradeID = nzdusdOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { nzdusdPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { nzdusdPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { nzdusdEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + nzdusdPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								nzdusdSideString = "";
							}
						}
					}

					// If triggered pair is "USD/CAD"
					if (offerTableRow.Instrument == "USD/CAD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { usdcadBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { usdcadAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { usdcadSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (usdcadPastBox.Text == "--" && usdcadAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { usdcadPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { usdcadPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (usdcadPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - usdcadPast);
							double moveConvert = moveCalc * 10000;
							usdcadMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (usdcadMove > highestMove)
							{
								highestMove = usdcadMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (usdcadMove > 0)
								{
									usdcadMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (usdcadMove >= buyLong & startSpread < maxSpread && usdcadEntry.Text == "--" && usdcadReEnter == false && usdcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "7";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										usdcadJustSold = false;
										usdcadReEnter = true;
										usdcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (usdcadMove < 0)
								{
									usdcadMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (usdcadMove <= sellShort & startSpread < maxSpread && usdcadEntry.Text == "--" && usdcadReEnter == false && usdcadInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "7";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										usdcadJustSold = false;
										usdcadReEnter = true;
										usdcadSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (usdcadSideString == "Long" && usdcadJustSold == false)
							{
								// Subtract entry price from bid price
								usdcadBuyLongCheck = (bidd - usdcadEntryPrice);
								usdcadPosMoveDec = (bidd - usdcadEntryPrice) * usdcadPosLot;


								// LONG: Sell Trigger
								if (usdcadBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdcadJustSold = true;
									usdcadReEnter = true;
									sOfferID = "7";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = usdcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdcadPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + usdcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (usdcadBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdcadJustSold = true;
									usdcadReEnter = true;
									sOfferID = "7";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = usdcadOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdcadPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (usdcadSideString == "Short" && usdcadJustSold == false)
							{
								// Subtract entry price from bid price
								usdcadShortSaleCheck = (usdcadEntryPrice - askd);
								usdcadPosMoveShortDec = (usdcadEntryPrice - askd) * usdcadPosLot;

								// SHORT: Buy Trigger
								if (usdcadShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdcadJustSold = true;
									usdcadReEnter = true;
									sOfferID = "7";
									iAmount = Convert.ToInt32(usdcadPosLot);
									sBuySell = "B";
									sTradeID = usdcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdcadPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + usdcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (usdcadShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdcadJustSold = true;
									usdcadReEnter = true;
									sOfferID = "7";
									iAmount = Convert.ToInt32(usdcadPosLot);
									sBuySell = "B";
									sTradeID = usdcadOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdcadPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdcadPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdcadEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdcadPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								usdcadSideString = "";
							}
						}
					}

					// If triggered pair is "USD/CHF"
					if (offerTableRow.Instrument == "USD/CHF")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { usdchfBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { usdchfAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { usdchfSpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (usdchfPastBox.Text == "--" && usdchfAsk.Text != "--")
						{
							try
							{
								this.Invoke(new MethodInvoker(delegate { usdchfPastBox.Text = asks; }));
								this.Invoke(new MethodInvoker(delegate { usdchfPast = ask; }));
							}
							catch (Exception fillErr)
							{
								Console.WriteLine(fillErr);
								actiBox.AppendText(fillErr + Environment.NewLine);
							}
						}

						// If "Past" TextBox is not null
						if (usdchfPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - usdchfPast);
							double moveConvert = moveCalc * 10000;
							usdchfMove = (decimal)(moveCalc);
							string moveString = Convert.ToString(moveConvert);


							if (usdchfMove > highestMove)
							{
								highestMove = usdchfMove;
								autoParam();
							}

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

							// ENTER POSITION CHECKS
							try
							{
								// LONG: Entry check
								if (usdchfMove > 0)
								{
									usdchfMoveBox.BackColor = System.Drawing.Color.Blue;
									this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.Text = move; }));

									// Check if a Buy Long is triggered
									if (usdchfMove >= buyLong & startSpread < maxSpread && usdchfEntry.Text == "--" && usdchfReEnter == false && usdchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "4";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										usdchfJustSold = false;
										usdchfReEnter = true;
										usdchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Entry check
								else if (usdchfMove < 0)
								{
									usdchfMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.Text = "(" + move + ")"; }));

									// Check if a Sell Short is triggered
									if (usdchfMove <= sellShort & startSpread < maxSpread && usdchfEntry.Text == "--" && usdchfReEnter == false && usdchfInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "4";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										usdchfJustSold = false;
										usdchfReEnter = true;
										usdchfSideString = "";
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfMoveBox.BackColor = Color.FromArgb(40, 40, 40); }));
								}

							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: POSITION MANAGER
							if (usdchfSideString == "Long" && usdchfJustSold == false)
							{
								// Subtract entry price from bid price
								usdchfBuyLongCheck = (bidd - usdchfEntryPrice);
								usdchfPosMoveDec = (bidd - usdchfEntryPrice) * usdchfPosLot;


								// LONG: Sell Trigger
								if (usdchfBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdchfJustSold = true;
									usdchfReEnter = true;
									sOfferID = "4";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = usdchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdchfPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + usdchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// LONG: STOP Loss
								if (usdchfBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdchfJustSold = true;
									usdchfReEnter = true;
									sOfferID = "4";
									iAmount = Convert.ToInt32(lotSize);
									sBuySell = "S";
									sTradeID = usdchfOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdchfPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: POSITION MANGER
							else if (usdchfSideString == "Short" && usdchfJustSold == false)
							{
								// Subtract entry price from bid price
								usdchfShortSaleCheck = (usdchfEntryPrice - askd);
								usdchfPosMoveShortDec = (usdchfEntryPrice - askd) * usdchfPosLot;

								// SHORT: Buy Trigger
								if (usdchfShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdchfJustSold = true;
									usdchfReEnter = true;
									sOfferID = "4";
									iAmount = Convert.ToInt32(usdchfPosLot);
									sBuySell = "B";
									sTradeID = usdchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdchfPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + "CLOSED:" + offerTableRow.Instrument + ": " + usdchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (usdchfShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdchfJustSold = true;
									usdchfReEnter = true;
									sOfferID = "4";
									iAmount = Convert.ToInt32(usdchfPosLot);
									sBuySell = "B";
									sTradeID = usdchfOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdchfPL.BackColor = System.Drawing.Color.FromArgb(40, 40, 40); }));
									this.Invoke(new MethodInvoker(delegate { usdchfPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfSide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdchfEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdchfPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								usdchfSideString = "";
							}
						}
					}

					// If triggered pair is "USD/JPY"
					if (offerTableRow.Instrument == "USD/JPY")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { usdjpyBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { usdjpyAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { usdjpySpread.Text = spread; }));

						// Fill in past price if ask price has a value
						if (usdjpyPastBox.Text == "--" && usdjpyAsk.Text != "--")
						{
							this.Invoke(new MethodInvoker(delegate { usdjpyPastBox.Text = asks; }));
							this.Invoke(new MethodInvoker(delegate { usdjpyPast = Convert.ToDouble(asks); }));
						}

						// If "Past" TextBox is not null
						if (usdjpyPastBox.Text != "--")
						{
							// Move calculations
							double moveCalc = (ask - usdjpyPast);
							double moveConvert = moveCalc * 10000;
							usdjpyMove = (decimal)(moveCalc);
							if (usdjpyMove > highestMove)
							{
								highestMove = usdjpyMove;
								autoParam();
							}

							try
							{
								// **ENTER POSITION CHECKS**
								// LONG: Position entry check
								if (usdjpyMove > 0)
								{
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
									// Change the text in move TextBox for positive's
									usdjpyMoveBox.BackColor = Color.Blue;
									this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.Text = move; }));
									// Check if a Buy Long is triggered
									if (usdjpyMove >= buyLong & startSpread < maxSpread && usdjpyEntry.Text == "--" && usdjpyReEnter == false && usdjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "2";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "B";
										usdjpyJustSold = false;
										usdjpyReEnter = true;
										usdjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " LONG: " + offerTableRow.Instrument + " " + asks + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}

								// SHORT: Position entry check
								else if (usdjpyMove < 0)
								{
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
									// Change the text in move TextBox for negative's									
									usdjpyMoveBox.BackColor = System.Drawing.Color.Red;
									this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.Text = "(" + move + ")"; }));
									// Check if a Sell Short is triggered
									if (usdjpyMove <= sellShort & startSpread < maxSpread && usdjpyEntry.Text == "--" && usdjpyReEnter == false && usdjpyInitial < availLev)
									{
										openSound.PlaySync();
										sOfferID = "2";
										iAmount = Convert.ToInt32(lotSize);
										sBuySell = "S";
										usdjpyJustSold = false;
										usdjpyReEnter = true;
										usdjpySideString = "";
										this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " SHORT: " + offerTableRow.Instrument + " " + bids + Environment.NewLine); }));
										CreateTrueOpenMarketOrder(sOfferID, sAccountID, iAmount, sBuySell);
									}
								}
								else
								{
									this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.Text = "0"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpyMoveBox.BackColor = Color.FromArgb(60, 60, 60); }));
								}
							}
							catch (Exception posErr)
							{
								Console.WriteLine(posErr);
								actiBox.AppendText(posErr + Environment.NewLine);
							}

							// LONG: Position Manager
							if (usdjpySideString == "Long" && usdjpyJustSold == false)
							{
								// Subtract entry price from bid price
								usdjpyBuyLongCheck = (bidd - usdjpyEntryPrice);
								usdjpyPosMoveDec = (bidd - usdjpyEntryPrice) * usdjpyPosLot;

								// LONG: Sell Trigger
								if (usdjpyBuyLongCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdjpyJustSold = true;
									usdjpyReEnter = true;
									sOfferID = "2";
									iAmount = Convert.ToInt32(usdjpyPosLot);
									sBuySell = "S";
									sTradeID = usdjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + usdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);

								}
								// LONG: STOP Loss
								if (usdjpyBuyLongCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdjpyJustSold = true;
									usdjpyReEnter = true;
									sOfferID = "2";
									iAmount = Convert.ToInt32(usdjpyPosLot);
									sBuySell = "S";
									sTradeID = usdjpyOrderID;
									dRate = bid;
									this.Invoke(new MethodInvoker(delegate { usdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}

							// SHORT: Position Manager
							else if (usdjpySideString == "Short" && usdjpyJustSold == false)
							{
								// Subtract entry price from bid price
								usdjpyShortSaleCheck = (usdjpyEntryPrice - askd);
								usdjpyPosMoveShortDec = (usdjpyEntryPrice - askd) * usdjpyPosLot;

								// SHORT: Buy Trigger
								if (usdjpyShortSaleCheck >= goalLong)
								{
									closedSound.PlaySync();
									usdjpyJustSold = true;
									usdjpyReEnter = true;
									sOfferID = "2";
									iAmount = Convert.ToInt32(usdjpyPosLot);
									sBuySell = "B";
									sTradeID = usdjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = "GOAL"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " CLOSED: " + offerTableRow.Instrument + ": " + usdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
								// SHORT: STOP Loss
								if (usdjpyShortSaleCheck <= (stopLoss * -1))
								{
									closedSound.PlaySync();
									usdjpyJustSold = true;
									usdjpyReEnter = true;
									sOfferID = "2";
									iAmount = Convert.ToInt32(usdjpyPosLot);
									sBuySell = "B";
									sTradeID = usdjpyOrderID;
									dRate = ask;
									this.Invoke(new MethodInvoker(delegate { usdjpyPL.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); }));
									this.Invoke(new MethodInvoker(delegate { usdjpyPosSize.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpySide.Text = "STOP"; }));
									this.Invoke(new MethodInvoker(delegate { usdjpyEntry.Text = "--"; }));
									this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + " STOP LOSS: " + offerTableRow.Instrument + ": " + usdjpyPL.Text + Environment.NewLine); }));
									CreateTrueCloseMarketOrder(sOfferID, sAccountID, sTradeID, iAmount, sBuySell);
								}
							}
							else
							{
								usdjpySideString = "";
							}
						}
					}
				Thread.Sleep(50);
				}
				catch (Exception offerErr)
				{
					Console.WriteLine(offerErr);
					actiBox.AppendText(offerErr + Environment.NewLine);
				}
			}
		}	
	}
}
