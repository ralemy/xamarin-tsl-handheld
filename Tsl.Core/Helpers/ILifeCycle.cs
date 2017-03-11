using System;

namespace Tsl.Core
{
	public interface ILifeCycle
	{
		void Appearing(object sender, EventArgs e);
		void Disappearing(object sender, EventArgs e);
	}
}