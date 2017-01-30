using System.Text;

namespace Machete
{
    internal static class StringBuilderExtensions
	{
		public static void AppendLine(this StringBuilder sb, string format, params object[] args)
		{
			sb.AppendLine(string.Format(format, args));
		}
	}
}
