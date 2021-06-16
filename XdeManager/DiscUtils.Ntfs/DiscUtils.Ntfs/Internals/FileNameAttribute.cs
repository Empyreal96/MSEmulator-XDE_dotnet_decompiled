using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x0200005C RID: 92
	public sealed class FileNameAttribute : GenericAttribute
	{
		// Token: 0x060003DB RID: 987 RVA: 0x00015490 File Offset: 0x00013690
		internal FileNameAttribute(INtfsContext context, AttributeRecord record) : base(context, record)
		{
			byte[] buffer = StreamUtilities.ReadAll(base.Content);
			this._fnr = new FileNameRecord();
			this._fnr.ReadFrom(buffer, 0);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003DC RID: 988 RVA: 0x000154CA File Offset: 0x000136CA
		public long AllocatedSize
		{
			get
			{
				return (long)this._fnr.AllocatedSize;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000154D7 File Offset: 0x000136D7
		public DateTime CreationTime
		{
			get
			{
				return this._fnr.CreationTime;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003DE RID: 990 RVA: 0x000154E4 File Offset: 0x000136E4
		public long ExtendedAttributesSizeOrReparsePointTag
		{
			get
			{
				return (long)((ulong)this._fnr.EASizeOrReparsePointTag);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000154F2 File Offset: 0x000136F2
		public NtfsFileAttributes FileAttributes
		{
			get
			{
				return (NtfsFileAttributes)this._fnr.Flags;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x000154FF File Offset: 0x000136FF
		public string FileName
		{
			get
			{
				return this._fnr.FileName;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001550C File Offset: 0x0001370C
		public NtfsNamespace FileNameNamespace
		{
			get
			{
				return (NtfsNamespace)this._fnr.FileNameNamespace;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00015519 File Offset: 0x00013719
		public DateTime LastAccessTime
		{
			get
			{
				return this._fnr.LastAccessTime;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00015526 File Offset: 0x00013726
		public DateTime MasterFileTableChangedTime
		{
			get
			{
				return this._fnr.MftChangedTime;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00015533 File Offset: 0x00013733
		public DateTime ModificationTime
		{
			get
			{
				return this._fnr.ModificationTime;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00015540 File Offset: 0x00013740
		public MasterFileTableReference ParentDirectory
		{
			get
			{
				return new MasterFileTableReference(this._fnr.ParentDirectory);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00015552 File Offset: 0x00013752
		public long RealSize
		{
			get
			{
				return (long)this._fnr.RealSize;
			}
		}

		// Token: 0x040001B3 RID: 435
		private readonly FileNameRecord _fnr;
	}
}
