using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000010 RID: 16
	internal sealed class SubKeyIndirectListCell : ListCell
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00004C89 File Offset: 0x00002E89
		public SubKeyIndirectListCell(RegistryHive hive, int index) : base(index)
		{
			this._hive = hive;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004C99 File Offset: 0x00002E99
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004CA1 File Offset: 0x00002EA1
		public List<int> CellIndexes { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004CAC File Offset: 0x00002EAC
		internal override int Count
		{
			get
			{
				int num = 0;
				foreach (int index in this.CellIndexes)
				{
					ListCell listCell = this._hive.GetCell<Cell>(index) as ListCell;
					if (listCell != null)
					{
						num += listCell.Count;
					}
					else
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004D20 File Offset: 0x00002F20
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00004D28 File Offset: 0x00002F28
		public string ListType { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004D31 File Offset: 0x00002F31
		public override int Size
		{
			get
			{
				return 4 + this.CellIndexes.Count * 4;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004D44 File Offset: 0x00002F44
		public override int ReadFrom(byte[] buffer, int offset)
		{
			this.ListType = EndianUtilities.BytesToString(buffer, offset, 2);
			int num = (int)EndianUtilities.ToInt16LittleEndian(buffer, offset + 2);
			this.CellIndexes = new List<int>(num);
			for (int i = 0; i < num; i++)
			{
				this.CellIndexes.Add(EndianUtilities.ToInt32LittleEndian(buffer, offset + 4 + i * 4));
			}
			return 4 + this.CellIndexes.Count * 4;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004DA8 File Offset: 0x00002FA8
		public override void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.StringToBytes(this.ListType, buffer, offset, 2);
			EndianUtilities.WriteBytesLittleEndian((ushort)this.CellIndexes.Count, buffer, offset + 2);
			for (int i = 0; i < this.CellIndexes.Count; i++)
			{
				EndianUtilities.WriteBytesLittleEndian(this.CellIndexes[i], buffer, offset + 4 + i * 4);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004E08 File Offset: 0x00003008
		internal override int FindKey(string name, out int cellIndex)
		{
			if (this.CellIndexes.Count <= 0)
			{
				cellIndex = 0;
				return -1;
			}
			int num = this.DoFindKey(name, 0, out cellIndex);
			if (num <= 0)
			{
				return num;
			}
			num = this.DoFindKey(name, this.CellIndexes.Count - 1, out cellIndex);
			if (num >= 0)
			{
				return num;
			}
			SubKeyIndirectListCell.KeyFinder keyFinder = new SubKeyIndirectListCell.KeyFinder(this._hive, name);
			int num2 = this.CellIndexes.BinarySearch(-1, keyFinder);
			cellIndex = keyFinder.CellIndex;
			if (num2 >= 0)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004E7C File Offset: 0x0000307C
		internal override void EnumerateKeys(List<string> names)
		{
			for (int i = 0; i < this.CellIndexes.Count; i++)
			{
				Cell cell = this._hive.GetCell<Cell>(this.CellIndexes[i]);
				ListCell listCell = cell as ListCell;
				if (listCell != null)
				{
					listCell.EnumerateKeys(names);
				}
				else
				{
					names.Add(((KeyNodeCell)cell).Name);
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004EDB File Offset: 0x000030DB
		internal override IEnumerable<KeyNodeCell> EnumerateKeys()
		{
			int num;
			for (int i = 0; i < this.CellIndexes.Count; i = num)
			{
				Cell cell = this._hive.GetCell<Cell>(this.CellIndexes[i]);
				ListCell listCell = cell as ListCell;
				if (listCell != null)
				{
					foreach (KeyNodeCell keyNodeCell in listCell.EnumerateKeys())
					{
						yield return keyNodeCell;
					}
					IEnumerator<KeyNodeCell> enumerator = null;
				}
				else
				{
					yield return (KeyNodeCell)cell;
				}
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004EEC File Offset: 0x000030EC
		internal override int LinkSubKey(string name, int cellIndex)
		{
			if (!(this.ListType == "ri"))
			{
				for (int i = 0; i < this.CellIndexes.Count; i++)
				{
					KeyNodeCell cell = this._hive.GetCell<KeyNodeCell>(this.CellIndexes[i]);
					if (string.Compare(name, cell.Name, StringComparison.OrdinalIgnoreCase) < 0)
					{
						this.CellIndexes.Insert(i, cellIndex);
						return this._hive.UpdateCell(this, true);
					}
				}
				this.CellIndexes.Add(cellIndex);
				return this._hive.UpdateCell(this, true);
			}
			if (this.CellIndexes.Count == 0)
			{
				throw new NotImplementedException("Empty indirect list");
			}
			for (int j = 0; j < this.CellIndexes.Count - 1; j++)
			{
				ListCell cell2 = this._hive.GetCell<ListCell>(this.CellIndexes[j]);
				int num;
				if (cell2.FindKey(name, out num) <= 0)
				{
					this.CellIndexes[j] = cell2.LinkSubKey(name, cellIndex);
					return this._hive.UpdateCell(this, false);
				}
			}
			ListCell cell3 = this._hive.GetCell<ListCell>(this.CellIndexes[this.CellIndexes.Count - 1]);
			this.CellIndexes[this.CellIndexes.Count - 1] = cell3.LinkSubKey(name, cellIndex);
			return this._hive.UpdateCell(this, false);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005050 File Offset: 0x00003250
		internal override int UnlinkSubKey(string name)
		{
			if (this.ListType == "ri")
			{
				if (this.CellIndexes.Count == 0)
				{
					throw new NotImplementedException("Empty indirect list");
				}
				for (int i = 0; i < this.CellIndexes.Count; i++)
				{
					ListCell cell = this._hive.GetCell<ListCell>(this.CellIndexes[i]);
					int num;
					if (cell.FindKey(name, out num) <= 0)
					{
						this.CellIndexes[i] = cell.UnlinkSubKey(name);
						if (cell.Count == 0)
						{
							this._hive.FreeCell(this.CellIndexes[i]);
							this.CellIndexes.RemoveAt(i);
						}
						return this._hive.UpdateCell(this, false);
					}
				}
			}
			else
			{
				for (int j = 0; j < this.CellIndexes.Count; j++)
				{
					KeyNodeCell cell2 = this._hive.GetCell<KeyNodeCell>(this.CellIndexes[j]);
					if (string.Compare(name, cell2.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.CellIndexes.RemoveAt(j);
						return this._hive.UpdateCell(this, true);
					}
				}
			}
			return base.Index;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005174 File Offset: 0x00003374
		private int DoFindKey(string name, int listIndex, out int cellIndex)
		{
			Cell cell = this._hive.GetCell<Cell>(this.CellIndexes[listIndex]);
			ListCell listCell = cell as ListCell;
			if (listCell != null)
			{
				return listCell.FindKey(name, out cellIndex);
			}
			cellIndex = this.CellIndexes[listIndex];
			return string.Compare(name, ((KeyNodeCell)cell).Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04000058 RID: 88
		private readonly RegistryHive _hive;

		// Token: 0x0200001B RID: 27
		private class KeyFinder : IComparer<int>
		{
			// Token: 0x060000D5 RID: 213 RVA: 0x00005B3B File Offset: 0x00003D3B
			public KeyFinder(RegistryHive hive, string searchName)
			{
				this._hive = hive;
				this._searchName = searchName;
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005B51 File Offset: 0x00003D51
			// (set) Token: 0x060000D7 RID: 215 RVA: 0x00005B59 File Offset: 0x00003D59
			public int CellIndex { get; set; }

			// Token: 0x060000D8 RID: 216 RVA: 0x00005B64 File Offset: 0x00003D64
			public int Compare(int x, int y)
			{
				Cell cell = this._hive.GetCell<Cell>(x);
				ListCell listCell = cell as ListCell;
				if (listCell != null)
				{
					int cellIndex;
					int num = listCell.FindKey(this._searchName, out cellIndex);
					if (num == 0)
					{
						this.CellIndex = cellIndex;
					}
					return -num;
				}
				int num2 = string.Compare(((KeyNodeCell)cell).Name, this._searchName, StringComparison.OrdinalIgnoreCase);
				if (num2 == 0)
				{
					this.CellIndex = x;
				}
				return num2;
			}

			// Token: 0x04000099 RID: 153
			private readonly RegistryHive _hive;

			// Token: 0x0400009A RID: 154
			private readonly string _searchName;
		}
	}
}
