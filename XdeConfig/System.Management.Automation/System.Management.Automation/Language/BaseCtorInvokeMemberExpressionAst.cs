using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200057D RID: 1405
	public class BaseCtorInvokeMemberExpressionAst : InvokeMemberExpressionAst
	{
		// Token: 0x06003A3E RID: 14910 RVA: 0x00133BB7 File Offset: 0x00131DB7
		public BaseCtorInvokeMemberExpressionAst(IScriptExtent baseKeywordExtent, IScriptExtent baseCallExtent, IEnumerable<ExpressionAst> arguments) : base(baseCallExtent, new VariableExpressionAst(baseKeywordExtent, "this", false), new StringConstantExpressionAst(baseKeywordExtent, ".ctor", StringConstantType.BareWord), arguments, false)
		{
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00133BDC File Offset: 0x00131DDC
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitBaseCtorInvokeMemberExpression(this);
				if (astVisitAction == AstVisitAction.SkipChildren)
				{
					return visitor.CheckForPostAction(this, AstVisitAction.Continue);
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.InternalVisitChildren(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00133C20 File Offset: 0x00131E20
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitBaseCtorInvokeMemberExpression(this);
		}
	}
}
