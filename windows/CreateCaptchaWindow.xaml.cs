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
    public partial class CreateCaptchaWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Random random = new Random();       
        string captchaContent;
        int timerTick = 10;

        public CreateCaptchaWindow()
        {
            InitializeComponent();

            CaptchaCreate();

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void CaptchaCreate()
        {

            int numberCaptcha = random.Next(1, 3);

            if(numberCaptcha == 1)
            {
                CaptchaImage.Source = new BitmapImage(new Uri("images/captcha1.png", UriKind.Relative));
                captchaContent = "A0E6";
            }
            else if (numberCaptcha == 2)
            {
                CaptchaImage.Source = new BitmapImage(new Uri("images/captcha2.png", UriKind.Relative));
                captchaContent = "M37S";
            }
        }

        private void CaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            if(CaptchaBox.Text == captchaContent)
           this.Close();
            else
            {
                CaptchaBox.Text = "";
                CaptchaStackPanel.Visibility = Visibility.Hidden;
                TimerStackPanel.Visibility = Visibility.Visible;
                
                
                dispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
                dispatcherTimer.Start();
                while(timerTick != 0)
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                TimerLabel.Content = timerTick.ToString();             


                CaptchaCreate();
            }
        }
        private void DispatcherTimer_Tick(object sender ,EventArgs e)
        {
            TimerLabel.Content = timerTick.ToString();
            timerTick--;
            
        }
    }
}










