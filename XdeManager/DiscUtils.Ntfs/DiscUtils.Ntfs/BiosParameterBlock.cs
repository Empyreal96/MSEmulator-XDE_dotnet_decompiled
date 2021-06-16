using System;
using System.Globalization;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000F RID: 15
	internal class BiosParameterBlock
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002DC1 File Offset: 0x00000FC1
		public int BytesPerCluster
		{
			get
			{
				return (int)(this.BytesPerSector * (ushort)this.SectorsPerCluster);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public int IndexBufferSize
		{
			get
			{
				return this.CalcRecordSize(this.RawIndexBufferSize);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002DDE File Offset: 0x00000FDE
		public int MftRecordSize
		{
			get
			{
				return this.CalcRecordSize(this.RawMftRecordSize);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002DEC File Offset: 0x00000FEC
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "BIOS PARAMETER BLOCK (BPB)");
			writer.WriteLine(linePrefix + "                OEM ID: " + this.OemId);
			writer.WriteLine(linePrefix + "      Bytes per Sector: " + this.BytesPerSector);
			writer.WriteLine(linePrefix + "   Sectors per Cluster: " + this.SectorsPerCluster);
			writer.WriteLine(linePrefix + "      Reserved Sectors: " + this.ReservedSectors);
			writer.WriteLine(linePrefix + "                # FATs: " + this.NumFats);
			writer.WriteLine(linePrefix + "    # FAT Root Entries: " + this.FatRootEntriesCount);
			writer.WriteLine(linePrefix + "   Total Sectors (16b): " + this.TotalSectors16);
			writer.WriteLine(linePrefix + "                 Media: " + this.Media.ToString("X", CultureInfo.InvariantCulture) + "h");
			writer.WriteLine(linePrefix + "        FAT size (16b): " + this.FatSize16);
			writer.WriteLine(linePrefix + "     Sectors per Track: " + this.SectorsPerTrack);
			writer.WriteLine(linePrefix + "               # Heads: " + this.NumHeads);
			writer.WriteLine(linePrefix + "        Hidden Sectors: " + this.HiddenSectors);
			writer.WriteLine(linePrefix + "   Total Sectors (32b): " + this.TotalSectors32);
			writer.WriteLine(linePrefix + "     BIOS Drive Number: " + this.BiosDriveNumber);
			writer.WriteLine(linePrefix + "          Chkdsk Flags: " + this.ChkDskFlags);
			writer.WriteLine(linePrefix + "        Signature Byte: " + this.SignatureByte);
			writer.WriteLine(linePrefix + "   Total Sectors (64b): " + this.TotalSectors64);
			writer.WriteLine(linePrefix + "       MFT Record Size: " + this.RawMftRecordSize);
			writer.WriteLine(linePrefix + "     Index Buffer Size: " + this.RawIndexBufferSize);
			writer.WriteLine(linePrefix + "  Volume Serial Number: " + this.VolumeSerialNumber);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003044 File Offset: 0x00001244
		internal static BiosParameterBlock Initialized(Geometry diskGeometry, int clusterSize, uint partitionStartLba, long partitionSizeLba, int mftRecordSize, int indexBufferSize)
		{
			BiosParameterBlock biosParameterBlock = new BiosParameterBlock();
			biosParameterBlock.OemId = "NTFS    ";
			biosParameterBlock.BytesPerSector = 512;
			biosParameterBlock.SectorsPerCluster = (byte)(clusterSize / (int)biosParameterBlock.BytesPerSector);
			biosParameterBlock.ReservedSectors = 0;
			biosParameterBlock.NumFats = 0;
			biosParameterBlock.FatRootEntriesCount = 0;
			biosParameterBlock.TotalSectors16 = 0;
			biosParameterBlock.Media = 248;
			biosParameterBlock.FatSize16 = 0;
			biosParameterBlock.SectorsPerTrack = (ushort)diskGeometry.SectorsPerTrack;
			biosParameterBlock.NumHeads = (ushort)diskGeometry.HeadsPerCylinder;
			biosParameterBlock.HiddenSectors = partitionStartLba;
			biosParameterBlock.TotalSectors32 = 0U;
			biosParameterBlock.BiosDriveNumber = 128;
			biosParameterBlock.ChkDskFlags = 0;
			biosParameterBlock.SignatureByte = 128;
			biosParameterBlock.PaddingByte = 0;
			biosParameterBlock.TotalSectors64 = partitionSizeLba - 1L;
			biosParameterBlock.RawMftRecordSize = biosParameterBlock.CodeRecordSize(mftRecordSize);
			biosParameterBlock.RawIndexBufferSize = biosParameterBlock.CodeRecordSize(indexBufferSize);
			biosParameterBlock.VolumeSerialNumber = BiosParameterBlock.GenSerialNumber();
			return biosParameterBlock;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003128 File Offset: 0x00001328
		internal static BiosParameterBlock FromBytes(byte[] bytes, int offset)
		{
			BiosParameterBlock biosParameterBlock = new BiosParameterBlock();
			biosParameterBlock.OemId = EndianUtilities.BytesToString(bytes, offset + 3, 8);
			biosParameterBlock.BytesPerSector = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 11);
			biosParameterBlock.TotalSectors16 = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 19);
			biosParameterBlock.TotalSectors32 = EndianUtilities.ToUInt32LittleEndian(bytes, offset + 32);
			biosParameterBlock.SignatureByte = bytes[offset + 38];
			biosParameterBlock.TotalSectors64 = EndianUtilities.ToInt64LittleEndian(bytes, offset + 40);
			biosParameterBlock.MftCluster = EndianUtilities.ToInt64LittleEndian(bytes, offset + 48);
			biosParameterBlock.RawMftRecordSize = bytes[offset + 64];
			biosParameterBlock.SectorsPerCluster = bytes[offset + 13];
			if (!biosParameterBlock.IsValid(9223372036854775807L))
			{
				return biosParameterBlock;
			}
			biosParameterBlock.ReservedSectors = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 14);
			biosParameterBlock.NumFats = bytes[offset + 16];
			biosParameterBlock.FatRootEntriesCount = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 17);
			biosParameterBlock.Media = bytes[offset + 21];
			biosParameterBlock.FatSize16 = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 22);
			biosParameterBlock.SectorsPerTrack = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 24);
			biosParameterBlock.NumHeads = EndianUtilities.ToUInt16LittleEndian(bytes, offset + 26);
			biosParameterBlock.HiddenSectors = EndianUtilities.ToUInt32LittleEndian(bytes, offset + 28);
			biosParameterBlock.BiosDriveNumber = bytes[offset + 36];
			biosParameterBlock.ChkDskFlags = bytes[offset + 37];
			biosParameterBlock.PaddingByte = bytes[offset + 39];
			biosParameterBlock.MftMirrorCluster = EndianUtilities.ToInt64LittleEndian(bytes, offset + 56);
			biosParameterBlock.RawIndexBufferSize = bytes[offset + 68];
			biosParameterBlock.VolumeSerialNumber = EndianUtilities.ToUInt64LittleEndian(bytes, offset + 72);
			return biosParameterBlock;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000329C File Offset: 0x0000149C
		internal void ToBytes(byte[] buffer, int offset)
		{
			EndianUtilities.StringToBytes(this.OemId, buffer, offset + 3, 8);
			EndianUtilities.WriteBytesLittleEndian(this.BytesPerSector, buffer, offset + 11);
			buffer[offset + 13] = this.SectorsPerCluster;
			EndianUtilities.WriteBytesLittleEndian(this.ReservedSectors, buffer, offset + 14);
			buffer[offset + 16] = this.NumFats;
			EndianUtilities.WriteBytesLittleEndian(this.FatRootEntriesCount, buffer, offset + 17);
			EndianUtilities.WriteBytesLittleEndian(this.TotalSectors16, buffer, offset + 19);
			buffer[offset + 21] = this.Media;
			EndianUtilities.WriteBytesLittleEndian(this.FatSize16, buffer, offset + 22);
			EndianUtilities.WriteBytesLittleEndian(this.SectorsPerTrack, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(this.NumHeads, buffer, offset + 26);
			EndianUtilities.WriteBytesLittleEndian(this.HiddenSectors, buffer, offset + 28);
			EndianUtilities.WriteBytesLittleEndian(this.TotalSectors32, buffer, offset + 32);
			buffer[offset + 36] = this.BiosDriveNumber;
			buffer[offset + 37] = this.ChkDskFlags;
			buffer[offset + 38] = this.SignatureByte;
			buffer[offset + 39] = this.PaddingByte;
			EndianUtilities.WriteBytesLittleEndian(this.TotalSectors64, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this.MftCluster, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian(this.MftMirrorCluster, buffer, offset + 56);
			buffer[offset + 64] = this.RawMftRecordSize;
			buffer[offset + 68] = this.RawIndexBufferSize;
			EndianUtilities.WriteBytesLittleEndian(this.VolumeSerialNumber, buffer, offset + 72);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033F5 File Offset: 0x000015F5
		internal int CalcRecordSize(byte rawSize)
		{
			if ((rawSize & 128) != 0)
			{
				return 1 << (int)(-(int)((sbyte)rawSize));
			}
			return (int)((ushort)(rawSize * this.SectorsPerCluster) * this.BytesPerSector);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003418 File Offset: 0x00001618
		internal bool IsValidOemId()
		{
			return !string.IsNullOrEmpty(this.OemId) && this.OemId.Length == "NTFS    ".Length && string.Compare(this.OemId, 0, "NTFS    ", 0, "NTFS    ".Length) == 0;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000346C File Offset: 0x0000166C
		internal bool IsValid(long volumeSize)
		{
			if (!this.IsValidOemId() || this.TotalSectors16 != 0 || this.TotalSectors32 != 0U || this.TotalSectors64 == 0L || this.MftRecordSize == 0 || this.MftCluster == 0L || this.BytesPerSector == 0)
			{
				return false;
			}
			long num = this.MftCluster * (long)((ulong)this.SectorsPerCluster) * (long)((ulong)this.BytesPerSector);
			return num < this.TotalSectors64 * (long)((ulong)this.BytesPerSector) && num < volumeSize;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000034E4 File Offset: 0x000016E4
		private static ulong GenSerialNumber()
		{
			byte[] buffer = new byte[8];
			new Random().NextBytes(buffer);
			return EndianUtilities.ToUInt64LittleEndian(buffer, 0);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000350C File Offset: 0x0000170C
		private byte CodeRecordSize(int size)
		{
			if (size >= this.BytesPerCluster)
			{
				return (byte)(size / this.BytesPerCluster);
			}
			sbyte b = 0;
			while (size != 1)
			{
				size = (size >> 1 & int.MaxValue);
				b += 1;
			}
			return (byte)(-(byte)b);
		}

		// Token: 0x04000042 RID: 66
		private const string NTFS_OEM_ID = "NTFS    ";

		// Token: 0x04000043 RID: 67
		public byte BiosDriveNumber;

		// Token: 0x04000044 RID: 68
		public ushort BytesPerSector;

		// Token: 0x04000045 RID: 69
		public byte ChkDskFlags;

		// Token: 0x04000046 RID: 70
		public ushort FatRootEntriesCount;

		// Token: 0x04000047 RID: 71
		public ushort FatSize16;

		// Token: 0x04000048 RID: 72
		public uint HiddenSectors;

		// Token: 0x04000049 RID: 73
		public byte Media;

		// Token: 0x0400004A RID: 74
		public long MftCluster;

		// Token: 0x0400004B RID: 75
		public long MftMirrorCluster;

		// Token: 0x0400004C RID: 76
		public byte NumFats;

		// Token: 0x0400004D RID: 77
		public ushort NumHeads;

		// Token: 0x0400004E RID: 78
		public string OemId;

		// Token: 0x0400004F RID: 79
		public byte PaddingByte;

		// Token: 0x04000050 RID: 80
		public byte RawIndexBufferSize;

		// Token: 0x04000051 RID: 81
		public byte RawMftRecordSize;

		// Token: 0x04000052 RID: 82
		public ushort ReservedSectors;

		// Token: 0x04000053 RID: 83
		public byte SectorsPerCluster;

		// Token: 0x04000054 RID: 84
		public ushort SectorsPerTrack;

		// Token: 0x04000055 RID: 85
		public byte SignatureByte;

		// Token: 0x04000056 RID: 86
		public ushort TotalSectors16;

		// Token: 0x04000057 RID: 87
		public uint TotalSectors32;

		// Token: 0x04000058 RID: 88
		public long TotalSectors64;

		// Token: 0x04000059 RID: 89
		public ulong VolumeSerialNumber;
	}
}
