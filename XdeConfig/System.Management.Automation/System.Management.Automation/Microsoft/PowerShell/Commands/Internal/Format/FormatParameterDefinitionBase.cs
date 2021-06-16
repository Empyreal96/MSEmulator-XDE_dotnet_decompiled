using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C3 RID: 1219
	internal class FormatParameterDefinitionBase : CommandParameterDefinition
	{
		// Token: 0x060035A1 RID: 13729 RVA: 0x00124322 File Offset: 0x00122522
		protected override void SetEntries()
		{
			this.hashEntries.Add(new ExpressionEntryDefinition());
			this.hashEntries.Add(new FormatStringDefinition());
		}
	}
}
