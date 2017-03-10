using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class BarcodeMonitor : IBarcodeMonitor , IAsciiCommandResponder
	{
		BarcodeCommand _barcode;

		public BarcodeMonitor()
		{
			Enable = true;
			_barcode = new BarcodeCommand();
			_barcode.BarcodeReceived += (sender, e) =>
			{
				if (Enable)
					BarcodeReceivedHandler?.Invoke(this, new BarcodeData(e);
			};
		}

		public bool Enable { get; set; }

		public event EventHandler<BarcodeData> BarcodeReceivedHandler;

		public bool ProcessReceivedLine(IAsciiResponseLine line, bool moreLinesAvailable)
		{
			return _barcode.Responder.ProcessReceivedLine(line, moreLinesAvailable);
		}
	}
}
