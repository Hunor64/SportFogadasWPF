using System;
using System.Data.SqlClient;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    public partial class BalanceManager : Window
    {
        #region Values
        int currentBalance = 0;
        DebugWindow debugWindow;
        int userId;
        #endregion

        #region Init components
        public BalanceManager(int userId, DebugWindow debugWindow)
        {
            this.debugWindow = debugWindow;
            this.userId = userId;
            InitializeComponent();
            LoadBalanceFromDb();
            lblDepositBalance.Content = $"Egyenleg: {currentBalance}";
            lblWithdrawlBalance.Content = $"Egyenleg: {currentBalance}";
        }
        #endregion

        #region DB Methods
        private void LoadBalanceFromDb()
        {
            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            string query = "SELECT Balance FROM Bettors WHERE BettorsID = @BettorsID";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BettorsID", userId);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentBalance = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                debugWindow.Write("SQL error occurred while loading the balance: " + sqlEx.Message);
                MessageBox.Show("SQL error occurred while loading the balance: " + sqlEx.Message);
            }
        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (int.TryParse(txtWithdrawl.Text, out amount) && amount > 0 && amount <= currentBalance)
            {
                currentBalance -= amount;
                UpdateBalanceInDb();
                lblWithdrawlBalance.Content = $"Egyenleg: {currentBalance}";
                lblDepositBalance.Content = $"Egyenleg: {currentBalance}";
                MessageBox.Show("Sikeres kifizetés!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Hibás összeg.");
            }
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (int.TryParse(txtDeposit.Text, out amount) && amount > 0)
            {
                currentBalance += amount;
                UpdateBalanceInDb();
                lblWithdrawlBalance.Content = $"Egyenleg: {currentBalance}";
                lblDepositBalance.Content = $"Egyenleg: {currentBalance}";
                MessageBox.Show("Sikeres feltöltés!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Hibás összeg.");
            }
        }

        private void UpdateBalanceInDb()
        {
            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";
            string query = "UPDATE Bettors SET Balance = @Balance WHERE BettorsID = @BettorsID";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Balance", currentBalance);
                    command.Parameters.AddWithValue("@BettorsID", userId);

                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
            catch (MySqlException sqlEx)
            {
                debugWindow.Write("SQL error occurred while updating the balance: " + sqlEx.Message);
                MessageBox.Show("SQL error occurred while updating the balance: " + sqlEx.Message);
            }
        }
        private void btnDepositSwitch_Click(object sender, RoutedEventArgs e)
        {
            stpLoadBalance.Visibility = Visibility.Hidden;
            stpDeLoadBalance.Visibility = Visibility.Visible;
        }

        private void btnWithdrawSwitch_Click(object sender, RoutedEventArgs e)
        {
            stpLoadBalance.Visibility = Visibility.Visible;
            stpDeLoadBalance.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
