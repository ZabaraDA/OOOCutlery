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
using System.Windows.Media; // Для ImageSourceConverter
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // Для OpenFileDialog
using OOOCutlery.csclasses;
using OOOCutlery.databases;
using System.IO; //Для File.ReadAllBytes



namespace OOOCutlery.pages
{
    
    public partial class AddProductDataPage : Page // Страница добавления товара
    {
        TradeEntities tradeEntities = new TradeEntities();
        OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true, Multiselect = false };
        // OpenFileDialog позволяет импортировать файлы в приложение через проводник
        // Filter определяет типы допустимых импортируемых файлов
        // ValidateNames = true допускает только допустимые имена файлов
        // Multiselect = false запрещает выбирать несколько элементов (на один товар одно изображение)

        /* Эти параметры можно представить так:
        openFileDialog.Multiselect = false;
        openFileDialog.ValidateNames = true;
        openFileDialog.Filter = "JPEG|*.jpg|PNG|*.png";
        */

    public AddProductDataPage()
        {
            InitializeComponent();
            var itemsManufacturerList = tradeEntities.Manufacturer.ToList();
            ManufacturerComboBox.DisplayMemberPath = "ManufacturerName";
            ManufacturerComboBox.ItemsSource = itemsManufacturerList.ToList();
            ManufacturerComboBox.SelectedIndex = 0;
            
            var itemsCategoryList = tradeEntities.Category.ToList();
            CategoryComboBox.DisplayMemberPath = "CategoryName";
            CategoryComboBox.ItemsSource = itemsCategoryList.ToList();
            CategoryComboBox.SelectedIndex = 0;

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e) // Кнопка добавления изображения
        {

            if (openFileDialog.ShowDialog() == true) // Открывает проводник и, если было выбрано изображение(Клик по изобрадению и кнопке Открыть или двойной клик по изображению) то                                       
            {
                //ImageNameLabel.Content = openFileDialog.FileName; 
                // В Label сохраняем путь к файлу в виде текста
                ProductImage.Source = new BitmapImage(new Uri( openFileDialog.FileName, UriKind.Absolute)); // Источником изображения для товара указываем путь к выбранному изображению
                // UriKind.Absolute используется т.к. изображение загружается извне
                
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)  // Очистить все поля ввода
        {
            NameTextBox.Text = null;
            ArticleTextBox.Text = null;
            ProductImage.Source = null;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e) // Кнопка добавления нового товара
        {
            try // Обработка исключений. Код try{} либо выполнится полностью, либо не выполнится вовсе(Если пользователь неверно ввёл данные)
            {   // Обработка исключений необходима для избежания некорректного завершения работы приложения из-за проблем ввода

                tradeEntities.Product.Add(new Product // Создать новую сущность(строку)
            {
                ProductArticleNumber = ArticleTextBox.Text, // Артикль товара
                ProductName = NameTextBox.Text, // Наименование товара
                ProductPhoto = File.ReadAllBytes(openFileDialog.FileName), // Импорт выбранного ранее изображения в БД
                // File.ReadAllBytes читает выбранное изображение по байтам (В базе данных изображение хранится массивом байт)
                ProductDescription = DescriptionTextBox.Text, // Описание товара
                ProductCost = Convert.ToDecimal(CostTextBox.Text), // Стоимость товара
                ProductQuantityInStock = Convert.ToInt32(QuantityInStockTextBox.Text), // Количество товара на складе при первом поступлении
                ProductCategory = CategoryComboBox.SelectedIndex + 1, // Категория товара
                ProductManufacturer = ManufacturerComboBox.SelectedIndex + 1 // Производитель товара
            }) ;
            tradeEntities.SaveChanges(); // После добавления новой сущности сохранить изменения
            }
            catch // Если при выполнении блока кода try{} произошла ошибка, то выполнить блок catch{}
            {
                MessageBox.Show("Проверьте корретность ввода", "Введены неверные данные",  MessageBoxButton.OK); // Сообщение об ошибке пользователю
            }
        }

        //private void ImagePrewievButton_Click(object sender, RoutedEventArgs e) // Показать изображения из базы данных
        //{
        //    string i = PrewievImageBox.Text; // Переменная хранит артикль по которому выполняется поиск строки данного товара
        //    var lp = tradeEntities.Product.Where(x => x.ProductArticleNumber.Equals(i)).FirstOrDefault(); // Находит совпадение с введённым артиклем
        //    byte[] byteImage = lp.ProductPhoto; // Из найденной строки в переменную byteImage сохраняем изображение из найденной строки
        //    ImageSourceConverter imageSourceConverter = new ImageSourceConverter(); // Отобразить изображение в виде массива байт напрямую не получится
        //                                                                            // Для преобразования используется конвертер
        //    ProductImage.Source = (ImageSource)imageSourceConverter.ConvertFrom(byteImage); // Метод ConvertFrom() преобразует значения чтобы их можно было применить как источник изображения
        //    // В данном случае происходит явное преобразование массива байт
        //}
    }
}

