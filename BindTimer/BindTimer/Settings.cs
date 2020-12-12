using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BindTimer
{
	public class Settings
	{
		IniFile ini;
		readonly string INIFILENAME = "BindTimer.ini";

		private Settings()
		{
			ini = new IniFile();
			if (File.Exists(INIFILENAME))
				ini.Load(INIFILENAME);
			else
			{
				ini["BindTimer"]["Addtime"] = 2;
				ini["BindTimer"]["Hotkey"] = "Pause";
				Save();
			}
		}
		public void Save()
		{
			ini.Save(INIFILENAME);
		}

		private static readonly Lazy<Settings> _ins = new Lazy<Settings>(() => new Settings());

		public static Settings Instance => _ins.Value;

		public int Addtime
		{
			get => ini["BindTimer"]["Addtime"].ToInt();
			set => ini["BindTimer"]["Addtime"] = value;
		}
		public int Hotkey
		{
			get
			{
				string keystring = ini["BindTimer"]["Hotkey"].ToString();
				Key keyclass = (Key)Enum.Parse(typeof(Key), keystring);
				return KeyInterop.VirtualKeyFromKey(keyclass);
			}
		}

		public string HotkeyByString
		{
			get => ini["BindTimer"]["Hotkey"].ToString();
			set
			{
				_ = Enum.Parse(typeof(Key), value);
				ini["BindTimer"]["Hotkey"] = value;
			}
		}
	}
}
