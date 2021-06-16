using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000017 RID: 23
	internal sealed class MetadataEntry : IByteArraySerializable
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000052CB File Offset: 0x000034CB
		public int Size
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000052D0 File Offset: 0x000034D0
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.ItemId = EndianUtilities.ToGuidLittleEndian(buffer, offset);
			this.Offset = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 16);
			this.Length = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 20);
			this.Flags = (MetadataEntryFlags)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 24);
			this.Reserved = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 28);
			return 32;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000532C File Offset: 0x0000352C
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.ItemId, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.Offset, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.Length, buffer, offset + 20);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Flags, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(this.Reserved, buffer, offset + 28);
		}

		// Token: 0x0400005D RID: 93
		public MetadataEntryFlags Flags;

		// Token: 0x0400005E RID: 94
		public Guid ItemId;

		// Token: 0x0400005F RID: 95
		public uint Length;

		// Token: 0x04000060 RID: 96
		public uint Offset;

		// Token: 0x04000061 RID: 97
		public uint Reserved;
	}
}
