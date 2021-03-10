using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******** Part 1 ********/
//Talya Yazdi 211976295 && Naama Arivi 314660168
namespace PL
{
    class Program
    {

        static void Main(string[] args)
        {
            BL.IBL bl = BL.FactoryBL.getBL();
            DateTime t = DateTime.Now;
            GuestRequest temp = new GuestRequest
            {
                FamilyName = "Levi",
                EntryDate = DateTime.Now,
                MailAddress = "VVGG",
                Adults = 2,
                Area = AreaChoise.All,
                Children = 3,
                ChildrensAttractions = Extra.Necessary,
                Garden = Extra.Possible,
                Jacuzzi = Extra.Possible,
                Pool = Extra.Necessary,
                PrivateName = "Aharon",
                RegistrationDate = DateTime.MaxValue,
                ReleaseDate = DateTime.Now.AddDays(7),
                Status = RequestStatus.Open,
                SubArea = "all",
                Type = ResortType.Zimmer,
            };
            Host host1 = new Host
            {
                HostKey = 211976295,
                PrivateName = "Talya",
                FamilyName = "Yazdi",
                FhoneNumber = 0543725708,
                
                CollectionClearance = true,
                MailAddress = "t@jct.ac.il"
                

            };
            bool[,] d = new bool[12, 31];

            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < 31; i++)
                    d[j, i] = false;
            }
            HostingUnit hosting1 = new HostingUnit
            {
                HostingUnitName = "FUN FUN FUN",
                Owner = host1,
                Area = AreaChoise.Jerusalem,
                Diary = d,
                price=1500
                
            };


            Order o = new Order();
            try
            {
                bl.AddRequest(temp);
                bl.AddHostingUnit(hosting1);
                o = new Order { HostingUnitkey = hosting1.HostingUnitkey, GuestRequestKey = temp.GuestRequestKey, OrderDate = t };
                bl.AddOrder(o);
                bl.UpdateReuest(temp.GuestRequestKey, RequestStatus.ClosedByWeb);
                bl.UpdateReuest(123456, RequestStatus.ClosedByWeb);

            }
            catch (BlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            HostingUnit hosting2 = new HostingUnit
            {
                HostingUnitkey = hosting1.HostingUnitkey,
                HostingUnitName = "FUN FUN",
                Owner = host1,
                Area = AreaChoise.Center,
                Diary = d,
                price=2000
            };
        
            try
            {
                bl.UpdateHostingUnit(hosting2);
                bl.UpdateOrder(o.OrderKey, OrderStatus.EmailSent);
                bl.UpdateOrder(o.OrderKey, OrderStatus.ClosedForCustomerResponse);
                bl.UpdateOrder(o.OrderKey, OrderStatus.EmailSent);

            }
            catch (BlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                bl.UpdateReuest(temp.GuestRequestKey, RequestStatus.ClosedByWeb);
                foreach (var item in bl.GetRequests())
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.GetOrders())
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.GetUnits())
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.GetBranches())
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.AvailableHostingUnits(temp.ReleaseDate.AddDays(5), 5))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.OrdersByDays(3))
                {
                    Console.WriteLine(item);

                }
                foreach (var item in bl.OrdersByDays(0))
                {
                    Console.WriteLine(item);

                }
                Console.WriteLine(bl.NumOfSug(temp));
                Console.WriteLine(bl.NumOfOrders(hosting1));
                foreach (var item in bl.RequstsByArea(AreaChoise.All))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.RequstsByArea(AreaChoise.Center))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.UnitByArea(AreaChoise.Center))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.UnitByArea(AreaChoise.Jerusalem))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.RequstsByNum(5))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.RequstsByNum(7))
                {
                    Console.WriteLine(item);
                }
                foreach (var item in bl.HostByNum(2))
                {
                    Console.WriteLine(item);
                }
                bl.AddHostingUnit(hosting1);
                foreach (var item in bl.HostByNum(2))
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine(bl.MostPopularArea());
                Console.WriteLine(bl.ThePerfectHost());
                bl.RemoveHostingUnit(hosting2);
                foreach (var item in bl.GetUnits())
                {
                    Console.WriteLine(item);
                }
                bl.RemoveHostingUnit(hosting1);
                foreach (var item in bl.GetUnits())
                {
                    Console.WriteLine(item);
                }

               
                foreach (var item in bl.GetUnits())

                {
                    Console.WriteLine(item);
                }
                List<GuestRequest> lst = bl.SortRequests();
                foreach (var item in lst)
                {
                    if (item!=null)
                    Console.WriteLine(item);
                }
                foreach (var item in bl.IsPool())
                {
                    Console.WriteLine(item);
                }
                bl.RemoveHostingUnit(hosting2);
                
            }

            catch (BlException ex)
            {
                Console.WriteLine(ex.Message);
            }

           

        }

    }
}
