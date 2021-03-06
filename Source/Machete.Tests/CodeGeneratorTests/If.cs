﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
	[TestFixture]
    public class If : CodeGeneratorTestFixture
    {
		[Test]
        public void No_Block()
        {
            string template = "@if (shouldGreet) {}";

            this.AssertGeneratedMethodBody(
                template,
                "if (shouldGreet) {",
                "}"
            );
        }

		[Test]
        public void Single_Literal_Block()
        {
            string template = "@if (shouldGreet) { Hello! }";

            this.AssertGeneratedMethodBody(
                template,
                "if (shouldGreet) {",
                "WriteLiteral(@\" Hello! \");",
                "}"
            );
        }

		[Test]
        public void Single_Expression_Block()
        {
            string template = "@if (shouldGreet) { @person.Name }";

            this.AssertGeneratedMethodBody(
                template,
                "if (shouldGreet) {",
                "WriteLiteral(@\" \");",
                "Write(person.Name);",
                "WriteLiteral(@\" \");",
                "}"
            );
        }

		[Test]
		public void Block_With_Multiple_Lines()
		{
			string template = "@if (shouldGreet) {" + _ + "Hello!" + _ + "}";

			this.AssertGeneratedMethodBody(
				template,
				"if (shouldGreet) {",
				"WriteLiteral(@\"Hello!",
				"\");",
				"}"
			);
		}
    }
}
