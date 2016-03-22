using ExpensesManager.DesktopUI.Code;
using ExpensesManager.Entities;
using ExpensesManager.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ExpensesManager.DesktopUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly SqlUsersRepository _userRepository;
        private string _connectionString = ConfigurationManager.ConnectionStrings["ExpensesManager connection string"].ConnectionString;


        public Login()
        {
            InitializeComponent();
            _userRepository = new SqlUsersRepository(_connectionString);

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var password = Encrypt.GetHash(tbPassword.Password);
            Users user = _userRepository.SelectUserByLogin(login, password);

            if (user == null)
            {
                MessageBox.Show(this, "Invalid user name or password", "Authentication Error");
            }
            else
            {
                CurrentUser.Initialize(user);
                this.DialogResult = true;
            }

        }

    }
}
