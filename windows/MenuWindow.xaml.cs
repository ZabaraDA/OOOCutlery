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
using OOOCutlery.databases;
using OOOCutlery.csclasses;
using OOOCutlery.pages;
using OOOCutlery.Properties;

namespace OOOCutlery.windows
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window // Окно главного меню
    {
        TradeEntities2 tradeEntities = new TradeEntities2();
        
        public MenuWindow()
        {
            InitializeComponent();
            // Найти строку БД по значению id, сохранённом в статичном классе
            var lp = tradeEntities.User.Where(x => x.UserID.Equals(StaticDataClass.id)).FirstOrDefault();
            NameLabel.Content = lp.UserName; // Присвоить label значение имени из найденной строки БД
            SurnameLabel.Content = lp.UserSurname; // Присвоить label значение фамилии из найденной строки БД
            MenuFrame.Navigate(new WelcomePage()); // При старте окна MenuWindow открывает в фрейме приветственную страницу
            RoleLabel.Content = lp.Role.RoleName; // Подробнее  на странице с профилем пользователя (ProfileDataPage)

            if (lp.UserRole == 1) // 1 - роль администратора, по тз выходит так, что ему доступен весь функционал системы
            {
                // Это значит что все кнопки по умолчанию будут активны (видимы в окне меню)
                // В иных проектах некоторые кнопки могут быть скрыты, если администратор не имеет доступа к этому функционалу по тз
                // Для проверки недоступного функционала СИСТЕМНЫЙ администратор может иметь аккаунты со всеми уровнями доступа (ролями в системе)
            }
            if(lp.UserRole == 4) //Роль менеджера в БД представлена цифрой 4
            {
                UserButton.Visibility = Visibility.Collapsed; // Скрыть менеджеру доступ к просмотру пользователей, просмотр доступен толькот администратору
                RegistrationUserButton.Visibility = Visibility.Collapsed; // Менеджер также не может создавать новые аккаунты
                AddProductsButton.Visibility = Visibility.Collapsed; // Менеджер также не может добавлять новые товары
            }    
            if(lp.UserRole == 7) //Роль пользователя в БД представлена цифрой 7
            {
                UserButton.Visibility = Visibility.Collapsed; // Скрыть пользователю доступ к просмотру пользователей, просмотр доступен толькот администратору
                RegistrationUserButton.Visibility = Visibility.Collapsed; // Пользователь также не может создавать новые аккаунты
                AddProductsButton.Visibility = Visibility.Collapsed; // Пользователь также не может добавлять новые товары

                // В окне авторизации для скрытия полей ввода использовано Visibility.Hidden (У элементов не было общего макета)
                // В случае с кнопками в меню Visibility.Hidden оставит пустое пространство на местах скрытых кнопок
                // Visibility.Collapsed помимо скрытия кнопки исключит её разметку из макета и пустого пространства не будет
            }
            // Представленная выше конструкция из условий позволяет разместить в одном окне меню(MenuWindow) функционал всех пользователей приложения
            // Однако можно поступить иначе
            /* Ролей в системе по тз - 3, и их также можно разделить на три окна. Но такой подход будет не совсем корректен, так как:
             * 1 - может появится неограниченное количество модулей (В данном случае ролей всего 3, но их может быть 10 и более. И каждому прийдётся создавать собственное окно);
             * 2 - произойдёт дублирование кода - атака клонов ( К примеру:каждому окну потребуется строка подключения к БД, в общем меню применяется единожды);
             * 3 - при изменении предметной области(появлении новых ролей) программисту прийдётся верстать новое окно, добавлять базовый функционал, а лишь потом писать новый.
             * 
             * Все эти моменты в совокупности увеличивают время разработки приложения, его итоговую стоимость, использование ресурсов системы приложением,
             *  а также усложняет её дальнейшее сопровождение и модернизацию. (Нарушение принципов Бережливого производства)
             */
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new ProfileDataPage());  // Пользователь может посматривать свой профиль
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new UserDataPage()); // Администратор может просматривать данные пользователей

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); // Окно можно двигать зажатой кнопкой мыши
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Завершить работу приложения
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Свернуть окно
        }
        private void ChangeAccountButton_Click(object sender, RoutedEventArgs e) // Кнопка Сменить аккаунт возвращает на окно авторизации
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new SettingsPage()); // Открыть страницу настроек
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new ProductDataPage()); // Открыть страницу с данными о товарах
        }

        private void AddProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new AddProductDataPage()); // Открыть страницу для добавления нового товара и редактирования имеющихся товаров
        }

        private void RegistrationUserButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new RegistrationUserPage()); // Открыть страницу для добавления новых пользователей и редактирования имеющихся учётных записей
        }
    }
}
