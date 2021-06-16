using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000A4 RID: 164
	[Cmdlet("Export", "ModuleMember", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141551")]
	public sealed class ExportModuleMemberCommand : PSCmdlet
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00027786 File Offset: 0x00025986
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00027734 File Offset: 0x00025934
		[AllowEmptyCollection]
		[Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
		public string[] Function
		{
			get
			{
				return this._functionList;
			}
			set
			{
				this._functionList = value;
				this._functionPatterns = new List<WildcardPattern>();
				if (this._functionList != null)
				{
					foreach (string pattern in this._functionList)
					{
						this._functionPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
					}
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000277E2 File Offset: 0x000259E2
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x00027790 File Offset: 0x00025990
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[AllowEmptyCollection]
		public string[] Cmdlet
		{
			get
			{
				return this._cmdletList;
			}
			set
			{
				this._cmdletList = value;
				this._cmdletPatterns = new List<WildcardPattern>();
				if (this._cmdletList != null)
				{
					foreach (string pattern in this._cmdletList)
					{
						this._cmdletPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
					}
				}
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002783E File Offset: 0x00025A3E
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x000277EC File Offset: 0x000259EC
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[ValidateNotNull]
		public string[] Variable
		{
			get
			{
				return this._variableExportList;
			}
			set
			{
				this._variableExportList = value;
				this._variablePatterns = new List<WildcardPattern>();
				if (this._variableExportList != null)
				{
					foreach (string pattern in this._variableExportList)
					{
						this._variablePatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
					}
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0002789A File Offset: 0x00025A9A
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00027848 File Offset: 0x00025A48
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[ValidateNotNull]
		public string[] Alias
		{
			get
			{
				return this._aliasExportList;
			}
			set
			{
				this._aliasExportList = value;
				this._aliasPatterns = new List<WildcardPattern>();
				if (this._aliasExportList != null)
				{
					foreach (string pattern in this._aliasExportList)
					{
						this._aliasPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
					}
				}
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000278A4 File Offset: 0x00025AA4
		protected override void ProcessRecord()
		{
			if (base.Context.EngineSessionState == base.Context.TopLevelSessionState)
			{
				string message = StringUtil.Format(Modules.CanOnlyBeUsedFromWithinAModule, new object[0]);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_CanOnlyExecuteExportModuleMemberInsideAModule", ErrorCategory.PermissionDenied, null);
				base.ThrowTerminatingError(errorRecord);
			}
			ModuleIntrinsics.ExportModuleMembers(this, base.Context.EngineSessionState, this._functionPatterns, this._cmdletPatterns, this._aliasPatterns, this._variablePatterns, null);
		}

		// Token: 0x04000397 RID: 919
		private string[] _functionList;

		// Token: 0x04000398 RID: 920
		private List<WildcardPattern> _functionPatterns;

		// Token: 0x04000399 RID: 921
		private string[] _cmdletList;

		// Token: 0x0400039A RID: 922
		private List<WildcardPattern> _cmdletPatterns;

		// Token: 0x0400039B RID: 923
		private string[] _variableExportList;

		// Token: 0x0400039C RID: 924
		private List<WildcardPattern> _variablePatterns;

		// Token: 0x0400039D RID: 925
		private string[] _aliasExportList;

		// Token: 0x0400039E RID: 926
		private List<WildcardPattern> _aliasPatterns;
	}
}
