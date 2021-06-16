using System;
using System.Globalization;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x0200000C RID: 12
	internal sealed class RegistryValue
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000040BD File Offset: 0x000022BD
		internal RegistryValue(RegistryHive hive, ValueCell cell)
		{
			this._hive = hive;
			this._cell = cell;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000040D3 File Offset: 0x000022D3
		public RegistryValueType DataType
		{
			get
			{
				return this._cell.DataType;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000040E0 File Offset: 0x000022E0
		public string Name
		{
			get
			{
				return this._cell.Name ?? string.Empty;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000040F6 File Offset: 0x000022F6
		public object Value
		{
			get
			{
				return RegistryValue.ConvertToObject(this.GetData(), this.DataType);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000410C File Offset: 0x0000230C
		public byte[] GetData()
		{
			if (this._cell.DataLength < 0)
			{
				int num = this._cell.DataLength & int.MaxValue;
				byte[] array = new byte[4];
				EndianUtilities.WriteBytesLittleEndian(this._cell.DataIndex, array, 0);
				byte[] array2 = new byte[num];
				Array.Copy(array, array2, num);
				return array2;
			}
			return this._hive.RawCellData(this._cell.DataIndex, this._cell.DataLength);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004184 File Offset: 0x00002384
		public void SetData(byte[] data, int offset, int count, RegistryValueType valueType)
		{
			if ((valueType == RegistryValueType.Dword || valueType == RegistryValueType.DwordBigEndian) && count <= 4)
			{
				if (this._cell.DataLength >= 0)
				{
					this._hive.FreeCell(this._cell.DataIndex);
				}
				this._cell.DataLength = (count | int.MinValue);
				this._cell.DataIndex = EndianUtilities.ToInt32LittleEndian(data, offset);
				this._cell.DataType = valueType;
			}
			else
			{
				if (this._cell.DataIndex == -1 || this._cell.DataLength < 0)
				{
					this._cell.DataIndex = this._hive.AllocateRawCell(count);
				}
				if (!this._hive.WriteRawCellData(this._cell.DataIndex, data, offset, count))
				{
					int num = this._hive.AllocateRawCell(count);
					this._hive.WriteRawCellData(num, data, offset, count);
					this._hive.FreeCell(this._cell.DataIndex);
					this._cell.DataIndex = num;
				}
				this._cell.DataLength = count;
				this._cell.DataType = valueType;
			}
			this._hive.UpdateCell(this._cell, false);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000042B4 File Offset: 0x000024B4
		public void SetValue(object value, RegistryValueType valueType)
		{
			if (valueType == RegistryValueType.None)
			{
				if (value is int)
				{
					valueType = RegistryValueType.Dword;
				}
				else if (value is byte[])
				{
					valueType = RegistryValueType.Binary;
				}
				else if (value is string[])
				{
					valueType = RegistryValueType.MultiString;
				}
				else
				{
					valueType = RegistryValueType.String;
				}
			}
			byte[] array = RegistryValue.ConvertToData(value, valueType);
			this.SetData(array, 0, array.Length, valueType);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004302 File Offset: 0x00002502
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Name,
				":",
				this.DataType,
				":",
				this.DataAsString()
			});
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004340 File Offset: 0x00002540
		private static object ConvertToObject(byte[] data, RegistryValueType type)
		{
			switch (type)
			{
			case RegistryValueType.String:
			case RegistryValueType.ExpandString:
			case RegistryValueType.Link:
				return Encoding.Unicode.GetString(data).Trim(new char[1]);
			case RegistryValueType.Dword:
				return EndianUtilities.ToInt32LittleEndian(data, 0);
			case RegistryValueType.DwordBigEndian:
				return EndianUtilities.ToInt32BigEndian(data, 0);
			case RegistryValueType.MultiString:
				return Encoding.Unicode.GetString(data).Trim(new char[1]).Split(new char[1]);
			case RegistryValueType.QWord:
				return string.Empty + EndianUtilities.ToUInt64LittleEndian(data, 0);
			}
			return data;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000043F0 File Offset: 0x000025F0
		private static byte[] ConvertToData(object value, RegistryValueType valueType)
		{
			if (valueType == RegistryValueType.None)
			{
				throw new ArgumentException("Specific registry value type must be specified", "valueType");
			}
			byte[] array;
			switch (valueType)
			{
			case RegistryValueType.String:
			case RegistryValueType.ExpandString:
			{
				string text = value.ToString();
				array = new byte[text.Length * 2 + 2];
				Encoding.Unicode.GetBytes(text, 0, text.Length, array, 0);
				return array;
			}
			case RegistryValueType.Dword:
				array = new byte[4];
				EndianUtilities.WriteBytesLittleEndian((int)value, array, 0);
				return array;
			case RegistryValueType.DwordBigEndian:
				array = new byte[4];
				EndianUtilities.WriteBytesBigEndian((int)value, array, 0);
				return array;
			case RegistryValueType.MultiString:
			{
				string text2 = string.Join("\0", (string[])value) + "\0";
				array = new byte[text2.Length * 2 + 2];
				Encoding.Unicode.GetBytes(text2, 0, text2.Length, array, 0);
				return array;
			}
			}
			array = (byte[])value;
			return array;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000044E0 File Offset: 0x000026E0
		private string DataAsString()
		{
			switch (this.DataType)
			{
			case RegistryValueType.String:
			case RegistryValueType.ExpandString:
			case RegistryValueType.Dword:
			case RegistryValueType.DwordBigEndian:
			case RegistryValueType.Link:
			case RegistryValueType.QWord:
				return RegistryValue.ConvertToObject(this.GetData(), this.DataType).ToString();
			case RegistryValueType.MultiString:
				return string.Join(",", (string[])RegistryValue.ConvertToObject(this.GetData(), this.DataType));
			}
			byte[] data = this.GetData();
			string str = string.Empty;
			for (int i = 0; i < Math.Min(data.Length, 8); i++)
			{
				str += string.Format(CultureInfo.InvariantCulture, "{0:X2} ", new object[]
				{
					(int)data[i]
				});
			}
			return str + string.Format(CultureInfo.InvariantCulture, " ({0} bytes)", new object[]
			{
				data.Length
			});
		}

		// Token: 0x04000040 RID: 64
		private readonly ValueCell _cell;

		// Token: 0x04000041 RID: 65
		private readonly RegistryHive _hive;
	}
}
