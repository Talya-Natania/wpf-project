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
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    { 
        public AddOrder(Host h)
        {
            InitializeComponent();
            List<string> help=new List<string>();
            var v = from item in MainWindow.myBl.GetUnits()
                    where h.HostKey == item.Owner.HostKey
                    select item.HostingUnitName;
            help = v.ToList();
            numUnit.ItemsSource = help;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (GuestKey.Text!=""&& numUnit.SelectedValue!=null)
            {
                Order add = new Order
                {
                    HostingUnitkey = MainWindow.myBl.GetUnits().Where(x => x.HostingUnitName == numUnit.SelectedItem.ToString()).FirstOrDefault().HostingUnitkey,
                    GuestRequestKey = long.Parse(GuestKey.Text),
                    CreateDate = DateTime.Now,
                };
                try
                {
                    MainWindow.myBl.AddOrder(add);
                    MessageBox.Show("Your  requst added succesfully to your orders list press up", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (BlException ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter all details");
            }

        }

    }
}
