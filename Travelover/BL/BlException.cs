using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BlException : Exception
    {
        public BlException(string message) : base(message)
        {
        }
    }
}
