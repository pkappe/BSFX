using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BSFX;

namespace BSFX
{
	public partial class Parameters : Form
	{
		public Parameters()
		{
			InitializeComponent();			
		}
		private void Parameters_Load(object sender, EventArgs e)
		{
			getParams();
		}

		public void paramOK_Click(object sender, EventArgs e)
		{
			try
			{
				BSFX bsfx = new BSFX();
				bsfx.lotSizeBox.Text = lotSizeBoxParam.Text.ToString();
				this.Invoke(new MethodInvoker(delegate { bsfx.lotSizeBox.Text = lotSizeBoxParam.Text.ToString(); }));
				this.Invoke(new MethodInvoker(delegate { bsfx.maxPosBox.Text = maxPosBoxParam.Text; }));
				this.Invoke(new MethodInvoker(delegate { bsfx.maxSpreadBox.Text = maxSpreadBoxParam.Text; }));
				this.Invoke(new MethodInvoker(delegate { bsfx.moveBox.Text = moveBoxParam.Text; }));
				this.Invoke(new MethodInvoker(delegate { bsfx.goalBox.Text = goalBoxParam.Text; }));
				this.Invoke(new MethodInvoker(delegate { bsfx.stopLossBox.Text = stopLossBoxParam.Text; }));
				this.Invoke(new MethodInvoker(delegate { bsfx.intervalBox.Text = intervalBoxParam.Text; }));
				bsfx.Update();
				this.Close();
			}
			catch (Exception updateErr)
			{
				Console.WriteLine(updateErr);
			}						
		}

		public void getParams()
		{
			BSFX bsfx = new BSFX();
			lotSizeBoxParam.Text = bsfx.lotSizeBox.Text;
			maxPosBoxParam.Text = bsfx.maxPosBox.Text;
			maxSpreadBoxParam.Text = bsfx.maxSpreadBox.Text;
			moveBoxParam.Text = bsfx.moveBox.Text;
			goalBoxParam.Text = bsfx.goalBox.Text;
			stopLossBoxParam.Text = bsfx.stopLossBox.Text;
			intervalBoxParam.Text = bsfx.intervalBox.Text;
		}

		private void lotSizeBoxParam_TextChanged(object sender, EventArgs e)
		{
		}
		private void maxPosBoxParam_TextChanged(object sender, EventArgs e)
		{
		}
		private void maxSpreadBoxParam_TextChanged(object sender, EventArgs e)
		{
		}
		private void moveBoxParam_TextChanged(object sender, EventArgs e)
		{
		}
		private void stopLossBoxParam_TextChanged(object sender, EventArgs e)
		{
		}
		private void goalBoxParam_TextChanged(object sender, EventArgs e)
		{
		}

		private void paramCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
	}
}
