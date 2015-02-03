using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
	[TestFixture]
    public class CodeBlock : CodeGeneratorTestFixture
    {
        [Test]
        public void Inline_Code_Block()
        {
            string template = BuildTemplate("@{ x = 5; } Hello!");

            AssertGeneratedMethodBody(
                template,
                "x = 5;",
                "WriteLiteral(@\" Hello!\");"
            );
        }

		[Test]
        public void MultiLine_Code_Block()
        {
            string template = BuildTemplate(
                "@{",
                "x = 5;",
                "y = 10;",
                "}",
                "(@x, @y)");

            AssertGeneratedMethodBody(
                template,
                "x = 5;",
                "y = 10;",
                "",
                "WriteLiteral(@\"",
                "(\");",
                "Write(x);",
                "WriteLiteral(@\", \");",
                "Write(y);",
                "WriteLiteral(@\")\");"
            );
        }
    }
}
