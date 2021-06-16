using System;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x0200059D RID: 1437
	internal class IsConstantValueVisitor : ICustomAstVisitor
	{
		// Token: 0x06003C1C RID: 15388 RVA: 0x00137F50 File Offset: 0x00136150
		public static bool IsConstant(Ast ast, out object constantValue, bool forAttribute = false, bool forRequires = false)
		{
			try
			{
				if ((bool)ast.Accept(new IsConstantValueVisitor
				{
					CheckingAttributeArgument = forAttribute,
					CheckingRequiresArgument = forRequires
				}))
				{
					Ast parent = ast.Parent;
					while (parent != null && !(parent is DataStatementAst))
					{
						parent = parent.Parent;
					}
					if (parent == null)
					{
						constantValue = ast.Accept(new ConstantValueVisitor
						{
							AttributeArgument = forAttribute,
							RequiresArgument = forRequires
						});
						return true;
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			constantValue = null;
			return false;
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003C1D RID: 15389 RVA: 0x00137FE4 File Offset: 0x001361E4
		// (set) Token: 0x06003C1E RID: 15390 RVA: 0x00137FEC File Offset: 0x001361EC
		internal bool CheckingAttributeArgument { get; set; }

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003C1F RID: 15391 RVA: 0x00137FF5 File Offset: 0x001361F5
		// (set) Token: 0x06003C20 RID: 15392 RVA: 0x00137FFD File Offset: 0x001361FD
		internal bool CheckingClassAttributeArguments { get; set; }

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003C21 RID: 15393 RVA: 0x00138006 File Offset: 0x00136206
		// (set) Token: 0x06003C22 RID: 15394 RVA: 0x0013800E File Offset: 0x0013620E
		internal bool CheckingRequiresArgument { get; set; }

		// Token: 0x06003C23 RID: 15395 RVA: 0x00138017 File Offset: 0x00136217
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return false;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x0013801F File Offset: 0x0013621F
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x00138027 File Offset: 0x00136227
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return false;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0013802F File Offset: 0x0013622F
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return false;
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x00138037 File Offset: 0x00136237
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return false;
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0013803F File Offset: 0x0013623F
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return false;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x00138047 File Offset: 0x00136247
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return false;
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0013804F File Offset: 0x0013624F
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return false;
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x00138057 File Offset: 0x00136257
		public object VisitParameter(ParameterAst parameterAst)
		{
			return false;
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0013805F File Offset: 0x0013625F
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return false;
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x00138067 File Offset: 0x00136267
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return false;
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0013806F File Offset: 0x0013626F
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return false;
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x00138077 File Offset: 0x00136277
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return false;
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x0013807F File Offset: 0x0013627F
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return false;
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x00138087 File Offset: 0x00136287
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return false;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0013808F File Offset: 0x0013628F
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return false;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x00138097 File Offset: 0x00136297
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			return false;
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0013809F File Offset: 0x0013629F
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return false;
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x001380A7 File Offset: 0x001362A7
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return false;
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001380AF File Offset: 0x001362AF
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return false;
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x001380B7 File Offset: 0x001362B7
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return false;
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x001380BF File Offset: 0x001362BF
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return false;
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x001380C7 File Offset: 0x001362C7
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return false;
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x001380CF File Offset: 0x001362CF
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return false;
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x001380D7 File Offset: 0x001362D7
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return false;
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x001380DF File Offset: 0x001362DF
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return false;
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x001380E7 File Offset: 0x001362E7
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return false;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001380EF File Offset: 0x001362EF
		public object VisitCommand(CommandAst commandAst)
		{
			return false;
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x001380F7 File Offset: 0x001362F7
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x001380FF File Offset: 0x001362FF
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return false;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x00138107 File Offset: 0x00136307
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			return false;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0013810F File Offset: 0x0013630F
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return false;
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x00138117 File Offset: 0x00136317
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0013811F File Offset: 0x0013631F
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x00138127 File Offset: 0x00136327
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x0013812F File Offset: 0x0013632F
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return false;
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x00138137 File Offset: 0x00136337
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x00138140 File Offset: 0x00136340
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

		// Token: 0x06003C49 RID: 15433 RVA: 0x00138194 File Offset: 0x00136394
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			ExpressionAst pureExpression = pipelineAst.GetPureExpression();
			return pureExpression != null && (bool)pureExpression.Accept(this);
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x001381C0 File Offset: 0x001363C0
		private static bool IsNullDivisor(ExpressionAst operand)
		{
			VariableExpressionAst variableExpressionAst = operand as VariableExpressionAst;
			if (variableExpressionAst == null)
			{
				return false;
			}
			BinaryExpressionAst binaryExpressionAst = operand.Parent as BinaryExpressionAst;
			if (binaryExpressionAst == null || binaryExpressionAst.Right != operand)
			{
				return false;
			}
			TokenKind @operator = binaryExpressionAst.Operator;
			switch (@operator)
			{
			case TokenKind.Divide:
			case TokenKind.Rem:
				break;
			default:
				switch (@operator)
				{
				case TokenKind.DivideEquals:
				case TokenKind.RemainderEquals:
					break;
				default:
					return false;
				}
				break;
			}
			string unqualifiedPath = variableExpressionAst.VariablePath.UnqualifiedPath;
			return unqualifiedPath.Equals("false", StringComparison.OrdinalIgnoreCase) || unqualifiedPath.Equals("null", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x00138248 File Offset: 0x00136448
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return binaryExpressionAst.Operator.HasTrait(TokenFlags.CanConstantFold) && (bool)binaryExpressionAst.Left.Accept(this) && (bool)binaryExpressionAst.Right.Accept(this) && !IsConstantValueVisitor.IsNullDivisor(binaryExpressionAst.Right);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x001382A3 File Offset: 0x001364A3
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			return unaryExpressionAst.TokenKind.HasTrait(TokenFlags.CanConstantFold) && (bool)unaryExpressionAst.Child.Accept(this);
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x001382D0 File Offset: 0x001364D0
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
			return (bool)convertExpressionAst.Child.Accept(this);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00138323 File Offset: 0x00136523
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return true;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0013832B File Offset: 0x0013652B
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return true;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x00138333 File Offset: 0x00136533
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return subExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x00138341 File Offset: 0x00136541
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return usingExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x0013834F File Offset: 0x0013654F
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			return variableExpressionAst.IsConstantVariable();
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x0013835C File Offset: 0x0013655C
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return this.CheckingAttributeArgument || typeExpressionAst.TypeName.GetReflectionType() != null;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x00138380 File Offset: 0x00136580
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			if (!memberExpressionAst.Static || !(memberExpressionAst.Expression is TypeExpressionAst))
			{
				return false;
			}
			Type reflectionType = ((TypeExpressionAst)memberExpressionAst.Expression).TypeName.GetReflectionType();
			if (reflectionType == null)
			{
				return false;
			}
			StringConstantExpressionAst stringConstantExpressionAst = memberExpressionAst.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst == null)
			{
				return false;
			}
			MemberInfo[] member = reflectionType.GetMember(stringConstantExpressionAst.Value, MemberTypes.Field, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (member.Length != 1)
			{
				return false;
			}
			return (((FieldInfo)member[0]).Attributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope;
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0013841D File Offset: 0x0013661D
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return false;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x00138433 File Offset: 0x00136633
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return (this.CheckingAttributeArgument || this.CheckingRequiresArgument) && arrayLiteralAst.Elements.All((ExpressionAst e) => (bool)e.Accept(this));
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0013848C File Offset: 0x0013668C
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			return this.CheckingRequiresArgument && hashtableAst.KeyValuePairs.All((Tuple<ExpressionAst, StatementAst> pair) => (bool)pair.Item1.Accept(this) && (bool)pair.Item2.Accept(this));
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x001384B5 File Offset: 0x001366B5
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return this.CheckingAttributeArgument && !this.CheckingClassAttributeArguments;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x001384D0 File Offset: 0x001366D0
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return parenExpressionAst.Pipeline.Accept(this);
		}
	}
}
