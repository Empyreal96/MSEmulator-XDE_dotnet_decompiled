using System;
using System.Globalization;
using System.Management.Automation.Host;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008AC RID: 2220
	internal static class StringUtil
	{
		// Token: 0x060054C2 RID: 21698 RVA: 0x001BF3FC File Offset: 0x001BD5FC
		internal static string Format(string formatSpec, object o)
		{
			return string.Format(CultureInfo.CurrentCulture, formatSpec, new object[]
			{
				o
			});
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x001BF420 File Offset: 0x001BD620
		internal static string Format(string formatSpec, object o1, object o2)
		{
			return string.Format(CultureInfo.CurrentCulture, formatSpec, new object[]
			{
				o1,
				o2
			});
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x001BF448 File Offset: 0x001BD648
		internal static string Format(string formatSpec, params object[] o)
		{
			return string.Format(CultureInfo.CurrentCulture, formatSpec, o);
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x001BF458 File Offset: 0x001BD658
		internal static string TruncateToBufferCellWidth(PSHostRawUserInterface rawUI, string toTruncate, int maxWidthInBufferCells)
		{
			int num = Math.Min(toTruncate.Length, maxWidthInBufferCells);
			string text;
			for (;;)
			{
				text = toTruncate.Substring(0, num);
				int num2 = rawUI.LengthInBufferCells(text);
				if (num2 <= maxWidthInBufferCells)
				{
					break;
				}
				num--;
			}
			return text;
		}
	}
}
