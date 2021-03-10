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
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for units.xaml
    /// </summary>
    public partial class units : Window
    {
        public units()
        {
            InitializeComponent();
            ListView_Units.ItemsSource = new ObservableCollection<HostingUnit>(MainWindow.myBl.GetUnits());
            area1.ItemsSource = Enum.GetValues(typeof(BE.AreaChoise));
            //ListView_Units.
        }

        private void sor_Click(object sender, RoutedEventArgs e)
        {
            ListView_Units.ItemsSource = new ObservableCollection<HostingUnit>(MainWindow.myBl.UnitByArea((AreaChoise)area1.SelectedItem));

        }

        private void lowest_Click(object sender, RoutedEventArgs e)
        {
            ans.Text = MainWindow.myBl.TheLow();
        }
    }
}
