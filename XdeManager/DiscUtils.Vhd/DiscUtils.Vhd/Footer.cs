using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x0200000D RID: 13
	internal class Footer
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000545C File Offset: 0x0000365C
		public Footer(Geometry geometry, long capacity, FileType type)
		{
			this.Cookie = "conectix";
			this.Features = 2U;
			this.FileFormatVersion = 65536U;
			this.DataOffset = -1L;
			this.Timestamp = DateTime.UtcNow;
			this.CreatorApp = "dutl";
			this.CreatorVersion = 393217U;
			this.CreatorHostOS = "Wi2k";
			this.OriginalSize = capacity;
			this.CurrentSize = capacity;
			this.Geometry = geometry;
			this.DiskType = type;
			this.UniqueId = Guid.NewGuid();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000054E8 File Offset: 0x000036E8
		public Footer(Footer toCopy)
		{
			this.Cookie = toCopy.Cookie;
			this.Features = toCopy.Features;
			this.FileFormatVersion = toCopy.FileFormatVersion;
			this.DataOffset = toCopy.DataOffset;
			this.Timestamp = toCopy.Timestamp;
			this.CreatorApp = toCopy.CreatorApp;
			this.CreatorVersion = toCopy.CreatorVersion;
			this.CreatorHostOS = toCopy.CreatorHostOS;
			this.OriginalSize = toCopy.OriginalSize;
			this.CurrentSize = toCopy.CurrentSize;
			this.Geometry = toCopy.Geometry;
			this.DiskType = toCopy.DiskType;
			this.Checksum = toCopy.Checksum;
			this.UniqueId = toCopy.UniqueId;
			this.SavedState = toCopy.SavedState;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000055AF File Offset: 0x000037AF
		private Footer()
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000055B7 File Offset: 0x000037B7
		public bool IsValid()
		{
			return this.Cookie == "conectix" && this.IsChecksumValid() && this.FileFormatVersion == 65536U;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000055E2 File Offset: 0x000037E2
		public bool IsChecksumValid()
		{
			return this.Checksum == this.CalculateChecksum();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000055F2 File Offset: 0x000037F2
		public uint UpdateChecksum()
		{
			this.Checksum = this.CalculateChecksum();
			return this.Checksum;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005608 File Offset: 0x00003808
		private uint CalculateChecksum()
		{
			Footer footer = new Footer(this);
			footer.Checksum = 0U;
			byte[] array = new byte[512];
			footer.ToBytes(array, 0);
			uint num = 0U;
			foreach (uint num2 in array)
			{
				num += num2;
			}
			return ~num;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005654 File Offset: 0x00003854
		public static Footer FromBytes(byte[] buffer, int offset)
		{
			return new Footer
			{
				Cookie = EndianUtilities.BytesToString(buffer, offset, 8),
				Features = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8),
				FileFormatVersion = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12),
				DataOffset = EndianUtilities.ToInt64BigEndian(buffer, offset + 16),
				Timestamp = Footer.EpochUtc.AddSeconds(EndianUtilities.ToUInt32BigEndian(buffer, offset + 24)),
				CreatorApp = EndianUtilities.BytesToString(buffer, offset + 28, 4),
				CreatorVersion = EndianUtilities.ToUInt32BigEndian(buffer, offset + 32),
				CreatorHostOS = EndianUtilities.BytesToString(buffer, offset + 36, 4),
				OriginalSize = EndianUtilities.ToInt64BigEndian(buffer, offset + 40),
				CurrentSize = EndianUtilities.ToInt64BigEndian(buffer, offset + 48),
				Geometry = new Geometry((int)EndianUtilities.ToUInt16BigEndian(buffer, offset + 56), (int)buffer[58], (int)buffer[59]),
				DiskType = (FileType)EndianUtilities.ToUInt32BigEndian(buffer, offset + 60),
				Checksum = EndianUtilities.ToUInt32BigEndian(buffer, offset + 64),
				UniqueId = EndianUtilities.ToGuidBigEndian(buffer, offset + 68),
				SavedState = buffer[84]
			};
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000576C File Offset: 0x0000396C
		public void ToBytes(byte[] buffer, int offset)
		{
			EndianUtilities.StringToBytes(this.Cookie, buffer, offset, 8);
			EndianUtilities.WriteBytesBigEndian(this.Features, buffer, offset + 8);
			EndianUtilities.WriteBytesBigEndian(this.FileFormatVersion, buffer, offset + 12);
			EndianUtilities.WriteBytesBigEndian(this.DataOffset, buffer, offset + 16);
			EndianUtilities.WriteBytesBigEndian((uint)(this.Timestamp - Footer.EpochUtc).TotalSeconds, buffer, offset + 24);
			EndianUtilities.StringToBytes(this.CreatorApp, buffer, offset + 28, 4);
			EndianUtilities.WriteBytesBigEndian(this.CreatorVersion, buffer, offset + 32);
			EndianUtilities.StringToBytes(this.CreatorHostOS, buffer, offset + 36, 4);
			EndianUtilities.WriteBytesBigEndian(this.OriginalSize, buffer, offset + 40);
			EndianUtilities.WriteBytesBigEndian(this.CurrentSize, buffer, offset + 48);
			EndianUtilities.WriteBytesBigEndian((ushort)this.Geometry.Cylinders, buffer, offset + 56);
			buffer[offset + 58] = (byte)this.Geometry.HeadsPerCylinder;
			buffer[offset + 59] = (byte)this.Geometry.SectorsPerTrack;
			EndianUtilities.WriteBytesBigEndian((uint)this.DiskType, buffer, offset + 60);
			EndianUtilities.WriteBytesBigEndian(this.Checksum, buffer, offset + 64);
			EndianUtilities.WriteBytesBigEndian(this.UniqueId, buffer, offset + 68);
			buffer[84] = this.SavedState;
			Array.Clear(buffer, 85, 427);
		}

		// Token: 0x04000039 RID: 57
		public const string FileCookie = "conectix";

		// Token: 0x0400003A RID: 58
		public const uint FeatureNone = 0U;

		// Token: 0x0400003B RID: 59
		public const uint FeatureTemporary = 1U;

		// Token: 0x0400003C RID: 60
		public const uint FeatureReservedMustBeSet = 2U;

		// Token: 0x0400003D RID: 61
		public const uint Version1 = 65536U;

		// Token: 0x0400003E RID: 62
		public const uint Version6Point1 = 393217U;

		// Token: 0x0400003F RID: 63
		public const string VirtualPCSig = "vpc ";

		// Token: 0x04000040 RID: 64
		public const string VirtualServerSig = "vs  ";

		// Token: 0x04000041 RID: 65
		public const uint VirtualPC2004Version = 327680U;

		// Token: 0x04000042 RID: 66
		public const uint VirtualServer2004Version = 65536U;

		// Token: 0x04000043 RID: 67
		public const string WindowsHostOS = "Wi2k";

		// Token: 0x04000044 RID: 68
		public const string MacHostOS = "Mac ";

		// Token: 0x04000045 RID: 69
		public const uint CylindersMask = 65535U;

		// Token: 0x04000046 RID: 70
		public const uint HeadsMask = 16711680U;

		// Token: 0x04000047 RID: 71
		public const uint SectorsMask = 4278190080U;

		// Token: 0x04000048 RID: 72
		public static readonly DateTime EpochUtc = new DateTime(2000, 1, 1, 0, 0, 0, 0);

		// Token: 0x04000049 RID: 73
		public uint Checksum;

		// Token: 0x0400004A RID: 74
		public string Cookie;

		// Token: 0x0400004B RID: 75
		public string CreatorApp;

		// Token: 0x0400004C RID: 76
		public string CreatorHostOS;

		// Token: 0x0400004D RID: 77
		public uint CreatorVersion;

		// Token: 0x0400004E RID: 78
		public long CurrentSize;

		// Token: 0x0400004F RID: 79
		public long DataOffset;

		// Token: 0x04000050 RID: 80
		public FileType DiskType;

		// Token: 0x04000051 RID: 81
		public uint Features;

		// Token: 0x04000052 RID: 82
		public uint FileFormatVersion;

		// Token: 0x04000053 RID: 83
		public Geometry Geometry;

		// Token: 0x04000054 RID: 84
		public long OriginalSize;

		// Token: 0x04000055 RID: 85
		public byte SavedState;

		// Token: 0x04000056 RID: 86
		public DateTime Timestamp;

		// Token: 0x04000057 RID: 87
		public Guid UniqueId;
	}
}
