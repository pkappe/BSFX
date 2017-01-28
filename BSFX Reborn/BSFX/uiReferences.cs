using fxcore2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BSFX
{
	public partial class BSFX : Form
	{
		public void dataGridView_CellContentClick(object data, DataGridViewCellEventArgs e, O2GResponse response)
		{
			data = response.ToString();
		}
	}
}
