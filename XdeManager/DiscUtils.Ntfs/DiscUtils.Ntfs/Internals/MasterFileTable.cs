using System;
using System.Collections.Generic;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x0200005E RID: 94
	public sealed class MasterFileTable
	{
		// Token: 0x060003F0 RID: 1008 RVA: 0x00015630 File Offset: 0x00013830
		internal MasterFileTable(INtfsContext context, MasterFileTable mft)
		{
			this._context = context;
			this._mft = mft;
		}

		// Token: 0x17000118 RID: 280
		public MasterFileTableEntry this[long index]
		{
			get
			{
				FileRecord record = this._mft.GetRecord(index, true, true);
				if (record != null)
				{
					return new MasterFileTableEntry(this._context, record);
				}
				return null;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015675 File Offset: 0x00013875
		public IEnumerable<MasterFileTableEntry> GetEntries(EntryStates filter)
		{
			foreach (FileRecord fileRecord in this._mft.Records)
			{
				EntryStates entryStates;
				if ((fileRecord.Flags & FileRecordFlags.InUse) != FileRecordFlags.None)
				{
					entryStates = EntryStates.InUse;
				}
				else
				{
					entryStates = EntryStates.NotInUse;
				}
				if ((entryStates & filter) != EntryStates.None)
				{
					yield return new MasterFileTableEntry(this._context, fileRecord);
				}
			}
			IEnumerator<FileRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040001B6 RID: 438
		public const long MasterFileTableIndex = 0L;

		// Token: 0x040001B7 RID: 439
		public const long MasterFileTableMirrorIndex = 1L;

		// Token: 0x040001B8 RID: 440
		public const long LogFileIndex = 2L;

		// Token: 0x040001B9 RID: 441
		public const long VolumeIndex = 3L;

		// Token: 0x040001BA RID: 442
		public const long AttributeDefinitionIndex = 4L;

		// Token: 0x040001BB RID: 443
		public const long RootDirectoryIndex = 5L;

		// Token: 0x040001BC RID: 444
		public const long BitmapIndex = 6L;

		// Token: 0x040001BD RID: 445
		public const long BootIndex = 7L;

		// Token: 0x040001BE RID: 446
		public const long BadClusterIndex = 8L;

		// Token: 0x040001BF RID: 447
		public const long SecureIndex = 9L;

		// Token: 0x040001C0 RID: 448
		public const long UppercaseIndex = 10L;

		// Token: 0x040001C1 RID: 449
		public const long ExtendDirectoryIndex = 11L;

		// Token: 0x040001C2 RID: 450
		private const uint FirstNormalFileIndex = 24U;

		// Token: 0x040001C3 RID: 451
		private readonly INtfsContext _context;

		// Token: 0x040001C4 RID: 452
		private readonly MasterFileTable _mft;
	}
}
