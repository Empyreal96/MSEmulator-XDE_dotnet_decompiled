using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000753 RID: 1875
	internal class CacheDict<TKey, TValue>
	{
		// Token: 0x06004AF0 RID: 19184 RVA: 0x00188D48 File Offset: 0x00186F48
		public CacheDict(int maxSize)
		{
			this._maxSize = maxSize;
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x00188D70 File Offset: 0x00186F70
		public bool TryGetValue(TKey key, out TValue value)
		{
			CacheDict<TKey, TValue>.KeyInfo keyInfo;
			if (this._dict.TryGetValue(key, out keyInfo))
			{
				LinkedListNode<TKey> list = keyInfo.List;
				if (list.Previous != null)
				{
					this._list.Remove(list);
					this._list.AddFirst(list);
				}
				value = keyInfo.Value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x00188DCC File Offset: 0x00186FCC
		public void Add(TKey key, TValue value)
		{
			CacheDict<TKey, TValue>.KeyInfo keyInfo;
			if (this._dict.TryGetValue(key, out keyInfo))
			{
				this._list.Remove(keyInfo.List);
			}
			else if (this._list.Count == this._maxSize)
			{
				LinkedListNode<TKey> last = this._list.Last;
				this._list.RemoveLast();
				this._dict.Remove(last.Value);
			}
			LinkedListNode<TKey> linkedListNode = new LinkedListNode<TKey>(key);
			this._list.AddFirst(linkedListNode);
			this._dict[key] = new CacheDict<TKey, TValue>.KeyInfo(value, linkedListNode);
		}

		// Token: 0x17000FB8 RID: 4024
		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x04002436 RID: 9270
		private readonly Dictionary<TKey, CacheDict<TKey, TValue>.KeyInfo> _dict = new Dictionary<TKey, CacheDict<TKey, TValue>.KeyInfo>();

		// Token: 0x04002437 RID: 9271
		private readonly LinkedList<TKey> _list = new LinkedList<TKey>();

		// Token: 0x04002438 RID: 9272
		private readonly int _maxSize;

		// Token: 0x02000754 RID: 1876
		private struct KeyInfo
		{
			// Token: 0x06004AF5 RID: 19189 RVA: 0x00188E89 File Offset: 0x00187089
			internal KeyInfo(TValue value, LinkedListNode<TKey> list)
			{
				this.Value = value;
				this.List = list;
			}

			// Token: 0x04002439 RID: 9273
			internal readonly TValue Value;

			// Token: 0x0400243A RID: 9274
			internal readonly LinkedListNode<TKey> List;
		}
	}
}
