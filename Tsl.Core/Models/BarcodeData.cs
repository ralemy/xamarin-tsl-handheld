using System;
using TechnologySolutions.Rfid.AsciiProtocol;

namespace Tsl.Core
{
	public class BarcodeData : EventArgs
	{
		public string Barcode;
		public DateTime Timestamp;

		public BarcodeData() { }

		public BarcodeData(BarcodeEventArgs e) 
		{
			Barcode = e.Barcode;
			Timestamp = e.Timestamp;
		}
	}
}