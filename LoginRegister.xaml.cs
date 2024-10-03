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
            UserName = userName;
            InitializeComponent();
        }

        public string UserName { get; internal set; }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string tempUserName = txbLoginUsername.Text;
            string password = pswLoginPassword.Password;

            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Password FROM Bettors WHERE Username = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", tempUserName);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string dbPassword = reader.GetString(0);

                            if (string.Equals(password, dbPassword))
                            {
                                UserName = tempUserName;
                                MessageBox.Show("Login successful!");
                                this.DialogResult = true;
                                this.Close();
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
            UserName = txbRegisterUsername.Text;
            string password = pswRegisterPassword.Password;
            string email = txbRegisterEmail.Text;

            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Bettors (Username, Password, Email, JoinDate) VALUES (@Username, @Password, @Email, @JoinDate)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", UserName);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User registered successfully!");
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to register user!");
                    }
                }
            }
        }

        private void btnShowLogin_Click(object sender, RoutedEventArgs e)
        {
            stpLogin.Visibility = Visibility.Visible;
            stpRegister.Visibility = Visibility.Collapsed;
        }

        private void btnShowRegister_Click(object sender, RoutedEventArgs e)
        {
            stpLogin.Visibility = Visibility.Collapsed;
            stpRegister.Visibility = Visibility.Visible;
        }
    }
}
