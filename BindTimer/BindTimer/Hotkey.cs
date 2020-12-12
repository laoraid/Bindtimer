using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace BindTimer
{
	public class Hotkey
	{
		private readonly Window window;
		private int hotkeycode;
		Action callback;
		readonly Settings set = Settings.Instance;

		public Hotkey(Window w, Action callback)
		{
			window = w;
			hotkeycode = set.Hotkey;
			this.callback = callback;
		}

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

		[DllImport("user32.dll")]
		private static extern int UnregisterHotKey(IntPtr hwnd, int id);
		public void Register()
		{
			hotkeycode = set.Hotkey;
			var helper = new WindowInteropHelper(window);
			HwndSource source = HwndSource.FromHwnd(helper.Handle);
			source.AddHook(WndProc);
			if (!RegisterHotKey(helper.Handle, 9000, 0x00, hotkeycode))
			{
				MessageBox.Show("단축키 등록 오류", "단축키를 등록 할 수 없습니다.",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 0x0312 && wParam.ToInt32() == 9000)
			{
				callback();
			}
			return IntPtr.Zero;
		}

		public void Unregister()
		{
			UnregisterHotKey(new WindowInteropHelper(window).Handle, 9000);
		}
	}
}
