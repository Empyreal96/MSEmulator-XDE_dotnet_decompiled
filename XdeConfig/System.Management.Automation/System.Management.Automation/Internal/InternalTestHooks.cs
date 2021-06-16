using System;
using System.Reflection;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000436 RID: 1078
	public static class InternalTestHooks
	{
		// Token: 0x06002F78 RID: 12152 RVA: 0x00104884 File Offset: 0x00102A84
		public static void SetTestHook(string property, bool value)
		{
			FieldInfo field = typeof(InternalTestHooks).GetField(property, BindingFlags.Static | BindingFlags.NonPublic);
			if (field != null)
			{
				field.SetValue(null, value);
			}
		}

		// Token: 0x040019B5 RID: 6581
		internal static bool BypassGroupPolicyCaching;

		// Token: 0x040019B6 RID: 6582
		internal static bool ForceScriptBlockLogging;

		// Token: 0x040019B7 RID: 6583
		internal static bool UseDebugAmsiImplementation;

		// Token: 0x040019B8 RID: 6584
		internal static bool BypassAppLockerPolicyCaching;

		// Token: 0x040019B9 RID: 6585
		internal static bool ReadEngineTypesXmlFiles;

		// Token: 0x040019BA RID: 6586
		internal static bool IgnoreScriptBlockCache;
	}
}
