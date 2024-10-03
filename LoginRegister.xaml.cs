using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class LoginRegister : Window
    {
        public LoginRegister(string userName)
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = txbLoginUsername.Text;
            string password = pswLoginPassword.Password;

            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Password FROM Users WHERE Username = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", userName);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string dbPassword = reader.GetString(0);

                            if (string.Equals(password, dbPassword))
                            {
                                MessageBox.Show("Login successful!");
                            }
                            else
                            {
                                MessageBox.Show("Incorrect password!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("User does not exist!");
                        }
                    }
                }
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
