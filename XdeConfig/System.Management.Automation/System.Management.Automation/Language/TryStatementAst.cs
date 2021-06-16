using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000562 RID: 1378
	public class TryStatementAst : StatementAst
	{
		// Token: 0x06003936 RID: 14646 RVA: 0x0012F458 File Offset: 0x0012D658
		public TryStatementAst(IScriptExtent extent, StatementBlockAst body, IEnumerable<CatchClauseAst> catchClauses, StatementBlockAst @finally) : base(extent)
		{
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if ((catchClauses == null || !catchClauses.Any<CatchClauseAst>()) && @finally == null)
			{
				throw PSTraceSource.NewArgumentException("catchClauses");
			}
			this.Body = body;
			base.SetParent(body);
			if (catchClauses != null)
			{
				this.CatchClauses = new ReadOnlyCollection<CatchClauseAst>(catchClauses.ToArray<CatchClauseAst>());
				base.SetParents<CatchClauseAst>(this.CatchClauses);
			}
			else
			{
				this.CatchClauses = TryStatementAst.EmptyCatchClauses;
			}
			if (@finally != null)
			{
				this.Finally = @finally;
				base.SetParent(@finally);
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x0012F4E3 File Offset: 0x0012D6E3
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x0012F4EB File Offset: 0x0012D6EB
		public StatementBlockAst Body { get; private set; }

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x0012F4F4 File Offset: 0x0012D6F4
		// (set) Token: 0x0600393A RID: 14650 RVA: 0x0012F4FC File Offset: 0x0012D6FC
		public ReadOnlyCollection<CatchClauseAst> CatchClauses { get; private set; }

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x0012F505 File Offset: 0x0012D705
		// (set) Token: 0x0600393C RID: 14652 RVA: 0x0012F50D File Offset: 0x0012D70D
		public StatementBlockAst Finally { get; private set; }

		// Token: 0x0600393D RID: 14653 RVA: 0x0012F518 File Offset: 0x0012D718
		public override Ast Copy()
		{
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(this.Body);
			CatchClauseAst[] catchClauses = Ast.CopyElements<CatchClauseAst>(this.CatchClauses);
			StatementBlockAst @finally = Ast.CopyElement<StatementBlockAst>(this.Finally);
			return new TryStatementAst(base.Extent, body, catchClauses, @finally);
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x0012F8E4 File Offset: 0x0012DAE4
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			foreach (PSTypeName typename in this.Body.GetInferredType(context))
			{
				yield return typename;
			}
			foreach (PSTypeName typename2 in this.CatchClauses.SelectMany((CatchClauseAst clause) => clause.Body.GetInferredType(context)))
			{
				yield return typename2;
			}
			if (this.Finally != null)
			{
				foreach (PSTypeName typename3 in this.Finally.GetInferredType(context))
				{
					yield return typename3;
				}
			}
			yield break;
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x0012F908 File Offset: 0x0012DB08
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitTryStatement(this);
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x0012F914 File Offset: 0x0012DB14
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitTryStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Body.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.CatchClauses.Count; i++)
				{
					CatchClauseAst catchClauseAst = this.CatchClauses[i];
					astVisitAction = catchClauseAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.Finally != null)
			{
				astVisitAction = this.Finally.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001D04 RID: 7428
		private static readonly ReadOnlyCollection<CatchClauseAst> EmptyCatchClauses = new ReadOnlyCollection<CatchClauseAst>(new CatchClauseAst[0]);
	}
}
