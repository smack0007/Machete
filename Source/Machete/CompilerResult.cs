using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
    public class CompilerResult
    {
        public Assembly Assembly { get; set; }

        public string TypeName { get; set; }
    }
}
