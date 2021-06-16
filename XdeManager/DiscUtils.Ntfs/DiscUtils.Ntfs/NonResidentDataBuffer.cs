using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000038 RID: 56
	internal class NonResidentDataBuffer : DiscUtils.Streams.Buffer, IMappedBuffer, IBuffer
	{
		// Token: 0x06000246 RID: 582 RVA: 0x0000C663 File Offset: 0x0000A863
		public NonResidentDataBuffer(INtfsContext context, NonResidentAttributeRecord record) : this(context, new CookedDataRuns(record.DataRuns, record), false)
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C67C File Offset: 0x0000A87C
		public NonResidentDataBuffer(INtfsContext context, CookedDataRuns cookedRuns, bool isMft)
		{
			this._context = context;
			this._cookedRuns = cookedRuns;
			this._rawStream = new RawClusterStream(this._context, this._cookedRuns, isMft);
			this._activeStream = this._rawStream;
			this._bytesPerCluster = (long)this._context.BiosParameterBlock.BytesPerCluster;
			this._ioBuffer = new byte[this._bytesPerCluster];
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000C6EA File Offset: 0x0000A8EA
		public long VirtualClusterCount
		{
			get
			{
				return this._cookedRuns.NextVirtualCluster;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000C6F7 File Offset: 0x0000A8F7
		public override bool CanRead
		{
			get
			{
				return this._context.RawStream.CanRead;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000C709 File Offset: 0x0000A909
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000C70C File Offset: 0x0000A90C
		public override long Capacity
		{
			get
			{
				return this.VirtualClusterCount * this._bytesPerCluster;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000C71C File Offset: 0x0000A91C
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				List<StreamExtent> list = new List<StreamExtent>();
				foreach (Range<long, long> range in this._activeStream.StoredClusters)
				{
					list.Add(new StreamExtent(range.Offset * this._bytesPerCluster, range.Count * this._bytesPerCluster));
				}
				return StreamExtent.Intersect(list, new StreamExtent(0L, this.Capacity));
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			return StreamExtent.Intersect(this.Extents, new StreamExtent(start, count));
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		public long MapPosition(long pos)
		{
			long vcn = pos / this._bytesPerCluster;
			int index = this._cookedRuns.FindDataRun(vcn, 0);
			if (this._cookedRuns[index].IsSparse)
			{
				return -1L;
			}
			return this._cookedRuns[index].StartLcn * this._bytesPerCluster + (pos - this._cookedRuns[index].StartVcn * this._bytesPerCluster);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new IOException("Attempt to read from file not opened for read");
			}
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			int num = (int)Math.Min((long)count, this.Capacity - pos);
			if (num <= 0)
			{
				return 0;
			}
			long num2 = pos;
			while (num2 < pos + (long)num)
			{
				long num3 = num2 / this._bytesPerCluster;
				long num4 = pos + (long)num - num2;
				long num5 = num2 - num3 * this._bytesPerCluster;
				if (num3 * this._bytesPerCluster != num2 || num4 < this._bytesPerCluster)
				{
					this._activeStream.ReadClusters(num3, 1, this._ioBuffer, 0);
					int num6 = (int)Math.Min(num4, this._bytesPerCluster - num5);
					Array.Copy(this._ioBuffer, (int)num5, buffer, (int)((long)offset + (num2 - pos)), num6);
					num2 += (long)num6;
				}
				else
				{
					int num7 = (int)(num4 / this._bytesPerCluster);
					this._activeStream.ReadClusters(num3, num7, buffer, (int)((long)offset + (num2 - pos)));
					num2 += (long)num7 * this._bytesPerCluster;
				}
			}
			return num;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C923 File Offset: 0x0000AB23
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C92A File Offset: 0x0000AB2A
		public override void SetCapacity(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000111 RID: 273
		protected ClusterStream _activeStream;

		// Token: 0x04000112 RID: 274
		protected long _bytesPerCluster;

		// Token: 0x04000113 RID: 275
		protected INtfsContext _context;

		// Token: 0x04000114 RID: 276
		protected CookedDataRuns _cookedRuns;

		// Token: 0x04000115 RID: 277
		protected byte[] _ioBuffer;

		// Token: 0x04000116 RID: 278
		protected RawClusterStream _rawStream;
	}
}
