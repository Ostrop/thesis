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

namespace TimeTable.WPF.Controllers
{
    public partial class NotificationControl : UserControl
    {

        public static readonly DependencyProperty NotificationTextProperty =
            DependencyProperty.Register("NotificationText", typeof(string), typeof(NotificationControl));

        public string NotificationText
        {
            get { return (string)GetValue(NotificationTextProperty); }
            set { SetValue(NotificationTextProperty, value); }
        }


        /// <summary>
        /// Логика взаимодействия для NotificationControl.xaml
        /// </summary>
        public NotificationControl()
        {
            InitializeComponent();
        }
    }
}