using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Compression;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000013 RID: 19
	internal sealed class CompressedClusterStream : ClusterStream
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00003D84 File Offset: 0x00001F84
		public CompressedClusterStream(INtfsContext context, NtfsAttribute attr, RawClusterStream rawStream)
		{
			this._context = context;
			this._attr = attr;
			this._rawStream = rawStream;
			this._bytesPerCluster = this._context.BiosParameterBlock.BytesPerCluster;
			this._cacheBuffer = new byte[this._attr.CompressionUnitSize * context.BiosParameterBlock.BytesPerCluster];
			this._ioBuffer = new byte[this._attr.CompressionUnitSize * context.BiosParameterBlock.BytesPerCluster];
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003E0E File Offset: 0x0000200E
		public override long AllocatedClusterCount
		{
			get
			{
				return this._rawStream.AllocatedClusterCount;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003E1B File Offset: 0x0000201B
		public override IEnumerable<Range<long, long>> StoredClusters
		{
			get
			{
				return Range<long, long>.Chunked<long>(this._rawStream.StoredClusters, (long)this._attr.CompressionUnitSize);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003E39 File Offset: 0x00002039
		public override bool IsClusterStored(long vcn)
		{
			return this._rawStream.IsClusterStored(this.CompressionStart(vcn));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003E4D File Offset: 0x0000204D
		public override void ExpandToClusters(long numVirtualClusters, NonResidentAttributeRecord extent, bool allocate)
		{
			this._rawStream.ExpandToClusters(MathUtilities.RoundUp(numVirtualClusters, (long)this._attr.CompressionUnitSize), extent, false);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003E70 File Offset: 0x00002070
		public override void TruncateToClusters(long numVirtualClusters)
		{
			long num = MathUtilities.RoundUp(numVirtualClusters, (long)this._attr.CompressionUnitSize);
			this._rawStream.TruncateToClusters(num);
			if (num != numVirtualClusters)
			{
				this._rawStream.ReleaseClusters(numVirtualClusters, (int)(num - numVirtualClusters));
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003EB4 File Offset: 0x000020B4
		public override void ReadClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			if (buffer.Length < count * this._bytesPerCluster + offset)
			{
				throw new ArgumentException("Cluster buffer too small", "buffer");
			}
			int num3;
			for (int i = 0; i < count; i += num3)
			{
				long num = startVcn + (long)i;
				this.LoadCache(num);
				int num2 = (int)(num - this._cacheBufferVcn);
				num3 = Math.Min(this._attr.CompressionUnitSize - num2, count - i);
				Array.Copy(this._cacheBuffer, num2 * this._bytesPerCluster, buffer, offset + i * this._bytesPerCluster, num3 * this._bytesPerCluster);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003F40 File Offset: 0x00002140
		public override int WriteClusters(long startVcn, int count, byte[] buffer, int offset)
		{
			if (buffer.Length < count * this._bytesPerCluster + offset)
			{
				throw new ArgumentException("Cluster buffer too small", "buffer");
			}
			int num = 0;
			int i = 0;
			while (i < count)
			{
				long num2 = startVcn + (long)i;
				if (this.CompressionStart(num2) == num2 && count - i >= this._attr.CompressionUnitSize)
				{
					num += this.CompressAndWriteClusters(num2, this._attr.CompressionUnitSize, buffer, offset + i * this._bytesPerCluster);
					i += this._attr.CompressionUnitSize;
				}
				else
				{
					this.LoadCache(num2);
					int num3 = (int)(num2 - this._cacheBufferVcn);
					int num4 = Math.Min(count - i, this._attr.CompressionUnitSize - num3);
					Array.Copy(buffer, offset + i * this._bytesPerCluster, this._cacheBuffer, num3 * this._bytesPerCluster, num4 * this._bytesPerCluster);
					num += this.CompressAndWriteClusters(this._cacheBufferVcn, this._attr.CompressionUnitSize, this._cacheBuffer, 0);
					i += num4;
				}
			}
			return num;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004044 File Offset: 0x00002244
		public override int ClearClusters(long startVcn, int count)
		{
			int num = 0;
			int i = 0;
			while (i < count)
			{
				long num2 = startVcn + (long)i;
				if (this.CompressionStart(num2) == num2 && count - i >= this._attr.CompressionUnitSize)
				{
					num += this._rawStream.ReleaseClusters(startVcn, this._attr.CompressionUnitSize);
					i += this._attr.CompressionUnitSize;
				}
				else
				{
					int num3 = (int)Math.Min((long)(count - i), (long)this._attr.CompressionUnitSize - (num2 - this.CompressionStart(num2)));
					num -= this.WriteZeroClusters(num2, num3);
					i += num3;
				}
			}
			return num;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000040D8 File Offset: 0x000022D8
		private int WriteZeroClusters(long focusVcn, int count)
		{
			int num = 0;
			byte[] buffer = new byte[16 * this._bytesPerCluster];
			int num2;
			for (int i = 0; i < count; i += num2)
			{
				num2 = Math.Min(count - i, 16);
				num += this.WriteClusters(focusVcn + (long)i, num2, buffer, 0);
			}
			return num;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004120 File Offset: 0x00002320
		private int CompressAndWriteClusters(long focusVcn, int count, byte[] buffer, int offset)
		{
			BlockCompressor compressor = this._context.Options.Compressor;
			compressor.BlockSize = this._bytesPerCluster;
			int num = 0;
			int num2 = this._ioBuffer.Length;
			CompressionResult compressionResult = compressor.Compress(buffer, offset, this._attr.CompressionUnitSize * this._bytesPerCluster, this._ioBuffer, 0, ref num2);
			if (compressionResult == CompressionResult.AllZeros)
			{
				num -= this._rawStream.ReleaseClusters(focusVcn, count);
			}
			else if (compressionResult == CompressionResult.Compressed && this._attr.CompressionUnitSize * this._bytesPerCluster - num2 > this._bytesPerCluster)
			{
				int num3 = MathUtilities.Ceil(num2, this._bytesPerCluster);
				num += this._rawStream.AllocateClusters(focusVcn, num3);
				num += this._rawStream.WriteClusters(focusVcn, num3, this._ioBuffer, 0);
				num -= this._rawStream.ReleaseClusters(focusVcn + (long)num3, this._attr.CompressionUnitSize - num3);
			}
			else
			{
				num += this._rawStream.AllocateClusters(focusVcn, this._attr.CompressionUnitSize);
				num += this._rawStream.WriteClusters(focusVcn, this._attr.CompressionUnitSize, buffer, offset);
			}
			return num;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000423D File Offset: 0x0000243D
		private long CompressionStart(long vcn)
		{
			return MathUtilities.RoundDown(vcn, (long)this._attr.CompressionUnitSize);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004254 File Offset: 0x00002454
		private void LoadCache(long vcn)
		{
			long num = this.CompressionStart(vcn);
			if (this._cacheBufferVcn != num)
			{
				if (this._rawStream.AreAllClustersStored(num, this._attr.CompressionUnitSize))
				{
					this._rawStream.ReadClusters(num, this._attr.CompressionUnitSize, this._cacheBuffer, 0);
				}
				else if (this._rawStream.IsClusterStored(num))
				{
					this._rawStream.ReadClusters(num, this._attr.CompressionUnitSize, this._ioBuffer, 0);
					int num2 = (int)Math.Min(this._attr.Length - vcn * (long)this._bytesPerCluster, (long)(this._attr.CompressionUnitSize * this._bytesPerCluster));
					if (this._context.Options.Compressor.Decompress(this._ioBuffer, 0, this._ioBuffer.Length, this._cacheBuffer, 0) < num2)
					{
						throw new IOException("Decompression returned too little data");
					}
				}
				else
				{
					Array.Clear(this._cacheBuffer, 0, this._cacheBuffer.Length);
				}
				this._cacheBufferVcn = num;
			}
		}

		// Token: 0x04000062 RID: 98
		private readonly NtfsAttribute _attr;

		// Token: 0x04000063 RID: 99
		private readonly int _bytesPerCluster;

		// Token: 0x04000064 RID: 100
		private readonly byte[] _cacheBuffer;

		// Token: 0x04000065 RID: 101
		private long _cacheBufferVcn = -1L;

		// Token: 0x04000066 RID: 102
		private readonly INtfsContext _context;

		// Token: 0x04000067 RID: 103
		private readonly byte[] _ioBuffer;

		// Token: 0x04000068 RID: 104
		private readonly RawClusterStream _rawStream;
	}
}
