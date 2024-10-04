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
        #region Initialize Components
        DebugWindow debugWindow;
        public OrganiserPanel(DebugWindow debugWindow)
        {
            InitializeComponent();
            this.debugWindow = debugWindow;
        }
        #endregion

        #region Register Event Button Function
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");

            try
            {
                connection.Open();
                string query = "INSERT INTO Events (EventName, EventDate, Category, Location) VALUES (@EventName, @EventDate, @Category, @Location)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventName", txbEventName.Text);
                    command.Parameters.AddWithValue("@EventDate", dtpEventDate.DisplayDate);
                    command.Parameters.AddWithValue("@Category", txbCategory.Text);
                    command.Parameters.AddWithValue("@Location", txbLocation.Text);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Event created successfully!");
                        debugWindow.Write("Event created successfully!");
                        this.DialogResult = true;
                        connection.Close();
                        this.Close();
                    }
                    else
                    {
                        debugWindow.Write("Failed to create event!");
                        MessageBox.Show("Failed to create event!");
                    }
                }
            }
            catch (Exception ex)
            {
                debugWindow.Write("Failed to create event!");
                debugWindow.Write(ex.ToString());
                MessageBox.Show("Failed to create event!");
                throw;
            }
        }
        #endregion
    }
}
