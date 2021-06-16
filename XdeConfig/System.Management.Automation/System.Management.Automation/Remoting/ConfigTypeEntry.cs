using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200036D RID: 877
	internal class ConfigTypeEntry
	{
		// Token: 0x06002B2E RID: 11054 RVA: 0x000ED920 File Offset: 0x000EBB20
		internal ConfigTypeEntry(string key, ConfigTypeEntry.TypeValidationCallback callback)
		{
			this.Key = key;
			this.ValidationCallback = callback;
		}

		// Token: 0x04001598 RID: 5528
		internal string Key;

		// Token: 0x04001599 RID: 5529
		internal ConfigTypeEntry.TypeValidationCallback ValidationCallback;

		// Token: 0x0200036E RID: 878
		// (Invoke) Token: 0x06002B30 RID: 11056
		internal delegate bool TypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path);
	}
}
