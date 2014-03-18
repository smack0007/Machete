using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
    public class MacheteException : Exception
    {
        public MacheteException(string message)
            : base(message)
        {
        }
    }
}
