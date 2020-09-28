using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WIC_SDK;
using WIC_SDK_Sample.Utils;

namespace WIC_SDK_Sample.ViewModel
{
    public class ThermalImageViewModel : INotifyPropertyChanged
    {
        internal ApplicationViewModel ApplicationViewModel
        {
            get
            {
                return System.Windows.Application.Current.Resources["MainViewModel"] as ApplicationViewModel;
            }
        }

        // Instance of the Thermogram
        private ThermalImage thermalImage;

        // BitmapSource of the loaded thermogram
        public BitmapSource ThermalImageSource
        {
            get
            {
                if (thermalImage == null) return null;
                return thermalImage.ThermoImageSource;
            }
            set
            {
                thermalImageSource = value;
                RaisePropertyChanged("ThermalImage");
            }
        }
        private BitmapSource thermalImageSource;

        // Collection of available palettes
        private ObservableCollection<string> availablePalettes = new ObservableCollection<string>() {
            ThermalImage.AvailablePalettes.BlackRed.ToString(),
            ThermalImage.AvailablePalettes.BlueRed.ToString(),
            ThermalImage.AvailablePalettes.BWRGB.ToString(),
            ThermalImage.AvailablePalettes.Fire.ToString(),
            ThermalImage.AvailablePalettes.Gradient.ToString(),
            ThermalImage.AvailablePalettes.Gray.ToString(),
            ThermalImage.AvailablePalettes.Sepia.ToString(),
            ThermalImage.AvailablePalettes.Steps.ToString(),
            ThermalImage.AvailablePalettes.Temperature.ToString(),
            ThermalImage.AvailablePalettes.WBRGB.ToString()};
        public ObservableCollection<string> AvailablePalettes
        {
            get
            {
                return availablePalettes;
            }
        }

        // Selected palette
        public string SelectedPalette
        {
            get
            {
                if (thermalImage == null) return ThermalImage.AvailablePalettes.Sepia.ToString();
                return thermalImage.SelectedPalette.ToString();
            }
            set
            {
                switch (value)
                {
                    case "BlackRed":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.BlackRed;
                        break;
                    case "BlueRed":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.BlueRed;
                        break;
                    case "BWRGB":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.BWRGB;
                        break;
                    case "Fire":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Fire;
                        break;
                    case "Gradient":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Gradient;
                        break;
                    case "Gray":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Gray;
                        break;
                    case "Sepia":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Sepia;
                        break;
                    case "Steps":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Steps;
                        break;
                    case "Temperature":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.Temperature;
                        break;
                    case "WBRGB":
                        thermalImage.SelectedPalette = ThermalImage.AvailablePalettes.WBRGB;
                        break;
                    default:
                        break;
                }
                thermalImage.RegenerateThermalImage(true);
                RaisePropertyChanged("SelectedPalette");
                RaisePropertyChanged("ThermalImageSource");
            }
        }

        #region All settings of camera

        // Emmisivity settings
        public double? Emisivity
        {
            get
            {
                return thermalImage.ThermalParameters.Emissivity;
            }
            set
            {
                thermalImage.ThermalParameters.Emissivity = value;
                RaisePropertyChanged("Emisivity");
                RaiseImage();
            }
        }

        // Reflected temperature settings
        public double? ReflectedTemperature
        {
            get
            {
                return thermalImage.ThermalParameters.ReflectedTemperature;
            }
            set
            {
                thermalImage.ThermalParameters.ReflectedTemperature = value;
                RaisePropertyChanged("ReflectedTemperature");
                RaiseImage();
            }
        }

        // Atmospheric temperature settings
        public double? AtmosphericTemperature
        {
            get
            {
                return thermalImage.ThermalParameters.AtmosphericTemperature;
            }
            set
            {
                thermalImage.ThermalParameters.AtmosphericTemperature = value;
                RaisePropertyChanged("AtmosphericTemperature");
                RaiseImage();
            }
        }

        // gets or sets the Atmospheric temperature.
        public double? ExternalOpticsTransmission
        {
            get
            {
                return thermalImage.ThermalParameters.ExternalOpticsTransmission;
            }
            set
            {
                thermalImage.ThermalParameters.ExternalOpticsTransmission = value;
                RaisePropertyChanged("ExternalOpticsTransmission");
                RaiseImage();
            }
        }

        // Humidity settings
        public double? Humidity
        {
            get
            {
                return thermalImage.ThermalParameters.RelativeHumidity;
            }
            set
            {
                thermalImage.ThermalParameters.RelativeHumidity = value;
                RaisePropertyChanged("Humidity");
                RaiseImage();
            }
        }

        // Distance settings
        public double? Distance
        {
            get
            {
                return thermalImage.ThermalParameters.Distance;
            }
            set
            {
                thermalImage.ThermalParameters.Distance = value;
                RaisePropertyChanged("Distance");
                RaiseImage();
            }
        }

        // Maximum temperature
        public string MaxTemperature
        {
            get
            {
                return thermalImage.MaxTemperatureValue.ToString("f2");
            }
        }

        // Minimum temperature
        public string MinTemperature
        {
            get
            {
                return thermalImage.MinTemperatureValue.ToString("f2");
            }
        }

        // Manufacturer info
        public string ManufacturerInfo
        {
            get
            {
                return thermalImage.Manufacturer;
            }
        }

        // Model name
        public string ModelName
        {
            get
            {
                return thermalImage.ModelName;
            }
        }

        // Serial number
        public string SerialNumber
        {
            get
            {
                return thermalImage.SerialNumber;
            }
        }

        // User-defined device name
        public string UserDefinedName
        {
            get
            {
                return thermalImage.Name;
            }
        }

        #endregion

        #region Raise Properties

        // Raise parameters and settings
        private void RaiseAllSettings()
        {
            RaisePropertyChanged("Emisivity");
            RaisePropertyChanged("ReflectedTemperature");
            RaisePropertyChanged("AtmosphericTemperature");
            RaisePropertyChanged("Humidity");
            RaisePropertyChanged("Distance");
            RaisePropertyChanged("ExternalOpticsTransmission");
            RaisePropertyChanged("SelectedPalette");
            RaisePropertyChanged("ManufacturerInfo");
            RaisePropertyChanged("ModelName");
            RaisePropertyChanged("SerialNumber");
            RaisePropertyChanged("UserDefinedName");
        }

        // Raise proper variables after image change
        public void RaiseImage()
        {
            if (thermalImage != null)
            {
                thermalImage.RegenerateThermalImage(true);
            }
            RaisePropertyChanged("ThermalImageSource");
            RaisePropertyChanged("MaxTemperature");
            RaisePropertyChanged("MinTemperature");
        }

        #endregion

        // Open command for binding from view. Open the selected thermal image and load parameters.s
        public ICommand OpenImageCommand { get { return new RelayCommand(OpenImageCommandExecute, CanOpenImageCommandExecute); } }
        private bool CanOpenImageCommandExecute()
        {
            return thermalImage == null;
        }
        private void OpenImageCommandExecute()
        {
            string pathToFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            string pathToLicences = ApplicationViewModel.LicenseFilesFolder;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = dlg.FileName;
                    try
                    {
                        thermalImage = new ThermalImage(pathToFile, pathToLicences);
                    }
                    catch (Exception e)
                    {
                        string messageBoxText = e.Message;
                        string caption = "Warning";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
                    }
                    RaiseAllSettings();
                    RaiseImage();
                }
            }
        }

        // Close command for binding from view. Close the selected thermal image.
        public ICommand CloseImageCommand { get { return new RelayCommand(CloseImageCommandExecute, CanCloseImageCommandExecute); } }
        private bool CanCloseImageCommandExecute()
        {
            return thermalImage != null;
        }
        private void CloseImageCommandExecute()
        {
            thermalImage.UnloadImage();
            thermalImage = null;
            RaiseAllSettings();
            RaiseImage();
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
