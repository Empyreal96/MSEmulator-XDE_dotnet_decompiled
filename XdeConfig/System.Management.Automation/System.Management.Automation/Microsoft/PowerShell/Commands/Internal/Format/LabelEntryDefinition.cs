using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004BE RID: 1214
	internal class LabelEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x0600359A RID: 13722 RVA: 0x00124208 File Offset: 0x00122408
		internal LabelEntryDefinition() : base("label", new string[]
		{
			"name"
		}, new Type[]
		{
			typeof(string)
		}, false)
		{
		}
	}
}
