using fxcore2;
using System;

namespace BSFX
{
	class TableListener : IO2GTableListener
	{
		
		public void onAdded(string rowID, O2GRow rowData)
		{
			O2GTableType type = rowData.TableType;
			switch (type)
			{
				case O2GTableType.Offers:
					O2GOfferTableRow offers = (O2GOfferTableRow)(rowData);
					break;
				case O2GTableType.Accounts:
					O2GAccountTableRow account = (O2GAccountTableRow)(rowData);
					break;
				case O2GTableType.Summary:
					O2GSummaryTableRow row = (O2GSummaryTableRow)(rowData);
					break;
				case O2GTableType.ClosedTrades:
					Console.WriteLine("CLOSED TRADES TRIGGERED FROM ADDED TABLELISTENER!");
					O2GClosedTradeTableRow closed = (O2GClosedTradeTableRow)(rowData);
					break;
			}		
		}

		public void onChanged(string rowID, O2GRow rowData)
		{
			O2GTableType type = rowData.TableType;
			switch (type)
			{
				case O2GTableType.Offers:
					O2GOfferTableRow offers = (O2GOfferTableRow)(rowData);
					break;
				case O2GTableType.Accounts:
					O2GAccountTableRow account = (O2GAccountTableRow)(rowData);
					break;
				case O2GTableType.Summary:
					O2GSummaryTableRow row = (O2GSummaryTableRow)(rowData);
					break;
				case O2GTableType.ClosedTrades:
					Console.WriteLine("CLOSED TRADES TRIGGERED FROM CHANGED TABLELISTENER!");
					O2GClosedTradeTableRow closed = (O2GClosedTradeTableRow)(rowData);

					break;
			}
		}

		public void onDeleted(string rowID, O2GRow rowData)
		{
			O2GTableType type = rowData.TableType;
			switch (type)
			{
				case O2GTableType.Offers:
					O2GOfferTableRow offers = (O2GOfferTableRow)(rowData);
					break;
				case O2GTableType.Accounts:
					O2GAccountTableRow account = (O2GAccountTableRow)(rowData);
					break;
				case O2GTableType.Summary:
					O2GSummaryTableRow row = (O2GSummaryTableRow)(rowData);
					break;
				case O2GTableType.ClosedTrades:
					Console.WriteLine("CLOSED TRADES TRIGGERED FROM DELETED TABLELISTENER!");
					O2GClosedTradeTableRow closed = (O2GClosedTradeTableRow)(rowData);
					break;
			}		
		}

		public void onStatusChanged(O2GTableStatus status)
		{
			
		}
	}
}
