using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000002 RID: 2
	public class ByteSizeFormatProvider : IFormatProvider, ICustomFormatter
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public object GetFormat(Type formatType)
		{
			if (!(formatType == typeof(ICustomFormatter)))
			{
				return null;
			}
			return this;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (format == null)
			{
				return ByteSizeFormatProvider.DefaultFormat(format, arg, formatProvider);
			}
			if (arg is string)
			{
				return ByteSizeFormatProvider.DefaultFormat(format, arg, formatProvider);
			}
			decimal num;
			try
			{
				num = Convert.ToDecimal(arg);
			}
			catch (InvalidCastException)
			{
				return ByteSizeFormatProvider.DefaultFormat(format, arg, formatProvider);
			}
			string format2;
			if (format.StartsWith("au", StringComparison.OrdinalIgnoreCase))
			{
				ByteSizeFormatProvider.InitForAuto(ref num, out format2);
			}
			else if (format.StartsWith("by", StringComparison.OrdinalIgnoreCase))
			{
				ByteSizeFormatProvider.InitForBytes(ref num, out format2);
			}
			else if (format.StartsWith("kb", StringComparison.OrdinalIgnoreCase))
			{
				ByteSizeFormatProvider.InitForKB(ref num, out format2);
			}
			else if (format.StartsWith("mb", StringComparison.OrdinalIgnoreCase))
			{
				ByteSizeFormatProvider.InitForMB(ref num, out format2);
			}
			else if (format.StartsWith("gb", StringComparison.OrdinalIgnoreCase))
			{
				ByteSizeFormatProvider.InitForGB(ref num, out format2);
			}
			else
			{
				if (!format.StartsWith("tb", StringComparison.OrdinalIgnoreCase))
				{
					return ByteSizeFormatProvider.DefaultFormat(format, arg, formatProvider);
				}
				ByteSizeFormatProvider.InitForTB(ref num, out format2);
			}
			string text = format.Substring(2);
			if (string.IsNullOrEmpty(text))
			{
				text = "2";
			}
			return StringUtilities.CurrentCultureFormat(StringUtilities.CurrentCultureFormat(format2, new object[]
			{
				"{0:N" + text + "}"
			}), new object[]
			{
				num
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021A0 File Offset: 0x000003A0
		private static void InitForAuto(ref decimal value, out string format)
		{
			if (value > 1099511627776m)
			{
				ByteSizeFormatProvider.InitForTB(ref value, out format);
				return;
			}
			if (value > 1073741824m)
			{
				ByteSizeFormatProvider.InitForGB(ref value, out format);
				return;
			}
			if (value > 1048576m)
			{
				ByteSizeFormatProvider.InitForMB(ref value, out format);
				return;
			}
			if (value > 1024m)
			{
				ByteSizeFormatProvider.InitForKB(ref value, out format);
				return;
			}
			ByteSizeFormatProvider.InitForBytes(ref value, out format);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002234 File Offset: 0x00000434
		private static void InitForBytes(ref decimal value, out string format)
		{
			format = Strings.BytesSize_BFormat;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000223D File Offset: 0x0000043D
		private static void InitForKB(ref decimal value, out string format)
		{
			format = Strings.BytesSize_KBFormat;
			value /= 1024m;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002261 File Offset: 0x00000461
		private static void InitForMB(ref decimal value, out string format)
		{
			format = Strings.BytesSize_MBFormat;
			value /= 1048576m;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002285 File Offset: 0x00000485
		private static void InitForGB(ref decimal value, out string format)
		{
			format = Strings.BytesSize_GBFormat;
			value /= 1073741824m;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022A9 File Offset: 0x000004A9
		private static void InitForTB(ref decimal value, out string format)
		{
			format = Strings.BytesSize_TBFormat;
			value /= 1099511627776m;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022D4 File Offset: 0x000004D4
		private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
		{
			IFormattable formattable = arg as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return arg.ToString();
		}

		// Token: 0x04000001 RID: 1
		private const string AutoSizeFormat = "au";

		// Token: 0x04000002 RID: 2
		private const string BytesSizeFormat = "by";

		// Token: 0x04000003 RID: 3
		private const string KBSizeFormat = "kb";

		// Token: 0x04000004 RID: 4
		private const string MBSizeFormat = "mb";

		// Token: 0x04000005 RID: 5
		private const string GBSizeFormat = "gb";

		// Token: 0x04000006 RID: 6
		private const string TBSizeFormat = "tb";

		// Token: 0x04000007 RID: 7
		private const decimal OneKiloByte = 1024m;

		// Token: 0x04000008 RID: 8
		private const decimal OneMegaByte = 1048576m;

		// Token: 0x04000009 RID: 9
		private const decimal OneGigaByte = 1073741824m;

		// Token: 0x0400000A RID: 10
		private const decimal OneTeraByte = 1099511627776m;
	}
}
