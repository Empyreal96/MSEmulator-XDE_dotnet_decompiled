using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001B RID: 27
	internal sealed class MetadataTable : IByteArraySerializable
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000055D8 File Offset: 0x000037D8
		public bool IsValid
		{
			get
			{
				if (this.Signature != 7022344802737087853UL)
				{
					return false;
				}
				if (this.EntryCount > 2047)
				{
					return false;
				}
				foreach (MetadataEntry metadataEntry in this.Entries.Values)
				{
					if ((metadataEntry.Flags & MetadataEntryFlags.IsRequired) != MetadataEntryFlags.None && !MetadataTable.KnownMetadata.ContainsKey(metadataEntry.ItemId))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005668 File Offset: 0x00003868
		public int Size
		{
			get
			{
				return 65536;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005670 File Offset: 0x00003870
		public int ReadFrom(byte[] buffer, int offset)
		{
			Array.Copy(buffer, offset, this._headerData, 0, 32);
			this.Signature = EndianUtilities.ToUInt64LittleEndian(this._headerData, 0);
			this.EntryCount = EndianUtilities.ToUInt16LittleEndian(this._headerData, 10);
			this.Entries = new Dictionary<MetadataEntryKey, MetadataEntry>();
			if (this.IsValid)
			{
				for (int i = 0; i < (int)this.EntryCount; i++)
				{
					MetadataEntry metadataEntry = EndianUtilities.ToStruct<MetadataEntry>(buffer, offset + 32 + i * 32);
					this.Entries[MetadataEntryKey.FromEntry(metadataEntry)] = metadataEntry;
				}
			}
			return 65536;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005700 File Offset: 0x00003900
		public void WriteTo(byte[] buffer, int offset)
		{
			this.EntryCount = (ushort)this.Entries.Count;
			EndianUtilities.WriteBytesLittleEndian(this.Signature, this._headerData, 0);
			EndianUtilities.WriteBytesLittleEndian(this.EntryCount, this._headerData, 10);
			Array.Copy(this._headerData, 0, buffer, offset, 32);
			int num = 32 + offset;
			foreach (KeyValuePair<MetadataEntryKey, MetadataEntry> keyValuePair in this.Entries)
			{
				keyValuePair.Value.WriteTo(buffer, num);
				num += 32;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000057A8 File Offset: 0x000039A8
		private static Dictionary<Guid, object> InitMetadataTable()
		{
			Dictionary<Guid, object> dictionary = new Dictionary<Guid, object>();
			dictionary[MetadataTable.FileParametersGuid] = null;
			dictionary[MetadataTable.VirtualDiskSizeGuid] = null;
			dictionary[MetadataTable.Page83DataGuid] = null;
			dictionary[MetadataTable.LogicalSectorSizeGuid] = null;
			dictionary[MetadataTable.PhysicalSectorSizeGuid] = null;
			dictionary[MetadataTable.ParentLocatorGuid] = null;
			return dictionary;
		}

		// Token: 0x0400006A RID: 106
		public const int FixedSize = 65536;

		// Token: 0x0400006B RID: 107
		public const ulong MetadataTableSignature = 7022344802737087853UL;

		// Token: 0x0400006C RID: 108
		public static readonly Guid FileParametersGuid = new Guid("CAA16737-FA36-4D43-B3B6-33F0AA44E76B");

		// Token: 0x0400006D RID: 109
		public static readonly Guid VirtualDiskSizeGuid = new Guid("2FA54224-CD1B-4876-B211-5DBED83BF4B8");

		// Token: 0x0400006E RID: 110
		public static readonly Guid Page83DataGuid = new Guid("BECA12AB-B2E6-4523-93EF-C309E000C746");

		// Token: 0x0400006F RID: 111
		public static readonly Guid LogicalSectorSizeGuid = new Guid("8141Bf1D-A96F-4709-BA47-F233A8FAAb5F");

		// Token: 0x04000070 RID: 112
		public static readonly Guid PhysicalSectorSizeGuid = new Guid("CDA348C7-445D-4471-9CC9-E9885251C556");

		// Token: 0x04000071 RID: 113
		public static readonly Guid ParentLocatorGuid = new Guid("A8D35F2D-B30B-454D-ABF7-D3D84834AB0C");

		// Token: 0x04000072 RID: 114
		private static readonly Dictionary<Guid, object> KnownMetadata = MetadataTable.InitMetadataTable();

		// Token: 0x04000073 RID: 115
		private readonly byte[] _headerData = new byte[32];

		// Token: 0x04000074 RID: 116
		public IDictionary<MetadataEntryKey, MetadataEntry> Entries = new Dictionary<MetadataEntryKey, MetadataEntry>();

		// Token: 0x04000075 RID: 117
		public ushort EntryCount;

		// Token: 0x04000076 RID: 118
		public ulong Signature = 7022344802737087853UL;
	}
}
