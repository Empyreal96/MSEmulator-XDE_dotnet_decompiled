using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000762 RID: 1890
	[CmdletProvider("Alias", ProviderCapabilities.ShouldProcess)]
	[OutputType(new Type[]
	{
		typeof(AliasInfo)
	}, ProviderCmdlet = "Rename-Item")]
	[OutputType(new Type[]
	{
		typeof(AliasInfo)
	}, ProviderCmdlet = "Set-Item")]
	[OutputType(new Type[]
	{
		typeof(AliasInfo)
	}, ProviderCmdlet = "Copy-Item")]
	[OutputType(new Type[]
	{
		typeof(AliasInfo)
	}, ProviderCmdlet = "Get-ChildItem")]
	[OutputType(new Type[]
	{
		typeof(AliasInfo)
	}, ProviderCmdlet = "New-Item")]
	public sealed class AliasProvider : SessionStateProviderBase
	{
		// Token: 0x06004B95 RID: 19349 RVA: 0x0018BB80 File Offset: 0x00189D80
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			string aliasDriveDescription = SessionStateStrings.AliasDriveDescription;
			PSDriveInfo item = new PSDriveInfo("Alias", base.ProviderInfo, string.Empty, aliasDriveDescription, null);
			return new Collection<PSDriveInfo>
			{
				item
			};
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x0018BBB9 File Offset: 0x00189DB9
		protected override object NewItemDynamicParameters(string path, string type, object newItemValue)
		{
			return new AliasProviderDynamicParameters();
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x0018BBC0 File Offset: 0x00189DC0
		protected override object SetItemDynamicParameters(string path, object value)
		{
			return new AliasProviderDynamicParameters();
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x0018BBC8 File Offset: 0x00189DC8
		internal override object GetSessionStateItem(string name)
		{
			return base.SessionState.Internal.GetAlias(name, base.Context.Origin);
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x0018BBF4 File Offset: 0x00189DF4
		internal override object GetValueOfItem(object item)
		{
			object result = item;
			AliasInfo aliasInfo = item as AliasInfo;
			if (aliasInfo != null)
			{
				result = aliasInfo.Definition;
			}
			return result;
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x0018BC18 File Offset: 0x00189E18
		internal override void SetSessionStateItem(string name, object value, bool writeItem)
		{
			AliasProviderDynamicParameters aliasProviderDynamicParameters = base.DynamicParameters as AliasProviderDynamicParameters;
			AliasInfo aliasInfo = null;
			bool flag = aliasProviderDynamicParameters != null && aliasProviderDynamicParameters.OptionsSet;
			if (value == null)
			{
				if (flag)
				{
					aliasInfo = (AliasInfo)this.GetSessionStateItem(name);
					if (aliasInfo != null)
					{
						aliasInfo.SetOptions(aliasProviderDynamicParameters.Options, base.Force);
					}
				}
				else
				{
					this.RemoveSessionStateItem(name);
				}
			}
			else
			{
				string text = value as string;
				if (text != null)
				{
					if (flag)
					{
						aliasInfo = base.SessionState.Internal.SetAliasValue(name, text, aliasProviderDynamicParameters.Options, base.Force, base.Context.Origin);
					}
					else
					{
						aliasInfo = base.SessionState.Internal.SetAliasValue(name, text, base.Force, base.Context.Origin);
					}
				}
				else
				{
					AliasInfo aliasInfo2 = value as AliasInfo;
					if (aliasInfo2 == null)
					{
						throw PSTraceSource.NewArgumentException("value");
					}
					AliasInfo aliasInfo3 = new AliasInfo(name, aliasInfo2.Definition, base.Context.ExecutionContext, aliasInfo2.Options);
					if (flag)
					{
						aliasInfo3.SetOptions(aliasProviderDynamicParameters.Options, base.Force);
					}
					aliasInfo = base.SessionState.Internal.SetAliasItem(aliasInfo3, base.Force, base.Context.Origin);
				}
			}
			if (writeItem && aliasInfo != null)
			{
				base.WriteItemObject(aliasInfo, aliasInfo.Name, false);
			}
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x0018BD7F File Offset: 0x00189F7F
		internal override void RemoveSessionStateItem(string name)
		{
			base.SessionState.Internal.RemoveAlias(name, base.Force);
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x0018BD9D File Offset: 0x00189F9D
		internal override IDictionary GetSessionStateTable()
		{
			return (IDictionary)base.SessionState.Internal.GetAliasTable();
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x0018BDB4 File Offset: 0x00189FB4
		internal override bool CanRenameItem(object item)
		{
			bool result = false;
			AliasInfo aliasInfo = item as AliasInfo;
			if (aliasInfo != null)
			{
				if ((aliasInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || ((aliasInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None && !base.Force))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(aliasInfo.Name, SessionStateCategory.Alias, "CannotRenameAlias", SessionStateStrings.CannotRenameAlias);
					throw ex;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400247D RID: 9341
		public const string ProviderName = "Alias";
	}
}
