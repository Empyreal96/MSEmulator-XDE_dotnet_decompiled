using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000028 RID: 40
	[ComVisible(false)]
	public interface IXdeConnectionManager : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000105 RID: 261
		// (remove) Token: 0x06000106 RID: 262
		event EventHandler ConnectionsSucceeded;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000107 RID: 263
		// (remove) Token: 0x06000108 RID: 264
		event EventHandler ShellReady;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000109 RID: 265
		// (remove) Token: 0x0600010A RID: 266
		event EventHandler<MessageEventArgs> ConnectionsFailed;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600010B RID: 267
		// (remove) Token: 0x0600010C RID: 268
		event EventHandler ConnectionsReleased;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600010D RID: 269
		bool IsShellReady { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600010E RID: 270
		bool AreConnectionsUp { get; }

		// Token: 0x0600010F RID: 271
		void DoShellReady();

		// Token: 0x06000110 RID: 272
		void DoConnections();
	}
}
