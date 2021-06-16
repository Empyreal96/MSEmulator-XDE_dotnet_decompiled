using System;
using System.Collections.Concurrent;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000067 RID: 103
	internal class ThreadSafeStore<TKey, TValue>
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x00019929 File Offset: 0x00017B29
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			this._creator = creator;
			this._concurrentStore = new ConcurrentDictionary<TKey, TValue>();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001994E File Offset: 0x00017B4E
		public TValue Get(TKey key)
		{
			return this._concurrentStore.GetOrAdd(key, this._creator);
		}

		// Token: 0x04000208 RID: 520
		private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;

		// Token: 0x04000209 RID: 521
		private readonly Func<TKey, TValue> _creator;
	}
}
