using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000825 RID: 2085
	public abstract class InitialSessionStateEntry
	{
		// Token: 0x06004FF3 RID: 20467 RVA: 0x001A7B76 File Offset: 0x001A5D76
		protected InitialSessionStateEntry(string name)
		{
			this._name = name;
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06004FF4 RID: 20468 RVA: 0x001A7B85 File Offset: 0x001A5D85
		// (set) Token: 0x06004FF5 RID: 20469 RVA: 0x001A7B8D File Offset: 0x001A5D8D
		public string Name
		{
			get
			{
				return this._name;
			}
			internal set
			{
				this._name = value;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x001A7B96 File Offset: 0x001A5D96
		public PSSnapInInfo PSSnapIn
		{
			get
			{
				return this._psSnapIn;
			}
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001A7B9E File Offset: 0x001A5D9E
		internal void SetPSSnapIn(PSSnapInInfo psSnapIn)
		{
			this._psSnapIn = psSnapIn;
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x001A7BA7 File Offset: 0x001A5DA7
		public PSModuleInfo Module
		{
			get
			{
				return this._module;
			}
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x001A7BAF File Offset: 0x001A5DAF
		internal void SetModule(PSModuleInfo module)
		{
			this._module = module;
		}

		// Token: 0x06004FFA RID: 20474
		public abstract InitialSessionStateEntry Clone();

		// Token: 0x040028E7 RID: 10471
		private string _name;

		// Token: 0x040028E8 RID: 10472
		private PSSnapInInfo _psSnapIn;

		// Token: 0x040028E9 RID: 10473
		private PSModuleInfo _module;
	}
}
