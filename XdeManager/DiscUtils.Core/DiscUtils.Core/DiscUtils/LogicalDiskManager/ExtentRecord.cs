using System;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000066 RID: 102
	internal sealed class ExtentRecord : DatabaseRecord
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x0000BC38 File Offset: 0x00009E38
		protected override void DoReadFrom(byte[] buffer, int offset)
		{
			base.DoReadFrom(buffer, offset);
			int num = offset + 24;
			this.Id = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Name = DatabaseRecord.ReadVarString(buffer, ref num);
			this.Unknown1 = DatabaseRecord.ReadUInt(buffer, ref num);
			this.Unknown2 = DatabaseRecord.ReadUInt(buffer, ref num);
			this.PartitionComponentLink = DatabaseRecord.ReadUInt(buffer, ref num);
			this.DiskOffsetLba = DatabaseRecord.ReadLong(buffer, ref num);
			this.OffsetInVolumeLba = DatabaseRecord.ReadLong(buffer, ref num);
			this.SizeLba = DatabaseRecord.ReadVarLong(buffer, ref num);
			this.ComponentId = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.DiskId = DatabaseRecord.ReadVarULong(buffer, ref num);
			if ((this.Flags & 2048U) != 0U)
			{
				this.InterleaveOrder = DatabaseRecord.ReadVarULong(buffer, ref num);
			}
		}

		// Token: 0x04000134 RID: 308
		public ulong ComponentId;

		// Token: 0x04000135 RID: 309
		public ulong DiskId;

		// Token: 0x04000136 RID: 310
		public long DiskOffsetLba;

		// Token: 0x04000137 RID: 311
		public ulong InterleaveOrder;

		// Token: 0x04000138 RID: 312
		public long OffsetInVolumeLba;

		// Token: 0x04000139 RID: 313
		public uint PartitionComponentLink;

		// Token: 0x0400013A RID: 314
		public long SizeLba;

		// Token: 0x0400013B RID: 315
		public uint Unknown1;

		// Token: 0x0400013C RID: 316
		public uint Unknown2;
	}
}
