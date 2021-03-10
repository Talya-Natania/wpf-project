using BE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ListOrder.xaml
    /// </summary>
    public partial class ListOrder : Window
    {
        public ListOrder()
        {
            InitializeComponent();
            var v = from item in MainWindow.myBl.GetUnits()
                    select item.Owner.HostKey;
          choise.ItemsSource=  v.ToList().Distinct();
            ListView_Orders.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.GetOrders());
            status.ItemsSource= Enum.GetValues(typeof(BE.OrderStatus));
        }

        private void grop_click(object sender, RoutedEventArgs e)
        {
            Host v = MainWindow.myBl.GetUnits().Find(item => item.Owner.HostKey == (long)choise.SelectedItem).Owner;
            ListView_Orders.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.GetOrdersByHost(v));

        }

        private void gropD_click(object sender, RoutedEventArgs e)
        {
            ListView_Orders.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.OrdersByStatus((OrderStatus)(status.SelectedItem)));

        }
    }
}
