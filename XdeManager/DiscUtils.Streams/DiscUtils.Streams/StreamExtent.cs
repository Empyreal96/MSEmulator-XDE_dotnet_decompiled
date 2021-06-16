using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x0200002B RID: 43
	public sealed class StreamExtent : IEquatable<StreamExtent>, IComparable<StreamExtent>
	{
		// Token: 0x06000158 RID: 344 RVA: 0x00004F3A File Offset: 0x0000313A
		public StreamExtent(long start, long length)
		{
			this.Start = start;
			this.Length = length;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00004F50 File Offset: 0x00003150
		public long Length { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00004F58 File Offset: 0x00003158
		public long Start { get; }

		// Token: 0x0600015B RID: 347 RVA: 0x00004F60 File Offset: 0x00003160
		public int CompareTo(StreamExtent other)
		{
			if (this.Start > other.Start)
			{
				return 1;
			}
			if (this.Start == other.Start)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004F83 File Offset: 0x00003183
		public bool Equals(StreamExtent other)
		{
			return !(other == null) && this.Start == other.Start && this.Length == other.Length;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004FB0 File Offset: 0x000031B0
		public static IEnumerable<StreamExtent> Union(IEnumerable<StreamExtent> extents, StreamExtent other)
		{
			return StreamExtent.Union(new IEnumerable<StreamExtent>[]
			{
				extents,
				new List<StreamExtent>
				{
					other
				}
			});
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004FDD File Offset: 0x000031DD
		public static IEnumerable<StreamExtent> Union(params IEnumerable<StreamExtent>[] streams)
		{
			long num = long.MaxValue;
			long extentEnd = 0L;
			IEnumerator<StreamExtent>[] enums = new IEnumerator<StreamExtent>[streams.Length];
			bool[] streamsValid = new bool[streams.Length];
			int i = 0;
			for (int j = 0; j < streams.Length; j++)
			{
				enums[j] = streams[j].GetEnumerator();
				streamsValid[j] = enums[j].MoveNext();
				if (streamsValid[j])
				{
					i++;
					if (enums[j].Current.Start < num)
					{
						num = enums[j].Current.Start;
						extentEnd = enums[j].Current.Start + enums[j].Current.Length;
					}
				}
			}
			while (i > 0)
			{
				bool flag;
				do
				{
					flag = false;
					i = 0;
					for (int k = 0; k < streams.Length; k++)
					{
						while (streamsValid[k] && enums[k].Current.Start + enums[k].Current.Length <= extentEnd)
						{
							streamsValid[k] = enums[k].MoveNext();
						}
						if (streamsValid[k])
						{
							i++;
						}
						if (streamsValid[k] && enums[k].Current.Start <= extentEnd)
						{
							extentEnd = enums[k].Current.Start + enums[k].Current.Length;
							flag = true;
							streamsValid[k] = enums[k].MoveNext();
						}
					}
				}
				while (flag && i > 0);
				yield return new StreamExtent(num, extentEnd - num);
				num = long.MaxValue;
				i = 0;
				for (int l = 0; l < streams.Length; l++)
				{
					if (streamsValid[l])
					{
						i++;
						if (enums[l].Current.Start < num)
						{
							num = enums[l].Current.Start;
							extentEnd = enums[l].Current.Start + enums[l].Current.Length;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004FF0 File Offset: 0x000031F0
		public static IEnumerable<StreamExtent> Intersect(IEnumerable<StreamExtent> extents, StreamExtent other)
		{
			return StreamExtent.Intersect(new IEnumerable<StreamExtent>[]
			{
				extents,
				new List<StreamExtent>(1)
				{
					other
				}
			});
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000501E File Offset: 0x0000321E
		public static IEnumerable<StreamExtent> Intersect(params IEnumerable<StreamExtent>[] streams)
		{
			long num = long.MinValue;
			long extentEnd = long.MaxValue;
			IEnumerator<StreamExtent>[] enums = new IEnumerator<StreamExtent>[streams.Length];
			for (int j = 0; j < streams.Length; j++)
			{
				enums[j] = streams[j].GetEnumerator();
				if (!enums[j].MoveNext())
				{
					yield break;
				}
			}
			int num2 = 0;
			for (;;)
			{
				int num3;
				for (int i = 0; i < streams.Length; i = num3)
				{
					while (enums[i].Current.Length == 0L || enums[i].Current.Start + enums[i].Current.Length <= num)
					{
						if (!enums[i].MoveNext())
						{
							goto Block_3;
						}
					}
					if (enums[i].Current.Start <= num)
					{
						extentEnd = Math.Min(extentEnd, enums[i].Current.Start + enums[i].Current.Length);
						num2++;
					}
					else
					{
						num = enums[i].Current.Start;
						extentEnd = num + enums[i].Current.Length;
						num2 = 1;
					}
					if (num2 == streams.Length)
					{
						yield return new StreamExtent(num, extentEnd - num);
						num = extentEnd;
						extentEnd = long.MaxValue;
						num2 = 0;
					}
					num3 = i + 1;
				}
			}
			Block_3:
			yield break;
			yield break;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000502E File Offset: 0x0000322E
		public static IEnumerable<StreamExtent> Subtract(IEnumerable<StreamExtent> extents, StreamExtent other)
		{
			return StreamExtent.Subtract(extents, new StreamExtent[]
			{
				other
			});
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005040 File Offset: 0x00003240
		public static IEnumerable<StreamExtent> Subtract(IEnumerable<StreamExtent> a, IEnumerable<StreamExtent> b)
		{
			return StreamExtent.Intersect(new IEnumerable<StreamExtent>[]
			{
				a,
				StreamExtent.Invert(b)
			});
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000505A File Offset: 0x0000325A
		public static IEnumerable<StreamExtent> Invert(IEnumerable<StreamExtent> extents)
		{
			StreamExtent streamExtent = new StreamExtent(0L, 0L);
			foreach (StreamExtent extent in extents)
			{
				if (extent.Length != 0L)
				{
					long num = streamExtent.Start + streamExtent.Length;
					if (num < extent.Start)
					{
						yield return new StreamExtent(num, extent.Start - num);
					}
					streamExtent = extent;
					extent = null;
				}
			}
			IEnumerator<StreamExtent> enumerator = null;
			long num2 = streamExtent.Start + streamExtent.Length;
			if (num2 < 9223372036854775807L)
			{
				yield return new StreamExtent(num2, long.MaxValue - num2);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000506A File Offset: 0x0000326A
		public static IEnumerable<StreamExtent> Offset(IEnumerable<StreamExtent> stream, long delta)
		{
			foreach (StreamExtent streamExtent in stream)
			{
				yield return new StreamExtent(streamExtent.Start + delta, streamExtent.Length);
			}
			IEnumerator<StreamExtent> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005084 File Offset: 0x00003284
		public static long BlockCount(IEnumerable<StreamExtent> stream, long blockSize)
		{
			long num = 0L;
			long num2 = -1L;
			foreach (StreamExtent streamExtent in stream)
			{
				if (streamExtent.Length > 0L)
				{
					long num3 = streamExtent.Start / blockSize;
					long num4 = MathUtilities.Ceil(streamExtent.Start + streamExtent.Length, blockSize);
					long num5 = num4 - num3;
					if (num3 == num2)
					{
						num5 -= 1L;
					}
					num2 = num4 - 1L;
					num += num5;
				}
			}
			return num;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005110 File Offset: 0x00003310
		public static IEnumerable<Range<long, long>> Blocks(IEnumerable<StreamExtent> stream, long blockSize)
		{
			long? num = null;
			long num2 = 0L;
			foreach (StreamExtent streamExtent in stream)
			{
				if (streamExtent.Length > 0L)
				{
					long extentStartBlock = streamExtent.Start / blockSize;
					long extentNextBlock = MathUtilities.Ceil(streamExtent.Start + streamExtent.Length, blockSize);
					if (num == null)
					{
						goto IL_125;
					}
					long num3 = extentStartBlock;
					long? num4 = num + num2;
					if (!(num3 > num4.GetValueOrDefault() & num4 != null))
					{
						goto IL_125;
					}
					yield return new Range<long, long>(num.Value, num2);
					num = new long?(extentStartBlock);
					IL_13B:
					num2 = extentNextBlock - num.Value;
					continue;
					IL_125:
					if (num == null)
					{
						num = new long?(extentStartBlock);
						goto IL_13B;
					}
					goto IL_13B;
				}
			}
			IEnumerator<StreamExtent> enumerator = null;
			if (num != null)
			{
				yield return new Range<long, long>(num.Value, num2);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005127 File Offset: 0x00003327
		public static bool operator ==(StreamExtent a, StreamExtent b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005138 File Offset: 0x00003338
		public static bool operator !=(StreamExtent a, StreamExtent b)
		{
			return !(a == b);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005144 File Offset: 0x00003344
		public static bool operator <(StreamExtent a, StreamExtent b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005150 File Offset: 0x00003350
		public static bool operator >(StreamExtent a, StreamExtent b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000515C File Offset: 0x0000335C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.Start,
				":+",
				this.Length,
				"]"
			});
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000051A8 File Offset: 0x000033A8
		public override bool Equals(object obj)
		{
			return this.Equals(obj as StreamExtent);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000051B8 File Offset: 0x000033B8
		public override int GetHashCode()
		{
			return this.Start.GetHashCode() ^ this.Length.GetHashCode();
		}
	}
}
