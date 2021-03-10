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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ManagerArea.xaml
    /// </summary>
    public partial class ManagerArea : UserControl
    {
        public ManagerArea()
        {
            InitializeComponent();
            mounthes.ItemsSource = Enum.GetValues(typeof(BE.Month));
        }

        private void r_Click(object sender, RoutedEventArgs e)
        {
            rList win = new rList();
            win.ShowDialog();
        }

        private void h_Click(object sender, RoutedEventArgs e)
        {
            units win = new units();
            win.ShowDialog();
        }

        private void o_Click(object sender, RoutedEventArgs e)
        {
            ListOrder win = new ListOrder();
            win.ShowDialog();
        }

        private void Fee_click(object sender, RoutedEventArgs e)
        {
           fee.Text= MainWindow.myBl.FeebyMonth(mounthes.SelectedIndex +1 ).ToString();
        }

        private void perfect_Click(object sender, RoutedEventArgs e)
        {
            theH.Text = MainWindow.myBl.ThePerfectHost();
        }

        private void pop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pop_ans.Text = MainWindow.myBl.MostPopularArea().ToString();
            }
            catch (BlException ex)
            {
                MessageBox.Show(ex.Message,"",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
