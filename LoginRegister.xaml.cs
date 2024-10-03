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
using System.Security.Cryptography;


namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class LoginRegister : Window
    {
        DebugWindow debugWindow;
        public LoginRegister(DebugWindow debugWindow)
        {
            InitializeComponent();
            this.debugWindow = debugWindow;
        }

        public string UserName { get; internal set; }
        public int UserId { get; internal set; }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string tempUserName = txbLoginUsername.Text;
            string password = pswLoginPassword.Password;


            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Bettors WHERE Username = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", tempUserName);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string dbPassword = reader.GetString("Password");
                            debugWindow.Write(dbPassword);
                            debugWindow.Write(PasswordHasher(password));
                            debugWindow.Write((PasswordHasher(password) == dbPassword).ToString());
                            if (PasswordHasher(password) == dbPassword)
                            {
                                UserName = tempUserName;
                                UserId = reader.GetInt32("BettorsID");
                                MessageBox.Show("Login successful!");
                                this.DialogResult = true;
                                connection.Close();
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
                try
                {
                    string query = "INSERT INTO Bettors (Username, Password, Email, JoinDate) VALUES (@Username, @Password, @Email, @JoinDate)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", UserName);
                        command.Parameters.AddWithValue("@Password", PasswordHasher(password));
                        debugWindow.Write(PasswordHasher(password));
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            debugWindow.Write("User registered successfully!");
                            MessageBox.Show("User registered successfully!");
                            this.DialogResult = true;
                            connection.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to register user!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to register user!");
                    debugWindow.Write("Failed to register user!");
                    debugWindow.Write(ex.ToString());
                    throw;
                }
            }
        }

        public string PasswordHasher(string passwordIn)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, passwordIn);
            }
        }
        #region Microsoft Hash Code
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));


            var sBuilder = new StringBuilder();


            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        #endregion

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
