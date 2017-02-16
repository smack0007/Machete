using NUnit.Framework;

namespace Machete.Tests.CodeGeneratorTests
{
    [TestFixture]
    public class LineDirectives : CodeGeneratorTestFixture
    {
        protected override CodeGeneratorParameters CreateCodeGeneratorParameters()
        {
            return new CodeGeneratorParameters()
            {
                IncludeLineDirectives = true
            };
        }

        [Test]
        public void LineDirectivesAreIncrementedCorrectly()
        {
            string template = @"@foreach (var person in People) {
Hello
@Test
World!
}";

            this.AssertGeneratedMethodBody(
                template,
                "#line 1",
                "foreach (var person in People) {",
                "#line 2",
                "WriteLiteral(@\"Hello",
                "\");",
                "#line 3",
                "Write(Test);",
                "#line 4",
                "WriteLiteral(@\"",
                "World!",
                "\");",
                "}",
                "#line 8"
            );
        }
    }
}
