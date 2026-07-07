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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        ApiService api;
        Order _order = null;
        public ClientsPage()
        {
            InitializeComponent();
            api = new ApiService();

            if (Helper.employee.Position.position1 != "Менеджер")
            {
                btnAddClient.Visibility = Visibility.Collapsed;
            }

            ReloadList();
        }

        public ClientsPage(Order order) : this()
        {
            btnAddClient.Visibility = Visibility.Collapsed;
            _order = order;
            ReloadList();
        }

        async void ReloadList()
        {
            string searchedStr = txtSearch.Text.ToLower();

            lvClients.ItemsSource = null;
            var list = await api.GetClientsAsync();
            if (_order == null)
            {

                lvClients.ItemsSource = from c in list
                                        where c.address.ToLower().Contains(searchedStr) ||
                                              c.email.ToLower().Contains(searchedStr) ||
                                              c.INN.ToLower().Contains(searchedStr) ||
                                              c.OGRN.ToLower().Contains(searchedStr) ||
                                              c.phone.ToLower().Contains(searchedStr) ||
                                              c.recipientCompany.ToLower().Contains(searchedStr) ||
                                              c.recipientFullName.ToLower().Contains(searchedStr)
                                        select c;
            }
            else
            {
                lvClients.ItemsSource = from c in list.Where(x => x.recipientFullName != "Согласие отозвано")
                                        where c.address.ToLower().Contains(searchedStr) ||
                                              c.email.ToLower().Contains(searchedStr) ||
                                              c.INN.ToLower().Contains(searchedStr) ||
                                              c.OGRN.ToLower().Contains(searchedStr) ||
                                              c.phone.ToLower().Contains(searchedStr) ||
                                              c.recipientCompany.ToLower().Contains(searchedStr) ||
                                              c.recipientFullName.ToLower().Contains(searchedStr)
                                        select c;
            }
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            PrinterDocs.PersonData(Helper.employee.fullName);

            if (MessageBox.Show("Клиент дал согласие на обработку пресональных данных?", "Обработка ПД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Helper.frame.Navigate(new AddEditPages.AddEditClients());
            else
                MessageBox.Show("Извините, но без согласия продолжить нельзя", "Обработка ПД", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void lvClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Helper.employee.Position.position1 == "Менеджер")
            {
                if (_order == null)
                {
                    var client = lvClients.SelectedItem as Client;
                    Helper.frame.Navigate(new AddEditPages.AddEditClients(client));
                }
                else
                {
                    var client = lvClients.SelectedItem as Client;
                    _order.managerId = Helper.employee.id;
                    _order.recipientId = client.id;
                    _order.creationDate = DateTime.Now.ToString("dd.MM.yyyy");
                    _order.price = 0;
                    _order.orderStatus = "Создан";

                    Helper.frame.Navigate(new Pages.AddEditPages.AddOrder(_order, 0));
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadList();
        }

        private async void lvClients_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Helper.employee.Position.position1 == "Менеджер")
            {
                PrinterDocs.NoPersonData();

                if (MessageBox.Show("Клиент отозвал согласие на обработку пресональных данных?", "Обработка ПД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var client = lvClients.SelectedItem as Client;

                    client.recipientFullName = "Согласие отозвано";
                    client.phone = "Согласие отозвано";
                    client.OGRN = "-";
                    client.INN = "-";
                    client.phone = "Согласие отозвано";
                    client.email = "Согласие отозвано";

                    var ordersList = (await api.GetOrdersAsync()).Where(x => x.recipientId == client.id);

                    foreach (var o in ordersList)
                    {
                        if (o.orderStatus != "Завершён")
                        {
                            try
                            {
                                o.orderStatus = "Отменён";
                                await api.UpdateOrderAsync(o.id, o);
                            }
                            catch
                            {

                            }
                        }
                    }

                    try
                    {
                        await api.UpdateClientAsync(client.id, client);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
