using GalaSoft.MvvmLight;

namespace Tsl.Core.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConnectViewModel : ViewModelBase
    {
        public ConnectViewModel()
        {
        }
		public string PageKey
		{
			get
			{
				return "ConnectTsl";
			}
		}
    }
}