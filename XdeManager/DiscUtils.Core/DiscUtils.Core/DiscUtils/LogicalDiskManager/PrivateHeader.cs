using System;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000067 RID: 103
	internal class PrivateHeader
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x0000BD04 File Offset: 0x00009F04
		public void ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.BytesToString(buffer, offset, 8);
			this.Checksum = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
			this.Version = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
			this.Timestamp = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64BigEndian(buffer, offset + 16));
			this.Unknown2 = EndianUtilities.ToInt64BigEndian(buffer, offset + 24);
			this.Unknown3 = EndianUtilities.ToInt64BigEndian(buffer, offset + 32);
			this.Unknown4 = EndianUtilities.ToInt64BigEndian(buffer, offset + 40);
			this.DiskId = EndianUtilities.BytesToString(buffer, offset + 48, 64).Trim(new char[1]);
			this.HostId = EndianUtilities.BytesToString(buffer, offset + 112, 64).Trim(new char[1]);
			this.DiskGroupId = EndianUtilities.BytesToString(buffer, offset + 176, 64).Trim(new char[1]);
			this.DiskGroupName = EndianUtilities.BytesToString(buffer, offset + 240, 31).Trim(new char[1]);
			this.Unknown5 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 271);
			this.DataStartLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 283);
			this.DataSizeLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 291);
			this.ConfigurationStartLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 299);
			this.ConfigurationSizeLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 307);
			this.TocSizeLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 315);
			this.NextTocLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 323);
			this.NumberOfConfigs = (long)EndianUtilities.ToInt32BigEndian(buffer, offset + 331);
			this.NumberOfLogs = (long)EndianUtilities.ToInt32BigEndian(buffer, offset + 335);
			this.ConfigSizeLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 339);
			this.LogSizeLba = EndianUtilities.ToInt64BigEndian(buffer, offset + 347);
		}

		// Token: 0x0400013D RID: 317
		public uint Checksum;

		// Token: 0x0400013E RID: 318
		public long ConfigSizeLba;

		// Token: 0x0400013F RID: 319
		public long ConfigurationSizeLba;

		// Token: 0x04000140 RID: 320
		public long ConfigurationStartLba;

		// Token: 0x04000141 RID: 321
		public long DataSizeLba;

		// Token: 0x04000142 RID: 322
		public long DataStartLba;

		// Token: 0x04000143 RID: 323
		public string DiskGroupId;

		// Token: 0x04000144 RID: 324
		public string DiskGroupName;

		// Token: 0x04000145 RID: 325
		public string DiskId;

		// Token: 0x04000146 RID: 326
		public string HostId;

		// Token: 0x04000147 RID: 327
		public long LogSizeLba;

		// Token: 0x04000148 RID: 328
		public long NextTocLba;

		// Token: 0x04000149 RID: 329
		public long NumberOfConfigs;

		// Token: 0x0400014A RID: 330
		public long NumberOfLogs;

		// Token: 0x0400014B RID: 331
		public string Signature;

		// Token: 0x0400014C RID: 332
		public DateTime Timestamp;

		// Token: 0x0400014D RID: 333
		public long TocSizeLba;

		// Token: 0x0400014E RID: 334
		public long Unknown2;

		// Token: 0x0400014F RID: 335
		public long Unknown3;

		// Token: 0x04000150 RID: 336
		public long Unknown4;

		// Token: 0x04000151 RID: 337
		public uint Unknown5;

		// Token: 0x04000152 RID: 338
		public uint Version;
	}
}
