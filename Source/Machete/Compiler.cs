using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
	public class Compiler
	{
		int compileCount;
		CodeGenerator codeGenerator;
		
		public Compiler()
		{
			this.codeGenerator = new CodeGenerator();
		}
				
		public T Compile<T>(string template, CompilerParameters parameters)
			where T: Template
		{
			if (template == null)
				throw new ArgumentNullException("template");

			if (parameters == null)
				throw new ArgumentNullException("parameters");

			this.compileCount++;

			var generatorParameters = new CodeGeneratorParameters()
			{
				ClassName = "Template" + this.compileCount,
				BaseClassName = typeof(T).FullName
			};

			var generatorResult = this.codeGenerator.Generate(template, generatorParameters);

			var compilerParams = new System.CodeDom.Compiler.CompilerParameters() { GenerateInMemory = true };
			compilerParams.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

			foreach (var reference in parameters.ReferencedAssemblies)
			{
				string name = reference;

				if (!name.EndsWith(".dll"))
					name = name + ".dll";

				compilerParams.ReferencedAssemblies.Add(name);
			}

			compilerParams.ReferencedAssemblies.Add(typeof(T).Assembly.Location);

			var compiler = CodeDomProvider.CreateProvider("CSharp");
			var compilerResults = compiler.CompileAssemblyFromSource(compilerParams, generatorResult.Code);

			if (compilerResults.Errors.Count > 0)
			{
				StringBuilder errors = new StringBuilder();
				errors.AppendLine("Failed to compile script:");
				foreach (CompilerError error in compilerResults.Errors)
				{
					errors.AppendLine(string.Format("({0}, {1}): {2}", error.Line, error.Column, error.ErrorText));
				}

				throw new MacheteException(errors.ToString());
			}

			return (T)Activator.CreateInstance(compilerResults.CompiledAssembly.GetType("Machete.Templates." + generatorParameters.ClassName));
		}

		public Template Compile(string template, CompilerParameters parameters)
		{
			return this.Compile<Template>(template, parameters);
		}
	}
}
