using System;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004B RID: 75
	public sealed class BiosPartitionInfo : PartitionInfo
	{
		// Token: 0x0600030F RID: 783 RVA: 0x00006DF8 File Offset: 0x00004FF8
		internal BiosPartitionInfo(BiosPartitionTable table, BiosPartitionRecord record)
		{
			this._table = table;
			this._record = record;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00006E0E File Offset: 0x0000500E
		public override byte BiosType
		{
			get
			{
				return this._record.PartitionType;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00006E1B File Offset: 0x0000501B
		public ChsAddress End
		{
			get
			{
				return new ChsAddress((int)this._record.EndCylinder, (int)this._record.EndHead, (int)this._record.EndSector);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00006E43 File Offset: 0x00005043
		public override long FirstSector
		{
			get
			{
				return (long)((ulong)this._record.LBAStartAbsolute);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00006E51 File Offset: 0x00005051
		public override Guid GuidType
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00006E58 File Offset: 0x00005058
		public bool IsActive
		{
			get
			{
				return this._record.Status > 0;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00006E68 File Offset: 0x00005068
		public bool IsPrimary
		{
			get
			{
				return this.PrimaryIndex >= 0;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00006E76 File Offset: 0x00005076
		public override long LastSector
		{
			get
			{
				return (long)((ulong)(this._record.LBAStartAbsolute + this._record.LBALength - 1U));
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00006E92 File Offset: 0x00005092
		public int PrimaryIndex
		{
			get
			{
				return this._record.Index;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00006E9F File Offset: 0x0000509F
		public ChsAddress Start
		{
			get
			{
				return new ChsAddress((int)this._record.StartCylinder, (int)this._record.StartHead, (int)this._record.StartSector);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00006EC7 File Offset: 0x000050C7
		public override string TypeAsString
		{
			get
			{
				return this._record.FriendlyPartitionType;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00006ED4 File Offset: 0x000050D4
		internal override PhysicalVolumeType VolumeType
		{
			get
			{
				return PhysicalVolumeType.BiosPartition;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00006ED7 File Offset: 0x000050D7
		public override SparseStream Open()
		{
			return this._table.Open(this._record);
		}

		// Token: 0x040000AD RID: 173
		private readonly BiosPartitionRecord _record;

		// Token: 0x040000AE RID: 174
		private readonly BiosPartitionTable _table;
	}
}
