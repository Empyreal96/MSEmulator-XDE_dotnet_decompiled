using System;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000052 RID: 82
	public sealed class GuidPartitionInfo : PartitionInfo
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00008D3C File Offset: 0x00006F3C
		internal GuidPartitionInfo(GuidPartitionTable table, GptEntry entry)
		{
			this._table = table;
			this._entry = entry;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00008D52 File Offset: 0x00006F52
		public long Attributes
		{
			get
			{
				return (long)this._entry.Attributes;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00008D5F File Offset: 0x00006F5F
		public override byte BiosType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00008D62 File Offset: 0x00006F62
		public override long FirstSector
		{
			get
			{
				return this._entry.FirstUsedLogicalBlock;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00008D6F File Offset: 0x00006F6F
		public override Guid GuidType
		{
			get
			{
				return this._entry.PartitionType;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00008D7C File Offset: 0x00006F7C
		public Guid Identity
		{
			get
			{
				return this._entry.Identity;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00008D89 File Offset: 0x00006F89
		public override long LastSector
		{
			get
			{
				return this._entry.LastUsedLogicalBlock;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00008D96 File Offset: 0x00006F96
		public string Name
		{
			get
			{
				return this._entry.Name;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00008DA3 File Offset: 0x00006FA3
		public override string TypeAsString
		{
			get
			{
				return this._entry.FriendlyPartitionType;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00008DB0 File Offset: 0x00006FB0
		internal override PhysicalVolumeType VolumeType
		{
			get
			{
				return PhysicalVolumeType.GptPartition;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00008DB3 File Offset: 0x00006FB3
		public override SparseStream Open()
		{
			return this._table.Open(this._entry);
		}

		// Token: 0x040000E1 RID: 225
		private readonly GptEntry _entry;

		// Token: 0x040000E2 RID: 226
		private readonly GuidPartitionTable _table;
	}
}
