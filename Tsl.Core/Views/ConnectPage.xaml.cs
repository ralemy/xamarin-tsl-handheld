using System;
using System.Collections.Generic;
using Tsl.Core.ViewModel;
using Xamarin.Forms;

namespace Tsl.Core
{
	public partial class ConnectPage : ContentPage
	{
		public ConnectPage()
		{
			InitializeComponent();
			this.BindingContext = ViewModelLocator.GetDependency<ConnectViewModel>();
		}
	}
}
