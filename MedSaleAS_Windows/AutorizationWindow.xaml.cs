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
using System.Windows.Shapes;

namespace WPFModernVerticalMenu
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
        }

        // Start: Button Close | Restore | Minimize 
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // End: Button Close | Restore | Minimize

        private async void Autorizate()
        {
            try
            {
                string login = txtLogin.Text;
                string password = Cryptor.Encrypt(txtPassword.Password);

                // Создаем экземпляр ApiService
                ApiService apiService = new ApiService();

                // Получаем список сотрудников
                var empList = await apiService.GetEmployeesAsync();

                // Находим сотрудника по email
                Employee emp = empList.FirstOrDefault(x => x.email == login);
                if (emp == null || emp.myPassword != password)
                    throw new Exception("Сотрудник не найден");
                emp.Position = await apiService.GetPositionAsync(emp.positionId);

                if (emp.fullName == "Уволен")
                    throw new Exception("У вас нет прав доступа");

                Helper.employee = emp;

                MainWindow mw = new MainWindow();
                mw.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Autorizate();
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtLogin.IsFocused)
                {
                    txtPassword.Focus();
                }
                else if (txtPassword.IsFocused)
                {
                    Autorizate();
                }
            }
        }
    }
}
