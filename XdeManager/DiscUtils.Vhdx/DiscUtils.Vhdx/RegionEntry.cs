using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001F RID: 31
	internal sealed class RegionEntry : IByteArraySerializable
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005BA6 File Offset: 0x00003DA6
		public int Size
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005BAC File Offset: 0x00003DAC
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Guid = EndianUtilities.ToGuidLittleEndian(buffer, offset);
			this.FileOffset = EndianUtilities.ToInt64LittleEndian(buffer, offset + 16);
			this.Length = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 24);
			this.Flags = (RegionFlags)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 28);
			return 32;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.Guid, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.FileOffset, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.Length, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Flags, buffer, offset + 28);
		}

		// Token: 0x04000084 RID: 132
		public static readonly Guid BatGuid = new Guid("2dc27766-f623-4200-9d64-115e9bfd4a08");

		// Token: 0x04000085 RID: 133
		public static readonly Guid MetadataRegionGuid = new Guid("8b7ca206-4790-4b9a-b8fe-575f050f886e");

		// Token: 0x04000086 RID: 134
		public long FileOffset;

		// Token: 0x04000087 RID: 135
		public RegionFlags Flags;

		// Token: 0x04000088 RID: 136
		public Guid Guid;

		// Token: 0x04000089 RID: 137
		public uint Length;
	}
}
