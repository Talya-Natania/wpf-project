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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IBL myBl = FactoryBL.getBL();
       
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void back_hostUserControl(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(welcomeGrid);
        }
        private void Guest_click(object sender, RoutedEventArgs e)
        { 
            GuestUserControl myUser = new GuestUserControl();
            myUser.Back.MouseLeftButtonDown += back_hostUserControl;
            MainGrid.Opacity = 0.9;
            MainGrid.Children.Clear();
            MainGrid.Children.Add(myUser);


        }
        private void Button_Out(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        

        private void Host_click(object sender, RoutedEventArgs e)
        {
            HostUserControl H = new HostUserControl();
        H.Back.MouseLeftButtonDown += back_hostUserControl;
        MainGrid.Opacity = 0.9;
            MainGrid.Children.Clear();
            MainGrid.Children.Add(H);
        }

        private void Maneger_click(object sender, RoutedEventArgs e)
        {
            ManegerUserContrl win = new ManegerUserContrl();
            win.Back.MouseLeftButtonDown += back_hostUserControl;
            MainGrid.Opacity = 0.9;
            MainGrid.Children.Clear();
            MainGrid.Children.Add(win);
        }

      
    }
    }

