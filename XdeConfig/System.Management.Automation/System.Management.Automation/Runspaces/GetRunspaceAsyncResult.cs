using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200024A RID: 586
	internal sealed class GetRunspaceAsyncResult : AsyncResult
	{
		// Token: 0x06001B8C RID: 7052 RVA: 0x000A1954 File Offset: 0x0009FB54
		internal GetRunspaceAsyncResult(Guid ownerId, AsyncCallback callback, object state) : base(ownerId, callback, state)
		{
			this.isActive = true;
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001B8D RID: 7053 RVA: 0x000A1966 File Offset: 0x0009FB66
		// (set) Token: 0x06001B8E RID: 7054 RVA: 0x000A196E File Offset: 0x0009FB6E
		internal Runspace Runspace
		{
			get
			{
				return this.runspace;
			}
			set
			{
				this.runspace = value;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x000A1978 File Offset: 0x0009FB78
		// (set) Token: 0x06001B90 RID: 7056 RVA: 0x000A19BC File Offset: 0x0009FBBC
		internal bool IsActive
		{
			get
			{
				bool result;
				lock (base.SyncObject)
				{
					result = this.isActive;
				}
				return result;
			}
			set
			{
				lock (base.SyncObject)
				{
					this.isActive = value;
				}
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000A1A00 File Offset: 0x0009FC00
		internal void DoComplete(object state)
		{
			base.SetAsCompleted(null);
		}

		// Token: 0x04000B56 RID: 2902
		private Runspace runspace;

		// Token: 0x04000B57 RID: 2903
		private bool isActive;
	}
}
