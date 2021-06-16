using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006E8 RID: 1768
	[Serializable]
	internal class DebugInfo
	{
		// Token: 0x060048DA RID: 18650 RVA: 0x0017F9E8 File Offset: 0x0017DBE8
		public static DebugInfo GetMatchingDebugInfo(DebugInfo[] debugInfos, int index)
		{
			DebugInfo value = new DebugInfo
			{
				Index = index
			};
			int num = Array.BinarySearch<DebugInfo>(debugInfos, value, DebugInfo._debugComparer);
			if (num < 0)
			{
				num = ~num;
				if (num == 0)
				{
					return null;
				}
				num--;
			}
			return debugInfos[num];
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0017FA24 File Offset: 0x0017DC24
		public override string ToString()
		{
			if (this.IsClear)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}: clear", new object[]
				{
					this.Index
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}: [{1}-{2}] '{3}'", new object[]
			{
				this.Index,
				this.StartLine,
				this.EndLine,
				this.FileName
			});
		}

		// Token: 0x0400239D RID: 9117
		public int StartLine;

		// Token: 0x0400239E RID: 9118
		public int EndLine;

		// Token: 0x0400239F RID: 9119
		public int Index;

		// Token: 0x040023A0 RID: 9120
		public string FileName;

		// Token: 0x040023A1 RID: 9121
		public bool IsClear;

		// Token: 0x040023A2 RID: 9122
		private static readonly DebugInfo.DebugInfoComparer _debugComparer = new DebugInfo.DebugInfoComparer();

		// Token: 0x020006E9 RID: 1769
		private class DebugInfoComparer : IComparer<DebugInfo>
		{
			// Token: 0x060048DE RID: 18654 RVA: 0x0017FABD File Offset: 0x0017DCBD
			int IComparer<DebugInfo>.Compare(DebugInfo d1, DebugInfo d2)
			{
				if (d1.Index > d2.Index)
				{
					return 1;
				}
				if (d1.Index == d2.Index)
				{
					return 0;
				}
				return -1;
			}
		}
	}
}
