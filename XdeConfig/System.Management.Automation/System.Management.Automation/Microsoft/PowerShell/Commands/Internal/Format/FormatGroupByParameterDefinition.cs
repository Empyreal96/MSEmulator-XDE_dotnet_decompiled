using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C2 RID: 1218
	internal class FormatGroupByParameterDefinition : CommandParameterDefinition
	{
		// Token: 0x0600359F RID: 13727 RVA: 0x001242E8 File Offset: 0x001224E8
		protected override void SetEntries()
		{
			this.hashEntries.Add(new ExpressionEntryDefinition());
			this.hashEntries.Add(new FormatStringDefinition());
			this.hashEntries.Add(new LabelEntryDefinition());
		}
	}
}
