using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000067 RID: 103
	public class FunctionInfo : CommandInfo, IScriptCommandInfo
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x0001A1C9 File Offset: 0x000183C9
		internal FunctionInfo(string name, ScriptBlock function, ExecutionContext context) : this(name, function, context, null)
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001A1D8 File Offset: 0x000183D8
		internal FunctionInfo(string name, ScriptBlock function, ExecutionContext context, string helpFile)
		{
			this.verb = string.Empty;
			this.noun = string.Empty;
			this._helpFile = string.Empty;
			base..ctor(name, CommandTypes.Function, context);
			if (function == null)
			{
				throw PSTraceSource.NewArgumentNullException("function");
			}
			this._scriptBlock = function;
			CmdletInfo.SplitCmdletName(name, out this.verb, out this.noun);
			base.SetModule(function.Module);
			this._helpFile = helpFile;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001A24B File Offset: 0x0001844B
		internal FunctionInfo(string name, ScriptBlock function, ScopedItemOptions options, ExecutionContext context) : this(name, function, options, context, null)
		{
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001A259 File Offset: 0x00018459
		internal FunctionInfo(string name, ScriptBlock function, ScopedItemOptions options, ExecutionContext context, string helpFile) : this(name, function, context, helpFile)
		{
			this._options = options;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001A26E File Offset: 0x0001846E
		internal FunctionInfo(FunctionInfo other)
		{
			this.verb = string.Empty;
			this.noun = string.Empty;
			this._helpFile = string.Empty;
			base..ctor(other);
			this.CopyFieldsFromOther(other);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001A2A0 File Offset: 0x000184A0
		private void CopyFieldsFromOther(FunctionInfo other)
		{
			this.verb = other.verb;
			this.noun = other.noun;
			this._scriptBlock = other._scriptBlock;
			this._description = other._description;
			this._options = other._options;
			this._helpFile = other._helpFile;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001A2F8 File Offset: 0x000184F8
		internal FunctionInfo(string name, FunctionInfo other)
		{
			this.verb = string.Empty;
			this.noun = string.Empty;
			this._helpFile = string.Empty;
			base..ctor(name, other);
			this.CopyFieldsFromOther(other);
			CmdletInfo.SplitCmdletName(name, out this.verb, out this.noun);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001A348 File Offset: 0x00018548
		internal override CommandInfo CreateGetCommandCopy(object[] arguments)
		{
			return new FunctionInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = arguments
			};
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001A36D File Offset: 0x0001856D
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Function;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001A374 File Offset: 0x00018574
		public ScriptBlock ScriptBlock
		{
			get
			{
				return this._scriptBlock;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001A37C File Offset: 0x0001857C
		internal void Update(ScriptBlock newFunction, bool force, ScopedItemOptions options)
		{
			this.Update(newFunction, force, options, null);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001A388 File Offset: 0x00018588
		protected internal virtual void Update(FunctionInfo newFunction, bool force, ScopedItemOptions options, string helpFile)
		{
			this.Update(newFunction.ScriptBlock, force, options, helpFile);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001A39C File Offset: 0x0001859C
		internal void Update(ScriptBlock newFunction, bool force, ScopedItemOptions options, string helpFile)
		{
			if (newFunction == null)
			{
				throw PSTraceSource.NewArgumentNullException("function");
			}
			if ((this._options & ScopedItemOptions.Constant) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Function, "FunctionIsConstant", SessionStateStrings.FunctionIsConstant);
				throw ex;
			}
			if (!force && (this._options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Function, "FunctionIsReadOnly", SessionStateStrings.FunctionIsReadOnly);
				throw ex2;
			}
			this._scriptBlock = newFunction;
			base.SetModule(newFunction.Module);
			this._commandMetadata = null;
			this._parameterSets = null;
			base.ExternalCommandMetadata = null;
			if (options != ScopedItemOptions.Unspecified)
			{
				this.Options = options;
			}
			this._helpFile = helpFile;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001A43C File Offset: 0x0001863C
		public bool CmdletBinding
		{
			get
			{
				return this.ScriptBlock.UsesCmdletBinding;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001A449 File Offset: 0x00018649
		public string DefaultParameterSet
		{
			get
			{
				if (!this.CmdletBinding)
				{
					return null;
				}
				return this.CommandMetadata.DefaultParameterSetName;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001A460 File Offset: 0x00018660
		public override string Definition
		{
			get
			{
				return this._scriptBlock.ToString();
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001A46D File Offset: 0x0001866D
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x0001A490 File Offset: 0x00018690
		public ScopedItemOptions Options
		{
			get
			{
				if (base.CopiedCommand != null)
				{
					return ((FunctionInfo)base.CopiedCommand).Options;
				}
				return this._options;
			}
			set
			{
				if (base.CopiedCommand != null)
				{
					((FunctionInfo)base.CopiedCommand).Options = value;
					return;
				}
				if ((this._options & ScopedItemOptions.Constant) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Function, "FunctionIsConstant", SessionStateStrings.FunctionIsConstant);
					throw ex;
				}
				if ((value & ScopedItemOptions.Constant) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Function, "FunctionCannotBeMadeConstant", SessionStateStrings.FunctionCannotBeMadeConstant);
					throw ex2;
				}
				if ((value & ScopedItemOptions.AllScope) == ScopedItemOptions.None && (this._options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Function, "FunctionAllScopeOptionCannotBeRemoved", SessionStateStrings.FunctionAllScopeOptionCannotBeRemoved);
					throw ex3;
				}
				this._options = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001A527 File Offset: 0x00018727
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x0001A548 File Offset: 0x00018748
		public string Description
		{
			get
			{
				if (base.CopiedCommand != null)
				{
					return ((FunctionInfo)base.CopiedCommand).Description;
				}
				return this._description;
			}
			set
			{
				if (base.CopiedCommand == null)
				{
					this._description = value;
					return;
				}
				((FunctionInfo)base.CopiedCommand).Description = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001A56B File Offset: 0x0001876B
		public string Verb
		{
			get
			{
				return this.verb;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001A573 File Offset: 0x00018773
		public string Noun
		{
			get
			{
				return this.noun;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0001A57B File Offset: 0x0001877B
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0001A583 File Offset: 0x00018783
		public string HelpFile
		{
			get
			{
				return this._helpFile;
			}
			internal set
			{
				this._helpFile = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001A58C File Offset: 0x0001878C
		internal override string Syntax
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (CommandParameterSetInfo commandParameterSetInfo in base.ParameterSets)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0} {1}", new object[]
					{
						base.Name,
						commandParameterSetInfo.ToString((base.CommandType & CommandTypes.Workflow) == CommandTypes.Workflow)
					}));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001A628 File Offset: 0x00018828
		internal override bool ImplementsDynamicParameters
		{
			get
			{
				return this.ScriptBlock.HasDynamicParameters;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001A638 File Offset: 0x00018838
		internal override CommandMetadata CommandMetadata
		{
			get
			{
				CommandMetadata result;
				if ((result = this._commandMetadata) == null)
				{
					result = (this._commandMetadata = new CommandMetadata(this.ScriptBlock, base.Name, LocalPipeline.GetExecutionContextFromTLS()));
				}
				return result;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001A66E File Offset: 0x0001886E
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				return this.ScriptBlock.OutputType;
			}
		}

		// Token: 0x04000239 RID: 569
		private ScriptBlock _scriptBlock;

		// Token: 0x0400023A RID: 570
		private ScopedItemOptions _options;

		// Token: 0x0400023B RID: 571
		private string _description;

		// Token: 0x0400023C RID: 572
		private string verb;

		// Token: 0x0400023D RID: 573
		private string noun;

		// Token: 0x0400023E RID: 574
		private string _helpFile;

		// Token: 0x0400023F RID: 575
		private CommandMetadata _commandMetadata;
	}
}
