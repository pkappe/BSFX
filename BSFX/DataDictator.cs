using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		private void offersTable_RowChanged(object sender, RowEventArgs e)
		{

			#region

			// FxCore2 OfferTable Row Data Event
			O2GOfferTableRow offerTableRow = (O2GOfferTableRow)e.RowData;
			pair = offerTableRow.Instrument;
			// Identify pair triggering event
			this.Invoke(new MethodInvoker(delegate { pairBox.Text = offerTableRow.Instrument; }));
			// Bid Box
			this.Invoke(new MethodInvoker(delegate { bidBox.Text = bids; }));
			// Ask Box
			this.Invoke(new MethodInvoker(delegate { askBox.Text = asks; }));
			// Net P/L
			this.Invoke(new MethodInvoker(delegate { netPL.Text = Convert.ToString("$" + netPandL); }));
			
			// **SPREAD**
			try
			{
				// Calculate the spread
				double orginSpread = (ask - bid);
				startSpread = Convert.ToDecimal(orginSpread);
				// Convert spread to string.
				string convertSpread = (string)Convert.ChangeType(startSpread, typeof(string));
				// Trim periods and spaces.
				char[] trim = { ' ', '.' };
				string trimSpread = convertSpread.Trim(trim);
				// Trim for 2 character number
				if (orginSpread < 0.001)
				{
					string firstTrim = trimSpread.Substring(5, 2);
					string left = firstTrim.Substring(0, 1);
					string right = firstTrim.Substring(1);
					string fintrim = left + "." + right;
					spread = fintrim.ToString();
				}
				else if (orginSpread >= 0.001)
				{
					string firstTrim = trimSpread.Substring(4, 3);
					string left = firstTrim.Substring(0, 2);
					string right = firstTrim.Substring(2);
					string fintrim = left + "." + right;
					spread = fintrim.ToString();
				}

			}
			catch (Exception)
			{
				Console.WriteLine("ERROR IN SPREAD CALCULATION");
			}

			// **VOLATILITY**
			try
			{
				// Add all Volatility decimals together, then convert to Double. 
				// Store Volatility Double as private in this try method. 
				double volatility = Convert.ToDouble(audcadMoveInt + audjpyMoveInt + audusdMoveInt + cadjpyMoveInt + chfjpyMoveInt + euraudMoveInt + eurcadMoveInt + eurchfMoveInt + eurjpyMoveInt + eurusdMoveInt + gbpaudMoveInt + gbpchfMoveInt + gbpjpyMoveInt + gbpnzdMoveInt + gbpusdMoveInt + nzdjpyMoveInt + nzdusdMoveInt + usdcadMoveInt + usdchfMoveInt + usdjpyMoveInt);

				// Add 0.0000001 after volatility is calculated. 
				// This helps with errors caused by substring lengths. 
				double addToVol = (volatility + 0.0000001);

				// Finally, convert the Volatility double to a string to be used in uniText.
				string finVol = Convert.ToString(addToVol);

				// Check to make sure the volatility string starts with "0.00"
				// This helps with error control caused by incorrect decimals and calculation
				// This particular if-statement checks to see if volatility is greater than 0.
				if (finVol.StartsWith("0.00") && volatility > 0)
				{
					string finalVol = finVol.Substring(4, 2);
					volBox.ForeColor = System.Drawing.Color.Green;
					this.Invoke(new MethodInvoker(delegate { volBox.Text = finalVol; }));
				}
				// Checks if volatility is less than 0. 
				// Put a "-" in front of the string
				else if (finVol.StartsWith("-0.00") && volatility < 0)
				{
					string finalVol = finVol.Substring(5, 2);
					volBox.ForeColor = System.Drawing.Color.Red;
					this.Invoke(new MethodInvoker(delegate { volBox.Text = "(" + finalVol + ")"; }));
				}
				else
				{
					volBox.ForeColor = System.Drawing.Color.White;
					this.Invoke(new MethodInvoker(delegate { volBox.Text = "--"; }));
				}
			}
			// Catch any errors that may occur. 
			catch (Exception volErr)
			{
				this.Invoke(new MethodInvoker(delegate { volBox.Text = "--"; }));
				Console.WriteLine(volErr);
			}
			// **SET TABLE DATA**
			if (offerTableRow != null)
			{
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
				catch (Exception)
				{

					throw;
				}
			#endregion

				try
				{
					// If triggered pair is "AUD/CAD"
					if (offerTableRow.Instrument == "AUD/CAD")
					{
						// Fill in Bid, Ask and Spreads
						this.Invoke(new MethodInvoker(delegate { audcadBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { audcadPosBid.Text = bids; }));
						this.Invoke(new MethodInvoker(delegate { audcadAsk.Text = asks; }));
						this.Invoke(new MethodInvoker(delegate { audcadSpread.Text = spread; }));

						// If "Past" TextBox is not null
						if (audcadThirty.Text != "--")
						{
							// Convert Ask to decimal and subtract 1 for simpler use
							addedAsk = Convert.ToDecimal(ask - 1);
							// Convert past move to decimal and subtract 1
							adddedPast = Convert.ToDecimal(audcadThirtyInt - 1);
							// Subtract current ask price and past price
							moveDouble = Decimal.Subtract(addedAsk, adddedPast);
							// Convert move to usable string for text box
							convertMove = Convert.ToString(moveDouble);
							// Set the Move string
							move = convertMove;
							// Create a enter position checking decimal
							decimal buyCheck = Convert.ToDecimal(moveDouble);
							string audcadPosMoveString = Convert.ToString(audcadPosMoveDec);
						}
					}

					//// If triggered pair is "AUD/JPY"
					//if (offerTableRow.Instrument == "AUD/JPY")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { audjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { audjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { audjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { audjpySpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (audjpyThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(audjpyThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			audjpyMove.BackColor = System.Drawing.Color.Blue;
					//			audjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { audjpyMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && audjpyEntry.Text == "--")
					//		{
					//			audjpySideString = "Long";
					//			audjpyEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			audjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			audjpyMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { audjpyMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && audjpyEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			audjpyEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = bids; }));
					//			audjpySideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (audjpySideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			audjpyPosMoveDec = (bidd - audjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(audjpyPosMoveDec);
					//			string profit = moveString.Substring(4, 4);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (audjpyPosMoveDec >= 0)
					//			{
					//				audjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			else if (audjpyPosMoveDec < 0)
					//			{
					//				audjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			// LONG: Sell Trigger
					//			if (audjpyPosMoveDec > goalLong)
					//			{
					//				netPandL = netPandL + Convert.ToDecimal(startPL);
					//				audjpyEntryPrice = 0;
					//				audjpySideString = "";
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + audjpyPL.Text + Environment.NewLine); }));
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
					//			}
					//		}
					//		// Short Position Manager
					//		else if (audjpySideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			audjpyPosMoveShortDec = (askd - audjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(audjpyPosMoveShortDec);
					//			string profit = moveString.Substring(4, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (audjpyPosMoveShortDec < 0)
					//			{
					//				audjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			else if (audjpyPosMoveShortDec >= 0)
					//			{
					//				audjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			if (audjpyPosMoveShortDec <= goalShort)
					//			{

					//				netPandL = netPandL + Convert.ToDecimal(startPL);
					//				audjpyEntryPrice = 0;
					//				audjpySideString = "";
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + audjpyPL.Text + Environment.NewLine); }));
					//				this.Invoke(new MethodInvoker(delegate { audjpyPL.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audjpySide.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audjpyEntry.Text = "--"; }));
					//			}
					//		}
					//	}
					//}

					//// If triggered pair is "AUD/USD"
					//if (offerTableRow.Instrument == "AUD/USD")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { audusdBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { audusdPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { audusdAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { audusdSpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (audusdThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(audusdThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);


					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			audusdMove.BackColor = System.Drawing.Color.Blue;
					//			audusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { audusdMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && audusdEntry.Text == "--")
					//		{
					//			audusdSideString = "Long";
					//			this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			audusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			audusdMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { audusdMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && audusdEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			audusdEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = bids; }));
					//			audusdSideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (audusdSideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			audusdPosMoveDec = (bidd - audusdEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(audusdPosMoveDec);
					//			Console.WriteLine("MoveString on audusd: " + moveString);
					//			string profit = moveString.Substring(4, 4);
					//			string profitOne = profit.Substring(0, 2);
					//			string profitTwo = profit.Substring(2, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (audusdPosMoveDec >= 0)
					//			{
					//				audusdPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			else if (audusdPosMoveDec < 0)
					//			{
					//				audusdPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			// LONG: Sell Trigger
					//			if (audusdPosMoveDec > goalLong)
					//			{
					//				netPandL = netPandL + Convert.ToDecimal(startPL);
					//				audusdEntryPrice = 0;
					//				audusdSideString = "";
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + audusdPL.Text + Environment.NewLine); }));
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
					//			}
					//		}
					//		// Short Position Manager
					//		else if (audusdSideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			audcadPosMoveShortDec = (askd - audusdEntryPrice);

					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(audcadPosMoveShortDec);
					//			Console.WriteLine("MoveStringShort on audusd: " + moveString);
					//			string profit = moveString.Substring(4, 4);
					//			string profitOne = profit.Substring(0, 2);
					//			string profitTwo = profit.Substring(2, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (audcadPosMoveShortDec < 0)
					//			{
					//				audusdPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			else if (audcadPosMoveShortDec >= 0)
					//			{
					//				audusdPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			if (audcadPosMoveShortDec <= goalShort)
					//			{

					//				netPandL = netPandL + Convert.ToDecimal(startPL);
					//				audusdEntryPrice = 0;
					//				audusdSideString = "";
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + audusdPL.Text + Environment.NewLine); }));
					//				this.Invoke(new MethodInvoker(delegate { audusdPL.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audusdSide.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { audusdEntry.Text = "--"; }));
					//			}
					//		}
					//	}
					//}

					//// If triggered pair is "CAD/JPY"
					//if (offerTableRow.Instrument == "CAD/JPY")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { cadjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { cadjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { cadjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { cadjpySpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (cadjpyThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(cadjpyThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);



					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			cadjpyMove.BackColor = System.Drawing.Color.Blue;
					//			cadjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { cadjpyMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && cadjpyEntry.Text == "--")
					//		{
					//			cadjpySideString = "Long";
					//			cadjpyEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			cadjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			cadjpyMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { cadjpyMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && cadjpyEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			cadjpyEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = bids; }));
					//			cadjpySideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (cadjpySideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			cadjpyPosMoveDec = Decimal.Subtract(bidd, cadjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(cadjpyPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (cadjpyPosMoveDec >= 0)
					//			{
					//				cadjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			else if (cadjpyPosMoveDec < 0)
					//			{
					//				cadjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
					//			// LONG: Sell Trigger
					//			if (cadjpyPosMoveDec > goalLong)
					//			{
					//				netPandL = netPandL + Convert.ToDecimal(startPL);
					//				cadjpyEntryPrice = 0;
					//				cadjpySideString = "";
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + cadjpyPL.Text + Environment.NewLine); }));
					//				this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "--"; }));
					//				this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
					//			}
					//		}
					//		// Short Position Manager
					//		else if (cadjpySideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			cadjpyPosMoveShortDec = Decimal.Subtract(askd, cadjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(cadjpyPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (cadjpyPosMoveShortDec < 0)
					//			{
					//				cadjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (cadjpyPosMoveShortDec <= goalShort)
					//				{

					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					cadjpyEntryPrice = 0;
					//					cadjpySideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + cadjpyPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { cadjpySide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { cadjpyEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (cadjpyPosMoveShortDec >= 0)
					//			{
					//				cadjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { cadjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//	}
					//}

					//// If triggered pair is "CHF/JPY"
					//if (offerTableRow.Instrument == "CHF/JPY")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { chfjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { chfjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { chfjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { chfjpySpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (chfjpyThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(chfjpyThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			chfjpyMove.BackColor = System.Drawing.Color.Blue;
					//			chfjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { chfjpyMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && chfjpyEntry.Text == "--")
					//		{
					//			chfjpySideString = "Long";
					//			chfjpyEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			chfjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			chfjpyMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { chfjpyMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && chfjpyEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			chfjpyEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = bids; }));
					//			chfjpySideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (chfjpySideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			chfjpyPosMoveDec = Decimal.Subtract(bidd, chfjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(chfjpyPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (chfjpyPosMoveDec >= 0)
					//			{
					//				chfjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				// LONG: Sell Trigger
					//				if (chfjpyPosMoveDec > goalLong)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					chfjpyEntryPrice = 0;
					//					chfjpySideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + chfjpyPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (chfjpyPosMoveDec < 0)
					//			{
					//				chfjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//		// Short Position Manager
					//		else if (chfjpySideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			chfjpyPosMoveShortDec = Decimal.Subtract(askd, chfjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(chfjpyPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (chfjpyPosMoveShortDec < 0)
					//			{
					//				chfjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (chfjpyPosMoveShortDec <= goalShort)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					chfjpyEntryPrice = 0;
					//					chfjpySideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + chfjpyPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpySide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { chfjpyEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (chfjpyPosMoveShortDec >= 0)
					//			{
					//				chfjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { chfjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//	}
					//}

					//// If triggered pair is "EUR/AUD"
					//if (offerTableRow.Instrument == "EUR/AUD")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { euraudBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { euraudPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { euraudAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { euraudSpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (euraudThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(euraudThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);


					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			euraudMove.BackColor = System.Drawing.Color.Blue;
					//			euraudMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { euraudMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && euraudEntry.Text == "--")
					//		{
					//			euraudSideString = "Long";
					//			euraudEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			euraudMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			euraudMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { euraudMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && euraudEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			euraudEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = bids; }));
					//			euraudSideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (euraudSideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			euraudPosMoveDec = Decimal.Subtract(bidd, euraudEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(euraudPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (euraudPosMoveDec >= 0)
					//			{
					//				euraudPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				// LONG: Sell Trigger
					//				if (euraudPosMoveDec > goalLong)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					euraudEntryPrice = 0;
					//					euraudSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + euraudPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (euraudPosMoveDec < 0)
					//			{
					//				euraudPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//		// Short Position Manager
					//		else if (euraudSideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			euraudPosMoveShortDec = Decimal.Subtract(askd, euraudEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(euraudPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (euraudPosMoveShortDec < 0)
					//			{
					//				euraudPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (euraudPosMoveShortDec <= goalShort)
					//				{

					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					euraudEntryPrice = 0;
					//					euraudSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + euraudPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { euraudSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { euraudEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (euraudPosMoveShortDec >= 0)
					//			{
					//				euraudPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { euraudPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}								
					//		}
					//	}
					//}

					//// If triggered pair is "EUR/CAD"
					//if (offerTableRow.Instrument == "EUR/CAD")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { eurcadBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurcadPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurcadAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { eurcadSpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (eurcadThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(eurcadThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);


					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			eurcadMove.BackColor = System.Drawing.Color.Blue;
					//			eurcadMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { eurcadMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && eurcadEntry.Text == "--")
					//		{
					//			eurcadSideString = "Long";
					//			eurcadEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			eurcadMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			eurcadMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { eurcadMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && eurcadEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			eurcadEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = bids; }));
					//			eurcadSideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (eurcadSideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			eurcadPosMoveDec = Decimal.Subtract(bidd, eurcadEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurcadPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurcadPosMoveDec >= 0)
					//			{
					//				eurcadPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				// LONG: Sell Trigger
					//				if (eurcadPosMoveDec >= goalLong)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurcadEntryPrice = 0;
					//					eurcadSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurcadPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurcadPosMoveDec < 0)
					//			{
					//				eurcadPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//		// Short Position Manager
					//		else if (eurcadSideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			eurcadPosMoveShortDec = Decimal.Subtract(askd, eurcadEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurcadPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurcadPosMoveShortDec < 0)
					//			{
					//				eurcadPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (eurcadPosMoveShortDec <= goalShort)
					//				{

					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurcadEntryPrice = 0;
					//					eurcadSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurcadPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurcadEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurcadPosMoveShortDec >= 0)
					//			{
					//				eurcadPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurcadPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}								
					//		}
					//	}
					//}

					//// If triggered pair is "EUR/CHF"
					//if (offerTableRow.Instrument == "EUR/CHF")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { eurchfBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurchfPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurchfAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { eurchfSpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (eurchfThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(eurchfThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);


					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			eurchfMove.BackColor = System.Drawing.Color.Blue;
					//			eurchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { eurchfMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && eurchfEntry.Text == "--")
					//		{
					//			eurchfSideString = "Long";
					//			eurchfEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			eurchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			eurchfMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { eurchfMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && eurchfEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			eurchfEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = bids; }));
					//			eurchfSideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (eurchfSideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			eurchfPosMoveDec = Decimal.Subtract(bidd, eurchfEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurchfPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurchfPosMoveDec >= 0)
					//			{
					//				eurchfPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				// LONG: Sell Trigger
					//				if (eurchfPosMoveDec > goalLong)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurchfEntryPrice = 0;
					//					eurchfSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurchfPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurchfPosMoveDec < 0)
					//			{
					//				eurchfPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//		// Short Position Manager
					//		else if (eurchfSideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			eurchfPosMoveShortDec = Decimal.Subtract(askd, eurchfEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurchfPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurchfPosMoveShortDec <= 0)
					//			{
					//				eurchfPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (eurchfPosMoveShortDec <= goalShort)
					//				{

					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurchfEntryPrice = 0;
					//					eurchfSideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurchfPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfSide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurchfEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurchfPosMoveShortDec > 0)
					//			{
					//				eurchfPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurchfPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//	}
					//}

					//// If triggered pair is "EUR/JPY"
					//if (offerTableRow.Instrument == "EUR/JPY")
					//{
					//	// Fill in Bid, Ask and Spreads
					//	this.Invoke(new MethodInvoker(delegate { eurjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { eurjpySpread.Text = spread; }));

					//	// If "Past" TextBox is not null
					//	if (eurjpyThirty.Text != "--")
					//	{
					//		// Convert Ask to decimal and subtract 1 for simpler use
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		// Convert past move to decimal and subtract 1
					//		decimal adddedPast = Convert.ToDecimal(eurjpyThirtyInt - 1);
					//		// Subtract current ask price and past price
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		// Convert move to usable string for text box
					//		string convertMove = Convert.ToString(moveDouble);
					//		// Set the Move string
					//		string move = convertMove;
					//		// Create a enter position checking decimal
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);



					//		// **ENTER POSITION CHECKS**
					//		// LONG: Position entry check
					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			// Change the text in move TextBox for positive's
					//			eurjpyMove.BackColor = System.Drawing.Color.Blue;
					//			eurjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { eurjpyMove.Text = moveTwo; }));
					//		}
					//		// Check if a Buy Long is triggered
					//		if (buyCheck >= buyLong & startSpread < maxSpread && eurjpyEntry.Text == "--")
					//		{
					//			eurjpySideString = "Long";
					//			eurjpyEntryPrice = Convert.ToDecimal(ask);
					//			this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = asks; }));
					//			this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "Long"; }));
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//		}
					//		// SHORT: Position entry check
					//		if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			// Change the text in move TextBox for negative's
					//			eurjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			eurjpyMove.BackColor = System.Drawing.Color.Red;
					//			this.Invoke(new MethodInvoker(delegate { eurjpyMove.Text = "(" + moveTwo + ")"; }));
					//		}
					//		// Check if a Sell Short is triggered
					//		if (buyCheck <= sellShort & startSpread < maxSpread && eurjpyEntry.Text == "--")
					//		{
					//			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			eurjpyEntryPrice = Convert.ToDecimal(bid);
					//			this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = bids; }));
					//			eurjpySideString = "Short";
					//			this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "Short"; }));
					//		}

					//		// **POSITION MONITORING**
					//		// Long Position Manager
					//		if (eurjpySideString == "Long")
					//		{
					//			// Subtract entry price from bid price
					//			eurjpyPosMoveDec = Decimal.Subtract(bidd, eurjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurjpyPosMoveDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurjpyPosMoveDec >= 0)
					//			{
					//				eurjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				// LONG: Sell Trigger
					//				if (eurjpyPosMoveDec > goalLong)
					//				{
					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurjpyEntryPrice = 0;
					//					eurjpySideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurjpyPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurjpyPosMoveDec < 0)
					//			{
					//				eurjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//		// Short Position Manager
					//		else if (eurjpySideString == "Short")
					//		{
					//			// Subtract entry price from bid price
					//			eurjpyPosMoveShortDec = Decimal.Subtract(askd, eurjpyEntryPrice);
					//			// P/L or Position move string conversion
					//			string moveString = Convert.ToString(eurjpyPosMoveShortDec);
					//			string profit = moveString.Substring(5, 3);
					//			string profitOne = profit.Substring(0, 1);
					//			string profitTwo = profit.Substring(1, 2);
					//			string startPL = profitOne + "." + profitTwo;

					//			// Changes to P/L
					//			if (eurjpyPosMoveShortDec < 0)
					//			{
					//				eurjpyPL.BackColor = System.Drawing.Color.Blue;
					//				this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + profitOne + "." + profitTwo; }));

					//				if (eurjpyPosMoveShortDec <= goalShort)
					//				{

					//					netPandL = netPandL + Convert.ToDecimal(startPL);
					//					eurjpyEntryPrice = 0;
					//					eurjpySideString = "";
					//					this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": CLOSED:" + offerTableRow.Instrument + " @ " + offerTableRow.Bid + " For:" + eurjpyPL.Text + Environment.NewLine); }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpySide.Text = "--"; }));
					//					this.Invoke(new MethodInvoker(delegate { eurjpyEntry.Text = "--"; }));
					//				}
					//			}
					//			else if (eurjpyPosMoveShortDec >= 0)
					//			{
					//				eurjpyPL.BackColor = System.Drawing.Color.Red;
					//				this.Invoke(new MethodInvoker(delegate { eurjpyPL.Text = "$" + profitOne + "." + profitTwo; }));
					//			}
								
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "EUR/USD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { eurusdBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurusdPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { eurusdAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { eurusdSpread.Text = spread; }));

					//	if (eurusdThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(eurusdThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && eurusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			eurusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { eurusdMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && eurusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			eurusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { eurusdMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			eurusdMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { eurusdMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "GBP/AUD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { gbpaudBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpaudPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpaudAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpaudSpread.Text = spread; }));

					//	if (gbpaudThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(gbpaudThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && gbpaudEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpaudMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpaudMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && gbpaudEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpaudMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpaudMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			gbpaudMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { gbpaudMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "GBP/CHF")
					//{
					//	this.Invoke(new MethodInvoker(delegate { gbpchfBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpchfPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpchfAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpchfSpread.Text = spread; }));

					//	if (gbpchfThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(gbpchfThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && gbpchfEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpchfMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && gbpchfEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpchfMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			gbpchfMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { gbpchfMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "GBP/JPY")
					//{
					//	this.Invoke(new MethodInvoker(delegate { gbpjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpjpySpread.Text = spread; }));

					//	if (gbpjpyThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(gbpjpyThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && gbpjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpjpyMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && gbpjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpjpyMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			gbpjpyMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { gbpjpyMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "GBP/NZD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { gbpnzdBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpnzdPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpnzdAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpnzdSpread.Text = spread; }));

					//	if (gbpnzdThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(gbpnzdThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && gbpnzdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpnzdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpnzdMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && gbpnzdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpnzdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpnzdMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			gbpnzdMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { gbpnzdMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "GBP/USD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { gbpusdBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpusdPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpusdAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { gbpusdSpread.Text = spread; }));

					//	if (gbpusdThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(gbpusdThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && gbpusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpusdMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && gbpusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			gbpusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { gbpusdMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			gbpusdMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { gbpusdMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "NZD/JPY")
					//{
					//	this.Invoke(new MethodInvoker(delegate { nzdjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdjpySpread.Text = spread; }));

					//	if (nzdjpyThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(nzdjpyThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && nzdjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			nzdjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { nzdjpyMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && nzdjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			nzdjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { nzdjpyMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			nzdjpyMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { nzdjpyMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "NZD/USD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { nzdusdBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdusdPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdusdAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { nzdusdSpread.Text = spread; }));

					//	if (nzdusdThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(nzdusdThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && nzdusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			nzdusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { nzdusdMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && nzdusdEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			nzdusdMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { nzdusdMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			nzdusdMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { nzdusdMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "USD/CAD")
					//{
					//	this.Invoke(new MethodInvoker(delegate { usdcadBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdcadPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdcadAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { usdcadSpread.Text = spread; }));

					//	if (usdcadThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(usdcadThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && usdcadEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdcadMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdcadMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && usdcadEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdcadMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdcadMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			usdcadMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { usdcadMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "USD/CHF")
					//{
					//	this.Invoke(new MethodInvoker(delegate { usdchfBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdchfPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdchfAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { usdchfSpread.Text = spread; }));

					//	if (usdchfThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask + 1);
					//		decimal adddedPast = Convert.ToDecimal(usdchfThirtyInt + 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && usdchfEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdchfMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && usdchfEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdchfMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdchfMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			usdchfMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { usdchfMove.Text = "--"; }));
					//		}
					//	}
					//}
					//if (offerTableRow.Instrument == "USD/JPY")
					//{
					//	this.Invoke(new MethodInvoker(delegate { usdjpyBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdjpyPosBid.Text = bids; }));
					//	this.Invoke(new MethodInvoker(delegate { usdjpyAsk.Text = asks; }));
					//	this.Invoke(new MethodInvoker(delegate { usdjpySpread.Text = spread; }));

					//	if (usdjpyThirty.Text != "")
					//	{
					//		decimal addedAsk = Convert.ToDecimal(ask - 1);
					//		decimal adddedPast = Convert.ToDecimal(usdjpyThirtyInt - 1);
					//		decimal moveDouble = Decimal.Subtract(addedAsk, adddedPast);
					//		string convertMove = Convert.ToString(moveDouble);
					//		string move = convertMove;
					//		decimal buyCheck = Convert.ToDecimal(moveDouble);

					//		if (move.StartsWith("0.00") && moveDouble >= 0)
					//		{
					//			if (buyCheck >= buyLong && startSpread < maxSpread && usdjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": LONG: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(4, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdjpyMove.Text = moveTwo; }));
					//		}
					//		else if (move.StartsWith("-0.00") && moveDouble < 0)
					//		{
					//			if (buyCheck <= sellShort && startSpread < maxSpread && usdjpyEntry.Text == "")
					//			{
					//				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText(DateTime.Now + ": SHORT: " + offerTableRow.Instrument + " @ " + offerTableRow.Ask + Environment.NewLine); }));
					//			}
					//			usdjpyMoveInt = moveDouble;
					//			string moveTwo = move.Substring(5, 2);
					//			this.Invoke(new MethodInvoker(delegate { usdjpyMove.Text = "-" + moveTwo; }));
					//		}
					//		else
					//		{
					//			usdjpyMoveInt = 0;
					//			this.Invoke(new MethodInvoker(delegate { usdjpyMove.Text = "--"; }));
					//		}
					//	}
					//}
				}
				catch (Exception tabErr)
				{
					Console.WriteLine(tabErr);
				}

				Thread.Sleep(50);
			}
		}	

	}
}
