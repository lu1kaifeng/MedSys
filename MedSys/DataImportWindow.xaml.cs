
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for DataImportWindow.xaml
    /// </summary>
    /// 
    public class DataImportWindowDataContext : INotifyPropertyChanged
    {



        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<med> _page = null;
        public ObservableCollection<med> CurrentPage
        {
            get { return _page; }
            set { 
                _page = value;
                OnPropertyChanged();
                OnPropertyChanged("Loaded");
            }
        }

        public bool Loaded
        {
            get { return _page != null; }      
        }
    }
    public partial class DataImportWindow : Window
    {
        public DataImportWindowDataContext vm = new DataImportWindowDataContext();
        public DataImportWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Paginator.InitPage();
            
        }
    }
}
