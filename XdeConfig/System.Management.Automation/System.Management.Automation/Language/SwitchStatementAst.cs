using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000560 RID: 1376
	public class SwitchStatementAst : LabeledStatementAst
	{
		// Token: 0x0600391F RID: 14623 RVA: 0x0012EE00 File Offset: 0x0012D000
		public SwitchStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition, SwitchFlags flags, IEnumerable<Tuple<ExpressionAst, StatementBlockAst>> clauses, StatementBlockAst @default) : base(extent, label, condition)
		{
			if ((clauses == null || !clauses.Any<Tuple<ExpressionAst, StatementBlockAst>>()) && @default == null)
			{
				throw PSTraceSource.NewArgumentException("clauses");
			}
			this.Flags = flags;
			this.Clauses = new ReadOnlyCollection<Tuple<ExpressionAst, StatementBlockAst>>((clauses != null && clauses.Any<Tuple<ExpressionAst, StatementBlockAst>>()) ? clauses.ToArray<Tuple<ExpressionAst, StatementBlockAst>>() : SwitchStatementAst.EmptyClauseArray);
			base.SetParents<ExpressionAst, StatementBlockAst>(this.Clauses);
			if (@default != null)
			{
				this.Default = @default;
				base.SetParent(@default);
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x0012EE80 File Offset: 0x0012D080
		// (set) Token: 0x06003921 RID: 14625 RVA: 0x0012EE88 File Offset: 0x0012D088
		public SwitchFlags Flags { get; private set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x0012EE91 File Offset: 0x0012D091
		// (set) Token: 0x06003923 RID: 14627 RVA: 0x0012EE99 File Offset: 0x0012D099
		public ReadOnlyCollection<Tuple<ExpressionAst, StatementBlockAst>> Clauses { get; private set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x0012EEA2 File Offset: 0x0012D0A2
		// (set) Token: 0x06003925 RID: 14629 RVA: 0x0012EEAA File Offset: 0x0012D0AA
		public StatementBlockAst Default { get; private set; }

		// Token: 0x06003926 RID: 14630 RVA: 0x0012EEB4 File Offset: 0x0012D0B4
		public override Ast Copy()
		{
			PipelineBaseAst condition = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			StatementBlockAst @default = Ast.CopyElement<StatementBlockAst>(this.Default);
			List<Tuple<ExpressionAst, StatementBlockAst>> list = null;
			if (this.Clauses.Count > 0)
			{
				list = new List<Tuple<ExpressionAst, StatementBlockAst>>(this.Clauses.Count);
				for (int i = 0; i < this.Clauses.Count; i++)
				{
					Tuple<ExpressionAst, StatementBlockAst> tuple = this.Clauses[i];
					ExpressionAst item = Ast.CopyElement<ExpressionAst>(tuple.Item1);
					StatementBlockAst item2 = Ast.CopyElement<StatementBlockAst>(tuple.Item2);
					list.Add(Tuple.Create<ExpressionAst, StatementBlockAst>(item, item2));
				}
			}
			return new SwitchStatementAst(base.Extent, base.Label, condition, this.Flags, list, @default);
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x0012F228 File Offset: 0x0012D428
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			foreach (PSTypeName typename in this.Clauses.SelectMany((Tuple<ExpressionAst, StatementBlockAst> clause) => clause.Item2.GetInferredType(context)))
			{
				yield return typename;
			}
			if (this.Default != null)
			{
				foreach (PSTypeName typename2 in this.Default.GetInferredType(context))
				{
					yield return typename2;
				}
			}
			yield break;
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x0012F24C File Offset: 0x0012D44C
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitSwitchStatement(this);
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0012F258 File Offset: 0x0012D458
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitSwitchStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Condition.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.Clauses.Count; i++)
				{
					Tuple<ExpressionAst, StatementBlockAst> tuple = this.Clauses[i];
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
			if (astVisitAction == AstVisitAction.Continue && this.Default != null)
			{
				astVisitAction = this.Default.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001CFD RID: 7421
		private static readonly Tuple<ExpressionAst, StatementBlockAst>[] EmptyClauseArray = new Tuple<ExpressionAst, StatementBlockAst>[0];
	}
}
