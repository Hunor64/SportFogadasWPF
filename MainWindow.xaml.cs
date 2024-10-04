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
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool debugOn = true;

        #region Initial Variables
        private DebugWindow debugWindow;
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
        List<Events> events = new List<Events>();
        List<UserBets> bets = new List<UserBets>();
        string userName = "";
        int userID = -1;

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

            ReadEvents();
        }
        #region Read Events From Database
        public void ReadEvents()
        {
            for (int i = 0; stpCurrentEvents.Children.Count != 0; i++)
            {
                debugWindow.Write(stpCurrentEvents.Children.Count.ToString());
                debugWindow.Write($"Removing child {stpCurrentEvents.Children[0].ToString()}");
                stpCurrentEvents.Children.Remove(stpCurrentEvents.Children[0]);
            }
            events = new List<Events>();
            var eventReader = ReadDB("SELECT * FROM Events");

            while (eventReader.Read())
            {
                events.Add(new Events()
                {
                    EventID = eventReader.GetInt32("EventID"),
                    EventName = eventReader.GetString("EventName"),
                    EventDate = eventReader.GetDateTime("EventDate"),
                    Category = eventReader.GetString("Category"),
                    Location = eventReader.GetString("Location")
                });
            }
            debugWindow.Write($"{events.Count} events read from database");
            eventReader.Close();
            events.ForEach(e =>
            {
                debugWindow.Write($"{e.EventID},{e.EventName},{e.EventDate},{e.Category},{e.Location}");
                stpCurrentEvents.Children.Add(new TextBlock() { Text = e.EventName });
            });
        }
        #endregion

        #region Login Method
        public void Login()
        {
            LoginRegister loginRegister = new LoginRegister(debugWindow);
            loginRegister.ShowDialog();

            if (loginRegister.DialogResult.HasValue && loginRegister.DialogResult.Value)
            {
                userName = loginRegister.UserName;
                userID = loginRegister.UserId;
                loggedIn = true;
                debugWindow.Write(userName);
                debugWindow.Write(userID.ToString());
                LoadUserBets();
            }
            else
            {
                loggedIn = false;
            }
        }
        #endregion

        #region Load Users own bets
        public void LoadUserBets()
        {
            var betReader = ReadDB($"SELECT * FROM Bets WHERE BettorsID = {userID}");
            while (betReader.Read())
            {
                bets.Add(new UserBets()
                {
                    BetID = betReader.GetInt32("BetID"),
                    BettorsID = betReader.GetInt32("BettorsID"),
                    EventID = betReader.GetInt32("EventID"),
                    Amount = betReader.GetInt32("Amount"),
                    BetDate = betReader.GetDateTime("BetDate"),
                    Odds = betReader.GetFloat("Odds"),
                    Status = betReader.GetBoolean("Status")
                });

            }
            betReader.Close();

            debugWindow.Write($"{bets.Count} bets read from database");

            var stack = stpOngoingBets;
            foreach (var bet in bets)
            {
                debugWindow.Write($"{bet.BetID},{bet.BetDate},{bet.Odds},{bet.Amount},{bet.BettorsID},{bet.EventID},{bet.Status}");

                stack.Children.Add
                    (
                    new TextBlock()
                    {
                        Text = $"{bet.BetID},{bet.BettorsID},{bet.EventID},{bet.Amount},{bet.BetDate},{bet.Odds},{bet.Status}"
                    }
                    );
            }
        }
        #endregion

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

        #region Debug Button clicks
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OrganiserPanel organiserPanel = new OrganiserPanel(debugWindow);

            organiserPanel.ShowDialog();
            ReadEvents();
        }
        #endregion
    }
}