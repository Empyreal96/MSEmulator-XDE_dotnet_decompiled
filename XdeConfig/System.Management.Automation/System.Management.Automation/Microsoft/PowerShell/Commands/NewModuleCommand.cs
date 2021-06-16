using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000B2 RID: 178
	[OutputType(new Type[]
	{
		typeof(PSModuleInfo)
	})]
	[Cmdlet("New", "Module", DefaultParameterSetName = "ScriptBlock", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141554")]
	public sealed class NewModuleCommand : ModuleCmdletBase
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00038112 File Offset: 0x00036312
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00038109 File Offset: 0x00036309
		[Parameter(ParameterSetName = "Name", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0003811A File Offset: 0x0003631A
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00038122 File Offset: 0x00036322
		[ValidateNotNull]
		[Parameter(ParameterSetName = "Name", Mandatory = true, Position = 1)]
		[Parameter(ParameterSetName = "ScriptBlock", Mandatory = true, Position = 0)]
		public ScriptBlock ScriptBlock
		{
			get
			{
				return this._scriptBlock;
			}
			set
			{
				this._scriptBlock = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0003817A File Offset: 0x0003637A
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x0003812C File Offset: 0x0003632C
		[Parameter]
		[ValidateNotNull]
		public string[] Function
		{
			get
			{
				return this._functionImportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._functionImportList = value;
				base.BaseFunctionPatterns = new List<WildcardPattern>();
				foreach (string pattern in this._functionImportList)
				{
					base.BaseFunctionPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x000381D2 File Offset: 0x000363D2
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00038184 File Offset: 0x00036384
		[ValidateNotNull]
		[Parameter]
		public string[] Cmdlet
		{
			get
			{
				return this._cmdletImportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._cmdletImportList = value;
				base.BaseCmdletPatterns = new List<WildcardPattern>();
				foreach (string pattern in this._cmdletImportList)
				{
					base.BaseCmdletPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x000381DA File Offset: 0x000363DA
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x000381E7 File Offset: 0x000363E7
		[Parameter]
		public SwitchParameter ReturnResult
		{
			get
			{
				return this._returnResult;
			}
			set
			{
				this._returnResult = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000381F5 File Offset: 0x000363F5
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x00038202 File Offset: 0x00036402
		[Parameter]
		public SwitchParameter AsCustomObject
		{
			get
			{
				return this._asCustomObject;
			}
			set
			{
				this._asCustomObject = value;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x00038210 File Offset: 0x00036410
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x00038218 File Offset: 0x00036418
		[Parameter(ValueFromRemainingArguments = true)]
		[Alias(new string[]
		{
			"Args"
		})]
		public object[] ArgumentList
		{
			get
			{
				return this._arguments;
			}
			set
			{
				this._arguments = value;
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00038224 File Offset: 0x00036424
		protected override void EndProcessing()
		{
			if (this._scriptBlock != null)
			{
				string text = Guid.NewGuid().ToString();
				if (string.IsNullOrEmpty(this._name))
				{
					this._name = "__DynamicModule_" + text;
				}
				try
				{
					base.Context.Modules.IncrementModuleNestingDepth(this, this._name);
					List<object> sendToPipeline = null;
					PSModuleInfo psmoduleInfo = null;
					try
					{
						psmoduleInfo = base.Context.Modules.CreateModule(this._name, text, this._scriptBlock, null, out sendToPipeline, this._arguments);
						if (!psmoduleInfo.SessionState.Internal.UseExportList)
						{
							List<WildcardPattern> cmdletPatterns = (base.BaseCmdletPatterns != null) ? base.BaseCmdletPatterns : base.MatchAll;
							List<WildcardPattern> functionPatterns = (base.BaseFunctionPatterns != null) ? base.BaseFunctionPatterns : base.MatchAll;
							ModuleIntrinsics.ExportModuleMembers(this, psmoduleInfo.SessionState.Internal, functionPatterns, cmdletPatterns, base.BaseAliasPatterns, base.BaseVariablePatterns, null);
						}
					}
					catch (RuntimeException ex)
					{
						ex.ErrorRecord.PreserveInvocationInfoOnce = true;
						base.WriteError(ex.ErrorRecord);
					}
					if (psmoduleInfo != null)
					{
						if (this._returnResult)
						{
							base.ImportModuleMembers(psmoduleInfo, string.Empty);
							base.WriteObject(sendToPipeline, true);
						}
						else if (this._asCustomObject)
						{
							base.WriteObject(psmoduleInfo.AsCustomObject());
						}
						else
						{
							base.ImportModuleMembers(psmoduleInfo, string.Empty);
							base.WriteObject(psmoduleInfo);
						}
					}
				}
				finally
				{
					base.Context.Modules.DecrementModuleNestingCount();
				}
			}
		}

		// Token: 0x04000415 RID: 1045
		private string _name;

		// Token: 0x04000416 RID: 1046
		private ScriptBlock _scriptBlock;

		// Token: 0x04000417 RID: 1047
		private string[] _functionImportList = new string[0];

		// Token: 0x04000418 RID: 1048
		private string[] _cmdletImportList = new string[0];

		// Token: 0x04000419 RID: 1049
		private bool _returnResult;

		// Token: 0x0400041A RID: 1050
		private bool _asCustomObject;

		// Token: 0x0400041B RID: 1051
		private object[] _arguments;
	}
}
