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
using BE;
using BL;
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UpOrder.xaml
    /// </summary>
    public partial class UpOrder : Window
    {
        Order v;
        public UpOrder( )
        {
            InitializeComponent();
            Statusc.ItemsSource = Enum.GetValues(typeof(BE.OrderStatus));

        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                v = MainWindow.myBl.GetOrders().Where(x => x.OrderKey == long.Parse(unitKey.Text)).FirstOrDefault();
                if (v == null)
                    throw new Exception("The orderKey is wrong");
                if ((OrderStatus)Statusc.SelectedItem == OrderStatus.EmailSent && v.Status != OrderStatus.EmailSent)
                {
                    BackgroundWorker backgroundWorker = new BackgroundWorker();
                    backgroundWorker.DoWork += BackgroundWorker_DoWork;
                    backgroundWorker.RunWorkerAsync();
                }
                MainWindow.myBl.UpdateOrder(long.Parse(unitKey.Text), (OrderStatus)Statusc.SelectedItem);
                MessageBox.Show("The order status changed to" + Statusc.SelectedItem, "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }

            catch (BlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(  Exception str)
            {
                MessageBox.Show(str.Message);
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (v.Status != OrderStatus.EmailSent)
            {
                MainWindow.myBl.SendMail(v, MainWindow.myBl.GetRequests().Find(x => x.GuestRequestKey == v.GuestRequestKey));
            }

        }
    }
}
