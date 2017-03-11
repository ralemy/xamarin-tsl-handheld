using System;
using GalaSoft.MvvmLight.Messaging;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;
using TechnologySolutions.Rfid.AsciiProtocol.Extensions;

namespace Tsl.Core
{
	public class ReaderInfoService
	{
		private readonly IAsciiSerialTransportConsumer _commander;

		private readonly IReaderConnectionManager _connection;

		private readonly TslReaderInfo _info;


		public ReaderInfoService(IReaderConnectionManager readerConnectionManager, IAsciiSerialTransportConsumer commander, TslReaderInfo readerInfo)
		{
			_commander = commander;
			_connection = readerConnectionManager;
			_info = readerInfo;

			_connection.ConnectionStateChanged += ConnectionStateChanged;
		}

		void ConnectionStateChanged(object sender, EventArgs e)
		{
			if (_connection.ConnectionState == ReaderConnectionState.Disconnecting)
				DisconnectFromReader();
			else
				ConnectToReader();
		}

		void DisconnectFromReader()
		{
			if (_commander.Transport != null)
				Messenger.Default.Send(new GenericMessage<TslReaderInfo>(null));
			_commander.Transport = null;
		}

		void ConnectToReader()
		{
			var commander = _commander as IAsciiCommander;
			_commander.Transport = _connection.ConnectionTransport;
			if (_connection.ConnectedReader != null)
				SetReader(commander , new VersionInformationCommand(), 0);
		}

		bool SetReader(IAsciiCommander commander, VersionInformationCommand version, int tries)
		{

			if (tries > 2) 
				return false;

			commander.Execute(new AbortCommand());
			commander.Execute(new FactoryDefaultsCommand());
			commander.Execute(version);

			if (!version.Response.IsSuccessful)
				return SetReader(commander, new VersionInformationCommand(), tries +1);

			_info.Update(version);
			Messenger.Default.Send(new GenericMessage<TslReaderInfo>(_info));
			return true;
		}

	}
}
