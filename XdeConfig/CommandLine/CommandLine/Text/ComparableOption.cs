using System;

namespace CommandLine.Text
{
	// Token: 0x02000058 RID: 88
	public struct ComparableOption
	{
		// Token: 0x0400009D RID: 157
		public bool Required;

		// Token: 0x0400009E RID: 158
		public bool IsOption;

		// Token: 0x0400009F RID: 159
		public bool IsValue;

		// Token: 0x040000A0 RID: 160
		public string LongName;

		// Token: 0x040000A1 RID: 161
		public string ShortName;

		// Token: 0x040000A2 RID: 162
		public int Index;
	}
}
