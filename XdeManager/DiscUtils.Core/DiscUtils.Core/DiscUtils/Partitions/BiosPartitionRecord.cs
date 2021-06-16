using System;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004C RID: 76
	internal class BiosPartitionRecord : IComparable<BiosPartitionRecord>
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00006EEA File Offset: 0x000050EA
		public BiosPartitionRecord()
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00006EF4 File Offset: 0x000050F4
		public BiosPartitionRecord(byte[] data, int offset, uint lbaOffset, int index)
		{
			this._lbaOffset = lbaOffset;
			this.Status = data[offset];
			this.StartHead = data[offset + 1];
			this.StartSector = (data[offset + 2] & 63);
			this.StartCylinder = (ushort)((int)data[offset + 3] | (int)(data[offset + 2] & 192) << 2);
			this.PartitionType = data[offset + 4];
			this.EndHead = data[offset + 5];
			this.EndSector = (data[offset + 6] & 63);
			this.EndCylinder = (ushort)((int)data[offset + 7] | (int)(data[offset + 6] & 192) << 2);
			this.LBAStart = EndianUtilities.ToUInt32LittleEndian(data, offset + 8);
			this.LBALength = EndianUtilities.ToUInt32LittleEndian(data, offset + 12);
			this.Index = index;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00006FB1 File Offset: 0x000051B1
		// (set) Token: 0x0600031F RID: 799 RVA: 0x00006FB9 File Offset: 0x000051B9
		public ushort EndCylinder { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00006FC2 File Offset: 0x000051C2
		// (set) Token: 0x06000321 RID: 801 RVA: 0x00006FCA File Offset: 0x000051CA
		public byte EndHead { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00006FD3 File Offset: 0x000051D3
		// (set) Token: 0x06000323 RID: 803 RVA: 0x00006FDB File Offset: 0x000051DB
		public byte EndSector { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00006FE4 File Offset: 0x000051E4
		public string FriendlyPartitionType
		{
			get
			{
				return BiosPartitionTypes.ToString(this.PartitionType);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00006FF1 File Offset: 0x000051F1
		public int Index { get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00006FF9 File Offset: 0x000051F9
		public bool IsValid
		{
			get
			{
				return this.EndHead != 0 || this.EndSector != 0 || this.EndCylinder != 0 || this.LBALength > 0U;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000701E File Offset: 0x0000521E
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00007026 File Offset: 0x00005226
		public uint LBALength { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000702F File Offset: 0x0000522F
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00007037 File Offset: 0x00005237
		public uint LBAStart { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00007040 File Offset: 0x00005240
		public uint LBAStartAbsolute
		{
			get
			{
				return this.LBAStart + this._lbaOffset;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000704F File Offset: 0x0000524F
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00007057 File Offset: 0x00005257
		public byte PartitionType { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00007060 File Offset: 0x00005260
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00007068 File Offset: 0x00005268
		public ushort StartCylinder { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00007071 File Offset: 0x00005271
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00007079 File Offset: 0x00005279
		public byte StartHead { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00007082 File Offset: 0x00005282
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000708A File Offset: 0x0000528A
		public byte StartSector { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00007093 File Offset: 0x00005293
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000709B File Offset: 0x0000529B
		public byte Status { get; set; }

		// Token: 0x06000336 RID: 822 RVA: 0x000070A4 File Offset: 0x000052A4
		public int CompareTo(BiosPartitionRecord other)
		{
			return this.LBAStartAbsolute.CompareTo(other.LBAStartAbsolute);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000070C8 File Offset: 0x000052C8
		internal void WriteTo(byte[] buffer, int offset)
		{
			buffer[offset] = this.Status;
			buffer[offset + 1] = this.StartHead;
			buffer[offset + 2] = (byte)((int)(this.StartSector & 63) | (this.StartCylinder >> 2 & 192));
			buffer[offset + 3] = (byte)this.StartCylinder;
			buffer[offset + 4] = this.PartitionType;
			buffer[offset + 5] = this.EndHead;
			buffer[offset + 6] = (byte)((int)(this.EndSector & 63) | (this.EndCylinder >> 2 & 192));
			buffer[offset + 7] = (byte)this.EndCylinder;
			EndianUtilities.WriteBytesLittleEndian(this.LBAStart, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.LBALength, buffer, offset + 12);
		}

		// Token: 0x040000AF RID: 175
		private readonly uint _lbaOffset;
	}
}
