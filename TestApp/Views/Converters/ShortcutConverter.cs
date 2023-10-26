using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace TestApp.Views.Converters
{
    // Возможно, понадобиться сократить не только Guid, но и другие данные представленные длинной строкой.
    // Пусть будет.
    public class ShortcutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value.ToString() ?? string.Empty;
            if (str.Length > 8)
            {
                var chars = str.TakeLast(5);
                return "..." + string.Concat(chars);
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
