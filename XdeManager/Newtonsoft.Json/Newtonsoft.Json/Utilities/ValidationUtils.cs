using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000069 RID: 105
	internal static class ValidationUtils
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x00019AD8 File Offset: 0x00017CD8
		public static void ArgumentNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}
