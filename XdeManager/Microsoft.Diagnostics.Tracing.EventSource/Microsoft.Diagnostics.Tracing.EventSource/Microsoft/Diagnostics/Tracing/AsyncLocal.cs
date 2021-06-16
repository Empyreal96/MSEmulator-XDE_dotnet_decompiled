using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000011 RID: 17
	internal sealed class AsyncLocal<T>
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00008B3A File Offset: 0x00006D3A
		public AsyncLocal(Action<AsyncLocalValueChangedArgs<T>> valueChangedHandler)
		{
			throw new NotImplementedException("AsyncLocal only available on V4.6 and above");
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00008B4C File Offset: 0x00006D4C
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00008B62 File Offset: 0x00006D62
		public T Value
		{
			get
			{
				return default(T);
			}
			set
			{
			}
		}
	}
}
