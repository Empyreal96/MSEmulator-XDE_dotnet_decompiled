using System;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005C RID: 92
	internal class DatabaseHeader
	{
		// Token: 0x060003BE RID: 958 RVA: 0x0000A1FC File Offset: 0x000083FC
		public void ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.BytesToString(buffer, offset, 4);
			this.NumVBlks = EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
			this.BlockSize = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
			this.HeaderSize = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
			this.Unknown1 = EndianUtilities.ToUInt16BigEndian(buffer, offset + 16);
			this.VersionNum = EndianUtilities.ToUInt16BigEndian(buffer, offset + 18);
			this.VersionDenom = EndianUtilities.ToUInt16BigEndian(buffer, offset + 20);
			this.GroupName = EndianUtilities.BytesToString(buffer, offset + 22, 31).Trim(new char[1]);
			this.DiskGroupId = EndianUtilities.BytesToString(buffer, offset + 53, 64).Trim(new char[1]);
			this.CommittedSequence = EndianUtilities.ToInt64BigEndian(buffer, offset + 117);
			this.PendingSequence = EndianUtilities.ToInt64BigEndian(buffer, offset + 125);
			this.Unknown2 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 133);
			this.Unknown3 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 137);
			this.Unknown4 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 141);
			this.Unknown5 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 145);
			this.Unknown6 = EndianUtilities.ToInt64BigEndian(buffer, offset + 149);
			this.Unknown7 = EndianUtilities.ToInt64BigEndian(buffer, offset + 157);
			this.Unknown8 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 165);
			this.Unknown9 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 169);
			this.UnknownA = EndianUtilities.ToUInt32BigEndian(buffer, offset + 173);
			this.UnknownB = EndianUtilities.ToInt64BigEndian(buffer, offset + 177);
			this.UnknownC = EndianUtilities.ToUInt32BigEndian(buffer, offset + 185);
			this.Timestamp = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64BigEndian(buffer, offset + 189));
		}

		// Token: 0x04000104 RID: 260
		public uint BlockSize;

		// Token: 0x04000105 RID: 261
		public long CommittedSequence;

		// Token: 0x04000106 RID: 262
		public string DiskGroupId;

		// Token: 0x04000107 RID: 263
		public string GroupName;

		// Token: 0x04000108 RID: 264
		public uint HeaderSize;

		// Token: 0x04000109 RID: 265
		public uint NumVBlks;

		// Token: 0x0400010A RID: 266
		public long PendingSequence;

		// Token: 0x0400010B RID: 267
		public string Signature;

		// Token: 0x0400010C RID: 268
		public DateTime Timestamp;

		// Token: 0x0400010D RID: 269
		public ushort Unknown1;

		// Token: 0x0400010E RID: 270
		public uint Unknown2;

		// Token: 0x0400010F RID: 271
		public uint Unknown3;

		// Token: 0x04000110 RID: 272
		public uint Unknown4;

		// Token: 0x04000111 RID: 273
		public uint Unknown5;

		// Token: 0x04000112 RID: 274
		public long Unknown6;

		// Token: 0x04000113 RID: 275
		public long Unknown7;

		// Token: 0x04000114 RID: 276
		public uint Unknown8;

		// Token: 0x04000115 RID: 277
		public uint Unknown9;

		// Token: 0x04000116 RID: 278
		public uint UnknownA;

		// Token: 0x04000117 RID: 279
		public long UnknownB;

		// Token: 0x04000118 RID: 280
		public uint UnknownC;

		// Token: 0x04000119 RID: 281
		public ushort VersionDenom;

		// Token: 0x0400011A RID: 282
		public ushort VersionNum;
	}
}
