using System;
using System.Collections.Generic;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000022 RID: 34
	internal sealed class RegionTable : IByteArraySerializable
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public bool IsValid
		{
			get
			{
				if (this.Signature != 1768383858U)
				{
					return false;
				}
				if (this.EntryCount > 2047U)
				{
					return false;
				}
				byte[] array = new byte[65536];
				Array.Copy(this._data, array, 65536);
				EndianUtilities.WriteBytesLittleEndian(0U, array, 4);
				return this.Checksum == Crc32LittleEndian.Compute(Crc32Algorithm.Castagnoli, array, 0, 65536);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005D4C File Offset: 0x00003F4C
		public int Size
		{
			get
			{
				return 65536;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005D54 File Offset: 0x00003F54
		public int ReadFrom(byte[] buffer, int offset)
		{
			Array.Copy(buffer, offset, this._data, 0, 65536);
			this.Signature = EndianUtilities.ToUInt32LittleEndian(this._data, 0);
			this.Checksum = EndianUtilities.ToUInt32LittleEndian(this._data, 4);
			this.EntryCount = EndianUtilities.ToUInt32LittleEndian(this._data, 8);
			this.Reserved = EndianUtilities.ToUInt32LittleEndian(this._data, 12);
			this.Regions = new Dictionary<Guid, RegionEntry>();
			if (this.IsValid)
			{
				int num = 0;
				while ((long)num < (long)((ulong)this.EntryCount))
				{
					RegionEntry regionEntry = EndianUtilities.ToStruct<RegionEntry>(this._data, 16 + 32 * num);
					this.Regions.Add(regionEntry.Guid, regionEntry);
					num++;
				}
			}
			return this.Size;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005E10 File Offset: 0x00004010
		public void WriteTo(byte[] buffer, int offset)
		{
			this.EntryCount = (uint)this.Regions.Count;
			this.Checksum = 0U;
			EndianUtilities.WriteBytesLittleEndian(this.Signature, this._data, 0);
			EndianUtilities.WriteBytesLittleEndian(this.Checksum, this._data, 4);
			EndianUtilities.WriteBytesLittleEndian(this.EntryCount, this._data, 8);
			int num = 16;
			foreach (KeyValuePair<Guid, RegionEntry> keyValuePair in this.Regions)
			{
				keyValuePair.Value.WriteTo(this._data, num);
				num += 32;
			}
			this.Checksum = Crc32LittleEndian.Compute(Crc32Algorithm.Castagnoli, this._data, 0, 65536);
			EndianUtilities.WriteBytesLittleEndian(this.Checksum, this._data, 4);
			Array.Copy(this._data, 0, buffer, offset, 65536);
		}

		// Token: 0x0400008E RID: 142
		public const uint RegionTableSignature = 1768383858U;

		// Token: 0x0400008F RID: 143
		public const int FixedSize = 65536;

		// Token: 0x04000090 RID: 144
		public static readonly Guid BatGuid = new Guid("2DC27766-F623-4200-9D64-115E9BFD4A08");

		// Token: 0x04000091 RID: 145
		public static readonly Guid MetadataRegionGuid = new Guid("8B7CA206-4790-4B9A-B8FE-575F050F886E");

		// Token: 0x04000092 RID: 146
		private readonly byte[] _data = new byte[65536];

		// Token: 0x04000093 RID: 147
		public uint Checksum;

		// Token: 0x04000094 RID: 148
		public uint EntryCount;

		// Token: 0x04000095 RID: 149
		public IDictionary<Guid, RegionEntry> Regions = new Dictionary<Guid, RegionEntry>();

		// Token: 0x04000096 RID: 150
		public uint Reserved;

		// Token: 0x04000097 RID: 151
		public uint Signature = 1768383858U;
	}
}
