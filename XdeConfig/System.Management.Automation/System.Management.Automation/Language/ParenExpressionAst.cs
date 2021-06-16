using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058E RID: 1422
	public class ParenExpressionAst : ExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003AFB RID: 15099 RVA: 0x001368E6 File Offset: 0x00134AE6
		public ParenExpressionAst(IScriptExtent extent, PipelineBaseAst pipeline) : base(extent)
		{
			if (pipeline == null)
			{
				throw PSTraceSource.NewArgumentNullException("pipeline");
			}
			this.Pipeline = pipeline;
			base.SetParent(pipeline);
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x0013690B File Offset: 0x00134B0B
		// (set) Token: 0x06003AFD RID: 15101 RVA: 0x00136913 File Offset: 0x00134B13
		public PipelineBaseAst Pipeline { get; private set; }

		// Token: 0x06003AFE RID: 15102 RVA: 0x0013691C File Offset: 0x00134B1C
		public override Ast Copy()
		{
			PipelineBaseAst pipeline = Ast.CopyElement<PipelineBaseAst>(this.Pipeline);
			return new ParenExpressionAst(base.Extent, pipeline);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x00136941 File Offset: 0x00134B41
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Pipeline.GetInferredType(context);
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0013694F File Offset: 0x00134B4F
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitParenExpression(this);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x00136958 File Offset: 0x00134B58
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitParenExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Pipeline.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x00136992 File Offset: 0x00134B92
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return ((ISupportsAssignment)this.Pipeline.GetPureExpression()).GetAssignableValue();
		}
	}
}
