using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000401 RID: 1025
	public sealed class CmdletConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E3B RID: 11835 RVA: 0x000FEC46 File Offset: 0x000FCE46
		public CmdletConfigurationEntry(string name, Type implementingType, string helpFileName) : base(name)
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

		// Token: 0x06002E3C RID: 11836 RVA: 0x000FEC88 File Offset: 0x000FCE88
		internal CmdletConfigurationEntry(string name, Type implementingType, string helpFileName, PSSnapInInfo psSnapinInfo) : base(name, psSnapinInfo)
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

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x000FECD5 File Offset: 0x000FCED5
		public Type ImplementingType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x000FECDD File Offset: 0x000FCEDD
		public string HelpFileName
		{
			get
			{
				return this._helpFileName;
			}
		}

		// Token: 0x04001850 RID: 6224
		private Type _type;

		// Token: 0x04001851 RID: 6225
		private string _helpFileName;
	}
}
