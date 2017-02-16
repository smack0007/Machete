using System;
using System.Linq;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
    public abstract class CodeGeneratorTestFixture
    {
        protected static readonly string _ = Environment.NewLine;

        CodeGenerator generator;

		public CodeGeneratorTestFixture()
        {
            this.generator = new CodeGenerator();
        }

        protected static string BuildTemplate(params string[] lines)
        {
            return string.Join(Environment.NewLine, lines);
        }

        protected virtual CodeGeneratorParameters CreateCodeGeneratorParameters()
        {
            return new CodeGeneratorParameters()
            {
                IncludeLineDirectives = false
            };
        }

        protected void AssertGeneratedMethodBody(string template, params string[] expected)
        {
            var codeGeneratorParams = this.CreateCodeGeneratorParameters();

            string code = this.generator.GenerateMethodBody(template, codeGeneratorParams).Trim();
            
			string[] actual = code
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(x => x.Trim())
                .ToArray();

            if (expected.Length != actual.Length)
            {
                Console.WriteLine(code);
                Assert.Fail("Expected line count is {0} but actual line count was {1}.", expected.Length, actual.Length);
            }
            
            for (int i = 0; i < actual.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    Console.WriteLine(code);
                    Assert.Fail("Line {0}: Expected \"{1}\" but actual was \"{2}\".", i + 1, expected[i], actual[i]);
                }
            }
        }
    }
}
