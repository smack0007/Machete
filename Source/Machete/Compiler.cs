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

		public Template Compile(string template, CompilerParameters parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			var generatorResult = this.codeGenerator.Generate(template, parameters.CodeGenerator);

			var compilerParams = new System.CodeDom.Compiler.CompilerParameters() { GenerateInMemory = true };
			compilerParams.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

			foreach (var reference in parameters.ReferencedAssemblies)
			{
				string name = reference;

				if (!name.EndsWith(".dll"))
					name = name + ".dll";

				compilerParams.ReferencedAssemblies.Add(name);
			}

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

			return (Template)Activator.CreateInstance(compilerResults.CompiledAssembly.GetType("Machete.Templates." + parameters.CodeGenerator.ClassName));
		}
	}
}
