using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000357 RID: 855
	public sealed class PSHostProcessInfo
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000EB1C8 File Offset: 0x000E93C8
		// (set) Token: 0x06002A8A RID: 10890 RVA: 0x000EB1D0 File Offset: 0x000E93D0
		public string ProcessName { get; private set; }

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000EB1D9 File Offset: 0x000E93D9
		// (set) Token: 0x06002A8C RID: 10892 RVA: 0x000EB1E1 File Offset: 0x000E93E1
		public int ProcessId { get; private set; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x000EB1EA File Offset: 0x000E93EA
		// (set) Token: 0x06002A8E RID: 10894 RVA: 0x000EB1F2 File Offset: 0x000E93F2
		public string AppDomainName { get; private set; }

		// Token: 0x06002A8F RID: 10895 RVA: 0x000EB1FB File Offset: 0x000E93FB
		private PSHostProcessInfo()
		{
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000EB204 File Offset: 0x000E9404
		internal PSHostProcessInfo(string processName, int processId, string appDomainName)
		{
			if (string.IsNullOrEmpty(processName))
			{
				throw new PSArgumentNullException("processName");
			}
			if (string.IsNullOrEmpty(appDomainName))
			{
				throw new PSArgumentNullException("appDomainName");
			}
			this.ProcessName = processName;
			this.ProcessId = processId;
			this.AppDomainName = appDomainName;
		}
	}
}
