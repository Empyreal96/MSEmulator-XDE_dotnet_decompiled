using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AF RID: 1199
	internal static class DefaultScalarTypes
	{
		// Token: 0x0600356E RID: 13678 RVA: 0x00122D2C File Offset: 0x00120F2C
		internal static bool IsTypeInList(Collection<string> typeNames)
		{
			string text = PSObjectHelper.PSObjectIsOfExactType(typeNames);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string text2 = Deserializer.MaskDeserializationPrefix(text);
			return !string.IsNullOrEmpty(text2) && (PSObjectHelper.PSObjectIsEnum(typeNames) || DefaultScalarTypes.defaultScalarTypesHash.Contains(text2));
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x00122D70 File Offset: 0x00120F70
		static DefaultScalarTypes()
		{
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.String");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.SByte");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Byte");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Int16");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.UInt16");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Int32");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.UInt32");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Int64");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.UInt64");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Char");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Single");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Double");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Boolean");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Decimal");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.IntPtr");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Security.SecureString");
			DefaultScalarTypes.defaultScalarTypesHash.Add("System.Numerics.BigInteger");
		}

		// Token: 0x04001B43 RID: 6979
		private static readonly HashSet<string> defaultScalarTypesHash = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}
}
