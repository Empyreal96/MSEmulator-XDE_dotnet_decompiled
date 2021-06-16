using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000021 RID: 33
	public sealed class RegionInfo
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00005C5F File Offset: 0x00003E5F
		internal RegionInfo(RegionEntry entry)
		{
			this._entry = entry;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005C6E File Offset: 0x00003E6E
		public long FileOffset
		{
			get
			{
				return this._entry.FileOffset;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005C7B File Offset: 0x00003E7B
		public Guid Guid
		{
			get
			{
				return this._entry.Guid;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005C88 File Offset: 0x00003E88
		public bool IsRequired
		{
			get
			{
				return (this._entry.Flags & RegionFlags.Required) > RegionFlags.None;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005C9A File Offset: 0x00003E9A
		public long Length
		{
			get
			{
				return (long)((ulong)this._entry.Length);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005CA8 File Offset: 0x00003EA8
		public string WellKnownName
		{
			get
			{
				if (this._entry.Guid == RegionEntry.BatGuid)
				{
					return "BAT";
				}
				if (this._entry.Guid == RegionEntry.MetadataRegionGuid)
				{
					return "Metadata Region";
				}
				return null;
			}
		}

		// Token: 0x0400008D RID: 141
		private readonly RegionEntry _entry;
	}
}
