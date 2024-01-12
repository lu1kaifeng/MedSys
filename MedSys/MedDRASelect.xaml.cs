using ScottPlot.MarkerShapes;
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
using static MedSys.MedNameListView;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for MedDRASelect.xaml
    /// </summary>
    public partial class MedDRASelect : UserControl
    {
        public MedDRASelect()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectionProperty =
    DependencyProperty.Register("Selection",
         typeof(string), typeof(MedDRASelect),new PropertyMetadata(""));

        public string Selection
        {
            get
            {
                return (string)GetValue(SelectionProperty);
            }
            set
            {
                SetValue(SelectionProperty, value);

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            tree.ItemsSource = MedDRAEntry.TreeViewItems;
            popUp.IsOpen = true;
            Mouse.OverrideCursor = null;
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Selection = (string)((TreeViewItem)tree.SelectedItem).Header;
            popUp.IsOpen = false;
        }
    }
}
