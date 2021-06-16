using System;

namespace System.Management.Automation
{
	// Token: 0x02000044 RID: 68
	public interface ICommandRuntime2 : ICommandRuntime
	{
		// Token: 0x06000342 RID: 834
		void WriteInformation(InformationRecord informationRecord);

		// Token: 0x06000343 RID: 835
		bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll);
	}
}
