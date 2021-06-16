using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200000E RID: 14
	public interface IJsonLineInfo
	{
		// Token: 0x0600000C RID: 12
		bool HasLineInfo();

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13
		int LineNumber { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14
		int LinePosition { get; }
	}
}
