using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool debugOn = false;

        #region Initial Variables
        private DebugWindow debugWindow;
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
        List<Events> events = new List<Events>();
        string userName = "";
        bool loggedIn = false;
        #endregion

        public MainWindow()
        {
            #region Initialize Windows
            InitializeComponent();
            debugWindow = new DebugWindow(this);
            if (debugOn)
            {
                debugWindow.Show();
            }
            #endregion


            #region Database Connection
            try
            {
                connection.Open();
                debugWindow.Write("Connected to MySQL Server");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to MySQL Server: " + ex.Message);
                debugWindow.Write("Error connecting to MySQL Server: " + ex.Message);
                this.Close();
            }
            #endregion

            #region Read Events From Database
            var reader = ReadDB("SELECT * FROM Events");

            while (reader.Read())
            {
                events.Add(new Events()
                {
                    EventID = reader.GetInt32("EventID"),
                    EventName = reader.GetString("EventName"),
                    EventDate = reader.GetDateTime("EventDate"),
                    Category = reader.GetString("Category"),
                    Location = reader.GetString("Location")
                });
            }
            debugWindow.Write($"{events.Count} events read from database");

            events.ForEach(e =>
            {
                debugWindow.Write($"{e.EventID},{e.EventName},{e.EventDate},{e.Category},{e.Location}");
                stpCurrentEvents.Children.Add(new TextBlock() { Text = e.EventName });
            });

            #endregion

            Login();
        }

        public void Login()
        {
            LoginRegister loginRegister = new LoginRegister(userName);
            loginRegister.ShowDialog();

            if (loginRegister.DialogResult.HasValue && loginRegister.DialogResult.Value)
            {
                userName = loginRegister.UserName;
                loggedIn = true;
                MessageBox.Show(userName);
            }
            else
            {
                loggedIn = false;
            }
        }


        #region Database Functions
        public void Exec(string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();
                debugWindow.Write("Command executed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing command: " + ex.Message);
                debugWindow.Write("Error executing command: " + ex.Message);
            }
        }
        public MySql.Data.MySqlClient.MySqlDataReader ReadDB(string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                debugWindow.Write("Command executed successfully");
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing command: " + ex.Message);
                debugWindow.Write("Error executing command: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Event Handlers
        protected override void OnClosed(EventArgs e)
        {
            connection.Close();
            base.OnClosed(e);
            debugWindow.Close();

        }
        #endregion
    }
}