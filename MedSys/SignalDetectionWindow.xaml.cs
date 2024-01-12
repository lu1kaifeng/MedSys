using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for SignalDetectionWindow.xaml
    /// </summary>
    public partial class SignalDetectionWindow : Window
    {
        List<med> DataList;
        public SignalDetectionWindow(List<med> dataList)
        {
            DataList = dataList;
            InitializeComponent();
        }
    }
}
