using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000640 RID: 1600
	internal static class StringOps
	{
		// Token: 0x06004556 RID: 17750 RVA: 0x00173742 File Offset: 0x00171942
		internal static string Add(string lhs, string rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0017374B File Offset: 0x0017194B
		internal static string Add(string lhs, char rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x0017375C File Offset: 0x0017195C
		internal static string Multiply(string s, int times)
		{
			if (times < 0)
			{
				throw new ArgumentOutOfRangeException("times");
			}
			if (times == 0 || s.Length == 0)
			{
				return string.Empty;
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.RestrictedLanguage && s.Length * times > 1024)
			{
				throw InterpreterError.NewInterpreterException(times, typeof(RuntimeException), null, "StringMultiplyToolongInDataSection", ParserStrings.StringMultiplyToolongInDataSection, new object[]
				{
					1024
				});
			}
			if (s.Length == 1)
			{
				return new string(s[0], times);
			}
			return new string(ArrayOps.Multiply<char>(s.ToCharArray(), (uint)times));
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x00173808 File Offset: 0x00171A08
		internal static string FormatOperator(string formatString, object formatArgs)
		{
			string result;
			try
			{
				object[] array = formatArgs as object[];
				result = ((array != null) ? StringUtil.Format(formatString, array) : StringUtil.Format(formatString, formatArgs));
			}
			catch (FormatException ex)
			{
				throw InterpreterError.NewInterpreterException(formatString, typeof(RuntimeException), PositionUtilities.EmptyExtent, "FormatError", ParserStrings.FormatError, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x00173878 File Offset: 0x00171A78
		internal static int Compare(string strA, string strB, CultureInfo culture, CompareOptions option)
		{
			return culture.CompareInfo.Compare(strA, strB, option);
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00173888 File Offset: 0x00171A88
		internal static bool Equals(string strA, string strB, CultureInfo culture, CompareOptions option)
		{
			return culture.CompareInfo.Compare(strA, strB, option) == 0;
		}
	}
}
