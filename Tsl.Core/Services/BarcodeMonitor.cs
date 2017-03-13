using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class BarcodeMonitor : IBarcodeMonitor, IAsciiCommandResponder
	{
		BarcodeCommand _command;

		public BarcodeMonitor()
		{
			Enable = true;
			_command = new BarcodeCommand();
			OnBarcodeReceived(_command);
		}

		public void OnBarcodeReceived(BarcodeCommand command)
		{
			command.BarcodeReceived += (sender, e) =>
			{
				if (Enable)
					BarcodeReceivedHandler?.Invoke(this, new BarcodeData(e));
			};
		}

		public bool Enable { get; set; }

		public event EventHandler<BarcodeData> BarcodeReceivedHandler;

		public bool ProcessReceivedLine(IAsciiResponseLine line, bool moreLinesAvailable)
		{
			return _command.Responder.ProcessReceivedLine(line, moreLinesAvailable);
		}
	}
}
