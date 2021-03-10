using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum ResortType { Zimmer, Apartment, Hotel, Camping }
    public enum AreaChoise { All, North, South, Jerusalem, Center }
    public enum OrderStatus {Open, closed, NotAddressed, EmailSent, ClosedForCustomerUnresponsiveness, ClosedForCustomerResponse }
    public enum RequestStatus { Open, ClosedByWeb, Expired }
    public enum Extra { Necessary, Possible, Uninterested }
    public enum Month { January =1, February , March , April , May , June , July ,August , September , October , November , December }
}

