using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;
using TechnologySolutions.Rfid.AsciiProtocol.Extensions;

namespace Tsl.Core
{
	public class ReaderInfoService
	{
		private readonly IAsciiSerialTransportConsumer _consumer;
		private readonly IAsciiCommander _commander;
		private readonly IReaderConnectionManager _connection;
		private readonly TslReaderInfo _info;

		public INamedReader ConnectedReader()
		{
			return _connection.ConnectedReader;
		}

		public async Task ConfigureReader(InventoryConfig config)
		{
			var command = new InventoryCommand();
			command.TakeNoAction = true;
			command.ResetParameters = true;
			command.OutputPower = config.Power;
			command.FilterStrongest = BoolToTriState(config.StrongestTag);
			command.IncludeTransponderRssi = BoolToTriState(config.IncludeRssi);
			await Task.Run(() =>
			{
				try
				{
					_commander.Execute(command);
				}
				catch (Exception ex)
				{
					LogException(ex.Message);	
				}
			});
		}

		void LogException(string message)
		{
		}

		TriState? BoolToTriState(bool? flag)
		{
			return flag.HasValue && (bool)flag ? TriState.Yes : TriState.No;
		}

		public ReaderInfoService(IReaderConnectionManager readerConnectionManager, IAsciiSerialTransportConsumer consumer, TslReaderInfo readerInfo)
		{
			_consumer = consumer;
			_commander = consumer as IAsciiCommander;
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
			if (_consumer.Transport != null)
				Messenger.Default.Send(new GenericMessage<TslReaderInfo>(null));
			_consumer.Transport = null;
		}

		void ConnectToReader()
		{
			_consumer.Transport = _connection.ConnectionTransport;
			if (_connection.ConnectedReader != null)
				SetReader(new VersionInformationCommand(), 0);
		}

		bool SetReader(VersionInformationCommand version, int tries)
		{

			if (tries > 2) 
				return false;

			_commander.Execute(new AbortCommand());
			_commander.Execute(new FactoryDefaultsCommand());
			_commander.Execute(version);

			if (!version.Response.IsSuccessful)
				return SetReader(new VersionInformationCommand(), tries +1);

			_info.Update(version);
			Messenger.Default.Send(new GenericMessage<TslReaderInfo>(_info));
			return true;
		}

	}
}
