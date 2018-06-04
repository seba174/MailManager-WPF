using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1
{
    class SizeConventer : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isUserLogged = (bool)values[0];
            double size = (double)values[1];
            if (isUserLogged)
            {
                double d = 4 / 11d * size;
                if (d < 210)
                    return new GridLength(210);
                return new GridLength(d);
            }
            else
                return new GridLength(0);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
