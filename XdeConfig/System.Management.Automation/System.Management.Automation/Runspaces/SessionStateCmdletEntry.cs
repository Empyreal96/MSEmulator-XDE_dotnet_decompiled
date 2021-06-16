using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082B RID: 2091
	public sealed class SessionStateCmdletEntry : SessionStateCommandEntry
	{
		// Token: 0x06005014 RID: 20500 RVA: 0x001A7E58 File Offset: 0x001A6058
		public SessionStateCmdletEntry(string name, Type implementingType, string helpFileName) : base(name, SessionStateEntryVisibility.Public)
		{
			this._implementingType = implementingType;
			this._helpFileName = helpFileName;
			this._commandType = CommandTypes.Cmdlet;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x001A7E77 File Offset: 0x001A6077
		internal SessionStateCmdletEntry(string name, Type implementingType, string helpFileName, SessionStateEntryVisibility visibility) : base(name, visibility)
		{
			this._implementingType = implementingType;
			this._helpFileName = helpFileName;
			this._commandType = CommandTypes.Cmdlet;
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x001A7E98 File Offset: 0x001A6098
		public override InitialSessionStateEntry Clone()
		{
			SessionStateCmdletEntry sessionStateCmdletEntry = new SessionStateCmdletEntry(base.Name, this._implementingType, this._helpFileName, base.Visibility);
			sessionStateCmdletEntry.SetPSSnapIn(base.PSSnapIn);
			sessionStateCmdletEntry.SetModule(base.Module);
			return sessionStateCmdletEntry;
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x001A7EDC File Offset: 0x001A60DC
		public Type ImplementingType
		{
			get
			{
				return this._implementingType;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06005018 RID: 20504 RVA: 0x001A7EE4 File Offset: 0x001A60E4
		public string HelpFileName
		{
			get
			{
				return this._helpFileName;
			}
		}

		// Token: 0x040028F5 RID: 10485
		private Type _implementingType;

		// Token: 0x040028F6 RID: 10486
		private string _helpFileName;
	}
}
