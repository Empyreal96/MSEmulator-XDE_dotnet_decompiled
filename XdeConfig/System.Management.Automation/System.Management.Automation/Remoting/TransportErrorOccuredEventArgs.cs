using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200035D RID: 861
	internal class TransportErrorOccuredEventArgs : EventArgs
	{
		// Token: 0x06002AA9 RID: 10921 RVA: 0x000EB45A File Offset: 0x000E965A
		internal TransportErrorOccuredEventArgs(PSRemotingTransportException e, TransportMethodEnum m)
		{
			this.exception = e;
			this.method = m;
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002AAA RID: 10922 RVA: 0x000EB470 File Offset: 0x000E9670
		// (set) Token: 0x06002AAB RID: 10923 RVA: 0x000EB478 File Offset: 0x000E9678
		internal PSRemotingTransportException Exception
		{
			get
			{
				return this.exception;
			}
			set
			{
				this.exception = value;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002AAC RID: 10924 RVA: 0x000EB481 File Offset: 0x000E9681
		internal TransportMethodEnum ReportingTransportMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x04001525 RID: 5413
		private PSRemotingTransportException exception;

		// Token: 0x04001526 RID: 5414
		private TransportMethodEnum method;
	}
}
