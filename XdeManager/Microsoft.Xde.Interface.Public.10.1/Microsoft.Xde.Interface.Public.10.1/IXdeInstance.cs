using System;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000004 RID: 4
	public interface IXdeInstance
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6
		VirtualMachineEnabledState VmState { get; }

		// Token: 0x06000007 RID: 7
		void Connect(string virtualMachineName);

		// Token: 0x06000008 RID: 8
		void GetEndPoint(out string hostIP, out string deviceIP);

		// Token: 0x06000009 RID: 9
		void BringToFront();

		// Token: 0x0600000A RID: 10
		void Close();

		// Token: 0x0600000B RID: 11
		bool IsToolsPipeReady();

		// Token: 0x0600000C RID: 12
		void Disconnect();

		// Token: 0x0600000D RID: 13
		void Connect(string virtualMachineName, TimeSpan timeout);
	}
}
