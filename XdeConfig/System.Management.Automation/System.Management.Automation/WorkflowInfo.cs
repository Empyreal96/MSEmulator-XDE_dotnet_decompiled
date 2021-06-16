using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200008A RID: 138
	public class WorkflowInfo : FunctionInfo
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x00022188 File Offset: 0x00020388
		public WorkflowInfo(string name, string definition, ScriptBlock workflow, string xamlDefinition, WorkflowInfo[] workflowsCalled) : this(name, workflow, null)
		{
			if (string.IsNullOrEmpty(xamlDefinition))
			{
				throw PSTraceSource.NewArgumentNullException("xamlDefinition");
			}
			this._definition = definition;
			this.XamlDefinition = xamlDefinition;
			if (workflowsCalled != null)
			{
				this._workflowsCalled = new ReadOnlyCollection<WorkflowInfo>(workflowsCalled);
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000221C7 File Offset: 0x000203C7
		public WorkflowInfo(string name, string definition, ScriptBlock workflow, string xamlDefinition, WorkflowInfo[] workflowsCalled, PSModuleInfo module) : this(name, definition, workflow, xamlDefinition, workflowsCalled)
		{
			base.SetModule(module);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000221DE File Offset: 0x000203DE
		internal WorkflowInfo(string name, ScriptBlock workflow, ExecutionContext context) : this(name, workflow, context, null)
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000221EA File Offset: 0x000203EA
		internal WorkflowInfo(string name, ScriptBlock workflow, ExecutionContext context, string helpFile)
		{
			this._definition = "";
			base..ctor(name, workflow, context, helpFile);
			base.SetCommandType(CommandTypes.Workflow);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002220D File Offset: 0x0002040D
		internal WorkflowInfo(string name, ScriptBlock workflow, ScopedItemOptions options, ExecutionContext context) : this(name, workflow, options, context, null)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002221B File Offset: 0x0002041B
		internal WorkflowInfo(string name, ScriptBlock workflow, ScopedItemOptions options, ExecutionContext context, string helpFile)
		{
			this._definition = "";
			base..ctor(name, workflow, options, context, helpFile);
			base.SetCommandType(CommandTypes.Workflow);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00022240 File Offset: 0x00020440
		internal WorkflowInfo(WorkflowInfo other)
		{
			this._definition = "";
			base..ctor(other);
			base.SetCommandType(CommandTypes.Workflow);
			this.CopyFields(other);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00022266 File Offset: 0x00020466
		internal WorkflowInfo(string name, WorkflowInfo other)
		{
			this._definition = "";
			base..ctor(name, other);
			base.SetCommandType(CommandTypes.Workflow);
			this.CopyFields(other);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002228D File Offset: 0x0002048D
		private void CopyFields(WorkflowInfo other)
		{
			this.XamlDefinition = other.XamlDefinition;
			this.NestedXamlDefinition = other.NestedXamlDefinition;
			this._workflowsCalled = other.WorkflowsCalled;
			this._definition = other.Definition;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000222C0 File Offset: 0x000204C0
		protected internal override void Update(FunctionInfo function, bool force, ScopedItemOptions options, string helpFile)
		{
			WorkflowInfo workflowInfo = function as WorkflowInfo;
			if (workflowInfo == null)
			{
				throw PSTraceSource.NewArgumentException("function");
			}
			base.Update(function, force, options, helpFile);
			this.CopyFields(workflowInfo);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000222F4 File Offset: 0x000204F4
		internal override CommandInfo CreateGetCommandCopy(object[] arguments)
		{
			return new WorkflowInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = arguments
			};
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00022317 File Offset: 0x00020517
		public override string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002231F File Offset: 0x0002051F
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00022327 File Offset: 0x00020527
		public string XamlDefinition { get; internal set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00022330 File Offset: 0x00020530
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00022338 File Offset: 0x00020538
		public string NestedXamlDefinition { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00022341 File Offset: 0x00020541
		public ReadOnlyCollection<WorkflowInfo> WorkflowsCalled
		{
			get
			{
				return this._workflowsCalled ?? WorkflowInfo.EmptyCalledWorkflows;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00022352 File Offset: 0x00020552
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Workflow;
			}
		}

		// Token: 0x040002FB RID: 763
		private string _definition;

		// Token: 0x040002FC RID: 764
		private ReadOnlyCollection<WorkflowInfo> _workflowsCalled;

		// Token: 0x040002FD RID: 765
		private static ReadOnlyCollection<WorkflowInfo> EmptyCalledWorkflows = new ReadOnlyCollection<WorkflowInfo>(new WorkflowInfo[0]);
	}
}
