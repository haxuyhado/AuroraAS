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

namespace WPFModernVerticalMenu.Pages.AddEditPages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Page
    {
        ApiService api;
        Product _product = null;
        public AddEditProduct()
        {
            InitializeComponent();
            api = new ApiService();
        }

        public AddEditProduct(Product product) : this()
        {
            _product = product;
            DataContext = product;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";

                if (string.IsNullOrEmpty(ProductNameTextBox.Text))
                    error += "Введите наименование продукта!\n";
                if (!int.TryParse(CountProductTextBox.Text.Replace('.', ','), out int count) || count < 0)
                    error += "Введите корректное число продукции\n";
                if (!decimal.TryParse(PriceTextBox.Text.Replace('.', ','), out decimal price) || price < 0)
                    error += "Введите корректную цену\n";

                if (error != "")
                    throw new Exception(error);

                if (_product == null)
                {
                    Add(count, price);
                }
                else
                {
                    Update(count, price);
                }
                MessageBox.Show("Данные записаны!", "Запись", MessageBoxButton.OK, MessageBoxImage.Information);
                Helper.frame.Navigate(new Pages.Dashboard());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Запись", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        async void Add(int count, decimal price)
        {
            try
            {
                _product = new Product();

                _product.productName = ProductNameTextBox.Text;
                _product.price = price;
                _product.countProduct = count;
                if (string.IsNullOrEmpty(MainCharacteristicsTextBox.Text))
                    _product.mainCharacteristics = null;
                else
                    _product.mainCharacteristics = MainCharacteristicsTextBox.Text;

                await api.CreateProductAsync(_product);
            }
            catch
            {

            }
        }

        async void Update(int count, decimal price)
        {
            try
            {
                _product.productName = ProductNameTextBox.Text;
                _product.price = price;
                _product.countProduct = count;
                if (string.IsNullOrEmpty(MainCharacteristicsTextBox.Text))
                    _product.mainCharacteristics = null;
                else
                    _product.mainCharacteristics = MainCharacteristicsTextBox.Text;

                await api.UpdateProductAsync(_product.id, _product);
            }
            catch
            {

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Helper.frame.NavigationService.GoBack();
        }

    }
}
