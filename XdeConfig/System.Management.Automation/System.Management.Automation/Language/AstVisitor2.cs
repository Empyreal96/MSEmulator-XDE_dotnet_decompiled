using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000596 RID: 1430
	public abstract class AstVisitor2 : AstVisitor
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x0013759C File Offset: 0x0013579C
		public virtual AstVisitAction VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0013759F File Offset: 0x0013579F
		public virtual AstVisitAction VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x001375A2 File Offset: 0x001357A2
		public virtual AstVisitAction VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x001375A5 File Offset: 0x001357A5
		public virtual AstVisitAction VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x001375A8 File Offset: 0x001357A8
		public virtual AstVisitAction VisitUsingStatement(UsingStatementAst usingStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x001375AB File Offset: 0x001357AB
		public virtual AstVisitAction VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x001375AE File Offset: 0x001357AE
		public virtual AstVisitAction VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordStatementAst)
		{
			return AstVisitAction.Continue;
		}
	}
}
