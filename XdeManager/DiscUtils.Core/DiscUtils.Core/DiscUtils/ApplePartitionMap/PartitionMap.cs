using System;
using System.Collections.ObjectModel;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.ApplePartitionMap
{
	// Token: 0x02000092 RID: 146
	public sealed class PartitionMap : PartitionTable
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		public PartitionMap(Stream stream)
		{
			this._stream = stream;
			stream.Position = 0L;
			byte[] buffer = StreamUtilities.ReadExact(stream, 1024);
			new BlockZero().ReadFrom(buffer, 0);
			PartitionMapEntry partitionMapEntry = new PartitionMapEntry(this._stream);
			partitionMapEntry.ReadFrom(buffer, 512);
			byte[] buffer2 = StreamUtilities.ReadExact(stream, (int)((partitionMapEntry.MapEntries - 1U) * 512U));
			this._partitions = new PartitionMapEntry[partitionMapEntry.MapEntries - 1U];
			for (uint num = 0U; num < partitionMapEntry.MapEntries - 1U; num += 1U)
			{
				this._partitions[(int)num] = new PartitionMapEntry(this._stream);
				this._partitions[(int)num].ReadFrom(buffer2, (int)(512U * num));
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
		public override Guid DiskGuid
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0000EACB File Offset: 0x0000CCCB
		public override ReadOnlyCollection<PartitionInfo> Partitions
		{
			get
			{
				return new ReadOnlyCollection<PartitionInfo>(this._partitions);
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
		public override int Create(WellKnownPartitionType type, bool active)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000EADF File Offset: 0x0000CCDF
		public override int Create(long size, WellKnownPartitionType type, bool active)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000EAE6 File Offset: 0x0000CCE6
		public override int CreateAligned(WellKnownPartitionType type, bool active, int alignment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000EAED File Offset: 0x0000CCED
		public override int CreateAligned(long size, WellKnownPartitionType type, bool active, int alignment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		public override void Delete(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040001E4 RID: 484
		private readonly PartitionMapEntry[] _partitions;

		// Token: 0x040001E5 RID: 485
		private readonly Stream _stream;
	}
}
