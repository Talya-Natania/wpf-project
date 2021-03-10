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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostArea.xaml
    /// </summary>
    public partial class HostArea : UserControl
    {
        private Host CurrentHost;
        public HostArea(string id)
        {
            InitializeComponent( );
            CurrentHost = MainWindow.myBl.GetUnits().Find(x => x.Owner.HostKey == long.Parse(id)).Owner;
            upGrid.DataContext = CurrentHost;
            for(int i = 0; i < MainWindow.myBl.ListOfUnits(CurrentHost.HostKey).Count; i++)
            {
                unitcontrol win = new unitcontrol(MainWindow.myBl.ListOfUnits(CurrentHost.HostKey)[i]);
                upGrid.Children.Add(win);
                upGrid.Height += 158;
                //Grid.SetRow(a, i + 1);
            }
            
            DGrid.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.GetOrdersByHost(CurrentHost));
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest> (MainWindow.myBl.GetRequests().Where(x=>x.Status==RequestStatus.Open));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HostUserControl h = new HostUserControl();
            a.Children.Clear();
            a.Children.Add(h);
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            Addwin addWin = new Addwin(CurrentHost);
            addWin.ShowDialog();
        }

       
        
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpOrder win2 = new UpOrder();
            win2.ShowDialog();
            DGrid.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.GetOrdersByHost(CurrentHost));
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest>(MainWindow.myBl.GetRequests().Where(x => x.Status == RequestStatus.Open));

        }

        private void MouseEnter_click(object sender, MouseEventArgs e)
        {
           if (((ListView)sender).SelectedItem != null)
                    MessageBox.Show((((ListView)sender).SelectedItem as GuestRequest).ToString(), "Guest reqest's details", MessageBoxButton.OK, MessageBoxImage.Information,
                        MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void addOrder_Click(object sender, RoutedEventArgs e)
        {

            
                AddOrder Window = new AddOrder(CurrentHost );
                Window.ShowDialog();
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest>(MainWindow.myBl.GetRequests().Where(x => x.Status == RequestStatus.Open));
            DGrid.ItemsSource = new ObservableCollection<Order>(MainWindow.myBl.GetOrdersByHost(CurrentHost));


        }
    }
}
