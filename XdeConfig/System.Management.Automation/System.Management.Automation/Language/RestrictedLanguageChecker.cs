using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C4 RID: 1476
	internal class RestrictedLanguageChecker : AstVisitor
	{
		// Token: 0x06003F58 RID: 16216 RVA: 0x0014F504 File Offset: 0x0014D704
		internal RestrictedLanguageChecker(Parser parser, IEnumerable<string> allowedCommands, IEnumerable<string> allowedVariables, bool allowEnvironmentVariables)
		{
			this._parser = parser;
			this._allowedCommands = allowedCommands;
			if (allowedVariables != null)
			{
				IList<string> list = (allowedVariables as IList<string>) ?? allowedVariables.ToList<string>();
				if (list.Count == 1 && list.Contains("*"))
				{
					this._allVariablesAreAllowed = true;
				}
				else
				{
					this._allowedVariables = new HashSet<string>(RestrictedLanguageChecker._defaultAllowedVariables).Union(list);
				}
			}
			else
			{
				this._allowedVariables = RestrictedLanguageChecker._defaultAllowedVariables;
			}
			this._allowEnvironmentVariables = allowEnvironmentVariables;
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003F59 RID: 16217 RVA: 0x0014F583 File Offset: 0x0014D783
		// (set) Token: 0x06003F5A RID: 16218 RVA: 0x0014F58B File Offset: 0x0014D78B
		private bool FoundError { get; set; }

		// Token: 0x06003F5B RID: 16219 RVA: 0x0014F594 File Offset: 0x0014D794
		internal static void CheckDataStatementLanguageModeAtRuntime(DataStatementAst dataStatementAst, ExecutionContext executionContext)
		{
			if (executionContext.LanguageMode == PSLanguageMode.ConstrainedLanguage)
			{
				Parser parser = new Parser();
				parser.ReportError(dataStatementAst.CommandsAllowed[0].Extent, () => ParserStrings.DataSectionAllowedCommandDisallowed);
				throw new ParseException(parser.ErrorList.ToArray());
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x0014F5F8 File Offset: 0x0014D7F8
		internal static void CheckDataStatementAstAtRuntime(DataStatementAst dataStatementAst, string[] allowedCommands)
		{
			Parser parser = new Parser();
			RestrictedLanguageChecker visitor = new RestrictedLanguageChecker(parser, allowedCommands, null, false);
			dataStatementAst.Body.InternalVisit(visitor);
			if (parser.ErrorList.Any<ParseError>())
			{
				throw new ParseException(parser.ErrorList.ToArray());
			}
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x0014F640 File Offset: 0x0014D840
		internal static void EnsureUtilityModuleLoaded(ExecutionContext context)
		{
			Utils.EnsureModuleLoaded("Microsoft.PowerShell.Utility", context);
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x0014F64D File Offset: 0x0014D84D
		private void ReportError(Ast ast, Expression<Func<string>> errorExpr, params object[] args)
		{
			this.ReportError(ast.Extent, errorExpr, args);
			this.FoundError = true;
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x0014F664 File Offset: 0x0014D864
		private void ReportError(IScriptExtent extent, Expression<Func<string>> errorExpr, params object[] args)
		{
			this._parser.ReportError(extent, errorExpr, args);
			this.FoundError = true;
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0014F67B File Offset: 0x0014D87B
		public override AstVisitAction VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			this.ReportError(scriptBlockAst, () => ParserStrings.ScriptBlockNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0014F6AB File Offset: 0x0014D8AB
		public override AstVisitAction VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			this.ReportError(paramBlockAst, () => ParserStrings.ParameterDeclarationNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0014F6DB File Offset: 0x0014D8DB
		public override AstVisitAction VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0014F6E0 File Offset: 0x0014D8E0
		private void CheckTypeName(Ast ast, ITypeName typename)
		{
			Type reflectionType = typename.GetReflectionType();
			if (reflectionType == null || (reflectionType.IsArray ? reflectionType.GetElementType() : reflectionType).GetTypeCode() == TypeCode.Object)
			{
				this.ReportError(ast, () => ParserStrings.TypeNotAllowedInDataSection, new object[]
				{
					typename.FullName
				});
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0014F74E File Offset: 0x0014D94E
		public override AstVisitAction VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			this.CheckTypeName(typeConstraintAst, typeConstraintAst.TypeName);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x0014F75E File Offset: 0x0014D95E
		public override AstVisitAction VisitAttribute(AttributeAst attributeAst)
		{
			this.ReportError(attributeAst, () => ParserStrings.AttributeNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x0014F78E File Offset: 0x0014D98E
		public override AstVisitAction VisitParameter(ParameterAst parameterAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x0014F791 File Offset: 0x0014D991
		public override AstVisitAction VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			this.CheckTypeName(typeExpressionAst, typeExpressionAst.TypeName);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x0014F7A1 File Offset: 0x0014D9A1
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			this.ReportError(functionDefinitionAst, () => ParserStrings.FunctionDeclarationNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x0014F7D1 File Offset: 0x0014D9D1
		public override AstVisitAction VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x0014F7D4 File Offset: 0x0014D9D4
		public override AstVisitAction VisitIfStatement(IfStatementAst ifStmtAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0014F7D7 File Offset: 0x0014D9D7
		public override AstVisitAction VisitTrap(TrapStatementAst trapStatementAst)
		{
			this.ReportError(trapStatementAst, () => ParserStrings.TrapStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x0014F807 File Offset: 0x0014DA07
		public override AstVisitAction VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			this.ReportError(switchStatementAst, () => ParserStrings.SwitchStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x0014F837 File Offset: 0x0014DA37
		public override AstVisitAction VisitDataStatement(DataStatementAst dataStatementAst)
		{
			this.ReportError(dataStatementAst, () => ParserStrings.DataSectionStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x0014F867 File Offset: 0x0014DA67
		public override AstVisitAction VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			this.ReportError(forEachStatementAst, () => ParserStrings.ForeachStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x0014F897 File Offset: 0x0014DA97
		public override AstVisitAction VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			this.ReportError(doWhileStatementAst, () => ParserStrings.DoWhileStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0014F8C7 File Offset: 0x0014DAC7
		public override AstVisitAction VisitForStatement(ForStatementAst forStatementAst)
		{
			this.ReportError(forStatementAst, () => ParserStrings.ForWhileStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x0014F8F7 File Offset: 0x0014DAF7
		public override AstVisitAction VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			this.ReportError(whileStatementAst, () => ParserStrings.ForWhileStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x0014F927 File Offset: 0x0014DB27
		public override AstVisitAction VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x0014F92A File Offset: 0x0014DB2A
		public override AstVisitAction VisitTryStatement(TryStatementAst tryStatementAst)
		{
			this.ReportError(tryStatementAst, () => ParserStrings.TryStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x0014F95A File Offset: 0x0014DB5A
		public override AstVisitAction VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			this.ReportError(breakStatementAst, () => ParserStrings.FlowControlStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0014F98A File Offset: 0x0014DB8A
		public override AstVisitAction VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			this.ReportError(continueStatementAst, () => ParserStrings.FlowControlStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0014F9BA File Offset: 0x0014DBBA
		public override AstVisitAction VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			this.ReportError(returnStatementAst, () => ParserStrings.FlowControlStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0014F9EA File Offset: 0x0014DBEA
		public override AstVisitAction VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			this.ReportError(exitStatementAst, () => ParserStrings.FlowControlStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0014FA1A File Offset: 0x0014DC1A
		public override AstVisitAction VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			this.ReportError(throwStatementAst, () => ParserStrings.FlowControlStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0014FA4A File Offset: 0x0014DC4A
		public override AstVisitAction VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			this.ReportError(doUntilStatementAst, () => ParserStrings.DoWhileStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0014FA7A File Offset: 0x0014DC7A
		public override AstVisitAction VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			this.ReportError(assignmentStatementAst, () => ParserStrings.AssignmentStatementNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0014FAAA File Offset: 0x0014DCAA
		public override AstVisitAction VisitPipeline(PipelineAst pipelineAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0014FAC4 File Offset: 0x0014DCC4
		public override AstVisitAction VisitCommand(CommandAst commandAst)
		{
			if (commandAst.InvocationOperator == TokenKind.Dot)
			{
				this.ReportError(commandAst, () => ParserStrings.DotSourcingNotSupportedInDataSection, new object[0]);
				return AstVisitAction.Continue;
			}
			if (this._allowedCommands == null)
			{
				return AstVisitAction.Continue;
			}
			string commandName = commandAst.GetCommandName();
			if (commandName == null)
			{
				if (commandAst.InvocationOperator == TokenKind.Ampersand)
				{
					this.ReportError(commandAst, () => ParserStrings.OperatorNotSupportedInDataSection, new object[]
					{
						TokenKind.Ampersand.Text()
					});
				}
				else
				{
					this.ReportError(commandAst, () => ParserStrings.CmdletNotInAllowedListForDataSection, new object[]
					{
						commandAst.Extent.Text
					});
				}
				return AstVisitAction.Continue;
			}
			if (this._allowedCommands.Any((string allowedCommand) => allowedCommand.Equals(commandName, StringComparison.OrdinalIgnoreCase)))
			{
				return AstVisitAction.Continue;
			}
			this.ReportError(commandAst, () => ParserStrings.CmdletNotInAllowedListForDataSection, new object[]
			{
				commandName
			});
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0014FC09 File Offset: 0x0014DE09
		public override AstVisitAction VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0014FC0C File Offset: 0x0014DE0C
		public override AstVisitAction VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0014FC0F File Offset: 0x0014DE0F
		public override AstVisitAction VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			this.ReportError(mergingRedirectionAst, () => ParserStrings.RedirectionNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0014FC3F File Offset: 0x0014DE3F
		public override AstVisitAction VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			this.ReportError(fileRedirectionAst, () => ParserStrings.RedirectionNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0014FC70 File Offset: 0x0014DE70
		public override AstVisitAction VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			if (binaryExpressionAst.Operator.HasTrait(TokenFlags.DisallowedInRestrictedMode))
			{
				this.ReportError(binaryExpressionAst.ErrorPosition, () => ParserStrings.OperatorNotSupportedInDataSection, new object[]
				{
					binaryExpressionAst.Operator.Text()
				});
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0014FCD4 File Offset: 0x0014DED4
		public override AstVisitAction VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			if (unaryExpressionAst.TokenKind.HasTrait(TokenFlags.DisallowedInRestrictedMode))
			{
				this.ReportError(unaryExpressionAst, () => ParserStrings.OperatorNotSupportedInDataSection, new object[]
				{
					unaryExpressionAst.TokenKind.Text()
				});
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0014FD31 File Offset: 0x0014DF31
		public override AstVisitAction VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0014FD34 File Offset: 0x0014DF34
		public override AstVisitAction VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0014FD37 File Offset: 0x0014DF37
		public override AstVisitAction VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0014FD3A File Offset: 0x0014DF3A
		public override AstVisitAction VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0014FD3D File Offset: 0x0014DF3D
		public override AstVisitAction VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0014FD40 File Offset: 0x0014DF40
		public override AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			VariablePath variablePath = variableExpressionAst.VariablePath;
			if (this._allVariablesAreAllowed || this._allowedVariables.Contains(variablePath.UserPath, StringComparer.OrdinalIgnoreCase))
			{
				return AstVisitAction.Continue;
			}
			if (this._allowEnvironmentVariables && variablePath.IsDriveQualified && variablePath.DriveName.Equals("env", StringComparison.OrdinalIgnoreCase))
			{
				return AstVisitAction.Continue;
			}
			this.ReportError(variableExpressionAst, () => ParserStrings.VariableReferenceNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0014FDC9 File Offset: 0x0014DFC9
		public override AstVisitAction VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			this.ReportError(memberExpressionAst, () => ParserStrings.PropertyReferenceNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0014FDF9 File Offset: 0x0014DFF9
		public override AstVisitAction VisitInvokeMemberExpression(InvokeMemberExpressionAst methodCallAst)
		{
			this.ReportError(methodCallAst, () => ParserStrings.MethodCallNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0014FE29 File Offset: 0x0014E029
		public override AstVisitAction VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0014FE2C File Offset: 0x0014E02C
		public override AstVisitAction VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0014FE2F File Offset: 0x0014E02F
		public override AstVisitAction VisitHashtable(HashtableAst hashtableAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0014FE32 File Offset: 0x0014E032
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			this.ReportError(scriptBlockExpressionAst, () => ParserStrings.ScriptBlockNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0014FE62 File Offset: 0x0014E062
		public override AstVisitAction VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0014FE65 File Offset: 0x0014E065
		public override AstVisitAction VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0014FE68 File Offset: 0x0014E068
		public override AstVisitAction VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			this.ReportError(indexExpressionAst, () => ParserStrings.ArrayReferenceNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0014FE98 File Offset: 0x0014E098
		public override AstVisitAction VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			this.ReportError(attributedExpressionAst, () => ParserStrings.AttributeNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0014FEC8 File Offset: 0x0014E0C8
		public override AstVisitAction VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			this.ReportError(blockStatementAst, () => ParserStrings.ParallelAndSequenceBlockNotSupportedInDataSection, new object[0]);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0014FEF8 File Offset: 0x0014E0F8
		public override AstVisitAction VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x04001F51 RID: 8017
		private readonly Parser _parser;

		// Token: 0x04001F52 RID: 8018
		private readonly IEnumerable<string> _allowedCommands;

		// Token: 0x04001F53 RID: 8019
		private readonly IEnumerable<string> _allowedVariables;

		// Token: 0x04001F54 RID: 8020
		private readonly bool _allVariablesAreAllowed;

		// Token: 0x04001F55 RID: 8021
		private readonly bool _allowEnvironmentVariables;

		// Token: 0x04001F56 RID: 8022
		private static readonly HashSet<string> _defaultAllowedVariables = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"PSCulture",
			"PSUICulture",
			"true",
			"false",
			"null"
		};
	}
}
