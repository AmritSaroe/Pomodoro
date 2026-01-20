using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ZenFocus
{
    public partial class MainWindow : Window
    {
        // ---------------------------------------------------------
        // THE FIX: This is the "Ignition Key" the compiler was looking for.
        // It tells the app to create a new application instance and open this window.
        // ---------------------------------------------------------
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new MainWindow());
        }

        // ---------------------------------------------------------
        // APP LOGIC
        // ---------------------------------------------------------
        private DispatcherTimer _timer;
        private int _timeLeft;
        private const int WorkTime = 25 * 60;
        private bool _isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            _timeLeft = WorkTime;
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            // Allow dragging the window by clicking anywhere
            this.MouseLeftButtonDown += (s, e) => DragMove();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft--;
                UpdateUI();
            }
            else
            {
                _timer.Stop();
                _isRunning = false;
                BtnToggle.Content = "START";
                TimerText.Foreground = System.Windows.Media.Brushes.LightGreen;
                MessageBox.Show("Focus session complete!", "ZenFocus");
            }
        }

        private void ToggleTimer_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _timer.Stop();
                BtnToggle.Content = "RESUME";
            }
            else
            {
                _timer.Start();
                BtnToggle.Content = "PAUSE";
                TimerText.Foreground = System.Windows.Media.Brushes.White;
            }
            _isRunning = !_isRunning;
        }

        private void ResetTimer_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _isRunning = false;
            _timeLeft = WorkTime;
            BtnToggle.Content = "START";
            TimerText.Foreground = System.Windows.Media.Brushes.White;
            UpdateUI();
        }

        private void UpdateUI()
        {
            TimeSpan time = TimeSpan.FromSeconds(_timeLeft);
            TimerText.Text = time.ToString(@"mm\:ss");
            ProgressBar.Value = (double)_timeLeft / WorkTime * 100;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
