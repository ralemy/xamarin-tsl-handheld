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
			OnTagReceived(_command);
		}

		public void OnTagReceived(TranspondersCommandBase command)
		{
			command.TransponderReceived += TagReceived;
			command.Response.CommandComplete += ReceiveComplete;
		}

		protected virtual void TagReceived(object sender, TransponderDataEventArgs e)
		{
			if (Enabled)
				cache.Add(e.Transponder);
			if (ShouldDispatch(e))
				Dispatch(!_commandCompleted);
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
			if (!Enabled) 
				return false;
			if (!e.MoreAvailable)
				return true;
			if (cache.Count > _maxCacheCount)
				return true;
			return OldSend();
		}

		bool OldSend()
		{
			return DateTime.Now.Subtract(lastSentTime).TotalMilliseconds > _maxTimeBetweenSends;
		}

		void Dispatch(bool completed)
		{
			TagReceivedHandler?.Invoke(this, new TagData(Flush(), completed));
			lastSentTime = DateTime.Now;
		}

		void ReceiveComplete(object sender, EventArgs e)
		{
			Dispatch(_commandCompleted);
		}

		public bool ProcessReceivedLine(IAsciiResponseLine line, bool moreLinesAvailable)
		{
			return _command.Responder.ProcessReceivedLine(line, moreLinesAvailable);
		}
	}
}
