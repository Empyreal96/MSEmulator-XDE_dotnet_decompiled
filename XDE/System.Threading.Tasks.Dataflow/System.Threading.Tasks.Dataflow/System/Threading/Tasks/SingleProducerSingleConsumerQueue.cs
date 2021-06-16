using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Internal;

namespace System.Threading.Tasks
{
	// Token: 0x02000009 RID: 9
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(SingleProducerSingleConsumerQueue<>.SingleProducerSingleConsumerQueue_DebugView))]
	internal sealed class SingleProducerSingleConsumerQueue<T> : IProducerConsumerQueue<!0>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000021A0 File Offset: 0x000003A0
		internal SingleProducerSingleConsumerQueue()
		{
			this._head = (this._tail = new SingleProducerSingleConsumerQueue<T>.Segment(32));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000021D0 File Offset: 0x000003D0
		public void Enqueue(T item)
		{
			SingleProducerSingleConsumerQueue<T>.Segment tail = this._tail;
			T[] array = tail._array;
			int last = tail._state._last;
			int num = last + 1 & array.Length - 1;
			if (num != tail._state._firstCopy)
			{
				array[last] = item;
				tail._state._last = num;
				return;
			}
			this.EnqueueSlow(item, ref tail);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002234 File Offset: 0x00000434
		private void EnqueueSlow(T item, ref SingleProducerSingleConsumerQueue<T>.Segment segment)
		{
			if (segment._state._firstCopy != segment._state._first)
			{
				segment._state._firstCopy = segment._state._first;
				this.Enqueue(item);
				return;
			}
			int num = this._tail._array.Length << 1;
			if (num > 16777216)
			{
				num = 16777216;
			}
			SingleProducerSingleConsumerQueue<T>.Segment segment2 = new SingleProducerSingleConsumerQueue<T>.Segment(num);
			segment2._array[0] = item;
			segment2._state._last = 1;
			segment2._state._lastCopy = 1;
			try
			{
			}
			finally
			{
				Volatile.Write<SingleProducerSingleConsumerQueue<T>.Segment>(ref this._tail._next, segment2);
				this._tail = segment2;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000022FC File Offset: 0x000004FC
		public bool TryDequeue(out T result)
		{
			SingleProducerSingleConsumerQueue<T>.Segment head = this._head;
			T[] array = head._array;
			int first = head._state._first;
			if (first != head._state._lastCopy)
			{
				result = array[first];
				array[first] = default(T);
				head._state._first = (first + 1 & array.Length - 1);
				return true;
			}
			return this.TryDequeueSlow(ref head, ref array, out result);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002378 File Offset: 0x00000578
		private bool TryDequeueSlow(ref SingleProducerSingleConsumerQueue<T>.Segment segment, ref T[] array, out T result)
		{
			if (segment._state._last != segment._state._lastCopy)
			{
				segment._state._lastCopy = segment._state._last;
				return this.TryDequeue(out result);
			}
			if (segment._next != null && segment._state._first == segment._state._last)
			{
				segment = segment._next;
				array = segment._array;
				this._head = segment;
			}
			int first = segment._state._first;
			if (first == segment._state._last)
			{
				result = default(T);
				return false;
			}
			result = array[first];
			array[first] = default(T);
			segment._state._first = (first + 1 & segment._array.Length - 1);
			segment._state._lastCopy = segment._state._last;
			return true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002488 File Offset: 0x00000688
		public bool TryPeek(out T result)
		{
			SingleProducerSingleConsumerQueue<T>.Segment head = this._head;
			T[] array = head._array;
			int first = head._state._first;
			if (first != head._state._lastCopy)
			{
				result = array[first];
				return true;
			}
			return this.TryPeekSlow(ref head, ref array, out result);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024DC File Offset: 0x000006DC
		private bool TryPeekSlow(ref SingleProducerSingleConsumerQueue<T>.Segment segment, ref T[] array, out T result)
		{
			if (segment._state._last != segment._state._lastCopy)
			{
				segment._state._lastCopy = segment._state._last;
				return this.TryPeek(out result);
			}
			if (segment._next != null && segment._state._first == segment._state._last)
			{
				segment = segment._next;
				array = segment._array;
				this._head = segment;
			}
			int first = segment._state._first;
			if (first == segment._state._last)
			{
				result = default(T);
				return false;
			}
			result = array[first];
			return true;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025A4 File Offset: 0x000007A4
		public bool TryDequeueIf(Predicate<T> predicate, out T result)
		{
			SingleProducerSingleConsumerQueue<T>.Segment head = this._head;
			T[] array = head._array;
			int first = head._state._first;
			if (first == head._state._lastCopy)
			{
				return this.TryDequeueIfSlow(predicate, ref head, ref array, out result);
			}
			result = array[first];
			if (predicate == null || predicate(result))
			{
				array[first] = default(T);
				head._state._first = (first + 1 & array.Length - 1);
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002638 File Offset: 0x00000838
		private bool TryDequeueIfSlow(Predicate<T> predicate, ref SingleProducerSingleConsumerQueue<T>.Segment segment, ref T[] array, out T result)
		{
			if (segment._state._last != segment._state._lastCopy)
			{
				segment._state._lastCopy = segment._state._last;
				return this.TryDequeueIf(predicate, out result);
			}
			if (segment._next != null && segment._state._first == segment._state._last)
			{
				segment = segment._next;
				array = segment._array;
				this._head = segment;
			}
			int first = segment._state._first;
			if (first == segment._state._last)
			{
				result = default(T);
				return false;
			}
			result = array[first];
			if (predicate == null || predicate(result))
			{
				array[first] = default(T);
				segment._state._first = (first + 1 & segment._array.Length - 1);
				segment._state._lastCopy = segment._state._last;
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002768 File Offset: 0x00000968
		public void Clear()
		{
			T t;
			while (this.TryDequeue(out t))
			{
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002780 File Offset: 0x00000980
		public bool IsEmpty
		{
			get
			{
				SingleProducerSingleConsumerQueue<T>.Segment head = this._head;
				return head._state._first == head._state._lastCopy && head._state._first == head._state._last && head._next == null;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000027D9 File Offset: 0x000009D9
		public IEnumerator<T> GetEnumerator()
		{
			SingleProducerSingleConsumerQueue<T>.Segment segment;
			for (segment = this._head; segment != null; segment = segment._next)
			{
				for (int pt = segment._state._first; pt != segment._state._last; pt = (pt + 1 & segment._array.Length - 1))
				{
					yield return segment._array[pt];
				}
			}
			segment = null;
			yield break;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027E8 File Offset: 0x000009E8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000027F0 File Offset: 0x000009F0
		public int Count
		{
			get
			{
				int num = 0;
				for (SingleProducerSingleConsumerQueue<T>.Segment segment = this._head; segment != null; segment = segment._next)
				{
					int num2 = segment._array.Length;
					int first;
					int last;
					do
					{
						first = segment._state._first;
						last = segment._state._last;
					}
					while (first != segment._state._first);
					num += (last - first & num2 - 1);
				}
				return num;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002858 File Offset: 0x00000A58
		int IProducerConsumerQueue<!0>.GetCountSafe(object syncObj)
		{
			int count;
			lock (syncObj)
			{
				count = this.Count;
			}
			return count;
		}

		// Token: 0x04000002 RID: 2
		private volatile SingleProducerSingleConsumerQueue<T>.Segment _head;

		// Token: 0x04000003 RID: 3
		private volatile SingleProducerSingleConsumerQueue<T>.Segment _tail;

		// Token: 0x0200000A RID: 10
		[StructLayout(LayoutKind.Sequential)]
		private sealed class Segment
		{
			// Token: 0x0600002D RID: 45 RVA: 0x00002898 File Offset: 0x00000A98
			internal Segment(int size)
			{
				this._array = new T[size];
			}

			// Token: 0x04000004 RID: 4
			internal SingleProducerSingleConsumerQueue<T>.Segment _next;

			// Token: 0x04000005 RID: 5
			internal readonly T[] _array;

			// Token: 0x04000006 RID: 6
			internal SingleProducerSingleConsumerQueue<T>.SegmentState _state;
		}

		// Token: 0x0200000B RID: 11
		private struct SegmentState
		{
			// Token: 0x04000007 RID: 7
			internal PaddingFor32 _pad0;

			// Token: 0x04000008 RID: 8
			internal volatile int _first;

			// Token: 0x04000009 RID: 9
			internal int _lastCopy;

			// Token: 0x0400000A RID: 10
			internal PaddingFor32 _pad1;

			// Token: 0x0400000B RID: 11
			internal int _firstCopy;

			// Token: 0x0400000C RID: 12
			internal volatile int _last;

			// Token: 0x0400000D RID: 13
			internal PaddingFor32 _pad2;
		}

		// Token: 0x0200000C RID: 12
		private sealed class SingleProducerSingleConsumerQueue_DebugView
		{
			// Token: 0x0600002E RID: 46 RVA: 0x000028AC File Offset: 0x00000AAC
			public SingleProducerSingleConsumerQueue_DebugView(SingleProducerSingleConsumerQueue<T> queue)
			{
				this._queue = queue;
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600002F RID: 47 RVA: 0x000028BC File Offset: 0x00000ABC
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T[] Items
			{
				get
				{
					List<T> list = new List<T>();
					foreach (T item in this._queue)
					{
						list.Add(item);
					}
					return list.ToArray();
				}
			}

			// Token: 0x0400000E RID: 14
			private readonly SingleProducerSingleConsumerQueue<T> _queue;
		}
	}
}
