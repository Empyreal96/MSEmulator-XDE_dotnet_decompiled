using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000058 RID: 88
	public sealed class AttributeListAttribute : GenericAttribute
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x000153A0 File Offset: 0x000135A0
		internal AttributeListAttribute(INtfsContext context, AttributeRecord record) : base(context, record)
		{
			byte[] buffer = StreamUtilities.ReadAll(base.Content);
			this._list = new AttributeList();
			this._list.ReadFrom(buffer, 0);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x000153DC File Offset: 0x000135DC
		public ICollection<AttributeListEntry> Entries
		{
			get
			{
				List<AttributeListEntry> list = new List<AttributeListEntry>();
				foreach (AttributeListRecord record in this._list)
				{
					list.Add(new AttributeListEntry(record));
				}
				return list;
			}
		}

		// Token: 0x040001A7 RID: 423
		private readonly AttributeList _list;
	}
}
