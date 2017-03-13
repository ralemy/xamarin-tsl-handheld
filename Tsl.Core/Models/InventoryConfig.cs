using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class InventoryConfig : ConfigBase
	{
		public int? Power { get; set; }
		public bool? StrongestTag { get; set; }
		public bool? IncludeRssi { get; set; }

		public InventoryConfig()
		{
			Power = 29;
			IncludeRssi = false;
			StrongestTag = false;
		}

		public int MaxOutputPower
		{
			get { return LibraryConfiguration.Current.MaximumOutputPower; }
		}
		public int MinOutputPower
		{
			get { return LibraryConfiguration.Current.MinimumOutputPower; }
		}


		public void Configure(InventoryCommand command)
		{
			command.TakeNoAction = true;
			command.ResetParameters = true;
			command.OutputPower = Power;
			command.FilterStrongest = BoolToTriState(StrongestTag);
			command.IncludeTransponderRssi = BoolToTriState(IncludeRssi);

		}


	}

}