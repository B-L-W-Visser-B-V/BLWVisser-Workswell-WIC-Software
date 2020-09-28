using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WIC_SDK;
using WIC_SDK_Sample.Utils;
using WIC_SDK_Sample.View;

namespace WIC_SDK_Sample.ViewModel
{

    // Class for camera with settings and service
    public class CameraViewModel : INotifyPropertyChanged
    {
        private Camera camera;
        private CameraCenterViewModel cameraCenterVM;

        private int nFrame = 0;

        private System.Timers.Timer timer = new System.Timers.Timer();


        private void ComputeFps(object sender, System.Timers.ElapsedEventArgs e)
        {
            Fps = nFrame;
            nFrame = 0;
        }

        private int fps;

        public int Fps
        {
            get { return fps; }
            set
            {
                fps = value;
                RaisePropertyChanged("Fps");
            }
        }

        // Constructor for CameraViewModel where is needed DeviceInfo and CameraCenterViewModel
        public CameraViewModel(Camera camera/*PvDeviceInfo deviceInfo*/, CameraCenterViewModel cameraCenterVM)
        {
            this.camera = camera;
            this.cameraCenterVM = cameraCenterVM;

            camera.AcquisitionStoped += camera_AcquisitionStoped;
            camera.AcquisitionStarted += camera_AcquisitionStarted;
            camera.Disconnected += camera_Disconnected;
            camera.Connected += camera_Connected;

            camera.OnNewFrame += camera_OnNewFrame;

            foreach (var lens in camera.AvailableCalibratedLenses)
            {
                AvailableCalibratedLenses.Add(lens);
            }
            timer.Interval = (1000); // 1 sec
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(ComputeFps);
            timer.Start();
        }

        void camera_OnNewFrame(object sender, EventArgs e)
        {
            nFrame++;
            RaiseImage();
        }


        #region Settings & 

        //Manufacturer info
        public string ManufacturerInfo
        {
            get
            {
                return camera.ManufacturerInfo;
            }
        }

        //Model name
        public string ModelName
        {
            get
            {
                return camera.ModelName;
            }
        }

        // Gets the image timestamp acquired from video converter (Pleora - GevTimestampValue )
        public long ImageTimestamp
        {
            get
            {
                return camera.ImageTimestamp;
            }
        }

        // Serial number
        public string SerialNumber
        {
            get
            {
                return camera.SerialNumber;
            }
        }

        // User-defined device name
        public string UserDefinedName
        {
            get
            {
                return camera.UserDefinedName;
            }
        }

        // Device vendor
        public string VendorName
        {
            get
            {
                return camera.VendorName;
            }
        }

        // Device version
        public string Version
        {
            get
            {
                return camera.Version;
            }
        }

        // Device MACaddress (if device is not GEV, then this property is '-')
        public string MACaddress
        {
            get
            {
                return camera.MACaddress;
            }
        }

        // Device GUID (if device is not U3V, then this property is '-')
        public string GUID
        {
            get
            {
                return camera.GUID;
            }
        }

        // Camera is connected
        public bool IsConnected
        {
            get
            {
                return camera.IsConnected;
            }
        }

        // Camera is connected and acquiring data
        public bool IsAcquiring
        {
            get
            {
                return camera.IsAcquiring;
            }
        }

        // Camera wasnt find in last refresh.
        public bool IsMissing
        {
            get
            {
                return camera.IsMissing;
            }
        }

        // Status of camera
        public string Status
        {
            get
            {
                return camera.Status;
            }
        }

        public bool IsCalibrated
        {
            get
            {
                return camera.IsCalibrated;
            }
        }

        public bool IsRecording
        {
            get
            {
                return camera.IsRecordingSequence;
            }
        }

        #region All settings of camera

        // Emmisivity settings
        public float? Emisivity
        {
            get
            {
                return camera.Emissivity;
            }
            set
            {
                camera.Emissivity = value;
                RaisePropertyChanged("Emisivity");
            }
        }

        // Reflected temperature settings
        public float? ReflectedTemperature
        {
            get
            {
                return camera.ReflectedTemperature;
            }
            set
            {
                camera.ReflectedTemperature = value;
                RaisePropertyChanged("ReflectedTemperature");
            }
        }

        // Atmospheric temperature settings
        public float? AtmosphericTemperature
        {
            get
            {
                return camera.AtmosphericTemperature;
            }
            set
            {
                camera.AtmosphericTemperature = value;
            }
        }

        /// <summary>
        /// gets or sets the Atmospheric temperature.
        /// </summary>
        public float? ExternalOpticsTransmission
        {
            get
            {
                return camera.ExternalOpticsTransmission;
            }
            set
            {
                camera.ExternalOpticsTransmission = value;
            }
        }

        // Humadity settings
        public float? Humidity
        {
            get
            {
                return camera.Humidity;
            }
            set
            {
                camera.Humidity = value;
            }
        }

        // Distance settings
        public float? Distance
        {
            get
            {
                return camera.Distance;
            }
            set
            {
                camera.Distance = value;
            }
        }

        // Maximum and minimum temperature
        public string MaxTemperature
        {
            get
            {
                return camera.MaxTemperature;
            }
        }

        public string MinTemperature
        {
            get
            {
                return camera.MinTemperature;
            }
        }

        // Maximum and minimum temperature
        public string MaxSignal
        {
            get
            {
                return camera.MaxSignal;
            }
        }

        public string MinSignal
        {
            get
            {
                return camera.MinSignal;
            }
        }

        private void RaiseAllSettings()
        {
            RaisePropertyChanged("Emisivity");
            RaisePropertyChanged("ReflectedTemperature");
            RaisePropertyChanged("AtmosphericTemperature");
            RaisePropertyChanged("Humidity");
            RaisePropertyChanged("Distance");
            RaisePropertyChanged("ExternalOpticsTransmission");
            RaisePropertyChanged("SelectedGainMode");
            RaisePropertyChanged("SelectedPalette");
        }
        #endregion

        #endregion

        //Available temperature ranges
        public ObservableCollection<TemperatureRangeViewModel> TempRanges
        {
            get
            {
                ObservableCollection<TemperatureRangeViewModel> tempRangesTmp = new ObservableCollection<TemperatureRangeViewModel>();
                int index = 0;
                foreach (var range in camera.GetWicTempRanges())
                {
                    tempRangesTmp.Add(new TemperatureRangeViewModel(new TemperatureRange((int)range.RangeMinC, (int)range.RangeMaxC, index, range.Type)));
                    index++;
                }

                tempRanges = tempRangesTmp;
                return tempRanges;
            }
        }
        private ObservableCollection<TemperatureRangeViewModel> tempRanges = new ObservableCollection<TemperatureRangeViewModel>();

        public TemperatureRangeViewModel SelectedTempRange
        {
            get
            {
                if (tempRanges.Count - 1 >= camera.SelectedTempRangeIndex)
                {
                    return tempRanges.ElementAt(camera.SelectedTempRangeIndex);
                }
                return tempRanges.First();
            }
            set
            {
                if (value != null && tempRanges.ElementAt(camera.SelectedTempRangeIndex) != value)
                {
                    ProgressWindow.OpenWindow("Temperature range", "Changing range..");
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        camera.SelectedTempRangeIndex = value.Index;
                        Thread.Sleep(3000);
                        App.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            ProgressWindow.CloseWindow();
                            RaisePropertyChanged("SelectedTempRange");
                        }));

                    }, null);
                }
            }
        }

        // Bitmap source for Image
        public BitmapSource ThermalImage
        {
            get
            {
                if (!IsAcquiring) return null;
                return camera.ThermoImageSource;
            }
            set
            {
                thermalImage = value;
                RaisePropertyChanged("ThermalImage");
            }
        }
        private BitmapSource thermalImage;

        // Bitmap source for Image2
        public BitmapSource ThermalImage2
        {
            get
            {
                if (!IsAcquiring) return null;
                return camera.ThermoImageSource;
            }
            set
            {
                thermalImage2 = value;
                RaisePropertyChanged("ThermalImage2");
            }
        }
        private BitmapSource thermalImage2;

        private ObservableCollection<string> availablePalettes = new ObservableCollection<string>() { Camera.AvailablePalettes.BlackRed.ToString(),
            Camera.AvailablePalettes.BlueRed.ToString(),
            Camera.AvailablePalettes.BWRGB.ToString(),
            Camera.AvailablePalettes.Fire.ToString(),
            Camera.AvailablePalettes.Gradient.ToString(),
            Camera.AvailablePalettes.Gray.ToString(),
            Camera.AvailablePalettes.Sepia.ToString(),
            Camera.AvailablePalettes.Steps.ToString(),
            Camera.AvailablePalettes.Temperature.ToString(),
            Camera.AvailablePalettes.WBRGB.ToString()};

        public ObservableCollection<string> AvailablePalettes
        {
            get
            {
                return availablePalettes;
            }
        }

        //All available generation modes
        private ObservableCollection<string> availableModes = new ObservableCollection<string>() {
            Camera.GenerationModes.Full.ToString(),
            Camera.GenerationModes.Signal.ToString(),
            Camera.GenerationModes.SignalBitmap.ToString()};
        public ObservableCollection<string> AvailableModes
        {
            get
            {
                return availableModes;
            }
        }

        //Selected generation mode
        public string SelectedMode
        {
            get
            {
                if (camera == null) return Camera.GenerationModes.Full.ToString();
                return camera.SelectedMode.ToString();
            }
            set
            {
                switch (value)
                {
                    case "Full":
                        camera.SelectedMode = Camera.GenerationModes.Full;
                        break;
                    case "Signal":
                        camera.SelectedMode = Camera.GenerationModes.Signal;
                        break;
                    case "SignalBitmap":
                        camera.SelectedMode = Camera.GenerationModes.SignalBitmap;
                        break;
                    default:
                        camera.SelectedMode = Camera.GenerationModes.Full;
                        break;
                }
                RaiseImage();

            }
        }

        public string SelectedPalette
        {
            get
            {
                if (camera == null) return Camera.AvailablePalettes.Sepia.ToString();
                return camera.SelectedPalette.ToString();
            }
            set
            {
                switch (value)
                {
                    case "BlackRed":
                        camera.SelectedPalette = Camera.AvailablePalettes.BlackRed;
                        break;
                    case "BlueRed":
                        camera.SelectedPalette = Camera.AvailablePalettes.BlueRed;
                        break;
                    case "BWRGB":
                        camera.SelectedPalette = Camera.AvailablePalettes.BWRGB;
                        break;
                    case "Fire":
                        camera.SelectedPalette = Camera.AvailablePalettes.Fire;
                        break;
                    case "Gradient":
                        camera.SelectedPalette = Camera.AvailablePalettes.Gradient;
                        break;
                    case "Gray":
                        camera.SelectedPalette = Camera.AvailablePalettes.Gray;
                        break;
                    case "Sepia":
                        camera.SelectedPalette = Camera.AvailablePalettes.Sepia;
                        break;
                    case "Steps":
                        camera.SelectedPalette = Camera.AvailablePalettes.Steps;
                        break;
                    case "Temperature":
                        camera.SelectedPalette = Camera.AvailablePalettes.Temperature;
                        break;
                    case "WBRGB":
                        camera.SelectedPalette = Camera.AvailablePalettes.WBRGB;
                        break;
                    default:
                        break;
                }
                RaisePropertyChanged("SelectedPalette");
            }
        }

        public string SelectedLens
        {
            get
            {
                return camera.SelectedLens;
            }
            set
            {
                ProgressWindow.OpenWindow("Lens change", "Changing calibration data for lens..");

                System.Threading.ThreadPool.QueueUserWorkItem(delegate
                {
                    camera.SelectedLens = value;
                    ProgressWindow.CloseWindow();
                    RaisePropertyChanged("SelectedLens");
                }, null);
            }
        }

        public bool IsAnyCalibratedLens
        {
            get
            {
                return camera.IsAnyCalibratedLens;
            }
        }


        public ObservableCollection<string> AvailableCalibratedLenses
        {
            get
            {
                return availableCalibratedLenses;
            }
            set
            {
                availableCalibratedLenses = value;
                RaisePropertyChanged("AvailableCalibratedLenses");
            }
        }
        private ObservableCollection<string> availableCalibratedLenses = new ObservableCollection<string>();

        public void RaiseImage()
        {
            RaisePropertyChanged("ThermalImage");
            RaisePropertyChanged("MaxTemperature");
            RaisePropertyChanged("MinTemperature");
            RaisePropertyChanged("MaxSignal");
            RaisePropertyChanged("MinSignal");
        }

        void camera_AcquisitionStoped(object sender, EventArgs e)
        {
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("Status");
        }

        void camera_AcquisitionStarted(object sender, EventArgs e)
        {
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("Status");
            RaisePropertyChanged("SelectedPalette");
        }

        void camera_Disconnected(object sender, EventArgs e)
        {
            RaisePropertyChanged("IsConnected");
            RaisePropertyChanged("Status");
        }

        void camera_Connected(object sender, EventArgs e)
        {
            RaiseAllSettings();
            RaisePropertyChanged("IsConnected");
            RaisePropertyChanged("Status");
            RaisePropertyChanged("SelectedLens");
            RaisePropertyChanged("TempRanges");
            RaisePropertyChanged("SelectedPalette");
            RaisePropertyChanged("SelectedTempRange");
        }


        // Connect command for binding from view
        // Connect the selected camera and load gain modes and framerates modes
        public ICommand ConnectCommand { get { return new RelayCommand(ConnectCommandExecute, CanConnectCommandExecute); } }
        private bool CanConnectCommandExecute()
        {
            return !IsConnected && !IsMissing;
        }
        private void ConnectCommandExecute()
        {
            camera.Connect();
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("IsConnected");
            RaisePropertyChanged("Status");
            RaisePropertyChanged("SelectedTempRange");
            RaisePropertyChanged("SelectedPalette");
        }

        // Disconnect command for binding from view
        // Disconnect the selected camera
        public ICommand DisconnectCommand { get { return new RelayCommand(DisconnectCommandExecute, CanDisconnectCommandExecute); } }
        private bool CanDisconnectCommandExecute()
        {
            return IsConnected && !IsAcquiring;
        }
        private void DisconnectCommandExecute()
        {
            if (IsRecording)
            {
                StopRecordingExecute();
            }
            camera.Disconnect();
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("IsConnected");
            RaisePropertyChanged("Status");
        }

        public void DisconnectCamera()
        {
            camera.Disconnect();
        }

        // Start acquisition command for binding from view
        // Start acquisition of the selected camera
        public ICommand StartAcquisitionCommand { get { return new RelayCommand(StartAcquisitionCommandExecute, CanStartAcquisitionCommandExecute); } }
        private bool CanStartAcquisitionCommandExecute()
        {
            return !IsAcquiring && IsConnected && !IsMissing;
        }
        private void StartAcquisitionCommandExecute()
        {
            try
            {
                camera.StartAcquisition();
            }
            catch { }
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("ThermoImageSource");
            RaisePropertyChanged("Status");
        }


        // Stop acquisition command for binding from view
        // Stop acquisition of the selected camera
        public ICommand StopAcquisitionCommand { get { return new RelayCommand(StopAcquisitionCommandExecute, CanStopAcquisitionCommandExecute); } }
        private bool CanStopAcquisitionCommandExecute()
        {
            return IsAcquiring && !IsMissing;
        }
        private void StopAcquisitionCommandExecute()
        {
            if (IsRecording)
            {
                StopRecordingExecute();
            }
            camera.StopAcquisition();
            RaisePropertyChanged("IsAcquiring");
            RaisePropertyChanged("Status");
        }

        // Do FFC Correction
        public ICommand CalibrationCommand { get { return new RelayCommand(CalibrationCommandExecute, CanCalibrationCommandExecute); } }
        private bool CanCalibrationCommandExecute()
        {
            return IsAcquiring && !IsMissing;
        }
        private void CalibrationCommandExecute()
        {
            camera.DoNUC();
        }

        public ICommand SaveImageCommand { get { return new RelayCommand(SaveImageCommandExecute, CanSaveImageCommandExecute); } }
        private bool CanSaveImageCommandExecute()
        {
            return IsConnected && IsAcquiring && !camera.IsSavingImage && !camera.IsRecordingSequence && camera.SelectedMode != Camera.GenerationModes.Signal;
        }
        private void SaveImageCommandExecute()
        {
            string pathToFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            ApplicationViewModel ApplicationVM = System.Windows.Application.Current.Resources["MainViewModel"] as ApplicationViewModel;

            string filePath;
            string fullPath;
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select folder";
                dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    fullPath = filePath + "\\" + DateTime.Now.ToString("HH-mm-ss_dd-MM-yyyy") + ".jpeg";
                    camera.SaveThermalImage(fullPath);
                }
            }

        }

        public ICommand StartRecordingCommand { get { return new RelayCommand(StartRecordingExecute, CanStartRecordingExecute); } }
        private bool CanStartRecordingExecute()
        {
            return IsConnected && IsAcquiring && !camera.IsSavingImage && !camera.IsRecordingSequence && camera.SelectedMode != Camera.GenerationModes.Signal;
        }
        private void StartRecordingExecute()
        {
            string pathToFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            ApplicationViewModel ApplicationVM = System.Windows.Application.Current.Resources["MainViewModel"] as ApplicationViewModel;

            string filePath;
            string fullPath;
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select folder";
                dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (camera.CameraIsRadiometric)
                    {
                        filePath = dlg.SelectedPath;
                        fullPath = filePath + "\\" + DateTime.Now.ToString("HH-mm-ss_dd-MM-yyyy") + ".wseq";
                        camera.StartRecordingThermalSequence(fullPath);
                    }
                    else
                    {
                        filePath = dlg.SelectedPath;
                        fullPath = filePath + "\\" + DateTime.Now.ToString() + ".seq";
                        camera.StartRecordingThermalSequence(fullPath);
                    }
                    RaisePropertyChanged("IsRecording");
                }
            }

        }

        public ICommand StopRecordingCommand { get { return new RelayCommand(StopRecordingExecute, CanStopRecordingExecute); } }
        private bool CanStopRecordingExecute()
        {
            return IsConnected && IsAcquiring && camera.IsRecordingSequence;
        }
        private void StopRecordingExecute()
        {
            camera.StopRecordingThermalSequence();
            RaisePropertyChanged("IsRecording");
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
