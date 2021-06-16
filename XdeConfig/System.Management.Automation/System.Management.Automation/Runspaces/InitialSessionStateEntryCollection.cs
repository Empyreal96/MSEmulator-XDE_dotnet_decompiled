using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000833 RID: 2099
	public sealed class InitialSessionStateEntryCollection<T> : IEnumerable<!0>, IEnumerable where T : InitialSessionStateEntry
	{
		// Token: 0x0600504E RID: 20558 RVA: 0x001A84DC File Offset: 0x001A66DC
		public InitialSessionStateEntryCollection()
		{
			this._internalCollection = new Collection<T>();
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001A84FC File Offset: 0x001A66FC
		public InitialSessionStateEntryCollection(IEnumerable<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this._internalCollection = new Collection<T>();
			foreach (T item in items)
			{
				this._internalCollection.Add(item);
			}
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x001A8574 File Offset: 0x001A6774
		public InitialSessionStateEntryCollection<T> Clone()
		{
			InitialSessionStateEntryCollection<T> initialSessionStateEntryCollection;
			lock (this._syncObject)
			{
				initialSessionStateEntryCollection = new InitialSessionStateEntryCollection<T>();
				foreach (T t in this._internalCollection)
				{
					initialSessionStateEntryCollection.Add((T)((object)t.Clone()));
				}
			}
			return initialSessionStateEntryCollection;
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001A8608 File Offset: 0x001A6808
		public void Reset()
		{
			lock (this._syncObject)
			{
				this._internalCollection.Clear();
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005052 RID: 20562 RVA: 0x001A8650 File Offset: 0x001A6850
		public int Count
		{
			get
			{
				return this._internalCollection.Count;
			}
		}

		// Token: 0x17001069 RID: 4201
		public T this[int index]
		{
			get
			{
				T result;
				lock (this._syncObject)
				{
					result = this._internalCollection[index];
				}
				return result;
			}
		}

		// Token: 0x1700106A RID: 4202
		public Collection<T> this[string name]
		{
			get
			{
				Collection<T> collection = new Collection<T>();
				lock (this._syncObject)
				{
					foreach (T item in this._internalCollection)
					{
						if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
						{
							collection.Add(item);
						}
					}
				}
				return collection;
			}
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x001A8740 File Offset: 0x001A6940
		internal Collection<T> LookUpByName(string name)
		{
			if (name == null)
			{
				throw new PSArgumentNullException("name");
			}
			Collection<T> collection = new Collection<T>();
			WildcardPattern wildcardPattern = new WildcardPattern(name, WildcardOptions.IgnoreCase);
			lock (this._syncObject)
			{
				foreach (T item in this._internalCollection)
				{
					if (wildcardPattern.IsMatch(item.Name))
					{
						collection.Add(item);
					}
				}
			}
			return collection;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001A87F0 File Offset: 0x001A69F0
		public void RemoveItem(int index)
		{
			lock (this._syncObject)
			{
				this._internalCollection.RemoveAt(index);
			}
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x001A8838 File Offset: 0x001A6A38
		public void RemoveItem(int index, int count)
		{
			lock (this._syncObject)
			{
				while (count-- > 0)
				{
					this._internalCollection.RemoveAt(index);
				}
			}
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x001A888C File Offset: 0x001A6A8C
		public void Clear()
		{
			lock (this._syncObject)
			{
				this._internalCollection.Clear();
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001A88D4 File Offset: 0x001A6AD4
		public void Remove(string name, object type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			lock (this._syncObject)
			{
				Type type2 = null;
				if (type != null)
				{
					type2 = (type as Type);
					if (type2 == null)
					{
						type2 = type.GetType();
					}
				}
				for (int i = this._internalCollection.Count - 1; i >= 0; i--)
				{
					T t = this._internalCollection[i];
					if (t != null && (type2 == null || t.GetType() == type2) && string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase))
					{
						this._internalCollection.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001A89A8 File Offset: 0x001A6BA8
		public void Add(T item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			lock (this._syncObject)
			{
				this._internalCollection.Add(item);
			}
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001A8A04 File Offset: 0x001A6C04
		public void Add(IEnumerable<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			lock (this._syncObject)
			{
				foreach (T item in items)
				{
					this._internalCollection.Add(item);
				}
			}
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001A8A88 File Offset: 0x001A6C88
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._internalCollection.GetEnumerator();
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x001A8A95 File Offset: 0x001A6C95
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this._internalCollection.GetEnumerator();
		}

		// Token: 0x0400290A RID: 10506
		private Collection<T> _internalCollection;

		// Token: 0x0400290B RID: 10507
		private object _syncObject = new object();
	}
}
