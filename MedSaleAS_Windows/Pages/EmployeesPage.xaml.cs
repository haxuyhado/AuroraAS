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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        ApiService api;
        public EmployeesPage()
        {
            InitializeComponent();
            api = new ApiService();
            ReloadList();
        }

        async void ReloadList()
        {
            string searchedStr = txtSearch.Text.ToLower();

            lvEmployees.ItemsSource = null;
            var empList = await api.GetEmployeesAsync();
            foreach (var x in empList)
            {
                var position = await api.GetPositionAsync(x.positionId);
                x.Position = position;
            }
            lvEmployees.ItemsSource = MSDataEntities.GetContext().Employees
                .Where(e => e.fullName.ToLower().Contains(searchedStr) ||
                             e.address.ToLower().Contains(searchedStr) ||
                             e.phone.ToLower().Contains(searchedStr) ||
                             e.email.ToLower().Contains(searchedStr))
                .ToList();
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Helper.frame.Navigate(new AddEditPages.AddEditEmployee());
        }

        private void lvEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var employee = lvEmployees.SelectedItem as Employee;
            if (employee != null)
            {
                Helper.frame.Navigate(new AddEditPages.AddEditEmployee(employee));
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadList();
        }

        private async void lvEmployees_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var employee = lvEmployees.SelectedItem as Employee;
            if (employee != null && employee.id != Helper.employee.id)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var empToDelete = employee;

                        empToDelete.fullName = "Уволен";
                        empToDelete.myPassword = "";
                        empToDelete.address = "Уволен";
                        empToDelete.phone = "Уволен";
                        empToDelete.email = "Уволен";

                        await api.UpdateEmployeeAsync(employee.id, empToDelete);
                    }
                    catch
                    {

                    }

                    ReloadList();
                }
            }
        }
    }

}
