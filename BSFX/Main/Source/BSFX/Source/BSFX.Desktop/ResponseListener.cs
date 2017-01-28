using System;
using fxcore2;
using System.Threading;

namespace BSFX
{
	public class ResponseListener : IO2GResponseListener
	{
		private O2GSession mSession = null;

		public WaitHandle ResponseHandle
		{
			get { return mResponseHandle; }
		}
		private EventWaitHandle mResponseHandle;
		public ResponseListener(O2GSession session)
		{
			mSession = session;
			mResponseHandle = new AutoResetEvent(false);
		}

		public void onRequestCompleted(string requestId, O2GResponse response)
		{
			if (response.Type == O2GResponseType.MarketDataSnapshot)
			{
				BSFX bsfx = new BSFX();
				bsfx.PrintPrices(mSession, response, "");
			}
			mResponseHandle.Set();
		}

		public void onRequestFailed(string requestId, string error)
		{
			if (String.IsNullOrEmpty(error)) // not an error - we are finished - no more candles
			{
				Console.WriteLine("\n There is no history data for the specified period!");
			}
			else
			{
				Console.WriteLine("Request failed requestID={0} error={1}", requestId, error);
			}
			mResponseHandle.Set();
		}

		public void onTablesUpdates(O2GResponse data)
		{
			//STUB
		}
	}
}
