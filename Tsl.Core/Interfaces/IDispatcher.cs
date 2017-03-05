using System;
namespace Tsl.Core
{
	public interface IDispatcher
	{
		void InvokeOnUserInterfaceThread(Action action);
	}
}
