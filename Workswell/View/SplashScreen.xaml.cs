/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using PvDotNet;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            Loadprogressbar();
        }

        private void Loadprogressbar()
        {
            pbLoading.Value = 0;
            pbLoading.Maximum = 100;

            Duration duration = new Duration(TimeSpan.FromSeconds(30));
            DoubleAnimation dblanim = new DoubleAnimation(3000.0, duration);
            pbLoading.ValueChanged += (s, e) =>
            {
                if (pbLoading.Value == 100)
                {
                    MainWindow MainWindow = new MainWindow();
                    MainWindow.Show();
                    this.Hide();
                }
            };
            pbLoading.BeginAnimation(ProgressBar.ValueProperty, dblanim);
        }
    }
}
