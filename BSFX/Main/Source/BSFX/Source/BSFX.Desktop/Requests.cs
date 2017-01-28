using fxcore2;
using System;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// Place live market OPEN order
		public void CreateTrueOpenMarketOrder(string sOfferID, string sAccountID, int iAmount, string sBuySell)
		{
			try
			{
				O2GRequestFactory factory = Session.getRequestFactory();
				O2GValueMap valuemap = factory.createValueMap();
				valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.CreateOrder);
				valuemap.setString(O2GRequestParamsEnum.OrderType, Constants.Orders.TrueMarketOpen);
				// The identifier of the account the order should be placed for.
				valuemap.setString(O2GRequestParamsEnum.AccountID, sAccountID);
				// The identifier of the instrument the order should be placed for.
				valuemap.setString(O2GRequestParamsEnum.OfferID, sOfferID);
				// The order direction: Constants.Sell for "Sell", Constants.Buy for "Buy".
				valuemap.setString(O2GRequestParamsEnum.BuySell, sBuySell);
				// The quantity of the instrument to be bought or sold.
				valuemap.setInt(O2GRequestParamsEnum.Amount, iAmount);
				// The custom identifier of the order.
				valuemap.setString(O2GRequestParamsEnum.CustomID, "TrueMarketOrder");

				O2GRequest request = factory.createOrderRequest(valuemap);
				if (request != null)
				{
					mRequestID = request.RequestID;
					Session.sendRequest(request);
				}
				else
				{
					Console.WriteLine("Cannot create request; probably some arguments are missing or incorrect");
				}
			}
			catch (Exception openErr)
			{
				Console.WriteLine(openErr);
			}			
		}

		// Place live market CLOSE order
		public void CreateTrueCloseMarketOrder(string sOfferID, string sAccountID, string sTradeID, int iAmount, string sBuySell)
		{
			try
			{
				Console.WriteLine("CLOSING ORDER");
				O2GRequestFactory factory = mSession.getRequestFactory();
				if (factory == null)
					return;

				Console.WriteLine("Factory not null");
				Console.WriteLine("sOfferID: " + sOfferID);
				Console.WriteLine("sAccountID: " + sAccountID);
				Console.WriteLine("sTradeID: " + sTradeID);
				Console.WriteLine("iAmount: " + iAmount);
				Console.WriteLine("dRate: " + dRate);
				Console.WriteLine("sBuySell: " + sBuySell);

				O2GValueMap valuemap = factory.createValueMap();
				valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.CreateOrder);
				valuemap.setString(O2GRequestParamsEnum.OrderType, Constants.Orders.TrueMarketClose);
				valuemap.setString(O2GRequestParamsEnum.AccountID, sAccountID);
				valuemap.setString(O2GRequestParamsEnum.OfferID, sOfferID);                   
				valuemap.setString(O2GRequestParamsEnum.NetQuantity, "Y");
				valuemap.setString(O2GRequestParamsEnum.BuySell, sBuySell);
				valuemap.setString(O2GRequestParamsEnum.CustomID, "CloseTrueMarketOrder");

				O2GRequest request = factory.createOrderRequest(valuemap);
				mSession.sendRequest(request);
			}
			catch (Exception closeErr)
			{
				Console.WriteLine(closeErr);
			}
			
		}
		// Change subscription status
		public void CreateSetSubscriptionStatusRequest(string sOfferID, string sStatus)
		{
			O2GRequestFactory factory = Session.getRequestFactory();
			O2GValueMap valuemap = factory.createValueMap();
			valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.SetSubscriptionStatus);
			valuemap.setString(O2GRequestParamsEnum.OfferID, sOfferID);
			valuemap.setString(O2GRequestParamsEnum.SubscriptionStatus, sStatus);

			O2GRequest request = factory.createOrderRequest(valuemap);
			if (request != null)
			{
				try
				{
					mRequestID = request.RequestID;
					Session.sendRequest(request);
				}
				catch (Exception subErr)
				{
					Console.WriteLine(subErr);
				}				
			}
			else
			{
				Console.WriteLine("Cannot create request; probably some arguments are missing or incorrect");
			}
		}

		// Request 
		public void GetHistoryPrices(DateTime timeFrom, DateTime timeTo)
		{			
			O2GRequestFactory factory = mSession.getRequestFactory();
			O2GTimeframeCollection timeframes = factory.Timeframes;
			O2GTimeframe tfo = timeframes["D1"];
			O2GRequest request = factory.createMarketDataSnapshotRequestInstrument("GBP/NZD", tfo, 7);
			timeFrom = today;
			timeTo = DateTime.Today;

			factory.fillMarketDataSnapshotRequestTime(request, timeFrom, timeTo, false);
			mSession.sendRequest(request);
			Thread.Sleep(5000);
		}

		public string PrintPrices(O2GSession session, O2GResponse response, string newPast)
		{
			Console.WriteLine();
			O2GResponseReaderFactory factory = session.getResponseReaderFactory();
			if (factory != null)
			{
				O2GMarketDataSnapshotResponseReader reader = factory.createMarketDataSnapshotReader(response);
				for (int ii = 0; ii < reader.Count; ii++)
				{
					newPast = Convert.ToString(reader.getAskOpen(ii));
					double newDouble = reader.getAskOpen(ii);
					gbpnzdPast = newDouble;
					gbpnzdPastBox.Text = newPast;
					Console.WriteLine(newPast);
				}				
			}
			return newPast;
		}
	}
}
