using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class SwitchConfig : ConfigBase
	{

		public bool AsyncReporting { get; set; }
		public bool HapticFeedback { get; set; }
		public SwitchAction DoublePress { get; set; }
		public SwitchAction SinglePress { get; set; }
		public EventHandler<SwitchStateEventArgs> SwitchStateChanged;

		public void Configure(SwitchActionCommand command)
		{
			command.AsynchronousReportingEnabled = BoolToTriState(AsyncReporting);
			command.DoublePressAction = DoublePress;
			command.SinglePressAction = SinglePress;
			command.IsHapticFeedbackEnabled = BoolToTriState(HapticFeedback);
			command.ResetParameters = true;
		}

		public SwitchConfig()
		{
			Reset();
		}

		public void Reset()
		{
			AsyncReporting = true;
			HapticFeedback = true;
			DoublePress = SwitchAction.Barcode;
			SinglePress = SwitchAction.Inventory;
			SwitchStateChanged = null;
		}

	}
}
