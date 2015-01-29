using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public class CompilerParameters
	{
		public CodeGeneratorParameters CodeGenerator
		{
			get;
			private set;
		}

		public List<string> ReferencedAssemblies
		{
			get;
			private set;
		}

		public CompilerParameters()
		{
			this.CodeGenerator = new CodeGeneratorParameters();
									
			this.ReferencedAssemblies = new List<string>();
			this.ReferencedAssemblies.Add("System");
			this.ReferencedAssemblies.Add("System.Core");
		}
	}
}
