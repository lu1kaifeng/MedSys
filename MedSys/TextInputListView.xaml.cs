using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for TextInputListView.xaml
    /// </summary>
    public partial class TextInputListView : UserControl
    {
        public TextInputListView()
        {
            InitializeComponent();
            PenisDataBinding.SelectedIndex = 0;
           
        }
        public static readonly DependencyProperty ContentsProperty =
    DependencyProperty.Register("Contents",
         typeof(ObservableCollection<TextInputListViewModel>), typeof(TextInputListView),new PropertyMetadata());

        public ObservableCollection<TextInputListViewModel> Contents
        {
            get
            {
                return (ObservableCollection<TextInputListViewModel>)GetValue(ContentsProperty);
            }
            set
            {
                SetValue(ContentsProperty, value);
            }

        }
        
        public class TextInputListViewModel
        {
            public string Content { get; set; } = string.Empty;
            public bool IsExact { get; set; } = true;
            public string ToWhereClause(string colName)
            {
                if (IsExact)
                {
                    return " (" + colName + " like N\'" + Content + "\') ";
                }
                else
                {
                    return " (" + colName + " like N\'%" + Content + "%\') ";
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Contents.Add(new TextInputListViewModel());
            Remove.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int selected = PenisDataBinding.SelectedIndex;
            Contents.RemoveAt(PenisDataBinding.SelectedIndex);
            if (Contents.Count == 1)
            {
                Remove.IsEnabled = false;
            }
            PenisDataBinding.SelectedIndex = 0;
        }
    }

}
