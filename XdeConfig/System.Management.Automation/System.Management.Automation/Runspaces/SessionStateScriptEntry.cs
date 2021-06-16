using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082D RID: 2093
	public sealed class SessionStateScriptEntry : SessionStateCommandEntry
	{
		// Token: 0x0600501E RID: 20510 RVA: 0x001A7F74 File Offset: 0x001A6174
		public SessionStateScriptEntry(string path) : base(path, SessionStateEntryVisibility.Public)
		{
			this._path = path;
			this._commandType = CommandTypes.ExternalScript;
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001A7F8D File Offset: 0x001A618D
		internal SessionStateScriptEntry(string path, SessionStateEntryVisibility visibility) : base(path, visibility)
		{
			this._path = path;
			this._commandType = CommandTypes.ExternalScript;
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001A7FA8 File Offset: 0x001A61A8
		public override InitialSessionStateEntry Clone()
		{
			SessionStateScriptEntry sessionStateScriptEntry = new SessionStateScriptEntry(this._path, base.Visibility);
			sessionStateScriptEntry.SetModule(base.Module);
			return sessionStateScriptEntry;
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x001A7FD4 File Offset: 0x001A61D4
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x040028F9 RID: 10489
		private string _path;
	}
}
