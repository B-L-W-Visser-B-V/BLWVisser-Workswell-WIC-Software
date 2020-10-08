/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using System;
using System.Media;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Forms;

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

        private readonly MainWindow MW = new MainWindow();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text == "admin")
            {
                if (password.Password == "123")
                {
                    correctlogin();
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
            System.Windows.Forms.MessageBox.Show("Gegevens Incorrect! Probeer opnieuw.", "Fout!");
        }

        private void correctlogin()
        {
            MW.Show();
            Hide();
        }

        private void password_KeyPress(object IChannelSender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                if (username.Text == "admin")
                {
                    if (password.Password == "123")
                    {
                        correctlogin();
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
        }
    }
}
