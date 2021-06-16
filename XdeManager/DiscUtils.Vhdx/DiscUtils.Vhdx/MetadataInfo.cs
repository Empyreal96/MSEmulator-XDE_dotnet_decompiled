using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001A RID: 26
	public sealed class MetadataInfo
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x000054AC File Offset: 0x000036AC
		internal MetadataInfo(MetadataEntry entry)
		{
			this._entry = entry;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000054BB File Offset: 0x000036BB
		public bool IsRequired
		{
			get
			{
				return (this._entry.Flags & MetadataEntryFlags.IsRequired) > MetadataEntryFlags.None;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000054CD File Offset: 0x000036CD
		public bool IsUser
		{
			get
			{
				return (this._entry.Flags & MetadataEntryFlags.IsUser) > MetadataEntryFlags.None;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000054DF File Offset: 0x000036DF
		public bool IsVirtualDisk
		{
			get
			{
				return (this._entry.Flags & MetadataEntryFlags.IsVirtualDisk) > MetadataEntryFlags.None;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000054F1 File Offset: 0x000036F1
		public Guid ItemId
		{
			get
			{
				return this._entry.ItemId;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000054FE File Offset: 0x000036FE
		public long Length
		{
			get
			{
				return (long)((ulong)this._entry.Length);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000550C File Offset: 0x0000370C
		public long Offset
		{
			get
			{
				return (long)((ulong)this._entry.Offset);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000551C File Offset: 0x0000371C
		public string WellKnownName
		{
			get
			{
				if (this._entry.ItemId == MetadataTable.FileParametersGuid)
				{
					return "File Parameters";
				}
				if (this._entry.ItemId == MetadataTable.LogicalSectorSizeGuid)
				{
					return "Logical Sector Size";
				}
				if (this._entry.ItemId == MetadataTable.Page83DataGuid)
				{
					return "SCSI Page 83 Data";
				}
				if (this._entry.ItemId == MetadataTable.ParentLocatorGuid)
				{
					return "Parent Locator";
				}
				if (this._entry.ItemId == MetadataTable.PhysicalSectorSizeGuid)
				{
					return "Physical Sector Size";
				}
				if (this._entry.ItemId == MetadataTable.VirtualDiskSizeGuid)
				{
					return "Virtual Disk Size";
				}
				return null;
			}
		}

		// Token: 0x04000069 RID: 105
		private readonly MetadataEntry _entry;
	}
}
