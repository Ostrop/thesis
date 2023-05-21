using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace TimeTable
{
    public class Phone
    {
        public string Title { get; set; }
        public string buf1 { get; set; }
        public string buf2 { get; set; }
        public string buf3 { get; set; }
        public string buf4 { get; set; }
        public string buf5 { get; set; }
        public string buf6 { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Stop);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<Phone> phonesList = new List<Phone>
            //  {
            //         new Phone { Title="25331/2", Company="Программирование", Price="09.01.2022" },
            //         new Phone {Title="23643/5", Company="Робототехника", Price="09.01.2022" },
            //        new Phone {Title="42312/1", Company="Электроника", Price="09.01.2022" }
            //   };
            //phonesGrid.ItemsSource = phonesList;
        }
    }
}
