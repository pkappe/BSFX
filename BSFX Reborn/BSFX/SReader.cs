using System;
using System.IO;

namespace BSFX
{
	class SReader : BSFX
	{
		public object sConsole = Console.ReadLine();
		//private StreamReader readCon;


		public void readDaCon(Object sConsole, StreamReader readCon)
		{
			string rConsole = sConsole.ToString();

			while (rConsole != null)
			{
				actiBox.AppendText(rConsole);
			}
			
		}
	
	
	}
}
