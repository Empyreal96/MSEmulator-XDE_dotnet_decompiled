using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D9 RID: 1241
	internal static class FormatInfoDataListDeserializer<T> where T : FormatInfoData
	{
		// Token: 0x06003626 RID: 13862 RVA: 0x00125CD0 File Offset: 0x00123ED0
		private static void ReadListHelper(IEnumerable en, List<T> lst, FormatObjectDeserializer deserializer)
		{
			deserializer.VerifyDataNotNull(en, "enumerable");
			foreach (object obj in en)
			{
				FormatInfoData formatInfoData = deserializer.DeserializeObject(PSObjectHelper.AsPSObject(obj));
				T t = formatInfoData as T;
				deserializer.VerifyDataNotNull(t, "entry");
				lst.Add(t);
			}
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00125D58 File Offset: 0x00123F58
		internal static void ReadList(PSObject so, string property, List<T> lst, FormatObjectDeserializer deserializer)
		{
			if (lst == null)
			{
				throw PSTraceSource.NewArgumentNullException("lst");
			}
			object property2 = FormatObjectDeserializer.GetProperty(so, property);
			FormatInfoDataListDeserializer<T>.ReadListHelper(PSObjectHelper.GetEnumerable(property2), lst, deserializer);
		}
	}
}
