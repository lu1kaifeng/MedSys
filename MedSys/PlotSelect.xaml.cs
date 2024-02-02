using MedSys.Converters;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MedSys.MedNameListView;
using static MedSys.TextInputListView;

namespace MedSys
{
    public class PlotDataArgs
    {
        public PlotDataArgs(List<med> backingData, string whereClause, SqlParameter[] paramList)
        {
            BackingData = backingData;
            WhereClause = whereClause;
            ParamList = paramList;
        }

        public List<med> BackingData { get; set; }
        public string WhereClause { get; set; }

        public SqlParameter[] ParamList { get; set; }
    }

    /// <summary>
    /// Interaction logic for PlotSelect.xaml
    /// </summary>
    public partial class PlotSelect : UserControl, INotifyPropertyChanged
    {
        TreeViewItemToIntOneWayToSourceConverter treeViewItemToIntOneWayConverter = new TreeViewItemToIntOneWayToSourceConverter();
        IEnumerable<MethodInfo> PlottingMethods ;
        public PlotSelect()
        {
            PlottingMethods = GetType().GetMethods().Where(x => x.GetCustomAttributes(typeof(Plotting), false).FirstOrDefault() != null);
            InitializeComponent();
            
        }

        

        public static readonly DependencyProperty BackingDataProperty =
DependencyProperty.Register("BackingData",
 typeof(PlotDataArgs), typeof(PlotSelect),
    new FrameworkPropertyMetadata(
        null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, onBackingDataPropertyChangedCallback));
        private int _selected = 0;

        public PlotDataArgs BackingData
        {
            get
            {
                return (PlotDataArgs)GetValue(BackingDataProperty);
            }
            set
            {
                SetValue(BackingDataProperty, value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmptyList));
                OnPropertyChanged(nameof(NotEmptyList));
                OnPropertyChanged(nameof(EmptyListVisibility));
                OnPropertyChanged(nameof(NotEmptyListVisibility));
            }

        }

        private static void onBackingDataPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PlotSelect myClass = (PlotSelect)sender;
            myClass.BackingData = (PlotDataArgs)args.NewValue;
            myClass.PlotSelected = 0;
            try
            {
                foreach (var m in myClass.PlottingMethods)
                {
                    m.Invoke(myClass, null);
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool EmptyList
        {
            get
            {
                return BackingData == null || BackingData.BackingData.Count == 0;
            }

        }

        public bool NotEmptyList
        {
            get
            {
                return BackingData != null && BackingData.BackingData.Count > 0;
            }

        }

        public Visibility EmptyListVisibility
        {
            get
            {
                return (BackingData == null || BackingData.BackingData.Count == 0)?Visibility.Visible:Visibility.Hidden;
            }

        }

        public Visibility NotEmptyListVisibility
        {
            get
            {
                return (BackingData != null && BackingData.BackingData.Count > 0 )? Visibility.Visible : Visibility.Hidden;
            }

        }



        public int PlotSelected
        {
            set { _selected = value;

                OnPropertyChanged();
            }
            get { return _selected; }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

       
    }

}
