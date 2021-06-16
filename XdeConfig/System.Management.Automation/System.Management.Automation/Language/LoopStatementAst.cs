using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000558 RID: 1368
	public abstract class LoopStatementAst : LabeledStatementAst
	{
		// Token: 0x060038FC RID: 14588 RVA: 0x0012E8F6 File Offset: 0x0012CAF6
		protected LoopStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition, StatementBlockAst body) : base(extent, label, condition)
		{
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x0012E920 File Offset: 0x0012CB20
		// (set) Token: 0x060038FE RID: 14590 RVA: 0x0012E928 File Offset: 0x0012CB28
		public StatementBlockAst Body { get; private set; }

		// Token: 0x060038FF RID: 14591 RVA: 0x0012E931 File Offset: 0x0012CB31
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}
	}
}
