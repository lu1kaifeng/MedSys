using MedSys.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Data;
using static MedSys.MedNameListView;
using static MedSys.TextInputListView;
using System.Linq;
using System.Data.Linq.SqlClient;
using ScottPlot;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RadioIntToBoolConverter radioBoolToIntConverter = new RadioIntToBoolConverter();
        CheckBoxesToListConverter checkBoxesToListConverter = new CheckBoxesToListConverter();
        CheckBoxesToListConverter baseMedConverter = new CheckBoxesToListConverter();
        MainWindowViewModel viewModel = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine(MedDRAEntry.Entries);
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var options1 = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.All),
                WriteIndented = false
            };
            string json = JsonSerializer.Serialize(this.DataContext, options1);
            MessageBox.Show(json);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            viewModel.DataList = null;
            Mouse.OverrideCursor = Cursors.Wait;
            var json = viewModel.Query();
            json.ContinueWith(x =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    viewModel.DataList = x.Result;
                    this.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                });
            });

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new DataImportWindow().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            viewModel.DataList = null;
            Mouse.OverrideCursor = Cursors.Wait;
            var json = viewModel.Query();
            json.ContinueWith(x =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    
                    this.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                    new SignalDetectionWindow(x.Result.BackingData).ShowDialog();
                    //viewModel.DataList = x.Result;
                });
            });
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            LoginWindow.NoAuto();
            new LoginWindow().Show();
            this.Close();
            
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
    public partial class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {

        }
        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        public bool DatePickerEnabled
        {
            get
            {
                return TimeRangeEntry == Typing.TimeRangeType[0];
            }

        }

        private DateTime _fromDate = DateTime.Now.AddYears(-10);
        public DateTime FromDate
        {
            set { _fromDate = value; OnPropertyChanged(); }
            get { return _fromDate; }
        }

        private DateTime _toDate = DateTime.Now;
        public DateTime ToDate
        {
            set { _toDate = value; OnPropertyChanged(); }
            get { return _toDate; }
        }

        private List<string> _reportSubject = new List<string>(Typing.ReportSubject);
        public List<string> ReportSubject
        {
            set
            {
                _reportSubject = value;
                OnPropertyChanged();
            }
            get { return _reportSubject; }
        }
        private string _medName = "";
        public string MedName
        {
            get { return _medName; }
            set
            {
                _medName = value;
                OnPropertyChanged();
            }
        }

        private string _medBatchNo = "";
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
            set { _manufacturerName = value; OnPropertyChanged(); }
            get { return _manufacturerName; }
        }

        private string _approvalNo = "";
        public string ApprovalNo
        {
            set { _approvalNo = value; OnPropertyChanged(); }
            get { return _approvalNo; }
        }


        private string _adverseEffectName = "";
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
        public bool ManufacturerQueyTypeExactOrContain
        {
            get { return _manufacturerQueyTypeExactOrContain; }
            set
            {
                _manufacturerQueyTypeExactOrContain = value;
                OnPropertyChanged();
            }
        }

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

        private ObservableCollection<TextInputListViewModel> _manufacturerNameList = new ObservableCollection<TextInputListViewModel>(new TextInputListViewModel[] { new TextInputListViewModel() });
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

        private List<string> _baseMed = new List<string>(Typing.BaseMedType);
        public List<string> BaseMed
        {
            set
            {
                _baseMed = value;
                OnPropertyChanged();
            }
            get { return _baseMed; }
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

        private string _reportNoFrom = "";
        public string ReportNoFrom
        {
            get { return _reportNoFrom; }
            set
            {
                _reportNoFrom = value;
                OnPropertyChanged();
            }
        }
        private string _reportNoTo = "";
        public string ReportNoTo
        {
            get { return _reportNoTo; }
            set
            {
                _reportNoTo = value;
                OnPropertyChanged();
            }
        }
        private string _dosageForm = "";
        public string DosageForm
        {
            get { return _dosageForm; }
            set
            {
                _dosageForm = value;
                OnPropertyChanged();
            }
        }
        private string _patientName = "";
        public string PatientName
        {
            get { return _patientName; }
            set
            {
                _patientName = value;
                OnPropertyChanged();
            }
        }
        private string _ageFrom = "";
        public string AgeFrom
        {
            get { return _ageFrom; }
            set
            {
                _ageFrom = value;
                OnPropertyChanged();
            }
        }
        private string _ageTo = "";
        public string AgeTo
        {
            get { return _ageTo; }
            set
            {
                _ageTo = value;
                OnPropertyChanged();
            }
        }
        private string _reportUnitName = "";
        public string ReportUnitName
        {
            get { return _reportUnitName; }
            set
            {
                _reportUnitName = value;
                OnPropertyChanged();
            }
        }
        private string _hospitalName = "";
        public string HospitalName
        {
            get { return _hospitalName; }
            set
            {
                _hospitalName = value;
                OnPropertyChanged();
            }
        }
        private string _preexistingCondition = "";
        public string PreexistingCondition
        {
            get { return _preexistingCondition; }
            set
            {
                _preexistingCondition = value;
                OnPropertyChanged();
            }
        }
        private string _medReason = "";
        public string MedReason
        {
            get { return _medReason; }
            set
            {
                _medReason = value;
                OnPropertyChanged();
            }
        }
        private string _sMQName = "";
        public string SMQName
        {
            get { return _sMQName; }
            set
            {
                _sMQName = value;
                OnPropertyChanged();
            }
        }

        private bool _otherUnique = false;
        public bool OtherUnique
        {
            set { _otherUnique = value; OnPropertyChanged(); }
            get { return _otherUnique; }
        }
        private bool _otherNoOutlier = false;
        public bool OtherNoOutlier
        {
            set { _otherNoOutlier = value; OnPropertyChanged(); }
            get { return _otherNoOutlier; }
        }

        private bool _commentStateCityCommented = false;
        public bool CommentStateCityCommented
        {
            set { _commentStateCityCommented = value; OnPropertyChanged(); }
            get { return _commentStateCityCommented; }
        }

        private bool _commentStateProvinceCommented = false;
        public bool CommentStateProvinceCommented
        {
            set { _commentStateProvinceCommented = value; OnPropertyChanged(); }
            get { return _commentStateProvinceCommented; }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

}
