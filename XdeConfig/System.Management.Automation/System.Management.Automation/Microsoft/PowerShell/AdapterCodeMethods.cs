using System;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;

namespace Microsoft.PowerShell
{
	// Token: 0x0200018C RID: 396
	public static class AdapterCodeMethods
	{
		// Token: 0x06001349 RID: 4937 RVA: 0x00078250 File Offset: 0x00076450
		public static long ConvertLargeIntegerToInt64(PSObject deInstance, PSObject largeIntegerInstance)
		{
			if (largeIntegerInstance == null)
			{
				throw PSTraceSource.NewArgumentException("largeIntegerInstance");
			}
			object baseObject = largeIntegerInstance.BaseObject;
			Type type = baseObject.GetType();
			int value = (int)type.InvokeMember("HighPart", BindingFlags.Public | BindingFlags.GetProperty, null, baseObject, null, CultureInfo.InvariantCulture);
			int value2 = (int)type.InvokeMember("LowPart", BindingFlags.Public | BindingFlags.GetProperty, null, baseObject, null, CultureInfo.InvariantCulture);
			byte[] array = new byte[8];
			BitConverter.GetBytes(value2).CopyTo(array, 0);
			BitConverter.GetBytes(value).CopyTo(array, 4);
			return BitConverter.ToInt64(array, 0);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000782E4 File Offset: 0x000764E4
		public static string ConvertDNWithBinaryToString(PSObject deInstance, PSObject dnWithBinaryInstance)
		{
			if (dnWithBinaryInstance == null)
			{
				throw PSTraceSource.NewArgumentException("dnWithBinaryInstance");
			}
			object baseObject = dnWithBinaryInstance.BaseObject;
			Type type = baseObject.GetType();
			return (string)type.InvokeMember("DNString", BindingFlags.Public | BindingFlags.GetProperty, null, baseObject, null, CultureInfo.InvariantCulture);
		}
	}
}
