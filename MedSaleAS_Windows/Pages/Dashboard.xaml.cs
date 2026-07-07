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

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        ApiService api;
        IEnumerable<ItemsInOrder> _iioList = null;
        Order _order = null;
        public Dashboard()
        {
            InitializeComponent();
            api = new ApiService();

            if (Helper.employee.Position.position1 != "Складовщик")
            {
                btnAddProduct.Visibility = Visibility.Collapsed;
            }

            ReloadList();
        }

        public Dashboard(IEnumerable<ItemsInOrder> iioList, Order order)
        {
            InitializeComponent();
            api = new ApiService();
            _iioList = iioList;
            _order = order;
            ReloadList();
        }

        async void ReloadList()
        {
            if (_iioList == null)
            {
                lvProducts.ItemsSource = null;
                lvProducts.ItemsSource = await api.GetProductsAsync();
            }
            else
            {
                lvProducts.ItemsSource = null;
                var prodList = await api.GetProductsAsync();

                List<Product> sortedProductList = new List<Product>();
                foreach (var p in prodList)
                {
                    int koef = 0;
                    foreach (var iio in _iioList)
                    {
                        if (p.id == iio.productId)
                            koef += 1;
                    }
                    if (koef == 0)
                        sortedProductList.Add(p);
                }
                lvProducts.ItemsSource = sortedProductList;
            }
        }

        private async void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Helper.employee.Position.position1 == "Складовщик" && _iioList == null)
            {
                var product = lvProducts.SelectedItem as Product;

                Helper.frame.Navigate(new AddEditPages.AddEditProduct(product));
            }
            else if (_iioList != null)
            {
                try
                {
                    await api.CreateItemInOrderAsync(new ItemsInOrder
                    {
                        productId = (lvProducts.SelectedItem as Product).id,
                        orderId = _order.id,
                        countProduct = 1
                    });
                }
                catch
                {

                }

                try
                {
                    _order.price = decimal.Parse(_order.price.ToString()) + decimal.Parse((lvProducts.SelectedItem as Product).price.ToString());
                    await api.UpdateOrderAsync(_order.id, _order);
                }
                catch
                {

                }
                Helper.frame.Navigate(new Pages.AddEditPages.AddOrder(_order));

            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Helper.frame.Navigate(new AddEditPages.AddEditProduct());
        }
    }
}
