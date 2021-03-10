using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public struct Host
    {
        public long HostKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public long FhoneNumber { get; set; }
        public string MailAddress { get; set; }
      public  long BankAccountNumber { get; set; }
        public BankBranch BankBranchDetails { get; set; }
        public bool CollectionClearance { get; set; }
        public long Password { get; set; }
        public override string ToString() { return HostKey.ToString(); }
    }
}
