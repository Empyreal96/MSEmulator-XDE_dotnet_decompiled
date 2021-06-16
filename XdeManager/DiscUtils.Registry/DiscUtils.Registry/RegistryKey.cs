using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using DiscUtils.Streams;
using Microsoft.Win32;

namespace DiscUtils.Registry
{
	// Token: 0x0200000A RID: 10
	public sealed class RegistryKey
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003493 File Offset: 0x00001693
		internal RegistryKey(RegistryHive hive, KeyNodeCell cell)
		{
			this._hive = hive;
			this._cell = cell;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000034A9 File Offset: 0x000016A9
		public string ClassName
		{
			get
			{
				if (this._cell.ClassNameIndex > 0)
				{
					return Encoding.Unicode.GetString(this._hive.RawCellData(this._cell.ClassNameIndex, this._cell.ClassNameLength));
				}
				return null;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000034E6 File Offset: 0x000016E6
		public RegistryKeyFlags Flags
		{
			get
			{
				return this._cell.Flags;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000034F4 File Offset: 0x000016F4
		public string Name
		{
			get
			{
				RegistryKey parent = this.Parent;
				if (parent != null && (parent.Flags & RegistryKeyFlags.Root) == (RegistryKeyFlags)0)
				{
					return parent.Name + "\\" + this._cell.Name;
				}
				return this._cell.Name;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000353C File Offset: 0x0000173C
		public RegistryKey Parent
		{
			get
			{
				if ((this._cell.Flags & RegistryKeyFlags.Root) == (RegistryKeyFlags)0)
				{
					return new RegistryKey(this._hive, this._hive.GetCell<KeyNodeCell>(this._cell.ParentIndex));
				}
				return null;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003570 File Offset: 0x00001770
		public int SubKeyCount
		{
			get
			{
				return this._cell.NumSubKeys;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000357D File Offset: 0x0000177D
		public IEnumerable<RegistryKey> SubKeys
		{
			get
			{
				if (this._cell.NumSubKeys != 0)
				{
					ListCell cell = this._hive.GetCell<ListCell>(this._cell.SubKeysIndex);
					foreach (KeyNodeCell cell2 in cell.EnumerateKeys())
					{
						yield return new RegistryKey(this._hive, cell2);
					}
					IEnumerator<KeyNodeCell> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000358D File Offset: 0x0000178D
		public DateTime Timestamp
		{
			get
			{
				return this._cell.Timestamp;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000359A File Offset: 0x0000179A
		public int ValueCount
		{
			get
			{
				return this._cell.NumValues;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000035A7 File Offset: 0x000017A7
		private IEnumerable<RegistryValue> Values
		{
			get
			{
				if (this._cell.NumValues != 0)
				{
					byte[] valueList = this._hive.RawCellData(this._cell.ValueListIndex, this._cell.NumValues * 4);
					int num;
					for (int i = 0; i < this._cell.NumValues; i = num)
					{
						int index = EndianUtilities.ToInt32LittleEndian(valueList, i * 4);
						yield return new RegistryValue(this._hive, this._hive.GetCell<ValueCell>(index));
						num = i + 1;
					}
					valueList = null;
				}
				yield break;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000035B7 File Offset: 0x000017B7
		public RegistrySecurity GetAccessControl()
		{
			if (this._cell.SecurityIndex > 0)
			{
				return this._hive.GetCell<SecurityCell>(this._cell.SecurityIndex).SecurityDescriptor;
			}
			return null;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000035E4 File Offset: 0x000017E4
		public string[] GetSubKeyNames()
		{
			List<string> list = new List<string>();
			if (this._cell.NumSubKeys != 0)
			{
				this._hive.GetCell<ListCell>(this._cell.SubKeysIndex).EnumerateKeys(list);
			}
			return list.ToArray();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003626 File Offset: 0x00001826
		public object GetValue(string name)
		{
			return this.GetValue(name, null, RegistryValueOptions.None);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003631 File Offset: 0x00001831
		public object GetValue(string name, object defaultValue)
		{
			return this.GetValue(name, defaultValue, RegistryValueOptions.None);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000363C File Offset: 0x0000183C
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			RegistryValue registryValue = this.GetRegistryValue(name);
			if (registryValue == null)
			{
				return defaultValue;
			}
			if (registryValue.DataType == RegistryValueType.ExpandString && (options & RegistryValueOptions.DoNotExpandEnvironmentNames) == RegistryValueOptions.None)
			{
				return Environment.ExpandEnvironmentVariables((string)registryValue.Value);
			}
			return registryValue.Value;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000367B File Offset: 0x0000187B
		public void SetValue(string name, object value)
		{
			this.SetValue(name, value, RegistryValueType.None);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003688 File Offset: 0x00001888
		public void SetValue(string name, object value, RegistryValueType valueType)
		{
			RegistryValue registryValue = this.GetRegistryValue(name);
			if (registryValue == null)
			{
				registryValue = this.AddRegistryValue(name);
			}
			registryValue.SetValue(value, valueType);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000036B0 File Offset: 0x000018B0
		public void DeleteValue(string name)
		{
			this.DeleteValue(name, true);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000036BC File Offset: 0x000018BC
		public void DeleteValue(string name, bool throwOnMissingValue)
		{
			bool flag = false;
			if (this._cell.NumValues != 0)
			{
				byte[] array = this._hive.RawCellData(this._cell.ValueListIndex, this._cell.NumValues * 4);
				int i;
				for (i = 0; i < this._cell.NumValues; i++)
				{
					int index = EndianUtilities.ToInt32LittleEndian(array, i * 4);
					if (string.Compare(this._hive.GetCell<ValueCell>(index).Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						this._hive.FreeCell(index);
						this._cell.NumValues--;
						this._hive.UpdateCell(this._cell, false);
						break;
					}
				}
				if (i < this._cell.NumValues)
				{
					while (i < this._cell.NumValues)
					{
						EndianUtilities.WriteBytesLittleEndian(EndianUtilities.ToInt32LittleEndian(array, (i + 1) * 4), array, i * 4);
						i++;
					}
					this._hive.WriteRawCellData(this._cell.ValueListIndex, array, 0, this._cell.NumValues * 4);
				}
			}
			if (throwOnMissingValue && !flag)
			{
				throw new ArgumentException("No such value: " + name, "name");
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000037E8 File Offset: 0x000019E8
		public RegistryValueType GetValueType(string name)
		{
			RegistryValue registryValue = this.GetRegistryValue(name);
			if (registryValue != null)
			{
				return registryValue.DataType;
			}
			return RegistryValueType.None;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003808 File Offset: 0x00001A08
		public string[] GetValueNames()
		{
			List<string> list = new List<string>();
			foreach (RegistryValue registryValue in this.Values)
			{
				list.Add(registryValue.Name);
			}
			return list.ToArray();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003868 File Offset: 0x00001A68
		public RegistryKey CreateSubKey(string subkey)
		{
			if (string.IsNullOrEmpty(subkey))
			{
				return this;
			}
			string[] array = subkey.Split(new char[]
			{
				'\\'
			}, 2);
			int num = this.FindSubKeyCell(array[0]);
			if (num < 0)
			{
				KeyNodeCell keyNodeCell = new KeyNodeCell(array[0], this._cell.Index);
				keyNodeCell.SecurityIndex = this._cell.SecurityIndex;
				this.ReferenceSecurityCell(keyNodeCell.SecurityIndex);
				this._hive.UpdateCell(keyNodeCell, true);
				this.LinkSubKey(array[0], keyNodeCell.Index);
				if (array.Length == 1)
				{
					return new RegistryKey(this._hive, keyNodeCell);
				}
				return new RegistryKey(this._hive, keyNodeCell).CreateSubKey(array[1]);
			}
			else
			{
				KeyNodeCell cell = this._hive.GetCell<KeyNodeCell>(num);
				if (array.Length == 1)
				{
					return new RegistryKey(this._hive, cell);
				}
				return new RegistryKey(this._hive, cell).CreateSubKey(array[1]);
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000394C File Offset: 0x00001B4C
		public RegistryKey OpenSubKey(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return this;
			}
			string[] array = path.Split(new char[]
			{
				'\\'
			}, 2);
			int num = this.FindSubKeyCell(array[0]);
			if (num < 0)
			{
				return null;
			}
			KeyNodeCell cell = this._hive.GetCell<KeyNodeCell>(num);
			if (array.Length == 1)
			{
				return new RegistryKey(this._hive, cell);
			}
			return new RegistryKey(this._hive, cell).OpenSubKey(array[1]);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000039BC File Offset: 0x00001BBC
		public void DeleteSubKeyTree(string subkey)
		{
			RegistryKey registryKey = this.OpenSubKey(subkey);
			if (registryKey == null)
			{
				return;
			}
			if ((registryKey.Flags & RegistryKeyFlags.Root) != (RegistryKeyFlags)0)
			{
				throw new ArgumentException("Attempt to delete root key");
			}
			foreach (string subkey2 in registryKey.GetSubKeyNames())
			{
				registryKey.DeleteSubKeyTree(subkey2);
			}
			this.DeleteSubKey(subkey);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003A11 File Offset: 0x00001C11
		public void DeleteSubKey(string subkey)
		{
			this.DeleteSubKey(subkey, true);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003A1C File Offset: 0x00001C1C
		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			if (string.IsNullOrEmpty(subkey))
			{
				throw new ArgumentException("Invalid SubKey", "subkey");
			}
			string[] array = subkey.Split(new char[]
			{
				'\\'
			}, 2);
			int num = this.FindSubKeyCell(array[0]);
			if (num < 0)
			{
				if (throwOnMissingSubKey)
				{
					throw new ArgumentException("No such SubKey", "subkey");
				}
				return;
			}
			else
			{
				KeyNodeCell cell = this._hive.GetCell<KeyNodeCell>(num);
				if (array.Length != 1)
				{
					new RegistryKey(this._hive, cell).DeleteSubKey(array[1], throwOnMissingSubKey);
					return;
				}
				if (cell.NumSubKeys != 0)
				{
					throw new InvalidOperationException("The registry key has subkeys");
				}
				if (cell.ClassNameIndex != -1)
				{
					this._hive.FreeCell(cell.ClassNameIndex);
					cell.ClassNameIndex = -1;
					cell.ClassNameLength = 0;
				}
				if (cell.SecurityIndex != -1)
				{
					this.DereferenceSecurityCell(cell.SecurityIndex);
					cell.SecurityIndex = -1;
				}
				if (cell.SubKeysIndex != -1)
				{
					this.FreeSubKeys(cell);
				}
				if (cell.ValueListIndex != -1)
				{
					this.FreeValues(cell);
				}
				this.UnlinkSubKey(subkey);
				this._hive.FreeCell(num);
				this._hive.UpdateCell(this._cell, false);
				return;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003B40 File Offset: 0x00001D40
		private RegistryValue GetRegistryValue(string name)
		{
			if (name != null && name.Length == 0)
			{
				name = null;
			}
			if (this._cell.NumValues != 0)
			{
				byte[] buffer = this._hive.RawCellData(this._cell.ValueListIndex, this._cell.NumValues * 4);
				for (int i = 0; i < this._cell.NumValues; i++)
				{
					int index = EndianUtilities.ToInt32LittleEndian(buffer, i * 4);
					ValueCell cell = this._hive.GetCell<ValueCell>(index);
					if (string.Compare(cell.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return new RegistryValue(this._hive, cell);
					}
				}
			}
			return null;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003BD8 File Offset: 0x00001DD8
		private RegistryValue AddRegistryValue(string name)
		{
			byte[] array = this._hive.RawCellData(this._cell.ValueListIndex, this._cell.NumValues * 4);
			if (array == null)
			{
				array = new byte[0];
			}
			int i;
			for (i = 0; i < this._cell.NumValues; i++)
			{
				int index = EndianUtilities.ToInt32LittleEndian(array, i * 4);
				ValueCell cell = this._hive.GetCell<ValueCell>(index);
				if (string.Compare(name, cell.Name, StringComparison.OrdinalIgnoreCase) < 0)
				{
					break;
				}
			}
			ValueCell valueCell = new ValueCell(name);
			this._hive.UpdateCell(valueCell, true);
			byte[] array2 = new byte[this._cell.NumValues * 4 + 4];
			Array.Copy(array, 0, array2, 0, i * 4);
			EndianUtilities.WriteBytesLittleEndian(valueCell.Index, array2, i * 4);
			Array.Copy(array, i * 4, array2, i * 4 + 4, (this._cell.NumValues - i) * 4);
			if (this._cell.ValueListIndex == -1 || !this._hive.WriteRawCellData(this._cell.ValueListIndex, array2, 0, array2.Length))
			{
				int num = this._hive.AllocateRawCell(MathUtilities.RoundUp(array2.Length, 8));
				this._hive.WriteRawCellData(num, array2, 0, array2.Length);
				if (this._cell.ValueListIndex != -1)
				{
					this._hive.FreeCell(this._cell.ValueListIndex);
				}
				this._cell.ValueListIndex = num;
			}
			this._cell.NumValues++;
			this._hive.UpdateCell(this._cell, false);
			return new RegistryValue(this._hive, valueCell);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003D70 File Offset: 0x00001F70
		private int FindSubKeyCell(string name)
		{
			int result;
			if (this._cell.NumSubKeys != 0 && this._hive.GetCell<ListCell>(this._cell.SubKeysIndex).FindKey(name, out result) == 0)
			{
				return result;
			}
			return -1;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003DB0 File Offset: 0x00001FB0
		private void LinkSubKey(string name, int cellIndex)
		{
			if (this._cell.SubKeysIndex == -1)
			{
				SubKeyHashedListCell subKeyHashedListCell = new SubKeyHashedListCell(this._hive, "lf");
				subKeyHashedListCell.Add(name, cellIndex);
				this._hive.UpdateCell(subKeyHashedListCell, true);
				this._cell.NumSubKeys = 1;
				this._cell.SubKeysIndex = subKeyHashedListCell.Index;
			}
			else
			{
				ListCell cell = this._hive.GetCell<ListCell>(this._cell.SubKeysIndex);
				this._cell.SubKeysIndex = cell.LinkSubKey(name, cellIndex);
				this._cell.NumSubKeys++;
			}
			this._hive.UpdateCell(this._cell, false);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003E64 File Offset: 0x00002064
		private void UnlinkSubKey(string name)
		{
			if (this._cell.SubKeysIndex == -1 || this._cell.NumSubKeys == 0)
			{
				throw new InvalidOperationException("No subkey list");
			}
			ListCell cell = this._hive.GetCell<ListCell>(this._cell.SubKeysIndex);
			this._cell.SubKeysIndex = cell.UnlinkSubKey(name);
			this._cell.NumSubKeys--;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003ED4 File Offset: 0x000020D4
		private void ReferenceSecurityCell(int cellIndex)
		{
			SecurityCell cell = this._hive.GetCell<SecurityCell>(cellIndex);
			SecurityCell securityCell = cell;
			int usageCount = securityCell.UsageCount;
			securityCell.UsageCount = usageCount + 1;
			this._hive.UpdateCell(cell, false);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003F0C File Offset: 0x0000210C
		private void DereferenceSecurityCell(int cellIndex)
		{
			SecurityCell cell = this._hive.GetCell<SecurityCell>(cellIndex);
			SecurityCell securityCell = cell;
			int usageCount = securityCell.UsageCount;
			securityCell.UsageCount = usageCount - 1;
			if (cell.UsageCount == 0)
			{
				SecurityCell cell2 = this._hive.GetCell<SecurityCell>(cell.PreviousIndex);
				cell2.NextIndex = cell.NextIndex;
				this._hive.UpdateCell(cell2, false);
				SecurityCell cell3 = this._hive.GetCell<SecurityCell>(cell.NextIndex);
				cell3.PreviousIndex = cell.PreviousIndex;
				this._hive.UpdateCell(cell3, false);
				this._hive.FreeCell(cellIndex);
				return;
			}
			this._hive.UpdateCell(cell, false);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003FB4 File Offset: 0x000021B4
		private void FreeValues(KeyNodeCell cell)
		{
			if (cell.NumValues != 0 && cell.ValueListIndex != -1)
			{
				byte[] buffer = this._hive.RawCellData(cell.ValueListIndex, cell.NumValues * 4);
				for (int i = 0; i < cell.NumValues; i++)
				{
					int index = EndianUtilities.ToInt32LittleEndian(buffer, i * 4);
					this._hive.FreeCell(index);
				}
				this._hive.FreeCell(cell.ValueListIndex);
				cell.ValueListIndex = -1;
				cell.NumValues = 0;
				cell.MaxValDataBytes = 0;
				cell.MaxValNameBytes = 0;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004040 File Offset: 0x00002240
		private void FreeSubKeys(KeyNodeCell subkeyCell)
		{
			if (subkeyCell.SubKeysIndex == -1)
			{
				throw new InvalidOperationException("No subkey list");
			}
			Cell cell = this._hive.GetCell<Cell>(subkeyCell.SubKeysIndex);
			SubKeyIndirectListCell subKeyIndirectListCell = cell as SubKeyIndirectListCell;
			if (subKeyIndirectListCell != null)
			{
				for (int i = 0; i < subKeyIndirectListCell.CellIndexes.Count; i++)
				{
					int index = subKeyIndirectListCell.CellIndexes[i];
					this._hive.FreeCell(index);
				}
			}
			this._hive.FreeCell(cell.Index);
		}

		// Token: 0x0400002D RID: 45
		private readonly KeyNodeCell _cell;

		// Token: 0x0400002E RID: 46
		private readonly RegistryHive _hive;
	}
}
