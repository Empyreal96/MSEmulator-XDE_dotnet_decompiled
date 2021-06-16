using System;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000024 RID: 36
	internal sealed class VhdxHeader : IByteArraySerializable
	{
		// Token: 0x06000129 RID: 297 RVA: 0x000060B1 File Offset: 0x000042B1
		public VhdxHeader()
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000060D4 File Offset: 0x000042D4
		public VhdxHeader(VhdxHeader header)
		{
			Array.Copy(header._data, this._data, 4096);
			this.Signature = header.Signature;
			this.Checksum = header.Checksum;
			this.SequenceNumber = header.SequenceNumber;
			this.FileWriteGuid = header.FileWriteGuid;
			this.DataWriteGuid = header.DataWriteGuid;
			this.LogGuid = header.LogGuid;
			this.LogVersion = header.LogVersion;
			this.Version = header.Version;
			this.LogLength = header.LogLength;
			this.LogOffset = header.LogOffset;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006190 File Offset: 0x00004390
		public bool IsValid
		{
			get
			{
				if (this.Signature != 1684104552U)
				{
					return false;
				}
				byte[] array = new byte[4096];
				Array.Copy(this._data, array, 4096);
				EndianUtilities.WriteBytesLittleEndian(0U, array, 4);
				return this.Checksum == Crc32LittleEndian.Compute(Crc32Algorithm.Castagnoli, array, 0, 4096);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000061E5 File Offset: 0x000043E5
		public int Size
		{
			get
			{
				return 4096;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000061EC File Offset: 0x000043EC
		public int ReadFrom(byte[] buffer, int offset)
		{
			Array.Copy(buffer, offset, this._data, 0, 4096);
			this.Signature = EndianUtilities.ToUInt32LittleEndian(this._data, 0);
			this.Checksum = EndianUtilities.ToUInt32LittleEndian(this._data, 4);
			this.SequenceNumber = EndianUtilities.ToUInt64LittleEndian(this._data, 8);
			this.FileWriteGuid = EndianUtilities.ToGuidLittleEndian(this._data, 16);
			this.DataWriteGuid = EndianUtilities.ToGuidLittleEndian(this._data, 32);
			this.LogGuid = EndianUtilities.ToGuidLittleEndian(this._data, 48);
			this.LogVersion = EndianUtilities.ToUInt16LittleEndian(this._data, 64);
			this.Version = EndianUtilities.ToUInt16LittleEndian(this._data, 66);
			this.LogLength = EndianUtilities.ToUInt32LittleEndian(this._data, 68);
			this.LogOffset = EndianUtilities.ToUInt64LittleEndian(this._data, 72);
			return this.Size;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000062CD File Offset: 0x000044CD
		public void WriteTo(byte[] buffer, int offset)
		{
			this.RefreshData();
			Array.Copy(this._data, 0, buffer, offset, 4096);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000062E8 File Offset: 0x000044E8
		public void CalcChecksum()
		{
			this.Checksum = 0U;
			this.RefreshData();
			this.Checksum = Crc32LittleEndian.Compute(Crc32Algorithm.Castagnoli, this._data, 0, 4096);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006310 File Offset: 0x00004510
		private void RefreshData()
		{
			EndianUtilities.WriteBytesLittleEndian(this.Signature, this._data, 0);
			EndianUtilities.WriteBytesLittleEndian(this.Checksum, this._data, 4);
			EndianUtilities.WriteBytesLittleEndian(this.SequenceNumber, this._data, 8);
			EndianUtilities.WriteBytesLittleEndian(this.FileWriteGuid, this._data, 16);
			EndianUtilities.WriteBytesLittleEndian(this.DataWriteGuid, this._data, 32);
			EndianUtilities.WriteBytesLittleEndian(this.LogGuid, this._data, 48);
			EndianUtilities.WriteBytesLittleEndian(this.LogVersion, this._data, 64);
			EndianUtilities.WriteBytesLittleEndian(this.Version, this._data, 66);
			EndianUtilities.WriteBytesLittleEndian(this.LogLength, this._data, 68);
			EndianUtilities.WriteBytesLittleEndian(this.LogOffset, this._data, 72);
		}

		// Token: 0x04000099 RID: 153
		public const uint VhdxHeaderSignature = 1684104552U;

		// Token: 0x0400009A RID: 154
		private readonly byte[] _data = new byte[4096];

		// Token: 0x0400009B RID: 155
		public uint Checksum;

		// Token: 0x0400009C RID: 156
		public Guid DataWriteGuid;

		// Token: 0x0400009D RID: 157
		public Guid FileWriteGuid;

		// Token: 0x0400009E RID: 158
		public Guid LogGuid;

		// Token: 0x0400009F RID: 159
		public uint LogLength;

		// Token: 0x040000A0 RID: 160
		public ulong LogOffset;

		// Token: 0x040000A1 RID: 161
		public ushort LogVersion;

		// Token: 0x040000A2 RID: 162
		public ulong SequenceNumber;

		// Token: 0x040000A3 RID: 163
		public uint Signature = 1684104552U;

		// Token: 0x040000A4 RID: 164
		public ushort Version;
	}
}
