using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Tests.CodeGeneratorTests
{
    [TestFixture]
    public class ForEach : CodeGeneratorTestFixture
    {
        [Test]
        public void No_Block()
        {
            string template = "@foreach (var person in People) {}";

            this.AssertGeneratedCode(
                template,
                "foreach (var person in People) {",
                "}"
            );
        }

        [Test]
        public void Single_Literal_Block()
        {
            string template = "@foreach (var person in People) { Hello! }";

            this.AssertGeneratedCode(
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

            this.AssertGeneratedCode(
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
