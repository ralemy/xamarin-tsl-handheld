using System;
namespace Tsl.Core
{
	public class Dispatcher : IDispatcher
	{
		public void InvokeOnUserInterfaceThread(Action action)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(action);
		}
	}
}
