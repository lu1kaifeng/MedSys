
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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

        
    }
    public partial class DataImportWindow : Window
    {
        public DataImportWindowDataContext vm = new DataImportWindowDataContext();
        public DataImportWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
