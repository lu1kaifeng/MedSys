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
    /// Interaction logic for MedNameListView.xaml
    /// </summary>
    public partial class MedNameListView : UserControl
    {
        public MedNameListView()
        {
            InitializeComponent();
            PenisDataBinding.SelectedIndex = 0;
           
        }
        public static readonly DependencyProperty ContentsProperty =
    DependencyProperty.Register("Contents",
         typeof(ObservableCollection<MedNameListViewModel>), typeof(MedNameListView));

        public ObservableCollection<MedNameListViewModel> Contents
        {
            get
            {
                return (ObservableCollection<MedNameListViewModel>)GetValue(ContentsProperty);
            }
            set
            {
                SetValue(ContentsProperty, value);

            }

        }
        
        public class MedNameListViewModel
        {
            public string Content { get; set; } = string.Empty;
            public bool IsExact { get; set; } = true;

            private readonly CollectionView _medNameTypeEntries = new CollectionView(Typing.MedNameType);
            private string _medNameTypeEntry = Typing.MedNameType[0];

            public CollectionView MedNameTypeEntries
            {
                get { return _medNameTypeEntries; }
            }

            public string MedNameTypeEntry
            {
                get { return _medNameTypeEntry; }
                set
                {
                    if (_medNameTypeEntry == value) return;
                    _medNameTypeEntry = value;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Contents.Add(new MedNameListViewModel());
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
