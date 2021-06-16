using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x0200000F RID: 15
	internal sealed class SubKeyHashedListCell : ListCell
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004792 File Offset: 0x00002992
		public SubKeyHashedListCell(RegistryHive hive, string hashType) : base(-1)
		{
			this._hive = hive;
			this._hashType = hashType;
			this._subKeyIndexes = new List<int>();
			this._nameHashes = new List<uint>();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000047BF File Offset: 0x000029BF
		public SubKeyHashedListCell(RegistryHive hive, int index) : base(index)
		{
			this._hive = hive;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000047CF File Offset: 0x000029CF
		internal override int Count
		{
			get
			{
				return this._subKeyIndexes.Count;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000047DC File Offset: 0x000029DC
		public override int Size
		{
			get
			{
				return (int)(4 + this._numElements * 8);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000047E8 File Offset: 0x000029E8
		public override int ReadFrom(byte[] buffer, int offset)
		{
			this._hashType = EndianUtilities.BytesToString(buffer, offset, 2);
			this._numElements = EndianUtilities.ToInt16LittleEndian(buffer, offset + 2);
			this._subKeyIndexes = new List<int>((int)this._numElements);
			this._nameHashes = new List<uint>((int)this._numElements);
			for (int i = 0; i < (int)this._numElements; i++)
			{
				this._subKeyIndexes.Add(EndianUtilities.ToInt32LittleEndian(buffer, offset + 4 + i * 8));
				this._nameHashes.Add(EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4 + i * 8 + 4));
			}
			return (int)(4 + this._numElements * 8);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004884 File Offset: 0x00002A84
		public override void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.StringToBytes(this._hashType, buffer, offset, 2);
			EndianUtilities.WriteBytesLittleEndian(this._numElements, buffer, offset + 2);
			for (int i = 0; i < (int)this._numElements; i++)
			{
				EndianUtilities.WriteBytesLittleEndian(this._subKeyIndexes[i], buffer, offset + 4 + i * 8);
				EndianUtilities.WriteBytesLittleEndian(this._nameHashes[i], buffer, offset + 4 + i * 8 + 4);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000048F4 File Offset: 0x00002AF4
		internal int Add(string name, int cellIndex)
		{
			for (int i = 0; i < (int)this._numElements; i++)
			{
				if (string.Compare(this._hive.GetCell<KeyNodeCell>(this._subKeyIndexes[i]).Name, name, StringComparison.OrdinalIgnoreCase) > 0)
				{
					this._subKeyIndexes.Insert(i, cellIndex);
					this._nameHashes.Insert(i, this.CalcHash(name));
					this._numElements += 1;
					return i;
				}
			}
			this._subKeyIndexes.Add(cellIndex);
			this._nameHashes.Add(this.CalcHash(name));
			short numElements = this._numElements;
			this._numElements = numElements + 1;
			return (int)numElements;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000499C File Offset: 0x00002B9C
		internal override int FindKey(string name, out int cellIndex)
		{
			int num = this.FindKeyAt(name, 0, out cellIndex);
			if (num <= 0)
			{
				return num;
			}
			num = this.FindKeyAt(name, this._subKeyIndexes.Count - 1, out cellIndex);
			if (num >= 0)
			{
				return num;
			}
			SubKeyHashedListCell.KeyFinder keyFinder = new SubKeyHashedListCell.KeyFinder(this._hive, name);
			int num2 = this._subKeyIndexes.BinarySearch(-1, keyFinder);
			cellIndex = keyFinder.CellIndex;
			if (num2 >= 0)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004A00 File Offset: 0x00002C00
		internal override void EnumerateKeys(List<string> names)
		{
			for (int i = 0; i < this._subKeyIndexes.Count; i++)
			{
				names.Add(this._hive.GetCell<KeyNodeCell>(this._subKeyIndexes[i]).Name);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004A45 File Offset: 0x00002C45
		internal override IEnumerable<KeyNodeCell> EnumerateKeys()
		{
			int num;
			for (int i = 0; i < this._subKeyIndexes.Count; i = num)
			{
				yield return this._hive.GetCell<KeyNodeCell>(this._subKeyIndexes[i]);
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004A55 File Offset: 0x00002C55
		internal override int LinkSubKey(string name, int cellIndex)
		{
			this.Add(name, cellIndex);
			return this._hive.UpdateCell(this, true);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004A70 File Offset: 0x00002C70
		internal override int UnlinkSubKey(string name)
		{
			int num = this.IndexOf(name);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return this._hive.UpdateCell(this, true);
			}
			return base.Index;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004AA4 File Offset: 0x00002CA4
		internal int IndexOf(string name)
		{
			foreach (int num in this.Find(name, 0))
			{
				if (this._hive.GetCell<KeyNodeCell>(this._subKeyIndexes[num]).Name.ToUpperInvariant() == name.ToUpperInvariant())
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004B24 File Offset: 0x00002D24
		internal void RemoveAt(int index)
		{
			this._nameHashes.RemoveAt(index);
			this._subKeyIndexes.RemoveAt(index);
			this._numElements -= 1;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004B50 File Offset: 0x00002D50
		private uint CalcHash(string name)
		{
			uint num = 0U;
			if (this._hashType == "lh")
			{
				for (int i = 0; i < name.Length; i++)
				{
					num *= 37U;
					num += (uint)char.ToUpperInvariant(name[i]);
				}
			}
			else
			{
				string text = name + "\0\0\0\0";
				for (int j = 0; j < 4; j++)
				{
					num |= (uint)((uint)(text[j] & 'ÿ') << (j * 8 & 31));
				}
			}
			return num;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004BC8 File Offset: 0x00002DC8
		private int FindKeyAt(string name, int listIndex, out int cellIndex)
		{
			Cell cell = this._hive.GetCell<Cell>(this._subKeyIndexes[listIndex]);
			if (cell == null)
			{
				cellIndex = 0;
				return -1;
			}
			ListCell listCell = cell as ListCell;
			if (listCell != null)
			{
				return listCell.FindKey(name, out cellIndex);
			}
			cellIndex = this._subKeyIndexes[listIndex];
			return string.Compare(name, ((KeyNodeCell)cell).Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004C28 File Offset: 0x00002E28
		private IEnumerable<int> Find(string name, int start)
		{
			if (this._hashType == "lh")
			{
				return this.FindByHash(name, start);
			}
			return this.FindByPrefix(name, start);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004C4D File Offset: 0x00002E4D
		private IEnumerable<int> FindByHash(string name, int start)
		{
			uint hash = this.CalcHash(name);
			int num;
			for (int i = start; i < this._nameHashes.Count; i = num)
			{
				if (this._nameHashes[i] == hash)
				{
					yield return i;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004C6B File Offset: 0x00002E6B
		private IEnumerable<int> FindByPrefix(string name, int start)
		{
			int length = Math.Min(name.Length, 4);
			string compStr = name.Substring(0, length).ToUpperInvariant() + "\0\0\0\0";
			int num2;
			for (int i = start; i < this._nameHashes.Count; i = num2)
			{
				bool flag = true;
				uint num = this._nameHashes[i];
				for (int j = 0; j < 4; j++)
				{
					if (char.ToUpperInvariant((char)(num >> j * 8 & 255U)) != compStr[j])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					yield return i;
				}
				num2 = i + 1;
			}
			yield break;
		}

		// Token: 0x04000053 RID: 83
		private string _hashType;

		// Token: 0x04000054 RID: 84
		private readonly RegistryHive _hive;

		// Token: 0x04000055 RID: 85
		private List<uint> _nameHashes;

		// Token: 0x04000056 RID: 86
		private short _numElements;

		// Token: 0x04000057 RID: 87
		private List<int> _subKeyIndexes;

		// Token: 0x02000017 RID: 23
		private class KeyFinder : IComparer<int>
		{
			// Token: 0x060000B9 RID: 185 RVA: 0x000056FB File Offset: 0x000038FB
			public KeyFinder(RegistryHive hive, string searchName)
			{
				this._hive = hive;
				this._searchName = searchName;
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000BA RID: 186 RVA: 0x00005711 File Offset: 0x00003911
			// (set) Token: 0x060000BB RID: 187 RVA: 0x00005719 File Offset: 0x00003919
			public int CellIndex { get; set; }

			// Token: 0x060000BC RID: 188 RVA: 0x00005722 File Offset: 0x00003922
			public int Compare(int x, int y)
			{
				int num = string.Compare(this._hive.GetCell<KeyNodeCell>(x).Name, this._searchName, StringComparison.OrdinalIgnoreCase);
				if (num == 0)
				{
					this.CellIndex = x;
				}
				return num;
			}

			// Token: 0x0400007D RID: 125
			private readonly RegistryHive _hive;

			// Token: 0x0400007E RID: 126
			private readonly string _searchName;
		}
	}
}
