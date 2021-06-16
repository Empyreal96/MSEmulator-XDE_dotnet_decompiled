using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000408 RID: 1032
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class RunspaceConfigurationTypeAttribute : Attribute
	{
		// Token: 0x06002E64 RID: 11876 RVA: 0x000FF77D File Offset: 0x000FD97D
		public RunspaceConfigurationTypeAttribute(string runspaceConfigurationType)
		{
			this._runspaceConfigType = runspaceConfigurationType;
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000FF78C File Offset: 0x000FD98C
		public string RunspaceConfigurationType
		{
			get
			{
				return this._runspaceConfigType;
			}
		}

		// Token: 0x0400185F RID: 6239
		private string _runspaceConfigType;
	}
}
