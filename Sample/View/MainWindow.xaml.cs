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
using WIC_SDK_Sample.ViewModel;
using WIC_SDK;
using System.Diagnostics;
using System.Windows.Forms;


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
