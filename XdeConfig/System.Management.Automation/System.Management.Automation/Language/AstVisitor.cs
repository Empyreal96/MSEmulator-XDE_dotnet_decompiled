using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020000C1 RID: 193
	public abstract class AstVisitor
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0003F438 File Offset: 0x0003D638
		internal AstVisitAction CheckForPostAction(Ast ast, AstVisitAction action)
		{
			IAstPostVisitHandler astPostVisitHandler = this as IAstPostVisitHandler;
			if (astPostVisitHandler != null)
			{
				astPostVisitHandler.PostVisit(ast);
			}
			return action;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0003F457 File Offset: 0x0003D657
		public virtual AstVisitAction VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0003F45A File Offset: 0x0003D65A
		public virtual AstVisitAction VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0003F45D File Offset: 0x0003D65D
		public virtual AstVisitAction VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0003F460 File Offset: 0x0003D660
		public virtual AstVisitAction VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0003F463 File Offset: 0x0003D663
		public virtual AstVisitAction VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0003F466 File Offset: 0x0003D666
		public virtual AstVisitAction VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0003F469 File Offset: 0x0003D669
		public virtual AstVisitAction VisitAttribute(AttributeAst attributeAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0003F46C File Offset: 0x0003D66C
		public virtual AstVisitAction VisitParameter(ParameterAst parameterAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0003F46F File Offset: 0x0003D66F
		public virtual AstVisitAction VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0003F472 File Offset: 0x0003D672
		public virtual AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0003F475 File Offset: 0x0003D675
		public virtual AstVisitAction VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0003F478 File Offset: 0x0003D678
		public virtual AstVisitAction VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0003F47B File Offset: 0x0003D67B
		public virtual AstVisitAction VisitTrap(TrapStatementAst trapStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0003F47E File Offset: 0x0003D67E
		public virtual AstVisitAction VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0003F481 File Offset: 0x0003D681
		public virtual AstVisitAction VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0003F484 File Offset: 0x0003D684
		public virtual AstVisitAction VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0003F487 File Offset: 0x0003D687
		public virtual AstVisitAction VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0003F48A File Offset: 0x0003D68A
		public virtual AstVisitAction VisitForStatement(ForStatementAst forStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0003F48D File Offset: 0x0003D68D
		public virtual AstVisitAction VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0003F490 File Offset: 0x0003D690
		public virtual AstVisitAction VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0003F493 File Offset: 0x0003D693
		public virtual AstVisitAction VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0003F496 File Offset: 0x0003D696
		public virtual AstVisitAction VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0003F499 File Offset: 0x0003D699
		public virtual AstVisitAction VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0003F49C File Offset: 0x0003D69C
		public virtual AstVisitAction VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0003F49F File Offset: 0x0003D69F
		public virtual AstVisitAction VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0003F4A2 File Offset: 0x0003D6A2
		public virtual AstVisitAction VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0003F4A5 File Offset: 0x0003D6A5
		public virtual AstVisitAction VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0003F4A8 File Offset: 0x0003D6A8
		public virtual AstVisitAction VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0003F4AB File Offset: 0x0003D6AB
		public virtual AstVisitAction VisitPipeline(PipelineAst pipelineAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0003F4AE File Offset: 0x0003D6AE
		public virtual AstVisitAction VisitCommand(CommandAst commandAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0003F4B1 File Offset: 0x0003D6B1
		public virtual AstVisitAction VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0003F4B4 File Offset: 0x0003D6B4
		public virtual AstVisitAction VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0003F4B7 File Offset: 0x0003D6B7
		public virtual AstVisitAction VisitMergingRedirection(MergingRedirectionAst redirectionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0003F4BA File Offset: 0x0003D6BA
		public virtual AstVisitAction VisitFileRedirection(FileRedirectionAst redirectionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0003F4BD File Offset: 0x0003D6BD
		public virtual AstVisitAction VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0003F4C0 File Offset: 0x0003D6C0
		public virtual AstVisitAction VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0003F4C3 File Offset: 0x0003D6C3
		public virtual AstVisitAction VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0003F4C6 File Offset: 0x0003D6C6
		public virtual AstVisitAction VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0003F4C9 File Offset: 0x0003D6C9
		public virtual AstVisitAction VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0003F4CC File Offset: 0x0003D6CC
		public virtual AstVisitAction VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0003F4CF File Offset: 0x0003D6CF
		public virtual AstVisitAction VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0003F4D2 File Offset: 0x0003D6D2
		public virtual AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0003F4D5 File Offset: 0x0003D6D5
		public virtual AstVisitAction VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		public virtual AstVisitAction VisitInvokeMemberExpression(InvokeMemberExpressionAst methodCallAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0003F4DB File Offset: 0x0003D6DB
		public virtual AstVisitAction VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0003F4DE File Offset: 0x0003D6DE
		public virtual AstVisitAction VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0003F4E1 File Offset: 0x0003D6E1
		public virtual AstVisitAction VisitHashtable(HashtableAst hashtableAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0003F4E4 File Offset: 0x0003D6E4
		public virtual AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0003F4E7 File Offset: 0x0003D6E7
		public virtual AstVisitAction VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0003F4EA File Offset: 0x0003D6EA
		public virtual AstVisitAction VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0003F4ED File Offset: 0x0003D6ED
		public virtual AstVisitAction VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0003F4F0 File Offset: 0x0003D6F0
		public virtual AstVisitAction VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0003F4F3 File Offset: 0x0003D6F3
		public virtual AstVisitAction VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0003F4F6 File Offset: 0x0003D6F6
		public virtual AstVisitAction VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return AstVisitAction.Continue;
		}
	}
}
