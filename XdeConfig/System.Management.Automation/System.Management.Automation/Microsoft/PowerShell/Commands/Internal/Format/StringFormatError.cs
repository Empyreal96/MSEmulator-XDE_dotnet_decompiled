using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000911 RID: 2321
	internal sealed class StringFormatError : FormattingError
	{
		// Token: 0x04002E72 RID: 11890
		internal string formatString;

		// Token: 0x04002E73 RID: 11891
		internal Exception exception;
	}
}
