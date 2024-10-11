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
        public bool debugOn = false;
        public bool trueGambling = false; //CSAK TRUE LEHET. SEMMI MÁS!!!!

        #region Initial Variables
        private DebugWindow debugWindow;
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
        List<Events> events = new List<Events>();
        List<UserBets> bets = new List<UserBets>();
        string userName = "";
        int userID = -1;
        string userPrivilage = "guest";


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

        #region Create text
        public TextBlock NewTextBlock(string content)
        {
            var block = new TextBlock()
            {
                Text = content,
            };
            return block;
        }

        #endregion

        #region Read Events From Database
        public void ReadEvents()
        {


            for (int i = 0; stpEvents.Children.Count != 0; i++)
            {
                debugWindow.Write(stpEvents.Children.Count.ToString());
                debugWindow.Write($"Removing child {stpEvents.Children[0].ToString()}");
                stpEvents.Children.Remove(stpEvents.Children[0]);
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
            var newEvent = new StackPanel();
            newEvent.Style = (Style)FindResource("EventStyle");

            events.ForEach(e =>
            {
                var card = new Border();
                card.Style = (Style)FindResource("CardStyle");


                var eventName = NewTextBlock(e.EventName);
                eventName.FontWeight = FontWeights.Bold;
                var location = NewTextBlock(e.Location);
                var category = NewTextBlock(e.Category);
                var eventDate = NewTextBlock(e.EventDate.ToString());
                int evemtID = e.EventID;

                var gomb = new Button()
                {
                    Content = "Fogadás",
                };
                gomb.Style = (Style)FindResource("BetButtonStyle");
                gomb.Click += (s, e) =>
                {
                    if (userID != -1)
                    {
                        Bet betWindow = new Bet(userID, debugWindow, evemtID);
                        betWindow.ShowDialog();
                        LoadUserBets();
                    }
                };

                if (userID == -1)
                {
                    gomb.Visibility = Visibility.Collapsed;    
                }
                card.Child = new StackPanel()
                {
                    Children =
                    {
                        eventName,
                        location,
                        category,
                        eventDate,
                        gomb
                    }
                };
                newEvent.Children.Add(card);
            });

            stpEvents.Children.Add(newEvent);
        }
        #endregion

        #region Login Method
        public void Login()
        {
            debugWindow.Write("Login method called");
            LoginRegister loginRegister = new LoginRegister(debugWindow);
            debugWindow.Write("LoginRegister window opened");
            loginRegister.ShowDialog();
            debugWindow.Write("LoginRegister window closed");

            if (loginRegister.DialogResult.HasValue && loginRegister.DialogResult.Value)
            {
                userName = loginRegister.UserName;
                userID = loginRegister.UserId;
                userPrivilage = loginRegister.Privilage;
                debugWindow.Write(userName);
                debugWindow.Write(userID.ToString());
                LoadUserBets();
                if (userPrivilage == "organiser")
                {
                    btnOrganiser.Visibility = Visibility.Visible;
                }
                else
                {
                    btnOrganiser.Visibility = Visibility.Collapsed;
                }
                if (userPrivilage == "admin")
                {
                    btnAdmin.Visibility = Visibility.Visible;

                }
                else
                {
                    btnAdmin.Visibility = Visibility.Collapsed;
                }
                btnBet.Visibility = Visibility.Visible;
                btnTopup.Visibility = Visibility.Visible;
                btnRealTopup.Visibility = Visibility.Visible;
                btnLogin.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Visible;
                stpOngoingBets.Visibility = Visibility.Visible;
            }

            else
            {
                debugWindow.Write("No user logged in");
            }
        }
        #endregion

        #region Load Users own bets
        public void LoadUserBets()
        {
            for (int i = 0; stpOngoingBets.Children.Count != 0; i++)
            {
                debugWindow.Write(stpOngoingBets.Children.Count.ToString());
                debugWindow.Write($"Removing child {stpOngoingBets.Children[0].ToString()}");
                stpOngoingBets.Children.Remove(stpOngoingBets.Children[0]);
            }
            bets = new List<UserBets>();

            lblUsername.Content = userName;

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

            var userReader = ReadDB($"SELECT * FROM Bettors WHERE BettorsID = {userID}");
            userReader.Read();
            lblBalance.Content = $"Egyenleg: {userReader.GetInt32("Balance")}";
            userReader.Close();
            debugWindow.Write($"Balance read from database");

            var stack = stpOngoingBets;
            stack.Visibility = Visibility.Collapsed;
            stack.Children.Clear();

            foreach (var bet in bets)
            {
                stack.Visibility = Visibility.Visible;

                var title = $"Fogadás ID: {bet.BetID}";
                var content = $"Esemény: {events.First(x => x.EventID == bet.EventID).EventName}\n" +
                              $"Összeg: {bet.Amount}\n" +
                              $"Dátum: {bet.BetDate}\n" +
                              $"Szorzó: {bet.Odds}\n" +
                              $"Befejeződött: {bet.Status}";

                AddCardToStackPanel(stack, title, content);
            }
        }
        private Border CreateCard(string title, string content)
        {
            var card = new Border();
            card.Style = (Style)FindResource("BetCardStyle");

            var titleLabel = new TextBlock()
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 5),
                Foreground= new SolidColorBrush(Colors.White)
            };

            var contentLabel = new TextBlock()
            {
                Text = content,
                FontSize= 14,
                Margin = new Thickness(0, 0, 0, 5),
                Foreground = new SolidColorBrush(Colors.White)
            };

            card.Child = new StackPanel()
            {
                Children =
                    {
                        titleLabel,
                        contentLabel
                    }
            };

            return card;
        }

        private void AddCardToStackPanel(StackPanel stackPanel, string title, string content)
        {
            var card = CreateCard(title, content);
            stackPanel.Style = (Style)FindResource("BetStyle");
            stackPanel.Children.Add(card);
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

        #region On close override
        protected override void OnClosed(EventArgs e)
        {
            if (trueGambling)
            {

                KeepGambling keepGamblingWindow = new();
                keepGamblingWindow.ShowDialog();

                var newwin = new MainWindow();
                newwin.Show();
            }

            base.OnClosed(e);
            debugWindow.Close();

        }
        #endregion

        #region Button clicks
        private void LogIn(object sender, RoutedEventArgs e)
        {
            Login();
            ReadEvents();
        }

        private void OrganiserPanel(object sender, RoutedEventArgs e)
        {
            if (userPrivilage == "organiser")
            {
                OrganiserPanel organiserPanel = new OrganiserPanel(debugWindow);
                organiserPanel.ShowDialog();
                ReadEvents();
            }
            else
            {
                MessageBox.Show("You do not have the required privilages to access this feature.");
            }
        }

        private void TopUp(object sender, RoutedEventArgs e)
        {
            if (userID != -1)
            {
                BalanceManager balanceManager = new BalanceManager(userID, debugWindow);
                balanceManager.ShowDialog();
                LoadUserBets();
            }
        }

        private void BetClick(object sender, RoutedEventArgs e)
        {
            if (userID != -1)
            {
                Bet betWindow = new Bet(userID, debugWindow, -1);
                betWindow.ShowDialog();
                LoadUserBets();
            }
        }

        private void TopUpReal(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://fos.hu/1grj",
                UseShellExecute = true
            });
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            userName = "";
            userID = -1;
            userPrivilage = "guest";
            btnLogout.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Visible;
            btnBet.Visibility = Visibility.Collapsed;
            btnTopup.Visibility = Visibility.Collapsed;
            btnRealTopup.Visibility = Visibility.Collapsed;
            btnOrganiser.Visibility = Visibility.Collapsed;
            btnAdmin.Visibility = Visibility.Collapsed;
            stpOngoingBets.Visibility = Visibility.Collapsed;
            lblBalance.Content = "";
            lblUsername.Content = "";
            stpOngoingBets.Children.Clear();
            ReadEvents();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel(debugWindow);
            adminPanel.ShowDialog();

        }

        #endregion
    }
}