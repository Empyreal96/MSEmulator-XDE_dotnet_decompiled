using System;

namespace System.Management.Automation
{
	// Token: 0x020000AE RID: 174
	public interface IModuleAssemblyCleanup
	{
		// Token: 0x060008F4 RID: 2292
		void OnRemove(PSModuleInfo psModuleInfo);
	}
}
