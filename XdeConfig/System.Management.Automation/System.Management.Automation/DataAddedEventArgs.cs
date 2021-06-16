using System;

namespace System.Management.Automation
{
	// Token: 0x0200023D RID: 573
	public sealed class DataAddedEventArgs : EventArgs
	{
		// Token: 0x06001B02 RID: 6914 RVA: 0x0009FF6E File Offset: 0x0009E16E
		internal DataAddedEventArgs(Guid psInstanceId, int index)
		{
			this.psInstanceId = psInstanceId;
			this.index = index;
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x0009FF84 File Offset: 0x0009E184
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0009FF8C File Offset: 0x0009E18C
		public Guid PowerShellInstanceId
		{
			get
			{
				return this.psInstanceId;
			}
		}

		// Token: 0x04000B1C RID: 2844
		private int index;

		// Token: 0x04000B1D RID: 2845
		private Guid psInstanceId;
	}
}
