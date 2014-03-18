using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Tests.CodeGeneratorTests
{
    [TestFixture]
    public class For : CodeGeneratorTestFixture
    {
        [Test]
        public void No_Block()
        {
            string template = "@for (int i = 0; i < 10; i++) {}";

            this.AssertGeneratedCode(
                template,
                "for (int i = 0; i < 10; i++) {",
                "}"
            );
        }

        [Test]
        public void Single_Literal_Block()
        {
            string template = "@for (int i = 0; i < 10; i++) { Hello! }";

            this.AssertGeneratedCode(
                template,
                "for (int i = 0; i < 10; i++) {",
                "WriteLiteral(@\" Hello! \");",
                "}"
            );
        }

        [Test]
        public void Single_Expression_Block()
        {
            string template = "@for (int i = 0; i < 10; i++) { @i }";

            this.AssertGeneratedCode(
                template,
                "for (int i = 0; i < 10; i++) {",
                "WriteLiteral(@\" \");",
                "Write(i);",
                "WriteLiteral(@\" \");",
                "}"
            );
        }
    }
}
