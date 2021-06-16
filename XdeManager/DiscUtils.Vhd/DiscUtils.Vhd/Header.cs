using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x0200000E RID: 14
	internal class Header
	{
		// Token: 0x060000AC RID: 172 RVA: 0x000058C3 File Offset: 0x00003AC3
		public static Header FromStream(Stream stream)
		{
			return Header.FromBytes(StreamUtilities.ReadExact(stream, 16), 0);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000058D3 File Offset: 0x00003AD3
		public static Header FromBytes(byte[] data, int offset)
		{
			return new Header
			{
				Cookie = EndianUtilities.BytesToString(data, offset, 8),
				DataOffset = EndianUtilities.ToInt64BigEndian(data, offset + 8)
			};
		}

		// Token: 0x04000058 RID: 88
		public string Cookie;

		// Token: 0x04000059 RID: 89
		public long DataOffset;
	}
}
