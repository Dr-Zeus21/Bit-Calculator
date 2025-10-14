using System;
using System.Globalization;
using System.Windows.Data;

namespace BrendensFuntimeApp.core
{
    public class BitConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int baseVal) && int.TryParse(parameter?.ToString(), out int offset))
            {
               return baseVal + offset;
            }

            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
