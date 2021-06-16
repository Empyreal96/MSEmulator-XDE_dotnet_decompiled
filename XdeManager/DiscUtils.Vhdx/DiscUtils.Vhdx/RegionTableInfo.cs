using System;
using System.Collections;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000023 RID: 35
	public sealed class RegionTableInfo : ICollection<RegionInfo>, IEnumerable<RegionInfo>, IEnumerable
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00005F4A File Offset: 0x0000414A
		internal RegionTableInfo(RegionTable table)
		{
			this._table = table;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005F59 File Offset: 0x00004159
		public int Checksum
		{
			get
			{
				return (int)this._table.Checksum;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005F66 File Offset: 0x00004166
		private IEnumerable<RegionInfo> Entries
		{
			get
			{
				foreach (KeyValuePair<Guid, RegionEntry> keyValuePair in this._table.Regions)
				{
					yield return new RegionInfo(keyValuePair.Value);
				}
				IEnumerator<KeyValuePair<Guid, RegionEntry>> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00005F78 File Offset: 0x00004178
		public string Signature
		{
			get
			{
				byte[] array = new byte[4];
				EndianUtilities.WriteBytesLittleEndian(this._table.Signature, array, 0);
				return EndianUtilities.BytesToString(array, 0, 4);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005FA6 File Offset: 0x000041A6
		public int Count
		{
			get
			{
				return (int)this._table.EntryCount;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00005FB3 File Offset: 0x000041B3
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005FB6 File Offset: 0x000041B6
		public void Add(RegionInfo item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005FBD File Offset: 0x000041BD
		public void Clear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005FC4 File Offset: 0x000041C4
		public bool Contains(RegionInfo item)
		{
			foreach (KeyValuePair<Guid, RegionEntry> keyValuePair in this._table.Regions)
			{
				if (keyValuePair.Key == item.Guid)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000602C File Offset: 0x0000422C
		public void CopyTo(RegionInfo[] array, int arrayIndex)
		{
			int num = 0;
			foreach (KeyValuePair<Guid, RegionEntry> keyValuePair in this._table.Regions)
			{
				array[arrayIndex + num] = new RegionInfo(keyValuePair.Value);
				num++;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006090 File Offset: 0x00004290
		public bool Remove(RegionInfo item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006097 File Offset: 0x00004297
		public IEnumerator<RegionInfo> GetEnumerator()
		{
			return this.Entries.GetEnumerator();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000060A4 File Offset: 0x000042A4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Entries.GetEnumerator();
		}

		// Token: 0x04000098 RID: 152
		private readonly RegionTable _table;
	}
}
