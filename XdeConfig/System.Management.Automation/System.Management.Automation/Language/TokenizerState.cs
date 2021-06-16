using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005E2 RID: 1506
	internal class TokenizerState
	{
		// Token: 0x0400205C RID: 8284
		internal int NestedTokensAdjustment;

		// Token: 0x0400205D RID: 8285
		internal string Script;

		// Token: 0x0400205E RID: 8286
		internal int TokenStart;

		// Token: 0x0400205F RID: 8287
		internal int CurrentIndex;

		// Token: 0x04002060 RID: 8288
		internal Token LastToken;

		// Token: 0x04002061 RID: 8289
		internal BitArray SkippedCharOffsets;

		// Token: 0x04002062 RID: 8290
		internal List<Token> TokenList;
	}
}
