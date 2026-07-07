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
    /// Логика взаимодействия для AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Page
    {
        Employee _employee = null;
        ApiService api;
        public AddEditEmployee()
        {
            InitializeComponent();
            api = new ApiService();
            cbLoad();
        }

        async void cbLoad()
        {
            PositionComboBox.ItemsSource = await api.GetPositionsAsync();
        }

        public AddEditEmployee(Employee employee) : this()
        {
            _employee = employee;
            DataContext = employee;
            PasswordTextBox.Text = Cryptor.Decrypt(employee.myPassword);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";

                if (string.IsNullOrEmpty(FullNameTextBox.Text))
                    error += "Вы не указали ФИО\n";
                if (PositionComboBox.SelectedItem == null)
                    error += "Вы не выбрали должность\n";
                if (string.IsNullOrEmpty(AddressTextBox.Text))
                    error += "Вы не указали адрес\n";
                if (PhoneTextBox.Text.Length != 11 || !long.TryParse(PhoneTextBox.Text, out long phone))
                    error += "Вы указали некорректный номер телефона (Пример: 89000000000)\n";
                if (string.IsNullOrEmpty(EmailTextBox.Text) || EmailTextBox.Text.Split('@').Length != 2 || EmailTextBox.Text.Split('@')[1].Split('.').Length != 2)
                    error += "Вы указали некорректную почту (Пример: example-mail@gmail.com)\n";
                if (string.IsNullOrEmpty(PasswordTextBox.Text) || PasswordTextBox.Text.Length < 6)
                    error += "Пароль должен содержать не менее 6 символов\n";

                if (error != "")
                    throw new Exception(error);

                if (_employee == null)
                {
                    Add();
                }
                else
                {
                    Update();
                }

                MessageBox.Show("Данные записаны!", "Запись данных", MessageBoxButton.OK, MessageBoxImage.Information);
                Helper.frame.Navigate(new Pages.EmployeesPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Запись данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        async void Add()
        {
            try
            {
                _employee = new Employee();

                _employee.fullName = FullNameTextBox.Text;
                _employee.positionId = (int)PositionComboBox.SelectedValue;
                _employee.address = AddressTextBox.Text;
                _employee.phone = PhoneTextBox.Text;
                _employee.email = EmailTextBox.Text;
                _employee.myPassword = Cryptor.Encrypt(PasswordTextBox.Text);

                await api.CreateEmployeeAsync(_employee);
            }
            catch
            {

            }
        }

        async void Update()
        {
            try
            {
                _employee.fullName = FullNameTextBox.Text;
                _employee.positionId = (int)PositionComboBox.SelectedValue;
                _employee.address = AddressTextBox.Text;
                _employee.phone = PhoneTextBox.Text;
                _employee.email = EmailTextBox.Text;
                _employee.myPassword = Cryptor.Encrypt(PasswordTextBox.Text);

                await api.UpdateEmployeeAsync(_employee.id, _employee);
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
