using System;
using System.Globalization;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000049 RID: 73
	internal sealed class ReparsePointRecord : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000373 RID: 883 RVA: 0x000139F3 File Offset: 0x00011BF3
		public int Size
		{
			get
			{
				return 8 + this.Content.Length;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00013A00 File Offset: 0x00011C00
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Tag = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
			ushort num = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 4);
			this.Content = new byte[(int)num];
			Array.Copy(buffer, offset + 8, this.Content, 0, (int)num);
			return (int)(8 + num);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00013A44 File Offset: 0x00011C44
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.Tag, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian((ushort)this.Content.Length, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 6);
			Array.Copy(this.Content, 0, buffer, offset + 8, this.Content.Length);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00013A94 File Offset: 0x00011C94
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "                Tag: " + this.Tag.ToString("x", CultureInfo.InvariantCulture));
			string text = string.Empty;
			for (int i = 0; i < Math.Min(this.Content.Length, 32); i++)
			{
				text += string.Format(CultureInfo.InvariantCulture, " {0:X2}", new object[]
				{
					this.Content[i]
				});
			}
			writer.WriteLine(linePrefix + "               Data:" + text + ((this.Content.Length > 32) ? "..." : string.Empty));
		}

		// Token: 0x04000168 RID: 360
		public byte[] Content;

		// Token: 0x04000169 RID: 361
		public uint Tag;
	}
}
