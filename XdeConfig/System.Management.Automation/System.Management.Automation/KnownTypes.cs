using System;
using System.Collections.Generic;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000459 RID: 1113
	internal static class KnownTypes
	{
		// Token: 0x060030A0 RID: 12448 RVA: 0x00109CB8 File Offset: 0x00107EB8
		static KnownTypes()
		{
			for (int i = 0; i < KnownTypes._TypeSerializationInfo.Length; i++)
			{
				KnownTypes._knownTableKeyType.Add(KnownTypes._TypeSerializationInfo[i].Type.FullName, KnownTypes._TypeSerializationInfo[i]);
				KnownTypes._knownTableKeyItemTag.Add(KnownTypes._TypeSerializationInfo[i].ItemTag, KnownTypes._TypeSerializationInfo[i]);
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x0010A1C4 File Offset: 0x001083C4
		internal static TypeSerializationInfo GetTypeSerializationInfo(Type type)
		{
			TypeSerializationInfo xdInfo;
			if (!KnownTypes._knownTableKeyType.TryGetValue(type.FullName, out xdInfo) && typeof(XmlDocument).IsAssignableFrom(type))
			{
				xdInfo = KnownTypes._xdInfo;
			}
			return xdInfo;
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x0010A200 File Offset: 0x00108400
		internal static TypeSerializationInfo GetTypeSerializationInfoFromItemTag(string itemTag)
		{
			TypeSerializationInfo result;
			KnownTypes._knownTableKeyItemTag.TryGetValue(itemTag, out result);
			return result;
		}

		// Token: 0x04001A2E RID: 6702
		private static readonly TypeSerializationInfo _xdInfo = new TypeSerializationInfo(typeof(XmlDocument), "XD", "XD", new TypeSerializerDelegate(InternalSerializer.WriteXmlDocument), new TypeDeserializerDelegate(InternalDeserializer.DeserializeXmlDocument));

		// Token: 0x04001A2F RID: 6703
		private static readonly TypeSerializationInfo[] _TypeSerializationInfo = new TypeSerializationInfo[]
		{
			new TypeSerializationInfo(typeof(bool), "B", "B", new TypeSerializerDelegate(InternalSerializer.WriteBoolean), new TypeDeserializerDelegate(InternalDeserializer.DeserializeBoolean)),
			new TypeSerializationInfo(typeof(byte), "By", "By", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeByte)),
			new TypeSerializationInfo(typeof(char), "C", "C", new TypeSerializerDelegate(InternalSerializer.WriteChar), new TypeDeserializerDelegate(InternalDeserializer.DeserializeChar)),
			new TypeSerializationInfo(typeof(DateTime), "DT", "DT", new TypeSerializerDelegate(InternalSerializer.WriteDateTime), new TypeDeserializerDelegate(InternalDeserializer.DeserializeDateTime)),
			new TypeSerializationInfo(typeof(decimal), "D", "D", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeDecimal)),
			new TypeSerializationInfo(typeof(double), "Db", "Db", new TypeSerializerDelegate(InternalSerializer.WriteDouble), new TypeDeserializerDelegate(InternalDeserializer.DeserializeDouble)),
			new TypeSerializationInfo(typeof(Guid), "G", "G", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeGuid)),
			new TypeSerializationInfo(typeof(short), "I16", "I16", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeInt16)),
			new TypeSerializationInfo(typeof(int), "I32", "I32", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeInt32)),
			new TypeSerializationInfo(typeof(long), "I64", "I64", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeInt64)),
			new TypeSerializationInfo(typeof(sbyte), "SB", "SB", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeSByte)),
			new TypeSerializationInfo(typeof(float), "Sg", "Sg", new TypeSerializerDelegate(InternalSerializer.WriteSingle), new TypeDeserializerDelegate(InternalDeserializer.DeserializeSingle)),
			new TypeSerializationInfo(typeof(ScriptBlock), "SBK", "SBK", new TypeSerializerDelegate(InternalSerializer.WriteScriptBlock), new TypeDeserializerDelegate(InternalDeserializer.DeserializeScriptBlock)),
			new TypeSerializationInfo(typeof(string), "S", "S", new TypeSerializerDelegate(InternalSerializer.WriteEncodedString), new TypeDeserializerDelegate(InternalDeserializer.DeserializeString)),
			new TypeSerializationInfo(typeof(TimeSpan), "TS", "TS", new TypeSerializerDelegate(InternalSerializer.WriteTimeSpan), new TypeDeserializerDelegate(InternalDeserializer.DeserializeTimeSpan)),
			new TypeSerializationInfo(typeof(ushort), "U16", "U16", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeUInt16)),
			new TypeSerializationInfo(typeof(uint), "U32", "U32", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeUInt32)),
			new TypeSerializationInfo(typeof(ulong), "U64", "U64", null, new TypeDeserializerDelegate(InternalDeserializer.DeserializeUInt64)),
			new TypeSerializationInfo(typeof(Uri), "URI", "URI", new TypeSerializerDelegate(InternalSerializer.WriteUri), new TypeDeserializerDelegate(InternalDeserializer.DeserializeUri)),
			new TypeSerializationInfo(typeof(byte[]), "BA", "BA", new TypeSerializerDelegate(InternalSerializer.WriteByteArray), new TypeDeserializerDelegate(InternalDeserializer.DeserializeByteArray)),
			new TypeSerializationInfo(typeof(Version), "Version", "Version", new TypeSerializerDelegate(InternalSerializer.WriteVersion), new TypeDeserializerDelegate(InternalDeserializer.DeserializeVersion)),
			KnownTypes._xdInfo,
			new TypeSerializationInfo(typeof(ProgressRecord), "PR", "PR", new TypeSerializerDelegate(InternalSerializer.WriteProgressRecord), new TypeDeserializerDelegate(InternalDeserializer.DeserializeProgressRecord)),
			new TypeSerializationInfo(typeof(SecureString), "SS", "SS", new TypeSerializerDelegate(InternalSerializer.WriteSecureString), new TypeDeserializerDelegate(InternalDeserializer.DeserializeSecureString))
		};

		// Token: 0x04001A30 RID: 6704
		private static readonly Dictionary<string, TypeSerializationInfo> _knownTableKeyType = new Dictionary<string, TypeSerializationInfo>();

		// Token: 0x04001A31 RID: 6705
		private static readonly Dictionary<string, TypeSerializationInfo> _knownTableKeyItemTag = new Dictionary<string, TypeSerializationInfo>();
	}
}
