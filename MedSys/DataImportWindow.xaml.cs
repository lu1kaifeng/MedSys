
using FastMember;
using Microsoft.Win32;
using MiniExcelLibs;
using NumSharp.Utilities;
using ScottPlot.Palettes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

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
            set
            {
                _page = value;
                OnPropertyChanged();
                OnPropertyChanged("Loaded");
            }
        }

        private string _rangeSelectTxt = "";
        public string RangeSelectTxt
        {
            get { return _rangeSelectTxt; }
            set { 
                _rangeSelectTxt = value;
                OnPropertyChanged(); 
            }
        }

        public HashSet<int> SelectRange
        {
            get
            {
                var set = new HashSet<int>();
                foreach (var item in RangeSelectTxt.Split(';'))
                {
                    if(item == string.Empty)
                    {
                        continue;
                    }
                    
                    var splitted = item.Split('-');
                    if(splitted.Length > 1) {
                        var start = long.Parse(splitted[0]);
                        var end = long.Parse(splitted[1]);
                        if(start <= end)
                        {
                            set.UnionWith(Enumerable.Range((int)(start), (int)end - (int)start + 1));
                        }
                        else
                        {
                            set.UnionWith(Enumerable.Range((int)(end), (int)start- (int)end + 1));
                        }
                    }
                    else{
                        set.Add(int.Parse(item));
                    }
                    
                }
                return set;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "报表文件|*.xlsx";
            of.Multiselect = false;
            bool ifComplete = (bool)of.ShowDialog();
            HashSet<string> extraCol = null;
            HashSet<string> missingCol = null;
            
            if (ifComplete && of.FileName != null)
            {
                var excelDialog = new ExcelColSelectDialog(
                    (MiniExcel.Query(of.FileName, useHeaderRow: true).First() as IDictionary<string, object>).Keys);
                bool diagResult =(bool) excelDialog.ShowDialog();
                var alias = excelDialog.Alias;
                if (!diagResult)
                {
                    return;
                }
                var result = MiniExcel.Query(of.FileName, useHeaderRow: true).Select((ee) => ConvertToObject<med>(ee, out extraCol, out missingCol,alias)).Cast<med>().ToList();
                string message = "即将导入" + result.Count.ToString() + "条条目;\n";
                if (extraCol.Count == 0)
                {
                    message += "无多余列；\n";
                }
                else
                {
                    message += "发现多余列：";
                    foreach (var ec in extraCol)
                    {
                        message += "\n" + ec.ToString() + "";
                    }
                    message += "；";
                }
                if (missingCol.Count == 0)
                {
                    message += "无缺失列；\n";
                }
                else
                {
                    message += "发现缺失列：";
                    foreach (var mc in missingCol)
                    {
                        message += "\n" + mc.ToString() + "";
                    }
                    message += "；";
                }
                var res = MessageBox.Show(message, "确认导入", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        var entities = new medEntities();
                        entities.meds.AddRange(result);
                        entities.SaveChanges();
                        Mouse.OverrideCursor = null;
                        this.Paginator.ResetPage();
                    }
                    catch (DataException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                }
            }

        }

        

        private T ConvertToObject<T>(IDictionary<string, object> rd, out HashSet<string> extraCol, out HashSet<string> missingCol, IDictionary<string, string> alias) where T : class, new()
        {
            IDictionary<string, object> rd1 = new Dictionary<string, object>();
            foreach (var r in rd)
            {
                rd1.Add(alias.ContainsKey(r.Key) ? alias[r.Key] : r.Key, r.Value);
            }

            rd = rd1;
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();
            extraCol = new HashSet<string>();
            missingCol = t.GetType()
                     .GetProperties()
                     .Select(field => field.Name)
                     .Where((e) => !rd.Keys.ToHashSet().Contains(e)).ToHashSet();
            missingCol.Remove("ID");
            foreach (var e in rd)
            {
                if (e.Value != string.Empty)
                {
                    string fieldName = e.Key;//alias.ContainsKey(e.Key) ? alias[e.Key]:e.Key;

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {

                        accessor[t, fieldName] = e.Value;
                    }
                    else
                    {
                        extraCol.Add(fieldName);
                    }
                }
            }
            return t;
        }

        private void MatchingStrBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var m =  new Regex("^((\\d+)?(-?)(\\d+)?)$");
            var list = MatchingStrBox.Text.Insert(MatchingStrBox.CaretIndex,e.Text).Split(';');
            var match = true;
            foreach (var item in list)
            {
                match &= m.IsMatch(item);
            }
            e.Handled = !match;
        }


        private void MatchingStrBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            int k = 0;
            int newCaret = 0;
            switch (e.Key)
            {
                case Key.Back: case Key.Delete:
                    foreach (var item in MatchingStrBox.Text)
                    {
                        if (k == MatchingStrBox.CaretIndex)
                        {
                            break;
                        }
                        if (item == ';')
                        {
                            i++;
                            newCaret = k;
                        }
                        k++;
                    }
                    vm.RangeSelectTxt = String.Join(";", MatchingStrBox.Text.Split(';').RemoveAt(i));
                    MatchingStrBox.CaretIndex = newCaret;
                    e.Handled = true;
                    break;
            }
        }

        private void Delete_Selected(object sender, RoutedEventArgs e)
        {
            using (var ctx = new medEntities())
            {
                var selected = (from med in ctx.meds where vm.SelectRange.Contains(med.ID) select med).ToList();
                ctx.meds.RemoveRange(selected);
                ctx.SaveChanges();
            }
        }

        private void Export_Selected(object sender, RoutedEventArgs e)
        {
            using (var ctx = new medEntities())
            {
                var selected = (from med in ctx.meds where vm.SelectRange.Contains(med.ID) select med).ToList();
                SaveFileDialog of = new SaveFileDialog();
                of.Filter = "报表文件|*.xlsx";
                bool dialogResult  = (bool)of.ShowDialog();
                if (dialogResult)
                {
                    MiniExcel.SaveAs(of.FileName, selected);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("清空所有数据", "危险：请确认清除所有数据", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var med = new medEntities();

                med.meds.SqlQuery("delete from meds");
                using (var sc = med.Database.Connection)
                using (var cmd = sc.CreateCommand())
                {
                    sc.Open();
                    cmd.CommandText = "DELETE FROM med";
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
        }
    }
}
