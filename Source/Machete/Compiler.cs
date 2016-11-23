using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

namespace Machete
{
    public class Compiler
    {
        int compileCount;
        CodeGenerator codeGenerator;

        public Compiler()
        {
            this.codeGenerator = new CodeGenerator();
        }

        public CompilerResult Compile(string template, CompilerParameters parameters)
        {
            return this.Compile<Template>(template, parameters);
        }

        public CompilerResult Compile<T>(string template, CompilerParameters parameters)
            where T : Template
        {
            if (template == null)
                throw new ArgumentNullException("template");

            if (parameters == null)
                throw new ArgumentNullException("parameters");

            this.compileCount++;

            var generatorParameters = new CodeGeneratorParameters()
            {
                ClassName = "Template" + this.compileCount,
                BaseClassName = typeof(T).FullName
            };

            var generatorResult = this.codeGenerator.Generate(template, generatorParameters);

            var compilerParams = new System.CodeDom.Compiler.CompilerParameters() { GenerateInMemory = true };
            compilerParams.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

            foreach (var reference in parameters.ReferencedAssemblies)
            {
                string name = reference;

                if (!name.EndsWith(".dll"))
                    name = name + ".dll";

                compilerParams.ReferencedAssemblies.Add(name);
            }

            compilerParams.ReferencedAssemblies.Add(typeof(T).Assembly.Location);

            var compiler = CodeDomProvider.CreateProvider("CSharp");
            var compilerResults = compiler.CompileAssemblyFromSource(compilerParams, generatorResult.Code);

            if (compilerResults.Errors.Count > 0)
            {
                StringBuilder errors = new StringBuilder();
                errors.AppendLine("Failed to compile script:");
                foreach (CompilerError error in compilerResults.Errors)
                {
                    errors.AppendLine(string.Format("({0}, {1}): {2}", error.Line, error.Column, error.ErrorText));
                }

                throw new MacheteException(errors.ToString());
            }

            return new CompilerResult()
            {
                Assembly = compilerResults.CompiledAssembly,
                TypeName = "Machete.Templates." + generatorParameters.ClassName
            };
        }

        public Template CompileTemplate(string template, CompilerParameters parameters)
        {
            return this.CompileTemplate<Template>(template, parameters);
        }
        
        public T CompileTemplate<T>(string template, CompilerParameters parameters)
			where T : Template
		{
            var result = this.Compile<T>(template, parameters);
			return (T)Activator.CreateInstance(result.Assembly.GetType(result.TypeName));
		}
        public TemplateFactory<Template> CompileFactory(string template, CompilerParameters parameters)
        {
            return this.CompileFactory<Template>(template, parameters);
        }

        public TemplateFactory<T> CompileFactory<T>(string template, CompilerParameters parameters)
            where T : Template
		{
            var result = this.Compile<T>(template, parameters);
            return new TemplateFactory<T>(result.Assembly, result.TypeName);
		}
	}
}
