/**************************************************************
 		Davide Infantino - 2019
    This file is part of Sfondo.exe

    Sfondo.exe is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Nome-Programma is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Nome-Programma.If not, see<http://www.gnu.org/licenses/>.
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Sfondo
{
	[DataContract]
	public class DirectoryBase
	{
		[DataMember]
		public string dirpath;
		[DataMember]
		public string filter;
		[DataMember]
		public bool recursive;
	}

	[DataContract]
	public class FileBase
	{
		[DataMember]
		public List<string> filepath;
	}

	[DataContract]
	public class LoginSFTP
	{
		[DataMember]
		public string user;
		[DataMember]
		public string password;
		[DataMember]
		public int port;
	}

	[DataContract]
	public class ListFileFS
	{
		[DataMember]
		public FileBase files;
	}

	[DataContract]
	public class ListFileSFTP: LoginSFTP
	{
		[DataMember]
		public FileBase files;
	}

	[DataContract]
	public class DirFS
	{
		[DataMember]
		public string directory;
	}

	[DataContract]
	public class ListDirSFTP : LoginSFTP
	{
		[DataMember]
		public FileBase files;
	}

	[DataContract]
	public class EnableOrNotEnable 
	{
		[DataMember]
		public bool enable;
	}

	[DataContract]
	public class Location : EnableOrNotEnable
	{
		[DataMember]
		public int X;
		[DataMember]
		public int Y;
	}

	[DataContract]
	public class FormSize : EnableOrNotEnable
	{
		[DataMember]
		public int Width;
		[DataMember]
		public int Height;
	}


	[DataContract]
	public class Configuration
	{
		[DataMember]
		public int SecondNextImage;
		[DataMember]
		public int MillisecondNextTransition;
		[DataMember]
		public int StepForTransition;

		[DataMember]
		public Location StartLocation;
		[DataMember]
		public FormSize StartFormSize;

		[DataMember]
		public List<DirFS> WatchDirFS;
	}

	public class GestoreConfigurazione
	{
		private string GetJsonConfiguration()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().Location + ".json";
		}

		public Configuration LoadConfiguration()
		{
			try
			{
				return Deserializzatore();
			}
			catch
			{
				Configuration cfg = new Configuration();
				cfg.SecondNextImage = 10;
				cfg.MillisecondNextTransition = 20;
				cfg.StepForTransition = 20;

				cfg.StartLocation = new Location();
				cfg.StartLocation.enable = false;
				cfg.StartLocation.X = 0;
				cfg.StartLocation.Y = 0;

				cfg.StartFormSize = new FormSize();
				cfg.StartFormSize.enable = false;
				cfg.StartFormSize.Width = 100;
				cfg.StartFormSize.Height = 100;

				cfg.WatchDirFS = new List<DirFS>();
				cfg.WatchDirFS.Add(new DirFS() { directory = "c:\\cacio\\fa" });
				Serializzatore(cfg);
			}

			return Deserializzatore();
		}

		
		private void Serializzatore(Configuration cfg)
		{
			string path = GetJsonConfiguration();
			using (StreamWriter sw = new StreamWriter(path))
			{
				DataContractJsonSerializerSettings setting = new DataContractJsonSerializerSettings();
				setting.UseSimpleDictionaryFormat = true;
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration), setting);
				ser.WriteObject(sw.BaseStream, cfg);
				sw.Flush();
			}
		}

		private Configuration Deserializzatore()
		{
			string path = GetJsonConfiguration();
			Configuration ret;
			using (StreamReader sr = new StreamReader(path))
			{
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration));
				ret = ser.ReadObject(sr.BaseStream) as Configuration;
			}
			return ret;
		}
	}
}