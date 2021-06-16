using System;

namespace System.Management.Automation
{
	// Token: 0x020002B7 RID: 695
	internal sealed class RemoteDataEventArgs<T> : EventArgs
	{
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000C0DB7 File Offset: 0x000BEFB7
		internal T Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000C0DBF File Offset: 0x000BEFBF
		internal RemoteDataEventArgs(object data)
		{
			this.data = (T)((object)data);
		}

		// Token: 0x04000EE4 RID: 3812
		private T data;
	}
}
