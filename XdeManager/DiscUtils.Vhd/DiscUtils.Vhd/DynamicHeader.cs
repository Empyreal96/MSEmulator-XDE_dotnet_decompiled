using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000009 RID: 9
	internal class DynamicHeader
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00003970 File Offset: 0x00001B70
		public DynamicHeader()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003978 File Offset: 0x00001B78
		public DynamicHeader(long dataOffset, long tableOffset, uint blockSize, long diskSize)
		{
			this.Cookie = "cxsparse";
			this.DataOffset = dataOffset;
			this.TableOffset = tableOffset;
			this.HeaderVersion = 65536U;
			this.BlockSize = blockSize;
			this.MaxTableEntries = (int)((diskSize + (long)((ulong)blockSize) - 1L) / (long)((ulong)blockSize));
			this.ParentTimestamp = Footer.EpochUtc;
			this.ParentUnicodeName = string.Empty;
			this.ParentLocators = new ParentLocator[8];
			for (int i = 0; i < 8; i++)
			{
				this.ParentLocators[i] = new ParentLocator();
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003A04 File Offset: 0x00001C04
		public DynamicHeader(DynamicHeader toCopy)
		{
			this.Cookie = toCopy.Cookie;
			this.DataOffset = toCopy.DataOffset;
			this.TableOffset = toCopy.TableOffset;
			this.HeaderVersion = toCopy.HeaderVersion;
			this.MaxTableEntries = toCopy.MaxTableEntries;
			this.BlockSize = toCopy.BlockSize;
			this.Checksum = toCopy.Checksum;
			this.ParentUniqueId = toCopy.ParentUniqueId;
			this.ParentTimestamp = toCopy.ParentTimestamp;
			this.ParentUnicodeName = toCopy.ParentUnicodeName;
			this.ParentLocators = new ParentLocator[toCopy.ParentLocators.Length];
			for (int i = 0; i < this.ParentLocators.Length; i++)
			{
				this.ParentLocators[i] = new ParentLocator(toCopy.ParentLocators[i]);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003ACC File Offset: 0x00001CCC
		public static DynamicHeader FromBytes(byte[] data, int offset)
		{
			DynamicHeader dynamicHeader = new DynamicHeader();
			dynamicHeader.Cookie = EndianUtilities.BytesToString(data, offset, 8);
			dynamicHeader.DataOffset = EndianUtilities.ToInt64BigEndian(data, offset + 8);
			dynamicHeader.TableOffset = EndianUtilities.ToInt64BigEndian(data, offset + 16);
			dynamicHeader.HeaderVersion = EndianUtilities.ToUInt32BigEndian(data, offset + 24);
			dynamicHeader.MaxTableEntries = EndianUtilities.ToInt32BigEndian(data, offset + 28);
			dynamicHeader.BlockSize = EndianUtilities.ToUInt32BigEndian(data, offset + 32);
			dynamicHeader.Checksum = EndianUtilities.ToUInt32BigEndian(data, offset + 36);
			dynamicHeader.ParentUniqueId = EndianUtilities.ToGuidBigEndian(data, offset + 40);
			dynamicHeader.ParentTimestamp = Footer.EpochUtc.AddSeconds(EndianUtilities.ToUInt32BigEndian(data, offset + 56));
			dynamicHeader.ParentUnicodeName = Encoding.BigEndianUnicode.GetString(data, offset + 64, 512).TrimEnd(new char[1]);
			dynamicHeader.ParentLocators = new ParentLocator[8];
			for (int i = 0; i < 8; i++)
			{
				dynamicHeader.ParentLocators[i] = ParentLocator.FromBytes(data, offset + 576 + i * 24);
			}
			return dynamicHeader;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public void ToBytes(byte[] data, int offset)
		{
			EndianUtilities.StringToBytes(this.Cookie, data, offset, 8);
			EndianUtilities.WriteBytesBigEndian(this.DataOffset, data, offset + 8);
			EndianUtilities.WriteBytesBigEndian(this.TableOffset, data, offset + 16);
			EndianUtilities.WriteBytesBigEndian(this.HeaderVersion, data, offset + 24);
			EndianUtilities.WriteBytesBigEndian(this.MaxTableEntries, data, offset + 28);
			EndianUtilities.WriteBytesBigEndian(this.BlockSize, data, offset + 32);
			EndianUtilities.WriteBytesBigEndian(this.Checksum, data, offset + 36);
			EndianUtilities.WriteBytesBigEndian(this.ParentUniqueId, data, offset + 40);
			EndianUtilities.WriteBytesBigEndian((uint)(this.ParentTimestamp - Footer.EpochUtc).TotalSeconds, data, offset + 56);
			EndianUtilities.WriteBytesBigEndian(0U, data, offset + 60);
			Array.Clear(data, offset + 64, 512);
			Encoding.BigEndianUnicode.GetBytes(this.ParentUnicodeName, 0, this.ParentUnicodeName.Length, data, offset + 64);
			for (int i = 0; i < 8; i++)
			{
				this.ParentLocators[i].ToBytes(data, offset + 576 + i * 24);
			}
			Array.Clear(data, offset + 1024 - 256, 256);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003CFB File Offset: 0x00001EFB
		public bool IsValid()
		{
			return this.Cookie == "cxsparse" && this.IsChecksumValid() && this.HeaderVersion == 65536U;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003D26 File Offset: 0x00001F26
		public bool IsChecksumValid()
		{
			return this.Checksum == this.CalculateChecksum();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003D36 File Offset: 0x00001F36
		public uint UpdateChecksum()
		{
			this.Checksum = this.CalculateChecksum();
			return this.Checksum;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003D4A File Offset: 0x00001F4A
		internal static DynamicHeader FromStream(Stream stream)
		{
			return DynamicHeader.FromBytes(StreamUtilities.ReadExact(stream, 1024), 0);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003D60 File Offset: 0x00001F60
		private uint CalculateChecksum()
		{
			DynamicHeader dynamicHeader = new DynamicHeader(this);
			dynamicHeader.Checksum = 0U;
			byte[] array = new byte[1024];
			dynamicHeader.ToBytes(array, 0);
			uint num = 0U;
			foreach (uint num2 in array)
			{
				num += num2;
			}
			return ~num;
		}

		// Token: 0x04000011 RID: 17
		public const string HeaderCookie = "cxsparse";

		// Token: 0x04000012 RID: 18
		public const uint Version1 = 65536U;

		// Token: 0x04000013 RID: 19
		public const uint DefaultBlockSize = 2097152U;

		// Token: 0x04000014 RID: 20
		public uint BlockSize;

		// Token: 0x04000015 RID: 21
		public uint Checksum;

		// Token: 0x04000016 RID: 22
		public string Cookie;

		// Token: 0x04000017 RID: 23
		public long DataOffset;

		// Token: 0x04000018 RID: 24
		public uint HeaderVersion;

		// Token: 0x04000019 RID: 25
		public int MaxTableEntries;

		// Token: 0x0400001A RID: 26
		public ParentLocator[] ParentLocators;

		// Token: 0x0400001B RID: 27
		public DateTime ParentTimestamp;

		// Token: 0x0400001C RID: 28
		public string ParentUnicodeName;

		// Token: 0x0400001D RID: 29
		public Guid ParentUniqueId;

		// Token: 0x0400001E RID: 30
		public long TableOffset;
	}
}
