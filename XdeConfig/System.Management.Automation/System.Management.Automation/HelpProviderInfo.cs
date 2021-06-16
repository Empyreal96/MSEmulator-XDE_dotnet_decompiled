using System;

namespace System.Management.Automation
{
	// Token: 0x020001B2 RID: 434
	internal class HelpProviderInfo
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x0007C324 File Offset: 0x0007A524
		internal HelpProviderInfo(string assemblyName, string className, HelpCategory helpCategory)
		{
			this.AssemblyName = assemblyName;
			this.ClassName = className;
			this.HelpCategory = helpCategory;
		}

		// Token: 0x040008B8 RID: 2232
		internal string AssemblyName = "";

		// Token: 0x040008B9 RID: 2233
		internal string ClassName = "";

		// Token: 0x040008BA RID: 2234
		internal HelpCategory HelpCategory;
	}
}
