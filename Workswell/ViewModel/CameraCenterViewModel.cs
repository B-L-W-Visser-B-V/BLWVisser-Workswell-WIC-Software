using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using WIC_SDK;
using WIC_SDK_Sample.Utils;

namespace WIC_SDK_Sample.ViewModel
{
    /// <summary>
    /// Class that contains ViewModels
    /// </summary>
    public class CameraCenterViewModel : INotifyPropertyChanged
    {

        private readonly CameraCenter cameraCenter;

        public CameraCenterViewModel()
        {
            string pathToLicences = @"C:\blwvisser";

            if (pathToLicences != "")
            {
                cameraCenter = new CameraCenter(pathToLicences);
            }
            else
            {
                System.Windows.MessageBox.Show("No camera licences found.\n", "Error");
            }

            cameraCenter.CollectionChanged += cameraCenter_CollectionChanged;

            if (cameraCenter.Cameras.Count != 0)
            {
                foreach (Camera cam in cameraCenter.Cameras)
                {
                    FoundCameras.Add(new CameraViewModel(cam, this));
                }
            }
        }

        // Update FoundCameras collection after change in cameraCenter.Cameras occurred
        private void cameraCenter_CollectionChanged(object sender, EventArgs e)
        {
            int count = cameraCenter.Cameras.Count;
            if (count > FoundCameras.Count)
            {
                //New camera was found
                App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    Camera addedCam = cameraCenter.Cameras.Last();
                    FoundCameras.Add(new CameraViewModel(addedCam, this));
                }));
            }
            else
            {
                //One of the cameras was removed
                List<int> removedIndexes = new List<int>();
                foreach (CameraViewModel Cam in FoundCameras)
                {
                    if (cameraCenter.Cameras.Where(x => x.SerialNumber == Cam.SerialNumber).Count() == 0)
                    {
                        removedIndexes.Insert(0, FoundCameras.IndexOf(Cam));
                    }
                }

                foreach (int index in removedIndexes)
                {
                    FoundCameras.RemoveAt(index);
                }

            }
        }

        // All available CameraViewModels which were found and are listed in ListBox in WPF
        private ObservableCollection<CameraViewModel> foundCameras = new ObservableCollection<CameraViewModel>();
        public ObservableCollection<CameraViewModel> FoundCameras
        {
            get => foundCameras;
            set
            {
                foundCameras = value;
                RaisePropertyChanged("FoundCameras");
            }
        }

        // CameraViewModel that is selected in ListBox at WPF
        private CameraViewModel selectedCamera;
        public CameraViewModel SelectedCamera
        {
            get => selectedCamera;
            set
            {
                selectedCamera = value;
                RaisePropertyChanged("SelectedCamera");
            }
        }

        // Boolean which is used to prevent execute the RefreshCommandExecute before terminates previous execute
        public bool IsRefreshing => cameraCenter.IsRefreshing;
        public bool isRefreshing = false;

        // Command for clearing cameras in observable collection and again finds the cameras subsequently added to Cameras collection
        public ICommand RefreshCommand => new RelayCommand(RefreshCommandExecute, CanRefreshCommandExecute);
        private bool CanRefreshCommandExecute()
        {
            foreach (CameraViewModel Cam in FoundCameras)
            {
                if (Cam.IsConnected)
                {
                    return false;
                }
            }
            return (!IsRefreshing);
        }
        private void RefreshCommandExecute()
        {
            cameraCenter.ManualRefresh();
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
