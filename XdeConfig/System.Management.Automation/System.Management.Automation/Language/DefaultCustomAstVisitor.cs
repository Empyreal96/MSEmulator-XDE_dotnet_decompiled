using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000598 RID: 1432
	public abstract class DefaultCustomAstVisitor : ICustomAstVisitor
	{
		// Token: 0x06003BCE RID: 15310 RVA: 0x0013792F File Offset: 0x00135B2F
		public virtual object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return null;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00137932 File Offset: 0x00135B32
		public virtual object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x00137935 File Offset: 0x00135B35
		public virtual object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return null;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00137938 File Offset: 0x00135B38
		public virtual object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return null;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0013793B File Offset: 0x00135B3B
		public virtual object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return null;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0013793E File Offset: 0x00135B3E
		public virtual object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return null;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00137941 File Offset: 0x00135B41
		public virtual object VisitAttribute(AttributeAst attributeAst)
		{
			return null;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00137944 File Offset: 0x00135B44
		public virtual object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return null;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00137947 File Offset: 0x00135B47
		public virtual object VisitParameter(ParameterAst parameterAst)
		{
			return null;
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0013794A File Offset: 0x00135B4A
		public virtual object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			return null;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0013794D File Offset: 0x00135B4D
		public virtual object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return null;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x00137950 File Offset: 0x00135B50
		public virtual object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x00137953 File Offset: 0x00135B53
		public virtual object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x00137956 File Offset: 0x00135B56
		public virtual object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x00137959 File Offset: 0x00135B59
		public virtual object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0013795C File Offset: 0x00135B5C
		public virtual object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0013795F File Offset: 0x00135B5F
		public virtual object VisitForStatement(ForStatementAst forStatementAst)
		{
			return null;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00137962 File Offset: 0x00135B62
		public virtual object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00137965 File Offset: 0x00135B65
		public virtual object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return null;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x00137968 File Offset: 0x00135B68
		public virtual object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0013796B File Offset: 0x00135B6B
		public virtual object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0013796E File Offset: 0x00135B6E
		public virtual object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x00137971 File Offset: 0x00135B71
		public virtual object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00137974 File Offset: 0x00135B74
		public virtual object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00137977 File Offset: 0x00135B77
		public virtual object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0013797A File Offset: 0x00135B7A
		public virtual object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0013797D File Offset: 0x00135B7D
		public virtual object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return null;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x00137980 File Offset: 0x00135B80
		public virtual object VisitPipeline(PipelineAst pipelineAst)
		{
			return null;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x00137983 File Offset: 0x00135B83
		public virtual object VisitCommand(CommandAst commandAst)
		{
			return null;
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00137986 File Offset: 0x00135B86
		public virtual object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x00137989 File Offset: 0x00135B89
		public virtual object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return null;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x0013798C File Offset: 0x00135B8C
		public virtual object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			return null;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0013798F File Offset: 0x00135B8F
		public virtual object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return null;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x00137992 File Offset: 0x00135B92
		public virtual object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00137995 File Offset: 0x00135B95
		public virtual object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00137998 File Offset: 0x00135B98
		public virtual object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0013799B File Offset: 0x00135B9B
		public virtual object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0013799E File Offset: 0x00135B9E
		public virtual object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x001379A1 File Offset: 0x00135BA1
		public virtual object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x001379A4 File Offset: 0x00135BA4
		public virtual object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001379A7 File Offset: 0x00135BA7
		public virtual object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x001379AA File Offset: 0x00135BAA
		public virtual object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x001379AD File Offset: 0x00135BAD
		public virtual object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x001379B0 File Offset: 0x00135BB0
		public virtual object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x001379B3 File Offset: 0x00135BB3
		public virtual object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x001379B6 File Offset: 0x00135BB6
		public virtual object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return null;
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x001379B9 File Offset: 0x00135BB9
		public virtual object VisitHashtable(HashtableAst hashtableAst)
		{
			return null;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x001379BC File Offset: 0x00135BBC
		public virtual object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x001379BF File Offset: 0x00135BBF
		public virtual object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return null;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x001379C2 File Offset: 0x00135BC2
		public virtual object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return null;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x001379C5 File Offset: 0x00135BC5
		public virtual object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return null;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x001379C8 File Offset: 0x00135BC8
		public virtual object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return null;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x001379CB File Offset: 0x00135BCB
		public virtual object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return null;
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x001379CE File Offset: 0x00135BCE
		public virtual object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return null;
		}
	}
}
