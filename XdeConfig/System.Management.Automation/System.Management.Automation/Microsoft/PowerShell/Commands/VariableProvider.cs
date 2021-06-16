using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200078E RID: 1934
	[OutputType(new Type[]
	{
		typeof(PSVariable)
	}, ProviderCmdlet = "New-Item")]
	[CmdletProvider("Variable", ProviderCapabilities.ShouldProcess)]
	[OutputType(new Type[]
	{
		typeof(PSVariable)
	}, ProviderCmdlet = "Set-Item")]
	[OutputType(new Type[]
	{
		typeof(PSVariable)
	}, ProviderCmdlet = "Get-Item")]
	[OutputType(new Type[]
	{
		typeof(PSVariable)
	}, ProviderCmdlet = "Rename-Item")]
	[OutputType(new Type[]
	{
		typeof(PSVariable)
	}, ProviderCmdlet = "Copy-Item")]
	public sealed class VariableProvider : SessionStateProviderBase
	{
		// Token: 0x06004CB4 RID: 19636 RVA: 0x00195618 File Offset: 0x00193818
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			string variableDriveDescription = SessionStateStrings.VariableDriveDescription;
			PSDriveInfo item = new PSDriveInfo("Variable", base.ProviderInfo, string.Empty, variableDriveDescription, null);
			return new Collection<PSDriveInfo>
			{
				item
			};
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x00195651 File Offset: 0x00193851
		internal override object GetSessionStateItem(string name)
		{
			return base.SessionState.Internal.GetVariable(name, base.Context.Origin);
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x00195670 File Offset: 0x00193870
		internal override void SetSessionStateItem(string name, object value, bool writeItem)
		{
			PSVariable psvariable;
			if (value != null)
			{
				psvariable = (value as PSVariable);
				if (psvariable == null)
				{
					psvariable = new PSVariable(name, value);
				}
				else if (!string.Equals(name, psvariable.Name, StringComparison.OrdinalIgnoreCase))
				{
					psvariable = new PSVariable(name, psvariable.Value, psvariable.Options, psvariable.Attributes)
					{
						Description = psvariable.Description
					};
				}
			}
			else
			{
				psvariable = new PSVariable(name, null);
			}
			PSVariable psvariable2 = base.SessionState.Internal.SetVariable(psvariable, base.Force, base.Context.Origin) as PSVariable;
			if (writeItem && psvariable2 != null)
			{
				base.WriteItemObject(psvariable2, psvariable2.Name, false);
			}
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x00195717 File Offset: 0x00193917
		internal override void RemoveSessionStateItem(string name)
		{
			base.SessionState.Internal.RemoveVariable(name, base.Force);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x00195735 File Offset: 0x00193935
		internal override IDictionary GetSessionStateTable()
		{
			return (IDictionary)base.SessionState.Internal.GetVariableTable();
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x0019574C File Offset: 0x0019394C
		internal override object GetValueOfItem(object item)
		{
			object result = base.GetValueOfItem(item);
			PSVariable psvariable = item as PSVariable;
			if (psvariable != null)
			{
				result = psvariable.Value;
			}
			return result;
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x00195774 File Offset: 0x00193974
		internal override bool CanRenameItem(object item)
		{
			bool result = false;
			PSVariable psvariable = item as PSVariable;
			if (psvariable != null)
			{
				if ((psvariable.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || ((psvariable.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None && !base.Force))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(psvariable.Name, SessionStateCategory.Variable, "CannotRenameVariable", SessionStateStrings.CannotRenameVariable);
					throw ex;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400253D RID: 9533
		public const string ProviderName = "Variable";
	}
}
