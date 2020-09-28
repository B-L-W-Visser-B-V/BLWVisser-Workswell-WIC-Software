/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for CameraTab.xaml
    /// </summary>
    public partial class InfoTab : UserControl
    {
        public InfoTab()
        {
            InitializeComponent();
            return;
        }

        //Website Button

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blwvisser.nl");
        }

        //Ping function

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Ping p = new Ping();
            PingReply r;
            string s;
            s = TextBox1.Text;
            r = p.Send(s);

            if (r.Status == IPStatus.Success)
            {
                string Resultaat = s.ToString() + " is bereikbaar! ";
                MessageBox.Show(Resultaat, "Online!");
            }
            if (r.Status == IPStatus.TimedOut)
            {
                string Resultaat = s.ToString() + " is onbereikbaar! ";
                MessageBox.Show(Resultaat, "Offline!");
            }
        }

        //CMD

        private void Button_Click2(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Windows\system32\cmd.exe";
            Process.Start(start);
        }

        //IP Scanner

        private void Button_Click3(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Program Files (x86)\Advanced IP Scanner\advanced_ip_scanner.exe";
            Process.Start(start);
        }

        //Teamviewer

        private void Button_Click4(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\blwvisser\TeamViewerQS.exe";
            Process.Start(start);
        }

        //Taakbeheer

        private void Button_Click5(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Windows\system32\Taskmgr.exe";
            Process.Start(start);
        }

        //Afsluiten

        private void Button_Click6(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        //Restart

        private void Button_Click7(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0");
        }

        //MsInfo32

        private void Button_Click8(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Windows\system32\msinfo32.exe";
            Process.Start(start);
        }

        //webserver

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            string ip = TextBox2.Text;
            System.Diagnostics.Process.Start("http://" + ip);
        }
    }
}
