using System;
using GalaSoft.MvvmLight.Views;
using Tsl.Core;
using Xamarin.Forms;
namespace Tsl
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			InitializeMvvmPlugins();
			MainPage = ConfigureAppPages();
		}

		Page ConfigureAppPages()
		{
			var navigation = Dependency.Get<INavigationService>() as NavigationServices;
			var mainPage = new NavigationPage(new MainPage());
			Core.ViewConfigurator.Configure(navigation);
			return mainPage;
		}

		void InitializeMvvmPlugins()
		{
			Core.ViewModel.ViewModelLocator.InjectDependencies();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
