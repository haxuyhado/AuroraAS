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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        ApiService api = new ApiService();
        public OrdersPage()
        {
            InitializeComponent();
            ReloadOrders();
        }

        async void ReloadOrders()
        {
            lvOrders.ItemsSource = null;
            IEnumerable<Order> orderList = null;
            if (Helper.employee.Position.position1 == "Менеджер")
                orderList = (await api.GetOrdersAsync()).Where(x => x.managerId == Helper.employee.id);
            else if (Helper.employee.Position.position1 == "Бухгалтер")
                orderList = (await api.GetOrdersAsync()).Where(x => x.orderStatus == "Ожидает оплаты");
            else if (Helper.employee.Position.position1 == "Складовщик")
                orderList = (await api.GetOrdersAsync()).Where(x => x.orderStatus == "Собирается" || x.orderStatus == "Отправлена заявка на производство" || x.orderStatus == "Ожидает самовывоза");
            else if (Helper.employee.Position.position1 == "Доставщик")
                orderList = (await api.GetOrdersAsync()).Where(x => x.orderStatus == "Доставляется");

            foreach (var o in orderList)
            {
                o.Client = await api.GetClientAsync(int.Parse(o.recipientId.ToString()));
                o.Employee = await api.GetEmployeeAsync(int.Parse(o.managerId.ToString()));
                o.Employee.Position = await api.GetPositionAsync(o.Employee.positionId);
            }

            string searchStr = txtSearch.Text.ToLower();

            orderList = from o in orderList
                        where o.orderNumber.ToLower().Contains(searchStr) ||
                              o.orderStatus.ToLower().Contains(searchStr) ||
                              o.paymentMethod.ToLower().Contains(searchStr) ||
                              o.price.ToString().ToLower().Contains(searchStr) ||
                              DateTime.Parse(o.creationDate).ToString("dd.MM.yyy").ToLower().Contains(searchStr) ||
                              o.Client.recipientCompany.ToLower().Contains(searchStr) ||
                              o.Client.recipientFullName.ToLower().Contains(searchStr)
                        select o;

            lvOrders.ItemsSource = orderList;
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            Helper.frame.Navigate(new Pages.ClientsPage(new Order()));
        }

        private void lvOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Helper.frame.Navigate(new Pages.AddEditPages.AddOrder(lvOrders.SelectedItem as Order));
        }

        private void lvOrders_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Helper.employee.Position.position1 == "Менеджер")
            {
                var order = lvOrders.SelectedItem as Order;

                switch (order.orderStatus)
                {
                    case "Создан":
                        if (MessageBox.Show("Передать в бухгалтерию?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var client = MSDataEntities.GetContext().Clients.Find(order.recipientId);

                            PrinterDocs.DKP(client.recipientCompany, client.recipientFullName, client.address, client.phone, client.INN, client.OGRN);

                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Ожидает оплаты";
                                db.SaveChanges();

                                var iioList = db.ItemsInOrders.ToList().Where(x => x.orderId == order.id);

                                foreach (var item in iioList)
                                {
                                    var product = db.Products.Find(item.productId);

                                    int count = int.Parse(product.countProduct.ToString()) - int.Parse(item.countProduct.ToString());

                                    if (count < 0)
                                    {
                                        product.countProduct = 0;

                                        CreateProduct cp = new CreateProduct();

                                        cp.countProduct = count * -1;
                                        cp.forOrder = order.id;
                                        cp.productId = product.id;
                                        cp.status = "Выполняется";

                                        db.CreateProducts.Add(cp);
                                    }
                                    else
                                    {
                                        product.countProduct = count;
                                    }
                                    db.SaveChanges();
                                }
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    case "Оплачено":
                        if (MSDataEntities.GetContext().CreateProducts.Count(x => x.forOrder == order.id && x.status == "Выполняется") == 0)
                        {
                            if (MessageBox.Show("Передать в сборку?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                using (MSDataEntities db = new MSDataEntities())
                                {
                                    var order2 = db.Orders.Find(order.id);

                                    order2.orderStatus = "Собирается";
                                    db.SaveChanges();
                                }
                                MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                                ReloadOrders();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Передать в сборку?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                using (MSDataEntities db = new MSDataEntities())
                                {
                                    var order2 = db.Orders.Find(order.id);

                                    order2.orderStatus = "Отправлена заявка на производство";
                                    db.SaveChanges();
                                }
                                MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                                ReloadOrders();
                            }
                        }
                        break;
                    case "Собран":
                        if (MessageBox.Show("Самовывоз?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Ожидает самовывоза";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        else if (MessageBox.Show("Передать в доставку?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Доставляется";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    case "Ожидает самовывоза":
                    case "Доставляется":
                        if (MessageBox.Show("Завершить заказ?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Завершён";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (Helper.employee.Position.position1 == "Бухгалтер")
            {
                var order = lvOrders.SelectedItem as Order;

                switch (order.orderStatus)
                {
                    case "Ожидает оплаты":
                        if (MessageBox.Show("Заказ оплачен?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Оплачено";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (Helper.employee.Position.position1 == "Складовщик")
            {
                var order = lvOrders.SelectedItem as Order;
                switch (order.orderStatus)
                {
                    case "Собирается":
                        if (MessageBox.Show("Заказ собран?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Собран";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    case "Ожидает самовывоза":
                        if (MessageBox.Show("Завершить заказ?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (MSDataEntities db = new MSDataEntities())
                            {
                                var order2 = db.Orders.Find(order.id);

                                order2.orderStatus = "Завершён";
                                db.SaveChanges();
                            }
                            MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                            ReloadOrders();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (Helper.employee.Position.position1 == "Доставщик")
            {
                var order = lvOrders.SelectedItem as Order;

                if (order.orderStatus == "Доставляется" && MessageBox.Show("Завершить заказ?", "Изменение статуса", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (MSDataEntities db = new MSDataEntities())
                    {
                        var order2 = db.Orders.Find(order.id);

                        order2.orderStatus = "Завершён";
                        db.SaveChanges();
                    }
                    MSDataEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                    ReloadOrders();
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintOrders po = new PrintOrders();
            po.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadOrders();
        }
    }
}
