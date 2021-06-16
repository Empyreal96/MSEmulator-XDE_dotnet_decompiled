using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000050 RID: 80
	internal sealed class SparseClusterStream : ClusterStream
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x000149CC File Offset: 0x00012BCC
		public SparseClusterStream(NtfsAttribute attr, RawClusterStream rawStream)
		{
			this._attr = attr;
			this._rawStream = rawStream;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000149E2 File Offset: 0x00012BE2
		public override long AllocatedClusterCount
		{
			get
			{
				return this._rawStream.AllocatedClusterCount;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000149EF File Offset: 0x00012BEF
		public override IEnumerable<Range<long, long>> StoredClusters
		{
			get
			{
				return this._rawStream.StoredClusters;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000149FC File Offset: 0x00012BFC
		public override bool IsClusterStored(long vcn)
		{
			return this._rawStream.IsClusterStored(vcn);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00014A0A File Offset: 0x00012C0A
		public override void ExpandToClusters(long numVirtualClusters, NonResidentAttributeRecord extent, bool allocate)
		{
			this._rawStream.ExpandToClusters(this.CompressionStart(numVirtualClusters), extent, false);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00014A20 File Offset: 0x00012C20
		public override void TruncateToClusters(long numVirtualClusters)
		{
			long num = this.CompressionStart(numVirtualClusters);
			this._rawStream.TruncateToClusters(num);
			if (num != numVirtualClusters)
			{
				this._rawStream.ReleaseClusters(numVirtualClusters, (int)(num - numVirtualClusters));
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00014A56 File Offset: 0x00012C56
		public override void ReadClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			this._rawStream.ReadClusters(startVcn, count, buffer, offset);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00014A68 File Offset: 0x00012C68
		public override int WriteClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			return 0 + this._rawStream.AllocateClusters(startVcn, count) + this._rawStream.WriteClusters(startVcn, count, buffer, offset);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00014A8A File Offset: 0x00012C8A
		public override int ClearClusters(long startVcn, int count)
		{
			return this._rawStream.ReleaseClusters(startVcn, count);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00014A99 File Offset: 0x00012C99
		private long CompressionStart(long vcn)
		{
			return MathUtilities.RoundUp(vcn, (long)this._attr.CompressionUnitSize);
		}

		// Token: 0x0400017E RID: 382
		private readonly NtfsAttribute _attr;

		// Token: 0x0400017F RID: 383
		private readonly RawClusterStream _rawStream;
	}
}
