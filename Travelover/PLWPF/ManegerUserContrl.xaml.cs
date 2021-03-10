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
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ManegerUserContrl.xaml
    /// </summary>
    public partial class ManegerUserContrl : UserControl
    {
        public ManegerUserContrl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ps.Password == MainWindow.myBl.GetManagerPassword())
            {
                ManagerArea win = new ManagerArea();
                MyGrid.Children.Clear();
                MyGrid.Children.Add(win);
            }
            else
            {
                MessageBox.Show("Sorry,the password is wrong","Password incorrect",MessageBoxButton.OK,MessageBoxImage.Stop);
                ps.Password = "";
            }

        }
    }
}
