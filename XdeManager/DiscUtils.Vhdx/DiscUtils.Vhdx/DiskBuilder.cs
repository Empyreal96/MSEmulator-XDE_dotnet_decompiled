using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000007 RID: 7
	public sealed class DiskBuilder : DiskImageBuilder
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000033AC File Offset: 0x000015AC
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000033B4 File Offset: 0x000015B4
		public long BlockSize
		{
			get
			{
				return this._blockSize;
			}
			set
			{
				if (value % 1048576L != 0L)
				{
					throw new ArgumentException("BlockSize must be a multiple of 1MB", "value");
				}
				this._blockSize = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000033D7 File Offset: 0x000015D7
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000033DF File Offset: 0x000015DF
		public DiskType DiskType { get; set; }

		// Token: 0x0600004C RID: 76 RVA: 0x000033E8 File Offset: 0x000015E8
		public override DiskImageFileSpecification[] Build(string baseName)
		{
			if (string.IsNullOrEmpty(baseName))
			{
				throw new ArgumentException("Invalid base file name", "baseName");
			}
			if (base.Content == null)
			{
				throw new InvalidOperationException("No content stream specified");
			}
			DiskImageFileSpecification diskImageFileSpecification = new DiskImageFileSpecification(baseName + ".vhdx", new DiskBuilder.DiskStreamBuilder(base.Content, this.DiskType, this.BlockSize));
			return new DiskImageFileSpecification[]
			{
				diskImageFileSpecification
			};
		}

		// Token: 0x0400001C RID: 28
		private long _blockSize = 33554432L;

		// Token: 0x02000027 RID: 39
		private class DiskStreamBuilder : StreamBuilder
		{
			// Token: 0x06000142 RID: 322 RVA: 0x00006753 File Offset: 0x00004953
			public DiskStreamBuilder(SparseStream content, DiskType diskType, long blockSize)
			{
				this._content = content;
				this._diskType = diskType;
				this._blockSize = blockSize;
			}

			// Token: 0x06000143 RID: 323 RVA: 0x00006770 File Offset: 0x00004970
			protected override List<BuilderExtent> FixExtents(out long totalLength)
			{
				if (this._diskType != DiskType.Dynamic)
				{
					throw new NotSupportedException("Creation of only dynamic disks currently implemented");
				}
				List<BuilderExtent> list = new List<BuilderExtent>();
				int num = 512;
				int physicalSectorSize = 4096;
				long num2 = 8388608L * (long)num / this._blockSize;
				long num3 = MathUtilities.Ceil(this._content.Length, this._blockSize);
				MathUtilities.Ceil(num3, num2);
				long num4 = num3 + (num3 - 1L) / num2;
				FileHeader structure = new FileHeader
				{
					Creator = ".NET DiscUtils"
				};
				long num5 = 1048576L;
				VhdxHeader vhdxHeader = new VhdxHeader();
				vhdxHeader.SequenceNumber = 0UL;
				vhdxHeader.FileWriteGuid = Guid.NewGuid();
				vhdxHeader.DataWriteGuid = Guid.NewGuid();
				vhdxHeader.LogGuid = Guid.Empty;
				vhdxHeader.LogVersion = 0;
				vhdxHeader.Version = 1;
				vhdxHeader.LogLength = 1048576U;
				vhdxHeader.LogOffset = (ulong)num5;
				vhdxHeader.CalcChecksum();
				num5 += (long)((ulong)vhdxHeader.LogLength);
				VhdxHeader vhdxHeader2 = new VhdxHeader(vhdxHeader);
				vhdxHeader2.SequenceNumber = 1UL;
				vhdxHeader2.CalcChecksum();
				RegionTable regionTable = new RegionTable();
				RegionEntry regionEntry = new RegionEntry();
				regionEntry.Guid = RegionEntry.MetadataRegionGuid;
				regionEntry.FileOffset = num5;
				regionEntry.Length = 1048576U;
				regionEntry.Flags = RegionFlags.Required;
				regionTable.Regions.Add(regionEntry.Guid, regionEntry);
				num5 += (long)((ulong)regionEntry.Length);
				RegionEntry regionEntry2 = new RegionEntry();
				regionEntry2.Guid = RegionEntry.BatGuid;
				regionEntry2.FileOffset = num5;
				regionEntry2.Length = (uint)MathUtilities.RoundUp(num4 * 8L, 1048576L);
				regionEntry2.Flags = RegionFlags.Required;
				regionTable.Regions.Add(regionEntry2.Guid, regionEntry2);
				num5 += (long)((ulong)regionEntry2.Length);
				list.Add(DiskBuilder.DiskStreamBuilder.ExtentForStruct(structure, 0L));
				list.Add(DiskBuilder.DiskStreamBuilder.ExtentForStruct(vhdxHeader, 65536L));
				list.Add(DiskBuilder.DiskStreamBuilder.ExtentForStruct(vhdxHeader2, 131072L));
				list.Add(DiskBuilder.DiskStreamBuilder.ExtentForStruct(regionTable, 196608L));
				list.Add(DiskBuilder.DiskStreamBuilder.ExtentForStruct(regionTable, 262144L));
				FileParameters fileParameters = new FileParameters
				{
					BlockSize = (uint)this._blockSize,
					Flags = FileParametersFlags.None
				};
				new ParentLocator();
				byte[] buffer = new byte[regionEntry.Length];
				Metadata.Initialize(new MemoryStream(buffer), fileParameters, (ulong)this._content.Length, (uint)num, (uint)physicalSectorSize, null);
				list.Add(new BuilderBufferExtent(regionEntry.FileOffset, buffer));
				List<Range<long, long>> list2 = new List<Range<long, long>>(StreamExtent.Blocks(this._content.Extents, this._blockSize));
				DiskBuilder.BlockAllocationTableBuilderExtent item = new DiskBuilder.BlockAllocationTableBuilderExtent(regionEntry2.FileOffset, (long)((ulong)regionEntry2.Length), list2, num5, this._blockSize, num2);
				list.Add(item);
				foreach (Range<long, long> range in list2)
				{
					long num6 = range.Offset * this._blockSize;
					long length = Math.Min(this._content.Length - num6, range.Count * this._blockSize);
					SubStream stream = new SubStream(this._content, num6, length);
					BuilderSparseStreamExtent item2 = new BuilderSparseStreamExtent(num5, stream);
					list.Add(item2);
					num5 += range.Count * this._blockSize;
				}
				totalLength = num5;
				return list;
			}

			// Token: 0x06000144 RID: 324 RVA: 0x00006AE0 File Offset: 0x00004CE0
			private static BuilderExtent ExtentForStruct(IByteArraySerializable structure, long position)
			{
				byte[] buffer = new byte[structure.Size];
				structure.WriteTo(buffer, 0);
				return new BuilderBufferExtent(position, buffer);
			}

			// Token: 0x040000B7 RID: 183
			private readonly long _blockSize;

			// Token: 0x040000B8 RID: 184
			private readonly SparseStream _content;

			// Token: 0x040000B9 RID: 185
			private readonly DiskType _diskType;
		}

		// Token: 0x02000028 RID: 40
		private class BlockAllocationTableBuilderExtent : BuilderExtent
		{
			// Token: 0x06000145 RID: 325 RVA: 0x00006B08 File Offset: 0x00004D08
			public BlockAllocationTableBuilderExtent(long start, long length, List<Range<long, long>> blocks, long dataStart, long blockSize, long chunkRatio) : base(start, length)
			{
				this._blocks = blocks;
				this._dataStart = dataStart;
				this._blockSize = blockSize;
				this._chunkRatio = chunkRatio;
			}

			// Token: 0x06000146 RID: 326 RVA: 0x00006B31 File Offset: 0x00004D31
			public override void Dispose()
			{
				this._batData = null;
			}

			// Token: 0x06000147 RID: 327 RVA: 0x00006B3C File Offset: 0x00004D3C
			public override void PrepareForRead()
			{
				this._batData = new byte[base.Length];
				long num = this._dataStart;
				BatEntry batEntry = default(BatEntry);
				foreach (Range<long, long> range in this._blocks)
				{
					for (long num2 = range.Offset; num2 < range.Offset + range.Count; num2 += 1L)
					{
						long num3 = num2 / this._chunkRatio;
						long num4 = num2 % this._chunkRatio;
						long num5 = num3 * (this._chunkRatio + 1L) + num4;
						batEntry.FileOffsetMB = num / 1048576L;
						batEntry.PayloadBlockStatus = PayloadBlockStatus.FullyPresent;
						batEntry.WriteTo(this._batData, (int)(num5 * 8L));
						num += this._blockSize;
					}
				}
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00006C28 File Offset: 0x00004E28
			public override int Read(long diskOffset, byte[] block, int offset, int count)
			{
				int num = (int)Math.Min(diskOffset - base.Start, (long)this._batData.Length);
				int num2 = Math.Min(count, this._batData.Length - num);
				Array.Copy(this._batData, num, block, offset, num2);
				return num2;
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00006C6F File Offset: 0x00004E6F
			public override void DisposeReadState()
			{
				this._batData = null;
			}

			// Token: 0x040000BA RID: 186
			private byte[] _batData;

			// Token: 0x040000BB RID: 187
			private readonly List<Range<long, long>> _blocks;

			// Token: 0x040000BC RID: 188
			private readonly long _blockSize;

			// Token: 0x040000BD RID: 189
			private readonly long _chunkRatio;

			// Token: 0x040000BE RID: 190
			private readonly long _dataStart;
		}
	}
}
