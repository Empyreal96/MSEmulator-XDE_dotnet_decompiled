using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000567 RID: 1383
	public class ExitStatementAst : StatementAst
	{
		// Token: 0x06003960 RID: 14688 RVA: 0x0012FC9A File Offset: 0x0012DE9A
		public ExitStatementAst(IScriptExtent extent, PipelineBaseAst pipeline) : base(extent)
		{
			if (pipeline != null)
			{
				this.Pipeline = pipeline;
				base.SetParent(pipeline);
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x0012FCB4 File Offset: 0x0012DEB4
		// (set) Token: 0x06003962 RID: 14690 RVA: 0x0012FCBC File Offset: 0x0012DEBC
		public PipelineBaseAst Pipeline { get; private set; }

		// Token: 0x06003963 RID: 14691 RVA: 0x0012FCC8 File Offset: 0x0012DEC8
		public override Ast Copy()
		{
			PipelineBaseAst pipeline = Ast.CopyElement<PipelineBaseAst>(this.Pipeline);
			return new ExitStatementAst(base.Extent, pipeline);
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0012FCED File Offset: 0x0012DEED
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0012FCF4 File Offset: 0x0012DEF4
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitExitStatement(this);
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0012FD00 File Offset: 0x0012DF00
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitExitStatement(this);
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
