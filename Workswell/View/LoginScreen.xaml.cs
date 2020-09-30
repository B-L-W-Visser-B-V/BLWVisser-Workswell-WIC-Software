/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();

            password.PasswordChar = '*';
        }
        MainWindow MW = new MainWindow();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text == "admin")
            {
                if (password.Password == "123")
                {
                    MW.Show();
                    this.Hide();
                }
                else
                {
                    incorrectlogin();
                }
            }
            else
            {
                incorrectlogin();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void incorrectlogin()
        {
            SystemSounds.Beep.Play();
            MessageBox.Show("Gegevens Incorrect! Probeer opnieuw.", "Fout!");
        }
    }
}
