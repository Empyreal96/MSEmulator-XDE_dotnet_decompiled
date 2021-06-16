using System;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000499 RID: 1177
	public static class CodeGeneration
	{
		// Token: 0x060034C8 RID: 13512 RVA: 0x0011EC3C File Offset: 0x0011CE3C
		public static string EscapeSingleQuotedStringContent(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			foreach (char c in value)
			{
				stringBuilder.Append(c);
				if (SpecialCharacters.IsSingleQuote(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x0011EC9C File Offset: 0x0011CE9C
		public static string EscapeBlockCommentContent(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			return value.Replace("<#", "<`#").Replace("#>", "#`>");
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x0011ECCC File Offset: 0x0011CECC
		public static string EscapeFormatStringContent(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			foreach (char c in value)
			{
				stringBuilder.Append(c);
				if (SpecialCharacters.IsCurlyBracket(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0011ED2C File Offset: 0x0011CF2C
		public static string EscapeVariableName(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			return value.Replace("`", "``").Replace("}", "`}").Replace("{", "`{");
		}
	}
}
