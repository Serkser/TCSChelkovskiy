using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TCSChelkovskiy.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value;
            string[] filenameParts = val.Split('/','\\');
            var bitmap = Services.ImageDownloader.DownloadImage(val, filenameParts[filenameParts.Length-1]);
            return Services.BitmapToImageSourceConverter.BitmapToImageSource(bitmap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
