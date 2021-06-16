using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000062 RID: 98
	public sealed class MasterFileTableRecord
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x00015787 File Offset: 0x00013987
		internal MasterFileTableRecord(FileRecord fileRecord)
		{
			this._fileRecord = fileRecord;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00015796 File Offset: 0x00013996
		public MasterFileTableReference BaseRecordReference
		{
			get
			{
				return new MasterFileTableReference(this._fileRecord.BaseFile);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x000157A8 File Offset: 0x000139A8
		public MasterFileTableRecordFlags Flags
		{
			get
			{
				return (MasterFileTableRecordFlags)this._fileRecord.Flags;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000157B5 File Offset: 0x000139B5
		public int HardLinkCount
		{
			get
			{
				return (int)this._fileRecord.HardLinkCount;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000157C2 File Offset: 0x000139C2
		public long JournalSequenceNumber
		{
			get
			{
				return (long)this._fileRecord.LogFileSequenceNumber;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000157CF File Offset: 0x000139CF
		public int NextAttributeId
		{
			get
			{
				return (int)this._fileRecord.NextAttributeId;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x000157DC File Offset: 0x000139DC
		public int SequenceNumber
		{
			get
			{
				return (int)this._fileRecord.SequenceNumber;
			}
		}

		// Token: 0x040001CD RID: 461
		private readonly FileRecord _fileRecord;
	}
}
