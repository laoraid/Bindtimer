using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BindTimer
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

		[DllImport("user32.dll")]
		private static extern int UnregisterHotKey(IntPtr hwnd, int id);

		readonly int PEROID = 92;
		int now = 92;
		Timer timer;
		bool istimerstarted = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var helper = new WindowInteropHelper(this);
			HwndSource source = HwndSource.FromHwnd(helper.Handle);
			source.AddHook(WndProc);
			if (!RegisterHotKey(helper.Handle, 9000, 0x00, 0x13))
			{
				MessageBox.Show("오류");
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UnregisterHotKey(new WindowInteropHelper(this).Handle, 9000);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 0x0312 && wParam.ToInt32() == 9000)
			{
				StartTimer();
			}
			return IntPtr.Zero;
		}

		private void StartTimer()
		{
			if (istimerstarted)
				timer.Dispose();

			now = PEROID;
			SystemSounds.Asterisk.Play();
			BackGroundAnimationStart();

			txttimer.Text = now.ToString();

			istimerstarted = true;
			timer = new Timer(Tick, null, 0, 1000);
		}

		private void Tick(object o)
		{
			if (now == 0)
			{
				timer.Dispose();
				Dispatcher.Invoke(() => txttimer.Text = "써도 됨");
				istimerstarted = false;
				SystemSounds.Asterisk.Play();
				return;
			}

			Dispatcher.Invoke(() => txttimer.Text = now.ToString());
			now--;
		}

		private void BackGroundAnimationStart()
		{
			Storyboard sb = new Storyboard();

			ColorAnimation backgroundAnim = new ColorAnimation
			{
				From = Colors.Red,
				To = Colors.Lime,
				Duration = new Duration(TimeSpan.FromSeconds(now))
			};

			Storyboard.SetTarget(backgroundAnim, timerbackground);
			Storyboard.SetTargetProperty(backgroundAnim, new PropertyPath("Background.Color"));
			sb.Children.Add(backgroundAnim);
			sb.Begin(this);
		}

		private void TimerStartWrapper(object sender, RoutedEventArgs e)
		{
			StartTimer();
		}

		private void menualwaystop_Click(object sender, RoutedEventArgs e)
		{
			if (menualwaystop.IsChecked)
				Topmost = true;
			else
				Topmost = false;
		}
	}
}
