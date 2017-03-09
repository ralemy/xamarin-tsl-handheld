using System;
namespace Tsl.Core
{
	public interface IUIRunner
	{
		void RunOnUIThread(Action action);
	}
}
