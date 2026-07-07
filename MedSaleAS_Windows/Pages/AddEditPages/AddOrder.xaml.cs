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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        ApiService api;
        Order _order = null;
        int x = -1;
        public AddOrder(Order order, int x)
        {
            InitializeComponent();
            api = new ApiService();
            _order = order;
            btnAddProduct.Visibility = Visibility.Collapsed;
            dgProducts.Visibility = Visibility.Collapsed;
            this.x = x;
        }

        public AddOrder(Order order)
        {
            InitializeComponent();
            api = new ApiService();
            _order = order;
            txtNum.Text = order.orderNumber;
            txtNum.IsEnabled = false;
            PaymentMethodComboBox.Text = order.paymentMethod;
            if (order.orderStatus != "Создан")
            {
                PaymentMethodComboBox.IsEnabled = false;
                btnAddProduct.Visibility = Visibility.Collapsed;
            }
            ReloadList();
        }

        async void ReloadList()
        {
            dgProducts.ItemsSource = null;
            var iioList = (await api.GetItemsInOrderAsync()).Where(x => x.orderId == _order.id);

            foreach (var x in iioList)
            {
                var product = await api.GetProductAsync(int.Parse(x.productId.ToString()));
                x.Product = product;
            }

            dgProducts.ItemsSource = iioList;
        }

        private async void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            ItemsInOrder iio = (sender as Button).DataContext as ItemsInOrder;

            _order = await api.GetOrderAsync(int.Parse(iio.orderId.ToString()));

            _order.price = decimal.Parse(_order.price.ToString()) + decimal.Parse(iio.Product.price.ToString());
            iio.countProduct += 1;

            try
            {
                await api.UpdateOrderAsync(_order.id, _order);
                await api.UpdateItemInOrderAsync(iio.id, iio);
            }
            catch
            {

            }

            ReloadList();
        }

        private async void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            ItemsInOrder iio = (sender as Button).DataContext as ItemsInOrder;
            if (iio.countProduct == 1)
            {
                _order = await api.GetOrderAsync(int.Parse(iio.orderId.ToString()));

                _order.price = decimal.Parse(_order.price.ToString()) - decimal.Parse(iio.Product.price.ToString());
                
                try
                {
                    await api.DeleteItemInOrderAsync(iio.id);
                    await api.UpdateOrderAsync(_order.id, _order);
                }
                catch
                {

                }
            }
            else
            {
                _order = await api.GetOrderAsync(int.Parse(iio.orderId.ToString()));

                _order.price = decimal.Parse(_order.price.ToString()) - decimal.Parse(iio.Product.price.ToString());
                iio.countProduct -= 1;

                try
                {
                    await api.UpdateOrderAsync(_order.id, _order);
                    await api.UpdateItemInOrderAsync(iio.id, iio);
                }
                catch
                {

                }
            }
            ReloadList();
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (x == -1)
            {
                _order.paymentMethod = PaymentMethodComboBox.Text;
                try
                {
                    await api.UpdateOrderAsync(_order.id, _order);
                }
                catch
                {

                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtNum.Text))
                {
                    MessageBox.Show("ВЫ не ввели номер заказа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _order.orderNumber = txtNum.Text;
                _order.paymentMethod = PaymentMethodComboBox.Text;
                _order.contractSkan = null;
                try
                {
                    await api.CreateOrderAsync(_order);
                }
                catch
                {

                }
            }
            Helper.frame.Navigate(new Pages.OrdersPage());
        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var iioList = (await api.GetItemsInOrderAsync()).ToList().Where(x => x.orderId == _order.id);
            Helper.frame.Navigate(new Pages.Dashboard(iioList, _order));
        }
    }
}
