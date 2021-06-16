using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C5 RID: 1221
	internal class FormatListParameterDefinition : FormatParameterDefinitionBase
	{
		// Token: 0x060035A5 RID: 13733 RVA: 0x0012438C File Offset: 0x0012258C
		protected override void SetEntries()
		{
			base.SetEntries();
			this.hashEntries.Add(new LabelEntryDefinition());
		}
	}
}
