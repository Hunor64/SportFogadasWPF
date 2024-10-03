using System.Data.SqlClient;
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
        #region Initial Variables
        private DebugWindow debugWindow;
        public bool debugOn = true;
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=Bets;Uid=root;Pwd=;");
        #endregion

        public MainWindow()
        {
            #region Initialize Windows
            InitializeComponent();
            debugWindow = new DebugWindow(this, debugOn);
            debugWindow.Show();
            #endregion

            #region Database Connection

            try
            {
                connection.Open();
                debugWindow.Write("Connected to MySQL Server");
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to MySQL Server: " + ex.Message);
                debugWindow.Write("Error connecting to MySQL Server: " + ex.Message);
                this.Close();
            }

            #endregion

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