using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F1 RID: 497
	public sealed class RunspaceStateInfo
	{
		// Token: 0x06001694 RID: 5780 RVA: 0x000901B5 File Offset: 0x0008E3B5
		internal RunspaceStateInfo(RunspaceState state) : this(state, null)
		{
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000901BF File Offset: 0x0008E3BF
		internal RunspaceStateInfo(RunspaceState state, Exception reason)
		{
			this._state = state;
			this._reason = reason;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x000901D5 File Offset: 0x0008E3D5
		internal RunspaceStateInfo(RunspaceStateInfo runspaceStateInfo)
		{
			this._state = runspaceStateInfo.State;
			this._reason = runspaceStateInfo.Reason;
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x000901F5 File Offset: 0x0008E3F5
		public RunspaceState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x000901FD File Offset: 0x0008E3FD
		public Exception Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00090205 File Offset: 0x0008E405
		public override string ToString()
		{
			return this._state.ToString();
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00090217 File Offset: 0x0008E417
		internal RunspaceStateInfo Clone()
		{
			return new RunspaceStateInfo(this);
		}

		// Token: 0x040009B5 RID: 2485
		private RunspaceState _state;

		// Token: 0x040009B6 RID: 2486
		private Exception _reason;
	}
}
