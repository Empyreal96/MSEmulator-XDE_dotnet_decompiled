using System;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200006A RID: 106
	internal sealed class VolumeRecord : DatabaseRecord
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		protected override void DoReadFrom(byte[] buffer, int offset)
		{
			base.DoReadFrom(buffer, offset);
			int num = offset + 24;
			this.Id = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Name = DatabaseRecord.ReadVarString(buffer, ref num);
			this.GenString = DatabaseRecord.ReadVarString(buffer, ref num);
			this.NumberString = DatabaseRecord.ReadVarString(buffer, ref num);
			this.ActiveString = DatabaseRecord.ReadString(buffer, 6, ref num);
			this.UnknownA = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.UnknownB = DatabaseRecord.ReadULong(buffer, ref num);
			this.DupCount = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.UnknownC = DatabaseRecord.ReadUInt(buffer, ref num);
			this.ComponentCount = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.UnknownD = DatabaseRecord.ReadUInt(buffer, ref num);
			this.PartitionComponentLink = DatabaseRecord.ReadUInt(buffer, ref num);
			this.Unknown1 = DatabaseRecord.ReadULong(buffer, ref num);
			this.Size = DatabaseRecord.ReadVarLong(buffer, ref num);
			this.Unknown2 = DatabaseRecord.ReadUInt(buffer, ref num);
			this.BiosType = DatabaseRecord.ReadByte(buffer, ref num);
			this.VolumeGuid = EndianUtilities.ToGuidBigEndian(buffer, num);
			num += 16;
			if ((this.Flags & 512U) != 0U)
			{
				this.MountHint = DatabaseRecord.ReadVarString(buffer, ref num);
			}
		}

		// Token: 0x04000169 RID: 361
		public string ActiveString;

		// Token: 0x0400016A RID: 362
		public byte BiosType;

		// Token: 0x0400016B RID: 363
		public ulong ComponentCount;

		// Token: 0x0400016C RID: 364
		public ulong DupCount;

		// Token: 0x0400016D RID: 365
		public string GenString;

		// Token: 0x0400016E RID: 366
		public string MountHint;

		// Token: 0x0400016F RID: 367
		public string NumberString;

		// Token: 0x04000170 RID: 368
		public uint PartitionComponentLink;

		// Token: 0x04000171 RID: 369
		public long Size;

		// Token: 0x04000172 RID: 370
		public ulong Unknown1;

		// Token: 0x04000173 RID: 371
		public uint Unknown2;

		// Token: 0x04000174 RID: 372
		public ulong UnknownA;

		// Token: 0x04000175 RID: 373
		public ulong UnknownB;

		// Token: 0x04000176 RID: 374
		public uint UnknownC;

		// Token: 0x04000177 RID: 375
		public uint UnknownD;

		// Token: 0x04000178 RID: 376
		public Guid VolumeGuid;
	}
}
