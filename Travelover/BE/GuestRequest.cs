using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class GuestRequest

    {
        public long GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public AreaChoise Area { get; set; }
        public string SubArea { get; set; }
        public ResortType Type { get; set; }
        public uint Adults { get; set; }
        public uint Children { get; set; }
        public Extra Pool { get; set; }
        public Extra Jacuzzi { get; set; }
        public Extra Garden { get; set; }
        public Extra ChildrensAttractions { get; set; }
        public Extra Synagogue { get; set; }
        public override string ToString() { return "Guest Request Key: " + GuestRequestKey.ToString() + "\n"
                + "PrivateName: " + PrivateName.ToString() + "\n"
                + "FamilyName: " + FamilyName.ToString() + "\n"
                + "Area: " + Area.ToString() + "\n"
                + "Sub Area: " + SubArea.ToString() + "\n"
                + "Type: " + Type.ToString() + "\n"
                + "Adults: " + Adults.ToString() + "\n"
                + "Children: " + Children.ToString() + "\n"
                + "Pool: " + Pool.ToString() + "\n"
                + "Jacuzzi " + Jacuzzi.ToString() + "\n"
                + "Garden: " + Garden.ToString() + "\n"
                + "Status: " + Status.ToString() + "\n"
                + "Mail Address: " + MailAddress.ToString() + "\n"
            + "Registration Date:  " + RegistrationDate.ToString() + "\n"
                + "EntryDate: " + EntryDate.ToString() + "\n"
                + "ReleaseDate: " + ReleaseDate.ToString() + "\n"
                
                + "Synagogue: " + Synagogue.ToString() + "\n"
            + "Childrens Attractions:  " + ChildrensAttractions.ToString() + "\n"
               ;
        }


    }
}
