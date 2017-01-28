using fxcore2;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		void messagesTable_RowChanged(object sender, RowEventArgs e)
		{
			Console.WriteLine("UPDATE: MESSAGES TABLE");
		}
	}
}
