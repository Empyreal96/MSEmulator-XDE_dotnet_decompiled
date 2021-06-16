using System;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000049 RID: 73
	public enum RemoteProgramResult
	{
		// Token: 0x04000006 RID: 6
		remoteAppResultOk,
		// Token: 0x04000007 RID: 7
		remoteAppResultLocked,
		// Token: 0x04000008 RID: 8
		remoteAppResultProtocolError,
		// Token: 0x04000009 RID: 9
		remoteAppResultNotInWhitelist,
		// Token: 0x0400000A RID: 10
		remoteAppResultNetworkPathDenied,
		// Token: 0x0400000B RID: 11
		remoteAppResultFileNotFound,
		// Token: 0x0400000C RID: 12
		remoteAppResultFailure,
		// Token: 0x0400000D RID: 13
		remoteAppResultHookNotLoaded
	}
}
