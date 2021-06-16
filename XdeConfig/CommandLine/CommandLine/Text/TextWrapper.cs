using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine.Infrastructure;

namespace CommandLine.Text
{
	// Token: 0x0200005C RID: 92
	public class TextWrapper
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00009EF4 File Offset: 0x000080F4
		public TextWrapper(string input)
		{
			this.lines = input.Replace("\r", "").Split(new char[]
			{
				'\n'
			}, StringSplitOptions.None);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009F24 File Offset: 0x00008124
		public TextWrapper WordWrap(int columnWidth)
		{
			columnWidth = Math.Max(1, columnWidth);
			this.lines = this.lines.SelectMany((string line) => this.WordWrapLine(line, columnWidth)).ToArray<string>();
			return this;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009F7C File Offset: 0x0000817C
		public TextWrapper Indent(int numberOfSpaces)
		{
			this.lines = (from line in this.lines
			select numberOfSpaces.Spaces() + line).ToArray<string>();
			return this;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009FB9 File Offset: 0x000081B9
		public string ToText()
		{
			return string.Join(Environment.NewLine, this.lines);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009FCB File Offset: 0x000081CB
		public static string WrapAndIndentText(string input, int indentLevel, int columnWidth)
		{
			return new TextWrapper(input).WordWrap(columnWidth).Indent(indentLevel).ToText();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009FE4 File Offset: 0x000081E4
		private string[] WordWrapLine(string line, int columnWidth)
		{
			string text = line.TrimStart(Array.Empty<char>());
			int currentIndentLevel = Math.Min(line.Length - text.Length, columnWidth - 1);
			columnWidth -= currentIndentLevel;
			return (from builder in text.Split(new char[]
			{
				' '
			}).Aggregate(new List<StringBuilder>(), (List<StringBuilder> lineList, string word) => TextWrapper.AddWordToLastLineOrCreateNewLineIfNecessary(lineList, word, columnWidth))
			select currentIndentLevel.Spaces() + builder.ToString().TrimEnd(Array.Empty<char>())).ToArray<string>();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A07C File Offset: 0x0000827C
		private static List<StringBuilder> AddWordToLastLineOrCreateNewLineIfNecessary(List<StringBuilder> lines, string word, int columnWidth)
		{
			StringBuilder stringBuilder = lines.LastOrDefault<StringBuilder>();
			string text = ((stringBuilder != null) ? stringBuilder.ToString() : null) ?? string.Empty;
			if (lines.Any<StringBuilder>() && (word.Length <= 0 || text.Length + word.Length <= columnWidth))
			{
				lines.Last<StringBuilder>().Append(word + " ");
			}
			else
			{
				do
				{
					int n = Math.Min(columnWidth, word.Length);
					string value = TextWrapper.LeftString(word, n) + " ";
					lines.Add(new StringBuilder(value));
					word = TextWrapper.RightString(word, n);
				}
				while (word.Length > 0);
			}
			return lines;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A125 File Offset: 0x00008325
		private static string RightString(string str, int n)
		{
			if (n < str.Length && str.Length != 0)
			{
				return str.Substring(n);
			}
			return string.Empty;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A145 File Offset: 0x00008345
		private static string LeftString(string str, int n)
		{
			if (n < str.Length && str.Length != 0)
			{
				return str.Substring(0, n);
			}
			return str;
		}

		// Token: 0x040000BC RID: 188
		private string[] lines;
	}
}
