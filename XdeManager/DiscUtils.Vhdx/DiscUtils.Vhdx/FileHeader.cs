using System;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200000D RID: 13
	internal sealed class FileHeader : IByteArraySerializable
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000046C2 File Offset: 0x000028C2
		public bool IsValid
		{
			get
			{
				return this.Signature == 7308332184142899318UL;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000046D5 File Offset: 0x000028D5
		public int Size
		{
			get
			{
				return 65536;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000046DC File Offset: 0x000028DC
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.ToUInt64LittleEndian(buffer, offset);
			this.Creator = Encoding.Unicode.GetString(buffer, offset + 8, 512).TrimEnd(new char[1]);
			return this.Size;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004715 File Offset: 0x00002915
		public void WriteTo(byte[] buffer, int offset)
		{
			Array.Clear(buffer, offset, this.Size);
			EndianUtilities.WriteBytesLittleEndian(this.Signature, buffer, offset);
			Encoding.Unicode.GetBytes(this.Creator, 0, this.Creator.Length, buffer, offset + 8);
		}

		// Token: 0x04000035 RID: 53
		public const ulong VhdxSignature = 7308332184142899318UL;

		// Token: 0x04000036 RID: 54
		public string Creator;

		// Token: 0x04000037 RID: 55
		public ulong Signature = 7308332184142899318UL;
	}
}
