using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for ExcelColSelectDialog.xaml
    /// </summary>
    public partial class ExcelColSelectDialog : Window
    {
        public ExcelColSelectDialog(IEnumerable<string> cols)
        {
            InitializeComponent();
            
            var fieldList = (new med()).GetType()
                .GetProperties()
                .Select(field => field.Name);
            foreach (var f in fieldList)
            {
                if(f != "ID")
                    NameList.Children.Add(new ExcelColSelect(f, cols));
            }
        }
         
        public IDictionary<string, string> Alias
        {
            get
            {
                Dictionary<string, string> aliasDict = new Dictionary<string, string>();
                foreach (var n in NameList.Children)
                {
                    var col = (n as ExcelColSelect);
                    if(col.Select.SelectedItem!=null && !aliasDict.ContainsKey(col.Select.SelectedItem.ToString())) aliasDict.Add(col.Select.SelectedItem.ToString(), col.Field.Content.ToString());
                }

                return aliasDict;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
