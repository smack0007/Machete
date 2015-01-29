using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public class CodeGeneratorResult
	{
		public string Code
		{
			get;
			set;
		}

		public List<string> ReferencedAssemblies
		{
			get;
			private set;
		}

		public List<string> Usings
		{
			get;
			private set;
		}

		public CodeGeneratorResult()
		{
			this.ReferencedAssemblies = new List<string>();
			this.Usings = new List<string>();
		}
	}
}
