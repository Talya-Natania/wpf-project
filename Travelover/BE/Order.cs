using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public long HostingUnitkey { get; set; }
        public long GuestRequestKey { get; set; }
        public long OrderKey { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public uint commission { get; set; }
        public override string ToString() { return "Order"; }
    }
}
