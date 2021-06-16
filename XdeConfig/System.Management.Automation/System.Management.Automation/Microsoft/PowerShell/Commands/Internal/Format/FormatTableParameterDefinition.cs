using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C4 RID: 1220
	internal class FormatTableParameterDefinition : FormatParameterDefinitionBase
	{
		// Token: 0x060035A3 RID: 13731 RVA: 0x0012434C File Offset: 0x0012254C
		protected override void SetEntries()
		{
			base.SetEntries();
			this.hashEntries.Add(new WidthEntryDefinition());
			this.hashEntries.Add(new AligmentEntryDefinition());
			this.hashEntries.Add(new LabelEntryDefinition());
		}
	}
}
