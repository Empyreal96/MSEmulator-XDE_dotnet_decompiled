using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200044D RID: 1101
	internal class CimClassSerializationCache<TKey>
	{
		// Token: 0x06002FEC RID: 12268 RVA: 0x0010584F File Offset: 0x00103A4F
		internal bool DoesDeserializerAlreadyHaveCimClass(TKey key)
		{
			return this._cimClassesHeldByDeserializer.Contains(key);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0010585D File Offset: 0x00103A5D
		internal void AddClassToCache(TKey key)
		{
			if (this._cimClassesHeldByDeserializer.Count >= DeserializationContext.MaxItemsInCimClassCache)
			{
				this._cimClassesHeldByDeserializer.Clear();
			}
			this._cimClassesHeldByDeserializer.Add(key);
		}

		// Token: 0x040019F4 RID: 6644
		private readonly HashSet<TKey> _cimClassesHeldByDeserializer = new HashSet<TKey>(EqualityComparer<TKey>.Default);
	}
}
