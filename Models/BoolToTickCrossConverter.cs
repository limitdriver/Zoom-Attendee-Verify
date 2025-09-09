using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ZoomAttendeeVerifyApp
{
    public class BoolToTickCrossConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = (bool)value;
            string path = flag ? "/Images/tick.png" : "/Images/cross.png";
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}