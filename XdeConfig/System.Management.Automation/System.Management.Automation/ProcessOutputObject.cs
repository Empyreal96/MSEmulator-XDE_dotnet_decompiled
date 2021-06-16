using System;

namespace System.Management.Automation
{
	// Token: 0x02000093 RID: 147
	internal class ProcessOutputObject
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00022E34 File Offset: 0x00021034
		internal object Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00022E3C File Offset: 0x0002103C
		internal MinishellStream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00022E44 File Offset: 0x00021044
		internal ProcessOutputObject(object data, MinishellStream stream)
		{
			this.data = data;
			this.stream = stream;
		}

		// Token: 0x04000329 RID: 809
		private object data;

		// Token: 0x0400032A RID: 810
		private MinishellStream stream;
	}
}
