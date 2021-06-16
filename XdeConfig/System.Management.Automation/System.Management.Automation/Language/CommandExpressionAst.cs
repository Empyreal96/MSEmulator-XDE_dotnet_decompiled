using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200056D RID: 1389
	public class CommandExpressionAst : CommandBaseAst
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x00130E0E File Offset: 0x0012F00E
		public CommandExpressionAst(IScriptExtent extent, ExpressionAst expression, IEnumerable<RedirectionAst> redirections) : base(extent, redirections)
		{
			if (expression == null)
			{
				throw PSTraceSource.NewArgumentNullException("expression");
			}
			this.Expression = expression;
			base.SetParent(expression);
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x00130E34 File Offset: 0x0012F034
		// (set) Token: 0x06003996 RID: 14742 RVA: 0x00130E3C File Offset: 0x0012F03C
		public ExpressionAst Expression { get; private set; }

		// Token: 0x06003997 RID: 14743 RVA: 0x00130E48 File Offset: 0x0012F048
		public override Ast Copy()
		{
			ExpressionAst expression = Ast.CopyElement<ExpressionAst>(this.Expression);
			RedirectionAst[] redirections = Ast.CopyElements<RedirectionAst>(base.Redirections);
			return new CommandExpressionAst(base.Extent, expression, redirections);
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x00130E7A File Offset: 0x0012F07A
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Expression.GetInferredType(context);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x00130E88 File Offset: 0x0012F088
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitCommandExpression(this);
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x00130E94 File Offset: 0x0012F094
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitCommandExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Expression.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < base.Redirections.Count; i++)
				{
					RedirectionAst redirectionAst = base.Redirections[i];
					if (astVisitAction == AstVisitAction.Continue)
					{
						astVisitAction = redirectionAst.InternalVisit(visitor);
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
