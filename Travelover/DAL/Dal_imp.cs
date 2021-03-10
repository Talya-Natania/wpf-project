using DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    class Dal_imp : Idal
    {
        public void AddHostingUnit(HostingUnit e)
        {
            var v = DataSource.listOfHostingUnits.Find(x => x.HostingUnitName == e.HostingUnitName);
            if (v == null)
            {
                e.HostingUnitkey = Configuration.hostingUnitKey++;
                DataSource.listOfHostingUnits.Add(e);
            }
            else
            {
                throw new DalException("There is allready hosting unit with this name.");
            }
          //  Console.WriteLine("Your hosting unit has been received by the system ");

        }
        public void AddOrder(Order e)
        {
            var v = DataSource.listOfOrders.Find(x => (x.GuestRequestKey == e.GuestRequestKey) && (x.HostingUnitkey == e.HostingUnitkey));
            if (v != null)
                throw new DalException("This order allready exists");
            e.OrderKey = Configuration.orderKey++;
            e.CreateDate = DateTime.Now;
            e.Status = OrderStatus.Open;
            DataSource.listOfOrders.Add(e);
         // Console.WriteLine("Your order has been received by the system ");

        }

        public void AddRequest(GuestRequest e)
        {
           
            e.GuestRequestKey = Configuration.requestKey++;
            DataSource.listOfRequests.Add(e);
           // Console.WriteLine("Your request has been received by the system ");
        }

        public void ChangeCommision(long orderKey, uint fee)
        {
           int i= DataSource.listOfOrders.FindIndex(x => x.OrderKey == orderKey);
            if (i == -1)
            {
                throw new DalException("The Order does not exist");
            }
            DataSource.listOfOrders[i].commission = fee;
        }

        public List<BankBranch> GetBranches()
        {
            List<BankBranch> branches = new List<BankBranch>
            {
                new BankBranch
                {
                    
                    BankName="Discont",
                    BankNumber=11,
                    BranchAddress="Neve Avivim",
                    BranchCity="Tel-Aviv",
                    BranchNumber=118,
                },
                new BankBranch
                {
                 
                  BankName="Jerusalem",
                  BankNumber=54,
                  BranchAddress="Yahuda Hanasi",
                  BranchCity="Elad",
                  BranchNumber=76
                },
                new BankBranch
                {
                 
                  BankName="Leumi",
                  BankNumber=10,
                  BranchAddress="King Salamon",
                  BranchCity="Natsrat",
                  BranchNumber=966,
                },
                   new BankBranch()
                {
                   
                   BankName="Pagi",
                   BankNumber=12,
                   BranchAddress="Histadrut 108",
                   BranchCity="Holon",
                   BranchNumber=785
                },
                new BankBranch
                {
                    
                    BankNumber=12,
                    BankName="Pagi",
                    BranchAddress="Harishonim 2",
                    BranchCity="Hadera",
                    BranchNumber=659
                }
            };
            return branches;
        }

        public uint GetExpiary()
        {
             return BE.Configuration.expiry;
        }

        public uint GetFee()
        {
            return BE.Configuration.commissionFee;
        }

        public long GethostingUnitKey()
        {
            throw new NotImplementedException();
        }

        public long GetManagerPassword()
        {
            throw new NotImplementedException();
        }

        public long GetorderKey()
        {
            return BE.Configuration.orderKey;
        }

        public List<Order> GetOrders()
        {
            Order[] ordersArr = new Order[DataSource.listOfOrders.Count];
            DataSource.listOfOrders.CopyTo(ordersArr);
            return ordersArr.ToList();
        }

        public long GetrequestKey()
        {
            return BE.Configuration.requestKey;
        }

        public List<GuestRequest> GetRequests()
        {
            GuestRequest[] requestsArr = new GuestRequest[DataSource.listOfRequests.Count];
            DataSource.listOfRequests.CopyTo(requestsArr);
            return requestsArr.ToList();
        }

        public List<HostingUnit> GetUnits()
        {
            HostingUnit[] unitsArr = new HostingUnit[DataSource.listOfHostingUnits.Count];
            DataSource.listOfHostingUnits.CopyTo(unitsArr);
            return unitsArr.ToList();
        }

        public void RemoveHostingUnit(HostingUnit e)
        {
            if (DataSource.listOfHostingUnits.Where(item => e.HostingUnitkey == item.HostingUnitkey).ToList().Count>0)
            {
                DataSource.listOfHostingUnits.Remove(e);
            }
            else
                throw new DalException("item isn't exists");
        }

        public void UpdateHostingUnit(HostingUnit e)
        {
           
            var v = DataSource.listOfHostingUnits.Where(item => e.HostingUnitkey == item.HostingUnitkey).FirstOrDefault();
            if (v == null)
                throw new DalException("the hosting unit doesnt exist");

            for (int i = 0; i < DataSource.listOfHostingUnits.Count; i++)
            {
                if (DataSource.listOfHostingUnits[i].Owner.HostKey == e.Owner.HostKey)
                    DataSource.listOfHostingUnits[i].Owner = e.Owner;
            }
            int n = DataSource.listOfHostingUnits.FindIndex(item => e.HostingUnitkey == item.HostingUnitkey);
            if (e.Uris == null)
            {
                e.Uris = DataSource.listOfHostingUnits[n].Uris;
            }
            e.Diary = DataSource.listOfHostingUnits[n].Diary;
            DataSource.listOfHostingUnits[n] = e;

        }

        public void UpdateOrder(long t, OrderStatus s)
            
        {
            if (s!= OrderStatus.closed|| s != OrderStatus.ClosedForCustomerResponse|| s != OrderStatus.ClosedForCustomerUnresponsiveness|| s != OrderStatus.EmailSent|| s != OrderStatus.NotAddressed|| s != OrderStatus.Open)
                throw new DalException("Please choose status");
            var v = DataSource.listOfOrders.Where(x => x.OrderKey == t).FirstOrDefault();
            if (v == null)
                throw new DalException("the order does not exists");
            v.Status = s;
        }

        public void UpdateReuest(long t, RequestStatus s)
        {
            var v = DataSource.listOfRequests.Where(x => x.GuestRequestKey == t).FirstOrDefault();
            if (v == null)
                throw new DalException("the request does not exists");
            v.Status = s;

        }

        string Idal.GetManagerPassword()
        {
            return BE.Configuration.ManagerPassword;
        }
    }
}
