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
    /// Interaction logic for unitcontrol.xaml
    /// </summary>
    public partial class unitcontrol : UserControl
    {
         public HostingUnit Current { get; set; }
        int imageindex;
        Viewbox VBimage;
        Image myImage;
        long Id;
        
        public unitcontrol(HostingUnit hu)
        {
            VBimage = new Viewbox();

            InitializeComponent();
            Current = hu;
            Id = Current.HostingUnitkey;
            userGrid.DataContext = Current;
            imageindex = 0;
            
            VBimage.Width = 170;
            VBimage.Height = 150;
            VBimage.Stretch = Stretch.Fill;
            userGrid.Children.Add(VBimage);
            Grid.SetColumn(VBimage, 2);
            Grid.SetRow(VBimage, 0);
            myImage = CreatViewImage();
            VBimage.Child = null;
            VBimage.Child = myImage;
            VBimage.MouseUp += vbImage_MouseUp;
            occupancy.Text = MainWindow.myBl.GetAnnualBusyDays(Current).ToString();
           
        }
    
        private Image CreatViewImage()
        {
            Image dynamicImage = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Current.Uris[imageindex],UriKind.Absolute);
            bitmap.EndInit();
            dynamicImage.Source = bitmap;
            return dynamicImage;
        }
        private void vbImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            VBimage.Child = null;
            imageindex =
            (imageindex + Current.Uris.Count - 1) % Current.Uris.Count;
            myImage = CreatViewImage();
            VBimage.Child = myImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Updatewin win = new Updatewin(Id);
            win.ShowDialog();
            
            //MainWindow.myBl.UpdateHostingUnit()
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.myBl.RemoveHostingUnit(Current);
                MessageBox.Show("The unit" + Current.HostingUnitName + " remove from the system");
                garb.IsEnabled=false;
                upd.IsEnabled = false;
            }
            catch (BlException ex  )
            {
                MessageBox.Show(ex.Message,"",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }
    }
}
