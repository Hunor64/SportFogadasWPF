using System.Windows;

namespace SportFogadas
{
    public partial class DebugWindow : Window
    {
        private MainWindow mainWindow;
        public DebugWindow(MainWindow mainWindow, bool debugOn)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            if (!debugOn)
            {
                this.Visibility = Visibility.Hidden;

            }
        }

        protected override void OnClosed(EventArgs e)
        {

            base.OnClosed(e);
            mainWindow.Close();

            
        }

        public void Write(string message)
        {
            if (txtDebug != null)
            {
                txtDebug.Dispatcher.Invoke(() =>
                {
                    txtDebug.AppendText(message + Environment.NewLine);
                });
            }
        }
    }
}