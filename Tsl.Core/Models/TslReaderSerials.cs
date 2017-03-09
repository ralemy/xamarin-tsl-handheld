using System;
using GalaSoft.MvvmLight;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class TslReaderSerials: ObservableObject
	{
		private string _device = string.Empty;
		public string Device
		{
			get
			{
				return _device;
			}
			set
			{
				Set(() => Device, ref _device, value);
			}
		}

		private string _radio = string.Empty;
		public string Radio
		{
			get
			{
				return _radio;
			}
			set
			{
				Set(() => Radio, ref _radio, value);
			}
		}	

		private string _antenna = string.Empty;
		public string Antenna
		{
			get
			{
				return _antenna;
			}
			set
			{
				Set(() => Antenna, ref _antenna, value);
			}
		}

		public TslReaderSerials()
		{
		}

		internal void update(VersionInformationCommand v)
		{
			Antenna = v.AntennaSerialNumber;
			Device = v.SerialNumber;
			Radio = v.RadioSerialNumber;
		}
	}
}
