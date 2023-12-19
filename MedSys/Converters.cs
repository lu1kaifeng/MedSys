using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

}
