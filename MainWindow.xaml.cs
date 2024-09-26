using System.Data.SqlClient;
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
        public MainWindow()
        {
            InitializeComponent();
            ConnectToMySql();
        }

        public void ConnectToMySql()
        {
            string connectionString = "Server=87.248.157.245;Database=nhgmuyyb_Bets;Uid=nhgmuyyb_sql;Pwd=SuliSqlJelszó;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connected to MySQL Server");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to MySQL Server: " + ex.Message);
                }
            }
        }
    }
}