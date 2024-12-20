﻿using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for Bet.xaml
    /// </summary>
    public partial class Bet : Window
    {
        #region Fields
        private int userID;
        private DebugWindow debugWindow;
        private MySqlConnection connection;
        public int betId;
        #endregion

        #region Constructor
        public Bet(int userID, DebugWindow debugWindow, int selectedBet)
        {
            InitializeComponent();
            this.userID = userID;
            this.debugWindow = debugWindow;
            connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
            CheckBalance(0);
            LoadEvents();
            this.betId = selectedBet;
            if (betId != -1)
            {
                EventComboBox.SelectedIndex = betId - 1;
            }

        }
        #endregion

        #region Methods
        private void LoadEvents()
        {
            connection.Open();
            using (MySqlDataReader reader = new MySqlCommand("SELECT EventID, EventName FROM Events", connection).ExecuteReader())
            {
                while (reader.Read())
                {
                    EventComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = reader["EventName"].ToString(),
                        Tag = reader["EventID"]
                    });
                }
            }

        }

        private void PlaceBetButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventComboBox.SelectedItem == null || string.IsNullOrEmpty(BetAmountTextBox.Text))
            {
                MessageBox.Show("Adjon meg egy eseményt és egy összeget!");
                return;
            }

            if (!int.TryParse(BetAmountTextBox.Text, out int betAmount))
            {
                MessageBox.Show("Kérjük, adjon meg egy érvényes számot az összeghez!");
                return;
            }

            int eventId = (int)((ComboBoxItem)EventComboBox.SelectedItem).Tag;

            if (!CheckBalance(betAmount))
            {
                MessageBox.Show("Nem megfelelő egyenleg!");
                return;
            }

            float odds = 1f;
            SaveBetToDatabase(DateTime.Now, odds, betAmount, userID, eventId, false);
            UpdateBalance(betAmount);
            CheckBalance(0);
        }

        private bool CheckBalance(int betAmount)
        {
            using (var connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                connection.Open();
                string query = "SELECT Balance FROM Bettors WHERE BettorsID = @userID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userID", userID);
                int balance = Convert.ToInt32(command.ExecuteScalar());
                lblEgyneleg.Content = $"Egyenleg: {balance}";
                if (betAmount == 0)
                {
                    return false;
                }
                return balance >= betAmount;
            }
        }


        private void SaveBetToDatabase(DateTime betDate, float odds, int amount, int bettorsId, int eventId, bool status)
        {
            using (var connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                connection.Open();
                string query = "INSERT INTO Bets (BetDate, Odds, Amount, BettorsID, EventID, Status) VALUES (@BetDate, @Odds, @Amount, @BettorsID, @EventID, @Status)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@BetDate", betDate);
                command.Parameters.AddWithValue("@Odds", odds);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@BettorsID", bettorsId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@Status", status);

                command.ExecuteNonQuery();
            }
            MessageBox.Show("Siikeres fogadás!");
        }

        private void UpdateBalance(int betAmount)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            string query = "UPDATE Bettors SET Balance = Balance - @betAmount WHERE BettorsID = @userID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@betAmount", betAmount);
            command.Parameters.AddWithValue("@userID", userID);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void btn_BET_Txt_Click(object sender, RoutedEventArgs e)
        {
            BetAmountTextBox.Text = "";
        }

        #endregion
    }
}