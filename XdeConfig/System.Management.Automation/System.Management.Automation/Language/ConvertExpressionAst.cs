using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200057A RID: 1402
	public class ConvertExpressionAst : AttributedExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003A21 RID: 14881 RVA: 0x00132C21 File Offset: 0x00130E21
		public ConvertExpressionAst(IScriptExtent extent, TypeConstraintAst typeConstraint, ExpressionAst child) : base(extent, typeConstraint, child)
		{
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x00132C2C File Offset: 0x00130E2C
		public TypeConstraintAst Type
		{
			get
			{
				return (TypeConstraintAst)base.Attribute;
			}
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00132C3C File Offset: 0x00130E3C
		public override Ast Copy()
		{
			TypeConstraintAst typeConstraint = Ast.CopyElement<TypeConstraintAst>(this.Type);
			ExpressionAst child = Ast.CopyElement<ExpressionAst>(base.Child);
			return new ConvertExpressionAst(base.Extent, typeConstraint, child);
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x00132C6E File Offset: 0x00130E6E
		public override Type StaticType
		{
			get
			{
				return this.Type.TypeName.GetReflectionType() ?? typeof(object);
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00132DC0 File Offset: 0x00130FC0
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			Type type = this.Type.TypeName.GetReflectionType();
			if (type != null)
			{
				yield return new PSTypeName(type);
			}
			else
			{
				yield return new PSTypeName(this.Type.TypeName.FullName);
			}
			yield break;
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00132DDD File Offset: 0x00130FDD
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitConvertExpression(this);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00132DE8 File Offset: 0x00130FE8
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitConvertExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Type.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Child.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x00132E34 File Offset: 0x00131034
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			VariableExpressionAst variableExpressionAst = base.Child as VariableExpressionAst;
			if (variableExpressionAst != null && variableExpressionAst.TupleIndex >= 0)
			{
				return variableExpressionAst;
			}
			return this;
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00132E5C File Offset: 0x0013105C
		internal bool IsRef()
		{
			return this.Type.TypeName.Name.Equals("ref", StringComparison.OrdinalIgnoreCase);
		}
	}
}
