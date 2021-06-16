using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000013 RID: 19
	internal sealed class LogEntryHeader : IByteArraySerializable
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004CFD File Offset: 0x00002EFD
		public bool IsValid
		{
			get
			{
				return this.Signature == 1701277548U;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004D0C File Offset: 0x00002F0C
		public int Size
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004D10 File Offset: 0x00002F10
		public int ReadFrom(byte[] buffer, int offset)
		{
			this._data = new byte[this.Size];
			Array.Copy(buffer, offset, this._data, 0, this.Size);
			this.Signature = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
			this.Checksum = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
			this.EntryLength = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 8);
			this.Tail = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 12);
			this.SequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 16);
			this.DescriptorCount = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 24);
			this.Reserved = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 28);
			this.LogGuid = EndianUtilities.ToGuidLittleEndian(buffer, offset + 32);
			this.FlushedFileOffset = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 48);
			this.LastFileOffset = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 56);
			return this.Size;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004DE3 File Offset: 0x00002FE3
		public void WriteTo(byte[] buffer, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000048 RID: 72
		public const uint LogEntrySignature = 1701277548U;

		// Token: 0x04000049 RID: 73
		private byte[] _data;

		// Token: 0x0400004A RID: 74
		public uint Checksum;

		// Token: 0x0400004B RID: 75
		public uint DescriptorCount;

		// Token: 0x0400004C RID: 76
		public uint EntryLength;

		// Token: 0x0400004D RID: 77
		public ulong FlushedFileOffset;

		// Token: 0x0400004E RID: 78
		public ulong LastFileOffset;

		// Token: 0x0400004F RID: 79
		public Guid LogGuid;

		// Token: 0x04000050 RID: 80
		public uint Reserved;

		// Token: 0x04000051 RID: 81
		public ulong SequenceNumber;

		// Token: 0x04000052 RID: 82
		public uint Signature;

		// Token: 0x04000053 RID: 83
		public uint Tail;
	}
}
