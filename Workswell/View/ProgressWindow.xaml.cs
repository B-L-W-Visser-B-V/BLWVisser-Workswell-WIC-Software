using System;
using System.Windows;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public static ProgressWindow Instance = new ProgressWindow();

        public static void OpenWindow(string info, string title)
        {
            Instance.InfoLabel.Text = info;
            Instance.Title = title;
            Application.Current.MainWindow.IsEnabled = true;
            Instance.Show();
            Instance.Focus();
        }

        public static void CloseWindow()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                Instance.Visibility = Visibility.Hidden;
                Application.Current.MainWindow.IsEnabled = true;
            }));
        }
    }
}
