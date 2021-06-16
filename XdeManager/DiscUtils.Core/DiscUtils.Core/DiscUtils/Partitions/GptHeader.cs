using System;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000051 RID: 81
	internal class GptHeader
	{
		// Token: 0x06000360 RID: 864 RVA: 0x000089A4 File Offset: 0x00006BA4
		public GptHeader(int sectorSize)
		{
			this.Signature = "EFI PART";
			this.Version = 65536U;
			this.HeaderSize = 92;
			this.Buffer = new byte[sectorSize];
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000089D8 File Offset: 0x00006BD8
		public GptHeader(GptHeader toCopy)
		{
			this.Signature = toCopy.Signature;
			this.Version = toCopy.Version;
			this.HeaderSize = toCopy.HeaderSize;
			this.Crc = toCopy.Crc;
			this.HeaderLba = toCopy.HeaderLba;
			this.AlternateHeaderLba = toCopy.AlternateHeaderLba;
			this.FirstUsable = toCopy.FirstUsable;
			this.LastUsable = toCopy.LastUsable;
			this.DiskGuid = toCopy.DiskGuid;
			this.PartitionEntriesLba = toCopy.PartitionEntriesLba;
			this.PartitionEntryCount = toCopy.PartitionEntryCount;
			this.PartitionEntrySize = toCopy.PartitionEntrySize;
			this.EntriesCrc = toCopy.EntriesCrc;
			this.Buffer = new byte[toCopy.Buffer.Length];
			Array.Copy(toCopy.Buffer, this.Buffer, this.Buffer.Length);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00008AB4 File Offset: 0x00006CB4
		public bool ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.BytesToString(buffer, offset, 8);
			this.Version = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 8);
			this.HeaderSize = EndianUtilities.ToInt32LittleEndian(buffer, offset + 12);
			this.Crc = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 16);
			this.HeaderLba = EndianUtilities.ToInt64LittleEndian(buffer, offset + 24);
			this.AlternateHeaderLba = EndianUtilities.ToInt64LittleEndian(buffer, offset + 32);
			this.FirstUsable = EndianUtilities.ToInt64LittleEndian(buffer, offset + 40);
			this.LastUsable = EndianUtilities.ToInt64LittleEndian(buffer, offset + 48);
			this.DiskGuid = EndianUtilities.ToGuidLittleEndian(buffer, offset + 56);
			this.PartitionEntriesLba = EndianUtilities.ToInt64LittleEndian(buffer, offset + 72);
			this.PartitionEntryCount = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 80);
			this.PartitionEntrySize = EndianUtilities.ToInt32LittleEndian(buffer, offset + 84);
			this.EntriesCrc = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 88);
			this.Buffer = new byte[this.HeaderSize];
			Array.Copy(buffer, offset, this.Buffer, 0, this.HeaderSize);
			return !(this.Signature != "EFI PART") && this.HeaderSize != 0 && this.Crc == GptHeader.CalcCrc(buffer, offset, this.HeaderSize);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00008BE4 File Offset: 0x00006DE4
		public void WriteTo(byte[] buffer, int offset)
		{
			Array.Copy(this.Buffer, 0, buffer, offset, this.Buffer.Length);
			EndianUtilities.StringToBytes(this.Signature, buffer, offset, 8);
			EndianUtilities.WriteBytesLittleEndian(this.Version, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.HeaderSize, buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian(0U, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.HeaderLba, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(this.AlternateHeaderLba, buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this.FirstUsable, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this.LastUsable, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian(this.DiskGuid, buffer, offset + 56);
			EndianUtilities.WriteBytesLittleEndian(this.PartitionEntriesLba, buffer, offset + 72);
			EndianUtilities.WriteBytesLittleEndian(this.PartitionEntryCount, buffer, offset + 80);
			EndianUtilities.WriteBytesLittleEndian(this.PartitionEntrySize, buffer, offset + 84);
			EndianUtilities.WriteBytesLittleEndian(this.EntriesCrc, buffer, offset + 88);
			EndianUtilities.WriteBytesLittleEndian(GptHeader.CalcCrc(buffer, offset, this.HeaderSize), buffer, offset + 16);
			this.Buffer = new byte[this.HeaderSize];
			Array.Copy(buffer, offset, this.Buffer, 0, this.HeaderSize);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00008D0C File Offset: 0x00006F0C
		internal static uint CalcCrc(byte[] buffer, int offset, int count)
		{
			byte[] array = new byte[count];
			Array.Copy(buffer, offset, array, 0, count);
			EndianUtilities.WriteBytesLittleEndian(0U, array, 16);
			return Crc32LittleEndian.Compute(Crc32Algorithm.Common, array, 0, count);
		}

		// Token: 0x040000D2 RID: 210
		public const string GptSignature = "EFI PART";

		// Token: 0x040000D3 RID: 211
		public long AlternateHeaderLba;

		// Token: 0x040000D4 RID: 212
		public byte[] Buffer;

		// Token: 0x040000D5 RID: 213
		public uint Crc;

		// Token: 0x040000D6 RID: 214
		public Guid DiskGuid;

		// Token: 0x040000D7 RID: 215
		public uint EntriesCrc;

		// Token: 0x040000D8 RID: 216
		public long FirstUsable;

		// Token: 0x040000D9 RID: 217
		public long HeaderLba;

		// Token: 0x040000DA RID: 218
		public int HeaderSize;

		// Token: 0x040000DB RID: 219
		public long LastUsable;

		// Token: 0x040000DC RID: 220
		public long PartitionEntriesLba;

		// Token: 0x040000DD RID: 221
		public uint PartitionEntryCount;

		// Token: 0x040000DE RID: 222
		public int PartitionEntrySize;

		// Token: 0x040000DF RID: 223
		public string Signature;

		// Token: 0x040000E0 RID: 224
		public uint Version;
	}
}
