using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WIC_SDK_Sample.Utils
{
    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolToStringConverter : BoolConverter<String>
    {
    }

    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolToBoolConverter : BoolConverter<bool>
    {
    }

    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolToFillConverter : BoolConverter<Brush>
    {
    }

    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolToFloatConverter : BoolConverter<float>
    {
    }

    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolToVisibilityConverter : BoolConverter<Visibility>
    {
    }

    [ClassInterface(ClassInterfaceType.None), ComVisible(false)]
    public class BoolConverter<T> : IValueConverter
    {
        public T FalseValue
        {
            get;
            set;
        }
        public T TrueValue
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }
}
