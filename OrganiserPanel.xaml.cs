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
            dtpEventDate.DisplayDateStart = DateTime.Now;
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
                        MessageBox.Show("Esemény sikeresen elkészítve!");
                        debugWindow.Write("Event created successfully!");
                        this.DialogResult = true;
                        connection.Close();
                        this.Close();
                    }
                    else
                    {
                        debugWindow.Write("Failed to create event!");
                        MessageBox.Show("Sikertelen esemény készítés!");
                    }
                }
            }
            catch (Exception ex)
            {
                debugWindow.Write("Failed to create event!");
                debugWindow.Write(ex.ToString());
                MessageBox.Show("Sikertelen esemény készítés!");
                throw;
            }
        }
        #endregion

        #region Other clicks
        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;  
                textBox.Text = "";
           
        }

        private void txbEventName_button_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(txbEventName, null);
        }
        private void txbCategory_button_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(txbCategory, null);
        }
        private void txbLocation_button_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(txbLocation, null);
        }
        #endregion

    }
}
