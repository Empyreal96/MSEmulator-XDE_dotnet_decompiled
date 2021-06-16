using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000067 RID: 103
	public sealed class StandardInformationAttribute : GenericAttribute
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x0001586C File Offset: 0x00013A6C
		internal StandardInformationAttribute(INtfsContext context, AttributeRecord record) : base(context, record)
		{
			byte[] buffer = StreamUtilities.ReadAll(base.Content);
			this._si = new StandardInformation();
			this._si.ReadFrom(buffer, 0);
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000158A6 File Offset: 0x00013AA6
		public long ClassId
		{
			get
			{
				return (long)((ulong)this._si.ClassId);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x000158B4 File Offset: 0x00013AB4
		public DateTime CreationTime
		{
			get
			{
				return this._si.CreationTime;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000158C1 File Offset: 0x00013AC1
		public NtfsFileAttributes FileAttributes
		{
			get
			{
				return (NtfsFileAttributes)this._si.FileAttributes;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000158CE File Offset: 0x00013ACE
		public long JournalSequenceNumber
		{
			get
			{
				return (long)this._si.UpdateSequenceNumber;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000158DB File Offset: 0x00013ADB
		public DateTime LastAccessTime
		{
			get
			{
				return this._si.LastAccessTime;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x000158E8 File Offset: 0x00013AE8
		public DateTime MasterFileTableChangedTime
		{
			get
			{
				return this._si.MftChangedTime;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x000158F5 File Offset: 0x00013AF5
		public long MaxVersions
		{
			get
			{
				return (long)((ulong)this._si.MaxVersions);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00015903 File Offset: 0x00013B03
		public DateTime ModificationTime
		{
			get
			{
				return this._si.ModificationTime;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00015910 File Offset: 0x00013B10
		public long OwnerId
		{
			get
			{
				return (long)((ulong)this._si.OwnerId);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001591E File Offset: 0x00013B1E
		public long QuotaCharged
		{
			get
			{
				return (long)this._si.QuotaCharged;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001592B File Offset: 0x00013B2B
		public long SecurityId
		{
			get
			{
				return (long)((ulong)this._si.SecurityId);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00015939 File Offset: 0x00013B39
		public long Version
		{
			get
			{
				return (long)((ulong)this._si.Version);
			}
		}

		// Token: 0x040001EB RID: 491
		private readonly StandardInformation _si;
	}
}
