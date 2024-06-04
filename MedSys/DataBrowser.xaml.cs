using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for DataBrowser.xaml
    /// </summary>
    public partial class DataBrowser : UserControl,INotifyPropertyChanged
    {
        public DataBrowser()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data",
                typeof(List<med>), typeof(DataBrowser), new PropertyMetadata(null, (depObj, arg) => {
                    ((DataBrowser)depObj).Data = (List<med>)arg.NewValue;
                }));

        public List<med> Data
        {
            get
            {
                return (List<med>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
                dataGrid.ItemsSource = value;
                //OnPropertyChanged(nameof(DisplayAllVisibility));
                OnPropertyChanged();
            }

        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
