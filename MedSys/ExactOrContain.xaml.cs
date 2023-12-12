using MedSys.Converters;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedSys
{
    /// <summary>
    /// Interaction logic for ExactOrContain.xaml
    /// </summary>
    public partial class ExactOrContain : UserControl
    {
        public static readonly DependencyProperty IsExactProperty =
    DependencyProperty.Register("IsExact",
         typeof(bool), typeof(ExactOrContain), new PropertyMetadata(true));

        public static readonly DependencyProperty GroupNameProperty =
    DependencyProperty.Register("GroupName",
         typeof(string), typeof(ExactOrContain));
        public string GroupName
        {
            get
            {
                return (string)GetValue(GroupNameProperty);
            }
            set
            {
                SetValue(GroupNameProperty, value);

            }

        }

        public bool IsExact
        {
            get
            {
                return (bool)GetValue(IsExactProperty);
            }
            set
            {
                SetValue(IsExactProperty, value);

            }
        }
        RadioIntToBoolConverter radioBoolToIntConverter = new RadioIntToBoolConverter();
        public ExactOrContain()
        {
            InitializeComponent();
        }
    }
}
