using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082F RID: 2095
	public sealed class SessionStateApplicationEntry : SessionStateCommandEntry
	{
		// Token: 0x0600502A RID: 20522 RVA: 0x001A80E6 File Offset: 0x001A62E6
		public SessionStateApplicationEntry(string path) : base(path, SessionStateEntryVisibility.Public)
		{
			this._path = path;
			this._commandType = CommandTypes.Application;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x001A80FF File Offset: 0x001A62FF
		internal SessionStateApplicationEntry(string path, SessionStateEntryVisibility visibility) : base(path, visibility)
		{
			this._path = path;
			this._commandType = CommandTypes.Application;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001A8118 File Offset: 0x001A6318
		public override InitialSessionStateEntry Clone()
		{
			SessionStateApplicationEntry sessionStateApplicationEntry = new SessionStateApplicationEntry(this._path, base.Visibility);
			sessionStateApplicationEntry.SetModule(base.Module);
			return sessionStateApplicationEntry;
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x0600502D RID: 20525 RVA: 0x001A8144 File Offset: 0x001A6344
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x040028FD RID: 10493
		private string _path;
	}
}
