using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for Bet.xaml
    /// </summary>
    public partial class Bet : Window
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
        DebugWindow debugWindow;
        int bettorIDMain;

        public Bet(int bettorId, DebugWindow debugWindow)
        {
            bettorIDMain = bettorId;
            this.debugWindow = debugWindow;
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                connection.Open();
                string query = "SELECT EventID, EventName FROM Events";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

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
                MessageBox.Show("Please select an event and enter a bet amount.");
                return;
            }

            int eventId = (int)((ComboBoxItem)EventComboBox.SelectedItem).Tag;
            int betAmount = int.Parse(BetAmountTextBox.Text);
            int bettorId = bettorIDMain; 
            float odds = 1.5f; 

            SaveBetToDatabase(DateTime.Now, odds, betAmount, bettorId, eventId, true);
            this.Close();
        }

        private void SaveBetToDatabase(DateTime betDate, float odds, int amount, int bettorsId, int eventId, bool status)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;"))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string query = "INSERT INTO Bets (BetDate, Odds, Amount, BettorsID, EventID, Status) VALUES (@BetDate, @Odds, @Amount, @BettorsID, @EventID, @Status)";
                    MySqlCommand command = new MySqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@BetDate", betDate);
                    command.Parameters.AddWithValue("@Odds", odds);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@BettorsID", bettorsId);
                    command.Parameters.AddWithValue("@EventID", eventId);
                    command.Parameters.AddWithValue("@Status", status);
                    command.ExecuteNonQuery();

                    string updateBalanceQuery = "UPDATE Bettors SET Balance = Balance - @Amount WHERE BettorID = @BettorsID";
                    MySqlCommand updateBalanceCommand = new MySqlCommand(updateBalanceQuery, connection, transaction);
                    updateBalanceCommand.Parameters.AddWithValue("@Amount", amount);
                    updateBalanceCommand.Parameters.AddWithValue("@BettorsID", bettorsId);
                    updateBalanceCommand.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Bet placed successfully!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
