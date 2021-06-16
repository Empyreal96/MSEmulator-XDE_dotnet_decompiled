using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000568 RID: 1384
	public class ThrowStatementAst : StatementAst
	{
		// Token: 0x06003967 RID: 14695 RVA: 0x0012FD42 File Offset: 0x0012DF42
		public ThrowStatementAst(IScriptExtent extent, PipelineBaseAst pipeline) : base(extent)
		{
			if (pipeline != null)
			{
				this.Pipeline = pipeline;
				base.SetParent(pipeline);
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x0012FD5C File Offset: 0x0012DF5C
		// (set) Token: 0x06003969 RID: 14697 RVA: 0x0012FD64 File Offset: 0x0012DF64
		public PipelineBaseAst Pipeline { get; private set; }

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x0012FD70 File Offset: 0x0012DF70
		public bool IsRethrow
		{
			get
			{
				if (this.Pipeline != null)
				{
					return false;
				}
				for (Ast parent = base.Parent; parent != null; parent = parent.Parent)
				{
					if (parent is CatchClauseAst)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0012FDA8 File Offset: 0x0012DFA8
		public override Ast Copy()
		{
			PipelineBaseAst pipeline = Ast.CopyElement<PipelineBaseAst>(this.Pipeline);
			return new ThrowStatementAst(base.Extent, pipeline);
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x0012FDCD File Offset: 0x0012DFCD
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0012FDD4 File Offset: 0x0012DFD4
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitThrowStatement(this);
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x0012FDE0 File Offset: 0x0012DFE0
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitThrowStatement(this);
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
