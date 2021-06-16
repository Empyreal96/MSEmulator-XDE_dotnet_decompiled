using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
	// Token: 0x02000007 RID: 7
	internal interface IProducerConsumerQueue<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000013 RID: 19
		void Enqueue(T item);

		// Token: 0x06000014 RID: 20
		bool TryDequeue(out T result);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000015 RID: 21
		bool IsEmpty { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000016 RID: 22
		int Count { get; }

		// Token: 0x06000017 RID: 23
		int GetCountSafe(object syncObj);
	}
}
