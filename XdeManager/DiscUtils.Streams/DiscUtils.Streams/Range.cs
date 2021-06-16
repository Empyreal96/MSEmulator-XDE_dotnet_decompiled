using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000035 RID: 53
	public class Range<TOffset, TCount> : IEquatable<Range<TOffset, TCount>> where TOffset : IEquatable<TOffset> where TCount : IEquatable<TCount>
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00006F74 File Offset: 0x00005174
		public Range(TOffset offset, TCount count)
		{
			this.Offset = offset;
			this.Count = count;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00006F8A File Offset: 0x0000518A
		public TCount Count { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00006F92 File Offset: 0x00005192
		public TOffset Offset { get; }

		// Token: 0x060001F7 RID: 503 RVA: 0x00006F9C File Offset: 0x0000519C
		public bool Equals(Range<TOffset, TCount> other)
		{
			if (other == null)
			{
				return false;
			}
			TOffset offset = this.Offset;
			if (offset.Equals(other.Offset))
			{
				TCount count = this.Count;
				return count.Equals(other.Count);
			}
			return false;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006FE6 File Offset: 0x000051E6
		public static IEnumerable<Range<T, T>> Chunked<T>(IEnumerable<Range<T, T>> ranges, T chunkSize) where T : struct, IEquatable<T>, IComparable<T>
		{
			T? t = new T?(Numbers<T>.Zero);
			T t2 = Numbers<T>.Zero;
			foreach (Range<T, T> range in ranges)
			{
				if (Numbers<T>.NotEqual(range.Count, Numbers<T>.Zero))
				{
					T rangeStart = Numbers<T>.RoundDown(range.Offset, chunkSize);
					T rangeNext = Numbers<T>.RoundUp(Numbers<T>.Add(range.Offset, range.Count), chunkSize);
					if (t != null && Numbers<T>.GreaterThan(rangeStart, Numbers<T>.Add(t.Value, t2)))
					{
						yield return new Range<T, T>(t.Value, t2);
						t = new T?(rangeStart);
					}
					else if (t == null)
					{
						t = new T?(rangeStart);
					}
					t2 = Numbers<T>.Subtract(rangeNext, t.Value);
					rangeStart = default(T);
					rangeNext = default(T);
				}
			}
			IEnumerator<Range<T, T>> enumerator = null;
			if (t != null)
			{
				yield return new Range<T, T>(t.Value, t2);
			}
			yield break;
			yield break;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007000 File Offset: 0x00005200
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.Offset,
				":+",
				this.Count,
				"]"
			});
		}
	}
}
