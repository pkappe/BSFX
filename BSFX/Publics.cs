using fxcore2;
using System;
using System.Windows.Forms;
using System.Media;

namespace BSFX
{
	public partial class BSFX : Form
	{
		#region PUBLIC STRINGS

		// System and application commands
		public SoundPlayer openSound;
		public SoundPlayer closedSound;

		// O2G from fxcore2 designators
		public O2GSession mSession;
		public O2GAccountRow account;
		public O2GTableManager tableManager;
		public O2GAccountsTable accountsTable;
		public O2GTableManagerStatus managerStatus { get; set; }

		// Account information
		public string sUserID { get; set; }
		public string sPassword { get; set; }
		public string sURL { get; set; }
		public string sConnection { get; set; }
		public string sAccountID { get; set; }
		public string sStatus { get; set; }
		public double dayPL { get; set; }
		public bool volRestrict { get; set; }
		public bool volResEnabled { get; set; }
		public Int32 amountLimit { get; set; }
		public Int32 baseSize { get; set; }
		public double acctEq { get; set; }
		public double acctAllocated { get; set; }
		public double unrealPL { get; set; }
		public double accountValue { get; set; }
		public double marginLev { get; set; }
		public double restrictBuy { get; set; }
		public bool hedgingLong { get; set; }
		public bool hedgingShort { get; set; }

		// Order Placement
		public string sOfferID { get; set; }
		public string mRequestID { get; set; }
		public int iAmount { get; set; }
		public string sBuySell { get; set; }
		public string sOrderType { get; set; }
		public double shortLot { get; set; }
		public string sTradeID { get; set; }
		public double dRate { get; set; }
		public double dRateLimit { get; set; }

		// Session Information
		public O2GSession Session
		{
			get { return mSession; }
		}		
		private static string sSessionID = "";
		public static string SessionID
		{
			get { return sSessionID; }
		}
		private static string sPin = "";
		public static string Pin
		{
			get { return sPin; }
		}

		// Forex Data information		
		public string pair { get; set; }
		public string spread { get; set; }
		public decimal bidTrue { get; set; }
		public double bid { get; set; }
		public double ask { get; set; }

		// Trading Parameters
		public decimal buyLong { get; set; }
		public decimal sellShort { get; set; }
		public decimal goalLong { get; set; }
		public decimal goalShort { get; set; }
		public decimal maxSpread { get; set; }
		public Int32 lotSize { get; set; }

		// User Interface
		public decimal volatility { get; set; }
		public decimal volValue { get; set; }
		public int intervalLeft { get; set; }

		// Pair MOVE decimals
		public decimal audcadMove { get; set; }
		public decimal audjpyMove { get; set; }
		public decimal audusdMove { get; set; }
		public decimal cadjpyMove { get; set; }
		public decimal chfjpyMove { get; set; }
		public decimal euraudMove { get; set; }
		public decimal eurcadMove { get; set; }
		public decimal eurchfMove { get; set; }
		public decimal eurjpyMove { get; set; }
		public decimal eurusdMove { get; set; }
		public decimal gbpaudMove { get; set; }
		public decimal gbpchfMove { get; set; }
		public decimal gbpjpyMove { get; set; }
		public decimal gbpnzdMove { get; set; }
		public decimal gbpusdMove { get; set; }
		public decimal nzdjpyMove { get; set; }
		public decimal nzdusdMove { get; set; }
		public decimal usdcadMove { get; set; }
		public decimal usdchfMove { get; set; }
		public decimal usdjpyMove { get; set; }

		// Pair PAST prices
		public double audcadPast { get; set; }
		public double audjpyPast { get; set; }
		public double audusdPast { get; set; }
		public double cadjpyPast { get; set; }
		public double chfjpyPast { get; set; }
		public double euraudPast { get; set; }
		public double eurcadPast { get; set; }
		public double eurchfPast { get; set; }
		public double eurjpyPast { get; set; }
		public double eurusdPast { get; set; }
		public double gbpaudPast { get; set; }
		public double gbpchfPast { get; set; }
		public double gbpjpyPast { get; set; }
		public double gbpnzdPast { get; set; }
		public double gbpusdPast { get; set; }
		public double nzdjpyPast { get; set; }
		public double nzdusdPast { get; set; }
		public double usdcadPast { get; set; }
		public double usdchfPast { get; set; }
		public double usdjpyPast { get; set; }

		// Just Sold Checks
		public bool audcadJustTraded { get; set; }
		public bool audjpyJustTraded { get; set; }
		public bool audusdJustTraded { get; set; }
		public bool cadjpyJustTraded { get; set; }
		public bool chfjpyJustTraded { get; set; }
		public bool euraudJustTraded { get; set; }
		public bool eurcadJustTraded { get; set; }
		public bool eurchfJustTraded { get; set; }
		public bool eurjpyJustTraded { get; set; }
		public bool eurusdJustTraded { get; set; }
		public bool gbpaudJustTraded { get; set; }
		public bool gbpchfJustTraded { get; set; }
		public bool gbpjpyJustTraded { get; set; }
		public bool gbpnzdJustTraded { get; set; }
		public bool gbpusdJustTraded { get; set; }
		public bool nzdjpyJustTraded { get; set; }
		public bool nzdusdJustTraded { get; set; }
		public bool usdcadJustTraded { get; set; }
		public bool usdchfJustTraded { get; set; }
		public bool usdjpyJustTraded { get; set; }

		// Pair POSITION P/L
		// Converted string from Position P/L text box, to a decimal
		// Simple recognized numeric values that can be used to measure true P/L of every pair currently owned
		public double audcadPosPL { get; set; }
		public double audjpyPosPL { get; set; }
		public double audusdPosPL { get; set; }
		public double cadjpyPosPL { get; set; }
		public double chfjpyPosPL { get; set; }
		public double euraudPosPL { get; set; }
		public double eurcadPosPL { get; set; }
		public double eurchfPosPL { get; set; }
		public double eurjpyPosPL { get; set; }
		public double eurusdPosPL { get; set; }
		public double gbpaudPosPL { get; set; }
		public double gbpchfPosPL { get; set; }
		public double gbpjpyPosPL { get; set; }
		public double gbpnzdPosPL { get; set; }
		public double gbpusdPosPL { get; set; }
		public double nzdjpyPosPL { get; set; }
		public double nzdusdPosPL { get; set; }
		public double usdcadPosPL { get; set; }
		public double usdchfPosPL { get; set; }
		public double usdjpyPosPL { get; set; }

		// Pair entry price decimals
		// Stored decimal values of the entry price in it' decimal point, non-converted, state
		public decimal audcadEntryPrice { get; set; }
		public decimal audjpyEntryPrice { get; set; }
		public decimal audusdEntryPrice { get; set; }
		public decimal cadjpyEntryPrice { get; set; }
		public decimal chfjpyEntryPrice { get; set; }
		public decimal euraudEntryPrice { get; set; }
		public decimal eurcadEntryPrice { get; set; }
		public decimal eurchfEntryPrice { get; set; }
		public decimal eurjpyEntryPrice { get; set; }
		public decimal eurusdEntryPrice { get; set; }
		public decimal gbpaudEntryPrice { get; set; }
		public decimal gbpchfEntryPrice { get; set; }
		public decimal gbpjpyEntryPrice { get; set; }
		public decimal gbpnzdEntryPrice { get; set; }
		public decimal gbpusdEntryPrice { get; set; }
		public decimal nzdjpyEntryPrice { get; set; }
		public decimal nzdusdEntryPrice { get; set; }
		public decimal usdcadEntryPrice { get; set; }
		public decimal usdchfEntryPrice { get; set; }
		public decimal usdjpyEntryPrice { get; set; }

		// Public Re-Entry Bools
		// True or false statements if the last entry was executed during the same interval
		public bool audcadReEnter { get; set; }
		public bool audjpyReEnter { get; set; }
		public bool audusdReEnter { get; set; }
		public bool cadjpyReEnter { get; set; }
		public bool chfjpyReEnter { get; set; }
		public bool euraudReEnter { get; set; }
		public bool eurcadReEnter { get; set; }
		public bool eurchfReEnter { get; set; }
		public bool eurjpyReEnter { get; set; }
		public bool eurusdReEnter { get; set; }
		public bool gbpaudReEnter { get; set; }
		public bool gbpchfReEnter { get; set; }
		public bool gbpjpyReEnter { get; set; }
		public bool gbpnzdReEnter { get; set; }
		public bool gbpusdReEnter { get; set; }
		public bool nzdjpyReEnter { get; set; }
		public bool nzdusdReEnter { get; set; }
		public bool usdcadReEnter { get; set; }
		public bool usdchfReEnter { get; set; }
		public bool usdjpyReEnter { get; set; }

		// Pair POSITION size integers
		public double audcadPosLot { get; set; }
		public double audjpyPosLot { get; set; }
		public double audusdPosLot { get; set; }
		public double cadjpyPosLot { get; set; }
		public double chfjpyPosLot { get; set; }
		public double euraudPosLot { get; set; }
		public double eurcadPosLot { get; set; }
		public double eurchfPosLot { get; set; }
		public double eurjpyPosLot { get; set; }
		public double eurusdPosLot { get; set; }
		public double gbpaudPosLot { get; set; }
		public double gbpchfPosLot { get; set; }
		public double gbpjpyPosLot { get; set; }
		public double gbpnzdPosLot { get; set; }
		public double gbpusdPosLot { get; set; }
		public double nzdjpyPosLot { get; set; }
		public double nzdusdPosLot { get; set; }
		public double usdcadPosLot { get; set; }
		public double usdchfPosLot { get; set; }
		public double usdjpyPosLot { get; set; }



		#endregion PUBLIC STRINGS


	}
}
