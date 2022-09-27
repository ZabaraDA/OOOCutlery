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
using OOOCutlery.csclasses;
using OOOCutlery.databases;

namespace OOOCutlery.pages
{
    public partial class ProfileDataPage : Page // Страница профиля пользователя
    {
        TradeEntities2 tradeEntities = new TradeEntities2();
        public ProfileDataPage()
        {
            InitializeComponent();

            var lp = tradeEntities.User.Where(x => x.UserID.Equals(StaticDataClass.id)).FirstOrDefault(); // Найти строку БД по значению id, сохранённом в статичном классе
            NameLabel.Content = lp.UserName; 
            SurnameLabel.Content = lp.UserSurname;
            PatronymicLabel.Content = lp.UserPatronymic;
            // Берём данные о пользователе из соответствующей id строки в БД
            RoleLabel.Content = lp.Role.RoleName;
            // Но с выводом роли пользователя так работать не получится
            // В строке роли пользователя указано число, а не текст, который будет понятен пользователю
            // Чтобы вывести наименование должности(или роль) необходимо обратится к таблице UserRole, где ханится id роли и её наименование
            // Это можно сделать с помощью связи двух таблиц в БД, User и UserRole (много пользователей с одной ролью - не совсем корректно, но по тз)
            // lp.Role.RoleName - таким образом обращаемся сначала к связанной таблице Role, а потом к включённом в неё столбцу RoleName (С Workbench будет работать иначе)
            /* Для Workbench можно заранее сохранить в статичном классе не только id пользователя, а ещё id роли(также можно хранить  наименование роли и ФИО пользователя)
             * Это позволит избежать повторного написания огромного массива кода для поиска данных
             * 
             * Важно брать наименование должностей именно из базы данных, а не программно назначать числам наименование должностей т.к
             * при добавление новой должности в базу данных прийдётся также менять программный код приложения(новые затраты ресурсов и времени),
             * Например ComboBox не должен выводить вручную введённые данные в коде, а читать их из базы данных
             */
        }
    }
}
