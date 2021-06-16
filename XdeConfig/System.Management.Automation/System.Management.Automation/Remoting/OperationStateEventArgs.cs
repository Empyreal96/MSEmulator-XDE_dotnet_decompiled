using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C5 RID: 709
	internal sealed class OperationStateEventArgs : EventArgs
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x000C1170 File Offset: 0x000BF370
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x000C1178 File Offset: 0x000BF378
		internal OperationState OperationState
		{
			get
			{
				return this.operationState;
			}
			set
			{
				this.operationState = value;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x000C1181 File Offset: 0x000BF381
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x000C1189 File Offset: 0x000BF389
		internal EventArgs BaseEvent
		{
			get
			{
				return this.baseEvent;
			}
			set
			{
				this.baseEvent = value;
			}
		}

		// Token: 0x04000FFA RID: 4090
		private OperationState operationState;

		// Token: 0x04000FFB RID: 4091
		private EventArgs baseEvent;
	}
}
