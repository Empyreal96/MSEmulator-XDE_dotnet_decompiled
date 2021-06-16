using System;

namespace System.Management.Automation
{
	// Token: 0x0200023E RID: 574
	public sealed class DataAddingEventArgs : EventArgs
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x0009FF94 File Offset: 0x0009E194
		internal DataAddingEventArgs(Guid psInstanceId, object itemAdded)
		{
			this.psInstanceId = psInstanceId;
			this.itemAdded = itemAdded;
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0009FFAA File Offset: 0x0009E1AA
		public object ItemAdded
		{
			get
			{
				return this.itemAdded;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x0009FFB2 File Offset: 0x0009E1B2
		public Guid PowerShellInstanceId
		{
			get
			{
				return this.psInstanceId;
			}
		}

		// Token: 0x04000B1E RID: 2846
		private Guid psInstanceId;

		// Token: 0x04000B1F RID: 2847
		private object itemAdded;
	}
}
