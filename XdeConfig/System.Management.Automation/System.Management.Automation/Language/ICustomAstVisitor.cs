using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000594 RID: 1428
	public interface ICustomAstVisitor
	{
		// Token: 0x06003B45 RID: 15173
		object VisitErrorStatement(ErrorStatementAst errorStatementAst);

		// Token: 0x06003B46 RID: 15174
		object VisitErrorExpression(ErrorExpressionAst errorExpressionAst);

		// Token: 0x06003B47 RID: 15175
		object VisitScriptBlock(ScriptBlockAst scriptBlockAst);

		// Token: 0x06003B48 RID: 15176
		object VisitParamBlock(ParamBlockAst paramBlockAst);

		// Token: 0x06003B49 RID: 15177
		object VisitNamedBlock(NamedBlockAst namedBlockAst);

		// Token: 0x06003B4A RID: 15178
		object VisitTypeConstraint(TypeConstraintAst typeConstraintAst);

		// Token: 0x06003B4B RID: 15179
		object VisitAttribute(AttributeAst attributeAst);

		// Token: 0x06003B4C RID: 15180
		object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst);

		// Token: 0x06003B4D RID: 15181
		object VisitParameter(ParameterAst parameterAst);

		// Token: 0x06003B4E RID: 15182
		object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst);

		// Token: 0x06003B4F RID: 15183
		object VisitStatementBlock(StatementBlockAst statementBlockAst);

		// Token: 0x06003B50 RID: 15184
		object VisitIfStatement(IfStatementAst ifStmtAst);

		// Token: 0x06003B51 RID: 15185
		object VisitTrap(TrapStatementAst trapStatementAst);

		// Token: 0x06003B52 RID: 15186
		object VisitSwitchStatement(SwitchStatementAst switchStatementAst);

		// Token: 0x06003B53 RID: 15187
		object VisitDataStatement(DataStatementAst dataStatementAst);

		// Token: 0x06003B54 RID: 15188
		object VisitForEachStatement(ForEachStatementAst forEachStatementAst);

		// Token: 0x06003B55 RID: 15189
		object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst);

		// Token: 0x06003B56 RID: 15190
		object VisitForStatement(ForStatementAst forStatementAst);

		// Token: 0x06003B57 RID: 15191
		object VisitWhileStatement(WhileStatementAst whileStatementAst);

		// Token: 0x06003B58 RID: 15192
		object VisitCatchClause(CatchClauseAst catchClauseAst);

		// Token: 0x06003B59 RID: 15193
		object VisitTryStatement(TryStatementAst tryStatementAst);

		// Token: 0x06003B5A RID: 15194
		object VisitBreakStatement(BreakStatementAst breakStatementAst);

		// Token: 0x06003B5B RID: 15195
		object VisitContinueStatement(ContinueStatementAst continueStatementAst);

		// Token: 0x06003B5C RID: 15196
		object VisitReturnStatement(ReturnStatementAst returnStatementAst);

		// Token: 0x06003B5D RID: 15197
		object VisitExitStatement(ExitStatementAst exitStatementAst);

		// Token: 0x06003B5E RID: 15198
		object VisitThrowStatement(ThrowStatementAst throwStatementAst);

		// Token: 0x06003B5F RID: 15199
		object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst);

		// Token: 0x06003B60 RID: 15200
		object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst);

		// Token: 0x06003B61 RID: 15201
		object VisitPipeline(PipelineAst pipelineAst);

		// Token: 0x06003B62 RID: 15202
		object VisitCommand(CommandAst commandAst);

		// Token: 0x06003B63 RID: 15203
		object VisitCommandExpression(CommandExpressionAst commandExpressionAst);

		// Token: 0x06003B64 RID: 15204
		object VisitCommandParameter(CommandParameterAst commandParameterAst);

		// Token: 0x06003B65 RID: 15205
		object VisitFileRedirection(FileRedirectionAst fileRedirectionAst);

		// Token: 0x06003B66 RID: 15206
		object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst);

		// Token: 0x06003B67 RID: 15207
		object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst);

		// Token: 0x06003B68 RID: 15208
		object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst);

		// Token: 0x06003B69 RID: 15209
		object VisitConvertExpression(ConvertExpressionAst convertExpressionAst);

		// Token: 0x06003B6A RID: 15210
		object VisitConstantExpression(ConstantExpressionAst constantExpressionAst);

		// Token: 0x06003B6B RID: 15211
		object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst);

		// Token: 0x06003B6C RID: 15212
		object VisitSubExpression(SubExpressionAst subExpressionAst);

		// Token: 0x06003B6D RID: 15213
		object VisitUsingExpression(UsingExpressionAst usingExpressionAst);

		// Token: 0x06003B6E RID: 15214
		object VisitVariableExpression(VariableExpressionAst variableExpressionAst);

		// Token: 0x06003B6F RID: 15215
		object VisitTypeExpression(TypeExpressionAst typeExpressionAst);

		// Token: 0x06003B70 RID: 15216
		object VisitMemberExpression(MemberExpressionAst memberExpressionAst);

		// Token: 0x06003B71 RID: 15217
		object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst);

		// Token: 0x06003B72 RID: 15218
		object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst);

		// Token: 0x06003B73 RID: 15219
		object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst);

		// Token: 0x06003B74 RID: 15220
		object VisitHashtable(HashtableAst hashtableAst);

		// Token: 0x06003B75 RID: 15221
		object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst);

		// Token: 0x06003B76 RID: 15222
		object VisitParenExpression(ParenExpressionAst parenExpressionAst);

		// Token: 0x06003B77 RID: 15223
		object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst);

		// Token: 0x06003B78 RID: 15224
		object VisitIndexExpression(IndexExpressionAst indexExpressionAst);

		// Token: 0x06003B79 RID: 15225
		object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst);

		// Token: 0x06003B7A RID: 15226
		object VisitBlockStatement(BlockStatementAst blockStatementAst);
	}
}
