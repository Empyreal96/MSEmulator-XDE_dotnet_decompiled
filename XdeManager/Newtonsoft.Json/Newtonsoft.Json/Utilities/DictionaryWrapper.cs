using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000047 RID: 71
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IWrappedDictionary, IDictionary, ICollection
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00013B49 File Offset: 0x00011D49
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00013B63 File Offset: 0x00011D63
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00013B7D File Offset: 0x00011D7D
		public DictionaryWrapper(IReadOnlyDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._readOnlyDictionary = dictionary;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00013B97 File Offset: 0x00011D97
		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00013BD4 File Offset: 0x00011DD4
		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey(key);
			}
			return this._genericDictionary.ContainsKey(key);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00013C14 File Offset: 0x00011E14
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this._genericDictionary.Keys;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00013C64 File Offset: 0x00011E64
		public bool Remove(TKey key)
		{
			if (this._dictionary != null)
			{
				if (this._dictionary.Contains(key))
				{
					this._dictionary.Remove(key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(key);
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00013CBC File Offset: 0x00011EBC
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(key))
				{
					value = default(TValue);
					return false;
				}
				value = (TValue)((object)this._dictionary[key]);
				return true;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.TryGetValue(key, out value);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00013D28 File Offset: 0x00011F28
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this._genericDictionary.Values;
			}
		}

		// Token: 0x170000AF RID: 175
		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)((object)this._dictionary[key]);
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[key];
				}
				return this._genericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[key] = value;
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00013E04 File Offset: 0x00012004
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			IDictionary<TKey, TValue> genericDictionary = this._genericDictionary;
			if (genericDictionary == null)
			{
				return;
			}
			genericDictionary.Add(item);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00013E50 File Offset: 0x00012050
		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Clear();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00013E80 File Offset: 0x00012080
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.Contains(item);
			}
			return this._genericDictionary.Contains(item);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00013ED0 File Offset: 0x000120D0
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary != null)
			{
				using (IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry entry = enumerator.Entry;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)entry.Key), (TValue)((object)entry.Value));
					}
					return;
				}
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00013F6C File Offset: 0x0001216C
		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Count;
				}
				return this._genericDictionary.Count;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00013FA1 File Offset: 0x000121A1
		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				return this._readOnlyDictionary != null || this._genericDictionary.IsReadOnly;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00013FCC File Offset: 0x000121CC
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(item.Key))
				{
					return true;
				}
				if (object.Equals(this._dictionary[item.Key], item.Value))
				{
					this._dictionary.Remove(item.Key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(item);
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001405C File Offset: 0x0001225C
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return (from DictionaryEntry de in this._dictionary
				select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.GetEnumerator();
			}
			return this._genericDictionary.GetEnumerator();
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000140C5 File Offset: 0x000122C5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000140CD File Offset: 0x000122CD
		void IDictionary.Add(object key, object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x170000B2 RID: 178
		object IDictionary.this[object key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[(TKey)((object)key)];
				}
				return this._genericDictionary[(TKey)((object)key)];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000141A0 File Offset: 0x000123A0
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._readOnlyDictionary.GetEnumerator());
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000141F4 File Offset: 0x000123F4
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00014241 File Offset: 0x00012441
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && (this._readOnlyDictionary != null || this._dictionary.IsFixedSize);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00014262 File Offset: 0x00012462
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000142A1 File Offset: 0x000124A1
		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Remove((TKey)((object)key));
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x000142D8 File Offset: 0x000124D8
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00014317 File Offset: 0x00012517
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001434F File Offset: 0x0001254F
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._dictionary != null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00014366 File Offset: 0x00012566
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00014388 File Offset: 0x00012588
		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary;
				}
				return this._genericDictionary;
			}
		}

		// Token: 0x040001B4 RID: 436
		private readonly IDictionary _dictionary;

		// Token: 0x040001B5 RID: 437
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x040001B6 RID: 438
		private readonly IReadOnlyDictionary<TKey, TValue> _readOnlyDictionary;

		// Token: 0x040001B7 RID: 439
		private object _syncRoot;

		// Token: 0x0200015D RID: 349
		private readonly struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000E7E RID: 3710 RVA: 0x0004162A File Offset: 0x0003F82A
			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0004163E File Offset: 0x0003F83E
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0004164C File Offset: 0x0003F84C
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00041668 File Offset: 0x0003F868
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00041684 File Offset: 0x0003F884
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object key = keyValuePair.Key;
					keyValuePair = this._e.Current;
					return new DictionaryEntry(key, keyValuePair.Value);
				}
			}

			// Token: 0x06000E83 RID: 3715 RVA: 0x000416CB File Offset: 0x0003F8CB
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x06000E84 RID: 3716 RVA: 0x000416D8 File Offset: 0x0003F8D8
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x04000675 RID: 1653
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
