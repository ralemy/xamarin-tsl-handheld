using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public interface IBarcodeMonitor
	{
		void OnBarcodeReceived(BarcodeCommand command);
		bool Enable { get; set; }
		event EventHandler<BarcodeData> BarcodeReceivedHandler;

}
}