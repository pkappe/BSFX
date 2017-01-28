using fxcore2;
namespace BSFX {


	public partial class Data : IO2GTableListener
	{
		static O2GSession mSession;
		static TableListener tableListener;
		private O2GTableManager mTblMgr;

		partial class O2GOfferTableDataTable
		{

		}
	
		partial class PriceTableDataTable
		{

		}

		public void onAdded(string rowID, O2GRow rowData)
		{
			O2GOfferTableDataTable offer = (O2GOfferTableDataTable)(rowData);
			

		}

		public void onChanged(string rowID, O2GRow rowData)
		{
			throw new System.NotImplementedException();
		}

		public void onDeleted(string rowID, O2GRow rowData)
		{
			throw new System.NotImplementedException();
		}

		public void onStatusChanged(O2GTableStatus status)
		{
			throw new System.NotImplementedException();
		}
	}
}
