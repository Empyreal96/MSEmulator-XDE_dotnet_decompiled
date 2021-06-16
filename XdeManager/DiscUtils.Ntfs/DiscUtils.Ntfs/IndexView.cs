using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000030 RID: 48
	internal class IndexView<K, D> where K : IByteArraySerializable, new() where D : IByteArraySerializable, new()
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000A193 File Offset: 0x00008393
		public IndexView(Index index)
		{
			this._index = index;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000A1A2 File Offset: 0x000083A2
		public int Count
		{
			get
			{
				return this._index.Count;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000A1AF File Offset: 0x000083AF
		public IEnumerable<KeyValuePair<K, D>> Entries
		{
			get
			{
				foreach (KeyValuePair<byte[], byte[]> keyValuePair in this._index.Entries)
				{
					yield return new KeyValuePair<K, D>(IndexView<K, D>.Convert<K>(keyValuePair.Key), IndexView<K, D>.Convert<D>(keyValuePair.Value));
				}
				IEnumerator<KeyValuePair<byte[], byte[]>> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000070 RID: 112
		public D this[K key]
		{
			get
			{
				return IndexView<K, D>.Convert<D>(this._index[IndexView<K, D>.Unconvert<K>(key)]);
			}
			set
			{
				this._index[IndexView<K, D>.Unconvert<K>(key)] = IndexView<K, D>.Unconvert<D>(value);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A1F0 File Offset: 0x000083F0
		public IEnumerable<KeyValuePair<K, D>> FindAll(IComparable<byte[]> query)
		{
			foreach (KeyValuePair<byte[], byte[]> keyValuePair in this._index.FindAll(query))
			{
				yield return new KeyValuePair<K, D>(IndexView<K, D>.Convert<K>(keyValuePair.Key), IndexView<K, D>.Convert<D>(keyValuePair.Value));
			}
			IEnumerator<KeyValuePair<byte[], byte[]>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A208 File Offset: 0x00008408
		public KeyValuePair<K, D> FindFirst(IComparable<byte[]> query)
		{
			using (IEnumerator<KeyValuePair<K, D>> enumerator = this.FindAll(query).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(KeyValuePair<K, D>);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A25C File Offset: 0x0000845C
		public IEnumerable<KeyValuePair<K, D>> FindAll(IComparable<K> query)
		{
			foreach (KeyValuePair<byte[], byte[]> keyValuePair in this._index.FindAll(new IndexView<K, D>.ComparableConverter(query)))
			{
				yield return new KeyValuePair<K, D>(IndexView<K, D>.Convert<K>(keyValuePair.Key), IndexView<K, D>.Convert<D>(keyValuePair.Value));
			}
			IEnumerator<KeyValuePair<byte[], byte[]>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A274 File Offset: 0x00008474
		public KeyValuePair<K, D> FindFirst(IComparable<K> query)
		{
			using (IEnumerator<KeyValuePair<K, D>> enumerator = this.FindAll(query).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(KeyValuePair<K, D>);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public bool TryGetValue(K key, out D data)
		{
			byte[] data2;
			if (this._index.TryGetValue(IndexView<K, D>.Unconvert<K>(key), out data2))
			{
				data = IndexView<K, D>.Convert<D>(data2);
				return true;
			}
			data = default(D);
			return false;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A300 File Offset: 0x00008500
		public bool ContainsKey(K key)
		{
			return this._index.ContainsKey(IndexView<K, D>.Unconvert<K>(key));
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A313 File Offset: 0x00008513
		public void Remove(K key)
		{
			this._index.Remove(IndexView<K, D>.Unconvert<K>(key));
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A328 File Offset: 0x00008528
		private static T Convert<T>(byte[] data) where T : IByteArraySerializable, new()
		{
			T result = Activator.CreateInstance<T>();
			result.ReadFrom(data, 0);
			return result;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A34C File Offset: 0x0000854C
		private static byte[] Unconvert<T>(T value) where T : IByteArraySerializable, new()
		{
			byte[] array = new byte[value.Size];
			value.WriteTo(array, 0);
			return array;
		}

		// Token: 0x040000E6 RID: 230
		private readonly Index _index;

		// Token: 0x02000077 RID: 119
		private class ComparableConverter : IComparable<byte[]>
		{
			// Token: 0x06000478 RID: 1144 RVA: 0x00016B55 File Offset: 0x00014D55
			public ComparableConverter(IComparable<K> toWrap)
			{
				this._wrapped = toWrap;
			}

			// Token: 0x06000479 RID: 1145 RVA: 0x00016B64 File Offset: 0x00014D64
			public int CompareTo(byte[] other)
			{
				return this._wrapped.CompareTo(IndexView<K, D>.Convert<K>(other));
			}

			// Token: 0x04000229 RID: 553
			private readonly IComparable<K> _wrapped;
		}
	}
}
