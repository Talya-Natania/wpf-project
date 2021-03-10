using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface Idal
    {
        /// <summary>
        ///  Add GuestRequest
        /// </summary>
        /// <param name="e"></param>
        void AddRequest(GuestRequest e);
        /// <summary>
        /// Update HostingUnit(change the status)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        void UpdateReuest(long t, RequestStatus s);
        /// <summary>
        /// Adding HostingUnit
        /// </summary>
        /// <param name="e"></param>
        void AddHostingUnit(HostingUnit e);
        /// <summary>
        /// Remove the unit
        /// </summary>
        /// <param name="e"></param>
        void RemoveHostingUnit(HostingUnit e);
        /// <summary>
        /// Update Hosting Unit
        /// </summary>
        /// <param name="e"></param>
        void UpdateHostingUnit(HostingUnit e);
        /// <summary>
        /// Adding order
        /// </summary>
        /// <param name="e"></param>
        void AddOrder(Order e);
        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        void UpdateOrder(long t, OrderStatus s);
        /// <summary>
        /// Getting list of hosting units
        /// </summary>
        /// <returns></returns>
        List<HostingUnit> GetUnits();//  
        /// <summary>
        /// Getting list of guest requests
        /// </summary>
        /// <returns></returns>
        
        List<GuestRequest> GetRequests();
        /// <summary>
        /// Getting list of orders
        /// </summary>
        /// <returns></returns>
        List<Order> GetOrders();
        /// <summary>
        /// Getting list of bank branches 
        /// </summary>
        /// <returns></returns>
        List<BankBranch> GetBranches();

        string GetManagerPassword();
        long GetorderKey();
        long GethostingUnitKey();
        long GetrequestKey();
        uint GetFee();
        uint GetExpiary();
        void ChangeCommision(long orderKey, uint fee);


    }
}
