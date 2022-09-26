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
using System.Windows.Threading;

namespace OOOCutlery
{
  
    public partial class MainWindow : Window // Окно авторизации
    {
        TradeEntities1 tradeEntities = new TradeEntities1();
        DispatcherTimer dispatcherTimer = new DispatcherTimer(); // Для блокировки входа на определённое время нужно обратится к таймеру
        bool incorrectly = false; // Логическая переменная, которая определяет был ли допущен неверный ввод логина или пароля
        int timerTick = 10; // Время блокировки после неверного ввода 
        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1); // Определяет интервал выполнения для таймера ( в данный момент 1 секунда)
            dispatcherTimer.Tick += DispatcherTimer_Tick; // Каждая иттерация таймера после завершения интервала в 1 секунду обращается
                                                          // к методу DispatcherTimer_Tick
            TimerLabel.Content = timerTick.ToString();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Найти в БД строку с совпадающими логином и паролем
            var lp = tradeEntities.User.Where(x => x.UserLogin.Equals(LoginBox.Text) && x.UserPassword.Equals(PasswordBox.Password)).FirstOrDefault();
            
            if (lp == null) // Если совпадений нет, то FirstORDefault вернёт null
            {
                LoginBox.Text = ""; // Очистить поле ввода логина
                PasswordBox.Password = ""; // Очистить поле ввода пароля


                if (incorrectly == false) // Если ошибка ввода логина или пароля допущена 1 раз
                {
                    MessageBox.Show("Логин или пароль введены неверно: \n\n1) Проверьте правильность ввода\n\n2) Обратитесь к администратору", "Ошибка авторизации");
                    incorrectly = true; // Теперь вход только с каптчей
                }
                else if (incorrectly == true) //  Если ошибка ввода логина или пароля допущена более 1 раза
                {
                    AuthorizationStackPanel.Visibility = Visibility.Hidden; // Скрыть StackPanel, которая включает в себя LoginBox и PasswordBox
                    TimerStackPanel.Visibility = Visibility.Visible; // Сделать видимой StackPanel, которая включает в себя таймер
                    // Теперь нельзя ввести логин и пароль
                    dispatcherTimer.Start(); // Запускает таймер
                    
                }

            }
            else if(incorrectly == true) // Если ошибка ввода логина или пароля допущена более 1 раза, но логин и пароль введены верно
            {

                StaticDataClass.id = lp.UserID; // Присвоить переменной значение id равное UserID из найденной строки БД 
                StaticDataClass.role = lp.UserRole; // Запомнить роль пользователя, в данном коде не используется, можно применить для СУБД Workbench
                CreateCaptchaWindow createCaptchaWindow = new CreateCaptchaWindow();
                createCaptchaWindow.ShowDialog(); // Открыть окно с каптчей
                // Метод ShowDialog() заблокирует возможность взаимодействия c окном авторизации пока открыто окно с каптчей
                this.Close();
            }
            else if (incorrectly == false) // Если логин и пароль были верно введены с первого раза
            {
                StaticDataClass.id = lp.UserID; // Присвоить переменной значение id равное UserID из найденной строки БД 
                StaticDataClass.role = lp.UserRole; // Запомнить роль пользователя, в данном коде не используется, можно применить для СУБД Workbench
                MenuWindow menuWindow = new MenuWindow(); 
                menuWindow.Show(); // Открыть главное меню
                this.Close(); // И закрыть текущее окно
            }


        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Закрыть приложение
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); // Метод DragMove() позволяет двигать окно при нажатой клавише мыши
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e) // Метод содержит все операции, которые происходят при каждой иттерации таймера
        {
            TimerLabel.Content = timerTick.ToString(); // Вывести на экран текущее время до отключения блокировки
            timerTick--; // При каждой иттерации уменьшает значение на 1
            if(timerTick == 0) // Если 10 секунд ожидания закончились
            {
                dispatcherTimer.Stop(); // Остановить таймер
                timerTick = 10; // Вернуть изначальное значение переменной
                TimerStackPanel.Visibility = Visibility.Hidden; // Скрыть StackPanel, которая включает в себя таймер
                AuthorizationStackPanel.Visibility = Visibility.Visible; // Сделать видимой StackPanel, которая включает в себя LoginBox и PasswordBox
                                                                          // Теперь снова можно ввести логин и пароль
            }
        }

    }
}
