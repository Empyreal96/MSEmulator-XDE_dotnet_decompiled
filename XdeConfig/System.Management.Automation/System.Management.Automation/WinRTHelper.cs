using System;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000176 RID: 374
	internal class WinRTHelper
	{
		// Token: 0x060012AF RID: 4783 RVA: 0x000743A0 File Offset: 0x000725A0
		internal static bool IsWinRTType(Type type)
		{
			return type.GetTypeInfo().Attributes.ToString().Contains("WindowsRuntime");
		}
	}
}
