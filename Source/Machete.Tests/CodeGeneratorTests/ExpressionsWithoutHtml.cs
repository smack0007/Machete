﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
	[TestFixture]
    public class ExpressionsWithoutHtml : CodeGeneratorTestFixture
    {
        [Test]
        public void Without_Expression()
        {
            string template = "Hello World!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello World!\");"
            );
        }

		[Test]
        public void Multiline_Without_Expression()
        {
            string template = "Hello World!" + _ + "Goodbye World!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello World!",
                "Goodbye World!\");"
            );
        }

		[Test]
        public void With_One_Expression_Of_Depth_One()
        {
            string template = "Hello @Name!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello \");",
                "Write(Name);",
                "WriteLiteral(@\"!\");"
            );
        }

		[Test]
        public void With_Two_Expressions_Of_Depth_One()
        {
            string template = "Hello @Name1 and @Name2!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello \");",
                "Write(Name1);",
                "WriteLiteral(@\" and \");",
                "Write(Name2);",
                "WriteLiteral(@\"!\");"
            );
        }

		[Test]
        public void With_One_Expression_Of_Depth_Two()
        {
            string template = "Hello @Person.Name!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello \");",
                "Write(Person.Name);",
                "WriteLiteral(@\"!\");"
            );
        }

		[Test]
        public void With_Two_Expression_Of_Depth_Two()
        {
            string template = "Hello @Person1.Name and @Person2.Name!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello \");",
                "Write(Person1.Name);",
                "WriteLiteral(@\" and \");",
                "Write(Person2.Name);",
                "WriteLiteral(@\"!\");"
            );
        }

		[Test]
        public void Array_Index_Expressions()
        {
            string template = "Hello @Person[0], @Person[1] and @Person[2]!";

            this.AssertGeneratedMethodBody(
                template,
                "WriteLiteral(@\"Hello \");",
                "Write(Person[0]);",
                "WriteLiteral(@\", \");",
                "Write(Person[1]);",
                "WriteLiteral(@\" and \");",
                "Write(Person[2]);",
                "WriteLiteral(@\"!\");"
            );
        }
    }
}
