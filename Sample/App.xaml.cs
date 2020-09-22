using System;
using System.Collections.Generic;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WIC_SDK_Sample.ViewModel;


namespace WIC_SDK_Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static string ApplicationName = "WIC_SDK";

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throw new ApplicationException("Application exception", e.Exception.InnerException);
        }

    }
}
