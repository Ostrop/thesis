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

namespace TimeTable.WPF
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Функция отображения пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeePassword_Click(object sender, RoutedEventArgs e)
        {
            var SeePassword_checkBox = sender as CheckBox;
            if (SeePassword_checkBox.IsChecked.Value)
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Hidden;
                SeePassword_checkBox.Content = Resources["Image.See"];
            }
            else
            {
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Hidden;
                SeePassword_checkBox.Content = Resources["Image.SeeNot"];
            }
        }
    }
}
