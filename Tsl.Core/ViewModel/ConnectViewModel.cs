using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using TechnologySolutions.Rfid.AsciiProtocol.Extensions;

namespace Tsl.Core.ViewModel
{
	public class ConnectViewModel : ViewModelBase, ILifeCycle
	{
		public ICommand AddNewCommand { get; private set; }
		public ICommand BackCommand { get; private set; }
		public ICommand ConnectCommand { get; private set; }
		public ICommand DisconnectCommand { get; private set; }
		public ICommand RefreshListCommand { get; private set; }


		public ObservableCollection<INamedReader> Readers { get; private set; }

		private string _errorMessage;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			private set { Set(() => ErrorMessage, ref _errorMessage, value); }
		}

		private string _infoMessage;
		public string InformationMessage
		{
			get { return _infoMessage; }
			private set { Set(() => InformationMessage, ref _infoMessage, value); }
		}

		public bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			private set { Set(() => IsBusy, ref _isBusy, value); }
		}

		private INamedReader _selectedReader;
		public INamedReader SelectedReader
		{
			get
			{
				return _selectedReader;
			}
			set
			{
				if (Set(() => SelectedReader, ref _selectedReader, value))
					UpdateCommands();
			}
		}


		INavigationService _navigator;


		IReaderConnectionManager _connection;

		public ConnectViewModel(INavigationService navigator, IReaderConnectionManager connection)
		{
			_navigator = navigator;
			_connection = connection;
			SetupCommands();
			SignalStartOp();
			Readers = new ObservableCollection<INamedReader>();
		}

		void SetupCommands()
		{
			AddNewCommand = new RelayCommand(ExecuteAddNew);
			BackCommand = new RelayCommand(_navigator.GoBack);
			ConnectCommand = new RelayCommand(ExecuteConnect, CanExecuteConnect);
			DisconnectCommand = new RelayCommand(ExecuteDisconnect, CanExecuteDisconnect);
			RefreshListCommand = new RelayCommand(ExecuteRefreshList);

		}
		void SignalStartOp()
		{
			SignalStartOp(null);
		}
		void SignalStartOp(string p)
		{
			IsBusy = true;
			ErrorMessage = null;
			InformationMessage = p;
		}

		void SignalSuccess(string p)
		{
			IsBusy = false;
			ErrorMessage = null;
			InformationMessage = p;
		}

		void SignalFailure(string p)
		{
			IsBusy = false;
			InformationMessage = null;
			ErrorMessage = p;
		}
		async void ExecuteRefreshList()
		{
			SignalStartOp();
			try
			{
				var currentReaders = await _connection.ListAvailableReadersAsync();
				UpdateReaderList(currentReaders.ToList());
				SignalSuccess(null);
			}
			catch (Exception ex)
			{
				SignalFailure("Exception in RefreshList " + ex.Message);
			}
			finally
			{
				UpdateCommands();
			}
		}

		void UpdateReaderList(List<INamedReader> list)
		{
			var unchangedAvailables = Readers.Join(list, x => x.DisplayName, y => y.DisplayName, (x, y) => x);
			var unchangedPaired = Readers.Join(list, x => x.DisplayName, y => y.DisplayName, (x, y) => y);
			Readers.Except(unchangedAvailables).FirstOrDefault(p => Readers.Remove(p) && false);
			list.Except(unchangedPaired).FirstOrDefault(p => { Readers.Add(p); return false;});
		}

		bool CanExecuteDisconnect()
		{
			return _connection != null && _connection.ConnectedReader != null;
		}

		void ExecuteDisconnect()
		{
			SignalStartOp();
			try
			{
				_connection.Disconnect();
				SignalSuccess("Disconnected");
			}
			catch (Exception ex)
			{
				SignalFailure("Disconnect Failed: " + ex.Message);
			}
			finally
			{
				UpdateCommands();
			}
		}

		bool CanExecuteConnect()
		{
			return SelectedReader != null &&
				_connection != null &&
				_connection.ConnectedReader == null;
		}

		async void ExecuteConnect()
		{
			SignalStartOp("Connecting");
			try
			{
				await _connection.ConnectAsync(SelectedReader);
				SignalSuccess("Connected");
			}
			catch (Exception ex)
			{
				SignalFailure("Error Connecting " + ex.Message);
			}
			finally
			{
				UpdateCommands();
			}

		}

		private void ExecuteAddNew()
		{
			SignalStartOp();
			try
			{
				_connection.AddNewReader();
				SignalSuccess(null);
			}
			catch (Exception ex)
			{
				SignalFailure("Failed to Add new reader " + ex.Message);
			}
			finally
			{
				UpdateCommands();
			}
		}

		void UpdateCommands()
		{
			(ConnectCommand as RelayCommand).RaiseCanExecuteChanged();
			(DisconnectCommand as RelayCommand).RaiseCanExecuteChanged();
		}

		public static string PageKey
		{
			get
			{
				return "ConnectTsl";
			}
		}

		public void Appearing(object sender, EventArgs e)
		{
			RefreshListCommand.Execute(null);
		}

		public void Disappearing(object sender, EventArgs e)
		{
		}
	}
}