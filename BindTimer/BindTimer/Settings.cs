using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindTimer
{
	public class Settings
	{
		IniFile ini;

		private Settings()
		{
			ini = new IniFile();
			// if ini exist : load / else : create
		}

		private static readonly Lazy<Settings> _ins = new Lazy<Settings>(() => new Settings());

		public static Settings Instance => _ins.Value;

		public int addtime
		{
			get => ini["BindTimer"]["addtime"].ToInt();
			set => ini["BindTimer"]["addtime"] = value;
		}
	}
}
