/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Tsl.Core"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Extensions;

namespace Tsl.Core.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public static class ViewModelLocator
    {
		private static readonly string _tagResponderKey = "RFIDResponder";
		private static readonly string _barcodeResponderKey = "BarcodeResponder";

		private static bool AlreadyInjected = false;
		/// <summary>
		/// Registers the following Dependencies:
		/// 1- INavigationService: used to Navigate to app pages following MVVMLight protocol
		/// 2- INavigationManager: used to register app pages
		/// 3- IDispatcher: used to ensure that code is dispatched to the main thread for UI double binding
		/// </summary>
		public static void InjectDependencies()
		{
			if (AlreadyInjected) return;

			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			var container = SimpleIoc.Default;

			container.Register<INavigationService>(() => new NavigationManager());
			container.Register(() =>
							   container.GetInstance<INavigationService>()
							   as INavigationManager);

			container.Register<IReaderConnectionManager>(() => new ReaderConnectionManager());


			container.Register<IUIRunner>(() => new UIRunner());
			container.Register<TslReaderInfo>();
			container.Register<ConnectViewModel>(true);


			container.Register(() => AddResponders(container, new AsciiCommander()));
			container.Register(() => container.GetInstance<IAsciiCommander>()
							   as IAsciiSerialTransportConsumer);
			container.Register<ReaderInfoService>(true);
			AlreadyInjected = true;
		}

		static void RegisterResponders(SimpleIoc container)
		{
			container.Register<SwitchAsynchronousResponder>();

			container.Register<IAsciiCommandResponder>(() => TagMonitor.InventoryMonitorFactory(), _tagResponderKey);
			container.Register(() =>
							   container.GetInstance<IAsciiCommandResponder>(_tagResponderKey)
							   as ITagMonitor);

			container.Register<IAsciiCommandResponder>(() => new BarcodeMonitor(), _barcodeResponderKey);
			container.Register(() =>
							   container.GetInstance<IAsciiCommandResponder>(_barcodeResponderKey)
							   as IBarcodeMonitor);
		}

		static IAsciiCommander AddResponders(SimpleIoc container, AsciiCommander asciiCommander)
		{

			RegisterResponders(container);

			asciiCommander.AddSynchronousResponder();
			asciiCommander.AddResponder(container.GetInstance<IAsciiCommandResponder>(_tagResponderKey));
			asciiCommander.AddResponder(container.GetInstance<IAsciiCommandResponder>(_barcodeResponderKey));
			asciiCommander.AddResponder(container.GetInstance<SwitchAsynchronousResponder>());
			return asciiCommander;
		}

		public static T GetDependency<T>()
		{
			return (SimpleIoc.Default.IsRegistered<T>()) ?
				SimpleIoc.Default.GetInstance<T>() :
						 default(T);
		}

		public static string ConnectPageKey { get { return ConnectViewModel.PageKey;} }

		public static void Register<T> () where T: class
		{
			if(!SimpleIoc.Default.IsRegistered<T>())
				SimpleIoc.Default.Register<T>();
		}

		public static void RegisterPages(INavigationManager navigation)
		{
			if (navigation == null)
				throw new ArgumentNullException(nameof(navigation),"Navigation Manager not registered. call InjectDependencies() before this function");
			navigation.Register(ConnectViewModel.PageKey, typeof(ConnectPage));

		}


        public static ConnectViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConnectViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

}
}