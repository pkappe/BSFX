using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFXLogin : Form
	{
		public BSFXLogin()
		{
			InitializeComponent();
		}

		public SqlConnection connect = new SqlConnection("Data Source=184.168.194.62;Initial Catalog=kaptech_;Integrated Security=False;User ID=AuthAccount;Password=1Tqnl12&;Encrypt=False;Packet Size=4096");
		public string userName { get; set; }
		public string password { get; set; }
		public string dbUserName { get; set; }
		public string dbPassword { get; set; }
		public string allow { get; set; }
		public string logLoc { get; set; }
		public string appLocation { get; set; }
		public string time { get; set; }
		public string date { get; set; }


		// Login Button
		private void logIn_Click(object sender, EventArgs e)
		{
			try
			{
				// Log Setters
				time = DateTime.Now.ToString("hh:mm:ss tt");
				date = DateTime.Now.ToString("M-d");
				appLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				if (appLocation.StartsWith(@"file:\"))
				{
					appLocation = appLocation.Substring(6);
				}
				logLoc = appLocation + @"\" + @"Logs\" + date + ".txt";
				File.AppendAllText(logLoc, Environment.NewLine + Environment.NewLine + time + ":" + " BSFX LOGIN INITIALIZED!" + Environment.NewLine);
				File.AppendAllText(logLoc, Environment.NewLine + time + ":" + " LOGIN BUTTON CLICKED" + Environment.NewLine);

				
			}
			catch (Exception loginErr)
			{
				Console.WriteLine(loginErr);
				File.AppendAllText(logLoc, time + ":" + " ERROR: " + loginErr + Environment.NewLine);				
			}

			// Grab user name and password from textboxes
			try
			{				
				if (userNameBox.Text != "" && userPwdBox.Text != "")
				{
					userName = userNameBox.Text.ToString();
					password = userPwdBox.Text.ToString();
				}
				else
				{
					MessageBox.Show("Please provide your BSFX user name and password.", "Missing Login Info", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			catch (Exception loginErr)
			{
				Console.WriteLine(loginErr);
			}

			// Connect to Database
			try
			{
				// If DB connection is not open...
				if (connect.State == ConnectionState.Closed)
				{
					connect.Open();
					File.AppendAllText(logLoc, time + ":" + " OPENING CONNECTION" + Environment.NewLine);
					File.AppendAllText(logLoc, time + ":" + " CONNECTION IS: " + connect.State + Environment.NewLine);
				}
			}
			catch (Exception loginErr)
			{
				Console.WriteLine(loginErr);
			}

			try
			{
				if (connect.State == ConnectionState.Open)
				{
					// Query to find user name in table
					string getQuery = "SELECT * FROM dbo.UserAuth WHERE UserName = " + "'" + userName + "'";
					// Set query command on connect
					SqlCommand command = new SqlCommand(getQuery, connect);
					File.AppendAllText(logLoc, time + ":" + " Query Commands Set..." + Environment.NewLine);
					// Execute reader to view row information
					SqlDataReader reader = command.ExecuteReader();
					File.AppendAllText(logLoc, time + ":" + " Row Reader Initialized..." + Environment.NewLine);
					// While the reader works...

					while (reader.Read())
					{
						try
						{
							ReadSingleRow((IDataRecord)reader);
							File.AppendAllText(logLoc, time + ":" + " Reader is reading..." + Environment.NewLine);
						}
						catch (Exception loginErr)
						{
							File.AppendAllText(logLoc, time + ":" + " ERROR: " + loginErr + Environment.NewLine);
						}
						
					}
					File.AppendAllText(logLoc, time + ":" + " Reader Complete!" + Environment.NewLine);
					reader.Close();
					File.AppendAllText(logLoc, time + ":" + " Reader Closed." + Environment.NewLine);
				}
				else
				{
					File.AppendAllText(logLoc, time + ":" + " Caution: Connection state is closed." + Environment.NewLine);
				}
				
			}
			catch (Exception loginErr)
			{
				File.AppendAllText(logLoc, time + ":" + " ERROR: " + loginErr + Environment.NewLine);
			}

			
		}

		// DB Reader Class
		private void ReadSingleRow(IDataRecord dataRecord)
		{
			try
			{
				Console.WriteLine(String.Format("{0}, {1}, {2}", dataRecord[0], dataRecord[1], dataRecord[2]));

				dbUserName = dataRecord[0].ToString();
				dbPassword = dataRecord[1].ToString();
				allow = dataRecord[2].ToString();

				if (dbUserName == userName && dbPassword == password && allow == "Y")
				{
					BSFX bsfx = new BSFX();
					connect.Close();
					this.Hide();
					File.AppendAllText(logLoc, Environment.NewLine + Environment.NewLine + time + ":" + " Login complete! Initializing main window..." + Environment.NewLine);
					bsfx.Show();					
				}
				else
				{
					Console.WriteLine("ERROR OCCURED WHEN COMPARING USER NAMES AND PASSWORDS");					
				}
			}
			catch (Exception readErr)
			{
				Console.WriteLine(readErr);			
			}
		}
		
		private void BSFXLogin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				logIn.Click += logIn_Click;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (connect.State == ConnectionState.Open)
			{
				connect.Close();				
			}
			Application.Exit();
		}
	}
}
