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
using OOOCutlery.databases;


namespace OOOCutlery.pages
{
    
    public partial class ProductDataPage : Page // Страница для просмотра товаров
    {
        TradeEntities tradeEntities2 = new TradeEntities();
       
        public ProductDataPage()
        {
            InitializeComponent();


            var itemsManufacturerList = tradeEntities2.Manufacturer.ToList(); // Прочитать(ToList()) все элементы таблицы Manufacturer(Производители) и сохранить в переменную itemsManufacturerList          
            // Для правильной фильтрации товаров необходимо создать строку с индексом(id) 0, которая отменит фильтрацию по производителям(Значение по умолчанию)
            itemsManufacturerList.Insert(0, new Manufacturer 
            {
                ManufacturerName = "Все производители" // Первый элемент ManufacturerSearchComboBox с индексом 0
            });
            
            ManufacturerSearchComboBox.DisplayMemberPath = "ManufacturerName"; // Прочтитать наименования производителей из таблицы Manufacturer
            ManufacturerSearchComboBox.ItemsSource = itemsManufacturerList.ToList(); // Источник данных для ManufacturerSearchComboBox
            ManufacturerSearchComboBox.SelectedIndex = 0; // При инициализации выбрать первый элемент списка (Все производители)

            var itemsCategoryList = tradeEntities2.Category.ToList(); 
            itemsCategoryList.Insert(0, new Category
            {
                CategoryName = "Все категории"
            });

            CategorySearchComboBox.DisplayMemberPath = "CategoryName";
            CategorySearchComboBox.ItemsSource = itemsCategoryList.ToList();
            CategorySearchComboBox.SelectedIndex = 0;
            // Тот же способ для инициализации элементов CategorySearchComboBox(Категории товаров)

            SearchProductDataUpdate(); // Вызвать метод, который выполняет фильтрацию данных и вывод в ListView
        }


        private void DeleteProductButton_Click(object sender, RoutedEventArgs e) // Кнопка удаления товара
        {
            var productItem = ProductListView.SelectedItem as Product; // Сохранить в переменную выбранный товар

            tradeEntities2.Product.Remove(productItem); // Удалить товар из таблицы базы данных

            tradeEntities2.SaveChanges(); // Сохранить изменения в БД
            SearchProductDataUpdate(); // Вызвать метод, который выполняет фильтрацию данных и вывод в ListView

        }
        private void SearchProductDataUpdate()
        {
            var itemProduct = tradeEntities2.Product.ToList(); // Записать в переменную все записи товаров из БД

            if (NameSearchTextBox.Text != null && NameSearchTextBox.Text != "") // Если фильтр по имени не равен null, то
                itemProduct = itemProduct.Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();
            // Прочитать только те элементы в которых есть совпадения с введённым текстом
            // ToLower() возвращает строку в нижнем регистре ( "ложка" вместо "Ложка" для корректного поиска
            if (ManufacturerSearchComboBox.SelectedIndex > 0) // Если для фильтрации выбран конкретный производитель (Не "Все призводители"), то
            {
                itemProduct = itemProduct.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).ToList();
                // Вывести все товары от этого производителя
            }
            if (CategorySearchComboBox.SelectedIndex > 0) // Если для фильтрации выбрана конкретная категория (Не "Все категории"), то
            {
                itemProduct = itemProduct.Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).ToList();
                // Вывести все товары выбранной категории
            }

            ProductListView.ItemsSource = itemProduct.ToList(); // Вывести отфильтрованные товары в KistView

           
        }

        private void ChangeParametersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e) // Кнопка для перехода на страницу добавления нового товара
        {
            NavigationService.Navigate(new AddProductDataPage()); // Вывести страницу для добавления нового товара
            // NavigationService позволяет обратиться к frame, который инициализировал текущую страницу
        }

        private void NameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e) // TextChanged выполняет код при кажом изменении NameSearchTextBox
        {                                                                                 // (Изменение вводимого имени товара для фильтрации и поиска)
            SearchProductDataUpdate(); // Вызвать метод, который выполняет фильтрацию данных и вывод в ListView
            
        }

     

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e) // Сбросить все фильтры поиска
        {
            ManufacturerSearchComboBox.SelectedIndex = 0; // "Все производители"
            CategorySearchComboBox.SelectedIndex = 0; // "Все производители"
            NameSearchTextBox.Text = null; // Все названия
            SearchProductDataUpdate();
        }

        private void CategorySearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // При каждом изменение CategorySearchComboBox выполнить код
        {
            SearchProductDataUpdate(); // Вызвать метод, который выполняет фильтрацию данных и вывод в ListView
        }

        private void ManufacturerSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // При каждом изменение ManufacturerSearchComboBox выполнить код
        {
            SearchProductDataUpdate(); // Вызвать метод, который выполняет фильтрацию данных и вывод в ListView
        }
    }
}

//if (NameSearchTextBox.Text != null && NameSearchTextBox.Text != "")
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();
//if (ManufacturerSearchComboBox.SelectedIndex > 0)
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).ToList();
//}
//if (CategorySearchComboBox.SelectedIndex > 0)
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).ToList();
//}


//if (CategorySearchComboBox.SelectedIndex > 0 && ManufacturerSearchComboBox.SelectedIndex > 0 && NameSearchTextBox.Text != null && NameSearchTextBox.Text != "")
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();
//}
//else if (CategorySearchComboBox.SelectedIndex > 0 && NameSearchTextBox.Text != null && NameSearchTextBox.Text != "")
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();
//}
//else if (CategorySearchComboBox.SelectedIndex > 0 && ManufacturerSearchComboBox.SelectedIndex > 0)
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).ToList();
//}
//else if (ManufacturerSearchComboBox.SelectedIndex > 0 && NameSearchTextBox.Text != null && NameSearchTextBox.Text != "")
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();
//}
//else if (ManufacturerSearchComboBox.SelectedIndex > 0)
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductManufacturer.Equals(ManufacturerSearchComboBox.SelectedIndex)).ToList();
//}
//else if (CategorySearchComboBox.SelectedIndex > 0)
//{
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductCategory.Equals(CategorySearchComboBox.SelectedIndex)).ToList();
//}
//else if (NameSearchTextBox.Text != null && NameSearchTextBox.Text != "")
//    ProductListView.ItemsSource = tradeEntities2.Product.Where(x => x.ProductName.ToLower().Contains(NameSearchTextBox.Text.ToLower())).ToList();