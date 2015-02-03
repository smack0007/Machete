using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Machete.Tests
{
	[TestFixture]
	public class IntegrationTests
	{
		private void RunTest(string input, string expected, Dictionary<string, object> properties, Action<Template> check)
		{
			CodeGenerator codeGenerator = new CodeGenerator();
			string code = codeGenerator.Generate(input, new CodeGeneratorParameters()).Code;

			Template template;
			string actual;

			try
			{
				Compiler compiler = new Compiler();
				template = compiler.Compile(input, new CompilerParameters());

				foreach (var pair in properties)
				{
					template.SetPropertyValue(pair.Key, pair.Value);
				}

				actual = template.Run();

				Console.WriteLine("=== Output ===");
				Console.WriteLine(actual);
			}
			finally
			{
				Console.WriteLine("=== Code ===");
				Console.WriteLine(code);
			}

			expected = expected.Replace(Environment.NewLine, string.Empty);
			actual = actual.Replace(Environment.NewLine, string.Empty);

			Assert.AreEqual(expected, actual);

			check(template);
		}

		[Test]
		public void HtmlDocument()
		{
			string input = @"
@property string BodyClass
@property int ForCount
@property string[] Items
@property bool Bool1
@property bool Bool2
@property bool Bool3
@property string Email
@property string Output1
@property int Output2
<html>
<body class=""@BodyClass"">
<strong>For</strong>
@for (var i = 0; i < ForCount; i++) {
<p>Line @i</p>
}
<strong>ForEach</strong>
<ul>
@foreach (var item in Items) {
<li>@item</li>
}
</ul>
<strong>If</strong>
@if (Bool1) {
<p>Bool1</p>
}
@if (Bool2) {
<p>Bool2</p>
}
@if (Bool3) {
<p>Bool3</p>
}
<strong>While</strong>
@var int x = 0
@while (x < Items.Length) {
<p>Counting...</p>
@{ x++; }
}
<em>bob@@freeman.com</em>
<em><a href=""mailto:@Email"">@Email</a></em>
<em>Items[2] = @Items[2]</em>
</body>
@{
	if (Bool1) {
		Output1 = ""ABC"";
	}
	if (Bool2) {
		Output1 = ""Foo"";
	}
	Output2 = x;
}
</html>";

			string expected = @"
<html>
<body class=""foo"">
<strong>For</strong>
<p>Line 0</p>
<p>Line 1</p>
<p>Line 2</p>
<p>Line 3</p>
<p>Line 4</p>
<p>Line 5</p>
<p>Line 6</p>
<p>Line 7</p>
<p>Line 8</p>
<p>Line 9</p>
<strong>ForEach</strong>
<ul>
<li>A</li>
<li>B</li>
<li>C</li>
</ul>
<strong>If</strong>
<p>Bool2</p>
<strong>While</strong>
<p>Counting...</p>
<p>Counting...</p>
<p>Counting...</p>
<em>bob@freeman.com</em>
<em><a href=""mailto:bob@freeman.com"">bob@freeman.com</a></em>
<em>Items[2] = C</em>
</body>
</html>";

			RunTest(input, expected, new Dictionary<string, object>()
				{
					{ "BodyClass", "foo" },
					{ "ForCount", 10 },
					{ "Items", new string[] { "A", "B", "C" } },
					{ "Bool1", false },
					{ "Bool2", true },
					{ "Bool3", false },
					{ "Email", "bob@freeman.com" }
				},
				(t) =>
				{
					Assert.AreEqual(t.GetPropertyValue<string>("Output1"), "Foo");
					Assert.AreEqual(t.GetPropertyValue<int>("Output2"), 3);
				});
		}
	}
}
