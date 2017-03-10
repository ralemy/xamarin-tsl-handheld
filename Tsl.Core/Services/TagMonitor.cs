using System;
using System.Collections.Generic;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class TagMonitor : ITagMonitor, IAsciiCommandResponder
	{
		private static readonly int _maxCacheCount = 10;
		private static readonly int _maxTimeBetweenSends = 500;
		private static readonly bool _commandCompleted = true;

		public bool Enabled { get; set; }
		public event EventHandler<TagData> TagReceivedHandler;

		DateTime lastSentTime = DateTime.Now;
		TranspondersCommandBase _command;
		List<TransponderData> cache = new List<TransponderData>();


		public static TagMonitor InventoryMonitorFactory()
		{
			return new TagMonitor(new InventoryCommand());
		}

		public TagMonitor(TranspondersCommandBase command)
		{
			Enabled = true;
			_command = command;
			_command.TransponderReceived += TagReceived;
			_command.Response.CommandComplete += ReceiveComplete;
		}

		protected virtual void TagReceived(object sender, TransponderDataEventArgs e)
		{
			if (!Enabled) return;
			cache.Add(e.Transponder);
			if (ShouldDispatch(e))
				TagReceivedHandler?.Invoke(this, new TagData(Flush(), !_commandCompleted));
		}

		private List<TransponderData> Flush()
		{
			var result = cache;
			cache = new List<TransponderData>();
			lastSentTime = DateTime.Now;
			return result;
		}

		bool ShouldDispatch(TransponderDataEventArgs e)
		{
			if (!Enabled) return false;
			if (e.MoreAvailable)
				if (cache.Count < _maxCacheCount)
					if (RecentSend())
						return false;
			return true;
		}

		bool RecentSend()
		{
			return DateTime.Now.Subtract(lastSentTime).TotalMilliseconds < _maxTimeBetweenSends;
		}

		void ReceiveComplete(object sender, EventArgs e)
		{
			TagReceivedHandler?.Invoke(this, new TagData(Flush(), _commandCompleted));
		}

		public bool ProcessReceivedLine(IAsciiResponseLine line, bool moreLinesAvailable)
		{
			return _command.Responder.ProcessReceivedLine(line, moreLinesAvailable);
		}
	}
}
