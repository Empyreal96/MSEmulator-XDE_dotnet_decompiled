using System;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000069 RID: 105
	internal class TocBlock
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		public void ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.BytesToString(buffer, offset, 8);
			this.Checksum = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
			this.SequenceNumber = EndianUtilities.ToInt64BigEndian(buffer, offset + 12);
			this.Unknown1 = EndianUtilities.ToInt64BigEndian(buffer, offset + 20);
			this.Unknown2 = EndianUtilities.ToInt64BigEndian(buffer, offset + 28);
			this.Item1Str = EndianUtilities.BytesToString(buffer, offset + 36, 10).Trim(new char[1]);
			this.Item1Start = EndianUtilities.ToInt64BigEndian(buffer, offset + 46);
			this.Item1Size = EndianUtilities.ToInt64BigEndian(buffer, offset + 54);
			this.Unknown3 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 62);
			this.Unknown4 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 66);
			this.Item2Str = EndianUtilities.BytesToString(buffer, offset + 70, 10).Trim(new char[1]);
			this.Item2Start = EndianUtilities.ToInt64BigEndian(buffer, offset + 80);
			this.Item2Size = EndianUtilities.ToInt64BigEndian(buffer, offset + 88);
			this.Unknown5 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 96);
			this.Unknown6 = EndianUtilities.ToUInt32BigEndian(buffer, offset + 100);
		}

		// Token: 0x0400015A RID: 346
		public uint Checksum;

		// Token: 0x0400015B RID: 347
		public long Item1Size;

		// Token: 0x0400015C RID: 348
		public long Item1Start;

		// Token: 0x0400015D RID: 349
		public string Item1Str;

		// Token: 0x0400015E RID: 350
		public long Item2Size;

		// Token: 0x0400015F RID: 351
		public long Item2Start;

		// Token: 0x04000160 RID: 352
		public string Item2Str;

		// Token: 0x04000161 RID: 353
		public long SequenceNumber;

		// Token: 0x04000162 RID: 354
		public string Signature;

		// Token: 0x04000163 RID: 355
		public long Unknown1;

		// Token: 0x04000164 RID: 356
		public long Unknown2;

		// Token: 0x04000165 RID: 357
		public uint Unknown3;

		// Token: 0x04000166 RID: 358
		public uint Unknown4;

		// Token: 0x04000167 RID: 359
		public uint Unknown5;

		// Token: 0x04000168 RID: 360
		public uint Unknown6;
	}
}
