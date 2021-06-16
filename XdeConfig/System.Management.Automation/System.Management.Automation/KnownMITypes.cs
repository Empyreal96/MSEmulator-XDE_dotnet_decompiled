using System;
using System.Collections.Generic;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A47 RID: 2631
	internal static class KnownMITypes
	{
		// Token: 0x0600698E RID: 27022 RVA: 0x00213D18 File Offset: 0x00211F18
		static KnownMITypes()
		{
			for (int i = 0; i < KnownMITypes._TypeSerializationInfo.Length; i++)
			{
				KnownMITypes._knownTableKeyType.Add(KnownMITypes._TypeSerializationInfo[i].Type.FullName, KnownMITypes._TypeSerializationInfo[i]);
			}
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x00213F50 File Offset: 0x00212150
		internal static MITypeSerializationInfo GetTypeSerializationInfo(Type type)
		{
			MITypeSerializationInfo result = null;
			KnownMITypes._knownTableKeyType.TryGetValue(type.FullName, out result);
			return result;
		}

		// Token: 0x04003287 RID: 12935
		private static readonly MITypeSerializationInfo[] _TypeSerializationInfo = new MITypeSerializationInfo[]
		{
			new MITypeSerializationInfo(typeof(bool), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_boolean"),
			new MITypeSerializationInfo(typeof(byte), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_uint8"),
			new MITypeSerializationInfo(typeof(char), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_char16"),
			new MITypeSerializationInfo(typeof(double), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_real64"),
			new MITypeSerializationInfo(typeof(Guid), null, CimType.String, "PS_ObjectProperty_string"),
			new MITypeSerializationInfo(typeof(short), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_sint16"),
			new MITypeSerializationInfo(typeof(int), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_sint32"),
			new MITypeSerializationInfo(typeof(long), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_sint64"),
			new MITypeSerializationInfo(typeof(sbyte), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_sint8"),
			new MITypeSerializationInfo(typeof(string), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForString), "PS_ObjectProperty_string"),
			new MITypeSerializationInfo(typeof(ushort), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_uint16"),
			new MITypeSerializationInfo(typeof(uint), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_uint32"),
			new MITypeSerializationInfo(typeof(ulong), new MITypeSerializerDelegate(InternalMISerializer.CreateCimInstanceForPrimitiveType), "PS_ObjectProperty_uint64"),
			new MITypeSerializationInfo(typeof(decimal), null, CimType.String, "PS_ObjectProperty_string")
		};

		// Token: 0x04003288 RID: 12936
		private static readonly Dictionary<string, MITypeSerializationInfo> _knownTableKeyType = new Dictionary<string, MITypeSerializationInfo>();
	}
}
