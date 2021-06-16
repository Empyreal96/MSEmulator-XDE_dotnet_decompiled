using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000059 RID: 89
	public sealed class AttributeListEntry
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00015438 File Offset: 0x00013638
		internal AttributeListEntry(AttributeListRecord record)
		{
			this._record = record;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00015447 File Offset: 0x00013647
		public int AttributeIdentifier
		{
			get
			{
				return (int)this._record.AttributeId;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00015454 File Offset: 0x00013654
		public string AttributeName
		{
			get
			{
				return this._record.Name;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00015461 File Offset: 0x00013661
		public AttributeType AttributeType
		{
			get
			{
				return this._record.Type;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001546E File Offset: 0x0001366E
		public long FirstFileCluster
		{
			get
			{
				return (long)this._record.StartVcn;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001547B File Offset: 0x0001367B
		public MasterFileTableReference MasterFileTableEntry
		{
			get
			{
				return new MasterFileTableReference(this._record.BaseFileReference);
			}
		}

		// Token: 0x040001A8 RID: 424
		private readonly AttributeListRecord _record;
	}
}
