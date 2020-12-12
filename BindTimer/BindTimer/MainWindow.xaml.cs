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
		readonly int resisttime;
		int now;
		readonly Settings set = Settings.Instance;
		Hotkey hotkeyhelper;
		Timer timer;
		bool istimerstarted = false;

		public MainWindow()
		{
			InitializeComponent();
			hotkeyhelper = new Hotkey(this, StartTimer);
			resisttime = 90 + set.Addtime;
			now = resisttime;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			hotkeyhelper.Register();
			txthelp.Text = string.Format(txthelp.Text, set.HotkeyByString);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			hotkeyhelper.Unregister();
		}


		private void StartTimer()
		{
			if (istimerstarted)
				timer.Dispose();

			now = resisttime;
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
