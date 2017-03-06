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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace Tsl.Core.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public static class ViewModelLocator
    {
		private static bool AlreadyInjected = false;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public static void InjectDependencies()
        {
			if (AlreadyInjected) return;

			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			var container = SimpleIoc.Default;
			var navigation = new NavigationManager();

			container.Register<INavigationService>(() => navigation);
			container.Register<INavigationManager>(() => navigation);
			container.Register<IDispatcher>(() => new Dispatcher());

			AlreadyInjected = true;
        }

		public static T GetDependency<T>()
		{
			return (SimpleIoc.Default.IsRegistered<T>()) ?
				SimpleIoc.Default.GetInstance<T>() :
						 default(T);
		}

        public static MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

	}
}