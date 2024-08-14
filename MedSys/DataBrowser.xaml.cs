using Microsoft.Win32;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for DataBrowser.xaml
    /// </summary>
    public partial class DataBrowser : UserControl,INotifyPropertyChanged
    {
        public DataBrowser()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data",
                typeof(List<med>), typeof(DataBrowser), new PropertyMetadata(null, (depObj, arg) => {
                    ((DataBrowser)depObj).Data = (List<med>)arg.NewValue;
                }));

        public List<med> Data
        {
            get
            {
                return (List<med>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
                dataGrid.ItemsSource = value;
                //OnPropertyChanged(nameof(DisplayAllVisibility));
                OnPropertyChanged();
            }

        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void MyGrid_SelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            var myGrid = (DataGrid)sender;
            myGrid.Focus();
            if (myGrid.SelectedCells.Count == myGrid.Columns.Count * myGrid.Items.Count)
            {
                myGrid.SelectedItems.Clear();
            }
            else
            {
                myGrid.SelectAll();
            }

            e.Handled = true;
        }

        private void Delete_Selected(object sender, RoutedEventArgs e)
        {
            var IDList = new List<int>();
            foreach (var si in dataGrid.SelectedItems)
            {
                IDList.Add(((med)si).ID);
            }
            using (var ctx = new medEntities())
            {
                var selected = (from med in ctx.meds where IDList.Contains(med.ID) select med).ToList();
                ctx.meds.RemoveRange(selected);
                ctx.SaveChanges();
            }
        }

        private void Export_Selected(object sender, RoutedEventArgs e)
        {
            var IDList = new List<int>();
            foreach (var si in dataGrid.SelectedItems)
            {
                IDList.Add(((med)si).ID);
            }

            
            using (var ctx = new medEntities())
            {
                var selected = (from med in ctx.meds where IDList.Contains(med.ID) select med).ToList();
                SaveFileDialog of = new SaveFileDialog();
                of.Filter = "报表文件|*.xlsx";
                bool dialogResult = (bool)of.ShowDialog();
                if (dialogResult)
                {
                    MiniExcel.SaveAs(of.FileName, selected);
                }
            }
        }
    }
}
