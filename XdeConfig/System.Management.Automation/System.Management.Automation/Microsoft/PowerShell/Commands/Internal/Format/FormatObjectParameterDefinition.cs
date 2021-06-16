using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C7 RID: 1223
	internal class FormatObjectParameterDefinition : CommandParameterDefinition
	{
		// Token: 0x060035A8 RID: 13736 RVA: 0x001243B4 File Offset: 0x001225B4
		protected override void SetEntries()
		{
			this.hashEntries.Add(new ExpressionEntryDefinition());
			this.hashEntries.Add(new HashtableEntryDefinition("depth", new Type[]
			{
				typeof(int)
			}));
		}
	}
}
