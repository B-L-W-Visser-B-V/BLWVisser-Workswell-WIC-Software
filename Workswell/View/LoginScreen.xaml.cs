/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using System.Media;
using System.Windows;

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
                    MW.Show();
                    Hide();
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
