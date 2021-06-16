using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Host
{
	// Token: 0x02000208 RID: 520
	public interface IHostSupportsInteractiveSession
	{
		// Token: 0x0600180E RID: 6158
		void PushRunspace(Runspace runspace);

		// Token: 0x0600180F RID: 6159
		void PopRunspace();

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001810 RID: 6160
		bool IsRunspacePushed { get; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001811 RID: 6161
		Runspace Runspace { get; }
	}
}
