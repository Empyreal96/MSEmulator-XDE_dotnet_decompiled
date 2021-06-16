using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200000D RID: 13
	public interface IArrayPool<T>
	{
		// Token: 0x0600000A RID: 10
		T[] Rent(int minimumLength);

		// Token: 0x0600000B RID: 11
		void Return(T[] array);
	}
}
