using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        #region CRUD
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

        #endregion
        /// <summary>
        /// Returns the list of all available hosting units on this date
        /// </summary>
        List<HostingUnit> AvailableHostingUnits(DateTime time, int days);
        /// <summary>
        /// Returns a list of orders that the time since an email sent to the customer greater than or equal to the number of days the function received
        /// </summary>
        List<Order> OrdersByDays(int days);
        /// <summary>
        /// Returns the number of orders that sent to the customer
        /// </summary>
        int NumOfSug(GuestRequest g);
        /// <summary>
        /// Returns the number of successfully closed orders for this unit
        /// </summary>
        int NumOfOrders(HostingUnit h);
        /// <summary>
        /// Returns a list of guest requests grouped according to the required area
        /// </summary>
        List<GuestRequest> RequstsByArea(AreaChoise a);
        /// <summary>
        /// Returns a list of guest requests grouped according to the number of vacationer
        /// </summary>
        List<GuestRequest> RequstsByNum(int num);
        /// <summary>
        /// Returns a list of hosts grouped according to the number of hosting they hold
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        List<Host> HostByNum(int num);
        /// <summary>
        /// Returns a list of hosting units grouped according to the required area
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        List<HostingUnit> UnitByArea(AreaChoise a);
        /// <summary>
        /// Returns the annual occupancy of the hosting unit
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        int GetAnnualBusyDays(HostingUnit h);
        /// <summary>
        /// Returns the most popular area 
        /// </summary>
        /// <returns></returns>
        AreaChoise MostPopularArea();
        /// <summary>
        /// Returns the key of the host with the largest number of units
        /// </summary>
        /// <returns></returns>
        string ThePerfectHost();
        /// <summary>
        /// sorts the guest requests by the number of the vacationer
        /// </summary>
        /// <returns></returns>
        List<GuestRequest> SortRequests();
        /// <summary>
        /// Returns the key of the the cheapest unit
        /// </summary>
        /// <returns></returns>
        string TheLow();
        /// <summary>
        /// Returns all accommodation requirements with pool
        /// </summary>
        /// <returns></returns>
        List<GuestRequest> IsPool();
        /// <summary>
        /// return the fee which the manager get each month
        /// </summary>
        /// <param name="key"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// 
         uint FeebyMonth(int month);
        /// <summary>
        /// This function returns all the orders on the same hosting unit.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        List<Order> GetSpOrders(HostingUnit e);
        bool CheckHost(long key, long password);
        List<HostingUnit> ListOfUnits(long key);
        List<Order> GetOrdersByHost(Host h);
        /// <summary>
        /// The Function send an email with vacation suggestion 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        void SendMail(Order a, GuestRequest e);
        ///Get the keys:
        string GetManagerPassword();
        long GetorderKey();
        long GethostingUnitKey();
        long GetrequestKey();
        uint GetFee();
        uint GetExpiary();
        List<Order> OrdersByStatus(OrderStatus s);
        


    }
}