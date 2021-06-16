using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000009 RID: 9
	[ComVisible(false)]
	public interface IXdeConnectionAddressInfo : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600001C RID: 28
		// (remove) Token: 0x0600001D RID: 29
		event EventHandler Ready;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600001E RID: 30
		// (remove) Token: 0x0600001F RID: 31
		event EventHandler<MessageEventArgs> Failed;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000020 RID: 32
		string HostIpAddress { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000021 RID: 33
		string GuestIpAddress { get; }

		// Token: 0x06000022 RID: 34
		Socket CreateSocket();

		// Token: 0x06000023 RID: 35
		EndPoint GetEndPoint();
	}
}
