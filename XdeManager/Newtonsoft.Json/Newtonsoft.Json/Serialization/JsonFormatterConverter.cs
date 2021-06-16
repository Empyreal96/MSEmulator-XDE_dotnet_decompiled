using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000086 RID: 134
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001D1DD File Offset: 0x0001B3DD
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001D210 File Offset: 0x0001B410
		private T GetTokenValue<T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001D244 File Offset: 0x0001B444
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken token;
			if ((token = (value as JToken)) == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(token, type, this._contract, this._member);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001D290 File Offset: 0x0001B490
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue;
			if ((jvalue = (value as JValue)) != null)
			{
				value = jvalue.Value;
			}
			return System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001D2C6 File Offset: 0x0001B4C6
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001D2CF File Offset: 0x0001B4CF
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001D2D8 File Offset: 0x0001B4D8
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001D2E1 File Offset: 0x0001B4E1
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001D2EA File Offset: 0x0001B4EA
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001D2F3 File Offset: 0x0001B4F3
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001D2FC File Offset: 0x0001B4FC
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001D305 File Offset: 0x0001B505
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001D30E File Offset: 0x0001B50E
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001D317 File Offset: 0x0001B517
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001D320 File Offset: 0x0001B520
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001D329 File Offset: 0x0001B529
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001D332 File Offset: 0x0001B532
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001D33B File Offset: 0x0001B53B
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001D344 File Offset: 0x0001B544
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x0400026F RID: 623
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x04000270 RID: 624
		private readonly JsonISerializableContract _contract;

		// Token: 0x04000271 RID: 625
		private readonly JsonProperty _member;
	}
}
