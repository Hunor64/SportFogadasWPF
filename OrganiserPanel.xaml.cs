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
    /// Interaction logic for OrganiserPanel.xaml
    /// </summary>
    public partial class OrganiserPanel : Window
    {
        public OrganiserPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
            connection.Open();
            string query = "INSERT INTO Events (EventName, EventDate, Category, Location) VALUES (@EventName, @EventDate, @Category, @Location)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EventName", txbEventName);
                command.Parameters.AddWithValue("@EventDate", dtpEventDate);
                command.Parameters.AddWithValue("@Category", txbCategory);
                command.Parameters.AddWithValue("@Location", txbLocation);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Event created successfully!");
                    this.DialogResult = true;
                    connection.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create event!");
                }
            }
        }
    }
}
