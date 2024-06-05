using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for ExcelColSelect.xaml
    /// </summary>
    public partial class ExcelColSelect : UserControl
    {
        public ExcelColSelect(string fieldName,IEnumerable<string> cols)
        {
            InitializeComponent();
            Field.Content = fieldName;
            Select.ItemsSource = cols;
            int ci = 0;
            Regex reg = new Regex("[*'\",-_&#^@/\\(\\)]");
            foreach (var  c in cols)
            {
                if (reg.Replace(c, string.Empty) == fieldName) Select.SelectedIndex = ci;
                ci++;
            }
        }
    }
}
