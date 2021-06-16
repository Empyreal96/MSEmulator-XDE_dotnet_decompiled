using System;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000470 RID: 1136
	public interface ICmdletProviderSupportsHelp
	{
		// Token: 0x060032A1 RID: 12961
		string GetHelpMaml(string helpItemName, string path);
	}
}
