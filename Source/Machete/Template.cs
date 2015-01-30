using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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

		private PropertyInfo GetProperty(string name)
		{
			var property = this.GetType().GetProperties().Where(x => x.Name == name).SingleOrDefault();

			if (property == null)			
				throw new MacheteException(string.Format("Template does not contain a property named {0}.", name));

			return property;
		}

		public object GetPropertyValue(string name)
		{
			return this.GetProperty(name).GetValue(this);
		}

		public T GetPropertyValue<T>(string name)
		{
			return (T)this.GetProperty(name).GetValue(this);
		}

		public void SetPropertyValue(string name, object value)
		{
			this.GetProperty(name).SetValue(this, value);
		}

		public void SetPropertyValue<T>(string name, T value)
		{
			this.GetProperty(name).SetValue(this, value);
		}
	}
}
