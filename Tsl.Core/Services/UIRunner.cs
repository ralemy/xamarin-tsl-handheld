using System;
using Xamarin.Forms;

namespace Tsl.Core
{
	public class UIRunner : IUIRunner
	{
		public void RunOnUIThread(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}
	}
}
