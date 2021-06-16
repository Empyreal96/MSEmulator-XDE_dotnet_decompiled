using System;
using System.Collections;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001C RID: 28
	public sealed class MetadataTableInfo : ICollection<MetadataInfo>, IEnumerable<MetadataInfo>, IEnumerable
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x000058A5 File Offset: 0x00003AA5
		internal MetadataTableInfo(MetadataTable table)
		{
			this._table = table;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000058B4 File Offset: 0x00003AB4
		private IEnumerable<MetadataInfo> Entries
		{
			get
			{
				foreach (KeyValuePair<MetadataEntryKey, MetadataEntry> keyValuePair in this._table.Entries)
				{
					yield return new MetadataInfo(keyValuePair.Value);
				}
				IEnumerator<KeyValuePair<MetadataEntryKey, MetadataEntry>> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000058C4 File Offset: 0x00003AC4
		public string Signature
		{
			get
			{
				byte[] array = new byte[8];
				EndianUtilities.WriteBytesLittleEndian(this._table.Signature, array, 0);
				return EndianUtilities.BytesToString(array, 0, 8);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000058F2 File Offset: 0x00003AF2
		public int Count
		{
			get
			{
				return (int)this._table.EntryCount;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000058FF File Offset: 0x00003AFF
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005902 File Offset: 0x00003B02
		public void Add(MetadataInfo item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005909 File Offset: 0x00003B09
		public void Clear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005910 File Offset: 0x00003B10
		public bool Contains(MetadataInfo item)
		{
			foreach (KeyValuePair<MetadataEntryKey, MetadataEntry> keyValuePair in this._table.Entries)
			{
				if (keyValuePair.Key.ItemId == item.ItemId && keyValuePair.Key.IsUser == item.IsUser)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005990 File Offset: 0x00003B90
		public void CopyTo(MetadataInfo[] array, int arrayIndex)
		{
			int num = 0;
			foreach (KeyValuePair<MetadataEntryKey, MetadataEntry> keyValuePair in this._table.Entries)
			{
				array[arrayIndex + num] = new MetadataInfo(keyValuePair.Value);
				num++;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000059F4 File Offset: 0x00003BF4
		public bool Remove(MetadataInfo item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000059FB File Offset: 0x00003BFB
		public IEnumerator<MetadataInfo> GetEnumerator()
		{
			return this.Entries.GetEnumerator();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005A08 File Offset: 0x00003C08
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Entries.GetEnumerator();
		}

		// Token: 0x04000077 RID: 119
		private readonly MetadataTable _table;
	}
}
