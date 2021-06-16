using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D1 RID: 1233
	public abstract class FrontEndCommandBase : PSCmdlet, IDisposable
	{
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x00124C44 File Offset: 0x00122E44
		// (set) Token: 0x060035DC RID: 13788 RVA: 0x00124C3B File Offset: 0x00122E3B
		[Parameter(ValueFromPipeline = true)]
		public PSObject InputObject
		{
			get
			{
				return this._inputObject;
			}
			set
			{
				this._inputObject = value;
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x00124C4C File Offset: 0x00122E4C
		protected override void BeginProcessing()
		{
			this.implementation.OuterCmdletCall = new ImplementationCommandBase.OuterCmdletCallback(this.OuterCmdletCall);
			this.implementation.InputObjectCall = new ImplementationCommandBase.InputObjectCallback(this.InputObjectCall);
			this.implementation.WriteObjectCall = new ImplementationCommandBase.WriteObjectCallback(this.WriteObjectCall);
			this.implementation.CreateTerminatingErrorContext();
			this.implementation.BeginProcessing();
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x00124CB7 File Offset: 0x00122EB7
		protected override void ProcessRecord()
		{
			this.implementation.ProcessRecord();
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x00124CC4 File Offset: 0x00122EC4
		protected override void EndProcessing()
		{
			this.implementation.EndProcessing();
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x00124CD1 File Offset: 0x00122ED1
		protected override void StopProcessing()
		{
			this.implementation.StopProcessing();
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x00124CDE File Offset: 0x00122EDE
		protected virtual PSCmdlet OuterCmdletCall()
		{
			return this;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x00124CE1 File Offset: 0x00122EE1
		protected virtual PSObject InputObjectCall()
		{
			return this.InputObject;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x00124CE9 File Offset: 0x00122EE9
		protected virtual void WriteObjectCall(object value)
		{
			base.WriteObject(value);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x00124CF2 File Offset: 0x00122EF2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x00124D01 File Offset: 0x00122F01
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x00124D0C File Offset: 0x00122F0C
		protected virtual void InternalDispose()
		{
			if (this.implementation == null)
			{
				return;
			}
			this.implementation.Dispose();
			this.implementation = null;
		}

		// Token: 0x04001B82 RID: 7042
		private PSObject _inputObject = AutomationNull.Value;

		// Token: 0x04001B83 RID: 7043
		internal ImplementationCommandBase implementation;
	}
}
