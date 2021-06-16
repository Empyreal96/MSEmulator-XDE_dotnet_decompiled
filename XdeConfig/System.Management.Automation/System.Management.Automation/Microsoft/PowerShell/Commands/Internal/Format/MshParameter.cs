using System;
using System.Collections;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200049E RID: 1182
	internal class MshParameter
	{
		// Token: 0x060034E2 RID: 13538 RVA: 0x0011F479 File Offset: 0x0011D679
		internal object GetEntry(string key)
		{
			if (this.hash.ContainsKey(key))
			{
				return this.hash[key];
			}
			return AutomationNull.Value;
		}

		// Token: 0x04001B17 RID: 6935
		internal Hashtable hash;
	}
}
