using MedSys.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
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
using static MedSys.MedNameListView;
using static MedSys.TextInputListView;

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
            var options1 = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.All),
                WriteIndented = false
            };
            string json = JsonSerializer.Serialize(this.DataContext,options1);
            
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


        private ObservableCollection<MedNameListViewModel> _medNameList = new ObservableCollection<MedNameListViewModel>(new MedNameListViewModel[] { new MedNameListViewModel() });
        public ObservableCollection<MedNameListViewModel> MedNameList
        {
            get
            {
                return _medNameList;
            }
            set
            {
                _medNameList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TextInputListViewModel> _manufacturerNameList  = new ObservableCollection<TextInputListViewModel>(new TextInputListViewModel[] { new TextInputListViewModel() });
        public ObservableCollection<TextInputListViewModel> ManufacturerNameList
        {
            get
            {
                return _manufacturerNameList;
            }
            set
            {
                _manufacturerNameList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TextInputListViewModel> _commercialNameList = new ObservableCollection<TextInputListViewModel>(new TextInputListViewModel[] { new TextInputListViewModel() });
        public ObservableCollection<TextInputListViewModel> CommercialNameList
        {
            get
            {
                return _commercialNameList;
            }
            set
            {
                _commercialNameList = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _reportEstimateEntries = new CollectionView(Typing.ReportEstimateType);
        private string _reportEstimateEntry = Typing.ReportEstimateType[0];

        public CollectionView ReportEstimateEntries
        {
            get { return _reportEstimateEntries; }
        }

        public string ReportEstimateEntry
        {
            get { return _reportEstimateEntry; }
            set
            {
                if (_reportEstimateEntry == value) return;
                _reportEstimateEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _sexEntries = new CollectionView(Typing.SexType);
        private string _sexEntry = Typing.SexType[0];

        public CollectionView SexEntries
        {
            get { return _sexEntries; }
        }

        public string SexEntry
        {
            get { return _sexEntry; }
            set
            {
                if (_sexEntry == value) return;
                _sexEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _isDomesticEntries = new CollectionView(Typing.IsDomesticType);
        private string _isDomesticEntry = Typing.IsDomesticType[0];

        public CollectionView IsDomesticEntries
        {
            get { return _isDomesticEntries; }
        }

        public string IsDomesticEntry
        {
            get { return _isDomesticEntry; }
            set
            {
                if (_isDomesticEntry == value) return;
                _isDomesticEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _reportTypeEntries = new CollectionView(Typing.ReportTypeType);
        private string _reportTypeEntry = Typing.ReportTypeType[0];

        public CollectionView ReportTypeEntries
        {
            get { return _reportTypeEntries; }
        }

        public string ReportTypeEntry
        {
            get { return _reportTypeEntry; }
            set
            {
                if (_reportTypeEntry == value) return;
                _reportTypeEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _infoSourceEntries = new CollectionView(Typing.InfoSourceType);
        private string _infoSourceEntry = Typing.InfoSourceType[0];

        public CollectionView InfoSourceEntries
        {
            get { return _infoSourceEntries; }
        }

        public string InfoSourceEntry
        {
            get { return _infoSourceEntry; }
            set
            {
                if (_infoSourceEntry == value) return;
                _infoSourceEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _medTypeEntries = new CollectionView(Typing.MedTypeType);
        private string _medTypeEntry = Typing.MedTypeType[0];

        public CollectionView MedTypeEntries
        {
            get { return _medTypeEntries; }
        }

        public string MedTypeEntry
        {
            get { return _medTypeEntry; }
            set
            {
                if (_medTypeEntry == value) return;
                _medTypeEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _baseMedEntries = new CollectionView(Typing.BaseMedType);
        private string _baseMedEntry = Typing.BaseMedType[0];

        public CollectionView BaseMedEntries
        {
            get { return _baseMedEntries; }
        }

        public string BaseMedEntry
        {
            get { return _baseMedEntry; }
            set
            {
                if (_baseMedEntry == value) return;
                _baseMedEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _deliveryEntries = new CollectionView(Typing.DeliveryType);
        private string _deliveryEntry = Typing.DeliveryType[0];

        public CollectionView DeliveryEntries
        {
            get { return _deliveryEntries; }
        }

        public string DeliveryEntry
        {
            get { return _deliveryEntry; }
            set
            {
                if (_deliveryEntry == value) return;
                _deliveryEntry = value;
                OnPropertyChanged();
            }
        }

        private readonly CollectionView _focusTypeEntries = new CollectionView(Typing.FocusTypeType);
        private string _focusTypeEntry = Typing.FocusTypeType[0];

        public CollectionView FocusTypeEntries
        {
            get { return _focusTypeEntries; }
        }

        public string FocusTypeEntry
        {
            get { return _focusTypeEntry; }
            set
            {
                if (_focusTypeEntry == value) return;
                _focusTypeEntry = value;
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
