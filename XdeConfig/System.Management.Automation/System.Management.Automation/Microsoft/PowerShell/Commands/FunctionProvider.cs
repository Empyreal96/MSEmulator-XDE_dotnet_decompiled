using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200078B RID: 1931
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "Rename-Item")]
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "Get-ChildItem")]
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "New-Item")]
	[CmdletProvider("Function", ProviderCapabilities.ShouldProcess)]
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "Set-Item")]
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "Copy-Item")]
	[OutputType(new Type[]
	{
		typeof(FunctionInfo)
	}, ProviderCmdlet = "Get-Item")]
	public sealed class FunctionProvider : SessionStateProviderBase
	{
		// Token: 0x06004C9F RID: 19615 RVA: 0x001951EC File Offset: 0x001933EC
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			string functionDriveDescription = SessionStateStrings.FunctionDriveDescription;
			PSDriveInfo item = new PSDriveInfo("Function", base.ProviderInfo, string.Empty, functionDriveDescription, null);
			return new Collection<PSDriveInfo>
			{
				item
			};
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x00195225 File Offset: 0x00193425
		protected override object NewItemDynamicParameters(string path, string type, object newItemValue)
		{
			return new FunctionProviderDynamicParameters();
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0019522C File Offset: 0x0019342C
		protected override object SetItemDynamicParameters(string path, object value)
		{
			return new FunctionProviderDynamicParameters();
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x00195234 File Offset: 0x00193434
		internal override object GetSessionStateItem(string name)
		{
			return base.SessionState.Internal.GetFunction(name, base.Context.Origin);
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x00195260 File Offset: 0x00193460
		internal override void SetSessionStateItem(string name, object value, bool writeItem)
		{
			FunctionProviderDynamicParameters functionProviderDynamicParameters = base.DynamicParameters as FunctionProviderDynamicParameters;
			bool flag = functionProviderDynamicParameters != null && functionProviderDynamicParameters.OptionsSet;
			if (value == null)
			{
				if (!flag)
				{
					this.RemoveSessionStateItem(name);
					return;
				}
				CommandInfo commandInfo = (CommandInfo)this.GetSessionStateItem(name);
				if (commandInfo != null)
				{
					FunctionProvider.SetOptions(commandInfo, functionProviderDynamicParameters.Options);
					return;
				}
			}
			else
			{
				PSObject psobject = value as PSObject;
				if (psobject != null)
				{
					value = psobject.BaseObject;
				}
				ScriptBlock scriptBlock = value as ScriptBlock;
				CommandInfo commandInfo;
				if (scriptBlock != null)
				{
					if (flag)
					{
						commandInfo = base.SessionState.Internal.SetFunction(name, scriptBlock, null, functionProviderDynamicParameters.Options, base.Force, base.Context.Origin);
					}
					else
					{
						commandInfo = base.SessionState.Internal.SetFunction(name, scriptBlock, null, base.Force, base.Context.Origin);
					}
				}
				else
				{
					FunctionInfo functionInfo = value as FunctionInfo;
					if (functionInfo != null)
					{
						ScopedItemOptions options = functionInfo.Options;
						if (flag)
						{
							options = functionProviderDynamicParameters.Options;
						}
						commandInfo = base.SessionState.Internal.SetFunction(name, functionInfo.ScriptBlock, functionInfo, options, base.Force, base.Context.Origin);
					}
					else
					{
						string text = value as string;
						if (text == null)
						{
							throw PSTraceSource.NewArgumentException("value");
						}
						ScriptBlock function = ScriptBlock.Create(base.Context.ExecutionContext, text);
						if (flag)
						{
							commandInfo = base.SessionState.Internal.SetFunction(name, function, null, functionProviderDynamicParameters.Options, base.Force, base.Context.Origin);
						}
						else
						{
							commandInfo = base.SessionState.Internal.SetFunction(name, function, null, base.Force, base.Context.Origin);
						}
					}
				}
				if (writeItem && commandInfo != null)
				{
					base.WriteItemObject(commandInfo, commandInfo.Name, false);
				}
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x00195437 File Offset: 0x00193637
		private static void SetOptions(CommandInfo function, ScopedItemOptions options)
		{
			((FunctionInfo)function).Options = options;
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00195445 File Offset: 0x00193645
		internal override void RemoveSessionStateItem(string name)
		{
			base.SessionState.Internal.RemoveFunction(name, base.Force);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x00195464 File Offset: 0x00193664
		internal override object GetValueOfItem(object item)
		{
			object result = item;
			FunctionInfo functionInfo = item as FunctionInfo;
			if (functionInfo != null)
			{
				result = functionInfo.ScriptBlock;
			}
			return result;
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x00195485 File Offset: 0x00193685
		internal override IDictionary GetSessionStateTable()
		{
			return base.SessionState.Internal.GetFunctionTable();
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00195498 File Offset: 0x00193698
		internal override bool CanRenameItem(object item)
		{
			bool result = false;
			FunctionInfo functionInfo = item as FunctionInfo;
			if (functionInfo != null)
			{
				if ((functionInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || ((functionInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None && !base.Force))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(functionInfo.Name, SessionStateCategory.Function, "CannotRenameFunction", SessionStateStrings.CannotRenameFunction);
					throw ex;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04002537 RID: 9527
		public const string ProviderName = "Function";
	}
}
