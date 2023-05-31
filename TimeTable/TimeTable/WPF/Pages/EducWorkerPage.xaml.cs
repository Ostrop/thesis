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

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EducWorkerPage.xaml
    /// </summary>
    public partial class EducWorkerPage : Page
    {
        public EducWorkerPage()
        {
            InitializeComponent();
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
