using System.Windows;
using System.Windows.Controls;

namespace WIC_SDK_Sample.View
{
    /// <summary>
    /// Interaction logic for CameraTab.xaml
    /// </summary>
    public partial class CameraTab : UserControl
    {
        public CameraTab()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blwvisser.nl");
        }
    }
}
