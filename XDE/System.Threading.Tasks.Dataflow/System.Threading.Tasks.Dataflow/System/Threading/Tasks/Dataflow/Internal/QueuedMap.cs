using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000080 RID: 128
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(EnumerableDebugView<, >))]
	internal sealed class QueuedMap<TKey, TValue>
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0000DACE File Offset: 0x0000BCCE
		internal QueuedMap()
		{
			this._queue = new QueuedMap<TKey, TValue>.ArrayBasedLinkedQueue<KeyValuePair<TKey, TValue>>();
			this._mapKeyToIndex = new Dictionary<TKey, int>();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		internal QueuedMap(int capacity)
		{
			this._queue = new QueuedMap<TKey, TValue>.ArrayBasedLinkedQueue<KeyValuePair<TKey, TValue>>(capacity);
			this._mapKeyToIndex = new Dictionary<TKey, int>(capacity);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000DB0C File Offset: 0x0000BD0C
		internal void Push(TKey key, TValue value)
		{
			int num;
			if (!this._queue.IsEmpty && this._mapKeyToIndex.TryGetValue(key, out num))
			{
				this._queue.Replace(num, new KeyValuePair<TKey, TValue>(key, value));
				return;
			}
			num = this._queue.Enqueue(new KeyValuePair<TKey, TValue>(key, value));
			this._mapKeyToIndex.Add(key, num);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		internal bool TryPop(out KeyValuePair<TKey, TValue> item)
		{
			bool flag = this._queue.TryDequeue(out item);
			if (flag)
			{
				this._mapKeyToIndex.Remove(item.Key);
			}
			return flag;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000DB9C File Offset: 0x0000BD9C
		internal int PopRange(KeyValuePair<TKey, TValue>[] items, int arrayOffset, int count)
		{
			int num = 0;
			int num2 = arrayOffset;
			KeyValuePair<TKey, TValue> keyValuePair;
			while (num < count && this.TryPop(out keyValuePair))
			{
				items[num2] = keyValuePair;
				num2++;
				num++;
			}
			return num;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000DBCE File Offset: 0x0000BDCE
		internal int Count
		{
			get
			{
				return this._mapKeyToIndex.Count;
			}
		}

		// Token: 0x0400018C RID: 396
		private readonly QueuedMap<TKey, TValue>.ArrayBasedLinkedQueue<KeyValuePair<TKey, TValue>> _queue;

		// Token: 0x0400018D RID: 397
		private readonly Dictionary<TKey, int> _mapKeyToIndex;

		// Token: 0x02000081 RID: 129
		private sealed class ArrayBasedLinkedQueue<T>
		{
			// Token: 0x060003F4 RID: 1012 RVA: 0x0000DBDB File Offset: 0x0000BDDB
			internal ArrayBasedLinkedQueue()
			{
				this._storage = new List<KeyValuePair<int, T>>();
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x0000DC03 File Offset: 0x0000BE03
			internal ArrayBasedLinkedQueue(int capacity)
			{
				this._storage = new List<KeyValuePair<int, T>>(capacity);
			}

			// Token: 0x060003F6 RID: 1014 RVA: 0x0000DC2C File Offset: 0x0000BE2C
			internal int Enqueue(T item)
			{
				int num;
				if (this._freeIndex != -1)
				{
					num = this._freeIndex;
					this._freeIndex = this._storage[this._freeIndex].Key;
					this._storage[num] = new KeyValuePair<int, T>(-1, item);
				}
				else
				{
					num = this._storage.Count;
					this._storage.Add(new KeyValuePair<int, T>(-1, item));
				}
				if (this._headIndex == -1)
				{
					this._headIndex = num;
				}
				else
				{
					this._storage[this._tailIndex] = new KeyValuePair<int, T>(num, this._storage[this._tailIndex].Value);
				}
				this._tailIndex = num;
				return num;
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
			internal bool TryDequeue(out T item)
			{
				if (this._headIndex == -1)
				{
					item = default(T);
					return false;
				}
				item = this._storage[this._headIndex].Value;
				int key = this._storage[this._headIndex].Key;
				this._storage[this._headIndex] = new KeyValuePair<int, T>(this._freeIndex, default(T));
				this._freeIndex = this._headIndex;
				this._headIndex = key;
				if (this._headIndex == -1)
				{
					this._tailIndex = -1;
				}
				return true;
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x0000DD8C File Offset: 0x0000BF8C
			internal void Replace(int index, T item)
			{
				this._storage[index] = new KeyValuePair<int, T>(this._storage[index].Key, item);
			}

			// Token: 0x17000150 RID: 336
			// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000DDBF File Offset: 0x0000BFBF
			internal bool IsEmpty
			{
				get
				{
					return this._headIndex == -1;
				}
			}

			// Token: 0x0400018E RID: 398
			private readonly List<KeyValuePair<int, T>> _storage;

			// Token: 0x0400018F RID: 399
			private int _headIndex = -1;

			// Token: 0x04000190 RID: 400
			private int _tailIndex = -1;

			// Token: 0x04000191 RID: 401
			private int _freeIndex = -1;
		}
	}
}
