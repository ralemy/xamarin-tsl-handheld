using System;
using Xamarin.Forms;

namespace Tsl.Core
{
	public interface INavigationManager
	{
		object parameter { get; }
		void Register(string key, Type pageClass);
		void SetMain(NavigationPage page);
	}
}
