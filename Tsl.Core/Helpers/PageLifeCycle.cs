using System;
using Xamarin.Forms;

namespace Tsl.Core
{
	public static class PageLifeCycle
	{
		public static void BindLifeCycle(this Page page, ILifeCycle viewModel)
		{
			page.BindingContext = viewModel;
			page.Appearing += viewModel.Appearing;
			page.Disappearing += viewModel.Disappearing;
		}
	}
}
