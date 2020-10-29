/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using lucashuls.blwv.WIC.Properties;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for CameraTab.xaml
    /// </summary>
    public partial class CameraTab : System.Windows.Controls.UserControl
    {

        public CameraTab()
        {
            InitializeComponent();
            tempalarmstatus.Fill = new SolidColorBrush(Colors.Red);
            camerastatus.Fill = new SolidColorBrush(Colors.Red);
            alarmemail.Text = Settings.Default.alarmemail;
        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blwvisser.nl");
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure?.", "Exit?", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                System.Environment.Exit(0);
            }
        }

        public async void TempAlarm()
        {
            while (alarmswitch.IsChecked == true)
            {
                Settings.Default.alarmemail = alarmemail.Text;
                Settings.Default.Save();
                string tempstringRaw = maxtemptxt.Text; //String with °C
                string tempstringC = tempstringRaw.Replace("°C", ""); //String without °C
                decimal tempdecimal = Math.Round(Convert.ToDecimal(tempstringC), 0); //String to decimal + no numbers after comma

                decimal alarmmaxtempdecimal = Convert.ToDecimal(alarmmaxtemp.Text); //String to decimal

                await Task.Delay(1);

                if (tempdecimal > alarmmaxtempdecimal)
                {
                    try
                    {
                        MailMessage mail = new MailMessage()
                        {
                            From = new MailAddress("blwvisser@gmail.com"),
                            Subject = "Temperatuur alarm! | BLW-Visser",
                            Body = "Temperatuur Gemeten: " + tempstringRaw + "\n" + "Screenshot: "
                        };

                        mail.To.Add(new MailAddress(alarmemail.Text));

                        SmtpClient client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = new NetworkCredential("blwvisser@gmail.com", "blwv2020")
                        };
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error in sending email: " + ex.Message);
                        return;
                    }
                    await Task.Delay(5000);
                }
            }
        }

        private void alarmswitch_Checked(object sender, RoutedEventArgs e)
        {
            alarmmaxtemp.IsEnabled = false;
            TempAlarm();
            tempalarmstatus.Fill = new SolidColorBrush(Colors.Green);
        }

        private void alarmswitch_Unchecked(object sender, RoutedEventArgs e)
        {
            alarmmaxtemp.IsEnabled = true;
            TempAlarm();
            tempalarmstatus.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            camerastatus.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            camerastatus.Fill = new SolidColorBrush(Colors.Red);
        }
    }
}