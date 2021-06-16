using System;
using System.Collections;

namespace System.Management.Automation.Language
{
	// Token: 0x020005DA RID: 1498
	internal class UnscannedSubExprToken : StringLiteralToken
	{
		// Token: 0x06003FFA RID: 16378 RVA: 0x00151D2E File Offset: 0x0014FF2E
		internal UnscannedSubExprToken(InternalScriptExtent scriptExtent, TokenFlags tokenFlags, string value, BitArray skippedCharOffsets) : base(scriptExtent, tokenFlags, TokenKind.StringLiteral, value)
		{
			this.SkippedCharOffsets = skippedCharOffsets;
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003FFB RID: 16379 RVA: 0x00151D43 File Offset: 0x0014FF43
		// (set) Token: 0x06003FFC RID: 16380 RVA: 0x00151D4B File Offset: 0x0014FF4B
		internal BitArray SkippedCharOffsets { get; private set; }
	}
}
