using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000012 RID: 18
	public interface IXdeConnectionController
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000073 RID: 115
		// (remove) Token: 0x06000074 RID: 116
		event EventHandler ConnectionSucceeded;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000075 RID: 117
		// (remove) Token: 0x06000076 RID: 118
		event EventHandler<ExEventArgs> ConnectionFailed;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000077 RID: 119
		string Name { get; }

		// Token: 0x06000078 RID: 120
		void InitiateConnection();

		// Token: 0x06000079 RID: 121
		void DisconnectFromGuest();
	}
}
