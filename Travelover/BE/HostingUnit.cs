using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class HostingUnit
    {
        public long HostingUnitkey { get; set; }
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }
        public AreaChoise Area { get; set; }
        [XmlIgnore]
        public bool[,] Diary { get; set; }
        [XmlArray("Diary")]
        public bool[] DiaryTo
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(13); }
        }
        public int price { get; set; }
        public List<string> Uris { get; set; }
        public override string ToString() { return "Hosting Unit"; }
    }
}
