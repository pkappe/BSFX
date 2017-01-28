using fxcore2;

namespace BSFX
{
	class MyResponseListener : IO2GResponseListener
	{
		public void onRequestCompleted(string requestId, O2GResponse response)
		{
			if (response.Type == O2GResponseType.GetOffers)
			{
				BSFX.printOffers(response);
			}		
		}

		public void onRequestFailed(string requestId, string error)
		{
			
		}

		public void onTablesUpdates(O2GResponse response)
		{
			if (response.Type == O2GResponseType.GetOffers || response.Type == O2GResponseType.TablesUpdates)
				BSFX.printOffers(response);
		}
	}
}
