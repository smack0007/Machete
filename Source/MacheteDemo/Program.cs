using System;
using Machete;

namespace MacheteDemo
{
    class Program
	{
		static readonly string _ = Environment.NewLine;

		static void Main(string[] args)
		{
			string templateSource = @"
@using System.Diagnostics
@property int Count
@property string Name

@for (int i = 0; i < Count; i++) {
Hello @Name!
}";

			CodeGenerator codeGenerator = new CodeGenerator();

			var codeGeneratorParameters = new CodeGeneratorParameters();
			codeGeneratorParameters.ClassName = "Foo";

			var codeGeneratorResult = codeGenerator.Generate(templateSource, codeGeneratorParameters);

			Console.WriteLine(codeGeneratorResult.Code);

			Compiler compiler = new Compiler();

			var compilerParameters = new CompilerParameters();
			
			var template = compiler.CompileTemplate(templateSource, compilerParameters);
			template.SetPropertyValue("Count", 20);
			template.SetPropertyValue("Name", "Bob Freeman");
			
			var template2 = compiler.CompileTemplate(templateSource, compilerParameters);
			template2.SetPropertyValue("Count", 5);
			template2.SetPropertyValue("Name", "John Doe");

			Console.WriteLine(template.Run());
			Console.WriteLine(template2.Run());
						
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
