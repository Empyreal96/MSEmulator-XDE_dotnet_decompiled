using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x0200000F RID: 15
	internal class ParentLocator
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000058FF File Offset: 0x00003AFF
		public ParentLocator()
		{
			this.PlatformCode = string.Empty;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005912 File Offset: 0x00003B12
		public ParentLocator(ParentLocator toCopy)
		{
			this.PlatformCode = toCopy.PlatformCode;
			this.PlatformDataSpace = toCopy.PlatformDataSpace;
			this.PlatformDataLength = toCopy.PlatformDataLength;
			this.PlatformDataOffset = toCopy.PlatformDataOffset;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000594C File Offset: 0x00003B4C
		public static ParentLocator FromBytes(byte[] data, int offset)
		{
			return new ParentLocator
			{
				PlatformCode = EndianUtilities.BytesToString(data, offset, 4),
				PlatformDataSpace = EndianUtilities.ToInt32BigEndian(data, offset + 4),
				PlatformDataLength = EndianUtilities.ToInt32BigEndian(data, offset + 8),
				PlatformDataOffset = EndianUtilities.ToInt64BigEndian(data, offset + 16)
			};
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000599C File Offset: 0x00003B9C
		internal void ToBytes(byte[] data, int offset)
		{
			EndianUtilities.StringToBytes(this.PlatformCode, data, offset, 4);
			EndianUtilities.WriteBytesBigEndian(this.PlatformDataSpace, data, offset + 4);
			EndianUtilities.WriteBytesBigEndian(this.PlatformDataLength, data, offset + 8);
			EndianUtilities.WriteBytesBigEndian(0U, data, offset + 12);
			EndianUtilities.WriteBytesBigEndian(this.PlatformDataOffset, data, offset + 16);
		}

		// Token: 0x0400005A RID: 90
		public const string PlatformCodeWindowsRelativeUnicode = "W2ru";

		// Token: 0x0400005B RID: 91
		public const string PlatformCodeWindowsAbsoluteUnicode = "W2ku";

		// Token: 0x0400005C RID: 92
		public string PlatformCode;

		// Token: 0x0400005D RID: 93
		public int PlatformDataLength;

		// Token: 0x0400005E RID: 94
		public long PlatformDataOffset;

		// Token: 0x0400005F RID: 95
		public int PlatformDataSpace;
	}
}
