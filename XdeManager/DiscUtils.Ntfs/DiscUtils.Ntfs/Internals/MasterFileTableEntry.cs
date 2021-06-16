using System;
using System.Collections.Generic;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000060 RID: 96
	public sealed class MasterFileTableEntry
	{
		// Token: 0x060003F4 RID: 1012 RVA: 0x00015694 File Offset: 0x00013894
		internal MasterFileTableEntry(INtfsContext context, FileRecord fileRecord)
		{
			this._context = context;
			this._fileRecord = fileRecord;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000156AC File Offset: 0x000138AC
		public ICollection<GenericAttribute> Attributes
		{
			get
			{
				List<GenericAttribute> list = new List<GenericAttribute>();
				foreach (AttributeRecord record in this._fileRecord.Attributes)
				{
					list.Add(GenericAttribute.FromAttributeRecord(this._context, record));
				}
				return list;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00015718 File Offset: 0x00013918
		public MasterFileTableReference BaseRecordReference
		{
			get
			{
				return new MasterFileTableReference(this._fileRecord.BaseFile);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001572A File Offset: 0x0001392A
		public MasterFileTableEntryFlags Flags
		{
			get
			{
				return (MasterFileTableEntryFlags)this._fileRecord.Flags;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00015737 File Offset: 0x00013937
		public int HardLinkCount
		{
			get
			{
				return (int)this._fileRecord.HardLinkCount;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00015744 File Offset: 0x00013944
		public long Index
		{
			get
			{
				return (long)((ulong)this._fileRecord.LoadedIndex);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00015752 File Offset: 0x00013952
		public long LogFileSequenceNumber
		{
			get
			{
				return (long)this._fileRecord.LogFileSequenceNumber;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0001575F File Offset: 0x0001395F
		public int NextAttributeId
		{
			get
			{
				return (int)this._fileRecord.NextAttributeId;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001576C File Offset: 0x0001396C
		public long SelfIndex
		{
			get
			{
				return (long)((ulong)this._fileRecord.MasterFileTableIndex);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001577A File Offset: 0x0001397A
		public int SequenceNumber
		{
			get
			{
				return (int)this._fileRecord.SequenceNumber;
			}
		}

		// Token: 0x040001C5 RID: 453
		private readonly INtfsContext _context;

		// Token: 0x040001C6 RID: 454
		private readonly FileRecord _fileRecord;
	}
}
