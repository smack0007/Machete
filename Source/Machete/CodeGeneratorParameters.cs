namespace Machete
{
    public class CodeGeneratorParameters
	{
        public string ClassName { get; set; } = "MyTemplate";

        public string BaseClassName { get; set; } = "Template";

        public bool IncludeLineDirectives { get; set; } = true;
	}
}
