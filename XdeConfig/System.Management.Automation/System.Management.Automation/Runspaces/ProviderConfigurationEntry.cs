using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000402 RID: 1026
	public sealed class ProviderConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E3F RID: 11839 RVA: 0x000FECE5 File Offset: 0x000FCEE5
		public ProviderConfigurationEntry(string name, Type implementingType, string helpFileName) : base(name)
		{
			if (implementingType == null)
			{
				throw PSTraceSource.NewArgumentNullException("implementingType");
			}
			this._type = implementingType;
			if (!string.IsNullOrEmpty(helpFileName))
			{
				this._helpFileName = helpFileName.Trim();
				return;
			}
			this._helpFileName = helpFileName;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000FED28 File Offset: 0x000FCF28
		internal ProviderConfigurationEntry(string name, Type implementingType, string helpFileName, PSSnapInInfo psSnapinInfo) : base(name, psSnapinInfo)
		{
			if (implementingType == null)
			{
				throw PSTraceSource.NewArgumentNullException("implementingType");
			}
			this._type = implementingType;
			if (!string.IsNullOrEmpty(helpFileName))
			{
				this._helpFileName = helpFileName.Trim();
				return;
			}
			this._helpFileName = helpFileName;
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x000FED75 File Offset: 0x000FCF75
		public Type ImplementingType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x000FED7D File Offset: 0x000FCF7D
		public string HelpFileName
		{
			get
			{
				return this._helpFileName;
			}
		}

		// Token: 0x04001852 RID: 6226
		private Type _type;

		// Token: 0x04001853 RID: 6227
		private string _helpFileName;
	}
}
