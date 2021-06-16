using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200000E RID: 14
	internal sealed class FileParameters : IByteArraySerializable
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004769 File Offset: 0x00002969
		public int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000476C File Offset: 0x0000296C
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.BlockSize = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
			this.Flags = (FileParametersFlags)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
			return 8;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000478B File Offset: 0x0000298B
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.BlockSize, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Flags, buffer, offset + 4);
		}

		// Token: 0x04000038 RID: 56
		public const uint DefaultBlockSize = 33554432U;

		// Token: 0x04000039 RID: 57
		public const uint DefaultDifferencingBlockSize = 2097152U;

		// Token: 0x0400003A RID: 58
		public const uint DefaultDynamicBlockSize = 33554432U;

		// Token: 0x0400003B RID: 59
		public uint BlockSize;

		// Token: 0x0400003C RID: 60
		public FileParametersFlags Flags;
	}
}
