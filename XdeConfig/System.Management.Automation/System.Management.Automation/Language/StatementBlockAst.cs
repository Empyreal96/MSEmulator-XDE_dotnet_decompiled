using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000548 RID: 1352
	public class StatementBlockAst : Ast
	{
		// Token: 0x06003846 RID: 14406 RVA: 0x0012CF18 File Offset: 0x0012B118
		public StatementBlockAst(IScriptExtent extent, IEnumerable<StatementAst> statements, IEnumerable<TrapStatementAst> traps) : base(extent)
		{
			if (statements != null)
			{
				this.Statements = new ReadOnlyCollection<StatementAst>(statements.ToArray<StatementAst>());
				base.SetParents<StatementAst>(this.Statements);
			}
			else
			{
				this.Statements = StatementBlockAst.emptyStatementCollection;
			}
			if (traps != null && traps.Any<TrapStatementAst>())
			{
				this.Traps = new ReadOnlyCollection<TrapStatementAst>(traps.ToArray<TrapStatementAst>());
				base.SetParents<TrapStatementAst>(this.Traps);
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003847 RID: 14407 RVA: 0x0012CF81 File Offset: 0x0012B181
		// (set) Token: 0x06003848 RID: 14408 RVA: 0x0012CF89 File Offset: 0x0012B189
		public ReadOnlyCollection<StatementAst> Statements { get; private set; }

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x0012CF92 File Offset: 0x0012B192
		// (set) Token: 0x0600384A RID: 14410 RVA: 0x0012CF9A File Offset: 0x0012B19A
		public ReadOnlyCollection<TrapStatementAst> Traps { get; private set; }

		// Token: 0x0600384B RID: 14411 RVA: 0x0012CFA4 File Offset: 0x0012B1A4
		public override Ast Copy()
		{
			StatementAst[] array = Ast.CopyElements<StatementAst>(this.Statements);
			TrapStatementAst[] traps = Ast.CopyElements<TrapStatementAst>(this.Traps);
			array = (array ?? new StatementAst[0]);
			return new StatementBlockAst(base.Extent, array, traps);
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0012CFF8 File Offset: 0x0012B1F8
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Statements.SelectMany((StatementAst nestedAst) => nestedAst.GetInferredType(context));
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0012D029 File Offset: 0x0012B229
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitStatementBlock(this);
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x0012D034 File Offset: 0x0012B234
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction action = visitor.VisitStatementBlock(this);
			return visitor.CheckForPostAction(this, StatementBlockAst.InternalVisit(visitor, this.Traps, this.Statements, action));
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x0012D064 File Offset: 0x0012B264
		internal static AstVisitAction InternalVisit(AstVisitor visitor, ReadOnlyCollection<TrapStatementAst> traps, ReadOnlyCollection<StatementAst> statements, AstVisitAction action)
		{
			if (action == AstVisitAction.SkipChildren)
			{
				return AstVisitAction.Continue;
			}
			if (action == AstVisitAction.Continue && traps != null)
			{
				for (int i = 0; i < traps.Count; i++)
				{
					TrapStatementAst trapStatementAst = traps[i];
					action = trapStatementAst.InternalVisit(visitor);
					if (action != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (action == AstVisitAction.Continue && statements != null)
			{
				for (int j = 0; j < statements.Count; j++)
				{
					StatementAst statementAst = statements[j];
					action = statementAst.InternalVisit(visitor);
					if (action != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return action;
		}

		// Token: 0x04001C9F RID: 7327
		private static ReadOnlyCollection<StatementAst> emptyStatementCollection = new ReadOnlyCollection<StatementAst>(new StatementAst[0]);
	}
}
