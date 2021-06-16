using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200025D RID: 605
	internal enum PSTask
	{
		// Token: 0x04000C4B RID: 3147
		None,
		// Token: 0x04000C4C RID: 3148
		CreateRunspace,
		// Token: 0x04000C4D RID: 3149
		ExecuteCommand,
		// Token: 0x04000C4E RID: 3150
		Serialization,
		// Token: 0x04000C4F RID: 3151
		PowershellConsoleStartup,
		// Token: 0x04000C50 RID: 3152
		EngineStart = 100,
		// Token: 0x04000C51 RID: 3153
		EngineStop,
		// Token: 0x04000C52 RID: 3154
		CommandStart,
		// Token: 0x04000C53 RID: 3155
		CommandStop,
		// Token: 0x04000C54 RID: 3156
		ProviderStart,
		// Token: 0x04000C55 RID: 3157
		ProviderStop,
		// Token: 0x04000C56 RID: 3158
		ExecutePipeline,
		// Token: 0x04000C57 RID: 3159
		ScheduledJob = 110,
		// Token: 0x04000C58 RID: 3160
		NamedPipe,
		// Token: 0x04000C59 RID: 3161
		ISEOperation = 120
	}
}
