using System;
using System.Diagnostics.Eventing;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008F4 RID: 2292
	public class EtwEventArgs : EventArgs
	{
		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060055F1 RID: 22001 RVA: 0x001C35C2 File Offset: 0x001C17C2
		// (set) Token: 0x060055F2 RID: 22002 RVA: 0x001C35CA File Offset: 0x001C17CA
		public EventDescriptor Descriptor { get; private set; }

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060055F3 RID: 22003 RVA: 0x001C35D3 File Offset: 0x001C17D3
		// (set) Token: 0x060055F4 RID: 22004 RVA: 0x001C35DB File Offset: 0x001C17DB
		public bool Success { get; private set; }

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060055F5 RID: 22005 RVA: 0x001C35E4 File Offset: 0x001C17E4
		// (set) Token: 0x060055F6 RID: 22006 RVA: 0x001C35EC File Offset: 0x001C17EC
		public object[] Payload { get; private set; }

		// Token: 0x060055F7 RID: 22007 RVA: 0x001C35F5 File Offset: 0x001C17F5
		public EtwEventArgs(EventDescriptor descriptor, bool success, object[] payload)
		{
			this.Descriptor = descriptor;
			this.Payload = payload;
			this.Success = success;
		}
	}
}
