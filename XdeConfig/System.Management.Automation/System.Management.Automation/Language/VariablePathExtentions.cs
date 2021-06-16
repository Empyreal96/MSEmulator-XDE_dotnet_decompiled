using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005EA RID: 1514
	internal static class VariablePathExtentions
	{
		// Token: 0x060040D0 RID: 16592 RVA: 0x00158644 File Offset: 0x00156844
		internal static bool IsAnyLocal(this VariablePath variablePath)
		{
			return variablePath.IsUnscopedVariable || variablePath.IsLocal || variablePath.IsPrivate;
		}
	}
}
