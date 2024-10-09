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
    /// Interaction logic for KeepGambling.xaml
    /// </summary>
    public partial class KeepGambling : Window
    {
        public KeepGambling()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            MessageBox.Show("Csak a pénzed 100% veszítheted el, de akár a 10000%-át is visszanyerheted!");
            MessageBox.Show("NE ADD FEL!");
            base.OnClosed(e);
            
        }
    }
}
