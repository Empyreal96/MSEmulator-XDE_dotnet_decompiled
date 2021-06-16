using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200007E RID: 126
	[DebuggerDisplay("Count={Count}")]
	internal readonly struct ImmutableArray<T>
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0000D999 File Offset: 0x0000BB99
		public static ImmutableArray<T> Empty
		{
			get
			{
				return ImmutableArray<T>.s_empty;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		private ImmutableArray(T[] elements)
		{
			this._array = elements;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000D9AC File Offset: 0x0000BBAC
		public ImmutableArray<T> Add(T item)
		{
			T[] array = new T[this._array.Length + 1];
			Array.Copy(this._array, 0, array, 0, this._array.Length);
			array[array.Length - 1] = item;
			return new ImmutableArray<T>(array);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		public ImmutableArray<T> Remove(T item)
		{
			int num = Array.IndexOf<T>(this._array, item);
			if (num < 0)
			{
				return this;
			}
			if (this._array.Length == 1)
			{
				return ImmutableArray<T>.Empty;
			}
			T[] array = new T[this._array.Length - 1];
			Array.Copy(this._array, 0, array, 0, num);
			Array.Copy(this._array, num + 1, array, num, this._array.Length - num - 1);
			return new ImmutableArray<T>(array);
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000DA6B File Offset: 0x0000BC6B
		public int Count
		{
			get
			{
				return this._array.Length;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000DA75 File Offset: 0x0000BC75
		public bool Contains(T item)
		{
			return Array.IndexOf<T>(this._array, item) >= 0;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000DA89 File Offset: 0x0000BC89
		public IEnumerator<T> GetEnumerator()
		{
			return this._array.GetEnumerator();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000DA96 File Offset: 0x0000BC96
		public T[] ToArray()
		{
			if (this._array.Length != 0)
			{
				return (T[])this._array.Clone();
			}
			return ImmutableArray<T>.s_empty._array;
		}

		// Token: 0x04000189 RID: 393
		private static readonly ImmutableArray<T> s_empty = new ImmutableArray<T>(new T[0]);

		// Token: 0x0400018A RID: 394
		private readonly T[] _array;
	}
}
