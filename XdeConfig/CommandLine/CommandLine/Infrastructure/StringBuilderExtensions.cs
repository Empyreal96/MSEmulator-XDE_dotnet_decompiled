using System;
using System.Text;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000065 RID: 101
	internal static class StringBuilderExtensions
	{
		// Token: 0x06000288 RID: 648 RVA: 0x0000A560 File Offset: 0x00008760
		public static StringBuilder AppendWhen(this StringBuilder builder, bool condition, params string[] values)
		{
			if (condition)
			{
				foreach (string value in values)
				{
					builder.Append(value);
				}
			}
			return builder;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A590 File Offset: 0x00008790
		public static StringBuilder AppendWhen(this StringBuilder builder, bool condition, params char[] values)
		{
			if (condition)
			{
				foreach (char value in values)
				{
					builder.Append(value);
				}
			}
			return builder;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A5BD File Offset: 0x000087BD
		public static StringBuilder AppendFormatWhen(this StringBuilder builder, bool condition, string format, params object[] args)
		{
			if (!condition)
			{
				return builder;
			}
			return builder.AppendFormat(format, args);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A5CC File Offset: 0x000087CC
		public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string ifTrue, string ifFalse)
		{
			if (!condition)
			{
				return builder.Append(ifFalse);
			}
			return builder.Append(ifTrue);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public static StringBuilder BimapIf(this StringBuilder builder, bool condition, Func<StringBuilder, StringBuilder> ifTrue, Func<StringBuilder, StringBuilder> ifFalse)
		{
			if (!condition)
			{
				return ifFalse(builder);
			}
			return ifTrue(builder);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A5F4 File Offset: 0x000087F4
		public static StringBuilder MapIf(this StringBuilder builder, bool condition, Func<StringBuilder, StringBuilder> ifTrue)
		{
			if (!condition)
			{
				return builder;
			}
			return ifTrue(builder);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A604 File Offset: 0x00008804
		public static StringBuilder AppendIfNotEmpty(this StringBuilder builder, params string[] values)
		{
			foreach (string text in values)
			{
				if (text.Length > 0)
				{
					builder.Append(text);
				}
			}
			return builder;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A637 File Offset: 0x00008837
		public static string SafeToString(this StringBuilder builder)
		{
			if (builder != null)
			{
				return builder.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A648 File Offset: 0x00008848
		public static int SafeLength(this StringBuilder builder)
		{
			if (builder != null)
			{
				return builder.Length;
			}
			return 0;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A655 File Offset: 0x00008855
		public static StringBuilder TrimEnd(this StringBuilder builder, char c)
		{
			if (builder.Length <= 0)
			{
				return builder;
			}
			return builder.Remove(builder.Length - 1, 1);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A671 File Offset: 0x00008871
		public static StringBuilder TrimEndIfMatch(this StringBuilder builder, char c)
		{
			if (builder.Length > 0 && builder[builder.Length - 1] == c)
			{
				builder.Remove(builder.Length - 1, 1);
			}
			return builder;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A69E File Offset: 0x0000889E
		public static StringBuilder TrimEndIfMatchWhen(this StringBuilder builder, bool condition, char c)
		{
			if (!condition)
			{
				return builder;
			}
			return builder.TrimEndIfMatch(c);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static int TrailingSpaces(this StringBuilder builder)
		{
			int num = builder.Length - 1;
			if (builder.Length == 0)
			{
				return 0;
			}
			if (builder[num] != ' ')
			{
				return 0;
			}
			int num2 = 0;
			int num3 = num;
			while (num3 <= num && num3 >= 0 && builder[num3] == ' ')
			{
				num2++;
				num3--;
			}
			return num2;
		}
	}
}
