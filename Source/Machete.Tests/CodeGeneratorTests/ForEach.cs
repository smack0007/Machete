using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
	[TestFixture]
    public class ForEach : CodeGeneratorTestFixture
    {
		[Test]
        public void No_Block()
        {
            string template = "@foreach (var person in People) {}";

            this.AssertGeneratedMethodBody(
                template,
                "foreach (var person in People) {",
                "}"
            );
        }

		[Test]
        public void Single_Literal_Block()
        {
            string template = "@foreach (var person in People) { Hello! }";

            this.AssertGeneratedMethodBody(
                template,
                "foreach (var person in People) {",
                "WriteLiteral(@\" Hello! \");",
                "}"
            );
        }

		[Test]
        public void Single_Expression_Block()
        {
            string template = "@foreach (var person in People) { @person.Name }";

            this.AssertGeneratedMethodBody(
                template,
                "foreach (var person in People) {",
                "WriteLiteral(@\" \");",
                "Write(person.Name);",
                "WriteLiteral(@\" \");",
                "}"
            );
        }
    }
}
