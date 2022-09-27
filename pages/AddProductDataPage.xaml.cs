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
using Microsoft.Win32; // Для OpenFileDialog
using OOOCutlery.csclasses;
using OOOCutlery.databases;
using System.IO; //Для File.ReadAllBytes



namespace OOOCutlery.pages
{
    
    public partial class AddProductDataPage : Page // Страница добавления товара
    {
        TradeEntities2 tradeEntities = new TradeEntities2();
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
            
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e) // Кнопка добавления изображения
        {

            if (openFileDialog.ShowDialog() == true) // Открывает проводник и, если было выбрано изображение(Клик по изобрадению и кнопке Открыть или двойной клик по изображению) то                                       
            {
                ImageNameLabel.Content = openFileDialog.FileName; // В Label сохраняем путь к файлу в виде текста
                ProductImage.Source = new BitmapImage(new Uri( openFileDialog.FileName, UriKind.Absolute)); // Источником изображения для товара указываем путь к выбранному изображению
                // UriKind.Absolute используется т.к. изображение загружается извне
           
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)  // Очистить все поля ввода
        {
            NameBox.Text = null; 
            ArticleBox.Text = null;
            ImageNameLabel.Content = null;
            ProductImage.Source = null;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e) // Кнопка добавления нового товара
        {

            tradeEntities.Product.Add(new Product // Создать новую сущность(строку)
            {
                ProductArticleNumber = ArticleBox.Text, // Артикль товара
                ProductName = NameBox.Text, // Наименование товара
                ProductPhoto = File.ReadAllBytes(openFileDialog.FileName) // Импорт выбранного ранее изображения в БД
                // File.ReadAllBytes читает выбранное изображение по байтам (В базе данных изображение хранится массивом байт)
            });
            tradeEntities.SaveChanges(); // После добавления новой сущности сохранить изменения
           
        }

        private void ImagePrewievButton_Click(object sender, RoutedEventArgs e) // Показать изображения из базы данных
        {
            string i = PrewievImageBox.Text; // Переменная хранит артикль по которому выполняется поиск строки данного товара
            var lp = tradeEntities.Product.Where(x => x.ProductArticleNumber.Equals(i)).FirstOrDefault(); // Находит совпадение с введённым артиклем
            byte[] byteImage = lp.ProductPhoto; // Из найденной строки в переменную byteImage сохраняем изображение из найденной строки
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter(); // Отобразить изображение в виде массива байт напрямую не получится
                                                                                    // Для преобразования используется конвертер
            ProductImage.Source = (ImageSource)imageSourceConverter.ConvertFrom(byteImage); // Метод ConvertFrom() преобразует значения чтобы их можно было применить как источник изображения
            // В данном случае происходит явное преобразование массива байт
        }
    }
}

