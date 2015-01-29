using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Machete.Tests.CodeGeneratorTests
{
    public class ForEach : CodeGeneratorTestFixture
    {
		[Fact]
        public void No_Block()
        {
            string template = "@foreach (var person in People) {}";

            this.AssertGeneratedMethodBody(
                template,
                "foreach (var person in People) {",
                "}"
            );
        }

		[Fact]
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

		[Fact]
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
