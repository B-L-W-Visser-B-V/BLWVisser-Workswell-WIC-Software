using System;
using System.Diagnostics;
using System.Windows;
using WIC_SDK_Sample.View;

namespace WIC_SDK_Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ApplicationName = "BLWVisser SAFETIS Workswell";

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throw new ApplicationException("Application exception", e.Exception.InnerException);
        }

    }
}
