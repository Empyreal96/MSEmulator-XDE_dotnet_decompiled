using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000579 RID: 1401
	public class AttributedExpressionAst : ExpressionAst, ISupportsAssignment, IAssignableValue
	{
		// Token: 0x06003A13 RID: 14867 RVA: 0x00132A56 File Offset: 0x00130C56
		public AttributedExpressionAst(IScriptExtent extent, AttributeBaseAst attribute, ExpressionAst child) : base(extent)
		{
			if (attribute == null || child == null)
			{
				throw PSTraceSource.NewArgumentNullException((attribute == null) ? "attribute" : "child");
			}
			this.Attribute = attribute;
			base.SetParent(attribute);
			this.Child = child;
			base.SetParent(child);
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x00132A96 File Offset: 0x00130C96
		// (set) Token: 0x06003A15 RID: 14869 RVA: 0x00132A9E File Offset: 0x00130C9E
		public ExpressionAst Child { get; private set; }

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x00132AA7 File Offset: 0x00130CA7
		// (set) Token: 0x06003A17 RID: 14871 RVA: 0x00132AAF File Offset: 0x00130CAF
		public AttributeBaseAst Attribute { get; private set; }

		// Token: 0x06003A18 RID: 14872 RVA: 0x00132AB8 File Offset: 0x00130CB8
		public override Ast Copy()
		{
			AttributeBaseAst attribute = Ast.CopyElement<AttributeBaseAst>(this.Attribute);
			ExpressionAst child = Ast.CopyElement<ExpressionAst>(this.Child);
			return new AttributedExpressionAst(base.Extent, attribute, child);
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00132AEA File Offset: 0x00130CEA
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Child.GetInferredType(context);
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00132AF8 File Offset: 0x00130CF8
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitAttributedExpression(this);
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00132B04 File Offset: 0x00130D04
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitAttributedExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Attribute.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Child.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00132B50 File Offset: 0x00130D50
		private ISupportsAssignment GetActualAssignableAst()
		{
			ExpressionAst expressionAst = this;
			for (AttributedExpressionAst attributedExpressionAst = expressionAst as AttributedExpressionAst; attributedExpressionAst != null; attributedExpressionAst = (expressionAst as AttributedExpressionAst))
			{
				expressionAst = attributedExpressionAst.Child;
			}
			return (ISupportsAssignment)expressionAst;
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00132B80 File Offset: 0x00130D80
		private List<AttributeBaseAst> GetAttributes()
		{
			List<AttributeBaseAst> list = new List<AttributeBaseAst>();
			for (AttributedExpressionAst attributedExpressionAst = this; attributedExpressionAst != null; attributedExpressionAst = (attributedExpressionAst.Child as AttributedExpressionAst))
			{
				list.Add(attributedExpressionAst.Attribute);
			}
			list.Reverse();
			return list;
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00132BB9 File Offset: 0x00130DB9
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return this;
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00132BBC File Offset: 0x00130DBC
		Expression IAssignableValue.GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			return (Expression)this.Accept(compiler);
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00132BCC File Offset: 0x00130DCC
		Expression IAssignableValue.SetValue(Compiler compiler, Expression rhs)
		{
			List<AttributeBaseAst> attributes = this.GetAttributes();
			IAssignableValue assignableValue = this.GetActualAssignableAst().GetAssignableValue();
			VariableExpressionAst variableExpressionAst = assignableValue as VariableExpressionAst;
			if (variableExpressionAst == null)
			{
				return assignableValue.SetValue(compiler, Compiler.ConvertValue(rhs, attributes));
			}
			return Compiler.CallSetVariable(Expression.Constant(variableExpressionAst.VariablePath), rhs, Expression.Constant(attributes.ToArray()));
		}
	}
}
