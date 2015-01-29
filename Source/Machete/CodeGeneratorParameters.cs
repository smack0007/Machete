using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public class CodeGeneratorParameters
	{
		public string ClassName
		{
			get;
			set;
		}

		public string BaseClassName
		{
			get;
			set;
		}

		public CodeGeneratorParameters()
		{
			this.ClassName = "MyTemplate";
			this.BaseClassName = "Template";
		}
	}
}
