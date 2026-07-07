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
    public partial class Home : Page
    {
        ApiService api = new ApiService();
        public Home()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Helper.employee = null;
            AutorizationWindow aw = new AutorizationWindow();
            aw.Show(); Helper.mw.Close();
        }

        private async void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldPassword = Cryptor.Encrypt(txtOldPass.Password),
                       newPassword = txtNewPassword.Password,
                       newPassword2 = txtNewPasswordReturn.Password;

                if (Helper.employee.myPassword != oldPassword)
                    throw new Exception("Старый пароль неверен!");

                if (newPassword != newPassword2)
                    throw new Exception("Пароли не совпадают!");

                var emp = Helper.employee;

                emp.myPassword = Cryptor.Encrypt(newPassword);

                try
                {
                    await api.UpdateEmployeeAsync(Helper.employee.id, emp);
                }
                catch
                {

                }

                Helper.employee = emp;

                MessageBox.Show("Пароль изменен!", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Information);

                txtNewPassword.Clear();
                txtNewPasswordReturn.Clear();
                txtOldPass.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
