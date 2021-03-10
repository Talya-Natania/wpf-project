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
    /// Interaction logic for GuestUserControl.xaml
    /// </summary>
    public partial class GuestUserControl : UserControl
    {
        //private object myBl;
        GuestRequest G;
        public GuestUserControl()
        {
            InitializeComponent();
           
           
            b.ItemsSource = Enum.GetValues(typeof(BE.AreaChoise));
            c.ItemsSource = Enum.GetValues(typeof(BE.ResortType));
            d.ItemsSource = Enum.GetValues(typeof(BE.Extra));
            e.ItemsSource = Enum.GetValues(typeof(BE.Extra));
            f.ItemsSource = Enum.GetValues(typeof(BE.Extra));
            h.ItemsSource = Enum.GetValues(typeof(BE.Extra));
            i.ItemsSource = Enum.GetValues(typeof(BE.Extra));
            G = new GuestRequest() {
                GuestRequestKey = MainWindow.myBl.GetrequestKey(),
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now

            };//bind to
            smaller.DataContext = G;
        }

        private void add_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                if ((privateName.Text.Length == 0) || !(privateName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                    (FamilyName.Text.Length == 0) || !(FamilyName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                    (subArea.Text.Length == 0) || !(subArea.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                    (adults.Text.Length == 0) || !(adults.Text.All(x => x == ' ' || char.IsDigit(x)))
                     || !(childrens.Text.All(x => x == ' ' || char.IsDigit(x))))
                   
                {
                    MessageBox.Show("Please enter all details");
                    return;
                }
                MainWindow.myBl.AddRequest(G);
              
                oh.Visibility = Visibility.Visible;
                MessageBox.Show("Your request recieve succesfuy");
                G = new GuestRequest()
                {
                    GuestRequestKey = MainWindow.myBl.GetrequestKey(),
                    RegistrationDate = DateTime.Now,
                    EntryDate = DateTime.Now,
                    ReleaseDate = DateTime.Now

                };//bind to
                smaller.DataContext = G;
            }
            catch (BlException m)
            {
                MessageBox.Show(m.Message,"",MessageBoxButton.OK,MessageBoxImage.Error);
                            }
        }



       
    }
}
