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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostUserControl.xaml
    /// </summary>
    public partial class HostUserControl : UserControl
    {
        
        public HostUserControl()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newHostwin j =new newHostwin( );
            j.ShowDialog();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (MainWindow.myBl.CheckHost(long.Parse(id.Text),long.Parse(ps.Password))) {
                HostArea hostArea = new HostArea(id.Text);
                MyGrid.Opacity = 0.9;
                MyGrid.Children.Clear();
                MyGrid.Children.Add(hostArea);
                
            }
            else
                MessageBox.Show("The user name or the password are not correct.");
        }

        private void yu(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
