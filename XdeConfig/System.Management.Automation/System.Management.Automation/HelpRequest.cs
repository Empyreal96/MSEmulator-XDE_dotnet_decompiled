using System;

namespace System.Management.Automation
{
	// Token: 0x020001CD RID: 461
	internal class HelpRequest
	{
		// Token: 0x06001536 RID: 5430 RVA: 0x00084828 File Offset: 0x00082A28
		internal HelpRequest(string target, HelpCategory helpCategory)
		{
			this._target = target;
			this._helpCategory = helpCategory;
			this._origin = CommandOrigin.Runspace;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0008484C File Offset: 0x00082A4C
		internal HelpRequest Clone()
		{
			return new HelpRequest(this.Target, this.HelpCategory)
			{
				Provider = this.Provider,
				MaxResults = this.MaxResults,
				Component = this.Component,
				Role = this.Role,
				Functionality = this.Functionality,
				ProviderContext = this.ProviderContext,
				CommandOrigin = this.CommandOrigin
			};
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x000848C0 File Offset: 0x00082AC0
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x000848C8 File Offset: 0x00082AC8
		internal ProviderContext ProviderContext
		{
			get
			{
				return this._providerContext;
			}
			set
			{
				this._providerContext = value;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x000848D1 File Offset: 0x00082AD1
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x000848D9 File Offset: 0x00082AD9
		internal string Target
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x000848E2 File Offset: 0x00082AE2
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x000848EA File Offset: 0x00082AEA
		internal HelpCategory HelpCategory
		{
			get
			{
				return this._helpCategory;
			}
			set
			{
				this._helpCategory = value;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x000848F3 File Offset: 0x00082AF3
		// (set) Token: 0x0600153F RID: 5439 RVA: 0x000848FB File Offset: 0x00082AFB
		internal string Provider
		{
			get
			{
				return this._provider;
			}
			set
			{
				this._provider = value;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00084904 File Offset: 0x00082B04
		// (set) Token: 0x06001541 RID: 5441 RVA: 0x0008490C File Offset: 0x00082B0C
		internal int MaxResults
		{
			get
			{
				return this._maxResults;
			}
			set
			{
				this._maxResults = value;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x00084915 File Offset: 0x00082B15
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x0008491D File Offset: 0x00082B1D
		internal string[] Component
		{
			get
			{
				return this._component;
			}
			set
			{
				this._component = value;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x00084926 File Offset: 0x00082B26
		// (set) Token: 0x06001545 RID: 5445 RVA: 0x0008492E File Offset: 0x00082B2E
		internal string[] Role
		{
			get
			{
				return this._role;
			}
			set
			{
				this._role = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00084937 File Offset: 0x00082B37
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x0008493F File Offset: 0x00082B3F
		internal string[] Functionality
		{
			get
			{
				return this._functionality;
			}
			set
			{
				this._functionality = value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00084948 File Offset: 0x00082B48
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x00084950 File Offset: 0x00082B50
		internal CommandOrigin CommandOrigin
		{
			get
			{
				return this._origin;
			}
			set
			{
				this._origin = value;
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0008495C File Offset: 0x00082B5C
		internal void Validate()
		{
			if (string.IsNullOrEmpty(this._target) && this._helpCategory == HelpCategory.None && string.IsNullOrEmpty(this._provider) && this._component == null && this._role == null && this._functionality == null)
			{
				this._target = "default";
				this._helpCategory = HelpCategory.DefaultHelp;
				return;
			}
			if (string.IsNullOrEmpty(this._target))
			{
				if (!string.IsNullOrEmpty(this._provider) && (this._helpCategory == HelpCategory.None || this._helpCategory == HelpCategory.Provider))
				{
					this._target = this._provider;
				}
				else
				{
					this._target = "*";
				}
			}
			if ((this._component != null || this._role != null || this._functionality != null) && this._helpCategory == HelpCategory.None)
			{
				this._helpCategory = (HelpCategory.Alias | HelpCategory.Cmdlet | HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow);
				return;
			}
			if ((this._helpCategory & HelpCategory.Cmdlet) > HelpCategory.None)
			{
				this._helpCategory |= HelpCategory.Alias;
			}
			if (this._helpCategory == HelpCategory.None)
			{
				this._helpCategory = HelpCategory.All;
			}
			this._helpCategory &= ~HelpCategory.DefaultHelp;
		}

		// Token: 0x040008FE RID: 2302
		private ProviderContext _providerContext;

		// Token: 0x040008FF RID: 2303
		private string _target;

		// Token: 0x04000900 RID: 2304
		private HelpCategory _helpCategory;

		// Token: 0x04000901 RID: 2305
		private string _provider;

		// Token: 0x04000902 RID: 2306
		private int _maxResults = -1;

		// Token: 0x04000903 RID: 2307
		private string[] _component;

		// Token: 0x04000904 RID: 2308
		private string[] _role;

		// Token: 0x04000905 RID: 2309
		private string[] _functionality;

		// Token: 0x04000906 RID: 2310
		private CommandOrigin _origin;
	}
}
