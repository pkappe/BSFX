using BSFX.Properties;
using fxcore2;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		// **LOG-IN**
		// Log-in Button for FXCM
		private void fxcmLogin_Click(object fxcmLogin, EventArgs session)
		{			
			login();
		}

		public void login()
		{
			// Strings for session variables
			sessionTextBox.Text = "Connecting...";
			sUserID = userIDbox.Text.ToString();
			sPassword = passwordBox.Text.ToString();
			sURL = "http://www.fxcorporate.com/Hosts.jsp";
			if (accountLive.Checked == true)
			{
				sConnection = "Real";
				Settings.Default.sConnection = "Real";
				Settings.Default.Save();
			}
			else
			{
				sConnection = "Demo";
				Settings.Default.sConnection = "Demo";
				Settings.Default.Save();
			}

			//Try to create a session and connect to FXCM servers. 
			try
			{
				//Session creation
				mSession = O2GTransport.createSession();
				//Session Listener
				SessionStatusListener statusListener = new SessionStatusListener(mSession);
				mSession.subscribeSessionStatus(statusListener);
				//Table Listener
				TableListener tableListener = new TableListener();
				mSession.useTableManager(O2GTableManagerMode.Yes, null);
				// Response Listener
				ResponseListener responseListener = new ResponseListener(mSession);
				mSession.subscribeResponse(responseListener);
				// LOG-IN
				statusListener.login(sUserID, sPassword, sURL, sConnection);
				Thread.Sleep(100);

				//Confirm Connected
				if (statusListener.Connected)
				{
					sessionTextBox.BackColor = Color.Green;
					actiBox.AppendText("***CONNECTED***" + Environment.NewLine);
					sessionTextBox.Text = "CONNECTED";

					try
					{
						priceBW.RunWorkerAsync();
					}
					catch (Exception)
					{
						actiBox.AppendText("ERROR: UNABLE TO RETRIEVE PRICE UPDATES. PLEASE TRY LOGGING IN AGAIN." + Environment.NewLine);
					}
				}

				else if (statusListener.Disconnected)
				{
					for (int i = 0; i < 25; i++)
					{
						login();
					}
				}
				// Confirm Error, then end login
				else if (statusListener.Error)
				{
					mSession.logout();
					while (!statusListener.Disconnected)
						Thread.Sleep(50);
					sessionTextBox.BackColor = Color.Red;
					sessionTextBox.Text = "DISCONNECTED";
					actiBox.AppendText("Login Failed!" + Environment.NewLine);
				}

				O2GSessionDescriptorCollection descs = mSession.getTradingSessionDescriptors();
				foreach (O2GSessionDescriptor desc in descs)
				{
					mSession.setTradingSession("sessionID", "pin");
				}

			}
			catch (Exception loginErr)
			{
				Console.WriteLine("Exception: {0}", loginErr.ToString());
			}
		}
		// **AFTER LOG IN**
		// Background worker for table listeners
		private void priceBW_DoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine("Entered BW");
			// Enable timers.
			pastTimer.Enabled = true;
			pastTimer.Start();
			oneSecTimer.Enabled = true;
			oneSecTimer.Start();

			mSession.useTableManager(O2GTableManagerMode.Yes, null);

			tableManager = mSession.getTableManager();
			managerStatus = tableManager.getStatus();

			while (managerStatus == O2GTableManagerStatus.TablesLoading)
			{
				Console.WriteLine("Tables are loading...");
				Thread.Sleep(50);
				managerStatus = tableManager.getStatus();
			}
			if (managerStatus == O2GTableManagerStatus.TablesLoadFailed)
			{
				Console.WriteLine("Tabled loading failed");
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("WARNING: LOADING TABLES FAILED!" + Environment.NewLine); }));
				return;
			}
			// Check Accounts Table and Grab Information
			try
			{
			
				O2GLoginRules loginRules = mSession.getLoginRules();
				Console.WriteLine("Tables are loaded!");	
				// Check if Accounts table is loaded automatically
				if (loginRules != null && loginRules.isTableLoadedByDefault(O2GTableType.Accounts))
				{
					Console.WriteLine("Login rules != null");
					// If table is loaded, use getTableRefreshResponse method
					O2GResponse accountsResponse = loginRules.getTableRefreshResponse(O2GTableType.Accounts);					
					O2GResponseReaderFactory responseFactory = mSession.getResponseReaderFactory();
					if (responseFactory != null)
					{
						Console.WriteLine("Grabbing account data....");
						O2GAccountsTableResponseReader accountsReader = responseFactory.createAccountsTableReader(accountsResponse);

						for (int i = 0; i < accountsReader.Count; i++)
						{
							account = accountsReader.getRow(i);							
							accountValue = account.Balance;
							this.Invoke(new MethodInvoker(delegate { accountValueBox.Text = "$" + Convert.ToString(accountValue); }));
							availLev = account.Balance;
							this.Invoke(new MethodInvoker(delegate { accountLevBox.Text = "$" + Convert.ToString(availLev); }));
							sAccountID = account.AccountID.ToString();
							amountLimit = account.AmountLimit;
							baseSize = account.BaseUnitSize;
						}
					}
				}
				else
				{
					// If table is not loaded, use createRefreshTableRequest method
					O2GRequestFactory requestFactory = mSession.getRequestFactory();
					if (requestFactory != null)
					{
						O2GRequest request = requestFactory.createRefreshTableRequest(O2GTableType.Accounts);
						mSession.sendRequest(request);
						Thread.Sleep(1000);
					}
				}				
			}
			catch (Exception acctErr)
			{
				Console.WriteLine(acctErr);
			}

			// Check if all 20 pairs needed are subscribed to on the account.
			try
			{
				O2GLoginRules loginRules = mSession.getLoginRules();

				if (loginRules != null && loginRules.isTableLoadedByDefault(O2GTableType.Offers))
				{
					O2GResponse offersResponse = loginRules.getTableRefreshResponse(O2GTableType.Offers);
					O2GResponseReaderFactory responseFactory = mSession.getResponseReaderFactory();
					if (responseFactory != null)
					{
						Console.WriteLine("Triggering offers row");
						O2GOffersTableResponseReader offersReader = responseFactory.createOffersTableReader(offersResponse);

						for (int i = 0; i < offersReader.Count; i++)
						{
							O2GOfferRow offers = offersReader.getRow(i);
							
							string checkOffer = offers.OfferID;
						}
					}					
				}
				// Get MMR prices and set position initial cost
				Console.WriteLine("Grabbing MMR");
				this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Retrieving Pair MMR..." + Environment.NewLine); }));
				O2GTradingSettingsProvider settingResponse = loginRules.getTradingSettingsProvider();
				audcadMMR = settingResponse.getMMR("AUD/CAD", account);
				audcadInitial = shortLot * audcadMMR;
				audjpyMMR = settingResponse.getMMR("AUD/JPY", account);
				audjpyInitial = shortLot * audjpyMMR;
				audusdMMR = settingResponse.getMMR("AUD/USD", account);
				audusdInitial = shortLot * audusdMMR;
				cadjpyMMR = settingResponse.getMMR("CAD/JPY", account);
				cadjpyInitial = shortLot * cadjpyMMR;
				chfjpyMMR = settingResponse.getMMR("CHF/JPY", account);
				chfjpyInitial = shortLot * chfjpyMMR;
				euraudMMR = settingResponse.getMMR("EUR/AUD", account);
				euraudInitial = shortLot * euraudMMR;
				eurcadMMR = settingResponse.getMMR("EUR/CAD", account);
				eurcadInitial = shortLot * eurcadMMR;
				eurchfMMR = settingResponse.getMMR("EUR/CHF", account);
				eurchfInitial = shortLot * eurchfMMR;
				eurjpyMMR = settingResponse.getMMR("EUR/JPY", account);
				eurjpyInitial = shortLot * eurjpyMMR;
				eurusdMMR = settingResponse.getMMR("EUR/USD", account);
				eurusdInitial = shortLot * eurusdMMR;
				gbpaudMMR = settingResponse.getMMR("GBP/AUD", account);
				gbpaudInitial = shortLot * gbpaudMMR;
				gbpchfMMR = settingResponse.getMMR("GBP/CHF", account);
				gbpchfInitial = shortLot * gbpchfMMR;
				gbpjpyMMR = settingResponse.getMMR("GBP/JPY", account);
				gbpjpyInitial = shortLot * gbpjpyMMR;
				gbpnzdMMR = settingResponse.getMMR("GBP/NZD", account);
				gbpnzdInitial = shortLot * gbpnzdMMR;
				gbpusdMMR = settingResponse.getMMR("GBP/USD", account);
				gbpusdInitial = shortLot * gbpusdMMR;
				nzdjpyMMR = settingResponse.getMMR("NZD/JPY", account);
				nzdjpyInitial = shortLot * nzdjpyMMR;
				nzdusdMMR = settingResponse.getMMR("NZD/USD", account);
				nzdusdInitial = shortLot * nzdusdMMR;
				usdcadMMR = settingResponse.getMMR("USD/CAD", account);
				usdcadInitial = shortLot * usdcadMMR;
				usdchfMMR = settingResponse.getMMR("USD/CHF", account);
				usdchfInitial = shortLot * usdchfMMR;
				usdjpyMMR = settingResponse.getMMR("USD/JPY", account);
				usdjpyInitial = shortLot * usdjpyMMR;

				// Set highest move to 0
				highestMove = 0;

				// Set all move values to 0;
			}
			catch (Exception mmrErr)
			{
				Console.WriteLine(mmrErr);
			}

			Console.WriteLine("Initializing needed table events.");
			// Initiate Table Getters
			O2GOffersTable offersTable = (O2GOffersTable)tableManager.getTable(O2GTableType.Offers);			
			accountsTable = (O2GAccountsTable)tableManager.getTable(O2GTableType.Accounts);
			O2GTradesTable tradesTable = (O2GTradesTable)tableManager.getTable(O2GTableType.Trades);
			//O2GOrdersTable ordersTable = (O2GOrdersTable)tableManager.getTable(O2GTableType.Orders);
			//O2GClosedTradesTable closedTradesTable = (O2GClosedTradesTable)tableManager.getTable(O2GTableType.ClosedTrades);
			//O2GMessagesTable messagesTable = (O2GMessagesTable)tableManager.getTable(O2GTableType.Messages);
			//O2GSummaryTable summaryTable = (O2GSummaryTable)tableManager.getTable(O2GTableType.Summary);

			
			// Trigger Table Events for Subscription
			offersTable.RowChanged += new EventHandler<RowEventArgs>(offersTable_RowChanged);
			accountsTable.RowChanged += new EventHandler<RowEventArgs>(accountsTable_RowChanged);
			tradesTable.RowChanged += new EventHandler<RowEventArgs>(tradesTable_RowChanged);
			//ordersTable.RowChanged += new EventHandler<RowEventArgs>(ordersTable_RowChanged);
			//closedTradesTable.RowChanged += new EventHandler<RowEventArgs>(closedTradesTable_RowChanged);
			//messagesTable.RowChanged += new EventHandler<RowEventArgs>(messagesTable_RowChanged);
			//summaryTable.RowChanged += new EventHandler<RowEventArgs>(summaryTable_RowChanged);

			// Check pair subscription status, and add if needed.

			this.Invoke(new MethodInvoker(delegate { actiBox.AppendText("Connection Established.... Monitoring Pairs..." + Environment.NewLine); }));
			// Start Past Price Timer
			pastTimer.Start();
		}
		
	}

	public class SessionStatusListener : IO2GSessionStatus
	{
		private bool mConnected = false;
		private bool mDisconnected = false;
		private bool mError = false;
		private O2GSession mSession;
		private object mEvent = new object();

		public SessionStatusListener(O2GSession session)
		{
			mSession = session;
		}
		public bool Connected
		{
			get
			{
				return mConnected;
			}
		}
		public bool Disconnected
		{
			get
			{
				return mDisconnected;
			}
		}
		public bool Error
		{
			get
			{
				return mError;
			}
		}

		public void onSessionStatusChanged(O2GSessionStatusCode status)
		{
			
			Console.WriteLine("Status: " + status.ToString());
			if (status == O2GSessionStatusCode.Connected)
			{
				mConnected = true;
			}				
			else
			{
				mConnected = false;
			}
				

			if (status == O2GSessionStatusCode.Disconnected)
			{
				mDisconnected = true;				
			}				
			else
			{
				mDisconnected = false;
			}
				

			if (status == O2GSessionStatusCode.TradingSessionRequested)
			{
				if (BSFX.SessionID == "")
					Console.WriteLine("Argument for trading session ID is missing");
				else
					mSession.setTradingSession(BSFX.SessionID, BSFX.Pin);
			}
			else if (status == O2GSessionStatusCode.Connected)
			{
				lock (mEvent)
					Monitor.PulseAll(mEvent);
			}

		}

		public void onLoginFailed(string error)
		{
			Console.WriteLine("Login error: " + error);
			mError = true;
			lock (mEvent)
				Monitor.PulseAll(mEvent);
		}

		public void login(string sUserID, string sPassword, string sURL, string sConnection)
		{
			mSession.login(sUserID, sPassword, sURL, sConnection);
			lock (mEvent)
				Monitor.Wait(mEvent, 60000);
		}
	}
}
