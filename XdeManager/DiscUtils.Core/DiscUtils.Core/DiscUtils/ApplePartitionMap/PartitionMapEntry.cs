using System;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.ApplePartitionMap
{
	// Token: 0x02000093 RID: 147
	internal sealed class PartitionMapEntry : PartitionInfo, IByteArraySerializable
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x0000EAFB File Offset: 0x0000CCFB
		public PartitionMapEntry(Stream diskStream)
		{
			this._diskStream = diskStream;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000EB0A File Offset: 0x0000CD0A
		public override byte BiosType
		{
			get
			{
				return 175;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000EB11 File Offset: 0x0000CD11
		public override long FirstSector
		{
			get
			{
				return (long)((ulong)this.PhysicalBlockStart);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000EB1A File Offset: 0x0000CD1A
		public override Guid GuidType
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0000EB21 File Offset: 0x0000CD21
		public override long LastSector
		{
			get
			{
				return (long)((ulong)(this.PhysicalBlockStart + this.PhysicalBlocks - 1U));
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000EB33 File Offset: 0x0000CD33
		public override string TypeAsString
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0000EB3B File Offset: 0x0000CD3B
		internal override PhysicalVolumeType VolumeType
		{
			get
			{
				return PhysicalVolumeType.ApplePartition;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0000EB3E File Offset: 0x0000CD3E
		public int Size
		{
			get
			{
				return 512;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000EB48 File Offset: 0x0000CD48
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.ToUInt16BigEndian(buffer, offset);
			this.MapEntries = EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
			this.PhysicalBlockStart = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
			this.PhysicalBlocks = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
			this.Name = EndianUtilities.BytesToString(buffer, offset + 16, 32).TrimEnd(new char[1]);
			this.Type = EndianUtilities.BytesToString(buffer, offset + 48, 32).TrimEnd(new char[1]);
			this.LogicalBlockStart = EndianUtilities.ToUInt32BigEndian(buffer, offset + 80);
			this.LogicalBlocks = EndianUtilities.ToUInt32BigEndian(buffer, offset + 84);
			this.Flags = EndianUtilities.ToUInt32BigEndian(buffer, offset + 88);
			this.BootBlock = EndianUtilities.ToUInt32BigEndian(buffer, offset + 92);
			this.BootBytes = EndianUtilities.ToUInt32BigEndian(buffer, offset + 96);
			return 512;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000EC1F File Offset: 0x0000CE1F
		public void WriteTo(byte[] buffer, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000EC26 File Offset: 0x0000CE26
		public override SparseStream Open()
		{
			return new SubStream(this._diskStream, (long)((ulong)(this.PhysicalBlockStart * 512U)), (long)((ulong)(this.PhysicalBlocks * 512U)));
		}

		// Token: 0x040001E6 RID: 486
		private readonly Stream _diskStream;

		// Token: 0x040001E7 RID: 487
		public uint BootBlock;

		// Token: 0x040001E8 RID: 488
		public uint BootBytes;

		// Token: 0x040001E9 RID: 489
		public uint Flags;

		// Token: 0x040001EA RID: 490
		public uint LogicalBlocks;

		// Token: 0x040001EB RID: 491
		public uint LogicalBlockStart;

		// Token: 0x040001EC RID: 492
		public uint MapEntries;

		// Token: 0x040001ED RID: 493
		public string Name;

		// Token: 0x040001EE RID: 494
		public uint PhysicalBlocks;

		// Token: 0x040001EF RID: 495
		public uint PhysicalBlockStart;

		// Token: 0x040001F0 RID: 496
		public ushort Signature;

		// Token: 0x040001F1 RID: 497
		public string Type;
	}
}
