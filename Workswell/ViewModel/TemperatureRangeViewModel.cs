using System.ComponentModel;
using WIC_SDK;

namespace WIC_SDK_Sample.ViewModel
{
    public class TemperatureRangeViewModel : INotifyPropertyChanged
    {
        private readonly TemperatureRange tempRange;

        public TemperatureRangeViewModel(TemperatureRange tempRange)
        {
            this.tempRange = tempRange;
        }

        public string TemperatureRange => tempRange.Type + ": " + tempRange.RangeMinC.ToString() + " - " + tempRange.RangeMaxC.ToString();

        public int Index => tempRange.Index;

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
