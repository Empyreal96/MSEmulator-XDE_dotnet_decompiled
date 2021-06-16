using System;
using System.Collections.Generic;

namespace DiscUtils.Internal
{
	// Token: 0x02000072 RID: 114
	internal class ObjectCache<K, V>
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x0000C4C1 File Offset: 0x0000A6C1
		public ObjectCache()
		{
			this._entries = new Dictionary<K, WeakReference>();
			this._recent = new List<KeyValuePair<K, V>>();
		}

		// Token: 0x17000119 RID: 281
		public V this[K key]
		{
			get
			{
				for (int i = 0; i < this._recent.Count; i++)
				{
					KeyValuePair<K, V> keyValuePair = this._recent[i];
					K key2 = keyValuePair.Key;
					if (key2.Equals(key))
					{
						this.MakeMostRecent(i);
						return keyValuePair.Value;
					}
				}
				WeakReference weakReference;
				if (this._entries.TryGetValue(key, out weakReference))
				{
					V v = (V)((object)weakReference.Target);
					if (v != null)
					{
						this.MakeMostRecent(key, v);
					}
					return v;
				}
				return default(V);
			}
			set
			{
				this._entries[key] = new WeakReference(value);
				this.MakeMostRecent(key, value);
				this.PruneEntries();
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		internal void Remove(K key)
		{
			for (int i = 0; i < this._recent.Count; i++)
			{
				K key2 = this._recent[i].Key;
				if (key2.Equals(key))
				{
					this._recent.RemoveAt(i);
					break;
				}
			}
			this._entries.Remove(key);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000C608 File Offset: 0x0000A808
		private void PruneEntries()
		{
			this._nextPruneCount++;
			if (this._nextPruneCount > 500)
			{
				List<K> list = new List<K>();
				foreach (KeyValuePair<K, WeakReference> keyValuePair in this._entries)
				{
					if (!keyValuePair.Value.IsAlive)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (K key in list)
				{
					this._entries.Remove(key);
				}
				this._nextPruneCount = 0;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000C6E0 File Offset: 0x0000A8E0
		private void MakeMostRecent(int i)
		{
			if (i == 0)
			{
				return;
			}
			KeyValuePair<K, V> item = this._recent[i];
			this._recent.RemoveAt(i);
			this._recent.Insert(0, item);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000C717 File Offset: 0x0000A917
		private void MakeMostRecent(K key, V val)
		{
			while (this._recent.Count >= 20)
			{
				this._recent.RemoveAt(this._recent.Count - 1);
			}
			this._recent.Insert(0, new KeyValuePair<K, V>(key, val));
		}

		// Token: 0x04000183 RID: 387
		private const int MostRecentListSize = 20;

		// Token: 0x04000184 RID: 388
		private const int PruneGap = 500;

		// Token: 0x04000185 RID: 389
		private readonly Dictionary<K, WeakReference> _entries;

		// Token: 0x04000186 RID: 390
		private int _nextPruneCount;

		// Token: 0x04000187 RID: 391
		private readonly List<KeyValuePair<K, V>> _recent;
	}
}
