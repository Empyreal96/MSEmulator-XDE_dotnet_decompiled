using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000019 RID: 25
	internal sealed class MetadataEntryKey : IEquatable<MetadataEntryKey>
	{
		// Token: 0x060000DF RID: 223 RVA: 0x0000538E File Offset: 0x0000358E
		public MetadataEntryKey(Guid itemId, bool isUser)
		{
			this._itemId = itemId;
			this.IsUser = isUser;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000053A4 File Offset: 0x000035A4
		public bool IsUser { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000053AC File Offset: 0x000035AC
		public Guid ItemId
		{
			get
			{
				return this._itemId;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000053B4 File Offset: 0x000035B4
		public bool Equals(MetadataEntryKey other)
		{
			return !(other == null) && this._itemId == other._itemId && this.IsUser == other.IsUser;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000053E4 File Offset: 0x000035E4
		public static bool operator ==(MetadataEntryKey x, MetadataEntryKey y)
		{
			return x == y || (x != null && y != null && x._itemId == y._itemId && x.IsUser == y.IsUser);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005417 File Offset: 0x00003617
		public static bool operator !=(MetadataEntryKey x, MetadataEntryKey y)
		{
			return !(x == y);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005423 File Offset: 0x00003623
		public static MetadataEntryKey FromEntry(MetadataEntry entry)
		{
			return new MetadataEntryKey(entry.ItemId, (entry.Flags & MetadataEntryFlags.IsUser) > MetadataEntryFlags.None);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000543C File Offset: 0x0000363C
		public override bool Equals(object other)
		{
			MetadataEntryKey metadataEntryKey = other as MetadataEntryKey;
			return metadataEntryKey != null && this.Equals(metadataEntryKey);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005462 File Offset: 0x00003662
		public override int GetHashCode()
		{
			return this._itemId.GetHashCode() ^ (this.IsUser ? 3937189 : 0);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005486 File Offset: 0x00003686
		public override string ToString()
		{
			return this._itemId + (this.IsUser ? " - User" : " - System");
		}

		// Token: 0x04000067 RID: 103
		private Guid _itemId;
	}
}
