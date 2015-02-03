using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
	[TestFixture]
	public class While : CodeGeneratorTestFixture
	{
		[Test]
		public void No_Block()
		{
			string template = "@while (i > 10) {}";

			this.AssertGeneratedMethodBody(
				template,
				"while (i > 10) {",
				"}"
			);
		}

		[Test]
		public void Single_Literal_Block()
		{
			string template = "@while (i > 10) { Hello! }";

			this.AssertGeneratedMethodBody(
				template,
				"while (i > 10) {",
				"WriteLiteral(@\" Hello! \");",
				"}"
			);
		}

		[Test]
		public void Single_Expression_Block()
		{
			string template = "@while (i > 10) { @i }";

			this.AssertGeneratedMethodBody(
				template,
				"while (i > 10) {",
				"WriteLiteral(@\" \");",
				"Write(i);",
				"WriteLiteral(@\" \");",
				"}"
			);
		}
	}
}
