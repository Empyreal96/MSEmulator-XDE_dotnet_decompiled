using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000CE RID: 206
	public sealed class PSEngineEvent
	{
		// Token: 0x06000BB0 RID: 2992 RVA: 0x00043977 File Offset: 0x00041B77
		private PSEngineEvent()
		{
		}

		// Token: 0x0400052C RID: 1324
		public const string Exiting = "PowerShell.Exiting";

		// Token: 0x0400052D RID: 1325
		public const string OnIdle = "PowerShell.OnIdle";

		// Token: 0x0400052E RID: 1326
		public const string WorkflowJobStartEvent = "PowerShell.WorkflowJobStartEvent";

		// Token: 0x0400052F RID: 1327
		internal const string OnScriptBlockInvoke = "PowerShell.OnScriptBlockInvoke";

		// Token: 0x04000530 RID: 1328
		internal static readonly HashSet<string> EngineEvents = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"PowerShell.Exiting",
			"PowerShell.OnIdle",
			"PowerShell.OnScriptBlockInvoke"
		};
	}
}
