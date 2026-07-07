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
    /// Логика взаимодействия для AddEditClients.xaml
    /// </summary>
    public partial class AddEditClients : Page
    {
        ApiService api;
        Client _client = null;
        public AddEditClients()
        {
            InitializeComponent();
            api = new ApiService();
        }

        public AddEditClients(Client client) : this()
        {
            _client = client;
            DataContext = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";

                if (string.IsNullOrEmpty(FirmTextBox.Text)) 
                    error += "Вы не указали фирму\n";
                if (string.IsNullOrEmpty(NameTextBox.Text)) 
                    error += "Вы не указали ФИО\n";
                else if (NameTextBox.Text.Split(' ').Length != 3) 
                    error += "Вы указали некорректные ФИО\n";
                if (string.IsNullOrEmpty(AddressTextBox.Text)) 
                    error += "Вы не указали адрес\n";
                if (PhoneTextBox.Text.Length != 11 || !long.TryParse(PhoneTextBox.Text, out long phone)) 
                    error += "Вы указали некорректный номер телефона (Пример: 89000000000)\n";
                if (EmailTextBox.Text.Split('@').Length != 2 || EmailTextBox.Text.Split('@')[1].Split('.').Length != 2) 
                    error += "Вы указали некорректную почту (Пример: example-mail@gmail.com)\n";
                if (INNTextBox.Text.Length < 11 || INNTextBox.Text.Length > 13 || !long.TryParse(INNTextBox.Text, out long inn)) 
                    error += "Вы указали некорректный ИНН\n";
                if (OGRNTextBox.Text.Length != 13 || !long.TryParse(OGRNTextBox.Text, out long ogrn)) 
                    error += "Вы указали некорректный ОГРН";

                if (error != "")
                    throw new Exception(error);

                if (_client == null)
                {
                    Add();
                }
                else
                {
                    Update();
                }

                MessageBox.Show("Данные записаны!", "Запись данных", MessageBoxButton.OK, MessageBoxImage.Information);
                Helper.frame.Navigate(new Pages.ClientsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Запись данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Add()
        {
            try
            {
                _client = new Client();

                _client.recipientFullName = NameTextBox.Text;
                _client.recipientCompany = FirmTextBox.Text;
                _client.address = AddressTextBox.Text;
                _client.phone = PhoneTextBox.Text;
                _client.email = EmailTextBox.Text;
                _client.INN = INNTextBox.Text;
                _client.OGRN = OGRNTextBox.Text;
                _client.acceptScan = null;

                await api.CreateClientAsync(_client);
            }
            catch
            {
                
            }
        }

        private async void Update()
        {

            try
            {
                _client.recipientFullName = NameTextBox.Text;
                _client.recipientCompany = FirmTextBox.Text;
                _client.address = AddressTextBox.Text;
                _client.phone = PhoneTextBox.Text;
                _client.email = EmailTextBox.Text;
                _client.INN = INNTextBox.Text;
                _client.OGRN = OGRNTextBox.Text;
                _client.acceptScan = _client.acceptScan;

                await api.UpdateClientAsync(_client.id, _client);
            }
            catch
            {

            }
        }
    }
}
