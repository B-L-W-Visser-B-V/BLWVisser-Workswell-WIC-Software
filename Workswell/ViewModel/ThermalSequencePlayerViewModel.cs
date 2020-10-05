using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WIC_SDK;
using WIC_SDK_Sample.Utils;
using WIC_SDK_Sample.View;


namespace WIC_SDK_Sample.ViewModel
{
    public class ThermalSequencePlayerViewModel : INotifyPropertyChanged
    {
        internal ApplicationViewModel ApplicationViewModel => System.Windows.Application.Current.Resources["MainViewModel"] as ApplicationViewModel;

        private ThermalSequencePlayer thermalSequencePlayer;

        // Command for opening thermal sequence
        public ICommand OpenThermalSequenceCommand => new RelayCommand(OpenThermalSequenceCommandExecute, CanOpenThermalSequenceCommandExecute);
        private bool CanOpenThermalSequenceCommandExecute()
        {
            return thermalSequencePlayer == null;
        }
        private void OpenThermalSequenceCommandExecute()
        {
            string pathToFile = null;
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
                        thermalSequencePlayer = new ThermalSequencePlayer(pathToFile, pathToLicences);
                    }
                    catch (Exception e)
                    {
                        string messageBoxText = e.Message;
                        string caption = "Warning";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
                    }
                    thermalSequencePlayer.RecalculationCompleted += thermalSequencePlayer_RecalculationCompleted;
                    if (thermalSequencePlayer.IsRadiometricSequence && !thermalSequencePlayer.IsRadiometrisSequenceRecalculated)
                    {
                        ProgressWindow.OpenWindow("Recalculating sequence", "Recalculating...");
                    }

                    RaiseInit();
                    RaiseParameters();
                    RaiseImage();
                }
            }

        }

        // Command for closing thermal sequence
        public ICommand CloseThermalSequenceCommand => new RelayCommand(CloseThermalSequenceCommandExecute, CanCloseThermalSequenceCommandExecute);
        private bool CanCloseThermalSequenceCommandExecute()
        {
            return thermalSequencePlayer != null;
        }
        private void CloseThermalSequenceCommandExecute()
        {
            Loop = false;
            thermalSequencePlayer.UnloadSequence();
            thermalSequencePlayer = null;
        }

        // Command for getting next frame of thermal sequence
        public ICommand NextSequenceImageCommand => new RelayCommand(NextSequenceImageCommandExecute, CanNextSequenceImageCommandExecute);
        private bool CanNextSequenceImageCommandExecute()
        {
            return thermalSequencePlayer != null && !thermalSequencePlayer.IsPlayingSequence && ((thermalSequencePlayer.IsRadiometricSequence && thermalSequencePlayer.IsRadiometrisSequenceRecalculated) || !thermalSequencePlayer.IsRadiometricSequence);
        }
        private void NextSequenceImageCommandExecute()
        {
            thermalSequencePlayer.NextFrame();
            RaiseImage();
        }

        // Command for getting previous frame of thermal sequence
        public ICommand PrevSequenceImageCommand => new RelayCommand(PrevSequenceImageCommandExecute, CanPrevSequenceImageCommandExecute);
        private bool CanPrevSequenceImageCommandExecute()
        {
            return thermalSequencePlayer != null && !thermalSequencePlayer.IsPlayingSequence && ((thermalSequencePlayer.IsRadiometricSequence && thermalSequencePlayer.IsRadiometrisSequenceRecalculated) || !thermalSequencePlayer.IsRadiometricSequence);
        }
        private void PrevSequenceImageCommandExecute()
        {
            thermalSequencePlayer.PreviousFrame();
            RaiseImage();
        }

        // Command for playing thermal sequence
        public ICommand PlaySequenceImageCommand => new RelayCommand(PlaySequenceImageCommandExecute, CanPlaySequenceImageCommandExecute);
        private bool CanPlaySequenceImageCommandExecute()
        {
            return thermalSequencePlayer != null && !thermalSequencePlayer.IsPlayingSequence && ((thermalSequencePlayer.IsRadiometricSequence && thermalSequencePlayer.IsRadiometrisSequenceRecalculated) || !thermalSequencePlayer.IsRadiometricSequence);
        }
        private void PlaySequenceImageCommandExecute()
        {
            thermalSequencePlayer.ImageChanged += thermalSequencePlayer_ImageChanged;
            thermalSequencePlayer.Play();
            RaiseImage();
        }

        // Command for stopping thermal sequence
        public ICommand StopSequenceImageCommand => new RelayCommand(StopSequenceImageCommandExecute, CanStopSequenceImageCommandExecute);
        private bool CanStopSequenceImageCommandExecute()
        {
            return thermalSequencePlayer != null && thermalSequencePlayer.IsPlayingSequence;
        }
        private void StopSequenceImageCommandExecute()
        {
            thermalSequencePlayer.Stop();
            RaiseImage();
        }

        //Triggered when ImageChanged event occurs
        private void thermalSequencePlayer_ImageChanged(object sender, EventArgs e)
        {
            RaiseImage();
        }

        //Triggered when RecalculationCompleted event occurs
        private void thermalSequencePlayer_RecalculationCompleted(object sender, EventArgs e)
        {
            RecalculationCompleted = true;
            App.Current.Dispatcher.Invoke(() =>
            {
                ProgressWindow.CloseWindow();
            });
        }

        //All available palettes
        private readonly ObservableCollection<string> availablePalettes = new ObservableCollection<string>() {
            ThermalSequencePlayer.AvailablePalettes.BlackRed.ToString(),
            ThermalSequencePlayer.AvailablePalettes.BlueRed.ToString(),
            ThermalSequencePlayer.AvailablePalettes.BWRGB.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Fire.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Gradient.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Gray.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Sepia.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Steps.ToString(),
            ThermalSequencePlayer.AvailablePalettes.Temperature.ToString(),
            ThermalSequencePlayer.AvailablePalettes.WBRGB.ToString()};
        public ObservableCollection<string> AvailablePalettes => availablePalettes;

        //Selected palette
        public string SelectedPalette
        {
            get
            {
                if (thermalSequencePlayer == null)
                {
                    return ThermalSequencePlayer.AvailablePalettes.Sepia.ToString();
                }

                return thermalSequencePlayer.SelectedPalette.ToString();
            }
            set
            {
                switch (value)
                {
                    case "BlackRed":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.BlackRed;
                        break;
                    case "BlueRed":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.BlueRed;
                        break;
                    case "BWRGB":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.BWRGB;
                        break;
                    case "Fire":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Fire;
                        break;
                    case "Gradient":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Gradient;
                        break;
                    case "Gray":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Gray;
                        break;
                    case "Sepia":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Sepia;
                        break;
                    case "Steps":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Steps;
                        break;
                    case "Temperature":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.Temperature;
                        break;
                    case "WBRGB":
                        thermalSequencePlayer.SelectedPalette = ThermalSequencePlayer.AvailablePalettes.WBRGB;
                        break;
                    default:
                        break;
                }
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();

            }
        }

        //All available generation modes
        private readonly ObservableCollection<string> availableModes = new ObservableCollection<string>() {
            ThermalSequencePlayer.GenerationModes.Full.ToString(),
            ThermalSequencePlayer.GenerationModes.Signal.ToString(),
            ThermalSequencePlayer.GenerationModes.SignalBitmap.ToString()};
        public ObservableCollection<string> AvailableModes => availableModes;

        //Selected generation mode
        public string SelectedMode
        {
            get
            {
                if (thermalSequencePlayer == null)
                {
                    return ThermalSequencePlayer.GenerationModes.Full.ToString();
                }

                return thermalSequencePlayer.SelectedMode.ToString();
            }
            set
            {
                switch (value)
                {
                    case "Full":
                        thermalSequencePlayer.SelectedMode = ThermalSequencePlayer.GenerationModes.Full;
                        break;
                    case "Signal":
                        thermalSequencePlayer.SelectedMode = ThermalSequencePlayer.GenerationModes.Signal;
                        break;
                    case "SignalBitmap":
                        thermalSequencePlayer.SelectedMode = ThermalSequencePlayer.GenerationModes.SignalBitmap;
                        break;
                    default:
                        thermalSequencePlayer.SelectedMode = ThermalSequencePlayer.GenerationModes.Full;
                        break;
                }
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();

            }
        }

        //BitmapSource of current frame
        public BitmapSource ThermalSequenceImageSource
        {
            get => thermalSequencePlayer.ThermalSequenceSource;
            set
            {
                thermalSequenceImageSource = value;
                RaisePropertyChanged("ThermalSequenceImageSource");
            }
        }
        private BitmapSource thermalSequenceImageSource;

        //Emissivity
        public double? Emisivity
        {
            get => thermalSequencePlayer.ThermalParameters.Emissivity;
            set
            {
                thermalSequencePlayer.ThermalParameters.Emissivity = value;
                RaisePropertyChanged("Emisivity");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //Reflected temperature
        public double? ReflectedTemperature
        {
            get => thermalSequencePlayer.ThermalParameters.ReflectedTemperature;
            set
            {
                thermalSequencePlayer.ThermalParameters.ReflectedTemperature = value;
                RaisePropertyChanged("ReflectedTemperature");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //Atmospheric temperature
        public double? AtmosphericTemperature
        {
            get => thermalSequencePlayer.ThermalParameters.AtmosphericTemperature;
            set
            {
                thermalSequencePlayer.ThermalParameters.AtmosphericTemperature = value;
                RaisePropertyChanged("AtmosphericTemperature");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //External optics transmission
        public double? ExternalOpticsTransmission
        {
            get => thermalSequencePlayer.ThermalParameters.ExternalOpticsTransmission;
            set
            {
                thermalSequencePlayer.ThermalParameters.ExternalOpticsTransmission = value;
                RaisePropertyChanged("ExternalOpticsTransmission");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //Humidity
        public double? Humidity
        {
            get => thermalSequencePlayer.ThermalParameters.RelativeHumidity;
            set
            {
                thermalSequencePlayer.ThermalParameters.RelativeHumidity = value;
                RaisePropertyChanged("Humidity");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //Distance
        public double? Distance
        {
            get => thermalSequencePlayer.ThermalParameters.Distance;
            set
            {
                thermalSequencePlayer.ThermalParameters.Distance = value;
                RaisePropertyChanged("Distance");
                thermalSequencePlayer.ReloadFrame();
                RaiseImage();
            }
        }

        //Maximum temperature
        public string MaxTemperature => thermalSequencePlayer.MaxTemperatureValue.ToString("f2");

        //Minimum temperature
        public string MinTemperature => thermalSequencePlayer.MinTemperatureValue.ToString("f2");

        //Maximum signal value
        public string MaxSignalValue => thermalSequencePlayer.MaxSignalValue.ToString();

        //Minimum signal temperature
        public string MinSignalValue => thermalSequencePlayer.MinSignalValue.ToString();

        //Manufacturer info
        public string ManufacturerInfo => thermalSequencePlayer.Manufacturer;

        //Model name
        public string ModelName => thermalSequencePlayer.ModelName;

        //Serial number
        public string SerialNumber => thermalSequencePlayer.SerialNumber;

        //Current time
        public TimeSpan CurrentTime =>
                //var x = (thermalSequencePlayer.EndTime.TimeOfDay - thermalSequencePlayer.StartTime.TimeOfDay).ToString();
                (thermalSequencePlayer.CurrentTime.TimeOfDay - thermalSequencePlayer.StartTime.TimeOfDay);

        //End time
        public TimeSpan EndTime => (thermalSequencePlayer.EndTime.TimeOfDay - thermalSequencePlayer.StartTime.TimeOfDay);

        // Loop play
        public bool Loop
        {
            get => thermalSequencePlayer.LoopPlay;
            set
            {
                thermalSequencePlayer.LoopPlay = value;
                RaisePropertyChanged("Loop");
            }
        }

        // Frame count
        public int FrameCount => thermalSequencePlayer.FrameCount;

        // Current frame index
        public int CurrentFrameIndex
        {
            get => thermalSequencePlayer.CurrentFrameIndex;
            set
            {
                thermalSequencePlayer.CurrentFrameIndex = value;
                RaisePropertyChanged("CurrentFrameIndex");
            }
        }

        //Recalculation completed
        public bool RecalculationCompleted
        {
            get => recalculationCompleted;
            set
            {
                recalculationCompleted = value;
                RaisePropertyChanged("RecalculationCompleted");
            }
        }
        private bool recalculationCompleted = false;

        //Initial raise needed only once after sequence is loaded
        public void RaiseInit()
        {
            RaisePropertyChanged("FrameCount");
            RaisePropertyChanged("EndTime");
            RaisePropertyChanged("ManufacturerInfo");
            RaisePropertyChanged("ModelName");
            RaisePropertyChanged("SerialNumber");
            RaisePropertyChanged("UserDefinedName");
        }

        //Raise properties after change of frame
        public void RaiseImage()
        {
            RaisePropertyChanged("ThermalSequenceImageSource");
            RaisePropertyChanged("CurrentFrameIndex");
            RaisePropertyChanged("CurrentTime");
            RaisePropertyChanged("MaxTemperature");
            RaisePropertyChanged("MinTemperature");
            RaisePropertyChanged("MinSignalValue");
            RaisePropertyChanged("MaxSignalValue");
            RaisePropertyChanged("SelectedPalette");
            RaisePropertyChanged("SelectedMode");
        }

        //Raising thermal parameters
        public void RaiseParameters()
        {
            RaisePropertyChanged("Emisivity");
            RaisePropertyChanged("ReflectedTemperature");
            RaisePropertyChanged("AtmosphericTemperature");
            RaisePropertyChanged("Humidity");
            RaisePropertyChanged("Distance");
            RaisePropertyChanged("ExternalOpticsTransmission");
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
