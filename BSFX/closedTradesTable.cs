using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		void closedTable_RowChanged(object sender, RowEventArgs e)
		{
			Console.WriteLine("CLOSED TABLE TRIGGERED!");
			O2GClosedTradeTableRow closed = (O2GClosedTradeTableRow)e.RowData;
			switch (closed.OfferID)
			{
				// EUR/USD
				case "1":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// USD/JPY
				case "2":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// GBP/USD
				case "3":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// USD/CHF
				case "4":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// EUR/CHF
				case "5":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// AUD/USD
				case "6":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// USD/CAD
				case "7":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// NZD/USD
				case "8":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// EUR/JPY
				case "10":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// GBP/JPY
				case "11":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// CHF/JPY
				case "12":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// GBP/CHF
				case "13":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// EUR/AUD
				case "14":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// EUR/CAD
				case "15":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// AUD/CAD
				case "16":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// AUD/CAD
				case "17":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// CAD/JPY
				case "18":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// NZD/JPY
				case "19":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// GBP/NZD
				case "21":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				// GBP/AUD
				case "22":
					Console.WriteLine("Closed table triggered for: " + closed.OfferID);
					break;
				default:
					break;
			}		
		}
	}
}
