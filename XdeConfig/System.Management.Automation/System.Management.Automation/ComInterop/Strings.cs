using System;
using System.Globalization;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A82 RID: 2690
	internal static class Strings
	{
		// Token: 0x06006ADF RID: 27359 RVA: 0x00218758 File Offset: 0x00216958
		private static string FormatString(string format, params object[] args)
		{
			return string.Format(CultureInfo.CurrentCulture, format, args);
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x00218768 File Offset: 0x00216968
		internal static string UnexpectedVarEnum(object p0)
		{
			return Strings.FormatString(ParserStrings.UnexpectedVarEnum, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE1 RID: 27361 RVA: 0x0021878C File Offset: 0x0021698C
		internal static string DispBadParamCount(object p0, int parameterCount)
		{
			return Strings.FormatString(ParserStrings.DispBadParamCount, new object[]
			{
				p0,
				parameterCount
			});
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x002187B8 File Offset: 0x002169B8
		internal static string DispMemberNotFound(object p0)
		{
			return Strings.FormatString(ParserStrings.DispMemberNotFound, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE3 RID: 27363 RVA: 0x002187DC File Offset: 0x002169DC
		internal static string DispNoNamedArgs(object p0)
		{
			return Strings.FormatString(ParserStrings.DispNoNamedArgs, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE4 RID: 27364 RVA: 0x00218800 File Offset: 0x00216A00
		internal static string DispOverflow(object p0)
		{
			return Strings.FormatString(ParserStrings.DispOverflow, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE5 RID: 27365 RVA: 0x00218824 File Offset: 0x00216A24
		internal static string DispTypeMismatch(object method, string value, string originalTypeName, string destinationTypeName)
		{
			return Strings.FormatString(ParserStrings.DispTypeMismatch, new object[]
			{
				method,
				value,
				originalTypeName,
				destinationTypeName
			});
		}

		// Token: 0x06006AE6 RID: 27366 RVA: 0x00218854 File Offset: 0x00216A54
		internal static string DispParamNotOptional(object p0)
		{
			return Strings.FormatString(ParserStrings.DispParamNotOptional, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x00218878 File Offset: 0x00216A78
		internal static string GetIDsOfNamesInvalid(object p0)
		{
			return Strings.FormatString(ParserStrings.GetIDsOfNamesInvalid, new object[]
			{
				p0
			});
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x0021889C File Offset: 0x00216A9C
		internal static string CouldNotGetDispId(object p0, object p1)
		{
			return Strings.FormatString(ParserStrings.CouldNotGetDispId, new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x002188C4 File Offset: 0x00216AC4
		internal static string AmbiguousConversion(object p0, object p1)
		{
			return Strings.FormatString(ParserStrings.AmbiguousConversion, new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x002188EC File Offset: 0x00216AEC
		internal static string VariantGetAccessorNYI(object p0)
		{
			return Strings.FormatString(ParserStrings.VariantGetAccessorNYI, new object[]
			{
				p0
			});
		}
	}
}
