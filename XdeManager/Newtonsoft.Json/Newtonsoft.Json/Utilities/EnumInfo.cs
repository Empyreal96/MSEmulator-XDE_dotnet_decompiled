using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004F RID: 79
	internal class EnumInfo
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x000157AB File Offset: 0x000139AB
		public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
		{
			this.IsFlags = isFlags;
			this.Values = values;
			this.Names = names;
			this.ResolvedNames = resolvedNames;
		}

		// Token: 0x040001BD RID: 445
		public readonly bool IsFlags;

		// Token: 0x040001BE RID: 446
		public readonly ulong[] Values;

		// Token: 0x040001BF RID: 447
		public readonly string[] Names;

		// Token: 0x040001C0 RID: 448
		public readonly string[] ResolvedNames;
	}
}
