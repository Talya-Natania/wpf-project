using System;
using System.Collections.Generic;
using System.IO;
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
using BE;
using BL;
using Microsoft.Win32;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Addwin.xaml
    /// </summary>
    public partial class Addwin : Window
    {
        HostingUnit hu;
        public Addwin(Host h)
        {
            InitializeComponent();
            hu = new HostingUnit
            {
                HostingUnitkey = Configuration.hostingUnitKey,
                Owner=h
            };
            ar.ItemsSource = Enum.GetValues(typeof(BE.AreaChoise));
            unitKey.IsEnabled = false;
            unit.DataContext = hu;
        }
       

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string photoPath = @"..\..\..\hostingUnitsIm\" + name.Text + @".jpg";
            (File.Create(photoPath)).Close();
            System.IO.File.Copy(openFileDialog.FileName, photoPath, true);
            plus.IsEnabled = true;
            image.Source = new BitmapImage(new Uri("images/camera2.jpg", UriKind.Relative));
        }
        OpenFileDialog openFileDialog;
        private void AddImage_click(object sender, MouseButtonEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                List<string> uris = new List<string>();
                openFileDialog.FileNames.ToList().ForEach(item => uris.Add((item)));
                if (hu.Uris == null)
                    hu.Uris = new List<string>();
                hu.Uris.AddRange(uris);
                image.Source = new BitmapImage(new Uri(uris[hu.Uris.Count - 1], UriKind.Absolute));
            }
            plus.IsEnabled = false;
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            HostingUnit hostUnit = new HostingUnit
            {
               
                HostingUnitkey = long.Parse(unitKey.Text),
                HostingUnitName = name.Text,
                price = int.Parse(price.Text),
                Owner =hu.Owner
                         };
            try
            {
                MainWindow.myBl.AddHostingUnit(hostUnit);
                MessageBox.Show(hostUnit.Owner.PrivateName + " your hosting unit addea succesfuly");
            }
            catch(BlException ex)
            {
                MessageBox.Show(ex.Message,"",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }
    }

}

