using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace SportFogadas
{
    public partial class AdminPanel : Window
    {
        DebugWindow debugWindow;
        public AdminPanel(DebugWindow debugWindow)
        {
            this.debugWindow = debugWindow;
            InitializeComponent();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = pwdPassword.Password;
            string email = txtEmail.Text;
            string query = "INSERT INTO Bettors (Username, Password, Email, JoinDate, IsActive, Privilage) VALUES (@Username, @Password, @Email, @JoinDate, @IsActive, @Privilage)";

            ExecuteQuery(query, username, password, email);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = pwdPassword.Password;
            string email = txtEmail.Text;
            string query = "UPDATE Bettors SET Password=@Password, Email=@Email WHERE Username=@Username";

            ExecuteQuery(query, username, password, email);
        }

        private void ExecuteQuery(string query, string username, string password, string email)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", PasswordHasher(password));
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                    command.Parameters.AddWithValue("@IsActive", true);
                    command.Parameters.AddWithValue("@Privilage", "user");

                    command.ExecuteNonQuery();
                    MessageBox.Show("Operation successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        #region Password Hasher
        public string PasswordHasher(string passwordIn)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                return GetHash(sha512Hash, passwordIn);
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
        #endregion

    }
}