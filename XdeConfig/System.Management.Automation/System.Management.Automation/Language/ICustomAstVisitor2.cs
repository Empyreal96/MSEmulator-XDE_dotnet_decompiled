using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000595 RID: 1429
	public interface ICustomAstVisitor2 : ICustomAstVisitor
	{
		// Token: 0x06003B7B RID: 15227
		object VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst);

		// Token: 0x06003B7C RID: 15228
		object VisitPropertyMember(PropertyMemberAst propertyMemberAst);

		// Token: 0x06003B7D RID: 15229
		object VisitFunctionMember(FunctionMemberAst functionMemberAst);

		// Token: 0x06003B7E RID: 15230
		object VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst);

		// Token: 0x06003B7F RID: 15231
		object VisitUsingStatement(UsingStatementAst usingStatement);

		// Token: 0x06003B80 RID: 15232
		object VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst);

		// Token: 0x06003B81 RID: 15233
		object VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst);
	}
}
