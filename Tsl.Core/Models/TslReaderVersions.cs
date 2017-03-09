using System;
using GalaSoft.MvvmLight;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class TslReaderVersions :ObservableObject
	{
		private static readonly string _emptyVersion = "0.0.0";

		private string firmware = _emptyVersion;
		public string Firmware
		{
			get
			{
				return firmware;
			}
			set
			{
				Set(() => Firmware, ref firmware, value);
			}
		}

		private string bootloader = _emptyVersion;
 		public string Bootloader
		{
			get
			{
				return bootloader;
			}
			set
			{
				Set(() => Bootloader, ref bootloader, value);
			}
		}

		private string radioFirmware = _emptyVersion;
		public string RadioFirmware
		{
			get
			{
				return radioFirmware;
			}
			set
			{
				Set(() => RadioFirmware, ref radioFirmware, value);
			}
		}

		private string radioBootloader = _emptyVersion;
		public string RadioBootloader
		{
			get
			{
				return radioBootloader;
			}
			set
			{
				Set(() => RadioBootloader, ref radioBootloader, value);
			}
		}


		public TslReaderVersions()
		{
		}

		internal void Update(VersionInformationCommand v)
		{
			Bootloader = v.BootloaderVersion;
			Firmware = v.FirmwareVersion;
			RadioBootloader = v.RadioBootloaderVersion;
			RadioFirmware = v.RadioFirmwareVersion;
		}
	}
}
