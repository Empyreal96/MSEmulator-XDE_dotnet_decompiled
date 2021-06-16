using System;
using System.Collections.Generic;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x0200044C RID: 1100
	internal class CimClassDeserializationCache<TKey>
	{
		// Token: 0x06002FE9 RID: 12265 RVA: 0x001057EF File Offset: 0x001039EF
		internal void AddCimClassToCache(TKey key, CimClass cimClass)
		{
			if (this._cimClassIdToClass.Count >= DeserializationContext.MaxItemsInCimClassCache)
			{
				this._cimClassIdToClass.Clear();
			}
			this._cimClassIdToClass.Add(key, cimClass);
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x0010581C File Offset: 0x00103A1C
		internal CimClass GetCimClassFromCache(TKey key)
		{
			CimClass result;
			if (this._cimClassIdToClass.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x040019F3 RID: 6643
		private readonly Dictionary<TKey, CimClass> _cimClassIdToClass = new Dictionary<TKey, CimClass>();
	}
}
