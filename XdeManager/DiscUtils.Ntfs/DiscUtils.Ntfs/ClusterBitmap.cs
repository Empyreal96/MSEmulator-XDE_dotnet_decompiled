using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000011 RID: 17
	internal class ClusterBitmap : IDisposable
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003994 File Offset: 0x00001B94
		public ClusterBitmap(File file)
		{
			this._file = file;
			this._bitmap = new Bitmap(this._file.OpenStream(AttributeType.Data, null, FileAccess.ReadWrite), MathUtilities.Ceil(file.Context.BiosParameterBlock.TotalSectors64, (long)((ulong)file.Context.BiosParameterBlock.SectorsPerCluster)));
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000039F1 File Offset: 0x00001BF1
		internal Bitmap Bitmap
		{
			get
			{
				return this._bitmap;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000039F9 File Offset: 0x00001BF9
		public void Dispose()
		{
			if (this._bitmap != null)
			{
				this._bitmap.Dispose();
				this._bitmap = null;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003A18 File Offset: 0x00001C18
		public Tuple<long, long>[] AllocateClusters(long count, long proposedStart, bool isMft, long total)
		{
			List<Tuple<long, long>> list = new List<Tuple<long, long>>();
			long num = 0L;
			long num2 = this._file.Context.RawStream.Length / (long)this._file.Context.BiosParameterBlock.BytesPerCluster;
			if (isMft)
			{
				if (proposedStart >= 0L)
				{
					num += this.ExtendRun(count - num, list, proposedStart, num2);
				}
				if (num < count && !this._fragmentedDiskMode)
				{
					num += this.FindClusters(count - num, list, 0L, num2, isMft, true, 0L);
				}
				if (num < count)
				{
					num += this.FindClusters(count - num, list, 0L, num2, isMft, false, 0L);
				}
			}
			else
			{
				if (proposedStart >= 0L)
				{
					num += this.ExtendRun(count - num, list, proposedStart, num2);
				}
				if (num < count && !this._fragmentedDiskMode)
				{
					num += this.FindClusters(count - num, list, num2 / 8L, num2, isMft, true, total / 4L);
				}
				if (num < count)
				{
					num += this.FindClusters(count - num, list, num2 / 8L, num2, isMft, false, 0L);
				}
				if (num < count)
				{
					num = this.FindClusters(count - num, list, num2 / 16L, num2 / 8L, isMft, false, 0L);
				}
				if (num < count)
				{
					num = this.FindClusters(count - num, list, num2 / 32L, num2 / 16L, isMft, false, 0L);
				}
				if (num < count)
				{
					num = this.FindClusters(count - num, list, 0L, num2 / 32L, isMft, false, 0L);
				}
			}
			if (num < count)
			{
				this.FreeClusters(list.ToArray());
				throw new IOException("Out of disk space");
			}
			if ((num > 4L && list.Count == 1) || list.Count > 1)
			{
				this._fragmentedDiskMode = (num / (long)list.Count < 4L);
			}
			return list.ToArray();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003BA3 File Offset: 0x00001DA3
		internal void MarkAllocated(long first, long count)
		{
			this._bitmap.MarkPresentRange(first, count);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003BB4 File Offset: 0x00001DB4
		internal void FreeClusters(params Tuple<long, long>[] runs)
		{
			foreach (Tuple<long, long> tuple in runs)
			{
				this._bitmap.MarkAbsentRange(tuple.Item1, tuple.Item2);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003BEC File Offset: 0x00001DEC
		internal void FreeClusters(params Range<long, long>[] runs)
		{
			foreach (Range<long, long> range in runs)
			{
				this._bitmap.MarkAbsentRange(range.Offset, range.Count);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003C24 File Offset: 0x00001E24
		internal void SetTotalClusters(long numClusters)
		{
			long num = this._bitmap.SetTotalEntries(numClusters);
			if (num != numClusters)
			{
				this.MarkAllocated(numClusters, num - numClusters);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003C4C File Offset: 0x00001E4C
		private long ExtendRun(long count, List<Tuple<long, long>> result, long start, long end)
		{
			long num = start;
			while (!this._bitmap.IsPresent(num) && num < end && num - start < count)
			{
				num += 1L;
			}
			long num2 = num - start;
			if (num2 > 0L)
			{
				this._bitmap.MarkPresentRange(start, num2);
				result.Add(new Tuple<long, long>(start, num2));
			}
			return num2;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003CA0 File Offset: 0x00001EA0
		private long FindClusters(long count, List<Tuple<long, long>> result, long start, long end, bool isMft, bool contiguous, long headroom)
		{
			long num = 0L;
			long num2;
			if (isMft)
			{
				num2 = start;
			}
			else
			{
				if (this._nextDataCluster < start || this._nextDataCluster >= end)
				{
					this._nextDataCluster = start;
				}
				num2 = this._nextDataCluster;
			}
			long num3 = 0L;
			while (num < count && num2 >= start && num3 < end - start)
			{
				if (!this._bitmap.IsPresent(num2))
				{
					long num4 = num2;
					num2 += 1L;
					while (!this._bitmap.IsPresent(num2) && num2 - num4 < count - num)
					{
						num2 += 1L;
						num3 += 1L;
					}
					if (!contiguous || num2 - num4 == count - num)
					{
						this._bitmap.MarkPresentRange(num4, num2 - num4);
						result.Add(new Tuple<long, long>(num4, num2 - num4));
						num += num2 - num4;
					}
				}
				else
				{
					num2 += 1L;
				}
				num3 += 1L;
				if (num2 >= end)
				{
					num2 = start;
				}
			}
			if (!isMft)
			{
				this._nextDataCluster = num2 + headroom;
			}
			return num;
		}

		// Token: 0x0400005E RID: 94
		private Bitmap _bitmap;

		// Token: 0x0400005F RID: 95
		private readonly File _file;

		// Token: 0x04000060 RID: 96
		private bool _fragmentedDiskMode;

		// Token: 0x04000061 RID: 97
		private long _nextDataCluster;
	}
}
