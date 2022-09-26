using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace OOOCutlery.windows
{
    /// <summary>
    /// Логика взаимодействия для CreateCaptchaWindow.xaml
    /// </summary>
    public partial class CreateCaptchaWindow : Window // Окно с капчей
    {
        Random random = new Random(); // Random позволяет сгенерировать случайное число для выбора определённой капчи
        string captchaContent; // Строка которая хранит текст капчи на представленном изображении

        public CreateCaptchaWindow()
        {
            InitializeComponent();

            CaptchaCreate(); // При запуске окна метод CaptchaCreate() случайным образом выводит капчу

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Если пользователь не хочет ввести капчу, то он может завершить работу приложения
        }
        private void CaptchaCreate() // Метод генерации капчи
        {

            int numberCaptcha = random.Next(1, 3); // Метод Next() генерирует заданное число в указанном диапазоне
            // В данном случае будет сгенерировано число 1 или 2 (число 3 не входит в диапазон)
            if(numberCaptcha == 1) // Если сгенерировано число 1
            {
                CaptchaImage.Source = new BitmapImage(new Uri("images/captcha1.png", UriKind.Relative)); // Определяет источник получаемого изображения
                // UriKind.Relative определяет, что адресс изображения относительный, т.е. оно расположенно внутри приложения
                captchaContent = "A0E6"; // Текст представленный на капче
            }
            else if (numberCaptcha == 2) // Если сгенерировано число 2
            {
                CaptchaImage.Source = new BitmapImage(new Uri("images/captcha2.png", UriKind.Relative));
                captchaContent = "M37S";
                // Таких повторений может быть больше
            }
        }

        private void CaptchaButton_Click(object sender, RoutedEventArgs e) // При нажатии на кнопку Готово
        {
            if(CaptchaBox.Text == captchaContent) // Если текст введённый пользователем соответствует содержимому капчи
            {
                
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show(); // Открыть окно главного меню
                this.Close(); // И закрыть текущее окно

            }
            else // Если пользователь решил капчу неверно
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show(); // Вернуть пользователя к окну авторизации
                // В окне авторизации будет активирован таймер блокировки на 10 секунд
                this.Close();
            }
        }

    }
}

