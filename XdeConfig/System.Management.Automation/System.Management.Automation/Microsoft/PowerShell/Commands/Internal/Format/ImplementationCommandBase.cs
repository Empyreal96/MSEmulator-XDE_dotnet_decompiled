using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C8 RID: 1224
	internal class ImplementationCommandBase : IDisposable
	{
		// Token: 0x060035AA RID: 13738 RVA: 0x00124403 File Offset: 0x00122603
		internal virtual void BeginProcessing()
		{
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x00124405 File Offset: 0x00122605
		internal virtual void ProcessRecord()
		{
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x00124407 File Offset: 0x00122607
		internal virtual void EndProcessing()
		{
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x00124409 File Offset: 0x00122609
		internal virtual void StopProcessing()
		{
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x0012440B File Offset: 0x0012260B
		internal virtual PSObject ReadObject()
		{
			return this.InputObjectCall();
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00124418 File Offset: 0x00122618
		internal virtual void WriteObject(object o)
		{
			this.WriteObjectCall(o);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00124426 File Offset: 0x00122626
		internal virtual PSCmdlet OuterCmdlet()
		{
			return this.OuterCmdletCall();
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060035B1 RID: 13745 RVA: 0x00124433 File Offset: 0x00122633
		protected TerminatingErrorContext TerminatingErrorContext
		{
			get
			{
				return this._terminatingErrorContext;
			}
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0012443B File Offset: 0x0012263B
		internal void CreateTerminatingErrorContext()
		{
			this._terminatingErrorContext = new TerminatingErrorContext(this.OuterCmdlet());
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0012444E File Offset: 0x0012264E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0012445D File Offset: 0x0012265D
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x00124468 File Offset: 0x00122668
		protected virtual void InternalDispose()
		{
		}

		// Token: 0x04001B6C RID: 7020
		private TerminatingErrorContext _terminatingErrorContext;

		// Token: 0x04001B6D RID: 7021
		internal ImplementationCommandBase.OuterCmdletCallback OuterCmdletCall;

		// Token: 0x04001B6E RID: 7022
		internal ImplementationCommandBase.InputObjectCallback InputObjectCall;

		// Token: 0x04001B6F RID: 7023
		internal ImplementationCommandBase.WriteObjectCallback WriteObjectCall;

		// Token: 0x020004C9 RID: 1225
		// (Invoke) Token: 0x060035B8 RID: 13752
		internal delegate PSCmdlet OuterCmdletCallback();

		// Token: 0x020004CA RID: 1226
		// (Invoke) Token: 0x060035BC RID: 13756
		internal delegate PSObject InputObjectCallback();

		// Token: 0x020004CB RID: 1227
		// (Invoke) Token: 0x060035C0 RID: 13760
		internal delegate void WriteObjectCallback(object o);
	}
}
