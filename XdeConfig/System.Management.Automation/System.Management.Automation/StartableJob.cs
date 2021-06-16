using System;

namespace System.Management.Automation
{
	// Token: 0x0200028F RID: 655
	internal abstract class StartableJob : Job
	{
		// Token: 0x06001F67 RID: 8039 RVA: 0x000B5D62 File Offset: 0x000B3F62
		internal StartableJob(string commandName, string jobName) : base(commandName, jobName)
		{
		}

		// Token: 0x06001F68 RID: 8040
		internal abstract void StartJob();
	}
}
