using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete
{
    public class CodeGenerator
    {
        enum BufferContents
        {
            Literal,

            Expression
        }
				
		public CodeGeneratorResult Generate(string template, CodeGeneratorParameters parameters)
		{
			if (template == null)
				throw new ArgumentNullException("template");

			StringBuilder output = new StringBuilder();
			CodeGeneratorResult result = new CodeGeneratorResult();

			GenerateClass(output, template, parameters, result);

			result.Code = output.ToString();

			return result;
		}

        public string GenerateMethodBody(string template)
        {
			if (template == null)
				throw new ArgumentNullException("template");

            StringBuilder output = new StringBuilder();

            GenerateMethodBody(output, template, 0, template.Length - 1, new CodeGeneratorResult());
            
            return output.ToString();
        }

		private static void GenerateClass(StringBuilder output, string template, CodeGeneratorParameters parameters, CodeGeneratorResult result)
		{
			StringBuilder methodOutput = new StringBuilder();
			GenerateMethodBody(methodOutput, template, 0, template.Length - 1, result);

			output.AppendLine("using System;");

			foreach (var usingString in result.Usings)
				output.AppendLine("using {0};", usingString);

			output.AppendLine();

			output.AppendLine("namespace Machete.Templates");
			output.AppendLine("{");
			output.AppendLine("\tpublic class {0} : {1}", parameters.ClassName, parameters.BaseClassName);
			output.AppendLine("\t{");

			foreach (var propertyString in result.Properties)
				output.AppendLine("\t\tpublic {0} {{ get; set; }}", propertyString);

			output.AppendLine();
			output.AppendLine("\t\tprotected override void Execute()");
			output.AppendLine("\t\t{");

			output.Append(methodOutput);

			output.AppendLine("\t\t}");
			output.AppendLine("\t}");
			output.AppendLine("}");
		}

        private static void GenerateMethodBody(StringBuilder output, string template, int start, int end, CodeGeneratorResult result)
        {
            StringBuilder buffer = new StringBuilder();
                                                
            int i = start;
            while (i <= end)
            {
                if (template[i] == '@')
                {
                    FlushBuffer(BufferContents.Literal, buffer, output);

                    i++;
                    if (LookAhead(template, i, "{"))
                    {
                        ParseCodeBlock(template, ref i, buffer, output);
                    }
                    else if (LookAhead(template, i, "for"))
                    {
                        ParseLogicBlock("for", template, ref i, buffer, output, result);
                    }
                    else if (LookAhead(template, i, "foreach"))
                    {
						ParseLogicBlock("foreach", template, ref i, buffer, output, result);
                    }
                    else if (LookAhead(template, i, "if"))
                    {
						ParseLogicBlock("if", template, ref i, buffer, output, result);
                    }
					else if (LookAhead(template, i, "property"))
					{
						ParseDeclaration("property", template, ref i, buffer, output, (x) => result.Properties.Add(x));
					}
					else if (LookAhead(template, i, "using"))
					{
						ParseDeclaration("using", template, ref i, buffer, output, (x) => result.Usings.Add(x));
					}
                    else
                    {
                        ParseExpression(template, ref i, buffer, output);
                    }
                }
                else
                {
                    buffer.Append(template[i]);
                    i++;
                }
            }

            FlushBuffer(BufferContents.Literal, buffer, output);
        }

        private static bool LookAhead(string template, int i, string keyword)
        {
            if (i + keyword.Length >= template.Length)
                return false;

            for (int j = 0; j < keyword.Length; j++)
            {
                if (template[i + j] != keyword[j])
                    return false;
            }

            return true;
        }

        private static int SearchAhead(string template, int i, string keyword)
        {
            for (int j = i; j < template.Length; j++)
            {
                if (LookAhead(template, j, keyword))
                    return j;
            }

            return -1;
        }

        private static int FindClosingParen(string template, int i)
        {
            int depth = 1;

            for (int j = i + 1; j < template.Length; j++)
            {
                if (template[j] == '(')
                {
                    depth++;
                }
                else if (template[j] == ')')
                {
                    depth--;

                    if (depth <= 0)
                        return j;
                }
            }

            return -1;
        }

        private static int FindClosingCurlyBrace(string template, int i)
        {
            int depth = 1;

            for (int j = i + 1; j < template.Length; j++)
            {
                if (template[j] == '{')
                {
                    depth++;
                }
                else if (template[j] == '}')
                {
                    depth--;

                    if (depth <= 0)
                        return j;
                }
            }

            return -1;
        }

        private static void AppendLine(StringBuilder output, string template, int start, int end)
        {
            for(int i = start; i <= end; i++)
                output.Append(template[i]);

            output.Append(Environment.NewLine);
        }

        private static void FlushBuffer(BufferContents state, StringBuilder buffer, StringBuilder output)
        {
            if (buffer.Length > 0)
            {
                switch (state)
                {
                    case BufferContents.Literal:
                        output.AppendLine(string.Format("WriteLiteral(@\"{0}\");", buffer.ToString().Replace("\"", "\"\"")));
                        break;

                    case BufferContents.Expression:
                        output.AppendLine(string.Format("Write({0});", buffer.ToString()));
                        break;
                }

                buffer.Clear();
            }
        }

        private static void ParseCodeBlock(string template, ref int i, StringBuilder buffer, StringBuilder output)
        {
            int closeCurlyBrace = FindClosingCurlyBrace(template, i);

            if (closeCurlyBrace == -1)
                throw new MacheteException("Matching } not found while parsing code block.");
                        
            int startCode = i + 1;
            int endCode = closeCurlyBrace - 1;            

            if (startCode < endCode)
                AppendLine(output, template, startCode, endCode);

            i = closeCurlyBrace + 1;
        }

        private static void ParseLogicBlock(string type, string template, ref int i, StringBuilder buffer, StringBuilder output, CodeGeneratorResult result)
        {
            int openParen = SearchAhead(template, i, "(");

            if (openParen == -1)
                throw new MacheteException(string.Format("Expected ( while parsing {0}.", type));

            int closeParen = FindClosingParen(template, openParen);

            if (closeParen == -1)
                throw new MacheteException(string.Format("Matching ) not found while parsing {0}.", type));

            int openCurlyBrace = SearchAhead(template, closeParen, "{");

            if (openCurlyBrace == -1)
                throw new MacheteException(string.Format("Expected { while parsing {0}.", type));

            int closeCurlyBrace = FindClosingCurlyBrace(template, openCurlyBrace);

            if (closeCurlyBrace == -1)
                throw new MacheteException(string.Format("Matching } not found while parsing {0}.", type));

            AppendLine(output, template, i, openCurlyBrace);
            
            int startCode = openCurlyBrace + 1;
            int endCode = closeCurlyBrace - 1;
            
            if (startCode < endCode)
                GenerateMethodBody(output, template, startCode, endCode, result);
            
            output.AppendLine("}");
            
            i = closeCurlyBrace + 1;
        }
				
        private static void ParseExpression(string template, ref int i, StringBuilder buffer, StringBuilder output)
        {
            bool finished = false;

            while (!finished)
            {
                if (!char.IsLetterOrDigit(template[i]))
                {
                    finished = true;

                    if (template[i] == '.')
                    {
                        buffer.Append(template[i]);
                        i++;
                        finished = false;
                    }
                    else if (template[i] == '[')
                    {
                        ProcessIndexer(template, ref i, buffer);
                        finished = false;
                    }
                }
                else
                { 
                    buffer.Append(template[i]);
                    i++;
                }

                if (i >= template.Length)
                    finished = true;
            }

            FlushBuffer(BufferContents.Expression, buffer, output);
        }

        private static void ProcessIndexer(string template, ref int i, StringBuilder buffer)
        {
            int depth = 1;

            buffer.Append(template[i]);
            i++;

            while (depth > 0 && i < template.Length)
            {
                if (template[i] == '[')
                {
                    depth++;
                }
                else if (template[i] == ']')
                {
                    depth--;
                }
                                
                buffer.Append(template[i]);
                i++;
            }
        }

		private static void ParseDeclaration(string type, string template, ref int i, StringBuilder buffer, StringBuilder output, Action<string> action)
		{
			int endOfLine = SearchAhead(template, i, Environment.NewLine);

			if (endOfLine == -1)
				endOfLine = template.Length;

			string content = template.Substring(i + type.Length + 1, endOfLine - i - type.Length - 1);
			action(content);

			i = endOfLine + 1;
		}
    }
}
