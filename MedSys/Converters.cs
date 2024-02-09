using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MedSys.Converters
{
    public class RadioIntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool back = (bool)value;
            if (int.Parse((string)parameter) == 1)
            {
                return back;
            }
            else
                return !back;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool back = (bool)value;
            if (int.Parse((string)parameter) == 1)
            {
                return back;
            }
            else
                return !back;
        }
    }

    public class TreeViewItemToIntOneWayToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((int)value == 0)
            {
                return false;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            bool back = (bool)value;
            if (back)
            {
                return parameter;
            }
            else
            {
                return 0;//Binding.DoNothing;
            }
        }
    }

    public class CheckBoxesToListConverter : IValueConverter
    {
        
        public CheckBoxesToListConverter()
        {
            
        }
        List<string> stringList;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            List<string> strings = (List<string>) value;
            this.stringList = strings;
            if (strings.Contains((string)parameter))
            {
                return true;
            }
            else
                return false;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value){
                stringList.Add((string)parameter);
                
            }
            else
            {
                stringList.Remove((string)parameter);
            }
            return stringList;
        }


    }

    public class BoolVisualConvertion : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? b = value as bool?;
            if (b == null)
            {
                return Visibility.Hidden;
            }
            return b == true ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility? v = value as Visibility?;
            v = v == null ? Visibility.Hidden : Visibility.Visible;
            return v;
        }
    }

    public class DoubleNaNToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value))
            {
                return null;
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return double.NaN;
            }
            else return value;
        }
    }

}
