using MedSys.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RadioIntToBoolConverter radioBoolToIntConverter = new RadioIntToBoolConverter();
        CheckBoxesToListConverter checkBoxesToListConverter = new CheckBoxesToListConverter();
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonSerializer.Serialize(this.DataContext);
            
            MessageBox.Show(json);
        }
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            
        }
        public bool DatePickerEnabled
        {
            get
            {
                return TimeRangeEntry == "自定义";
            }
           
        }

        private DateTime _fromDate = DateTime.MinValue;
        public DateTime FromDate
        {
            set { _fromDate = value;OnPropertyChanged(); }
            get { return _fromDate; }
        }

        private DateTime _toDate=DateTime.Now;
        public DateTime ToDate
        {
            set { _toDate = value; OnPropertyChanged(); }
            get { return _toDate; }
        }

        private List<string> _reportSubject = new List<string>();
        public List<string> ReportSubject
        {
            set { 
                _reportSubject = value;
                OnPropertyChanged();
            }
            get { return _reportSubject; }
        }
        private string _medName="";
        public string MedName
        {
             get { return _medName; }
            set {
                _medName = value;
                OnPropertyChanged();
            }
        }

        private string _medBatchNo="";
        public string MedBatchNo
        {
            get { return _medBatchNo; }
            set
            {
                _medBatchNo = value;
                OnPropertyChanged();
            }
        }
        
            private string _manufacturerName = "";
              public string ManufacturerName
        {
            set { _manufacturerName = value; OnPropertyChanged();}
            get { return _manufacturerName; }
        }

            private string _approvalNo = "";
        public string ApprovalNo
        {
            set { _approvalNo = value; OnPropertyChanged(); }
            get { return _approvalNo; }
        }


        private string _adverseEffectName="";
        public string AdverseEffectName
        {
            set { _adverseEffectName = value; OnPropertyChanged(); }
            get { return _adverseEffectName; }
        }


        private bool _medNameTypeExactOrContain = true;
        public bool MedNameTypeExactOrContain
        {
            get { return _medNameTypeExactOrContain; }
            set
            {
                _medNameTypeExactOrContain = value;
                OnPropertyChanged();
            }
        }

        private bool _manufacturerQueyTypeExactOrContain = true;
        public bool ManufacturerQueyTypeExactOrContain { get { return _manufacturerQueyTypeExactOrContain; } set {
                _manufacturerQueyTypeExactOrContain = value;
                OnPropertyChanged();
            } }

        private readonly CollectionView _timeTypeEntries = new CollectionView(Typing.TimeType);
        private string _timeTypeEntry = Typing.TimeType[0];

        public CollectionView TimeTypeEntries
        {
            get { return _timeTypeEntries; }
        }

        public string TimeTypeEntry
        {
            get { return _timeTypeEntry; }
            set
            {
                if (_timeTypeEntry == value) return;
                _timeTypeEntry = value;
                OnPropertyChanged();
            }
        }

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
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _timeRangeEntries = new CollectionView(Typing.TimeRangeType);
        private string _timeRangeEntry = Typing.TimeRangeType[0];

        public CollectionView TimeRangeEntries
        {
            get { return _timeRangeEntries; }
        }

        public string TimeRangeEntry
        {
            get { return _timeRangeEntry; }
            set
            {
                if (_timeRangeEntry == value) return;
                _timeRangeEntry = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DatePickerEnabled));
            }
        }
        
            private readonly CollectionView _adverseEffectResultTypeEntries = new CollectionView(Typing.AdverseEffectResultType);
        private string _adverseEffectResultTypeEntry = Typing.AdverseEffectResultType[0];

        public CollectionView AdverseEffectResultTypeEntries
        {
            get { return _adverseEffectResultTypeEntries; }
        }

        public string AdverseEffectResultTypeEntry
        {
            get { return _adverseEffectResultTypeEntry; }
            set
            {
                if (_adverseEffectResultTypeEntry == value) return;
                _adverseEffectResultTypeEntry = value;
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
