using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public abstract class Template
	{
		StringBuilder output;

		public string Run()
		{
			this.output = new StringBuilder();

			this.Execute();

			return this.output.ToString();
		}
				
		protected abstract void Execute();

		protected void WriteLiteral(string value)
		{
			this.output.Append(value);
		}

		protected void Write(object value)
		{
			this.output.Append(value.ToString());
		}
	}
}
