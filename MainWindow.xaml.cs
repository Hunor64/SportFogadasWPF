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
using MySql.Data.MySqlClient;

namespace SportFogadas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DebugWindow debugWindow;
        public bool debugOn = true;

        public MainWindow()
        {
            InitializeComponent();
            debugWindow = new DebugWindow(this,debugOn);
            debugWindow.Show();
            ConnectToMySql();
        }

        public void ConnectToMySql()
        {
            string connectionString = "Server=localhost;Database=Bets;Uid=root;Pwd=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    debugWindow.WriteDebugInfo("Connected to MySQL Server");
                    debugWindow.WriteDebugInfo("Connection String: " + connectionString);
                    
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to MySQL Server: " + ex.Message);
                    debugWindow.WriteDebugInfo("Error connecting to MySQL Server: " + ex.Message);
                    this.Close();
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            debugWindow.Close();
        }
    }
}