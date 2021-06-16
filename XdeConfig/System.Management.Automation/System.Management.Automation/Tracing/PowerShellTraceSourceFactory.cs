using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008E2 RID: 2274
	public static class PowerShellTraceSourceFactory
	{
		// Token: 0x060055BF RID: 21951 RVA: 0x001C2CDB File Offset: 0x001C0EDB
		public static PowerShellTraceSource GetTraceSource()
		{
			return new PowerShellTraceSource(PowerShellTraceTask.None, PowerShellTraceKeywords.None);
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x001C2CE5 File Offset: 0x001C0EE5
		public static PowerShellTraceSource GetTraceSource(PowerShellTraceTask task)
		{
			return new PowerShellTraceSource(task, PowerShellTraceKeywords.None);
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x001C2CEF File Offset: 0x001C0EEF
		public static PowerShellTraceSource GetTraceSource(PowerShellTraceTask task, PowerShellTraceKeywords keywords)
		{
			return new PowerShellTraceSource(task, keywords);
		}
	}
}
