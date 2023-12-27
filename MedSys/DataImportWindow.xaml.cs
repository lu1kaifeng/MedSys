
using FastMember;
using Microsoft.Win32;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
            of.ShowDialog();
            HashSet<string> extraCol = null;
            HashSet<string> missingCol = null;
            if (of.FileName != null)
            {
                var result = MiniExcel.Query(of.FileName, useHeaderRow: true).Select((ee) => ConvertToObject<med>(ee, out extraCol, out missingCol)).Cast<med>().ToList();
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
                    Mouse.OverrideCursor = Cursors.Wait;
                    var entities = new medEntities();
                    entities.meds.AddRange(result);
                    entities.SaveChanges();
                    Mouse.OverrideCursor = null;
                    this.Paginator.ResetPage();
                }
            }

        }

        private T ConvertToObject<T>(IDictionary<string, object> rd, out HashSet<string> extraCol, out HashSet<string> missingCol) where T : class, new()
        {
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
                    string fieldName = e.Key;

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
    }
}
