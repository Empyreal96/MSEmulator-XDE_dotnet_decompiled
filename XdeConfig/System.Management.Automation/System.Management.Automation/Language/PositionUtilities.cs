using System;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B6 RID: 1462
	internal static class PositionUtilities
	{
		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0014B42A File Offset: 0x0014962A
		public static IScriptPosition EmptyPosition
		{
			get
			{
				return PositionUtilities.emptyPosition;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003E4F RID: 15951 RVA: 0x0014B431 File Offset: 0x00149631
		public static IScriptExtent EmptyExtent
		{
			get
			{
				return PositionUtilities.emptyExtent;
			}
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0014B438 File Offset: 0x00149638
		internal static string VerboseMessage(IScriptExtent position)
		{
			if (PositionUtilities.EmptyExtent.Equals(position))
			{
				return "";
			}
			string text = position.File;
			if (string.IsNullOrEmpty(text))
			{
				text = ParserStrings.TextForWordLine;
			}
			string text2 = position.StartScriptPosition.Line.TrimEnd(new char[0]);
			string text3 = "";
			if (!string.IsNullOrEmpty(text2))
			{
				int num = position.StartColumnNumber - 1;
				int num2 = (position.StartLineNumber == position.EndLineNumber) ? (position.EndColumnNumber - position.StartColumnNumber) : (text2.TrimEnd(new char[0]).Length - position.StartColumnNumber + 1);
				if (text2.IndexOf('\t') != -1)
				{
					StringBuilder stringBuilder = new StringBuilder(text2.Length * 2);
					string text4 = text2.Substring(0, num).Replace("\t", "    ");
					string text5 = text2.Substring(num, num2).Replace("\t", "    ");
					stringBuilder.Append(text4);
					stringBuilder.Append(text5);
					stringBuilder.Append(text2.Substring(num + num2).Replace("\t", "    "));
					num = text4.Length;
					num2 = text5.Length;
					text2 = stringBuilder.ToString();
				}
				bool flag = false;
				bool flag2 = false;
				int length = text2.Length;
				StringBuilder stringBuilder2 = new StringBuilder(text2.Length * 2 + 4);
				if (length > 69)
				{
					int num3 = num;
					int num4 = Math.Min(num3, 12);
					int num5 = length - num2 - num;
					int num6 = Math.Min(num5, 8);
					int num7 = num4 + num2 + num6;
					if (num7 >= 69)
					{
						if (num4 + num2 >= 69)
						{
							num2 = 69 - num4;
						}
						flag2 = true;
					}
					else
					{
						int num8 = num3 - num4;
						if (num8 > 0)
						{
							num4 += Math.Min(num8, 69 - num7);
							num7 = num4 + num2 + num6;
						}
						if (num7 < 69 && num5 > 0)
						{
							num6 += Math.Min(num5, 69 - num7);
						}
						flag2 = (num6 < num5);
					}
					flag = (num4 < num3);
					int startIndex = Math.Max(num - num4, 0);
					text2 = text2.Substring(startIndex, 69);
					num = Math.Min(num, num4);
					num2 = Math.Min(num2, 69 - num);
				}
				if (flag)
				{
					stringBuilder2.Append("... ");
				}
				stringBuilder2.Append(text2);
				if (flag2)
				{
					stringBuilder2.Append(" ...");
				}
				stringBuilder2.Append(Environment.NewLine);
				stringBuilder2.Append("+ ");
				stringBuilder2.Append(' ', num + (flag ? 4 : 0));
				stringBuilder2.Append('~', (num2 > 0) ? num2 : 1);
				text3 = stringBuilder2.ToString();
			}
			return StringUtil.Format(ParserStrings.TextForPositionMessage, new object[]
			{
				text,
				position.StartLineNumber,
				position.StartColumnNumber,
				text3
			});
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x0014B714 File Offset: 0x00149914
		internal static string BriefMessage(IScriptPosition position)
		{
			StringBuilder stringBuilder = new StringBuilder(position.Line);
			if (position.ColumnNumber > stringBuilder.Length)
			{
				stringBuilder.Append(" <<<< ");
			}
			else
			{
				stringBuilder.Insert(position.ColumnNumber - 1, " >>>> ");
			}
			return StringUtil.Format(ParserStrings.TraceScriptLineMessage, position.LineNumber, stringBuilder.ToString());
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0014B778 File Offset: 0x00149978
		internal static IScriptExtent NewScriptExtent(IScriptExtent start, IScriptExtent end)
		{
			if (start == end)
			{
				return start;
			}
			if (start == PositionUtilities.emptyExtent)
			{
				return end;
			}
			if (end == PositionUtilities.emptyExtent)
			{
				return start;
			}
			InternalScriptExtent internalScriptExtent = start as InternalScriptExtent;
			InternalScriptExtent internalScriptExtent2 = end as InternalScriptExtent;
			return new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.StartOffset, internalScriptExtent2.EndOffset);
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0014B7C4 File Offset: 0x001499C4
		internal static bool IsBefore(this IScriptExtent extentToTest, IScriptExtent startExtent)
		{
			return extentToTest.EndLineNumber < startExtent.StartLineNumber || (extentToTest.EndLineNumber == startExtent.StartLineNumber && extentToTest.EndColumnNumber <= startExtent.StartColumnNumber);
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0014B7F7 File Offset: 0x001499F7
		internal static bool IsAfter(this IScriptExtent extentToTest, IScriptExtent endExtent)
		{
			return extentToTest.StartLineNumber > endExtent.EndLineNumber || (extentToTest.StartLineNumber == endExtent.EndLineNumber && extentToTest.StartColumnNumber >= endExtent.EndColumnNumber);
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x0014B82A File Offset: 0x00149A2A
		internal static bool IsWithin(this IScriptExtent extentToTest, IScriptExtent extent)
		{
			return extentToTest.StartLineNumber >= extent.StartLineNumber && extentToTest.EndLineNumber <= extent.EndLineNumber && extentToTest.StartColumnNumber >= extent.StartColumnNumber && extentToTest.EndColumnNumber <= extent.EndColumnNumber;
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0014B869 File Offset: 0x00149A69
		internal static bool IsAfter(this IScriptExtent extent, int line, int column)
		{
			return line < extent.StartLineNumber || (line == extent.StartLineNumber && column < extent.StartColumnNumber);
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0014B88C File Offset: 0x00149A8C
		internal static bool ContainsLineAndColumn(this IScriptExtent extent, int line, int column)
		{
			if (extent.StartLineNumber == line)
			{
				return column == 0 || (column >= extent.StartColumnNumber && (extent.EndLineNumber != extent.StartLineNumber || column < extent.EndColumnNumber));
			}
			return extent.StartLineNumber <= line && line <= extent.EndLineNumber && (extent.EndLineNumber != line || column < extent.EndColumnNumber);
		}

		// Token: 0x04001F30 RID: 7984
		private static readonly IScriptPosition emptyPosition = new EmptyScriptPosition();

		// Token: 0x04001F31 RID: 7985
		private static readonly IScriptExtent emptyExtent = new EmptyScriptExtent();
	}
}
