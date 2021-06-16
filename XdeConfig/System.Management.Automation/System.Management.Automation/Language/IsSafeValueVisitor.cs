using System;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C0 RID: 1472
	internal class IsSafeValueVisitor : ICustomAstVisitor
	{
		// Token: 0x06003EA6 RID: 16038 RVA: 0x0014BEF4 File Offset: 0x0014A0F4
		public static bool IsAstSafe(Ast ast, bool isForGetPowerShell)
		{
			IsSafeValueVisitor isSafeValueVisitor = new IsSafeValueVisitor(isForGetPowerShell);
			return (bool)ast.Accept(isSafeValueVisitor) && isSafeValueVisitor._visitCount < 5000;
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0014BF26 File Offset: 0x0014A126
		internal IsSafeValueVisitor(bool isForGetPowerShell)
		{
			this.isForGetPowerShell = isForGetPowerShell;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0014BF35 File Offset: 0x0014A135
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return false;
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x0014BF3D File Offset: 0x0014A13D
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return false;
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x0014BF45 File Offset: 0x0014A145
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return false;
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x0014BF4D File Offset: 0x0014A14D
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return false;
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x0014BF55 File Offset: 0x0014A155
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return false;
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x0014BF5D File Offset: 0x0014A15D
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return false;
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x0014BF65 File Offset: 0x0014A165
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return false;
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x0014BF6D File Offset: 0x0014A16D
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return false;
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0014BF75 File Offset: 0x0014A175
		public object VisitParameter(ParameterAst parameterAst)
		{
			return false;
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x0014BF7D File Offset: 0x0014A17D
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return false;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0014BF85 File Offset: 0x0014A185
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return false;
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0014BF8D File Offset: 0x0014A18D
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0014BF95 File Offset: 0x0014A195
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0014BF9D File Offset: 0x0014A19D
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0014BFA5 File Offset: 0x0014A1A5
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0014BFAD File Offset: 0x0014A1AD
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0014BFB5 File Offset: 0x0014A1B5
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			return false;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0014BFBD File Offset: 0x0014A1BD
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return false;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0014BFC5 File Offset: 0x0014A1C5
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return false;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0014BFCD File Offset: 0x0014A1CD
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return false;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0014BFD5 File Offset: 0x0014A1D5
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return false;
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0014BFDD File Offset: 0x0014A1DD
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return false;
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0014BFE5 File Offset: 0x0014A1E5
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return false;
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0014BFED File Offset: 0x0014A1ED
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return false;
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0014BFF5 File Offset: 0x0014A1F5
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return false;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0014BFFD File Offset: 0x0014A1FD
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return false;
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0014C005 File Offset: 0x0014A205
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return false;
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0014C00D File Offset: 0x0014A20D
		public object VisitCommand(CommandAst commandAst)
		{
			return false;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0014C015 File Offset: 0x0014A215
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return false;
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0014C01D File Offset: 0x0014A21D
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return false;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0014C025 File Offset: 0x0014A225
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			return false;
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0014C02D File Offset: 0x0014A22D
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return false;
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0014C035 File Offset: 0x0014A235
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return false;
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0014C03D File Offset: 0x0014A23D
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return false;
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0014C045 File Offset: 0x0014A245
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			return false;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0014C04D File Offset: 0x0014A24D
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return (bool)indexExpressionAst.Index.Accept(this) && (bool)indexExpressionAst.Target.Accept(this);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0014C07C File Offset: 0x0014A27C
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			bool flag = true;
			foreach (ExpressionAst expressionAst in expandableStringExpressionAst.NestedExpressions)
			{
				this._visitCount++;
				if (!(bool)expressionAst.Accept(this))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0014C0EC File Offset: 0x0014A2EC
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			bool flag = true;
			foreach (StatementAst statementAst in statementBlockAst.Statements)
			{
				this._visitCount++;
				if (statementAst == null)
				{
					flag = false;
					break;
				}
				if (!(bool)statementAst.Accept(this))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0014C164 File Offset: 0x0014A364
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			ExpressionAst pureExpression = pipelineAst.GetPureExpression();
			return pureExpression != null && (bool)pureExpression.Accept(this);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0014C18F File Offset: 0x0014A38F
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return false;
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0014C198 File Offset: 0x0014A398
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			bool flag = unaryExpressionAst.TokenKind.HasTrait(TokenFlags.CanConstantFold) && !unaryExpressionAst.TokenKind.HasTrait(TokenFlags.DisallowedInRestrictedMode) && (bool)unaryExpressionAst.Child.Accept(this);
			if (flag)
			{
				this._visitCount++;
			}
			return flag;
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0014C1F8 File Offset: 0x0014A3F8
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			Type reflectionType = convertExpressionAst.Type.TypeName.GetReflectionType();
			if (reflectionType == null)
			{
				return false;
			}
			if (!reflectionType.IsSafePrimitive())
			{
				return false;
			}
			this._visitCount++;
			return (bool)convertExpressionAst.Child.Accept(this);
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0014C259 File Offset: 0x0014A459
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			this._visitCount++;
			return true;
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0014C26F File Offset: 0x0014A46F
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			this._visitCount++;
			return true;
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0014C285 File Offset: 0x0014A485
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return subExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0014C293 File Offset: 0x0014A493
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			this._visitCount++;
			return usingExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x0014C2B0 File Offset: 0x0014A4B0
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			this._visitCount++;
			if (this.isForGetPowerShell)
			{
				return true;
			}
			bool flag = false;
			return variableExpressionAst.IsSafeVariableReference(null, ref flag);
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0014C2EA File Offset: 0x0014A4EA
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return false;
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0014C2F2 File Offset: 0x0014A4F2
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			return false;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0014C2FA File Offset: 0x0014A4FA
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return arrayExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0014C318 File Offset: 0x0014A518
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			bool flag = arrayLiteralAst.Elements.All((ExpressionAst e) => (bool)e.Accept(this));
			return flag;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0014C36B File Offset: 0x0014A56B
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			if (hashtableAst.KeyValuePairs.Count > 500)
			{
				return false;
			}
			return hashtableAst.KeyValuePairs.All((Tuple<ExpressionAst, StatementAst> pair) => (bool)pair.Item1.Accept(this) && (bool)pair.Item2.Accept(this));
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0014C3A2 File Offset: 0x0014A5A2
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return true;
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0014C3AA File Offset: 0x0014A5AA
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return parenExpressionAst.Pipeline.Accept(this);
		}

		// Token: 0x04001F45 RID: 8005
		private const int MaxVisitCount = 5000;

		// Token: 0x04001F46 RID: 8006
		private const int MaxHashtableKeyCount = 500;

		// Token: 0x04001F47 RID: 8007
		private int _visitCount;

		// Token: 0x04001F48 RID: 8008
		private bool isForGetPowerShell;
	}
}
