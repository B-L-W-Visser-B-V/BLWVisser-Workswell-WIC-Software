using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
