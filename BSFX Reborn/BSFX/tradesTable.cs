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
			
		}
	}
}
