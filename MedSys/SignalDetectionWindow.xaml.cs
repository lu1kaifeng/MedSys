using Python.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for SignalDetectionWindow.xaml
    /// </summary>
    public partial class SignalDetectionWindow : Window
    {

        SignalDetectionWindowViewModel vm;
        public SignalDetectionWindow(List<med> dataList)
        {

            InitializeComponent();
            vm = new SignalDetectionWindowViewModel(dataList);
            this.DataContext = vm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(() =>
            {
                var res = vm.SignalDetection();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    vm.DetectionResult = res;
                    vm.dr = res;
                    Tabs.SelectedItem = Detection;
                    this.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                });
            });

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Tabs.SelectedItem = Selection;
        }

        
    }
    public class SignalDetectionWindowViewModel : INotifyPropertyChanged
    {

        private void OnNameSelectionChanged(bool value)
        {
            OnPropertyChanged(nameof(AnySelected));
            if (allSelected && !value)
            { // all are selected, and one gets turned off
                allSelected = false;
                OnPropertyChanged(nameof(AllSelected));
            }
            else if (!allSelected && this.Names.All(c => c.Selected))
            { // last one off one gets turned on, resulting in all being selected
                allSelected = true;
                OnPropertyChanged(nameof(AllSelected));
            }
        }

        public List<SignalDetectionEntry> dr;
        private IEnumerable<SignalDetectionEntry> detectionResult;
        public IEnumerable<SignalDetectionEntry> DetectionResult
        {
            set
            {
                detectionResult = value;
                OnPropertyChanged();
            }
            get
            {
                return detectionResult;
            }
        }
        private bool allSelected;

        public bool AllSelected
        {
            get
            {
                return this.allSelected;
            }
            set
            {
                allSelected = value;

                foreach (var name in this.Names)
                    name.Selected = value;

                OnPropertyChanged();
            }
        }


        public bool AnySelected
        {
            get
            {
                return (from n in Names where n.Selected select n.Name).Count() > 0;
            }
            
        }

        public IEnumerable<med> SelectedEntities
        {
            get
            {
                return (from dd in DataList where (from n in Names where n.Selected select n.Name).Contains(dd.通用名称) select dd);
            }
        }


        private string filter = "";
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                Names = new ObservableCollection<DrugName>(from dd in dnl where dd.Name.Contains(value) select dd);
                
                OnPropertyChanged();
            }
        }

        private string resultFilter = "";
        public string ResultFilter
        {
            get
            {
                return resultFilter;
            }
            set
            {
                resultFilter = value;
                DetectionResult = (from dd in dr where dd.PreferredTerm.Name.Contains(value) select dd);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DrugName> names;
        public ObservableCollection<DrugName> Names
        {
            get { return names; }
            set
            {
                names = value;
                OnPropertyChanged();
            }
        }
        List<med> DataList;
        List<DrugName> dnl = new List<DrugName>();
        public SignalDetectionWindowViewModel(List<med> DataList)
        {
            this.DataList = DataList;
            Dictionary<string, int> buckets = new Dictionary<string, int>();
            foreach (var dd in this.DataList)
            {
                if (!buckets.ContainsKey(dd.通用名称)) buckets.Add(dd.通用名称, 1);
                else
                    buckets[dd.通用名称]++;
            }
            int i = 0;

            foreach (var dic in buckets)
            {
                dnl.Add(new DrugName(i, dic.Key, dic.Value, OnNameSelectionChanged));
                i++;
            }
            Names = new ObservableCollection<DrugName>(dnl);
            Names.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler
((sender, args) =>
{
    OnPropertyChanged(nameof(SelectedEntities));
});
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public List<SignalDetectionEntry> SignalDetection()
        {

            int N = MedDRAEntry.PreferredTerms.Count;
            ConcurrentBag<SignalDetectionEntry> sdl = new ConcurrentBag<SignalDetectionEntry>();
            var names = from n in Names where n.Selected select n.Name;
            var selectedEntitiesCount = SelectedEntities.Count();
            Parallel.ForEach(MedDRAEntry.PreferredTerms, (pt) =>
        {
            var aPlusCSrc = (from ss in this.DataList
                             where ss.系统不良反应术语.Contains(pt.Name)
                             select ss);
            var a = (from asrc in aPlusCSrc where names.Contains(asrc.通用名称) select asrc).Count();
            int b = selectedEntitiesCount - a;
            int aPlusC = (aPlusCSrc).Count();
            int d = N - b - aPlusC;
            sdl.Add(new SignalDetectionEntry(pt, a, b, aPlusC - a, d));
        });

            return sdl.ToList();
        }
    }
    public class SignalDetectionEntry
    {
        public MedDRAEntry PreferredTerm { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }

        public double ROR
        {
            get
            {
                return ((double)A * D) / ((double)B * C);
            }
        }

        public double LnROR
        {
            get
            {
                return Math.Log(ROR, Math.E);
            }
        }

        public double RORSE
        {
            get
            {
                return Math.Sqrt(1.0 / A + 1.0/B+ 1.0/C+ 1.0/D);
            }
        }

        public double RORP95CIPlus
        {
            get
            {
                return Math.Exp(LnROR + 1.96 * RORSE);
            }
        }
         public double RORP95CIMinus
        {
            get
            {
                return Math.Exp(LnROR - 1.96 * RORSE);
            }
        }

        public double PRR
        {
            get
            {
                return A / ((double)A + B) / C / ((double)C + D);
            }
        }

        public double LnPRR
        {
            get
            {
                return Math.Log(PRR, Math.E);
            }
        }

        public double PRRSE
        {
            get
            {
                return Math.Sqrt(1.0 / A - 1.0 / (A+B) + 1.0 / C - 1.0 / (C+D));
            }
        }

        public double PRRP95CIPlus
        {
            get
            {
                return Math.Exp(LnPRR + 1.96 * PRRSE);
            }
        }
        public double PRRP95CIMinus
        {
            get
            {
                return Math.Exp(LnPRR - 1.96 * PRRSE);
            }
        }
        
        public double MHRAX2
        {
            get
            {
                return Math.Pow(Math.Abs(A*D-B*C) - 0.5*(A+B+C+D),2)*(A+B+C+D)/((A+B)*(C+D)*(A+C)*(B+D));
            }
        }
        public double IC
        {
            get
            {
                return Math.Log((A * (A + B + C + D) / ((A + B) * (A + C))),2);
            }
        }
        public double EIC
        {
            get
            {
                return Math.Log((A + 1) * (A + B + C + D + 2) * (A + B + C + D + 2) / ((A + B + C + D + 2) * (A + B + 1) * (A + C + 1)), 2);
            }
        }
        public SignalDetectionEntry(MedDRAEntry PreferredTerm, int A, int B, int C, int D)
        {
            this.PreferredTerm = PreferredTerm;

            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }
    }
    public class DrugName : INotifyPropertyChanged
    {
        private int id;
        private int count;
        private string name;
        private bool selected;
        private Action<bool> onSelectionChanged;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged();
                onSelectionChanged(value);
            }
        }

        public DrugName(int id, string name, int count, Action<bool> onSelectionChanged)
        {
            this.id = id;
            this.name = name;
            this.count = count;
            this.onSelectionChanged = onSelectionChanged;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
