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
    /// Interaction logic for newHostwin.xaml
    /// </summary>
    public partial class newHostwin : Window
    {

        Host h = new Host();
        HostingUnit u;
        public newHostwin()
        {
            InitializeComponent();
            unitKey.IsEnabled = false;
            ar.ItemsSource = Enum.GetValues(typeof(BE.AreaChoise));
            host.DataContext = h;
            u = new HostingUnit()
            {
                HostingUnitkey = MainWindow.myBl.GethostingUnitKey(),
                Owner = h
            };
            unit.DataContext = u;
            List<string> t = new List<string>();
            foreach (var item in MainWindow.myBl.GetBranches())
            {
                t.Add(item.BankName);
            }
            Bname.ItemsSource = t.Distinct().ToList();
            List<int> s = new List<int>();
            foreach (var item in MainWindow.myBl.GetBranches())
            {
                s.Add(item.BankNumber);
            }
            Bnum.ItemsSource = s.Distinct().ToList();
            save.IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var v = MainWindow.myBl.GetUnits().Where(x => x.Owner.HostKey == long.Parse(hostKey.Text)).FirstOrDefault();
            if (v == null)
            {
                try
                {

                    if ((firstName.Text.Length == 0) || !(firstName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                   (lastName.Text.Length == 0) || !(lastName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                   (price.Text.Length == 0) || !(price.Text.All(x => x == ' ' || char.IsDigit(x))) ||
                   (phoneNumber.Text.Length < 8) || (phoneNumber.Text.Length > 9) || !(phoneNumber.Text.All(x => x == ' ' || char.IsDigit(x)))
                    || !(bankAccount.Text.All(x => x == ' ' || char.IsDigit(x))) 
                    || (hostKey.Text.Length != 9) || !(ID(((int.Parse(hostKey.Text))))) || (password.Text.Length != 8) || (name.Text.Length == 0) || (int.Parse(price.Text) <= 0))
                    {
                        MessageBox.Show("Please enter all details");
                        return;
                    }
                    BankBranch t = MainWindow.myBl.GetBranches().Find(x => x.BankName == Bname.SelectedValue.ToString() && x.BranchNumber == int.Parse(Bnum.SelectedValue.ToString()));
                    HostingUnit hostUnit = new HostingUnit
                    {
                        HostingUnitkey = long.Parse(unitKey.Text),
                        HostingUnitName = name.Text,
                        price = int.Parse(price.Text),
                        Uris = u.Uris,
                        Owner = new Host
                        {
                            BankAccountNumber=int.Parse(bankAccount.Text),
                            BankBranchDetails = t,
                            CollectionClearance = clearance.IsChecked.Value,
                            FamilyName = lastName.Text,
                            FhoneNumber = long.Parse(phoneNumber.Text),
                            HostKey = long.Parse(hostKey.Text),
                            MailAddress = mail.Text,
                            Password = long.Parse(password.Text),
                            PrivateName = firstName.Text,
                             
                        }
                    };
                    MainWindow.myBl.AddHostingUnit(hostUnit);
                    MessageBox.Show(hostUnit.Owner.PrivateName + " " + hostUnit.Owner.FamilyName + " " + "your hospitality unit added successfully to the system, your password is:  " + hostUnit.Owner.Password + "thank you");
                    Host w = new Host();
                    HostingUnit temp2 = new HostingUnit()
                    {
                        HostingUnitkey = MainWindow.myBl.GethostingUnitKey(),
                        Owner = w

                    };
                    host.DataContext = w;
                    unit.DataContext = t;
                }
                catch (BlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("User is allready exist,please log in");
        }
        private int SumDigits(int num)//פונקציה המחשבת סכום ספרות
        {
            int sum = 0;
            for (int i = 1; i <= 8; i++)
            {
                sum += num % 10;
                num = num / 10;
            }
            return (sum);
        }
        private bool ID(int number)//פונקצייה המחשבת ספרת ביקורת
        {

            int sum = 0;
            int a = number % 10;
            number /= 10;
            for (int i = 1; i < 5; i++)
            {
                int digit = number % 10;
                digit = digit * 2;
                sum = SumDigits(digit) + sum + (number / 10 % 10);
                number = number / 100;
            }
            int check_digit = 10 - (sum % 10);//חישוב סופי של ספרת ביקורת
            return (a == check_digit);

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string photoPath = @"..\..\..\hostingUnitsIm\" + firstName.Text + @".jpg";
            (File.Create(photoPath)).Close();
            System.IO.File.Copy(openFileDialog.FileName, photoPath, true);
            plus.IsEnabled = true;
            image.Source = new BitmapImage(new Uri("images/camera2.jpg", UriKind.Relative));
            MessageBox.Show("the picture added, to keep adding presse again","",MessageBoxButton.OK,MessageBoxImage.Information);
            save.IsEnabled = false;
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
                if (u.Uris == null)
                    u.Uris = new List<string>();
                u.Uris.AddRange(uris);
                image.Source = new BitmapImage(new Uri(uris[u.Uris.Count-1],UriKind.Absolute));
            }
            plus.IsEnabled = false;
            save.IsEnabled = true;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            pass.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            pass.Visibility = Visibility.Collapsed;
        }

          
    }
    }

