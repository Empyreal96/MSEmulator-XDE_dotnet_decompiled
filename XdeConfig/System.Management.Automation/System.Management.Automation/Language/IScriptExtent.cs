using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B5 RID: 1461
	public interface IScriptExtent
	{
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003E44 RID: 15940
		string File { get; }

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003E45 RID: 15941
		IScriptPosition StartScriptPosition { get; }

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003E46 RID: 15942
		IScriptPosition EndScriptPosition { get; }

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003E47 RID: 15943
		int StartLineNumber { get; }

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003E48 RID: 15944
		int StartColumnNumber { get; }

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003E49 RID: 15945
		int EndLineNumber { get; }

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003E4A RID: 15946
		int EndColumnNumber { get; }

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003E4B RID: 15947
		string Text { get; }

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003E4C RID: 15948
		int StartOffset { get; }

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06003E4D RID: 15949
		int EndOffset { get; }
	}
}
