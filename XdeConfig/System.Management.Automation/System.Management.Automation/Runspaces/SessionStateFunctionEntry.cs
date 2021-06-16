using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000830 RID: 2096
	public sealed class SessionStateFunctionEntry : SessionStateCommandEntry
	{
		// Token: 0x0600502E RID: 20526 RVA: 0x001A814C File Offset: 0x001A634C
		public SessionStateFunctionEntry(string name, string definition, ScopedItemOptions options, string helpFile) : base(name, SessionStateEntryVisibility.Public)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Function;
			this._options = options;
			this._scriptBlock = ScriptBlock.Create(this._definition);
			this._scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			this._helpFile = helpFile;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001A81A0 File Offset: 0x001A63A0
		public SessionStateFunctionEntry(string name, string definition, string helpFile) : this(name, definition, ScopedItemOptions.None, helpFile)
		{
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x001A81AC File Offset: 0x001A63AC
		public SessionStateFunctionEntry(string name, string definition) : this(name, definition, ScopedItemOptions.None, null)
		{
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x001A81B8 File Offset: 0x001A63B8
		internal SessionStateFunctionEntry(string name, string definition, ScopedItemOptions options, SessionStateEntryVisibility visibility, ScriptBlock scriptBlock, string helpFile) : base(name, visibility)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Function;
			this._options = options;
			this._scriptBlock = scriptBlock;
			this._helpFile = helpFile;
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x001A81E8 File Offset: 0x001A63E8
		public override InitialSessionStateEntry Clone()
		{
			SessionStateFunctionEntry sessionStateFunctionEntry = new SessionStateFunctionEntry(base.Name, this._definition, this._options, base.Visibility, this._scriptBlock, this._helpFile);
			sessionStateFunctionEntry.SetModule(base.Module);
			return sessionStateFunctionEntry;
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001A822C File Offset: 0x001A642C
		internal void SetHelpFile(string help)
		{
			this._helpFile = help;
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06005034 RID: 20532 RVA: 0x001A8235 File Offset: 0x001A6435
		public string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x001A823D File Offset: 0x001A643D
		// (set) Token: 0x06005036 RID: 20534 RVA: 0x001A8245 File Offset: 0x001A6445
		internal ScriptBlock ScriptBlock
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

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x001A824E File Offset: 0x001A644E
		public ScopedItemOptions Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005038 RID: 20536 RVA: 0x001A8256 File Offset: 0x001A6456
		public string HelpFile
		{
			get
			{
				return this._helpFile;
			}
		}

		// Token: 0x040028FE RID: 10494
		private string _definition;

		// Token: 0x040028FF RID: 10495
		private ScriptBlock _scriptBlock;

		// Token: 0x04002900 RID: 10496
		private ScopedItemOptions _options;

		// Token: 0x04002901 RID: 10497
		private string _helpFile;
	}
}
