using System;
using GalaSoft.MvvmLight;
using TechnologySolutions.Rfid.AsciiProtocol;
using TechnologySolutions.Rfid.AsciiProtocol.Commands;

namespace Tsl.Core
{
	public class TslReaderInfo : ObservableObject
	{
		private string _model = string.Empty;
		public String Model
		{
			get
			{
				return _model;
			}
			set
			{
				Set(ref _model, value);
			}
		}

		private string _imageUri = string.Empty;
		public string ImageUri
		{
			get
			{
				return _imageUri;
			}
			set
			{
				Set(ref _imageUri, value);
			}
		}

		private int _battery = 0;
		public int Battery
		{
			get
			{
				return _battery;
			}
			set
			{
				Set(() => Battery, ref _battery, value);
			}
		}

		private int _power = 29;
		public int Power
		{
			get
			{
				return _power;
			}
			set
			{
				Set(() => Power, ref _power, value);
			}
		}

		public int MaxPower
		{
			get
			{
				return LibraryConfiguration.Current.MaximumOutputPower;
			}
		}

		public int MinPower
		{
			get
			{
				return LibraryConfiguration.Current.MinimumOutputPower;
			}
		}


		public TslReaderSerials Serials { get; private set; }
		public TslReaderVersions Versions { get; private set; }

		public TslReaderInfo()
		{
			Serials = new TslReaderSerials();
			Versions = new TslReaderVersions();
		}
		public void Update(VersionInformationCommand v)
		{
			Serials.update(v);
			Versions.Update(v);
			Model = ModelFromSerial(v.SerialNumber);
		}

		string ModelFromSerial(string serialNumber)
		{
			return (!string.IsNullOrEmpty(serialNumber) && serialNumber.Length >= 4) ?
				serialNumber.Substring(0, 4) : "unknown";
		}
	}
}
