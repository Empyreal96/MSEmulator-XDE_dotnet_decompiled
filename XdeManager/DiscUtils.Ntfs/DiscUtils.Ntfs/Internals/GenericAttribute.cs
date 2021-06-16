using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x0200005D RID: 93
	public abstract class GenericAttribute
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0001555F File Offset: 0x0001375F
		internal GenericAttribute(INtfsContext context, AttributeRecord record)
		{
			this._context = context;
			this._record = record;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00015575 File Offset: 0x00013775
		public AttributeType AttributeType
		{
			get
			{
				return this._record.AttributeType;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00015582 File Offset: 0x00013782
		public IBuffer Content
		{
			get
			{
				return new SubBuffer(this._record.GetReadOnlyDataBuffer(this._context), 0L, this._record.DataLength);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x000155A7 File Offset: 0x000137A7
		public long ContentLength
		{
			get
			{
				return this._record.DataLength;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000155B4 File Offset: 0x000137B4
		public AttributeFlags Flags
		{
			get
			{
				return (AttributeFlags)this._record.Flags;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x000155C1 File Offset: 0x000137C1
		public int Identifier
		{
			get
			{
				return (int)this._record.AttributeId;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000155CE File Offset: 0x000137CE
		public bool IsResident
		{
			get
			{
				return !this._record.IsNonResident;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000155DE File Offset: 0x000137DE
		public string Name
		{
			get
			{
				return this._record.Name;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000155EC File Offset: 0x000137EC
		internal static GenericAttribute FromAttributeRecord(INtfsContext context, AttributeRecord record)
		{
			AttributeType attributeType = record.AttributeType;
			if (attributeType == AttributeType.StandardInformation)
			{
				return new StandardInformationAttribute(context, record);
			}
			if (attributeType == AttributeType.AttributeList)
			{
				return new AttributeListAttribute(context, record);
			}
			if (attributeType != AttributeType.FileName)
			{
				return new UnknownAttribute(context, record);
			}
			return new FileNameAttribute(context, record);
		}

		// Token: 0x040001B4 RID: 436
		private readonly INtfsContext _context;

		// Token: 0x040001B5 RID: 437
		private readonly AttributeRecord _record;
	}
}
