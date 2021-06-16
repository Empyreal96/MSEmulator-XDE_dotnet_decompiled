using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000237 RID: 567
	internal sealed class PowerShellAsyncResult : AsyncResult
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x0009B895 File Offset: 0x00099A95
		internal bool IsAssociatedWithAsyncInvoke
		{
			get
			{
				return this.isAssociatedWithAsyncInvoke;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x0009B89D File Offset: 0x00099A9D
		internal PSDataCollection<PSObject> Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0009B8A5 File Offset: 0x00099AA5
		internal PowerShellAsyncResult(Guid ownerId, AsyncCallback callback, object state, PSDataCollection<PSObject> output, bool isCalledFromBeginInvoke) : base(ownerId, callback, state)
		{
			this.isAssociatedWithAsyncInvoke = isCalledFromBeginInvoke;
			this.output = output;
		}

		// Token: 0x04000AE4 RID: 2788
		private bool isAssociatedWithAsyncInvoke;

		// Token: 0x04000AE5 RID: 2789
		private PSDataCollection<PSObject> output;
	}
}
