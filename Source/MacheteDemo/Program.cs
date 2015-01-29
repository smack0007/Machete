using Machete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacheteDemo
{
	class Program
	{
		static readonly string _ = Environment.NewLine;

		static void Main(string[] args)
		{
			string templateSource = @"
@using System.Diagnostics

@for (int i = 0; i < 10; i++) {
Hello!
}";

			CodeGenerator codeGenerator = new CodeGenerator();

			var codeGeneratorParameters = new CodeGeneratorParameters();
			codeGeneratorParameters.ClassName = "Foo";

			var codeGeneratorResult = codeGenerator.Generate(templateSource, codeGeneratorParameters);

			Console.WriteLine(codeGeneratorResult.Code);

			Compiler compiler = new Compiler();

			var compilerParameters = new CompilerParameters();
			compilerParameters.CodeGenerator.ClassName = "Foo";
			
			var template = compiler.Compile(templateSource, compilerParameters);
					
			Console.WriteLine(template.Run());
						
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
