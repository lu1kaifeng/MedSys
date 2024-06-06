using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Text.RegularExpressions;
using static MedSys.MedNameListView;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MedSys
{
    /// <summary>
    /// DataGridPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridPage : UserControl
    {
        public static readonly DependencyProperty PageProperty =
DependencyProperty.Register("Page",
  typeof(ObservableCollection<med>), typeof(DataGridPage),new PropertyMetadata());

        public ObservableCollection<med> Page
        {
            get
            {
                return (ObservableCollection<med>)GetValue(PageProperty);
            }
            set
            {
                SetValue(PageProperty, value);
            }

        }
        public DataGridPage()
        {
            InitializeComponent();
           
        }
        private medEntities db = new medEntities();
        //每页显示多少条
        private int pageNum = 10;
        //当前是第几页
        private int pIndex = 0;
        //最大页数
        private int MaxIndex = 1;
        //一共多少条
        private int allNum = 0;

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="dtt"></param>
        /// <param name="Num"></param>
        public void InitPage()
        {     
            SetMaxIndex();
            btnNext_MouseDown(null,null);
        }

        public void ResetPage()
        {
            this.pIndex = 0;
            SetMaxIndex();
            btnNext_MouseDown(null, null);
        }
        #endregion

        #region 画数据
        /// <summary>
        /// 画数据
        /// </summary>
        private void ReadDataTable()
        {
            try
            {
                page.Text = this.pIndex.ToString();
                countPage.Text = "页/共" + MaxIndex + "页";
                Page = null;
                Task.Run(() =>
                {
                    var arr = (from o in db.meds orderby o.ID select o).Skip(pageNum * (pIndex - 1)).Take(pageNum).ToArray();
                    Application.Current.Dispatcher.Invoke(() => {
                        Page = new ObservableCollection<med>(arr);
                    });
                });
                
            }
            catch(Exception e)
            {
                MessageBox.Show("错误");
            }
            finally
            {
                DisplayPagingInfo();
            }

        }
        #endregion

        #region 画每页显示等数据
        /// <summary>
        /// 画每页显示等数据
        /// </summary>
        private void DisplayPagingInfo()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush brush2 = new SolidColorBrush(Colors.Black);
            if (this.pIndex == 1)
            {
                this.btnPrev.IsEnabled = false;
                this.btnFirst.IsEnabled = false;
                this.btnPrev.Foreground = brush;
                this.btnFirst.Foreground = brush;
            }
            else
            {
                this.btnPrev.IsEnabled = true;
                this.btnFirst.IsEnabled = true;
                this.btnPrev.Foreground = brush2;
                this.btnFirst.Foreground = brush2;
            }
            if (this.pIndex == this.MaxIndex)
            {
                this.btnNext.IsEnabled = false;
                this.btnLast.IsEnabled = false;

                this.btnNext.Foreground = brush;
                this.btnLast.Foreground = brush;
            }
            else
            {
                this.btnNext.IsEnabled = true;
                this.btnLast.IsEnabled = true;
                this.btnNext.Foreground = brush2;
                this.btnLast.Foreground = brush2;
            }
            this.tbkRecords.Text = string.Format("每页{0}条/共{1}条", this.pageNum, this.allNum);
            int first = (this.pIndex - 4) > 0 ? (this.pIndex - 4) : 1;
            int last = (first + 9) > this.MaxIndex ? this.MaxIndex : (first + 9);
          //  this.grid.Children.Clear();
            //for (int i = first; i <= last; i++)
            //{
            //    ColumnDefinition cdf = new ColumnDefinition();
            //    this.grid.ColumnDefinitions.Add(cdf);
            //    TextBlock tbl = new TextBlock();
            //    tbl.Text = i.ToString();
            //    tbl.MouseLeftButtonUp += new MouseButtonEventHandler(tbl_MouseLeftButtonUp);
            //    if (i == this.pIndex)
            //        tbl.IsEnabled = false;
            //    Grid.SetColumn(tbl, this.grid.ColumnDefinitions.Count - 1);
            //    Grid.SetRow(tbl, 0);
            //    this.grid.Children.Add(tbl);
            //}
        }
        #endregion


        #region 设置最多大页面
        /// <summary>
        /// 设置最多大页面
        /// </summary>
        private void SetMaxIndex()
        {
            int rowCount = (from m in db.meds select m ).Count();
            //多少页
            int Pages = rowCount / pageNum;
            if (rowCount != (Pages * pageNum))
            {
                if (rowCount < (Pages * pageNum))
                    Pages--;
                else
                    Pages++;
            }
            this.MaxIndex = Pages;
            this.allNum = rowCount;
        }
        #endregion

        private void tbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tbl = sender as TextBlock;
            if (tbl == null)
                return;
            int index = int.Parse(tbl.Text.ToString());
            this.pIndex = index;
            if (index > this.MaxIndex)
                this.pIndex = this.MaxIndex;
            if (index < 1)
                this.pIndex = 1;
            ReadDataTable();
        }

        private void btnFirst_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.pIndex = 1;
            ReadDataTable();
        }

        private void btnPrev_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.pIndex <= 1)
                return;
            this.pIndex--;
            ReadDataTable();
        }

        private void btnNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.pIndex >= this.MaxIndex)
                return;
            this.pIndex++;
            ReadDataTable();
        }

        private void btnLast_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.pIndex = this.MaxIndex;
            ReadDataTable();
        }

       
       
        private void btnGO_Click(object sender, RoutedEventArgs e)
        {
            if (page.Text == "")
                return;
            if (Convert.ToInt32(page.Text) <= 1)
                return;
            this.pIndex = Convert.ToInt32(page.Text);
            ReadDataTable();
        }
    }
}
