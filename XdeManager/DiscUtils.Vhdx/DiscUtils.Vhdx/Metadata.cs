using System;
using System.IO;
using System.Security.Permissions;
using DiscUtils.CoreCompat;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000016 RID: 22
	internal sealed class Metadata
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00004F4C File Offset: 0x0000314C
		public Metadata(Stream regionStream)
		{
			this._regionStream = regionStream;
			this._regionStream.Position = 0L;
			this.Table = StreamUtilities.ReadStruct<MetadataTable>(this._regionStream);
			this.FileParameters = this.ReadStruct<FileParameters>(MetadataTable.FileParametersGuid, false);
			this.DiskSize = this.ReadValue<ulong>(MetadataTable.VirtualDiskSizeGuid, false, new Metadata.Reader<ulong>(EndianUtilities.ToUInt64LittleEndian));
			this._page83Data = this.ReadValue<Guid>(MetadataTable.Page83DataGuid, false, new Metadata.Reader<Guid>(EndianUtilities.ToGuidLittleEndian));
			this.LogicalSectorSize = this.ReadValue<uint>(MetadataTable.LogicalSectorSizeGuid, false, new Metadata.Reader<uint>(EndianUtilities.ToUInt32LittleEndian));
			this.PhysicalSectorSize = this.ReadValue<uint>(MetadataTable.PhysicalSectorSizeGuid, false, new Metadata.Reader<uint>(EndianUtilities.ToUInt32LittleEndian));
			this.ParentLocator = this.ReadStruct<ParentLocator>(MetadataTable.ParentLocatorGuid, false);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005020 File Offset: 0x00003220
		public MetadataTable Table { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005028 File Offset: 0x00003228
		public FileParameters FileParameters { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005030 File Offset: 0x00003230
		public ulong DiskSize { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005038 File Offset: 0x00003238
		public uint LogicalSectorSize { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005040 File Offset: 0x00003240
		public uint PhysicalSectorSize { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005048 File Offset: 0x00003248
		public ParentLocator ParentLocator { get; }

		// Token: 0x060000D6 RID: 214 RVA: 0x00005050 File Offset: 0x00003250
		internal static Metadata Initialize(Stream metadataStream, FileParameters fileParameters, ulong diskSize, uint logicalSectorSize, uint physicalSectorSize, ParentLocator parentLocator)
		{
			MetadataTable metadataTable = new MetadataTable();
			uint num = 65536U;
			num += Metadata.AddEntryStruct<FileParameters>(fileParameters, MetadataTable.FileParametersGuid, MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			num += Metadata.AddEntryValue<ulong>(diskSize, new Metadata.Writer<ulong>(EndianUtilities.WriteBytesLittleEndian), MetadataTable.VirtualDiskSizeGuid, MetadataEntryFlags.IsVirtualDisk | MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			num += Metadata.AddEntryValue<Guid>(Guid.NewGuid(), new Metadata.Writer<Guid>(EndianUtilities.WriteBytesLittleEndian), MetadataTable.Page83DataGuid, MetadataEntryFlags.IsVirtualDisk | MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			num += Metadata.AddEntryValue<uint>(logicalSectorSize, new Metadata.Writer<uint>(EndianUtilities.WriteBytesLittleEndian), MetadataTable.LogicalSectorSizeGuid, MetadataEntryFlags.IsVirtualDisk | MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			num += Metadata.AddEntryValue<uint>(physicalSectorSize, new Metadata.Writer<uint>(EndianUtilities.WriteBytesLittleEndian), MetadataTable.PhysicalSectorSizeGuid, MetadataEntryFlags.IsVirtualDisk | MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			if (parentLocator != null)
			{
				num += Metadata.AddEntryStruct<ParentLocator>(parentLocator, MetadataTable.ParentLocatorGuid, MetadataEntryFlags.IsRequired, metadataTable, num, metadataStream);
			}
			metadataStream.Position = 0L;
			StreamUtilities.WriteStruct<MetadataTable>(metadataStream, metadataTable);
			return new Metadata(metadataStream);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005124 File Offset: 0x00003324
		private static uint AddEntryStruct<T>(T data, Guid id, MetadataEntryFlags flags, MetadataTable header, uint dataOffset, Stream stream) where T : IByteArraySerializable
		{
			MetadataEntryKey key = new MetadataEntryKey(id, (flags & MetadataEntryFlags.IsUser) > MetadataEntryFlags.None);
			MetadataEntry metadataEntry = new MetadataEntry();
			metadataEntry.ItemId = id;
			metadataEntry.Offset = dataOffset;
			metadataEntry.Length = (uint)data.Size;
			metadataEntry.Flags = flags;
			header.Entries[key] = metadataEntry;
			stream.Position = (long)((ulong)dataOffset);
			StreamUtilities.WriteStruct<T>(stream, data);
			return metadataEntry.Length;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005194 File Offset: 0x00003394
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		private static uint AddEntryValue<T>(T data, Metadata.Writer<T> writer, Guid id, MetadataEntryFlags flags, MetadataTable header, uint dataOffset, Stream stream)
		{
			MetadataEntryKey key = new MetadataEntryKey(id, (flags & MetadataEntryFlags.IsUser) > MetadataEntryFlags.None);
			MetadataEntry metadataEntry = new MetadataEntry();
			metadataEntry.ItemId = id;
			metadataEntry.Offset = dataOffset;
			metadataEntry.Length = (uint)ReflectionHelper.SizeOf<T>();
			metadataEntry.Flags = flags;
			header.Entries[key] = metadataEntry;
			stream.Position = (long)((ulong)dataOffset);
			byte[] array = new byte[metadataEntry.Length];
			writer(data, array, 0);
			stream.Write(array, 0, array.Length);
			return metadataEntry.Length;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005214 File Offset: 0x00003414
		private T ReadStruct<T>(Guid itemId, bool isUser) where T : IByteArraySerializable, new()
		{
			MetadataEntryKey key = new MetadataEntryKey(itemId, isUser);
			MetadataEntry metadataEntry;
			if (this.Table.Entries.TryGetValue(key, out metadataEntry))
			{
				this._regionStream.Position = (long)((ulong)metadataEntry.Offset);
				return StreamUtilities.ReadStruct<T>(this._regionStream, (int)metadataEntry.Length);
			}
			return default(T);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000526C File Offset: 0x0000346C
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		private T ReadValue<T>(Guid itemId, bool isUser, Metadata.Reader<T> reader)
		{
			MetadataEntryKey key = new MetadataEntryKey(itemId, isUser);
			MetadataEntry metadataEntry;
			if (this.Table.Entries.TryGetValue(key, out metadataEntry))
			{
				this._regionStream.Position = (long)((ulong)metadataEntry.Offset);
				byte[] buffer = StreamUtilities.ReadExact(this._regionStream, ReflectionHelper.SizeOf<T>());
				return reader(buffer, 0);
			}
			return default(T);
		}

		// Token: 0x04000055 RID: 85
		private readonly Stream _regionStream;

		// Token: 0x04000056 RID: 86
		private Guid _page83Data;

		// Token: 0x02000030 RID: 48
		// (Invoke) Token: 0x0600017C RID: 380
		private delegate T Reader<T>(byte[] buffer, int offset);

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x06000180 RID: 384
		private delegate void Writer<T>(T val, byte[] buffer, int offset);
	}
}
