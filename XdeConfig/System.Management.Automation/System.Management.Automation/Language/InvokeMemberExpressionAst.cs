using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200057C RID: 1404
	public class InvokeMemberExpressionAst : MemberExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003A36 RID: 14902 RVA: 0x00133A67 File Offset: 0x00131C67
		public InvokeMemberExpressionAst(IScriptExtent extent, ExpressionAst expression, CommandElementAst method, IEnumerable<ExpressionAst> arguments, bool @static) : base(extent, expression, method, @static)
		{
			if (arguments != null && arguments.Any<ExpressionAst>())
			{
				this.Arguments = new ReadOnlyCollection<ExpressionAst>(arguments.ToArray<ExpressionAst>());
				base.SetParents<ExpressionAst>(this.Arguments);
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003A37 RID: 14903 RVA: 0x00133A9F File Offset: 0x00131C9F
		// (set) Token: 0x06003A38 RID: 14904 RVA: 0x00133AA7 File Offset: 0x00131CA7
		public ReadOnlyCollection<ExpressionAst> Arguments { get; private set; }

		// Token: 0x06003A39 RID: 14905 RVA: 0x00133AB0 File Offset: 0x00131CB0
		public override Ast Copy()
		{
			ExpressionAst expression = Ast.CopyElement<ExpressionAst>(base.Expression);
			CommandElementAst method = Ast.CopyElement<CommandElementAst>(base.Member);
			ExpressionAst[] arguments = Ast.CopyElements<ExpressionAst>(this.Arguments);
			return new InvokeMemberExpressionAst(base.Extent, expression, method, arguments, base.Static);
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00133AF5 File Offset: 0x00131CF5
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitInvokeMemberExpression(this);
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00133B00 File Offset: 0x00131D00
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitInvokeMemberExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.InternalVisitChildren(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x00133B38 File Offset: 0x00131D38
		internal AstVisitAction InternalVisitChildren(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = base.Expression.InternalVisit(visitor);
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Member.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.Arguments != null)
			{
				for (int i = 0; i < this.Arguments.Count; i++)
				{
					ExpressionAst expressionAst = this.Arguments[i];
					astVisitAction = expressionAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return astVisitAction;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x00133B9C File Offset: 0x00131D9C
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return new InvokeMemberAssignableValue
			{
				InvokeMemberExpressionAst = this
			};
		}
	}
}
