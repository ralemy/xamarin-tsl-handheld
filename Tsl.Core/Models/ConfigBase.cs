using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public abstract class ConfigBase
	{
		public ConfigBase()
		{
		}

		public virtual void Configure(AsciiCommandBase command) {
		}

		protected TriState? BoolToTriState(bool? flag)
		{
			return flag.HasValue && (bool)flag ? TriState.Yes : TriState.No;
		}

	}
}
