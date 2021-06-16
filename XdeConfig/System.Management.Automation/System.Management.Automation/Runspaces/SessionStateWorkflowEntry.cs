using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000831 RID: 2097
	public sealed class SessionStateWorkflowEntry : SessionStateCommandEntry
	{
		// Token: 0x06005039 RID: 20537 RVA: 0x001A825E File Offset: 0x001A645E
		public SessionStateWorkflowEntry(string name, string definition, ScopedItemOptions options, string helpFile) : base(name, SessionStateEntryVisibility.Public)
		{
			this._definition = definition;
			this._commandType = CommandTypes.Workflow;
			this._options = options;
			this._helpFile = helpFile;
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x001A8289 File Offset: 0x001A6489
		public SessionStateWorkflowEntry(string name, string definition, string helpFile) : this(name, definition, ScopedItemOptions.None, helpFile)
		{
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x001A8295 File Offset: 0x001A6495
		public SessionStateWorkflowEntry(string name, string definition) : this(name, definition, ScopedItemOptions.None, null)
		{
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x001A82A1 File Offset: 0x001A64A1
		internal SessionStateWorkflowEntry(string name, string definition, ScopedItemOptions options, SessionStateEntryVisibility visibility, WorkflowInfo workflow, string helpFile) : base(name, visibility)
		{
			this._definition = definition;
			this._options = options;
			this._workflow = workflow;
			this._helpFile = helpFile;
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x001A82CC File Offset: 0x001A64CC
		public override InitialSessionStateEntry Clone()
		{
			SessionStateWorkflowEntry sessionStateWorkflowEntry = new SessionStateWorkflowEntry(base.Name, this._definition, this._options, base.Visibility, this._workflow, this._helpFile);
			sessionStateWorkflowEntry.SetModule(base.Module);
			return sessionStateWorkflowEntry;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001A8310 File Offset: 0x001A6510
		internal void SetHelpFile(string help)
		{
			this._helpFile = help;
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x0600503F RID: 20543 RVA: 0x001A8319 File Offset: 0x001A6519
		public string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x001A8321 File Offset: 0x001A6521
		// (set) Token: 0x06005041 RID: 20545 RVA: 0x001A8329 File Offset: 0x001A6529
		internal WorkflowInfo WorkflowInfo
		{
			get
			{
				return this._workflow;
			}
			set
			{
				this._workflow = value;
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x001A8332 File Offset: 0x001A6532
		public ScopedItemOptions Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x001A833A File Offset: 0x001A653A
		public string HelpFile
		{
			get
			{
				return this._helpFile;
			}
		}

		// Token: 0x04002902 RID: 10498
		private string _definition;

		// Token: 0x04002903 RID: 10499
		private WorkflowInfo _workflow;

		// Token: 0x04002904 RID: 10500
		private ScopedItemOptions _options;

		// Token: 0x04002905 RID: 10501
		private string _helpFile;
	}
}
