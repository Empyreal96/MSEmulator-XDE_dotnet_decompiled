using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200085E RID: 2142
	public abstract class CustomPSSnapIn : PSSnapInInstaller
	{
		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x001B7CB6 File Offset: 0x001B5EB6
		internal string CustomPSSnapInType
		{
			get
			{
				return base.GetType().FullName;
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06005268 RID: 21096 RVA: 0x001B7CC3 File Offset: 0x001B5EC3
		public virtual Collection<CmdletConfigurationEntry> Cmdlets
		{
			get
			{
				if (this._cmdlets == null)
				{
					this._cmdlets = new Collection<CmdletConfigurationEntry>();
				}
				return this._cmdlets;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06005269 RID: 21097 RVA: 0x001B7CDE File Offset: 0x001B5EDE
		public virtual Collection<ProviderConfigurationEntry> Providers
		{
			get
			{
				if (this._providers == null)
				{
					this._providers = new Collection<ProviderConfigurationEntry>();
				}
				return this._providers;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x001B7CF9 File Offset: 0x001B5EF9
		public virtual Collection<TypeConfigurationEntry> Types
		{
			get
			{
				if (this._types == null)
				{
					this._types = new Collection<TypeConfigurationEntry>();
				}
				return this._types;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x001B7D14 File Offset: 0x001B5F14
		public virtual Collection<FormatConfigurationEntry> Formats
		{
			get
			{
				if (this._formats == null)
				{
					this._formats = new Collection<FormatConfigurationEntry>();
				}
				return this._formats;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x001B7D2F File Offset: 0x001B5F2F
		internal override Dictionary<string, object> RegValues
		{
			get
			{
				if (this._regValues == null)
				{
					this._regValues = base.RegValues;
					if (!string.IsNullOrEmpty(this.CustomPSSnapInType))
					{
						this._regValues["CustomPSSnapInType"] = this.CustomPSSnapInType;
					}
				}
				return this._regValues;
			}
		}

		// Token: 0x04002A40 RID: 10816
		private Collection<CmdletConfigurationEntry> _cmdlets;

		// Token: 0x04002A41 RID: 10817
		private Collection<ProviderConfigurationEntry> _providers;

		// Token: 0x04002A42 RID: 10818
		private Collection<TypeConfigurationEntry> _types;

		// Token: 0x04002A43 RID: 10819
		private Collection<FormatConfigurationEntry> _formats;

		// Token: 0x04002A44 RID: 10820
		private Dictionary<string, object> _regValues;
	}
}
