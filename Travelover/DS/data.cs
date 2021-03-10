using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        public static List<Order> listOfOrders = new List<Order>()
        {
            new Order()
            {
                 commission=3,
                   GuestRequestKey=10000000,
                    HostingUnitkey=211976295,
                      Status=OrderStatus.Open
            }
         };

        public static List<HostingUnit> listOfHostingUnits = new List<HostingUnit>()
        {
          new HostingUnit()
          {
               Area=AreaChoise.Jerusalem,
                HostingUnitkey=Configuration.hostingUnitKey++,
                 HostingUnitName="river",
                  price=2000,
                  Diary=new bool[12,31],
                  Owner=new Host()
                  {
                   HostKey = 211976295,
                PrivateName = "Talya",
                FamilyName = "Yazdi",
                FhoneNumber = 0543725708,
                
                CollectionClearance = true,
                MailAddress = "t@jct.ac.il",
                 Password=11111111
                  },
                   Uris=new List<string>()
                   {
                     "https://img.mako.co.il/2015/11/17/hamelech_david_i.jpg",
                      "https://www.b-zimmer.co.il/wp-content/uploads/2018/07/%D7%94%D7%A7%D7%9E%D7%AA-%D7%A6%D7%99%D7%9E%D7%A8.jpg"}
                    },

          new HostingUnit()
          {
               Area=AreaChoise.Center,
                HostingUnitkey=Configuration.hostingUnitKey++,
                 HostingUnitName="blue",
                  price=1800,
                  Diary=new bool[12,31],
                  Owner=new Host()
                  {
                   HostKey = 211976295,
                PrivateName = "Talya",
                FamilyName = "Yazdi",
                FhoneNumber = 0543725708,
                
                CollectionClearance = false,
                MailAddress = "t@jct.ac.il",
                 Password=11111111
                  },
                   Uris=new List<string>()
                   {
                    "https://img.mako.co.il/2015/11/17/hamelech_david_i.jpg",
                     "https://www.b-zimmer.co.il/wp-content/uploads/2018/07/%D7%94%D7%A7%D7%9E%D7%AA-%D7%A6%D7%99%D7%9E%D7%A8.jpg"
                       //new Uri("pictures/blblb.png", UriKind.Relative)
                   }
                    },
          };

        public static List<GuestRequest> listOfRequests = new List<GuestRequest>()
        {
         new GuestRequest
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
             }
          };


    }
}
