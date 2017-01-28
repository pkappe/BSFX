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
		public double availLev { get; set; }
		public double acctAllocated { get; set; }
		public double unrealPL { get; set; }
		public double accountValue { get; set; }

		// Order Placement
		public string sOfferID { get; set; }
		public string mRequestID { get; set; }		
		public int iAmount { get; set; }
		public string sBuySell { get; set; }
		public string sOrderType { get; set; }
		public double shortLot { get; set; }
		public string sTradeID { get; set; }
		public double dRate { get; set; }

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
		public decimal bidd { get; set; }
		public string bids { get; set; }
		public double ask { get; set; }
		public decimal askTrue { get; set; }
		public decimal askd { get; set; }
		public string asks { get; set; }
		public decimal startSpread { get; set; }
		public double orginSpread { get; set; }
		public int interval { get; set; }		
	
		// Trading Parameters
		public decimal buyLong { get; set; }
		public decimal sellShort { get; set; }
		public decimal goalLong { get; set; }
		public decimal goalShort { get; set; }
		public decimal maxSpread { get; set; }
		public Int64 lotSize { get; set; }
		public decimal stopLoss { get; set; }
		public decimal highestMove { get; set; }

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

		// MMR sets
		public double audcadMMR { get; set; }
		public double audjpyMMR { get; set; }
		public double audusdMMR { get; set; }
		public double cadjpyMMR { get; set; }
		public double chfjpyMMR { get; set; }
		public double euraudMMR { get; set; }
		public double eurcadMMR { get; set; }
		public double eurchfMMR { get; set; }
		public double eurjpyMMR { get; set; }
		public double eurusdMMR { get; set; }
		public double gbpaudMMR { get; set; }
		public double gbpchfMMR { get; set; }
		public double gbpjpyMMR { get; set; }
		public double gbpnzdMMR { get; set; }
		public double gbpusdMMR { get; set; }
		public double nzdjpyMMR { get; set; }
		public double nzdusdMMR { get; set; }
		public double usdcadMMR { get; set; }
		public double usdchfMMR { get; set; }
		public double usdjpyMMR { get; set; }

		// Initial Cost for position entry
		public double audcadInitial { get; set; }
		public double audjpyInitial { get; set; }
		public double audusdInitial { get; set; }
		public double cadjpyInitial { get; set; }
		public double chfjpyInitial { get; set; }
		public double euraudInitial { get; set; }
		public double eurcadInitial { get; set; }
		public double eurchfInitial { get; set; }
		public double eurjpyInitial { get; set; }
		public double eurusdInitial { get; set; }
		public double gbpaudInitial { get; set; }
		public double gbpchfInitial { get; set; }
		public double gbpjpyInitial { get; set; }
		public double gbpnzdInitial { get; set; }
		public double gbpusdInitial { get; set; }
		public double nzdjpyInitial { get; set; }
		public double nzdusdInitial { get; set; }
		public double usdcadInitial { get; set; }
		public double usdchfInitial { get; set; }
		public double usdjpyInitial { get; set; }

		// Pair sides  (Long or Short)		
		public string audcadSideString { get; set; }
		public string audjpySideString { get; set; }
		public string audusdSideString { get; set; }
		public string cadjpySideString { get; set; }
		public string chfjpySideString { get; set; }
		public string euraudSideString { get; set; }
		public string eurcadSideString { get; set; }
		public string eurchfSideString { get; set; }
		public string eurjpySideString { get; set; }
		public string eurusdSideString { get; set; }
		public string gbpaudSideString { get; set; }
		public string gbpchfSideString { get; set; }
		public string gbpjpySideString { get; set; }
		public string gbpnzdSideString { get; set; }
		public string gbpusdSideString { get; set; }
		public string nzdjpySideString { get; set; }
		public string nzdusdSideString { get; set; }
		public string usdcadSideString { get; set; }
		public string usdchfSideString { get; set; }
		public string usdjpySideString { get; set; }

		// Order ID's from FXCM
		public string audcadOrderID { get; set; }
		public string audjpyOrderID { get; set; }
		public string audusdOrderID { get; set; }
		public string cadjpyOrderID { get; set; }
		public string chfjpyOrderID { get; set; }
		public string euraudOrderID { get; set; }
		public string eurcadOrderID { get; set; }
		public string eurchfOrderID { get; set; }
		public string eurjpyOrderID { get; set; }
		public string eurusdOrderID { get; set; }
		public string gbpaudOrderID { get; set; }
		public string gbpchfOrderID { get; set; }
		public string gbpjpyOrderID { get; set; }
		public string gbpnzdOrderID { get; set; }
		public string gbpusdOrderID { get; set; }
		public string nzdjpyOrderID { get; set; }
		public string nzdusdOrderID { get; set; }
		public string usdcadOrderID { get; set; }
		public string usdchfOrderID { get; set; }
		public string usdjpyOrderID { get; set; }

		// Just Sold Checks
		public bool audcadJustSold { get; set; }
		public bool audjpyJustSold { get; set; }
		public bool audusdJustSold { get; set; }
		public bool cadjpyJustSold { get; set; }
		public bool chfjpyJustSold { get; set; }
		public bool euraudJustSold { get; set; }
		public bool eurcadJustSold { get; set; }
		public bool eurchfJustSold { get; set; }
		public bool eurjpyJustSold { get; set; }
		public bool eurusdJustSold { get; set; }
		public bool gbpaudJustSold { get; set; }
		public bool gbpchfJustSold { get; set; }
		public bool gbpjpyJustSold { get; set; }
		public bool gbpnzdJustSold { get; set; }
		public bool gbpusdJustSold { get; set; }
		public bool nzdjpyJustSold { get; set; }
		public bool nzdusdJustSold { get; set; }
		public bool usdcadJustSold { get; set; }
		public bool usdchfJustSold { get; set; }
		public bool usdjpyJustSold { get; set; }

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

		// Since the P/L is measured by the entry price minus the ask price for long, and bid price for short,
		// seperate decimals are needed to store those values.

		// Pair LONG POSITION Move
		public decimal audcadBuyLongCheck { get; set; }
		public decimal audjpyBuyLongCheck { get; set; }
		public decimal audusdBuyLongCheck { get; set; }
		public decimal cadjpyBuyLongCheck { get; set; }
		public decimal chfjpyBuyLongCheck { get; set; }
		public decimal euraudBuyLongCheck { get; set; }
		public decimal eurcadBuyLongCheck { get; set; }
		public decimal eurchfBuyLongCheck { get; set; }
		public decimal eurjpyBuyLongCheck { get; set; }
		public decimal eurusdBuyLongCheck { get; set; }
		public decimal gbpaudBuyLongCheck { get; set; }
		public decimal gbpchfBuyLongCheck { get; set; }
		public decimal gbpjpyBuyLongCheck { get; set; }
		public decimal gbpnzdBuyLongCheck { get; set; }
		public decimal gbpusdBuyLongCheck { get; set; }
		public decimal nzdjpyBuyLongCheck { get; set; }
		public decimal nzdusdBuyLongCheck { get; set; }
		public decimal usdcadBuyLongCheck { get; set; }
		public decimal usdchfBuyLongCheck { get; set; }
		public decimal usdjpyBuyLongCheck { get; set; }

		// Pair SHORT POSITION Move
		public decimal audcadShortSaleCheck { get; set; }
		public decimal audjpyShortSaleCheck { get; set; }
		public decimal audusdShortSaleCheck { get; set; }
		public decimal cadjpyShortSaleCheck { get; set; }
		public decimal chfjpyShortSaleCheck { get; set; }
		public decimal euraudShortSaleCheck { get; set; }
		public decimal eurcadShortSaleCheck { get; set; }
		public decimal eurchfShortSaleCheck { get; set; }
		public decimal eurjpyShortSaleCheck { get; set; }
		public decimal eurusdShortSaleCheck { get; set; }
		public decimal gbpaudShortSaleCheck { get; set; }
		public decimal gbpchfShortSaleCheck { get; set; }
		public decimal gbpjpyShortSaleCheck { get; set; }
		public decimal gbpnzdShortSaleCheck { get; set; }
		public decimal gbpusdShortSaleCheck { get; set; }
		public decimal nzdjpyShortSaleCheck { get; set; }
		public decimal nzdusdShortSaleCheck { get; set; }
		public decimal usdcadShortSaleCheck { get; set; }
		public decimal usdchfShortSaleCheck { get; set; }
		public decimal usdjpyShortSaleCheck { get; set; }

		// Pair POSITION Move, Multiplied by LotSize, Decimals for Longs
		// Used for conversion to find recognized value, in order to be placed into a textbox for the user
		public decimal audcadPosMoveDec { get; set; }
		public decimal audjpyPosMoveDec { get; set; }
		public decimal audusdPosMoveDec { get; set; }
		public decimal cadjpyPosMoveDec { get; set; }
		public decimal chfjpyPosMoveDec { get; set; }
		public decimal euraudPosMoveDec { get; set; }
		public decimal eurcadPosMoveDec { get; set; }
		public decimal eurchfPosMoveDec { get; set; }
		public decimal eurjpyPosMoveDec { get; set; }
		public decimal eurusdPosMoveDec { get; set; }
		public decimal gbpaudPosMoveDec { get; set; }
		public decimal gbpchfPosMoveDec { get; set; }
		public decimal gbpjpyPosMoveDec { get; set; }
		public decimal gbpnzdPosMoveDec { get; set; }
		public decimal gbpusdPosMoveDec { get; set; }
		public decimal nzdjpyPosMoveDec { get; set; }
		public decimal nzdusdPosMoveDec { get; set; }
		public decimal usdcadPosMoveDec { get; set; }
		public decimal usdchfPosMoveDec { get; set; }
		public decimal usdjpyPosMoveDec { get; set; }		

		// Pair POSITION Move, Multiplied by LotSize, Decimals for Shorts
		public decimal audcadPosMoveShortDec { get; set; }
		public decimal audjpyPosMoveShortDec { get; set; }
		public decimal audusdPosMoveShortDec { get; set; }
		public decimal cadjpyPosMoveShortDec { get; set; }
		public decimal chfjpyPosMoveShortDec { get; set; }
		public decimal euraudPosMoveShortDec { get; set; }
		public decimal eurcadPosMoveShortDec { get; set; }
		public decimal eurchfPosMoveShortDec { get; set; }
		public decimal eurjpyPosMoveShortDec { get; set; }
		public decimal eurusdPosMoveShortDec { get; set; }
		public decimal gbpaudPosMoveShortDec { get; set; }
		public decimal gbpchfPosMoveShortDec { get; set; }
		public decimal gbpjpyPosMoveShortDec { get; set; }
		public decimal gbpnzdPosMoveShortDec { get; set; }
		public decimal gbpusdPosMoveShortDec { get; set; }
		public decimal nzdjpyPosMoveShortDec { get; set; }
		public decimal nzdusdPosMoveShortDec { get; set; }
		public decimal usdcadPosMoveShortDec { get; set; }
		public decimal usdchfPosMoveShortDec { get; set; }
		public decimal usdjpyPosMoveShortDec { get; set; }
		
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
		public Int64 audcadPosLot { get; set; }
		public Int64 audjpyPosLot { get; set; }
		public Int64 audusdPosLot { get; set; }
		public Int64 cadjpyPosLot { get; set; }
		public Int64 chfjpyPosLot { get; set; }
		public Int64 euraudPosLot { get; set; }
		public Int64 eurcadPosLot { get; set; }
		public Int64 eurchfPosLot { get; set; }
		public Int64 eurjpyPosLot { get; set; }
		public Int64 eurusdPosLot { get; set; }
		public Int64 gbpaudPosLot { get; set; }
		public Int64 gbpchfPosLot { get; set; }
		public Int64 gbpjpyPosLot { get; set; }
		public Int64 gbpnzdPosLot { get; set; }
		public Int64 gbpusdPosLot { get; set; }
		public Int64 nzdjpyPosLot { get; set; }
		public Int64 nzdusdPosLot { get; set; }
		public Int64 usdcadPosLot { get; set; }
		public Int64 usdchfPosLot { get; set; }
		public Int64 usdjpyPosLot { get; set; }

		
		
		#endregion PUBLIC STRINGS
	}
}
