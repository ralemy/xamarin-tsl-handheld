using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace Tsl.Core
{
	public class NavigationManager : INavigationManager, INavigationService, INotifyPropertyChanged
	{
		private readonly Dictionary<string, Type> _pages;
		private NavigationPage _page
		{
			get; 
			set;
		}
		private string _current;

		public event PropertyChangedEventHandler PropertyChanged;

		public NavigationManager()
		{
			_pages = new Dictionary<string, Type>();
		}

		string FindKey(Type type)
		{
			return _pages.ContainsValue(type) ?
						 _pages.First(t => t.Value == type).Key :
						 null;
		}

		public string CurrentPageKey
		{
			get
			{
				return _current;
			}
			private set
			{
				if (_current == value) return;
				_current = value;
				OnPropertyChanged("CurrentPageKey");
			}
		}

		public object parameter { get; private set; }

		public void GoBack()
		{
			if (_current != null)
				_page.PopAsync();
		}

		public void GoTop()
		{
			_page.PopToRootAsync();
		}

		public void NavigateTo(string pageKey)
		{
			NavigateTo(pageKey, null);
		}

		public void NavigateTo(string pageKey, object parameter)
		{
			lock(_pages)
			{

				if (!_pages.ContainsKey(pageKey))
					throw new ArgumentException(
						string.Format("No page registered under key {0}", pageKey),
						"PageKey");
				NavigateToPage(pageKey, parameter);
			}
		}

		void NavigateToPage(string pageKey, object parameter)
		{
			var parameters = parameter == null ? new object[0] : new object[] { parameter };
			var constructor = GetConstructor(_pages[pageKey], parameter);
			if (constructor == null)
				throw new ArgumentException(
					string.Format("No constructor for page {0} that takes {1}" ,
					              pageKey,
			  					  parameter == null ? "no arguments" : "a single argument")
					, "PageParameters");
			_page.PushAsync(constructor.Invoke(parameters) as Page);
		}

		ConstructorInfo GetConstructor(Type type, object parameter)
		{
			return type.GetTypeInfo()
					   .DeclaredConstructors
					   .FirstOrDefault(GetConstructorPredicate(type, parameter));
		}

		Func<ConstructorInfo,bool> GetConstructorPredicate(Type type, object parameter)
		{
			return parameter == null ?
				(Func<ConstructorInfo, bool>)(f => f.GetParameters().Count() == 0) :
				f => f.GetParameters().Count() == 1 && 
			          f.GetParameters()[0].ParameterType == parameter.GetType();
		}

		public void Register(string key, Type pageClass)
		{
			lock(_pages)
			{
				if (_pages.ContainsKey(key))
					_pages[key] = pageClass;
				else
					_pages.Add(key, pageClass);
			}
		}

		public void SetMain(NavigationPage page)
		{
			EventHandler<NavigationEventArgs> syncKey = (sender, e) => CurrentPageKey = FindKey(_page.CurrentPage.GetType());
			_page = page;
			_page.Popped += syncKey;
			_page.PoppedToRoot += syncKey;
			_page.Pushed += syncKey;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
			INavigationService i;
		}
	}
}
