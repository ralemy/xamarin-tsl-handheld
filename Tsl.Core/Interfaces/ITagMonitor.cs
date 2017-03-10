using System;
namespace Tsl.Core
{
	public interface ITagMonitor
	{
		bool Enabled { get; set; }
		event EventHandler<TagData> TagReceivedHandler;
	}
}
