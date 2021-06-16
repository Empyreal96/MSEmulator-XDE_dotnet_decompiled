using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200053E RID: 1342
	public class ErrorExpressionAst : ExpressionAst
	{
		// Token: 0x060037B3 RID: 14259 RVA: 0x0012AF19 File Offset: 0x00129119
		internal ErrorExpressionAst(IScriptExtent extent, IEnumerable<Ast> nestedAsts = null) : base(extent)
		{
			if (nestedAsts != null && nestedAsts.Any<Ast>())
			{
				this.NestedAst = new ReadOnlyCollection<Ast>(nestedAsts.ToArray<Ast>());
				base.SetParents<Ast>(this.NestedAst);
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x0012AF4A File Offset: 0x0012914A
		// (set) Token: 0x060037B5 RID: 14261 RVA: 0x0012AF52 File Offset: 0x00129152
		public ReadOnlyCollection<Ast> NestedAst { get; private set; }

		// Token: 0x060037B6 RID: 14262 RVA: 0x0012AF5C File Offset: 0x0012915C
		public override Ast Copy()
		{
			Ast[] nestedAsts = Ast.CopyElements<Ast>(this.NestedAst);
			return new ErrorExpressionAst(base.Extent, nestedAsts);
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0012AF98 File Offset: 0x00129198
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.NestedAst.SelectMany((Ast nestedAst) => nestedAst.GetInferredType(context));
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0012AFC9 File Offset: 0x001291C9
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitErrorExpression(this);
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0012AFD4 File Offset: 0x001291D4
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitErrorExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.NestedAst != null)
			{
				for (int i = 0; i < this.NestedAst.Count; i++)
				{
					Ast ast = this.NestedAst[i];
					astVisitAction = ast.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
