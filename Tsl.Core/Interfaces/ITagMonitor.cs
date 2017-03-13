using System;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public interface ITagMonitor
	{
		void OnTagReceived(TranspondersCommandBase command);
		bool Enabled { get; set; }
		event EventHandler<TagData> TagReceivedHandler;
	}
}
