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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blwvisser.nl");
        }
    }
}
