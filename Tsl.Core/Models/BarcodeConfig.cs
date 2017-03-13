using System;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class BarcodeConfig : ConfigBase
	{
		
		public bool IncludeDateTime { get; set; }
		public bool EscapeBarcode { get; set; }
		public double SyncWaitTime { get; set; }
		public int ScanTime { get; set; }
		public bool UseAlert { get; set; }


		public BarcodeConfig()
		{
			IncludeDateTime = false;
			EscapeBarcode = false;
			SyncWaitTime = 8.0;
			ScanTime = 9;
		}
		public void Configure(BarcodeCommand command)
		{
			command.IncludeDateTime = BoolToTriState(IncludeDateTime);
			command.IsBarcodeEscaped = BoolToTriState(EscapeBarcode);
			command.UseAlert = BoolToTriState(UseAlert);
			command.MaxSynchronousWaitTime = SyncWaitTime;
			command.ScanTime = ScanTime;
			command.TakeNoAction = true;
			command.ResetParameters = true;
		}
	}
}
