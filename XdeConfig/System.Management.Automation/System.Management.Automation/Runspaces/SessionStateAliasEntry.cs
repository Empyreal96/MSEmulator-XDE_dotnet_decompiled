using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082E RID: 2094
	public sealed class SessionStateAliasEntry : SessionStateCommandEntry
	{
		// Token: 0x06005022 RID: 20514 RVA: 0x001A7FDC File Offset: 0x001A61DC
		public SessionStateAliasEntry(string name, string definition) : base(name, SessionStateEntryVisibility.Public)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Alias;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x001A7FFF File Offset: 0x001A61FF
		public SessionStateAliasEntry(string name, string definition, string description) : base(name, SessionStateEntryVisibility.Public)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Alias;
			this._description = description;
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x001A8029 File Offset: 0x001A6229
		public SessionStateAliasEntry(string name, string definition, string description, ScopedItemOptions options) : base(name, SessionStateEntryVisibility.Public)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Alias;
			this._description = description;
			this._options = options;
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x001A805B File Offset: 0x001A625B
		internal SessionStateAliasEntry(string name, string definition, string description, ScopedItemOptions options, SessionStateEntryVisibility visibility) : base(name, visibility)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Alias;
			this._description = description;
			this._options = options;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x001A8090 File Offset: 0x001A6290
		public override InitialSessionStateEntry Clone()
		{
			SessionStateAliasEntry sessionStateAliasEntry = new SessionStateAliasEntry(base.Name, this._definition, this._description, this._options, base.Visibility);
			sessionStateAliasEntry.SetModule(base.Module);
			return sessionStateAliasEntry;
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x001A80CE File Offset: 0x001A62CE
		public string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x001A80D6 File Offset: 0x001A62D6
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x001A80DE File Offset: 0x001A62DE
		public ScopedItemOptions Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x040028FA RID: 10490
		private string _definition;

		// Token: 0x040028FB RID: 10491
		private string _description = string.Empty;

		// Token: 0x040028FC RID: 10492
		private ScopedItemOptions _options;
	}
}
