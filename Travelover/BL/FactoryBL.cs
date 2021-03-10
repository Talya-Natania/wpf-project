using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public static  class FactoryBL
    {
        static IBL bl = null;
        public static IBL getBL()
        {
            if (bl == null)
                bl = new BL_imp();
            return bl;

        }
    }
}
