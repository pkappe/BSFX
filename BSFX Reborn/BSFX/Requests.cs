using fxcore2;
using System;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		public void CreateTrueMarketOrder(string sOfferID, string sAccountID, int iAmount, string sBuySell, double dRateLimit)
		{

			O2GRequestFactory factory = mSession.getRequestFactory();
			if (factory == null)
				return;
			O2GValueMap valuemap = factory.createValueMap();
			valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.CreateOrder);
			valuemap.setString(O2GRequestParamsEnum.OrderType, Constants.Orders.TrueMarketOpen);
			valuemap.setString(O2GRequestParamsEnum.AccountID, sAccountID);            // The identifier of the account the order should be placed for.
			valuemap.setString(O2GRequestParamsEnum.OfferID, sOfferID);                // The identifier of the instrument the order should be placed for.
			valuemap.setString(O2GRequestParamsEnum.BuySell, sBuySell);                // The order direction: Constants.Sell for "Sell", Constants.Buy for "Buy".
			valuemap.setInt(O2GRequestParamsEnum.Amount, iAmount);                    // The quantity of the instrument to be bought or sold.
			valuemap.setString(O2GRequestParamsEnum.CustomID, "TrueMarketOrder");    // The custom identifier of the order.
			valuemap.setDouble(O2GRequestParamsEnum.RateLimit, dRateLimit);

			O2GRequest request = factory.createOrderRequest(valuemap);
			mSession.sendRequest(request);
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
		
		public string PrintPrices(O2GSession session, O2GResponse response, string newPast)
		{
			Console.WriteLine();
			O2GResponseReaderFactory factory = session.getResponseReaderFactory();
			if (factory != null)
			{
				O2GMarketDataSnapshotResponseReader reader = factory.createMarketDataSnapshotReader(response);
				for (int ii = 0; ii < reader.Count; ii++)
				{
					
				}				
			}
			return newPast;
		}
	}
}
