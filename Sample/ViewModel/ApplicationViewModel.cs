using System;
using System.ComponentModel;

namespace WIC_SDK_Sample.ViewModel
{
    //Main application ViewModel
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        
        //This is the path that is used, when SDK looks for license files. 
        public string LicenseFilesFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            }
        }

        private CameraCenterViewModel cameraCenterVM = new CameraCenterViewModel();
        public CameraCenterViewModel CameraCenterVM
        {
            get
            {
                return cameraCenterVM;
            }

        }
       
        private ThermalImageViewModel thermalImageVM = new ThermalImageViewModel();
        public ThermalImageViewModel ThermalImageVM
        {
            get
            {
                return thermalImageVM;
            }
        }

        private ThermalSequencePlayerViewModel thermalSequencePlayerVM = new ThermalSequencePlayerViewModel();
        public ThermalSequencePlayerViewModel ThermalSequencePlayerVM
        {
            get
            {
                return thermalSequencePlayerVM;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
