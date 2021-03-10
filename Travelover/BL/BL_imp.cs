using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using System.Collections;
using System.Net.Mail;

namespace BL
{

    class BL_imp : IBL
    {
        Idal dal = FactoryDal.getDal();

        public void AddHostingUnit(HostingUnit e)
        {
            try
            {
                dal.AddHostingUnit(e);
            }
            catch(DalException ex)
            {
                throw new BlException(ex.Message);
            }
           
        }
        public void AddOrder(Order e)
        {
            try
            {
                if (IfAvailable(e.GuestRequestKey, e.HostingUnitkey) && e.HostingUnitkey <= dal.GethostingUnitKey()&& e.GuestRequestKey <= dal.GetrequestKey())
                {

                    dal.AddOrder(e);
                }
                else
                    throw new BlException("The hosting unit is unavailable on these dates");
            }
            catch(DalException ex)
            {
                throw new BlException(ex.Message);
            }
        }
        public void AddRequest(GuestRequest e)
        {
            if (e.ReleaseDate > DateTime.Now.AddMonths(11))
                throw new BlException("out of range");
            if (e.EntryDate >= e.ReleaseDate)
                throw new BlException("The Release date must be later then the entry date");
            dal.AddRequest(e);
        }

        public List<HostingUnit> AvailableHostingUnits(DateTime time, int days)
        {
            var v = from n in dal.GetUnits()
                    where CheckDates(n, time, time.AddDays(days))
                    select n;
            List<HostingUnit> list = new List<HostingUnit>();
            foreach (HostingUnit item in v)
            {
                list.Add(item);
            }
            return list;
        }

        public List<GuestRequest> IsPool()
        {
            return Conditions(Pool);

        }
    
        public List<BankBranch> GetBranches()
        {
            return dal.GetBranches();
        }

        
        public List<Order> GetOrders()
        {
            return dal.GetOrders();
        }

        public List<GuestRequest> GetRequests()
        {
            return dal.GetRequests();
        }

        public List<HostingUnit> GetUnits()
        {
            return dal.GetUnits();
        }

        
        public int NumOfOrders(HostingUnit h)
        {
            var v = from n in dal.GetOrders()
                    let n2 = h.HostingUnitkey
                    where n.HostingUnitkey == n2
                    select n;
            int counter = 0;
            foreach (Order item in v)
            {
                if (item.Status == OrderStatus.ClosedForCustomerResponse || item.Status == OrderStatus.EmailSent)
                    counter++;
            }
            return counter;
        }

        public int NumOfSug(GuestRequest g)
        {
            var v = from n in dal.GetOrders()
                    where n.GuestRequestKey == g.GuestRequestKey
                    select n;
            int counter = 0;
            foreach (Order item in v)
            {
                if (item.Status == OrderStatus.ClosedForCustomerResponse || item.Status == OrderStatus.EmailSent || item.Status == OrderStatus.ClosedForCustomerUnresponsiveness)
                    counter++;
            }
            return counter;

        }
        public List<Order> OrdersByStatus(OrderStatus s)
        {
            var v = from n in dal.GetOrders()
                    let temp = n.Status
                    group n by temp into g
                    select g;
            List<Order> lst = new List<Order>();
            foreach (var t in v)
            {
                if (t.Key == s)
                    foreach (var item in t)
                    {
                        lst.Add(item);
                    }
            }
            return lst;

        }
        public List<Order> OrdersByDays(int days)
        {
            var v = from n in dal.GetOrders()
                    where ((GetDaysBetween(n.CreateDate) >= days || GetDaysBetween(n.OrderDate) >= days))
                    select n;
            List<Order> list = new List<Order>();
            foreach (var t in v)
                list.Add(t);
            return list;
        }

        public void RemoveHostingUnit(HostingUnit e)
        {
            List<Order> list = dal.GetOrders();
            IEnumerable<Order> v2 = list.Where(x => x.HostingUnitkey == e.HostingUnitkey);
            foreach (Order item in v2)
            {
                if (item.Status == OrderStatus.NotAddressed)
                {
                    throw new BlException("The hosting unit can't be removed- there is an open order");
                }
            }
            try
            {
                dal.RemoveHostingUnit(e);
            }
            catch (DalException ex)
            {
                throw new BlException(ex.Message);
            }

        }
        public List<Order> GetSpOrders(HostingUnit e)
        {
            List<Order> lst = dal.GetOrders();
            var v = from n in dal.GetOrders()
                    where (n.HostingUnitkey == e.HostingUnitkey)
                    select n;
            lst.Clear();
            foreach (var item in v)
            {
                lst.Add(item);
            }
            return lst;
        }
    

        public void UpdateHostingUnit(HostingUnit e)
        {
            var c = dal.GetUnits().Where(x => x.HostingUnitkey == e.HostingUnitkey).FirstOrDefault();
            if (c == null)
                throw new BlException("The hosting unit does not exist");
            //If the host want to cancel the clearance-check if there are open requests
            if (e.Owner.CollectionClearance == false && c.Owner.CollectionClearance == true)
            {
                var v = from n in GetSpOrders(e)
                        from n2 in dal.GetRequests()
                        where (n2.GuestRequestKey == n.GuestRequestKey && n2.Status == RequestStatus.Open)
                        select n2;
                if (v.ToList().Count > 0)
                    throw new BlException("There are open requests");
            }
            try
            {
                dal.UpdateHostingUnit(e);
            }
            catch (DalException ex)
            {
                throw new BlException(ex.Message);
            }
        }

        public void UpdateOrder(long t, OrderStatus s)
        {
            try
            {
                if (dal.GetOrders().Find(delegate (Order x) { return (t == x.OrderKey); }).Status == OrderStatus.ClosedForCustomerResponse)
                    throw new BlException("the Order had already cloesd,the status can not be changed");
                var v = dal.GetOrders().Where(x => x.OrderKey == t).FirstOrDefault();
                if (v == null)
                    throw new BlException("the order does not exist");
                if (s == OrderStatus.EmailSent)
                {
                    if (!IfAvailable(v.GuestRequestKey, v.HostingUnitkey))
                        throw new BlException("The dates are not available");

                    if (dal.GetUnits().Find(x => v.HostingUnitkey == x.HostingUnitkey).Owner.CollectionClearance)
                    {
                        dal.UpdateOrder(t, s);
                        dal.GetOrders().Find(x => v.HostingUnitkey == x.HostingUnitkey).OrderDate = DateTime.Now;
                        return;
                    }
                }
                if (s == OrderStatus.ClosedForCustomerResponse)
                {

                    Order temp = dal.GetOrders().Find(x => v.GuestRequestKey == x.GuestRequestKey);
                    if (temp == null)
                        throw new BlException("The order isnt exist");
                  if(!IfAvailable(temp.GuestRequestKey,temp.HostingUnitkey))
                        throw new BlException("The dates are not available");

                    foreach (var item in dal.GetOrders())
                    {
                        if (item.GuestRequestKey == temp.GuestRequestKey)
                            item.Status = OrderStatus.closed;
                    }
                    
                    UpdateReuest(v.GuestRequestKey, RequestStatus.ClosedByWeb);
                    try
                    {
                        dal.UpdateOrder(t, s);
                    }
                    catch (DalException ex)
                    {
                        throw new BlException(ex.Message);
                    }
                    MarkingDiary(temp.GuestRequestKey, dal.GetUnits().Find(x => v.HostingUnitkey == x.HostingUnitkey).HostingUnitkey);
                    GuestRequest help = dal.GetRequests().Find(y => y.GuestRequestKey == temp.GuestRequestKey);
                    
                    ChangeCommision(t, dal.GetFee() * GetDaysBetween(help.EntryDate, help.ReleaseDate));
                    

                }

            }
            catch (DalException e)
            {
                throw new BlException(e.Message);
            }

        }
        public void UpdateReuest(long t, RequestStatus s)
        {
            try
            {
                dal.UpdateReuest(t, s);
            }
            catch (DalException ex)
            {
                throw new BlException(ex.Message);
            }
        }

        public List<GuestRequest> RequstsByArea(AreaChoise a)
        {
            if (a == AreaChoise.All)
                return dal.GetRequests();
            var v = from n in dal.GetRequests()
                    let temp = n.Area
                    group n by temp into g
                    select g;
            List<GuestRequest> lst = new List<GuestRequest>();
            foreach (var t in v)
            {
                if (t.Key == a)
                    foreach (var item in t)
                    {
                        lst.Add(item);
                    }
            }
            return lst;
        }
   
        public List<GuestRequest> RequstsByNum(int num)
        {
            var v = from n in dal.GetRequests()
                    let numOfPepole = n.Children + n.Adults
                    group n by numOfPepole into g
                    select g;
            List<GuestRequest> lst = new List<GuestRequest>();
            foreach (var g in v)
            {
                if (g.Key == num)
                    foreach (var item in g)
                    {
                        lst.Add(item);
                    }
            }
            return lst;

        }
        public List<Host> HostByNum(int num)
        {
            var v = from n in dal.GetUnits()
                    let numOfUnits = NumOfUnits(n.Owner.HostKey)
                    group n by numOfUnits into g
                    select g;
            List<Host> lst = new List<Host>();
            foreach (IGrouping<int, HostingUnit> g in v)
            {
                if (g.Key == num)
                    foreach (var item in g)
                    {
                        lst.Add(item.Owner);
                    }
            }
            return lst;
        }
        public List<HostingUnit> UnitByArea(AreaChoise t)
        {
            var v = from n in dal.GetUnits()
                    let a = n.Area
                    group n by a into g
                    select g;
            List<HostingUnit> lst = new List<HostingUnit>();
            foreach (var g in v)
            {
                if (g.Key == t)
                    foreach (var item in g)
                    {
                        lst.Add(item);
                    }
            }
            return lst;
        }
        public AreaChoise MostPopularArea()
        {
            List<GuestRequest> l1 = ClosedRequstsByArea(AreaChoise.Center);
            List<GuestRequest> l2 = ClosedRequstsByArea(AreaChoise.Jerusalem);
            List<GuestRequest> l3 = ClosedRequstsByArea(AreaChoise.North);
            List<GuestRequest> l4 = ClosedRequstsByArea(AreaChoise.South);
            if (l1.Count > l2.Count && l1.Count > l3.Count && l1.Count > l4.Count)
                return AreaChoise.Center;
            else if (l2.Count > l1.Count && l2.Count > l3.Count && l2.Count > l4.Count)
                return AreaChoise.Jerusalem;
            else if (l3.Count > l1.Count && l3.Count > l2.Count && l3.Count > l4.Count)
                return AreaChoise.North;
            else if (l4.Count > l1.Count && l4.Count > l2.Count && l4.Count > l3.Count)
                return AreaChoise.South;
            else throw new BlException("There is no popular area meanwhile");
        }

        public int GetAnnualBusyDays(HostingUnit h)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 31; j++)
                {
                    if (h.Diary[i, j])
                        sum++;
                }
            return sum;
        }
        public string ThePerfectHost()
        {
            float max = -1;
            string perfect = "";
            foreach (var item in dal.GetUnits())
            {
                if (GetAnnualBusyDays(item) > max)
                {
                    perfect = item.Owner.PrivateName+"  "+item.Owner.FamilyName;
                    max = GetAnnualBusyDays(item);
                }
            }
            return perfect;
        }
        public List<GuestRequest> SortRequests()
        {
            GuestRequest[] t = new GuestRequest[30];
            List<GuestRequest> temp = dal.GetRequests();
            var v = from n in dal.GetRequests()
                    select new
                    {
                        Num = n.Adults + n.Children
                    };
            foreach (var item in v)
            {
                int count = 0;
                if (t[item.Num + count] == null)
                {
                    t[item.Num + count] = temp.First();
                    temp.RemoveAt(0);
                }
                else
                {
                    while (t[item.Num + count] != null)
                        count++;
                    t[item.Num + count] = temp.First();
                    temp.RemoveAt(0);
                }
            }
            return t.ToList();
        }

        public string TheLow()
        {
            int min = dal.GetUnits().FirstOrDefault().price;
            string key = "";
            foreach (var v in dal.GetUnits())
                if (min >= v.price)
                {
                    key = v.HostingUnitName;
                    min = v.price;
                }
            return key;
        }
        /// <summary>
        /// The function  return all customer requirements that fit a particular condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private List<GuestRequest> Conditions(Predicate<GuestRequest> predicate)
        {
            var v = from n in dal.GetRequests()
                    where predicate(n)
                    select n;
            List<GuestRequest> list = new List<GuestRequest>();
            foreach (GuestRequest item in v)
            {
                list.Add(item);
            }
            return list;
        }
        //Predicate
        private bool Pool(GuestRequest g)
        {
            return (g.Pool == Extra.Necessary);
        }
        /// <summary>
        /// The function get one or two dates and return the number of days between them.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private uint GetDaysBetween(params DateTime[] arr)
        {
            if (arr.Length > 2 || arr.Length < 1)
                throw new BlException("one or two dates");
            uint count = 0;
            DateTime to, from;
            //if the function get just one date -return the nubers of days from/till today
            if (arr.Length == 1)
            {
                if (arr[0] > DateTime.Now) { to = arr[0]; from = DateTime.Now; }
                else { to = DateTime.Now; from = arr[0]; }
                for (DateTime tmp = from; tmp < to; tmp = tmp.AddDays(1))
                    count++;

            }
            else
            {
                if (arr[0] > arr[1]) { to = arr[0]; from = arr[1]; }
                else { to = arr[1]; from = arr[0]; }
                for (DateTime tmp = from; tmp < to; tmp = tmp.AddDays(1))
                    count++;
            }
            return count - 1;
        }
        /// <summary>
        /// The function check if the hosting unit is available according to the guest request
        /// </summary>
        /// <param name="t"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool IfAvailable(long t, long e)
        {
            List<GuestRequest> list = dal.GetRequests();
            var v = list.Where(x => x.GuestRequestKey == t).FirstOrDefault();
            if (v == null)
                throw new BlException("The guest request does not exist");
            DateTime dt = v.EntryDate;
            DateTime dt2 = v.ReleaseDate;
            List<HostingUnit> list2 = dal.GetUnits();
            var n = list2.Where(y => y.HostingUnitkey == e).FirstOrDefault();
            return CheckDates(n, dt, dt2);
        }
        /// <summary>
        ///  The function check if the hosting unit is available in the date
        /// </summary>
        /// <param name="h"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool CheckDates(HostingUnit h, DateTime from, DateTime to)
        {
            bool flag = true;
            for (DateTime tmp = from.AddDays(1); tmp < to; tmp = tmp.AddDays(1))
            {
                if (h.Diary[tmp.Month - 1, tmp.Day - 1])
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        /// <summary>
        /// Marking the diary when close an order
        /// </summary>
        /// <param name="t"></param>
        /// <param name="n"></param>
        private void MarkingDiary(long t, long n)
        {
            bool[,] d = dal.GetUnits().Find(y => y.HostingUnitkey == n).Diary;
            GuestRequest tmp = dal.GetRequests().Find(x => t == x.GuestRequestKey);
            for (DateTime dt = tmp.EntryDate; dt < tmp.ReleaseDate; dt = dt.AddDays(1))
            {
                d[dt.Month - 1, dt.Day - 1] = true;
            }
            HostingUnit h = dal.GetUnits().Find(y => y.HostingUnitkey == n);
            h.Diary = d;
            dal.UpdateHostingUnit(h);
        }
        /// <summary>
        /// return the closed guest request in each area
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private List<GuestRequest> ClosedRequstsByArea(AreaChoise a)
        {
            var v = from n in dal.GetRequests()
                    let temp = n.Area
                    group n by temp into g
                    select g;
            List<GuestRequest> lst = new List<GuestRequest>();
            foreach (var g in v)
            {
                if (g.Key == a)
                    foreach (var item in g)
                    {
                        if (item.Status == RequestStatus.ClosedByWeb)
                            lst.Add(item);
                    }
            }
            return lst;

        }
         /// <summary>
         /// The function return the number of units of the host
         /// </summary>
         /// <param name="key"></param>
         /// <returns></returns>
        private int NumOfUnits(long key)
        {
            var v = from n in dal.GetUnits()
                    where n.Owner.HostKey == key
                    select n;
            return v.ToList().Count;
        }
        public List<HostingUnit> ListOfUnits(long key)
        {
            var v = from n in dal.GetUnits()
                    where n.Owner.HostKey == key
                    select n;
            return v.ToList();
        }
        /// <summary>
        /// Function that check if there is host in system and if his password is correct
        /// </summary>
        /// <returns></returns>
        public bool CheckHost(long key,long password)
        {
            var v = dal.GetUnits().Where(x => x.Owner.HostKey == key).FirstOrDefault();
            if (v != null)
            {
                if (v.Owner.Password == password)
                    return true;
            }
            return false;
            
        }
       
        public List<Order> GetOrdersByHost(Host h)
        {
            var v = from n in dal.GetUnits()
                    from t in dal.GetOrders()
                    where (n.Owner.HostKey == h.HostKey) && (t.HostingUnitkey == n.HostingUnitkey) 
                    select t;
            return v.ToList();
            

        }
        public uint FeebyMonth(int month)
        {
            uint counter = 0;
            foreach (Order item in dal.GetOrders())
            {
                var v = dal.GetRequests().Where(x => x.GuestRequestKey == item.GuestRequestKey).FirstOrDefault();
                if (v != null && v.EntryDate.Month == month)

                    counter += item.commission;
            }
            return counter;
        }
        public void SendMail(Order a, GuestRequest e)
        {
            ///keep the email of the host
            HostingUnit hostingUnit = GetUnits().Find(x => x.HostingUnitkey == a.HostingUnitkey);
            string Hmail = hostingUnit.Owner.MailAddress;     
            a.OrderDate = DateTime.Now;
            MailMessage mail = new MailMessage();
            string email = e.MailAddress;
            mail.To.Add(email);
            mail.From = new MailAddress("vacationsugg@gmail.com");
            mail.Subject = "Vacation Offer";
            mail.Body = "Hi whatsupp?:)" +
                "Here from TraveLover" +
                 "We read your request for vacation and have suitable suggestion for you" +
                 "if you would like to here the details ..." +
                 "here is my email to keep in touch: " + Hmail +
                 "Thank you,here for any question "+hostingUnit.Owner.PrivateName+" "+hostingUnit.Owner.FamilyName+".";


            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("vacationsugg@gmail.com", "travel111");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            smtp.Dispose();
        }

        public string GetManagerPassword()
        {
            return dal.GetManagerPassword();
        }

        public long GetorderKey()
        {
            return dal.GetorderKey();
        }

        public long GethostingUnitKey()
        {
            return dal.GethostingUnitKey();
        }

        public long GetrequestKey()
        {
            return dal.GetrequestKey();
        }

        public uint GetFee()
        {
            return dal.GetFee();
        }

        public uint GetExpiary()
        {
            return dal.GetExpiary();
        }
         private void ChangeCommision(long orderKey, uint fee)
        {
            try{
                dal.ChangeCommision(orderKey, fee);
            }
            catch (DalException ex)
            {
                throw new BlException(ex.Message);
            }
        }
    }
}

