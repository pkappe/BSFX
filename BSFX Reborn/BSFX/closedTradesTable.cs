using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		void closedTradesTable_RowChanged(object sender, RowEventArgs e)
		{
			O2GClosedTradeTableRow closed = (O2GClosedTradeTableRow)e.RowData;
			// EUR/USD
			if (closed.OfferID == "1")
			{
				
			}

			// USD/JPY
			if (closed.OfferID == "2")
			{
				
			}

			// GBP/USD
			if (closed.OfferID == "3")
			{
				
			}

			// USD/CHF
			if (closed.OfferID == "4")
			{
				
			}

			// EUR/CHF
			if (closed.OfferID == "5")
			{
				
			}

			// AUD/USD
			if (closed.OfferID == "6")
			{
				
			}

			// USD/CAD
			if (closed.OfferID == "7")
			{
				
			}

			// NZD/USD
			if (closed.OfferID == "8")
			{
				
			}

			// EUR/JPY
			if (closed.OfferID == "10")
			{
				
			}

			// GBP/JPY
			if (closed.OfferID == "11")
			{
				
			}

			// CHF/JPY
			if (closed.OfferID == "12")
			{
				
			}

			// GBP/CHF
			if (closed.OfferID == "13")
			{
				
			}

			// EUR/AUD
			if (closed.OfferID == "14")
			{
				
			}

			// EUR/CAD
			if (closed.OfferID == "15")
			{
				
			}

			// AUD/CAD
			if (closed.OfferID == "16")
			{
				
			}

			// AUD/JPY
			if (closed.OfferID == "17")
			{
				
			}

			// CAD/JPY
			if (closed.OfferID == "18")
			{
				
			}

			// NZD/JPY
			if (closed.OfferID == "19")
			{
				
			}

			// GBP/NZD
			if (closed.OfferID == "21")
			{
				gbpnzdPosPL = closed.GrossPL;
				gbpnzdPL.Text = Convert.ToString(gbpnzdPosPL);
			}

			// GBP/AUD
			if (closed.OfferID == "22")
			{
				
			}
		}
	}
}
