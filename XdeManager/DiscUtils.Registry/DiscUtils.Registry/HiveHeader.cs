using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000005 RID: 5
	internal sealed class HiveHeader : IByteArraySerializable
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000027F4 File Offset: 0x000009F4
		public HiveHeader()
		{
			this.Sequence1 = 1;
			this.Sequence2 = 1;
			this.Timestamp = DateTime.UtcNow;
			this.MajorVersion = 1;
			this.MinorVersion = 3;
			this.RootCell = -1;
			this.Path = string.Empty;
			this.Guid1 = Guid.NewGuid();
			this.Guid2 = Guid.NewGuid();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002856 File Offset: 0x00000A56
		public int Size
		{
			get
			{
				return 512;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002860 File Offset: 0x00000A60
		public int ReadFrom(byte[] buffer, int offset)
		{
			if (EndianUtilities.ToUInt32LittleEndian(buffer, offset) != 1718052210U)
			{
				throw new IOException("Invalid signature for registry hive");
			}
			this.Sequence1 = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
			this.Sequence2 = EndianUtilities.ToInt32LittleEndian(buffer, offset + 8);
			this.Timestamp = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64LittleEndian(buffer, offset + 12));
			this.MajorVersion = EndianUtilities.ToInt32LittleEndian(buffer, 20);
			this.MinorVersion = EndianUtilities.ToInt32LittleEndian(buffer, 24);
			EndianUtilities.ToInt32LittleEndian(buffer, 28);
			this.RootCell = EndianUtilities.ToInt32LittleEndian(buffer, 36);
			this.Length = EndianUtilities.ToInt32LittleEndian(buffer, 40);
			this.Path = Encoding.Unicode.GetString(buffer, 48, 64).Trim(new char[1]);
			this.Guid1 = EndianUtilities.ToGuidLittleEndian(buffer, 112);
			this.Guid2 = EndianUtilities.ToGuidLittleEndian(buffer, 148);
			this.Checksum = EndianUtilities.ToUInt32LittleEndian(buffer, 508);
			if (this.Sequence1 != this.Sequence2)
			{
				throw new NotImplementedException("Support for replaying registry log file");
			}
			if (this.Checksum != HiveHeader.CalcChecksum(buffer, offset))
			{
				throw new IOException("Invalid checksum on registry file");
			}
			return 512;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002984 File Offset: 0x00000B84
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(1718052210U, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.Sequence1, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.Sequence2, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.Timestamp.ToFileTimeUtc(), buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian(this.MajorVersion, buffer, offset + 20);
			EndianUtilities.WriteBytesLittleEndian(this.MinorVersion, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(1U, buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this.RootCell, buffer, offset + 36);
			EndianUtilities.WriteBytesLittleEndian(this.Length, buffer, offset + 40);
			Encoding.Unicode.GetBytes(this.Path, 0, this.Path.Length, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 48 + this.Path.Length * 2);
			EndianUtilities.WriteBytesLittleEndian(this.Guid1, buffer, offset + 112);
			EndianUtilities.WriteBytesLittleEndian(this.Guid2, buffer, offset + 148);
			EndianUtilities.WriteBytesLittleEndian(HiveHeader.CalcChecksum(buffer, offset), buffer, offset + 508);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A90 File Offset: 0x00000C90
		private static uint CalcChecksum(byte[] buffer, int offset)
		{
			uint num = 0U;
			for (int i = 0; i < 508; i += 4)
			{
				num ^= EndianUtilities.ToUInt32LittleEndian(buffer, offset + i);
			}
			return num;
		}

		// Token: 0x0400000C RID: 12
		public const int HeaderSize = 512;

		// Token: 0x0400000D RID: 13
		private const uint Signature = 1718052210U;

		// Token: 0x0400000E RID: 14
		public uint Checksum;

		// Token: 0x0400000F RID: 15
		public Guid Guid1;

		// Token: 0x04000010 RID: 16
		public Guid Guid2;

		// Token: 0x04000011 RID: 17
		public int Length;

		// Token: 0x04000012 RID: 18
		public int MajorVersion;

		// Token: 0x04000013 RID: 19
		public int MinorVersion;

		// Token: 0x04000014 RID: 20
		public string Path;

		// Token: 0x04000015 RID: 21
		public int RootCell;

		// Token: 0x04000016 RID: 22
		public int Sequence1;

		// Token: 0x04000017 RID: 23
		public int Sequence2;

		// Token: 0x04000018 RID: 24
		public DateTime Timestamp;
	}
}
