using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A0 RID: 1184
	internal class NameEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x0011F584 File Offset: 0x0011D784
		internal NameEntryDefinition() : base("name", new string[]
		{
			"label"
		}, new Type[]
		{
			typeof(string)
		}, false)
		{
		}

		// Token: 0x04001B1C RID: 6940
		internal const string NameEntryKey = "name";
	}
}
