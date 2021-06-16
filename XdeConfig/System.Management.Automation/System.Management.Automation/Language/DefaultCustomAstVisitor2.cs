using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000599 RID: 1433
	public abstract class DefaultCustomAstVisitor2 : DefaultCustomAstVisitor, ICustomAstVisitor2, ICustomAstVisitor
	{
		// Token: 0x06003C05 RID: 15365 RVA: 0x001379D9 File Offset: 0x00135BD9
		public virtual object VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			return null;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001379DC File Offset: 0x00135BDC
		public virtual object VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
		{
			return null;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x001379DF File Offset: 0x00135BDF
		public virtual object VisitUsingStatement(UsingStatementAst usingStatement)
		{
			return null;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x001379E2 File Offset: 0x00135BE2
		public virtual object VisitConfigurationDefinition(ConfigurationDefinitionAst configurationAst)
		{
			return null;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x001379E5 File Offset: 0x00135BE5
		public virtual object VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst)
		{
			return null;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001379E8 File Offset: 0x00135BE8
		public virtual object VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			return null;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x001379EB File Offset: 0x00135BEB
		public virtual object VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			return null;
		}
	}
}
