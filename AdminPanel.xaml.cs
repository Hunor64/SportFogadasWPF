using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            ComboBoxFill();

        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            string username = EventComboBox.Text;
            string password = pwdPassword.Password;
            string email = txtEmail.Text;
            string query = "UPDATE Bettors SET Password=@Password, Email=@Email WHERE Username=@Username";

            ExecuteQuery(query, username, password, email);
            ComboBoxFill();
        }
        public void ComboBoxFill()
        {
            EventComboBox.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Username FROM Bettors", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                debugWindow.Write(reader.HasRows.ToString());
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string username = reader.GetString("Username");
                        EventComboBox.Items.Add(username);
                    }
                }
            }
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
                    MessageBox.Show("Siker!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
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

        #region Clear clicks

        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.Password = ""; // Clear PasswordBox password
            }
            else if (sender is TextBox textBox)
            {
                textBox.Text = ""; // Clear TextBox text
            }

        }

        private void pwdPassword_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(pwdPassword, null);
        }
        private void txtUsername_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(txtUsername, null);
        }
        private void txtEmail_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(txtEmail, null);
        }

        #endregion

        private void btnSwitchEdit_Click(object sender, RoutedEventArgs e)
        {
            btnAddUser.Visibility = Visibility.Hidden;
            btnEditUser.Visibility = Visibility.Visible;
            btnSwitchEdit.Visibility = Visibility.Collapsed;
            btnSwitchCreate.Visibility = Visibility.Visible;
            EventComboBox.Visibility = Visibility.Visible;
            grdUserName.Visibility = Visibility.Collapsed;
            ComboBoxFill();

        }

        private void btnSwitchCreate_Click(object sender, RoutedEventArgs e)
        {
            btnAddUser.Visibility = Visibility.Visible;
            btnEditUser.Visibility = Visibility.Hidden;
            btnSwitchEdit.Visibility = Visibility.Visible;
            btnSwitchCreate.Visibility = Visibility.Collapsed;
            EventComboBox.Visibility = Visibility.Collapsed;
            grdUserName.Visibility = Visibility.Visible;
            ComboBoxFill();

        }
    }
}