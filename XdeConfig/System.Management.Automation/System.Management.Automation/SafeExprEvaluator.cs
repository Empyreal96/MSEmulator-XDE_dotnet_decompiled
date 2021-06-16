using System;
using System.Linq;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000989 RID: 2441
	internal class SafeExprEvaluator : ICustomAstVisitor2, ICustomAstVisitor
	{
		// Token: 0x06005A00 RID: 23040 RVA: 0x001E558C File Offset: 0x001E378C
		internal static bool TrySafeEval(ExpressionAst ast, ExecutionContext executionContext, out object value)
		{
			if (!(bool)ast.Accept(new SafeExprEvaluator()))
			{
				value = null;
				return false;
			}
			bool result;
			try
			{
				value = Compiler.GetExpressionValue(ast, true, executionContext, null);
				result = true;
			}
			catch
			{
				value = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x001E55D8 File Offset: 0x001E37D8
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return false;
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x001E55E0 File Offset: 0x001E37E0
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x001E55E8 File Offset: 0x001E37E8
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return false;
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x001E55F0 File Offset: 0x001E37F0
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return false;
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x001E55F8 File Offset: 0x001E37F8
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return false;
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x001E5600 File Offset: 0x001E3800
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return false;
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x001E5608 File Offset: 0x001E3808
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return false;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x001E5610 File Offset: 0x001E3810
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return false;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x001E5618 File Offset: 0x001E3818
		public object VisitParameter(ParameterAst parameterAst)
		{
			return false;
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x001E5620 File Offset: 0x001E3820
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return false;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x001E5628 File Offset: 0x001E3828
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return false;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x001E5630 File Offset: 0x001E3830
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return false;
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x001E5638 File Offset: 0x001E3838
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return false;
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x001E5640 File Offset: 0x001E3840
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return false;
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x001E5648 File Offset: 0x001E3848
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return false;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x001E5650 File Offset: 0x001E3850
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return false;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x001E5658 File Offset: 0x001E3858
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			return false;
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x001E5660 File Offset: 0x001E3860
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return false;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x001E5668 File Offset: 0x001E3868
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return false;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x001E5670 File Offset: 0x001E3870
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return false;
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x001E5678 File Offset: 0x001E3878
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return false;
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x001E5680 File Offset: 0x001E3880
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return false;
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x001E5688 File Offset: 0x001E3888
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return false;
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x001E5690 File Offset: 0x001E3890
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return false;
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x001E5698 File Offset: 0x001E3898
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return false;
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x001E56A0 File Offset: 0x001E38A0
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return false;
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x001E56A8 File Offset: 0x001E38A8
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return false;
		}

		// Token: 0x06005A1C RID: 23068 RVA: 0x001E56B0 File Offset: 0x001E38B0
		public object VisitCommand(CommandAst commandAst)
		{
			return false;
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x001E56B8 File Offset: 0x001E38B8
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x001E56C0 File Offset: 0x001E38C0
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return false;
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x001E56C8 File Offset: 0x001E38C8
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			return false;
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x001E56D0 File Offset: 0x001E38D0
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return false;
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x001E56D8 File Offset: 0x001E38D8
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x001E56E0 File Offset: 0x001E38E0
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x001E56E8 File Offset: 0x001E38E8
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return false;
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x001E56F0 File Offset: 0x001E38F0
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x001E56F8 File Offset: 0x001E38F8
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x001E5700 File Offset: 0x001E3900
		public object VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			return false;
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x001E5708 File Offset: 0x001E3908
		public object VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			return false;
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x001E5710 File Offset: 0x001E3910
		public object VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			return false;
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x001E5718 File Offset: 0x001E3918
		public object VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
		{
			return false;
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x001E5720 File Offset: 0x001E3920
		public object VisitUsingStatement(UsingStatementAst usingStatementAst)
		{
			return false;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x001E5728 File Offset: 0x001E3928
		public object VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
		{
			return configurationDefinitionAst.Body.Accept(this);
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x001E5736 File Offset: 0x001E3936
		public object VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordStatementAst)
		{
			return false;
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x001E5740 File Offset: 0x001E3940
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			if (statementBlockAst.Traps != null)
			{
				return false;
			}
			if (statementBlockAst.Statements.Count > 1)
			{
				return false;
			}
			StatementAst statementAst = statementBlockAst.Statements.FirstOrDefault<StatementAst>();
			return statementAst != null && (bool)statementAst.Accept(this);
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x001E5794 File Offset: 0x001E3994
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			ExpressionAst pureExpression = pipelineAst.GetPureExpression();
			return pureExpression != null && (bool)pureExpression.Accept(this);
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001E57BF File Offset: 0x001E39BF
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return (bool)binaryExpressionAst.Left.Accept(this) && (bool)binaryExpressionAst.Right.Accept(this);
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x001E57ED File Offset: 0x001E39ED
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			return (bool)unaryExpressionAst.Child.Accept(this);
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x001E5805 File Offset: 0x001E3A05
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			return (bool)convertExpressionAst.Child.Accept(this);
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x001E581D File Offset: 0x001E3A1D
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return true;
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x001E5825 File Offset: 0x001E3A25
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return true;
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x001E582D File Offset: 0x001E3A2D
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return subExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x001E583B File Offset: 0x001E3A3B
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			return true;
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x001E5843 File Offset: 0x001E3A43
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return true;
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x001E584B File Offset: 0x001E3A4B
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			return (bool)memberExpressionAst.Expression.Accept(this) && (bool)memberExpressionAst.Member.Accept(this);
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x001E5879 File Offset: 0x001E3A79
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return (bool)indexExpressionAst.Target.Accept(this) && (bool)indexExpressionAst.Index.Accept(this);
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x001E58A7 File Offset: 0x001E3AA7
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return arrayExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x001E58C3 File Offset: 0x001E3AC3
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return arrayLiteralAst.Elements.All((ExpressionAst e) => (bool)e.Accept(this));
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x001E58E4 File Offset: 0x001E3AE4
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				if (!(bool)tuple.Item1.Accept(this))
				{
					return false;
				}
				if (!(bool)tuple.Item2.Accept(this))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x001E596C File Offset: 0x001E3B6C
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return true;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x001E5974 File Offset: 0x001E3B74
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return parenExpressionAst.Pipeline.Accept(this);
		}
	}
}
