using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200004D RID: 77
	internal sealed class SecurityDescriptorRecord : IByteArraySerializable
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0001427F File Offset: 0x0001247F
		public int Size
		{
			get
			{
				return this.SecurityDescriptor.Length + 20;
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001428C File Offset: 0x0001248C
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Read(buffer, offset);
			return this.SecurityDescriptor.Length + 20;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000142A4 File Offset: 0x000124A4
		public void WriteTo(byte[] buffer, int offset)
		{
			this.EntrySize = (uint)this.Size;
			EndianUtilities.WriteBytesLittleEndian(this.Hash, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.OffsetInFile, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.EntrySize, buffer, offset + 16);
			Array.Copy(this.SecurityDescriptor, 0, buffer, offset + 20, this.SecurityDescriptor.Length);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00014314 File Offset: 0x00012514
		public bool Read(byte[] buffer, int offset)
		{
			this.Hash = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
			this.Id = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
			this.OffsetInFile = EndianUtilities.ToInt64LittleEndian(buffer, offset + 8);
			this.EntrySize = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 16);
			if (this.EntrySize > 0U)
			{
				this.SecurityDescriptor = new byte[this.EntrySize - 20U];
				Array.Copy(buffer, offset + 20, this.SecurityDescriptor, 0, this.SecurityDescriptor.Length);
				return true;
			}
			return false;
		}

		// Token: 0x0400016F RID: 367
		public uint EntrySize;

		// Token: 0x04000170 RID: 368
		public uint Hash;

		// Token: 0x04000171 RID: 369
		public uint Id;

		// Token: 0x04000172 RID: 370
		public long OffsetInFile;

		// Token: 0x04000173 RID: 371
		public byte[] SecurityDescriptor;
	}
}
