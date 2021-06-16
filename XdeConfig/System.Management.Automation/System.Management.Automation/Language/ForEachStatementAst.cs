using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055A RID: 1370
	public class ForEachStatementAst : LoopStatementAst
	{
		// Token: 0x06003900 RID: 14592 RVA: 0x0012E940 File Offset: 0x0012CB40
		public ForEachStatementAst(IScriptExtent extent, string label, ForEachFlags flags, VariableExpressionAst variable, PipelineBaseAst expression, StatementBlockAst body) : base(extent, label, expression, body)
		{
			if (expression == null || variable == null)
			{
				throw PSTraceSource.NewArgumentNullException((expression == null) ? "expression" : "variablePath");
			}
			this.Flags = flags;
			this.Variable = variable;
			base.SetParent(variable);
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0012E98E File Offset: 0x0012CB8E
		public ForEachStatementAst(IScriptExtent extent, string label, ForEachFlags flags, ExpressionAst throttleLimit, VariableExpressionAst variable, PipelineBaseAst expression, StatementBlockAst body) : this(extent, label, flags, variable, expression, body)
		{
			this.ThrottleLimit = throttleLimit;
			if (throttleLimit != null)
			{
				base.SetParent(throttleLimit);
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x0012E9B3 File Offset: 0x0012CBB3
		// (set) Token: 0x06003903 RID: 14595 RVA: 0x0012E9BB File Offset: 0x0012CBBB
		public VariableExpressionAst Variable { get; private set; }

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0012E9C4 File Offset: 0x0012CBC4
		// (set) Token: 0x06003905 RID: 14597 RVA: 0x0012E9CC File Offset: 0x0012CBCC
		public ExpressionAst ThrottleLimit { get; private set; }

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x0012E9D5 File Offset: 0x0012CBD5
		// (set) Token: 0x06003907 RID: 14599 RVA: 0x0012E9DD File Offset: 0x0012CBDD
		public ForEachFlags Flags { get; private set; }

		// Token: 0x06003908 RID: 14600 RVA: 0x0012E9E8 File Offset: 0x0012CBE8
		public override Ast Copy()
		{
			VariableExpressionAst variable = Ast.CopyElement<VariableExpressionAst>(this.Variable);
			PipelineBaseAst expression = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(base.Body);
			if (this.ThrottleLimit != null)
			{
				ExpressionAst throttleLimit = Ast.CopyElement<ExpressionAst>(this.ThrottleLimit);
				return new ForEachStatementAst(base.Extent, base.Label, this.Flags, throttleLimit, variable, expression, body);
			}
			return new ForEachStatementAst(base.Extent, base.Label, this.Flags, variable, expression, body);
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0012EA63 File Offset: 0x0012CC63
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitForEachStatement(this);
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0012EA6C File Offset: 0x0012CC6C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitForEachStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Variable.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Condition.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
