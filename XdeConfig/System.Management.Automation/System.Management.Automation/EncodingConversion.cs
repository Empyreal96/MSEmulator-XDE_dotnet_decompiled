using System;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200089B RID: 2203
	internal static class EncodingConversion
	{
		// Token: 0x0600546B RID: 21611 RVA: 0x001BDD9C File Offset: 0x001BBF9C
		internal static Encoding Convert(Cmdlet cmdlet, string encoding)
		{
			if (string.IsNullOrEmpty(encoding))
			{
				return Encoding.Unicode;
			}
			if (string.Equals(encoding, "unknown", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.Unicode;
			}
			if (string.Equals(encoding, "string", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.Unicode;
			}
			if (string.Equals(encoding, "unicode", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.Unicode;
			}
			if (string.Equals(encoding, "bigendianunicode", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.BigEndianUnicode;
			}
			if (string.Equals(encoding, "utf8", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.UTF8;
			}
			if (string.Equals(encoding, "ascii", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.ASCII;
			}
			if (string.Equals(encoding, "utf7", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.UTF7;
			}
			if (string.Equals(encoding, "utf32", StringComparison.OrdinalIgnoreCase))
			{
				return Encoding.UTF32;
			}
			if (string.Equals(encoding, "default", StringComparison.OrdinalIgnoreCase))
			{
				return ClrFacade.GetDefaultEncoding();
			}
			if (string.Equals(encoding, "oem", StringComparison.OrdinalIgnoreCase))
			{
				return ClrFacade.GetOEMEncoding();
			}
			string o = string.Join(", ", new string[]
			{
				"unknown",
				"string",
				"unicode",
				"bigendianunicode",
				"ascii",
				"utf8",
				"utf7",
				"utf32",
				"default",
				"oem"
			});
			string message = StringUtil.Format(PathUtilsStrings.OutFile_WriteToFileEncodingUnknown, encoding, o);
			cmdlet.ThrowTerminatingError(new ErrorRecord(PSTraceSource.NewArgumentException("Encoding"), "WriteToFileEncodingUnknown", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
			return null;
		}

		// Token: 0x04002B37 RID: 11063
		internal const string Unknown = "unknown";

		// Token: 0x04002B38 RID: 11064
		internal const string String = "string";

		// Token: 0x04002B39 RID: 11065
		internal const string Unicode = "unicode";

		// Token: 0x04002B3A RID: 11066
		internal const string BigEndianUnicode = "bigendianunicode";

		// Token: 0x04002B3B RID: 11067
		internal const string Ascii = "ascii";

		// Token: 0x04002B3C RID: 11068
		internal const string Utf8 = "utf8";

		// Token: 0x04002B3D RID: 11069
		internal const string Utf7 = "utf7";

		// Token: 0x04002B3E RID: 11070
		internal const string Utf32 = "utf32";

		// Token: 0x04002B3F RID: 11071
		internal const string Default = "default";

		// Token: 0x04002B40 RID: 11072
		internal const string OEM = "oem";
	}
}
