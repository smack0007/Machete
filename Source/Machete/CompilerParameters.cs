using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public class CompilerParameters
	{
        public List<string> ReferencedAssemblies { get; } = new List<string>() { "System", "System.Core" };
	}
}
