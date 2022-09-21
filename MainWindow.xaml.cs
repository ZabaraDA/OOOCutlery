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
using OOOCutlery.windows;
using OOOCutlery.csclasses;
using OOOCutlery.databases;

namespace OOOCutlery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TradeEntities tradeEntities = new TradeEntities();  
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Найти в БД строку с совпадающими логином и паролем
            var lp = tradeEntities.User.Where(x => x.UserLogin.Equals(LoginBox.Text) && x.UserPassword.Equals(PasswordBox.Password)).FirstOrDefault();
            
            if (lp == null) // Если совпадений нет, то FirstORDefault вернёт null
            {
                MessageBox.Show("ERROR", "ERROR", MessageBoxButton.OK);
            }
            else 
            {
                StaticDataClass.id = lp.UserID; // Присвоить переменной значение id равное UserID из найденной строки БД
                MenuWindow window = new MenuWindow();
                window.Show();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
