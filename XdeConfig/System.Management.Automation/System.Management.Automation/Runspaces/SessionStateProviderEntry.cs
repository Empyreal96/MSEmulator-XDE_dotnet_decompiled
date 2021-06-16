using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082C RID: 2092
	public sealed class SessionStateProviderEntry : ConstrainedSessionStateEntry
	{
		// Token: 0x06005019 RID: 20505 RVA: 0x001A7EEC File Offset: 0x001A60EC
		public SessionStateProviderEntry(string name, Type implementingType, string helpFileName) : base(name, SessionStateEntryVisibility.Public)
		{
			this._implementingType = implementingType;
			this._helpFileName = helpFileName;
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x001A7F04 File Offset: 0x001A6104
		internal SessionStateProviderEntry(string name, Type implementingType, string helpFileName, SessionStateEntryVisibility visibility) : base(name, visibility)
		{
			this._implementingType = implementingType;
			this._helpFileName = helpFileName;
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x001A7F20 File Offset: 0x001A6120
		public override InitialSessionStateEntry Clone()
		{
			SessionStateProviderEntry sessionStateProviderEntry = new SessionStateProviderEntry(base.Name, this._implementingType, this._helpFileName, base.Visibility);
			sessionStateProviderEntry.SetPSSnapIn(base.PSSnapIn);
			sessionStateProviderEntry.SetModule(base.Module);
			return sessionStateProviderEntry;
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x001A7F64 File Offset: 0x001A6164
		public Type ImplementingType
		{
			get
			{
				return this._implementingType;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x001A7F6C File Offset: 0x001A616C
		public string HelpFileName
		{
			get
			{
				return this._helpFileName;
			}
		}

		// Token: 0x040028F7 RID: 10487
		private Type _implementingType;

		// Token: 0x040028F8 RID: 10488
		private string _helpFileName;
	}
}
