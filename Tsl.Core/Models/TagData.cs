using System;
using System.Collections.Generic;
using TechnologySolutions.Rfid.AsciiProtocol;

namespace Tsl.Core
{
	public class TagData : EventArgs
	{
		public IEnumerable<TransponderData> Tags { get; set; }

		public bool Complete { get; set; }

		public TagData()
		{
		}
		public TagData(IEnumerable<TransponderData> t, bool c)
		{
			Tags = t;
			Complete = c;
		}
	}
}
