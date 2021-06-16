using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000018 RID: 24
	internal class DirectoryEntry
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x000054D0 File Offset: 0x000036D0
		public DirectoryEntry(Directory directory, FileRecordReference fileReference, FileNameRecord fileDetails)
		{
			this._directory = directory;
			this.Reference = fileReference;
			this.Details = fileDetails;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000054ED File Offset: 0x000036ED
		public FileNameRecord Details { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000054F5 File Offset: 0x000036F5
		public bool IsDirectory
		{
			get
			{
				return (this.Details.Flags & FileAttributeFlags.Directory) > FileAttributeFlags.None;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000550B File Offset: 0x0000370B
		public FileRecordReference Reference { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005514 File Offset: 0x00003714
		public string SearchName
		{
			get
			{
				string fileName = this.Details.FileName;
				if (fileName.IndexOf('.') == -1)
				{
					return fileName + ".";
				}
				return fileName;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005545 File Offset: 0x00003745
		internal void UpdateFrom(File file)
		{
			file.FreshenFileName(this.Details, true);
			this._directory.UpdateEntry(this);
		}

		// Token: 0x04000074 RID: 116
		private readonly Directory _directory;
	}
}
