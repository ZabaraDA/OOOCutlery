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

namespace OOOCutlery.windows
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        TradeEntities tradeEntities = new TradeEntities();
        
        public MenuWindow()
        {
            InitializeComponent();
            // Найти строку БД по значению id, сохранённом в статичном классе
            var lp = tradeEntities.User.Where(x => x.UserID.Equals(StaticDataClass.id)).FirstOrDefault();
            NameLabel = lp.UserName; // Присвоить label значение имени из найденной строки БД
            SurnameLabel = lp.UserSurname; // Присвоить label значение фамилии из найденной строки БД

        }
    }
}
