﻿using System;
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
        #region Initialize Components
        DebugWindow debugWindow;
        public LoginRegister(DebugWindow debugWindow)
        {
            InitializeComponent();
            this.debugWindow = debugWindow;
        }

        public string UserName { get; internal set; }
        public int UserId { get; internal set; }
        public string Privilage { get; internal set; }
        #endregion

        #region Login Button Function
        private void btnLogin_Click()
        {
            string tempUserName = txbLoginUsername.Text;
            string password = pswLoginPassword.Password;


            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                debugWindow.Write("Searching for user");
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
                            debugWindow.Write(PasswordHasher(password));
                            debugWindow.Write((PasswordHasher(password) == dbPassword).ToString());
                            if (PasswordHasher(password) == dbPassword)
                            {
                                UserName = tempUserName;
                                UserId = reader.GetInt32("BettorsID");
                                Privilage = reader.GetString("Privilage");
                                MessageBox.Show("Login successful!");
                                this.DialogResult = true;
                                connection.Close();
                                this.Close();
                            }
                            else
                            {
                                debugWindow.Write("Password mismatch!");
                                MessageBox.Show("Incorrect password!");
                            }
                        }
                        else
                        {
                            debugWindow.Write("No user found!");
                            MessageBox.Show("User does not exist!");
                        }
                    }
                }
            }
        }
        #endregion

        #region Register Button Function
        private void btnRegister_Click()
        {
            UserName = txbRegisterUsername.Text;
            string password = pswRegisterPassword.Password;
            string email = txbRegisterEmail.Text;

            if (UserName.Length >= 5 && password.Length > 5 && HasNumber(password) && HasSpecialChar(password) && email.Contains('.') && email.Contains('@'))
            {
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
                                UserId = (int)command.LastInsertedId;
                                Privilage = "user";
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
            else
            {
                if (!(UserName.Length >= 5))
                {
                    debugWindow.Write("Username must be longer than 5 characters!");
                    debugWindow.Write(UserName);
                    MessageBox.Show("Username must be longer than 5 characters!");
                }
                else if (!(password.Length > 5))
                {
                    debugWindow.Write("Password must be longer than 5 characters!");
                    debugWindow.Write(password);
                    MessageBox.Show("Password must be longer than 5 characters!");
                }
                else if (!HasNumber(password))
                {
                    debugWindow.Write("Password must contain a number!");
                    debugWindow.Write(password);
                    MessageBox.Show("Password must contain a number!");
                }
                else if (!HasSpecialChar(password))
                {
                    debugWindow.Write("Password must contain a special character!");
                    debugWindow.Write(password);
                    MessageBox.Show("Password must contain a special character!");
                }
                else
                {

                    debugWindow.Write("Invalid email address!");
                    debugWindow.Write(email);
                    debugWindow.Write($"Email contains a dot: {email.Contains('.').ToString()}");
                    debugWindow.Write($"Email contains an at: {email.Contains('@').ToString()}");
                    MessageBox.Show("Invalid email address!");
                }
            }
        }

        private bool HasNumber(string input)
        {
            return input.Any(char.IsDigit);
        }

        private bool HasSpecialChar(string input)
        {
            return input.Any(ch => !char.IsLetterOrDigit(ch));
        }
        #endregion

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

        #region Switch Login Register Button Functions
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
        #endregion

        #region Click Stuff
        private void pswRegisterPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnRegister_Click();
            }
        }

        private void pswLoginPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click();
            }
        }

        private void btnRegister_Click_1(object sender, RoutedEventArgs e)
        {
            btnRegister_Click();
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            btnLogin_Click();
        }
        #endregion
    }
}
