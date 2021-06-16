using System;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000003 RID: 3
	public interface IXdeEventNotifier
	{
		// Token: 0x06000003 RID: 3
		void PipeReady();

		// Token: 0x06000004 RID: 4
		void XdeReboot();

		// Token: 0x06000005 RID: 5
		void XdeExit();
	}
}
