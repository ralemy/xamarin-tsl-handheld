using System;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Extensions;

namespace Tsl.Core
{
	public class ReaderInfoService
	{
		private readonly IAsciiSerialTransportConsumer _commander;

		private readonly IReaderConnectionManager _connection;

		private readonly TslReaderInfo _info;

		 
		public ReaderInfoService(IReaderConnectionManager readerConnectionManager,IAsciiSerialTransportConsumer commander, TslReaderInfo readerInfo)
		{
			_commander = commander;
			_connection = readerConnectionManager;
			_info = readerInfo;

			_connection.ConnectionStateChanged += ConnectionStateChanged;
		}

		void ConnectionStateChanged(object sender, EventArgs e)
		{
			
		}
}
}
