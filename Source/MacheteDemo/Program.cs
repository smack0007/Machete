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
		static void Main(string[] args)
		{
			CodeGenerator codeGenerator = new CodeGenerator();
			Console.WriteLine(codeGenerator.GenerateClass("Hello @Name!"));

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
