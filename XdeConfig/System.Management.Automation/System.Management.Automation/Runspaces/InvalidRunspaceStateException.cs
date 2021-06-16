using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001EE RID: 494
	[Serializable]
	public class InvalidRunspaceStateException : SystemException
	{
		// Token: 0x0600168B RID: 5771 RVA: 0x00090147 File Offset: 0x0008E347
		public InvalidRunspaceStateException() : base(StringUtil.Format(RunspaceStrings.InvalidRunspaceStateGeneral, new object[0]))
		{
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0009015F File Offset: 0x0008E35F
		public InvalidRunspaceStateException(string message) : base(message)
		{
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00090168 File Offset: 0x0008E368
		public InvalidRunspaceStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00090172 File Offset: 0x0008E372
		internal InvalidRunspaceStateException(string message, RunspaceState currentState, RunspaceState expectedState) : base(message)
		{
			this._expectedState = expectedState;
			this._currentState = currentState;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00090189 File Offset: 0x0008E389
		protected InvalidRunspaceStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x00090193 File Offset: 0x0008E393
		// (set) Token: 0x06001691 RID: 5777 RVA: 0x0009019B File Offset: 0x0008E39B
		public RunspaceState CurrentState
		{
			get
			{
				return this._currentState;
			}
			internal set
			{
				this._currentState = value;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x000901A4 File Offset: 0x0008E3A4
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x000901AC File Offset: 0x0008E3AC
		public RunspaceState ExpectedState
		{
			get
			{
				return this._expectedState;
			}
			internal set
			{
				this._expectedState = value;
			}
		}

		// Token: 0x040009A4 RID: 2468
		[NonSerialized]
		private RunspaceState _currentState;

		// Token: 0x040009A5 RID: 2469
		[NonSerialized]
		private RunspaceState _expectedState;
	}
}
