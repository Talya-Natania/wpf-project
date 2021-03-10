using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;

namespace DAL
{
    class Dal_XML_imp : Idal
    {

        XElement OrderRoot;
        XElement BankRoot;
        string OPath = @"Order.xml";
        XElement ConfigRoot;
        string CPath = @"config.xml";
        string HPath = @"HostingUnit.xml";
        string RPath = @"Request.xml";
        string BPath = @"atm.xml";
        string helpPath = @"help.xml";
        XElement helpRoot;
        Thread Worker;
        static bool flag = false;
        public Dal_XML_imp()
        {
            Worker = new Thread(bank);
            Worker.Start();
            Thread t = new Thread(check);
            t.Start();

            if (!File.Exists(HPath))
            {
                FileStream file = new FileStream(HPath, FileMode.Create);
                file.Close();
            }
            if (!File.Exists(RPath))
            {
                FileStream file = new FileStream(RPath, FileMode.Create);
                file.Close();
            }
            if (!File.Exists(OPath))
            {
                OrderRoot = new XElement("orders");
                OrderRoot.Save(OPath);
            }
            else
            {
                try
                {
                    OrderRoot = XElement.Load(OPath);

                }
                catch
                {
                    throw new KeyNotFoundException("file upload problem");
                }
            }
            if (!File.Exists(CPath))
            {
                ConfigRoot = new XElement("Configuration");
                ConfigRoot.Add(new XElement("requestKey", 10000000));
                ConfigRoot.Add(new XElement("hostingUnitKey", 10000000));
                ConfigRoot.Add(new XElement("orderKey", 10000000));
                ConfigRoot.Add(new XElement("ManagerPassword", 12345678));
                ConfigRoot.Add(new XElement("expiry", 30));
                ConfigRoot.Add(new XElement("commissionFee", 10));
                ConfigRoot.Save(CPath);
            }
            else
            {
                try
                {
                    ConfigRoot = XElement.Load(CPath);

                }
                catch
                {
                    throw new KeyNotFoundException("file upload problem");
                }
            }
            helpRoot = XElement.Load(helpPath);
        }
        private void bank()
        {
            while (flag == false)
            {
                WebClient wc = new WebClient();
                try
                {
                    string xmlServerPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                    wc.DownloadFile(xmlServerPath, BPath);
                    BankRoot = XElement.Load(BPath);
                    flag = true;
                }
                catch (Exception)
                {
                    try
                    {
                        string xmlServerPath = @"http://homedir.jct.ac.il/~coshri/atm.xml";
                        wc.DownloadFile(xmlServerPath, BPath);
                        BankRoot = XElement.Load(BPath);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(2000);
                        flag = false;
                    }
                }
                finally
                {
                    wc.Dispose();
                }
            }

        }
        private BankBranch ConvertBankBranch(XElement element)
        {
            return new BankBranch()
            {
                BankNumber = int.Parse(element.Element("קוד_בנק").Value),
                BankName = element.Element("שם_בנק").Value,
                BranchNumber = int.Parse(element.Element("קוד_סניף").Value),
                BranchAddress = element.Element("כתובת_ה-ATM").Value,
                BranchCity = element.Element("ישוב").Value
            };
        }
        private void check()
        { 
        //    while (true)
        //    {
        //        foreach (var item in GetOrders())
        //        {
        //            if (item.OrderDate.AddDays(30) > DateTime.Now)
        //                item.Status = OrderStatus.ClosedForCustomerUnresponsiveness;
        //        }
        //        Thread.Sleep(864000000);
        //    }
        }

        public List<BankBranch> GetBranches()
        {
            if (BankRoot != null)
            {
                return (from item in BankRoot.Elements()
                        let a = ConvertBankBranch(item)
                        select a).ToList();
            }
            else
            {
                return (from item in helpRoot.Elements()
                        let a = ConvertBankBranch(item)
                        select a).ToList();
            }
        }

        private void LoadOrder()
        {
            try
            {
                OrderRoot = XElement.Load(OPath);
            }
            catch
            {
                throw new DalException("File upload problem ");
            }
        }
        public long GetrequestKey()
        {
            return long.Parse(ConfigRoot.Element("requestKey").Value);
        }
        public long GethostingUnitKey()
        {
            return long.Parse(ConfigRoot.Element("hostingUnitKey").Value);
        }
        public long GetorderKey()
        {
            return long.Parse(ConfigRoot.Element("orderKey").Value);
        }
        public string GetManagerPassword()
        {
            return ConfigRoot.Element("ManagerPassword").Value;
        }

        private void Loadconfig()
        {
            try
            {
                ConfigRoot = XElement.Load(CPath);
            }
            catch
            {
                throw new DalException("File upload problem ");
            }
        }
        public List<Order> GetOrders()
        {
            LoadOrder();
            List<Order> orders;
            try
            {
                orders = (from p in OrderRoot.Elements()
                            select new Order()
                            {
                                HostingUnitkey = long.Parse(p.Element("HostingUnitkey").Value),
                                GuestRequestKey = long.Parse(p.Element("GuestRequestKey").Value),
                                commission=uint.Parse(p.Element("commission").Value),
                                 CreateDate=DateTime.Parse(p.Element("CreateDate").Value),
                                  OrderDate= DateTime.Parse(p.Element("OrderDate").Value),
                                 OrderKey = long.Parse(p.Element("OrderKey").Value),
                                  Status= (OrderStatus)Enum.Parse(typeof(OrderStatus),p.Element("Status").Value),

                            }).ToList();
            }
            catch
            {
                orders = null;
            }
            return orders;
        }
        public static void SaveListToXML<T>(T list, string path)
        {
            XmlSerializer x = new XmlSerializer(list.GetType());
            FileStream fs = new FileStream(path, FileMode.Create);
            x.Serialize(fs, list);
            fs.Close();
        }
        public static List<T> LoadListFromXML<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                List<T> list = (List<T>)x.Deserialize(fs);
                fs.Close();
                return list;
            }
            catch
            {
                fs.Close();
                return new List<T>();
            }
        }

        public void AddRequest(GuestRequest e)
        {
            
            List<GuestRequest> G_list = LoadListFromXML<GuestRequest>(RPath);

            var v = G_list.Where(item => item.GuestRequestKey == e.GuestRequestKey);
            if (v == null)
                throw new Exception("the guest request is already exists ");
            else
            {
                e.Status = RequestStatus.Open;
                e.RegistrationDate = DateTime.Now;
                long temp = long.Parse(ConfigRoot.Element("requestKey").Value);
                e.GuestRequestKey = temp++;
                ConfigRoot.Element("requestKey").Value = temp.ToString();
                ConfigRoot.Save(CPath);
                G_list.Add(e);
                SaveListToXML<List<GuestRequest>>(G_list, RPath);
            }
        }

        public void UpdateReuest(long t, RequestStatus s)
        {
            List<GuestRequest> G_list = LoadListFromXML<GuestRequest>(RPath);
            int v = G_list.FindIndex(x => x.GuestRequestKey == t);
            if (v == -1)
                throw new DalException("the request does not exists");
            G_list[v].Status = s;
            SaveListToXML<List<GuestRequest>>(G_list, RPath);
        }

        public void AddHostingUnit(HostingUnit e)
        {
            List<HostingUnit> list_unit = LoadListFromXML<HostingUnit>(HPath);
            var v = list_unit.Find(x => x.HostingUnitName == e.HostingUnitName);
            if (v == null)
            { 
               //XElement HostingUnitkey = new XElement("hostingUnitKey", ConfigRoot.Element("hostingUnitKey").Value);
                long temp = long.Parse(ConfigRoot.Element("hostingUnitKey").Value);
                e.HostingUnitkey = temp;
                temp++;
                
                ConfigRoot.Element("hostingUnitKey").Value = temp.ToString();
                ConfigRoot.Save(CPath);
                bool[,] D = new bool[13, 31];
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 31; j++)
                    {
                        D[i, j] = false;
                    }

                }
               
                e.Diary = D;
                list_unit.Add(e);
            }
            else
            {
                throw new DalException("There is allready hosting unit with this name.");
            }
            SaveListToXML<List<HostingUnit>>(list_unit, HPath);
        }

        public void RemoveHostingUnit(HostingUnit e)
        {
            List<HostingUnit> list_unit = LoadListFromXML<HostingUnit>(HPath);
            if (list_unit.Where(item => e.HostingUnitkey == item.HostingUnitkey).ToList().Count > 0)
            {

                list_unit.RemoveAll(item => e.HostingUnitkey == item.HostingUnitkey);
            }
            else
                throw new DalException("item isn't exists");
            SaveListToXML<List<HostingUnit>>(list_unit, HPath);
        }

        public void UpdateHostingUnit(HostingUnit e)
        {
            List<HostingUnit> list_unit=LoadListFromXML<HostingUnit> (HPath);
            var v = list_unit.Where(item => e.HostingUnitkey == item.HostingUnitkey).FirstOrDefault();
            if (v == null)
                throw new DalException("the hosting unit doesnt exist");

            for (int i = 0; i < list_unit.Count; i++)
            {
                if (list_unit[i].Owner.HostKey == e.Owner.HostKey)
                    list_unit[i].Owner = e.Owner;
            }
            int n = list_unit.FindIndex(item => e.HostingUnitkey == item.HostingUnitkey);
            if (e.Uris == null)
            {
                e.Uris = list_unit[n].Uris;
            }
           // e.Diary = list_unit[n].Diary;
            list_unit[n] = e;
            SaveListToXML<List<HostingUnit>>(list_unit,HPath);
        }

        public void AddOrder(Order e)
        {
            try
            {
                LoadOrder();
                Loadconfig();
            }
            catch (DalException eX)
            {
                throw new DalException(eX.Message);
            }
            var it = (from item in OrderRoot.Elements()
                      where (long.Parse(item.Element("HostingUnitkey").Value) == e.HostingUnitkey) && long.Parse(item.Element("GuestRequestKey").Value) == e.GuestRequestKey
                      select item).FirstOrDefault();
            if (it != null)
                throw new DalException("This order allready exists");
            XElement HostingUnitkey = new XElement("HostingUnitkey", e.HostingUnitkey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", e.GuestRequestKey);
            XElement OrderDate = new XElement("OrderDate", e.OrderDate);
            XElement OrderKey = new XElement("OrderKey", ConfigRoot.Element("orderKey").Value);
            XElement Status = new XElement("Status", OrderStatus.Open);
            XElement commission = new XElement("commission", e.commission);
            XElement CreateDate = new XElement("CreateDate", DateTime.Now);
            XElement order = new XElement("Order", HostingUnitkey, GuestRequestKey, OrderDate, OrderKey, Status, commission, CreateDate);
            OrderRoot.Add(order);
            long temp = long.Parse(ConfigRoot.Element("orderKey").Value);
            temp++;
            ConfigRoot.Element("orderKey").Value = temp.ToString();
            ConfigRoot.Save(CPath);
            OrderRoot.Save(OPath);
        }

        public void UpdateOrder(long t, OrderStatus s)
        {
            //if (s != OrderStatus.closed || s != OrderStatus.ClosedForCustomerResponse || s != OrderStatus.ClosedForCustomerUnresponsiveness || s != OrderStatus.EmailSent || s != OrderStatus.NotAddressed || s != OrderStatus.Open)
            //    throw new DalException("Please choose status");
            XElement orderElement = (from item in OrderRoot.Elements()
                                     where (long.Parse(item.Element("OrderKey").Value) == t)
                                     select item).FirstOrDefault();
            if (orderElement == null)
                throw new DalException("the order does not exists");
            orderElement.Element("Status").Value = s.ToString();
            OrderRoot.Save(OPath);
        }

        public List<HostingUnit> GetUnits()
        {
            List<HostingUnit> list_unit = LoadListFromXML<HostingUnit>(HPath);
            HostingUnit[] unitsArr = new HostingUnit[list_unit.Count];
            list_unit.CopyTo(unitsArr);
            return unitsArr.ToList();
        }

        public List<GuestRequest> GetRequests()
        {
            List<GuestRequest> list_unit = LoadListFromXML<GuestRequest>(RPath);
            GuestRequest[] unitsArr = new GuestRequest[list_unit.Count];
            list_unit.CopyTo(unitsArr);
            return unitsArr.ToList();
            
        }
       public void ChangeCommision(long orderKey, uint fee)
        {
            XElement OrderElement = (from p in OrderRoot.Elements()
                                     where long.Parse(p.Element("OrderKey").Value) == orderKey
                                     select p).FirstOrDefault();
            if (OrderElement == null)
            {
                throw new DalException("The Order does not exist");
            }
            OrderElement.Element("commission").Value = fee.ToString();
            OrderRoot.Save(OPath);
        }
        public uint GetFee()
        {
            return uint.Parse(ConfigRoot.Element("commissionFee").Value);
        }

        public uint GetExpiary()
        {
            return uint.Parse(ConfigRoot.Element("expiary").Value);
        }
    }
}
