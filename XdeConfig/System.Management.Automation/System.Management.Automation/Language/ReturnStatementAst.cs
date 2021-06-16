using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000566 RID: 1382
	public class ReturnStatementAst : StatementAst
	{
		// Token: 0x06003959 RID: 14681 RVA: 0x0012FBF2 File Offset: 0x0012DDF2
		public ReturnStatementAst(IScriptExtent extent, PipelineBaseAst pipeline) : base(extent)
		{
			if (pipeline != null)
			{
				this.Pipeline = pipeline;
				base.SetParent(pipeline);
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x0012FC0C File Offset: 0x0012DE0C
		// (set) Token: 0x0600395B RID: 14683 RVA: 0x0012FC14 File Offset: 0x0012DE14
		public PipelineBaseAst Pipeline { get; private set; }

		// Token: 0x0600395C RID: 14684 RVA: 0x0012FC20 File Offset: 0x0012DE20
		public override Ast Copy()
		{
			PipelineBaseAst pipeline = Ast.CopyElement<PipelineBaseAst>(this.Pipeline);
			return new ReturnStatementAst(base.Extent, pipeline);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0012FC45 File Offset: 0x0012DE45
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0012FC4C File Offset: 0x0012DE4C
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitReturnStatement(this);
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0012FC58 File Offset: 0x0012DE58
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitReturnStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.Pipeline != null)
			{
				astVisitAction = this.Pipeline.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
