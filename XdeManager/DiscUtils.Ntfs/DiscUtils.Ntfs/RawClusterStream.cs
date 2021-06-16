using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000048 RID: 72
	internal sealed class RawClusterStream : ClusterStream
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00013294 File Offset: 0x00011494
		public RawClusterStream(INtfsContext context, CookedDataRuns cookedRuns, bool isMft)
		{
			this._context = context;
			this._cookedRuns = cookedRuns;
			this._isMft = isMft;
			this._fsStream = this._context.RawStream;
			this._bytesPerCluster = context.BiosParameterBlock.BytesPerCluster;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000132D4 File Offset: 0x000114D4
		public override long AllocatedClusterCount
		{
			get
			{
				long num = 0L;
				for (int i = 0; i < this._cookedRuns.Count; i++)
				{
					CookedDataRun cookedDataRun = this._cookedRuns[i];
					num += (cookedDataRun.IsSparse ? 0L : cookedDataRun.Length);
				}
				return num;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00013320 File Offset: 0x00011520
		public override IEnumerable<Range<long, long>> StoredClusters
		{
			get
			{
				Range<long, long> range = null;
				List<Range<long, long>> list = new List<Range<long, long>>();
				int count = this._cookedRuns.Count;
				for (int i = 0; i < count; i++)
				{
					CookedDataRun cookedDataRun = this._cookedRuns[i];
					if (!cookedDataRun.IsSparse)
					{
						long startVcn = cookedDataRun.StartVcn;
						if (range != null && range.Offset + range.Count == startVcn)
						{
							range = new Range<long, long>(range.Offset, range.Count + cookedDataRun.Length);
							list[list.Count - 1] = range;
						}
						else
						{
							range = new Range<long, long>(cookedDataRun.StartVcn, cookedDataRun.Length);
							list.Add(range);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000133CC File Offset: 0x000115CC
		public override bool IsClusterStored(long vcn)
		{
			int index = this._cookedRuns.FindDataRun(vcn, 0);
			return !this._cookedRuns[index].IsSparse;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000133FC File Offset: 0x000115FC
		public bool AreAllClustersStored(long vcn, int count)
		{
			int num = 0;
			CookedDataRun cookedDataRun;
			for (long num2 = vcn; num2 < vcn + (long)count; num2 = cookedDataRun.StartVcn + cookedDataRun.Length)
			{
				num = this._cookedRuns.FindDataRun(num2, num);
				cookedDataRun = this._cookedRuns[num];
				if (cookedDataRun.IsSparse)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0001344C File Offset: 0x0001164C
		public override void ExpandToClusters(long numVirtualClusters, NonResidentAttributeRecord extent, bool allocate)
		{
			long nextVirtualCluster = this._cookedRuns.NextVirtualCluster;
			if (nextVirtualCluster < numVirtualClusters)
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = extent;
				if (nonResidentAttributeRecord == null)
				{
					nonResidentAttributeRecord = this._cookedRuns.Last.AttributeExtent;
				}
				DataRun dataRun = new DataRun(0L, numVirtualClusters - nextVirtualCluster, true);
				nonResidentAttributeRecord.DataRuns.Add(dataRun);
				this._cookedRuns.Append(dataRun, extent);
				nonResidentAttributeRecord.LastVcn = numVirtualClusters - 1L;
			}
			if (allocate)
			{
				this.AllocateClusters(nextVirtualCluster, (int)(numVirtualClusters - nextVirtualCluster));
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000134C0 File Offset: 0x000116C0
		public override void TruncateToClusters(long numVirtualClusters)
		{
			if (numVirtualClusters < this._cookedRuns.NextVirtualCluster)
			{
				this.ReleaseClusters(numVirtualClusters, (int)(this._cookedRuns.NextVirtualCluster - numVirtualClusters));
				int num = this._cookedRuns.FindDataRun(numVirtualClusters, 0);
				if (numVirtualClusters != this._cookedRuns[num].StartVcn)
				{
					this._cookedRuns.SplitRun(num, numVirtualClusters);
					num++;
				}
				this._cookedRuns.TruncateAt(num);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00013530 File Offset: 0x00011730
		public int AllocateClusters(long startVcn, int count)
		{
			if (startVcn + (long)count > this._cookedRuns.NextVirtualCluster)
			{
				throw new IOException("Attempt to allocate unknown clusters");
			}
			int num = 0;
			int num2 = 0;
			long num3 = startVcn;
			while (num3 < startVcn + (long)count)
			{
				num2 = this._cookedRuns.FindDataRun(num3, num2);
				CookedDataRun cookedDataRun = this._cookedRuns[num2];
				if (cookedDataRun.IsSparse)
				{
					if (num3 != cookedDataRun.StartVcn)
					{
						this._cookedRuns.SplitRun(num2, num3);
						num2++;
						cookedDataRun = this._cookedRuns[num2];
					}
					long num4 = Math.Min(startVcn + (long)count - num3, cookedDataRun.Length);
					if (num4 != cookedDataRun.Length)
					{
						this._cookedRuns.SplitRun(num2, num3 + num4);
						cookedDataRun = this._cookedRuns[num2];
					}
					long proposedStart = -1L;
					for (int i = num2 - 1; i >= 0; i--)
					{
						if (!this._cookedRuns[i].IsSparse)
						{
							proposedStart = this._cookedRuns[i].StartLcn + this._cookedRuns[i].Length;
							break;
						}
					}
					Tuple<long, long>[] array = this._context.ClusterBitmap.AllocateClusters(num4, proposedStart, this._isMft, this.AllocatedClusterCount);
					List<DataRun> list = new List<DataRun>();
					long num5 = (num2 == 0) ? 0L : this._cookedRuns[num2 - 1].StartLcn;
					foreach (Tuple<long, long> tuple in array)
					{
						list.Add(new DataRun(tuple.Item1 - num5, tuple.Item2, false));
						num5 = tuple.Item1;
					}
					this._cookedRuns.MakeNonSparse(num2, list);
					num += (int)num4;
					num3 += num4;
				}
				else
				{
					num3 = cookedDataRun.StartVcn + cookedDataRun.Length;
				}
			}
			return num;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000136F8 File Offset: 0x000118F8
		public int ReleaseClusters(long startVcn, int count)
		{
			int num = 0;
			int num2 = 0;
			long num3 = startVcn;
			while (num3 < startVcn + (long)count)
			{
				num = this._cookedRuns.FindDataRun(num3, num);
				CookedDataRun cookedDataRun = this._cookedRuns[num];
				if (cookedDataRun.IsSparse)
				{
					num3 += cookedDataRun.Length;
				}
				else
				{
					if (num3 != cookedDataRun.StartVcn)
					{
						this._cookedRuns.SplitRun(num, num3);
						num++;
						cookedDataRun = this._cookedRuns[num];
					}
					long num4 = Math.Min(startVcn + (long)count - num3, cookedDataRun.Length);
					if (num4 != cookedDataRun.Length)
					{
						this._cookedRuns.SplitRun(num, num3 + num4);
						cookedDataRun = this._cookedRuns[num];
					}
					this._context.ClusterBitmap.FreeClusters(new Range<long, long>[]
					{
						new Range<long, long>(cookedDataRun.StartLcn, cookedDataRun.Length)
					});
					this._cookedRuns.MakeSparse(num);
					num2 += (int)cookedDataRun.Length;
					num3 += num4;
				}
			}
			return num2;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000137F4 File Offset: 0x000119F4
		public override void ReadClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			StreamUtilities.AssertBufferParameters(buffer, offset, count * this._bytesPerCluster);
			int num = 0;
			int num3;
			for (int i = 0; i < count; i += num3)
			{
				long num2 = startVcn + (long)i;
				num = this._cookedRuns.FindDataRun(num2, num);
				CookedDataRun cookedDataRun = this._cookedRuns[num];
				num3 = (int)Math.Min((long)(count - i), cookedDataRun.Length - (num2 - cookedDataRun.StartVcn));
				if (cookedDataRun.IsSparse)
				{
					Array.Clear(buffer, offset + i * this._bytesPerCluster, num3 * this._bytesPerCluster);
				}
				else
				{
					long num4 = this._cookedRuns[num].StartLcn + (num2 - cookedDataRun.StartVcn);
					this._fsStream.Position = num4 * (long)this._bytesPerCluster;
					StreamUtilities.ReadExact(this._fsStream, buffer, offset + i * this._bytesPerCluster, num3 * this._bytesPerCluster);
				}
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000138D8 File Offset: 0x00011AD8
		public override int WriteClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			StreamUtilities.AssertBufferParameters(buffer, offset, count * this._bytesPerCluster);
			int num = 0;
			int num3;
			for (int i = 0; i < count; i += num3)
			{
				long num2 = startVcn + (long)i;
				num = this._cookedRuns.FindDataRun(num2, num);
				CookedDataRun cookedDataRun = this._cookedRuns[num];
				if (cookedDataRun.IsSparse)
				{
					throw new NotImplementedException("Writing to sparse datarun");
				}
				num3 = (int)Math.Min((long)(count - i), cookedDataRun.Length - (num2 - cookedDataRun.StartVcn));
				long num4 = this._cookedRuns[num].StartLcn + (num2 - cookedDataRun.StartVcn);
				this._fsStream.Position = num4 * (long)this._bytesPerCluster;
				this._fsStream.Write(buffer, offset + i * this._bytesPerCluster, num3 * this._bytesPerCluster);
			}
			return 0;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000139AC File Offset: 0x00011BAC
		public override int ClearClusters(long startVcn, int count)
		{
			byte[] buffer = new byte[16 * this._bytesPerCluster];
			int num = 0;
			int num2;
			for (int i = 0; i < count; i += num2)
			{
				num2 = Math.Min(count - i, 16);
				num += this.WriteClusters(startVcn + (long)i, num2, buffer, 0);
			}
			return -num;
		}

		// Token: 0x04000163 RID: 355
		private readonly int _bytesPerCluster;

		// Token: 0x04000164 RID: 356
		private readonly INtfsContext _context;

		// Token: 0x04000165 RID: 357
		private readonly CookedDataRuns _cookedRuns;

		// Token: 0x04000166 RID: 358
		private readonly Stream _fsStream;

		// Token: 0x04000167 RID: 359
		private readonly bool _isMft;
	}
}
