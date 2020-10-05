using System;
using System.ComponentModel;

namespace WIC_SDK_Sample.ViewModel
{
    //Main application ViewModel
    public class ApplicationViewModel : INotifyPropertyChanged
    {

        //This is the path that is used, when SDK looks for license files. 
        public string LicenseFilesFolder => Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);

        private readonly CameraCenterViewModel cameraCenterVM = new CameraCenterViewModel();
        public CameraCenterViewModel CameraCenterVM => cameraCenterVM;

        private readonly ThermalImageViewModel thermalImageVM = new ThermalImageViewModel();
        public ThermalImageViewModel ThermalImageVM => thermalImageVM;

        private readonly ThermalSequencePlayerViewModel thermalSequencePlayerVM = new ThermalSequencePlayerViewModel();
        public ThermalSequencePlayerViewModel ThermalSequencePlayerVM => thermalSequencePlayerVM;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
