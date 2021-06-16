using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000557 RID: 1367
	public abstract class LabeledStatementAst : StatementAst
	{
		// Token: 0x060038F7 RID: 14583 RVA: 0x0012E8A8 File Offset: 0x0012CAA8
		protected LabeledStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition) : base(extent)
		{
			if (string.IsNullOrWhiteSpace(label))
			{
				label = null;
			}
			this.Label = label;
			if (condition != null)
			{
				this.Condition = condition;
				base.SetParent(condition);
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x0012E8D4 File Offset: 0x0012CAD4
		// (set) Token: 0x060038F9 RID: 14585 RVA: 0x0012E8DC File Offset: 0x0012CADC
		public string Label { get; private set; }

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060038FA RID: 14586 RVA: 0x0012E8E5 File Offset: 0x0012CAE5
		// (set) Token: 0x060038FB RID: 14587 RVA: 0x0012E8ED File Offset: 0x0012CAED
		public PipelineBaseAst Condition { get; private set; }
	}
}
