using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000555 RID: 1365
	public class IfStatementAst : StatementAst
	{
		// Token: 0x060038DD RID: 14557 RVA: 0x0012E27C File Offset: 0x0012C47C
		public IfStatementAst(IScriptExtent extent, IEnumerable<Tuple<PipelineBaseAst, StatementBlockAst>> clauses, StatementBlockAst elseClause) : base(extent)
		{
			if (clauses == null || !clauses.Any<Tuple<PipelineBaseAst, StatementBlockAst>>())
			{
				throw PSTraceSource.NewArgumentException("clauses");
			}
			this.Clauses = new ReadOnlyCollection<Tuple<PipelineBaseAst, StatementBlockAst>>(clauses.ToArray<Tuple<PipelineBaseAst, StatementBlockAst>>());
			base.SetParents<PipelineBaseAst, StatementBlockAst>(this.Clauses);
			if (elseClause != null)
			{
				this.ElseClause = elseClause;
				base.SetParent(elseClause);
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060038DE RID: 14558 RVA: 0x0012E2D4 File Offset: 0x0012C4D4
		// (set) Token: 0x060038DF RID: 14559 RVA: 0x0012E2DC File Offset: 0x0012C4DC
		public ReadOnlyCollection<Tuple<PipelineBaseAst, StatementBlockAst>> Clauses { get; private set; }

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060038E0 RID: 14560 RVA: 0x0012E2E5 File Offset: 0x0012C4E5
		// (set) Token: 0x060038E1 RID: 14561 RVA: 0x0012E2ED File Offset: 0x0012C4ED
		public StatementBlockAst ElseClause { get; private set; }

		// Token: 0x060038E2 RID: 14562 RVA: 0x0012E2F8 File Offset: 0x0012C4F8
		public override Ast Copy()
		{
			List<Tuple<PipelineBaseAst, StatementBlockAst>> list = new List<Tuple<PipelineBaseAst, StatementBlockAst>>(this.Clauses.Count);
			for (int i = 0; i < this.Clauses.Count; i++)
			{
				Tuple<PipelineBaseAst, StatementBlockAst> tuple = this.Clauses[i];
				PipelineBaseAst item = Ast.CopyElement<PipelineBaseAst>(tuple.Item1);
				StatementBlockAst item2 = Ast.CopyElement<StatementBlockAst>(tuple.Item2);
				list.Add(Tuple.Create<PipelineBaseAst, StatementBlockAst>(item, item2));
			}
			StatementBlockAst elseClause = Ast.CopyElement<StatementBlockAst>(this.ElseClause);
			return new IfStatementAst(base.Extent, list, elseClause);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x0012E640 File Offset: 0x0012C840
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			foreach (PSTypeName typename in this.Clauses.SelectMany((Tuple<PipelineBaseAst, StatementBlockAst> clause) => clause.Item2.GetInferredType(context)))
			{
				yield return typename;
			}
			if (this.ElseClause != null)
			{
				foreach (PSTypeName typename2 in this.ElseClause.GetInferredType(context))
				{
					yield return typename2;
				}
			}
			yield break;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0012E664 File Offset: 0x0012C864
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitIfStatement(this);
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0012E670 File Offset: 0x0012C870
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitIfStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.Clauses.Count; i++)
				{
					Tuple<PipelineBaseAst, StatementBlockAst> tuple = this.Clauses[i];
					astVisitAction = tuple.Item1.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
					astVisitAction = tuple.Item2.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.ElseClause != null)
			{
				astVisitAction = this.ElseClause.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
