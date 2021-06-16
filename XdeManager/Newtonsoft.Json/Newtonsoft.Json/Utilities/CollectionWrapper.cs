using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200003E RID: 62
	internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IWrappedCollection, IList, ICollection
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x00010F18 File Offset: 0x0000F118
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			ICollection<T> genericCollection;
			if ((genericCollection = (list as ICollection<T>)) != null)
			{
				this._genericCollection = genericCollection;
				return;
			}
			this._list = list;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010F4F File Offset: 0x0000F14F
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00010F69 File Offset: 0x0000F169
		public virtual void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00010F92 File Offset: 0x0000F192
		public virtual void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010FB3 File Offset: 0x0000F1B3
		public virtual bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010FDB File Offset: 0x0000F1DB
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00011000 File Offset: 0x0000F200
		public virtual int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00011021 File Offset: 0x0000F221
		public virtual bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00011042 File Offset: 0x0000F242
		public virtual bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00011080 File Offset: 0x0000F280
		public virtual IEnumerator<T> GetEnumerator()
		{
			IEnumerable<T> genericCollection = this._genericCollection;
			return (genericCollection ?? this._list.Cast<T>()).GetEnumerator();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000110AC File Offset: 0x0000F2AC
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable genericCollection = this._genericCollection;
			return (genericCollection ?? this._list).GetEnumerator();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000110D0 File Offset: 0x0000F2D0
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000110EC File Offset: 0x0000F2EC
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011104 File Offset: 0x0000F304
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011139 File Offset: 0x0000F339
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001115A File Offset: 0x0000F35A
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0001118C File Offset: 0x0000F38C
		bool IList.IsFixedSize
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsFixedSize;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000111AD File Offset: 0x0000F3AD
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x170000A6 RID: 166
		object IList.this[int index]
		{
			get
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			set
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)((object)value);
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00011217 File Offset: 0x0000F417
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00011226 File Offset: 0x0000F426
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00011229 File Offset: 0x0000F429
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

		// Token: 0x06000456 RID: 1110 RVA: 0x0001124B File Offset: 0x0000F44B
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001127A File Offset: 0x0000F47A
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType() || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x000112AC File Offset: 0x0000F4AC
		public object UnderlyingCollection
		{
			get
			{
				return this._genericCollection ?? this._list;
			}
		}

		// Token: 0x04000153 RID: 339
		private readonly IList _list;

		// Token: 0x04000154 RID: 340
		private readonly ICollection<T> _genericCollection;

		// Token: 0x04000155 RID: 341
		private object _syncRoot;
	}
}
