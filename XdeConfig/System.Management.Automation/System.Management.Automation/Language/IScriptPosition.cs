using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B4 RID: 1460
	public interface IScriptPosition
	{
		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06003E3E RID: 15934
		string File { get; }

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003E3F RID: 15935
		int LineNumber { get; }

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06003E40 RID: 15936
		int ColumnNumber { get; }

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06003E41 RID: 15937
		int Offset { get; }

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003E42 RID: 15938
		string Line { get; }

		// Token: 0x06003E43 RID: 15939
		string GetFullScript();
	}
}
