using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008C1 RID: 2241
	internal class TestHelperSession : RemoteSession
	{
		// Token: 0x06005518 RID: 21784 RVA: 0x001C12B7 File Offset: 0x001BF4B7
		internal override void StartKeyExchange()
		{
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06005519 RID: 21785 RVA: 0x001C12B9 File Offset: 0x001BF4B9
		internal override RemotingDestination MySelf
		{
			get
			{
				return RemotingDestination.InvalidDestination;
			}
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x001C12BC File Offset: 0x001BF4BC
		internal override void CompleteKeyExchange()
		{
		}
	}
}
