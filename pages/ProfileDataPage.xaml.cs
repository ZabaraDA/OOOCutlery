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
    public partial class ProfileDataPage : Page
    {
        TradeEntities1 tradeEntities = new TradeEntities1();
        public ProfileDataPage()
        {
            InitializeComponent();

            var lp = tradeEntities.User.Where(x => x.UserID.Equals(StaticDataClass.id)).FirstOrDefault();

            NameLabel.Content = lp.UserName;
            SurnameLabel.Content = lp.UserSurname;
            PatronymicLabel.Content = lp.UserPatronymic;
            RoleLabel.Content = lp.Role.RoleName;

        }
    }
}
