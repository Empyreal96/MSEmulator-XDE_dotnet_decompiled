using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020003FE RID: 1022
	public abstract class RunspaceConfigurationEntry
	{
		// Token: 0x06002E28 RID: 11816 RVA: 0x000FE9DB File Offset: 0x000FCBDB
		protected RunspaceConfigurationEntry(string name)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			this._name = name.Trim();
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000FEA18 File Offset: 0x000FCC18
		internal RunspaceConfigurationEntry(string name, PSSnapInInfo psSnapin)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			this._name = name.Trim();
			if (psSnapin == null)
			{
				throw PSTraceSource.NewArgumentException("psSnapin");
			}
			this._PSSnapin = psSnapin;
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x000FEA73 File Offset: 0x000FCC73
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06002E2B RID: 11819 RVA: 0x000FEA7B File Offset: 0x000FCC7B
		public PSSnapInInfo PSSnapIn
		{
			get
			{
				return this._PSSnapin;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000FEA83 File Offset: 0x000FCC83
		public bool BuiltIn
		{
			get
			{
				return this._builtIn;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x000FEA8B File Offset: 0x000FCC8B
		internal UpdateAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x04001847 RID: 6215
		private string _name;

		// Token: 0x04001848 RID: 6216
		private PSSnapInInfo _PSSnapin;

		// Token: 0x04001849 RID: 6217
		internal bool _builtIn;

		// Token: 0x0400184A RID: 6218
		internal UpdateAction _action = UpdateAction.None;
	}
}
