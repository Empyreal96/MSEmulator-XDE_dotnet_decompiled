using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.PowerShell;
using Microsoft.PowerShell.DesiredStateConfiguration.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C2 RID: 1474
	internal class SemanticChecks : AstVisitor2, IAstPostVisitHandler
	{
		// Token: 0x06003F1A RID: 16154 RVA: 0x0014CA30 File Offset: 0x0014AC30
		internal static void CheckAst(Parser parser, ScriptBlockAst ast)
		{
			SemanticChecks semanticChecks = new SemanticChecks(parser);
			semanticChecks._scopeStack.Push(ast);
			ast.InternalVisit(semanticChecks);
			semanticChecks._scopeStack.Pop();
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0014CA64 File Offset: 0x0014AC64
		private SemanticChecks(Parser parser)
		{
			this._parser = parser;
			this._memberScopeStack = new Stack<MemberAst>();
			this._scopeStack = new Stack<ScriptBlockAst>();
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0014CA8C File Offset: 0x0014AC8C
		private bool AnalyzingStaticMember()
		{
			MemberAst memberAst;
			if (this._memberScopeStack.Count == 0 || (memberAst = this._memberScopeStack.Peek()) == null)
			{
				return false;
			}
			FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
			if (functionMemberAst == null)
			{
				return ((PropertyMemberAst)memberAst).IsStatic;
			}
			return functionMemberAst.IsStatic;
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0014CAD3 File Offset: 0x0014ACD3
		private bool IsValidAttributeArgument(Ast ast, IsConstantValueVisitor visitor)
		{
			return (bool)ast.Accept(visitor);
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0014CAE4 File Offset: 0x0014ACE4
		private Expression<Func<string>> GetNonConstantAttributeArgErrorExpr(IsConstantValueVisitor visitor)
		{
			if (!visitor.CheckingClassAttributeArguments)
			{
				return () => ParserStrings.ParameterAttributeArgumentNeedsToBeConstantOrScriptBlock;
			}
			return () => ParserStrings.ParameterAttributeArgumentNeedsToBeConstant;
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0014CB58 File Offset: 0x0014AD58
		private void CheckForDuplicateParameters(ReadOnlyCollection<ParameterAst> parameters)
		{
			if (parameters.Count > 0)
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (ParameterAst parameterAst in parameters)
				{
					string userPath = parameterAst.Name.VariablePath.UserPath;
					if (hashSet.Contains(userPath))
					{
						this._parser.ReportError(parameterAst.Name.Extent, () => ParserStrings.DuplicateFormalParameter, userPath);
					}
					else
					{
						hashSet.Add(userPath);
					}
					TypeConstraintAst typeConstraintAst = parameterAst.Attributes.OfType<TypeConstraintAst>().FirstOrDefault((TypeConstraintAst t) => typeof(void) == t.TypeName.GetReflectionType());
					if (typeConstraintAst != null)
					{
						this._parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.VoidTypeConstraintNotAllowed);
					}
				}
			}
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0014CC78 File Offset: 0x0014AE78
		public override AstVisitAction VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			this.CheckForDuplicateParameters(paramBlockAst.Parameters);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0014CC87 File Offset: 0x0014AE87
		public override AstVisitAction VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			SemanticChecks.CheckArrayTypeNameDepth(typeConstraintAst.TypeName, typeConstraintAst.Extent, this._parser);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0014CCA4 File Offset: 0x0014AEA4
		public override AstVisitAction VisitAttribute(AttributeAst attributeAst)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			bool flag = false;
			AttributeTargets attributeTargets = (AttributeTargets)0;
			Ast parent = attributeAst.Parent;
			TypeDefinitionAst typeDefinitionAst = parent as TypeDefinitionAst;
			if (typeDefinitionAst != null)
			{
				flag = true;
				attributeTargets = (typeDefinitionAst.IsClass ? AttributeTargets.Class : (typeDefinitionAst.IsEnum ? AttributeTargets.Enum : AttributeTargets.Interface));
			}
			else if (parent is PropertyMemberAst)
			{
				flag = true;
				attributeTargets = (AttributeTargets.Property | AttributeTargets.Field);
			}
			else
			{
				FunctionMemberAst functionMemberAst = parent as FunctionMemberAst;
				if (functionMemberAst != null)
				{
					flag = true;
					attributeTargets = (functionMemberAst.IsConstructor ? AttributeTargets.Constructor : AttributeTargets.Method);
				}
				else if (parent is ParameterAst && this._memberScopeStack.Peek() is FunctionMemberAst)
				{
					flag = true;
					attributeTargets = AttributeTargets.Parameter;
				}
			}
			IsConstantValueVisitor visitor = flag ? SemanticChecks._isConstantAttributeArgForClassVisitor : SemanticChecks._isConstantAttributeArgVisitor;
			if (flag)
			{
				Type reflectionAttributeType = attributeAst.TypeName.GetReflectionAttributeType();
				if (!(reflectionAttributeType == null))
				{
					AttributeUsageAttribute customAttribute = reflectionAttributeType.GetTypeInfo().GetCustomAttribute(true);
					if (customAttribute != null && (customAttribute.ValidOn & attributeTargets) == (AttributeTargets)0)
					{
						this._parser.ReportError(attributeAst.Extent, () => ParserStrings.AttributeNotAllowedOnDeclaration, ToStringCodeMethods.Type(reflectionAttributeType, false), customAttribute.ValidOn);
					}
					foreach (NamedAttributeArgumentAst namedAttributeArgumentAst in attributeAst.NamedArguments)
					{
						string argumentName = namedAttributeArgumentAst.ArgumentName;
						MemberInfo[] member = reflectionAttributeType.GetMember(argumentName, MemberTypes.Field | MemberTypes.Property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (member.Length != 1 || (!(member[0] is PropertyInfo) && !(member[0] is FieldInfo)))
						{
							this._parser.ReportError(namedAttributeArgumentAst.Extent, () => ParserStrings.PropertyNotFoundForAttribute, new object[]
							{
								argumentName,
								ToStringCodeMethods.Type(reflectionAttributeType, false),
								SemanticChecks.GetValidNamedAttributeProperties(reflectionAttributeType)
							});
						}
						else
						{
							PropertyInfo propertyInfo = member[0] as PropertyInfo;
							if (propertyInfo != null)
							{
								if (propertyInfo.GetSetMethod() == null)
								{
									this._parser.ReportError(namedAttributeArgumentAst.Extent, () => ExtendedTypeSystem.ReadOnlyProperty, argumentName);
								}
							}
							else
							{
								FieldInfo fieldInfo = (FieldInfo)member[0];
								if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
								{
									this._parser.ReportError(namedAttributeArgumentAst.Extent, () => ExtendedTypeSystem.ReadOnlyProperty, argumentName);
								}
							}
						}
					}
				}
			}
			foreach (NamedAttributeArgumentAst namedAttributeArgumentAst2 in attributeAst.NamedArguments)
			{
				string argumentName2 = namedAttributeArgumentAst2.ArgumentName;
				if (hashSet.Contains(argumentName2))
				{
					this._parser.ReportError(namedAttributeArgumentAst2.Extent, () => ParserStrings.DuplicateNamedArgument, argumentName2);
				}
				else
				{
					hashSet.Add(argumentName2);
					if (!namedAttributeArgumentAst2.ExpressionOmitted && !this.IsValidAttributeArgument(namedAttributeArgumentAst2.Argument, visitor))
					{
						this._parser.ReportError(namedAttributeArgumentAst2.Argument.Extent, this.GetNonConstantAttributeArgErrorExpr(visitor));
					}
				}
			}
			foreach (ExpressionAst expressionAst in attributeAst.PositionalArguments)
			{
				if (!this.IsValidAttributeArgument(expressionAst, visitor))
				{
					this._parser.ReportError(expressionAst.Extent, this.GetNonConstantAttributeArgErrorExpr(visitor));
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0014D0B8 File Offset: 0x0014B2B8
		private static string GetValidNamedAttributeProperties(Type attributeType)
		{
			List<string> list = new List<string>();
			foreach (PropertyInfo propertyInfo in attributeType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
			{
				if (propertyInfo.GetSetMethod() != null)
				{
					list.Add(propertyInfo.Name);
				}
			}
			foreach (FieldInfo fieldInfo in attributeType.GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
			{
				if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral)
				{
					list.Add(fieldInfo.Name);
				}
			}
			return string.Join(", ", list);
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0014D150 File Offset: 0x0014B350
		public override AstVisitAction VisitParameter(ParameterAst parameterAst)
		{
			bool flag = parameterAst.Parent.Parent is FunctionMemberAst;
			bool flag2 = false;
			foreach (AttributeBaseAst attributeBaseAst in parameterAst.Attributes)
			{
				if (attributeBaseAst is TypeConstraintAst)
				{
					if (attributeBaseAst.TypeName.FullName.Equals("ordered", StringComparison.OrdinalIgnoreCase))
					{
						this._parser.ReportError(attributeBaseAst.Extent, () => ParserStrings.OrderedAttributeOnlyOnHashLiteralNode, attributeBaseAst.TypeName.FullName);
					}
					else if (flag)
					{
						if (flag2)
						{
							this._parser.ReportError(attributeBaseAst.Extent, () => ParserStrings.MultipleTypeConstraintsOnMethodParam);
						}
						flag2 = true;
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0014D250 File Offset: 0x0014B450
		public override AstVisitAction VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			SemanticChecks.CheckArrayTypeNameDepth(typeExpressionAst.TypeName, typeExpressionAst.Extent, this._parser);
			if (typeof(Type) == typeExpressionAst.TypeName.GetReflectionType())
			{
				this.MarkAstParentsAsSuspicious(typeExpressionAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0014D290 File Offset: 0x0014B490
		internal static void CheckArrayTypeNameDepth(ITypeName typeName, IScriptExtent extent, Parser parser)
		{
			int num = 0;
			ITypeName typeName2 = typeName;
			while (!(typeName2 is TypeName))
			{
				num++;
				if (num > 200)
				{
					parser.ReportError(extent, () => ParserStrings.ScriptTooComplicated);
					return;
				}
				if (!(typeName2 is ArrayTypeName))
				{
					break;
				}
				typeName2 = ((ArrayTypeName)typeName2).ElementType;
			}
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0014D2F4 File Offset: 0x0014B4F4
		public override AstVisitAction VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			AttributeAst attributeAst = null;
			for (int i = 0; i < typeDefinitionAst.Attributes.Count; i++)
			{
				AttributeAst attributeAst2 = typeDefinitionAst.Attributes[i];
				if (attributeAst2.TypeName.GetReflectionAttributeType() == typeof(DscResourceAttribute))
				{
					attributeAst = attributeAst2;
					break;
				}
			}
			if (attributeAst != null)
			{
				DscResourceChecker.CheckType(this._parser, typeDefinitionAst, attributeAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0014D358 File Offset: 0x0014B558
		public override AstVisitAction VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			this._memberScopeStack.Push(functionMemberAst);
			ScriptBlockAst body = functionMemberAst.Body;
			if (body.ParamBlock != null)
			{
				this._parser.ReportError(body.ParamBlock.Extent, () => ParserStrings.ParamBlockNotAllowedInMethod);
			}
			if (body.BeginBlock != null || body.ProcessBlock != null || body.DynamicParamBlock != null || !body.EndBlock.Unnamed)
			{
				this._parser.ReportError(Parser.ExtentFromFirstOf(new object[]
				{
					body.DynamicParamBlock,
					body.BeginBlock,
					body.ProcessBlock,
					body.EndBlock
				}), () => ParserStrings.NamedBlockNotAllowedInMethod);
			}
			if (functionMemberAst.IsConstructor && functionMemberAst.ReturnType != null)
			{
				this._parser.ReportError(functionMemberAst.ReturnType.Extent, () => ParserStrings.ConstructorCantHaveReturnType);
			}
			if (!VariableAnalysis.AnalyzeMemberFunction(functionMemberAst) && !functionMemberAst.IsReturnTypeVoid())
			{
				this._parser.ReportError(functionMemberAst.NameExtent ?? functionMemberAst.Extent, () => ParserStrings.MethodHasCodePathNotReturn);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0014D4CC File Offset: 0x0014B6CC
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			if (functionDefinitionAst.Parameters != null && functionDefinitionAst.Body.ParamBlock != null)
			{
				this._parser.ReportError(functionDefinitionAst.Body.ParamBlock.Extent, () => ParserStrings.OnlyOneParameterListAllowed);
			}
			else if (functionDefinitionAst.Parameters != null)
			{
				this.CheckForDuplicateParameters(functionDefinitionAst.Parameters);
			}
			if (functionDefinitionAst.IsWorkflow)
			{
				try
				{
					IAstToWorkflowConverter astToWorkflowConverterAndEnsureWorkflowModuleLoaded = Utils.GetAstToWorkflowConverterAndEnsureWorkflowModuleLoaded(null);
					List<ParseError> list = astToWorkflowConverterAndEnsureWorkflowModuleLoaded.ValidateAst(functionDefinitionAst);
					foreach (ParseError error in list)
					{
						this._parser.ReportError(error);
					}
				}
				catch (NotSupportedException)
				{
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0014D5B0 File Offset: 0x0014B7B0
		public override AstVisitAction VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			if ((switchStatementAst.Flags & SwitchFlags.Parallel) == SwitchFlags.Parallel)
			{
				bool flag = !switchStatementAst.IsInWorkflow();
				if (flag)
				{
					this._parser.ReportError(switchStatementAst.Extent, () => ParserStrings.ParallelNotSupported);
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0014D7C4 File Offset: 0x0014B9C4
		private static IEnumerable<string> GetConstantDataStatementAllowedCommands(DataStatementAst dataStatementAst)
		{
			yield return "ConvertFrom-StringData";
			foreach (ExpressionAst allowed in dataStatementAst.CommandsAllowed)
			{
				yield return ((StringConstantExpressionAst)allowed).Value;
			}
			yield break;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0014D7E4 File Offset: 0x0014B9E4
		public override AstVisitAction VisitDataStatement(DataStatementAst dataStatementAst)
		{
			IEnumerable<string> allowedCommands = dataStatementAst.HasNonConstantAllowedCommand ? null : SemanticChecks.GetConstantDataStatementAllowedCommands(dataStatementAst);
			RestrictedLanguageChecker visitor = new RestrictedLanguageChecker(this._parser, allowedCommands, null, false);
			dataStatementAst.Body.InternalVisit(visitor);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0014D820 File Offset: 0x0014BA20
		public override AstVisitAction VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			if ((forEachStatementAst.Flags & ForEachFlags.Parallel) == ForEachFlags.Parallel)
			{
				bool flag = !forEachStatementAst.IsInWorkflow();
				if (flag)
				{
					this._parser.ReportError(forEachStatementAst.Extent, () => ParserStrings.ParallelNotSupported);
				}
			}
			if (forEachStatementAst.ThrottleLimit != null && (forEachStatementAst.Flags & ForEachFlags.Parallel) != ForEachFlags.Parallel)
			{
				this._parser.ReportError(forEachStatementAst.Extent, () => ParserStrings.ThrottleLimitRequresParallelFlag);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0014D8BC File Offset: 0x0014BABC
		public override AstVisitAction VisitTryStatement(TryStatementAst tryStatementAst)
		{
			if (tryStatementAst.CatchClauses.Count <= 1)
			{
				return AstVisitAction.Continue;
			}
			for (int i = 0; i < tryStatementAst.CatchClauses.Count - 1; i++)
			{
				CatchClauseAst catchClauseAst = tryStatementAst.CatchClauses[i];
				for (int j = i + 1; j < tryStatementAst.CatchClauses.Count; j++)
				{
					CatchClauseAst catchClauseAst2 = tryStatementAst.CatchClauses[j];
					if (catchClauseAst.IsCatchAll)
					{
						this._parser.ReportError(Parser.Before(catchClauseAst2.Extent), () => ParserStrings.EmptyCatchNotLast);
						break;
					}
					if (!catchClauseAst2.IsCatchAll)
					{
						foreach (TypeConstraintAst typeConstraintAst in catchClauseAst.CatchTypes)
						{
							Type reflectionType = typeConstraintAst.TypeName.GetReflectionType();
							if (!(reflectionType == null))
							{
								foreach (TypeConstraintAst typeConstraintAst2 in catchClauseAst2.CatchTypes)
								{
									Type reflectionType2 = typeConstraintAst2.TypeName.GetReflectionType();
									if (!(reflectionType2 == null) && (reflectionType == reflectionType2 || reflectionType2.IsSubclassOf(reflectionType)))
									{
										this._parser.ReportError(typeConstraintAst2.Extent, () => ParserStrings.ExceptionTypeAlreadyCaught, reflectionType2.FullName);
									}
								}
							}
						}
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0014DA80 File Offset: 0x0014BC80
		private void CheckLabelExists(StatementAst ast, string label)
		{
			if (string.IsNullOrEmpty(label))
			{
				return;
			}
			Ast parent = ast.Parent;
			while (parent != null)
			{
				if (parent is FunctionDefinitionAst)
				{
					if (parent.Parent is FunctionMemberAst)
					{
						this._parser.ReportError(ast.Extent, () => ParserStrings.LabelNotFound, label);
						return;
					}
					break;
				}
				else
				{
					LoopStatementAst loopStatementAst = parent as LoopStatementAst;
					if (loopStatementAst != null && LoopFlowException.MatchLoopLabel(label, loopStatementAst.Label ?? ""))
					{
						return;
					}
					parent = parent.Parent;
				}
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0014DB14 File Offset: 0x0014BD14
		private void CheckForFlowOutOfFinally(Ast ast, string label)
		{
			Ast parent = ast.Parent;
			while (parent != null && !(parent is NamedBlockAst) && !(parent is TrapStatementAst))
			{
				if (parent is ScriptBlockAst)
				{
					return;
				}
				if (label != null && parent is LoopStatementAst && LoopFlowException.MatchLoopLabel(label, ((LoopStatementAst)parent).Label ?? ""))
				{
					return;
				}
				StatementBlockAst statementBlockAst = parent as StatementBlockAst;
				if (statementBlockAst != null)
				{
					TryStatementAst tryStatementAst = statementBlockAst.Parent as TryStatementAst;
					if (tryStatementAst != null && tryStatementAst.Finally == statementBlockAst)
					{
						this._parser.ReportError(ast.Extent, () => ParserStrings.ControlLeavingFinally);
						return;
					}
				}
				parent = parent.Parent;
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x0014DBD8 File Offset: 0x0014BDD8
		private static string GetLabel(ExpressionAst expr)
		{
			if (expr == null)
			{
				return "";
			}
			StringConstantExpressionAst stringConstantExpressionAst = expr as StringConstantExpressionAst;
			if (stringConstantExpressionAst == null)
			{
				return null;
			}
			return stringConstantExpressionAst.Value;
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0014DC00 File Offset: 0x0014BE00
		public override AstVisitAction VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			string label = SemanticChecks.GetLabel(breakStatementAst.Label);
			this.CheckForFlowOutOfFinally(breakStatementAst, label);
			this.CheckLabelExists(breakStatementAst, label);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0014DC2C File Offset: 0x0014BE2C
		public override AstVisitAction VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			string label = SemanticChecks.GetLabel(continueStatementAst.Label);
			this.CheckForFlowOutOfFinally(continueStatementAst, label);
			this.CheckLabelExists(continueStatementAst, label);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0014DC58 File Offset: 0x0014BE58
		private void CheckForReturnStatement(ReturnStatementAst ast)
		{
			FunctionMemberAst functionMemberAst = this._memberScopeStack.Peek() as FunctionMemberAst;
			if (functionMemberAst == null)
			{
				return;
			}
			if (ast.Pipeline != null)
			{
				if (functionMemberAst.IsReturnTypeVoid())
				{
					this._parser.ReportError(ast.Extent, () => ParserStrings.VoidMethodHasReturn);
					return;
				}
			}
			else if (!functionMemberAst.IsReturnTypeVoid())
			{
				this._parser.ReportError(ast.Extent, () => ParserStrings.NonVoidMethodMissingReturnValue);
			}
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0014DCF5 File Offset: 0x0014BEF5
		public override AstVisitAction VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			this.CheckForFlowOutOfFinally(returnStatementAst, null);
			this.CheckForReturnStatement(returnStatementAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0014DD08 File Offset: 0x0014BF08
		private void CheckAssignmentTarget(ExpressionAst ast, bool simpleAssignment, Action<Ast> reportError)
		{
			ArrayLiteralAst arrayLiteralAst = ast as ArrayLiteralAst;
			Ast ast2 = null;
			if (arrayLiteralAst != null)
			{
				if (simpleAssignment)
				{
					this.CheckArrayLiteralAssignment(arrayLiteralAst, reportError);
				}
				else
				{
					ast2 = arrayLiteralAst;
				}
			}
			else
			{
				ParenExpressionAst parenExpressionAst = ast as ParenExpressionAst;
				if (parenExpressionAst != null)
				{
					ExpressionAst pureExpression = parenExpressionAst.Pipeline.GetPureExpression();
					if (pureExpression == null)
					{
						ast2 = parenExpressionAst.Pipeline;
					}
					else
					{
						this.CheckAssignmentTarget(pureExpression, simpleAssignment, reportError);
					}
				}
				else if (!(ast is ISupportsAssignment))
				{
					ast2 = ast;
				}
				else if (ast is AttributedExpressionAst)
				{
					ExpressionAst expressionAst = ast;
					int num = 0;
					IScriptExtent scriptExtent = null;
					while (expressionAst is AttributedExpressionAst)
					{
						ConvertExpressionAst convertExpressionAst = expressionAst as ConvertExpressionAst;
						if (convertExpressionAst != null)
						{
							num++;
							Type reflectionType = convertExpressionAst.Type.TypeName.GetReflectionType();
							if (typeof(PSReference) == reflectionType)
							{
								scriptExtent = convertExpressionAst.Type.Extent;
							}
							else if (typeof(void) == reflectionType)
							{
								this._parser.ReportError(convertExpressionAst.Type.Extent, () => ParserStrings.VoidTypeConstraintNotAllowed);
							}
						}
						expressionAst = ((AttributedExpressionAst)expressionAst).Child;
					}
					if (scriptExtent != null && num > 1)
					{
						this._parser.ReportError(scriptExtent, () => ParserStrings.ReferenceNeedsToBeByItselfInTypeConstraint);
					}
					else
					{
						this.CheckAssignmentTarget(expressionAst, simpleAssignment, reportError);
					}
				}
			}
			if (ast2 != null)
			{
				reportError(ast2);
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0014DE90 File Offset: 0x0014C090
		private void CheckArrayLiteralAssignment(ArrayLiteralAst ast, Action<Ast> reportError)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			foreach (ExpressionAst ast2 in ast.Elements)
			{
				this.CheckAssignmentTarget(ast2, true, reportError);
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0014DF17 File Offset: 0x0014C117
		public override AstVisitAction VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			this.CheckAssignmentTarget(assignmentStatementAst.Left, assignmentStatementAst.Operator == TokenKind.Equals, delegate(Ast ast)
			{
				this._parser.ReportError(ast.Extent, () => ParserStrings.InvalidLeftHandSide);
			});
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0014DF3C File Offset: 0x0014C13C
		public override AstVisitAction VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			if (binaryExpressionAst.Operator == TokenKind.AndAnd || binaryExpressionAst.Operator == TokenKind.OrOr)
			{
				this._parser.ReportError(binaryExpressionAst.ErrorPosition, () => ParserStrings.InvalidEndOfLine, binaryExpressionAst.Operator.Text());
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0014DFF8 File Offset: 0x0014C1F8
		public override AstVisitAction VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			TokenKind tokenKind = unaryExpressionAst.TokenKind;
			switch (tokenKind)
			{
			case TokenKind.MinusMinus:
			case TokenKind.PlusPlus:
				break;
			default:
				switch (tokenKind)
				{
				case TokenKind.PostfixPlusPlus:
				case TokenKind.PostfixMinusMinus:
					break;
				default:
					return AstVisitAction.Continue;
				}
				break;
			}
			this.CheckAssignmentTarget(unaryExpressionAst.Child, false, delegate(Ast ast)
			{
				this._parser.ReportError(ast.Extent, () => ParserStrings.OperatorRequiresVariableOrProperty, unaryExpressionAst.TokenKind.Text());
			});
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0014E084 File Offset: 0x0014C284
		public override AstVisitAction VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			if (convertExpressionAst.Type.TypeName.FullName.Equals("ordered", StringComparison.OrdinalIgnoreCase) && !(convertExpressionAst.Child is HashtableAst))
			{
				this._parser.ReportError(convertExpressionAst.Extent, () => ParserStrings.OrderedAttributeOnlyOnHashLiteralNode, convertExpressionAst.Type.TypeName.FullName);
			}
			if (typeof(PSReference) == convertExpressionAst.Type.TypeName.GetReflectionType())
			{
				ExpressionAst child = convertExpressionAst.Child;
				bool flag = false;
				for (;;)
				{
					AttributedExpressionAst attributedExpressionAst = child as AttributedExpressionAst;
					if (attributedExpressionAst == null)
					{
						break;
					}
					ConvertExpressionAst convertExpressionAst2 = attributedExpressionAst as ConvertExpressionAst;
					if (convertExpressionAst2 != null && typeof(PSReference) == convertExpressionAst2.Type.TypeName.GetReflectionType())
					{
						flag = true;
						this._parser.ReportError(convertExpressionAst2.Type.Extent, () => ParserStrings.ReferenceNeedsToBeByItselfInTypeSequence);
					}
					child = attributedExpressionAst.Child;
				}
				for (AttributedExpressionAst attributedExpressionAst2 = convertExpressionAst.Parent as AttributedExpressionAst; attributedExpressionAst2 != null; attributedExpressionAst2 = (attributedExpressionAst2.Child as AttributedExpressionAst))
				{
					ConvertExpressionAst convertExpressionAst3 = attributedExpressionAst2 as ConvertExpressionAst;
					if (convertExpressionAst3 != null && !flag)
					{
						if (typeof(PSReference) == convertExpressionAst3.Type.TypeName.GetReflectionType())
						{
							break;
						}
						Ast parent = attributedExpressionAst2.Parent;
						bool flag2 = false;
						while (parent != null)
						{
							AssignmentStatementAst assignmentStatementAst = parent as AssignmentStatementAst;
							if (assignmentStatementAst != null)
							{
								flag2 = (assignmentStatementAst.Left.Find((Ast ast1) => ast1 == convertExpressionAst, true) != null);
								break;
							}
							if (parent is CommandExpressionAst)
							{
								break;
							}
							parent = parent.Parent;
						}
						if (!flag2)
						{
							this._parser.ReportError(convertExpressionAst.Type.Extent, () => ParserStrings.ReferenceNeedsToBeLastTypeInTypeConversion);
						}
					}
				}
			}
			if (typeof(Type) == convertExpressionAst.Type.TypeName.GetReflectionType())
			{
				this.MarkAstParentsAsSuspicious(convertExpressionAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0014E310 File Offset: 0x0014C510
		public override AstVisitAction VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			ExpressionAst subExpression = usingExpressionAst.SubExpression;
			ExpressionAst expressionAst = this.CheckUsingExpression(subExpression);
			if (expressionAst != null)
			{
				this._parser.ReportError(expressionAst.Extent, () => ParserStrings.InvalidUsingExpression);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0014E364 File Offset: 0x0014C564
		private ExpressionAst CheckUsingExpression(ExpressionAst exprAst)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			if (exprAst is VariableExpressionAst)
			{
				return null;
			}
			MemberExpressionAst memberExpressionAst = exprAst as MemberExpressionAst;
			if (memberExpressionAst != null && !(memberExpressionAst is InvokeMemberExpressionAst) && memberExpressionAst.Member is StringConstantExpressionAst)
			{
				return this.CheckUsingExpression(memberExpressionAst.Expression);
			}
			IndexExpressionAst indexExpressionAst = exprAst as IndexExpressionAst;
			if (indexExpressionAst == null)
			{
				return exprAst;
			}
			if (!this.IsValidAttributeArgument(indexExpressionAst.Index, SemanticChecks._isConstantAttributeArgVisitor))
			{
				return indexExpressionAst.Index;
			}
			return this.CheckUsingExpression(indexExpressionAst.Target);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0014E3E0 File Offset: 0x0014C5E0
		public override AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			if (variableExpressionAst.Splatted && !(variableExpressionAst.Parent is CommandAst) && !(variableExpressionAst.Parent is UsingExpressionAst))
			{
				if (variableExpressionAst.Parent is ArrayLiteralAst && variableExpressionAst.Parent.Parent is CommandAst)
				{
					this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.SplattingNotPermittedInArgumentList, variableExpressionAst.VariablePath.UserPath);
				}
				else
				{
					this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.SplattingNotPermitted, variableExpressionAst.VariablePath.UserPath);
				}
			}
			if (variableExpressionAst.VariablePath.IsVariable && variableExpressionAst.TupleIndex == -2 && !variableExpressionAst.Assigned && !variableExpressionAst.VariablePath.IsGlobal && !variableExpressionAst.VariablePath.IsScript && !variableExpressionAst.IsConstantVariable() && !SpecialVariables.IsImplicitVariableAccessibleInClassMethod(variableExpressionAst.VariablePath))
			{
				this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.VariableNotLocal);
			}
			if (variableExpressionAst.VariablePath.UserPath.Equals("this", StringComparison.OrdinalIgnoreCase) && this.AnalyzingStaticMember())
			{
				this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.NonStaticMemberAccessInStaticMember, variableExpressionAst.VariablePath.UserPath);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0014E590 File Offset: 0x0014C790
		public override AstVisitAction VisitHashtable(HashtableAst hashtableAst)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				ConstantExpressionAst constantExpressionAst = tuple.Item1 as ConstantExpressionAst;
				if (constantExpressionAst != null)
				{
					string text = constantExpressionAst.Value.ToString();
					if (hashSet.Contains(text))
					{
						Expression<Func<string>> errorExpr = hashtableAst.IsSchemaElement ? (() => ParserStrings.DuplicatePropertyInInstanceDefinition) : (() => ParserStrings.DuplicateKeyInHashLiteral);
						this._parser.ReportError(tuple.Item1.Extent, errorExpr, text);
					}
					else
					{
						hashSet.Add(text);
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0014E684 File Offset: 0x0014C884
		public override AstVisitAction VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			AttributeBaseAst attribute = attributedExpressionAst.Attribute;
			while (attributedExpressionAst != null)
			{
				if (attributedExpressionAst.Child is VariableExpressionAst)
				{
					return AstVisitAction.Continue;
				}
				attributedExpressionAst = (attributedExpressionAst.Child as AttributedExpressionAst);
			}
			this._parser.ReportError(attribute.Extent, () => ParserStrings.UnexpectedAttribute, attribute.TypeName.FullName);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0014E6F8 File Offset: 0x0014C8F8
		public override AstVisitAction VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			if (blockStatementAst.IsInWorkflow())
			{
				return AstVisitAction.Continue;
			}
			this._parser.ReportError(blockStatementAst.Kind.Extent, () => ParserStrings.UnexpectedKeyword, blockStatementAst.Kind.Text);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x0014E751 File Offset: 0x0014C951
		public override AstVisitAction VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			this.CheckMemberAccess(memberExpressionAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x0014E75B File Offset: 0x0014C95B
		public override AstVisitAction VisitInvokeMemberExpression(InvokeMemberExpressionAst memberExpressionAst)
		{
			this.CheckMemberAccess(memberExpressionAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x0014E768 File Offset: 0x0014C968
		private void CheckMemberAccess(MemberExpressionAst ast)
		{
			if (!(ast.Member is ConstantExpressionAst))
			{
				this.MarkAstParentsAsSuspicious(ast);
			}
			TypeExpressionAst typeExpressionAst = ast.Expression as TypeExpressionAst;
			if (ast.Static && typeExpressionAst == null)
			{
				this.MarkAstParentsAsSuspicious(ast);
			}
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0014E7A8 File Offset: 0x0014C9A8
		private void MarkAstParentsAsSuspicious(Ast ast)
		{
			for (Ast ast2 = ast; ast2 != null; ast2 = ast2.Parent)
			{
				Ast ast3 = ast2;
				ast3.HasSuspiciousContent = true;
			}
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0014E7D0 File Offset: 0x0014C9D0
		public override AstVisitAction VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			this._scopeStack.Push(scriptBlockAst);
			if (scriptBlockAst.Parent == null || scriptBlockAst.Parent is ScriptBlockExpressionAst || !(scriptBlockAst.Parent.Parent is FunctionMemberAst))
			{
				this._memberScopeStack.Push(null);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0014E820 File Offset: 0x0014CA20
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			ConfigurationDefinitionAst ancestorAst = Ast.GetAncestorAst<ConfigurationDefinitionAst>(scriptBlockExpressionAst);
			if (ancestorAst != null)
			{
				Ast parent = scriptBlockExpressionAst.Parent;
				PipelineAst pipelineAst = null;
				int num = 0;
				while (parent != null && num <= 2)
				{
					NamedBlockAst namedBlockAst = parent as NamedBlockAst;
					if (namedBlockAst != null && pipelineAst != null && num == 2)
					{
						int num2 = namedBlockAst.Statements.IndexOf(pipelineAst);
						if (num2 <= 0)
						{
							break;
						}
						PipelineAst pipelineAst2 = namedBlockAst.Statements[num2 - 1] as PipelineAst;
						if (pipelineAst2 == null || pipelineAst2.PipelineElements.Count != 1)
						{
							break;
						}
						CommandAst commandAst = pipelineAst2.PipelineElements[0] as CommandAst;
						if (commandAst == null || commandAst.CommandElements.Count > 2 || commandAst.DefiningKeyword != null)
						{
							break;
						}
						StringConstantExpressionAst stringConstantExpressionAst = commandAst.CommandElements[0] as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null)
						{
							this._parser.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.ResourceNotDefined, stringConstantExpressionAst.Extent.Text);
							break;
						}
						break;
					}
					else
					{
						pipelineAst = (parent as PipelineAst);
						num++;
						parent = parent.Parent;
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0014E958 File Offset: 0x0014CB58
		public override AstVisitAction VisitUsingStatement(UsingStatementAst usingStatementAst)
		{
			bool flag = usingStatementAst.UsingStatementKind == UsingStatementKind.Namespace || usingStatementAst.UsingStatementKind == UsingStatementKind.Assembly || usingStatementAst.UsingStatementKind == UsingStatementKind.Module;
			if (!flag || usingStatementAst.Alias != null)
			{
				this._parser.ReportError(usingStatementAst.Extent, () => ParserStrings.UsingStatementNotSupported);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0014E9C0 File Offset: 0x0014CBC0
		public override AstVisitAction VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
		{
			ScriptBlockAst scriptBlock = configurationDefinitionAst.Body.ScriptBlock;
			if (scriptBlock.BeginBlock != null || scriptBlock.ProcessBlock != null || scriptBlock.DynamicParamBlock != null)
			{
				NamedBlockAst[] array = new NamedBlockAst[]
				{
					scriptBlock.BeginBlock,
					scriptBlock.ProcessBlock,
					scriptBlock.DynamicParamBlock
				};
				foreach (NamedBlockAst namedBlockAst in array)
				{
					if (namedBlockAst != null)
					{
						this._parser.ReportError(namedBlockAst.OpenCurlyExtent, () => ParserStrings.UnsupportedNamedBlockInConfiguration);
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0014EA78 File Offset: 0x0014CC78
		public override AstVisitAction VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordStatementAst)
		{
			if (dynamicKeywordStatementAst.Keyword.SemanticCheck != null)
			{
				try
				{
					ParseError[] array = dynamicKeywordStatementAst.Keyword.SemanticCheck(dynamicKeywordStatementAst);
					if (array != null && array.Length > 0)
					{
						array.ToList<ParseError>().ForEach(delegate(ParseError e)
						{
							this._parser.ReportError(e);
						});
					}
				}
				catch (Exception ex)
				{
					this._parser.ReportError(dynamicKeywordStatementAst.Extent, () => ParserStrings.DynamicKeywordSemanticCheckException, dynamicKeywordStatementAst.Keyword.ResourceName, ex.ToString());
				}
			}
			DynamicKeyword keyword = dynamicKeywordStatementAst.Keyword;
			HashtableAst hashtableAst = dynamicKeywordStatementAst.BodyExpression as HashtableAst;
			if (hashtableAst != null)
			{
				foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
				{
					StringConstantExpressionAst stringConstantExpressionAst = tuple.Item1 as StringConstantExpressionAst;
					if (stringConstantExpressionAst == null)
					{
						this._parser.ReportError(tuple.Item1.Extent, () => ParserStrings.ConfigurationInvalidPropertyName, dynamicKeywordStatementAst.FunctionName.Extent, tuple.Item1.Extent);
					}
					else if (!keyword.Properties.ContainsKey(stringConstantExpressionAst.Value))
					{
						this._parser.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.InvalidInstanceProperty, stringConstantExpressionAst.Value, string.Join("', '", keyword.Properties.Keys.OrderBy((string key) => key, StringComparer.OrdinalIgnoreCase)));
					}
				}
			}
			ConfigurationDefinitionAst ancestorAst = Ast.GetAncestorAst<ConfigurationDefinitionAst>(dynamicKeywordStatementAst);
			if (ancestorAst != null)
			{
				StringConstantExpressionAst stringConstantExpressionAst2 = dynamicKeywordStatementAst.CommandElements[0] as StringConstantExpressionAst;
				if (!DscClassCache.SystemResourceNames.Contains(stringConstantExpressionAst2.Extent.Text.Trim()))
				{
					if (ancestorAst.ConfigurationType == ConfigurationType.Meta && !dynamicKeywordStatementAst.Keyword.IsMetaDSCResource())
					{
						this._parser.ReportError(stringConstantExpressionAst2.Extent, () => ParserStrings.RegularResourceUsedInMetaConfig, stringConstantExpressionAst2.Extent.Text);
					}
					else if (ancestorAst.ConfigurationType != ConfigurationType.Meta && dynamicKeywordStatementAst.Keyword.IsMetaDSCResource())
					{
						this._parser.ReportError(stringConstantExpressionAst2.Extent, () => ParserStrings.MetaConfigurationUsedInRegularConfig, stringConstantExpressionAst2.Extent.Text);
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x0014ED78 File Offset: 0x0014CF78
		public override AstVisitAction VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			if (propertyMemberAst.PropertyType != null)
			{
				Type reflectionType = propertyMemberAst.PropertyType.TypeName.GetReflectionType();
				if (reflectionType != null && (reflectionType == typeof(void) || reflectionType.GetTypeInfo().IsGenericTypeDefinition))
				{
					this._parser.ReportError(propertyMemberAst.PropertyType.Extent, () => ParserStrings.TypeNotAllowedForProperty, propertyMemberAst.PropertyType.TypeName.FullName);
				}
			}
			this._memberScopeStack.Push(propertyMemberAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0014EE1C File Offset: 0x0014D01C
		public void PostVisit(Ast ast)
		{
			ScriptBlockAst scriptBlockAst = ast as ScriptBlockAst;
			if (scriptBlockAst != null)
			{
				if (scriptBlockAst.Parent == null || scriptBlockAst.Parent is ScriptBlockExpressionAst || !(scriptBlockAst.Parent.Parent is FunctionMemberAst))
				{
					this._memberScopeStack.Pop();
				}
				this._scopeStack.Pop();
				return;
			}
			if (ast is MemberAst)
			{
				this._memberScopeStack.Pop();
			}
		}

		// Token: 0x04001F4A RID: 8010
		private readonly Parser _parser;

		// Token: 0x04001F4B RID: 8011
		private static readonly IsConstantValueVisitor _isConstantAttributeArgVisitor = new IsConstantValueVisitor
		{
			CheckingAttributeArgument = true
		};

		// Token: 0x04001F4C RID: 8012
		private static readonly IsConstantValueVisitor _isConstantAttributeArgForClassVisitor = new IsConstantValueVisitor
		{
			CheckingAttributeArgument = true,
			CheckingClassAttributeArguments = true
		};

		// Token: 0x04001F4D RID: 8013
		private readonly Stack<MemberAst> _memberScopeStack;

		// Token: 0x04001F4E RID: 8014
		private readonly Stack<ScriptBlockAst> _scopeStack;
	}
}
