/*
 * Lucas Huls © 2020
 * lucashuls.nl
 */
using System;
using System.Diagnostics;
using System.Windows;


namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Check if some of the cameras aren't connected. Closing application without disconnect would leave them error state.
            //foreach (var Camera in FoundCamerasListBox.ItemsSource)
            //{
            //    if ((Camera as CameraViewModel).IsConnected)
            //    {
            //        (Camera as CameraViewModel).DisconnectCamera();
            //    }
            //}

            Process.GetCurrentProcess().Kill();
        }

        private void InfoTab_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
