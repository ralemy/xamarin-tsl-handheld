using System;
using GalaSoft.MvvmLight.Ioc;

namespace Tsl.Core
{
	public static class Dependency
	{
		public static T Get<T>()
		{
			return SimpleIoc.Default.IsRegistered<T>() ?
				            SimpleIoc.Default.GetInstance<T>() : default(T);
		}
		
	}
}
