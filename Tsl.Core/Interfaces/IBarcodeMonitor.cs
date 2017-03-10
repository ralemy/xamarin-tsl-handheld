using System;

namespace Tsl.Core
{
	public interface IBarcodeMonitor
	{
		bool Enable { get; set; }
		event EventHandler<BarcodeData> BarcodeReceivedHandler;
	}
}