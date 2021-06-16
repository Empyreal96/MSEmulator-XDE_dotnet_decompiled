using System;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005A RID: 90
	internal sealed class ComponentRecord : DatabaseRecord
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x00009E90 File Offset: 0x00008090
		protected override void DoReadFrom(byte[] buffer, int offset)
		{
			base.DoReadFrom(buffer, offset);
			int num = offset + 24;
			this.Id = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Name = DatabaseRecord.ReadVarString(buffer, ref num);
			this.StatusString = DatabaseRecord.ReadVarString(buffer, ref num);
			this.MergeType = (ExtentMergeType)DatabaseRecord.ReadByte(buffer, ref num);
			this.Unknown1 = DatabaseRecord.ReadUInt(buffer, ref num);
			this.NumExtents = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Unknown2 = DatabaseRecord.ReadUInt(buffer, ref num);
			this.LinkId = DatabaseRecord.ReadUInt(buffer, ref num);
			this.Unknown3 = DatabaseRecord.ReadULong(buffer, ref num);
			this.VolumeId = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Unknown4 = DatabaseRecord.ReadVarULong(buffer, ref num);
			if ((this.Flags & 4096U) != 0U)
			{
				this.StripeSizeSectors = DatabaseRecord.ReadVarLong(buffer, ref num);
				this.StripeStride = DatabaseRecord.ReadVarLong(buffer, ref num);
			}
		}

		// Token: 0x040000F7 RID: 247
		public uint LinkId;

		// Token: 0x040000F8 RID: 248
		public ExtentMergeType MergeType;

		// Token: 0x040000F9 RID: 249
		public ulong NumExtents;

		// Token: 0x040000FA RID: 250
		public string StatusString;

		// Token: 0x040000FB RID: 251
		public long StripeSizeSectors;

		// Token: 0x040000FC RID: 252
		public long StripeStride;

		// Token: 0x040000FD RID: 253
		public uint Unknown1;

		// Token: 0x040000FE RID: 254
		public uint Unknown2;

		// Token: 0x040000FF RID: 255
		public ulong Unknown3;

		// Token: 0x04000100 RID: 256
		public ulong Unknown4;

		// Token: 0x04000101 RID: 257
		public ulong VolumeId;
	}
}
