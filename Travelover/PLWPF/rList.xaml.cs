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
    /// Interaction logic for rList.xaml
    /// </summary>
    public partial class rList : Window
    {
        public rList()
        {
            InitializeComponent();
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest>(MainWindow.myBl.GetRequests());
            area.ItemsSource = Enum.GetValues(typeof(BE.AreaChoise));
        }

        private void Button_Out(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MouseEnter_click(object sender, MouseEventArgs e)
        {
            if (((ListView)sender).SelectedItem != null)
                MessageBox.Show((((ListView)sender).SelectedItem as GuestRequest).ToString(), "Guest reqest's details", MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void AreaS_click(object sender, RoutedEventArgs e)
        {
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest>(MainWindow.myBl.RequstsByArea((AreaChoise)area.SelectedItem));
        }

        private void Sorting_click(object sender, RoutedEventArgs e)
        {
            ListView_Reqest.ItemsSource = new ObservableCollection<GuestRequest>(MainWindow.myBl.SortRequests().Distinct());
        }

       
    }
}
