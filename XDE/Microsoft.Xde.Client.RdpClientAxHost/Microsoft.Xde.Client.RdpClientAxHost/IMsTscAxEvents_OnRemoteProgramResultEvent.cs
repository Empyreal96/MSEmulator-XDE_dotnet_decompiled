using System;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000014 RID: 20
	public class IMsTscAxEvents_OnRemoteProgramResultEvent
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000035D0 File Offset: 0x000017D0
		public IMsTscAxEvents_OnRemoteProgramResultEvent(string bstrRemoteProgram, RemoteProgramResult lError, bool vbIsExecutable)
		{
			this.bstrRemoteProgram = bstrRemoteProgram;
			this.lError = lError;
			this.vbIsExecutable = vbIsExecutable;
		}

		// Token: 0x0400002F RID: 47
		public string bstrRemoteProgram;

		// Token: 0x04000030 RID: 48
		public RemoteProgramResult lError;

		// Token: 0x04000031 RID: 49
		public bool vbIsExecutable;
	}
}
