using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200002C RID: 44
	internal class IndexHeader
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x000092E4 File Offset: 0x000074E4
		public IndexHeader(uint allocatedSize)
		{
			this.AllocatedSizeOfEntries = allocatedSize;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000092F3 File Offset: 0x000074F3
		public IndexHeader(byte[] data, int offset)
		{
			this.OffsetToFirstEntry = EndianUtilities.ToUInt32LittleEndian(data, offset);
			this.TotalSizeOfEntries = EndianUtilities.ToUInt32LittleEndian(data, offset + 4);
			this.AllocatedSizeOfEntries = EndianUtilities.ToUInt32LittleEndian(data, offset + 8);
			this.HasChildNodes = data[offset + 12];
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009334 File Offset: 0x00007534
		internal void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.OffsetToFirstEntry, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.TotalSizeOfEntries, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.AllocatedSizeOfEntries, buffer, offset + 8);
			buffer[offset + 12] = this.HasChildNodes;
			buffer[offset + 13] = 0;
			buffer[offset + 14] = 0;
			buffer[offset + 15] = 0;
		}

		// Token: 0x040000D5 RID: 213
		public const int Size = 16;

		// Token: 0x040000D6 RID: 214
		public uint AllocatedSizeOfEntries;

		// Token: 0x040000D7 RID: 215
		public byte HasChildNodes;

		// Token: 0x040000D8 RID: 216
		public uint OffsetToFirstEntry;

		// Token: 0x040000D9 RID: 217
		public uint TotalSizeOfEntries;
	}
}
