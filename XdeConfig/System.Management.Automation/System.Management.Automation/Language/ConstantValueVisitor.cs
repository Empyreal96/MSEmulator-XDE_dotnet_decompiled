using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x0200059E RID: 1438
	internal class ConstantValueVisitor : ICustomAstVisitor
	{
		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x001384E6 File Offset: 0x001366E6
		// (set) Token: 0x06003C5E RID: 15454 RVA: 0x001384EE File Offset: 0x001366EE
		internal bool AttributeArgument { get; set; }

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003C5F RID: 15455 RVA: 0x001384F7 File Offset: 0x001366F7
		// (set) Token: 0x06003C60 RID: 15456 RVA: 0x001384FF File Offset: 0x001366FF
		internal bool RequiresArgument { get; set; }

		// Token: 0x06003C61 RID: 15457 RVA: 0x00138508 File Offset: 0x00136708
		[Conditional("ASSERTIONS_TRACE")]
		[Conditional("DEBUG")]
		private void CheckIsConstant(Ast ast, string msg)
		{
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x00138518 File Offset: 0x00136718
		private static object CompileAndInvoke(Ast ast)
		{
			object result;
			try
			{
				Compiler visitor = new Compiler
				{
					CompilingConstantExpression = true
				};
				result = Expression.Lambda((Expression)ast.Accept(visitor), new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return result;
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x00138578 File Offset: 0x00136778
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0013857F File Offset: 0x0013677F
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x00138586 File Offset: 0x00136786
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0013858D File Offset: 0x0013678D
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x00138594 File Offset: 0x00136794
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0013859B File Offset: 0x0013679B
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x001385A2 File Offset: 0x001367A2
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x001385A9 File Offset: 0x001367A9
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x001385B0 File Offset: 0x001367B0
		public object VisitParameter(ParameterAst parameterAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x001385B7 File Offset: 0x001367B7
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x001385BE File Offset: 0x001367BE
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x001385C5 File Offset: 0x001367C5
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x001385CC File Offset: 0x001367CC
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x001385D3 File Offset: 0x001367D3
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x001385DA File Offset: 0x001367DA
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x001385E1 File Offset: 0x001367E1
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x001385E8 File Offset: 0x001367E8
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x001385EF File Offset: 0x001367EF
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x001385F6 File Offset: 0x001367F6
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x001385FD File Offset: 0x001367FD
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00138604 File Offset: 0x00136804
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x0013860B File Offset: 0x0013680B
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00138612 File Offset: 0x00136812
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x00138619 File Offset: 0x00136819
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00138620 File Offset: 0x00136820
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00138627 File Offset: 0x00136827
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0013862E File Offset: 0x0013682E
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00138635 File Offset: 0x00136835
		public object VisitCommand(CommandAst commandAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x0013863C File Offset: 0x0013683C
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x00138643 File Offset: 0x00136843
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0013864A File Offset: 0x0013684A
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00138651 File Offset: 0x00136851
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x00138658 File Offset: 0x00136858
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x0013865F File Offset: 0x0013685F
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x00138666 File Offset: 0x00136866
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0013866D File Offset: 0x0013686D
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00138674 File Offset: 0x00136874
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			return AutomationNull.Value;
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0013867B File Offset: 0x0013687B
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			return statementBlockAst.Statements.First<StatementAst>().Accept(this);
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x0013868E File Offset: 0x0013688E
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			return pipelineAst.GetPureExpression().Accept(this);
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0013869C File Offset: 0x0013689C
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			return ConstantValueVisitor.CompileAndInvoke(binaryExpressionAst);
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x001386A4 File Offset: 0x001368A4
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			return ConstantValueVisitor.CompileAndInvoke(unaryExpressionAst);
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x001386AC File Offset: 0x001368AC
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			return ConstantValueVisitor.CompileAndInvoke(convertExpressionAst);
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x001386B4 File Offset: 0x001368B4
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return constantExpressionAst.Value;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x001386BC File Offset: 0x001368BC
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return stringConstantExpressionAst.Value;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x001386C4 File Offset: 0x001368C4
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return subExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x001386D2 File Offset: 0x001368D2
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return usingExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x001386E0 File Offset: 0x001368E0
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			string unqualifiedPath = variableExpressionAst.VariablePath.UnqualifiedPath;
			if (unqualifiedPath.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (unqualifiedPath.Equals("false", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			return null;
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x00138724 File Offset: 0x00136924
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return typeExpressionAst.TypeName.GetReflectionType();
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00138734 File Offset: 0x00136934
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			Type reflectionType = ((TypeExpressionAst)memberExpressionAst.Expression).TypeName.GetReflectionType();
			string value = ((StringConstantExpressionAst)memberExpressionAst.Member).Value;
			MemberInfo[] member = reflectionType.GetMember(value, MemberTypes.Field, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			return ((FieldInfo)member[0]).GetValue(null);
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00138781 File Offset: 0x00136981
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return arrayExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x00138798 File Offset: 0x00136998
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return (from e in arrayLiteralAst.Elements
			select e.Accept(this)).ToArray<object>();
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x001387B6 File Offset: 0x001369B6
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return new ScriptBlock(scriptBlockExpressionAst.ScriptBlock, false);
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x001387C4 File Offset: 0x001369C4
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return parenExpressionAst.Pipeline.Accept(this);
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x001387D4 File Offset: 0x001369D4
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			Hashtable hashtable = new Hashtable();
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				hashtable.Add(tuple.Item1.Accept(this), tuple.Item2.Accept(this));
			}
			return hashtable;
		}
	}
}
