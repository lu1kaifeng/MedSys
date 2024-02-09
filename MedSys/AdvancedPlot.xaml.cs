using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Text.RegularExpressions;
using static MedSys.TextInputListView;
using System.Collections.ObjectModel;
using ScottPlot;
using static ScottPlot.Plottable.PopulationPlot;
using System.Reflection.Emit;
using NumSharp.Utilities;
using Color = System.Windows.Media.Color;
using System.Drawing;
using MedSys.Converters;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for AdvancedPlot.xaml
    /// </summary>
    public partial class AdvancedPlot : UserControl,INotifyPropertyChanged
    {
        public DoubleNaNToNullConverter nanConverter = new DoubleNaNToNullConverter();
        public AdvancedPlot()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty PlotDataProperty =
    DependencyProperty.Register("PlotData",
         typeof(PlotData), typeof(AdvancedPlot), new PropertyMetadata(null, (depObj,arg) => {
             ((AdvancedPlot)depObj).PlotData = arg.NewValue as PlotData;
         }));

        public static readonly DependencyProperty DisplayAllProperty =
            DependencyProperty.Register("DisplayAll",
                typeof(bool), typeof(AdvancedPlot), new PropertyMetadata(false,  (depObj, arg) => {
                    ((AdvancedPlot)depObj).DisplayAll = (bool)arg.NewValue;
                }));

        public bool DisplayAll
        {
            get
            {
                return (bool)GetValue(DisplayAllProperty);
            }
            set
            {
                SetValue(DisplayAllProperty,value);
                OnPropertyChanged(nameof(DisplayAllVisibility));
                OnPropertyChanged();
            }

        }

        public Visibility DisplayAllVisibility => DisplayAll ? Visibility.Collapsed : Visibility.Visible;

        /*private Color _fgColor;

        public Color FgColor
        {
            get
            {
                return _fgColor;
            }
            set
            {
                _fgColor = value;
                OnPropertyChanged();
            }
        }*/

        public PlotData PlotData
        {
            get
            {
                return (PlotData)GetValue(PlotDataProperty);
            }
            set
            {
                if (value == null)
                {
                    return; }
                SetValue(PlotDataProperty, value);
                TopBox.MaxValue = value.bins.Length;
                TotalLabel.Content = value.bins.Length;
                RefreshPlot();
            }

        }

        private readonly CollectionView _plotTypeEntries = new CollectionView(Typing.PlotType);
        private string _plotTypeEntry = Typing.PlotType[0];

        public CollectionView PlotTypeEntries
        {
            get { return _plotTypeEntries; }
        }

        public string PlotTypeEntry
        {
            get { return _plotTypeEntry; }
            set
            {
                if (_plotTypeEntry == value) return;
                _plotTypeEntry = value;
                OnPropertyChanged();
                RefreshPlot();
            }
        }

        private int _numTop = 10;

        public int NumTop
        {
            get { return _numTop; }
            set
            {
                _numTop = value;
                OnPropertyChanged();
                RefreshPlot();
            }
        }

        private void RefreshPlot()
        {
            if (PlotData == null) return;
            if (PlotData.bins.Length == 0) return;
            if (DisplayAll) _numTop = PlotData.bins.Length;
            if (PlotTypeEntry == Typing.PlotType[0])
            {
                var truncBins = PlotData.bins.Slice(0, _numTop>PlotData.bins.Length? PlotData.bins.Length: _numTop);
                var truncPositions = PlotData.positions.Slice(0, _numTop > PlotData.bins.Length ? PlotData.bins.Length : _numTop);
                var truncLabels = PlotData.labels.Slice(0, _numTop > PlotData.bins.Length ? PlotData.bins.Length : _numTop);
                InternalPlot.Plot.Clear();
                InternalPlot.Plot.Frameless(false);
                InternalPlot.Configuration.UseRenderQueue = true;
                InternalPlot.Plot.AddBar(truncBins, truncPositions);
                InternalPlot.Plot.XTicks(truncPositions, truncLabels);
                InternalPlot.Plot.AxisAuto();
                InternalPlot.Refresh();
            }
            else
            {
                InternalPlot.Plot.Clear();
                var plt = InternalPlot.Plot;
                var truncBins = PlotData.bins.Slice(0, _numTop > PlotData.bins.Length ? PlotData.bins.Length : _numTop);
                var truncLabels = PlotData.labels.Slice(0, _numTop > PlotData.bins.Length ? PlotData.bins.Length : _numTop);
                if (_numTop < PlotData.bins.Length)
                {
                    truncBins = truncBins.Append( PlotData.bins.Slice(_numTop,PlotData.bins.Length).Sum((eee)=>eee)).ToArray();
                    truncLabels = truncLabels.Append("其他").ToArray();
                }
                var pie = plt.AddPie(truncBins);
                pie.SliceLabels = truncLabels;
                pie.ShowPercentages = true;
                //pie.ShowValues = true;
                pie.ShowLabels = true;
                InternalPlot.Plot.Legend();
                InternalPlot.Refresh();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void AdvancedPlotControl_Loaded(object sender, RoutedEventArgs e)
        {
            var adrnLayer = AdornerLayer.GetAdornerLayer(InternalPlot);
            if(adrnLayer != null)
            {
                adrnLayer.Add(new ResizeAdorner(InternalPlot));
            }
            //Console.Write(adrnLayer);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InternalPlot.Height = Double.NaN;
            InternalPlot.Width = Double.NaN;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InternalPlot.Height = Double.NaN;
            InternalPlot.Width = Double.NaN;
            if (InternalPlot.ActualWidth / InternalPlot.ActualHeight <= 4.0 / 3)
            {
                InternalPlot.Height = InternalPlot.ActualWidth * 3 / 4;
            }
            else
            {
                InternalPlot.Width = InternalPlot.ActualHeight * 4 / 3;
            }
        }
    }

    public class PlotData
    {
        public PlotData(string[] labels, double[] bins, double[] positions)
        {
            this.labels = labels;
            this.bins = bins;
            this.positions = positions;
        }

        public string[] labels { set; get; }
        public double[] bins { set; get; }
        public double[] positions { set; get; }
    }
}
