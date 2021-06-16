using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000585 RID: 1413
	public class VariableExpressionAst : ExpressionAst, ISupportsAssignment, IAssignableValue
	{
		// Token: 0x06003A9B RID: 15003 RVA: 0x00134A18 File Offset: 0x00132C18
		public VariableExpressionAst(IScriptExtent extent, string variableName, bool splatted) : base(extent)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				throw PSTraceSource.NewArgumentNullException("variableName");
			}
			this.VariablePath = new VariablePath(variableName);
			this.Splatted = splatted;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00134A4E File Offset: 0x00132C4E
		internal VariableExpressionAst(VariableToken token) : this(token.Extent, token.VariablePath, token.Kind == TokenKind.SplattedVariable)
		{
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00134A6B File Offset: 0x00132C6B
		public VariableExpressionAst(IScriptExtent extent, VariablePath variablePath, bool splatted) : base(extent)
		{
			if (variablePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("variablePath");
			}
			this.VariablePath = variablePath;
			this.Splatted = splatted;
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003A9E RID: 15006 RVA: 0x00134A97 File Offset: 0x00132C97
		// (set) Token: 0x06003A9F RID: 15007 RVA: 0x00134A9F File Offset: 0x00132C9F
		public VariablePath VariablePath { get; private set; }

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x00134AA8 File Offset: 0x00132CA8
		// (set) Token: 0x06003AA1 RID: 15009 RVA: 0x00134AB0 File Offset: 0x00132CB0
		public bool Splatted { get; private set; }

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00134ABC File Offset: 0x00132CBC
		public bool IsConstantVariable()
		{
			if (this.VariablePath.IsVariable)
			{
				string unqualifiedPath = this.VariablePath.UnqualifiedPath;
				if (unqualifiedPath.Equals("true", StringComparison.OrdinalIgnoreCase) || unqualifiedPath.Equals("false", StringComparison.OrdinalIgnoreCase) || unqualifiedPath.Equals("null", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00134B0F File Offset: 0x00132D0F
		public override Ast Copy()
		{
			return new VariableExpressionAst(base.Extent, this.VariablePath, this.Splatted);
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00135790 File Offset: 0x00133990
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			if (this.VariablePath.IsVariable)
			{
				Ast parent = base.Parent;
				if (this.VariablePath.IsUnqualified)
				{
					if (!this.VariablePath.UserPath.Equals("_", StringComparison.Ordinal))
					{
						if (!this.VariablePath.UserPath.Equals("PSItem", StringComparison.OrdinalIgnoreCase))
						{
							goto IL_416;
						}
					}
					while (parent != null && !(parent is ScriptBlockExpressionAst))
					{
						parent = parent.Parent;
					}
					if (parent != null)
					{
						if (parent.Parent is CommandExpressionAst && parent.Parent.Parent is PipelineAst)
						{
							if (parent.Parent.Parent.Parent is HashtableAst)
							{
								parent = parent.Parent.Parent.Parent;
							}
							else if (parent.Parent.Parent.Parent is ArrayLiteralAst && parent.Parent.Parent.Parent.Parent is HashtableAst)
							{
								parent = parent.Parent.Parent.Parent.Parent;
							}
						}
						if (parent.Parent is CommandParameterAst)
						{
							parent = parent.Parent;
						}
						CommandAst commandAst = parent.Parent as CommandAst;
						if (commandAst != null)
						{
							PipelineAst pipelineAst = (PipelineAst)commandAst.Parent;
							int previousCommandIndex = pipelineAst.PipelineElements.IndexOf(commandAst) - 1;
							if (previousCommandIndex >= 0)
							{
								foreach (PSTypeName result in pipelineAst.PipelineElements[0].GetInferredType(context))
								{
									if (result.Type != null)
									{
										if (result.Type.IsArray)
										{
											yield return new PSTypeName(result.Type.GetElementType());
											continue;
										}
										if (typeof(IEnumerable).IsAssignableFrom(result.Type))
										{
											IEnumerable<Type> enumerableInterfaces = from t in result.Type.GetInterfaces()
											where t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
											select t;
											foreach (Type i in enumerableInterfaces)
											{
												yield return new PSTypeName(i.GetGenericArguments()[0]);
											}
											continue;
										}
									}
									yield return result;
								}
								goto IL_93B;
							}
							goto IL_93B;
						}
					}
				}
				IL_416:
				if (this.VariablePath.IsUnqualified)
				{
					if (this.VariablePath.UserPath.Equals("this", StringComparison.OrdinalIgnoreCase) && context.CurrentTypeDefinitionAst != null)
					{
						yield return new PSTypeName(context.CurrentTypeDefinitionAst);
						goto IL_93B;
					}
					int j = 0;
					while (j < SpecialVariables.AutomaticVariables.Length)
					{
						if (this.VariablePath.UserPath.Equals(SpecialVariables.AutomaticVariables[j], StringComparison.OrdinalIgnoreCase))
						{
							Type type = SpecialVariables.AutomaticVariableTypes[j];
							if (!(type == typeof(object)))
							{
								yield return new PSTypeName(type);
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				if (parent.Parent is FunctionDefinitionAst)
				{
					parent = parent.Parent;
				}
				int startOffset = base.Extent.StartOffset;
				IEnumerable<Ast> targetAsts = AstSearcher.FindAll(parent, (Ast ast) => (ast is ParameterAst || ast is AssignmentStatementAst || ast is ForEachStatementAst || ast is CommandAst) && this.AstAssignsToSameVariable(ast) && ast.Extent.EndOffset < startOffset, true);
				ParameterAst parameterAst = targetAsts.OfType<ParameterAst>().FirstOrDefault<ParameterAst>();
				if (parameterAst != null)
				{
					PSTypeName[] parameterTypes = parameterAst.GetInferredType(context).ToArray<PSTypeName>();
					if (parameterTypes.Length > 0)
					{
						foreach (PSTypeName parameterType in parameterTypes)
						{
							yield return parameterType;
						}
						goto IL_93B;
					}
				}
				AssignmentStatementAst[] assignAsts = targetAsts.OfType<AssignmentStatementAst>().ToArray<AssignmentStatementAst>();
				foreach (AssignmentStatementAst assignAst in assignAsts)
				{
					ConvertExpressionAst lhsConvert = assignAst.Left as ConvertExpressionAst;
					if (lhsConvert != null)
					{
						yield return new PSTypeName(lhsConvert.Type.TypeName);
						yield break;
					}
				}
				ForEachStatementAst foreachAst = targetAsts.OfType<ForEachStatementAst>().FirstOrDefault<ForEachStatementAst>();
				if (foreachAst != null)
				{
					foreach (PSTypeName typeName in foreachAst.Condition.GetInferredType(context))
					{
						yield return typeName;
					}
				}
				else
				{
					CommandAst commandCompletionAst = targetAsts.OfType<CommandAst>().FirstOrDefault<CommandAst>();
					if (commandCompletionAst != null)
					{
						foreach (PSTypeName typeName2 in commandCompletionAst.GetInferredType(context))
						{
							yield return typeName2;
						}
					}
					else
					{
						int smallestDiff = int.MaxValue;
						AssignmentStatementAst closestAssignment = null;
						foreach (AssignmentStatementAst assignmentStatementAst in assignAsts)
						{
							int endOffset = assignmentStatementAst.Extent.EndOffset;
							if (startOffset - endOffset < smallestDiff)
							{
								smallestDiff = startOffset - endOffset;
								closestAssignment = assignmentStatementAst;
							}
						}
						if (closestAssignment != null)
						{
							foreach (PSTypeName type2 in closestAssignment.Right.GetInferredType(context))
							{
								yield return type2;
							}
						}
					}
				}
			}
			IL_93B:
			yield break;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001357B4 File Offset: 0x001339B4
		internal bool IsSafeVariableReference(HashSet<string> validVariables, ref bool usesParameter)
		{
			bool result = false;
			if (this.VariablePath.IsAnyLocal())
			{
				string unqualifiedPath = this.VariablePath.UnqualifiedPath;
				if ((validVariables != null && validVariables.Contains(unqualifiedPath)) || unqualifiedPath.Equals("args", StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					usesParameter = true;
				}
				else
				{
					result = (!this.Splatted && this.IsConstantVariable());
				}
			}
			return result;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x00135810 File Offset: 0x00133A10
		private bool AstAssignsToSameVariable(Ast ast)
		{
			ParameterAst parameterAst = ast as ParameterAst;
			if (parameterAst != null)
			{
				return this.VariablePath.IsUnscopedVariable && parameterAst.Name.VariablePath.UnqualifiedPath.Equals(this.VariablePath.UnqualifiedPath, StringComparison.OrdinalIgnoreCase);
			}
			ForEachStatementAst forEachStatementAst = ast as ForEachStatementAst;
			if (forEachStatementAst != null)
			{
				return this.VariablePath.IsUnscopedVariable && forEachStatementAst.Variable.VariablePath.UnqualifiedPath.Equals(this.VariablePath.UnqualifiedPath, StringComparison.OrdinalIgnoreCase);
			}
			CommandAst commandAst = ast as CommandAst;
			if (commandAst != null)
			{
				string[] array = new string[]
				{
					"PV",
					"PipelineVariable",
					"OV",
					"OutVariable"
				};
				StaticBindingResult staticBindingResult = StaticParameterBinder.BindCommand(commandAst, false, array);
				if (staticBindingResult != null)
				{
					foreach (string key in array)
					{
						ParameterBindingResult parameterBindingResult;
						if (staticBindingResult.BoundParameters.TryGetValue(key, out parameterBindingResult) && string.Equals(this.VariablePath.UnqualifiedPath, (string)parameterBindingResult.ConstantValue, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
				return false;
			}
			AssignmentStatementAst assignmentStatementAst = (AssignmentStatementAst)ast;
			ExpressionAst expressionAst = assignmentStatementAst.Left;
			ConvertExpressionAst convertExpressionAst = expressionAst as ConvertExpressionAst;
			if (convertExpressionAst != null)
			{
				expressionAst = convertExpressionAst.Child;
			}
			VariableExpressionAst variableExpressionAst = expressionAst as VariableExpressionAst;
			if (variableExpressionAst == null)
			{
				return false;
			}
			VariablePath variablePath = variableExpressionAst.VariablePath;
			return variablePath.UserPath.Equals(this.VariablePath.UserPath, StringComparison.OrdinalIgnoreCase) || (this.VariablePath.IsScript && this.VariablePath.UnqualifiedPath.Equals(variablePath.UnqualifiedPath, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001359BD File Offset: 0x00133BBD
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitVariableExpression(this);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001359C8 File Offset: 0x00133BC8
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitVariableExpression(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x001359EC File Offset: 0x00133BEC
		// (set) Token: 0x06003AAA RID: 15018 RVA: 0x001359F4 File Offset: 0x00133BF4
		internal int TupleIndex
		{
			get
			{
				return this._tupleIndex;
			}
			set
			{
				this._tupleIndex = value;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x001359FD File Offset: 0x00133BFD
		// (set) Token: 0x06003AAC RID: 15020 RVA: 0x00135A05 File Offset: 0x00133C05
		internal bool Automatic { get; set; }

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06003AAD RID: 15021 RVA: 0x00135A0E File Offset: 0x00133C0E
		// (set) Token: 0x06003AAE RID: 15022 RVA: 0x00135A16 File Offset: 0x00133C16
		internal bool Assigned { get; set; }

		// Token: 0x06003AAF RID: 15023 RVA: 0x00135A1F File Offset: 0x00133C1F
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return this;
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00135A22 File Offset: 0x00133C22
		Expression IAssignableValue.GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			return (Expression)compiler.VisitVariableExpression(this);
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00135A30 File Offset: 0x00133C30
		Expression IAssignableValue.SetValue(Compiler compiler, Expression rhs)
		{
			if (this.VariablePath.IsVariable && this.VariablePath.UnqualifiedPath.Equals("null", StringComparison.OrdinalIgnoreCase))
			{
				return rhs;
			}
			IEnumerable<PropertyInfo> enumerable;
			bool flag;
			Type variableType = this.GetVariableType(compiler, out enumerable, out flag);
			Type type = rhs.Type;
			if (flag && (variableType == typeof(object) || variableType == typeof(PSObject)) && (type == typeof(object) || type == typeof(PSObject)))
			{
				rhs = DynamicExpression.Dynamic(PSVariableAssignmentBinder.Get(), typeof(object), rhs);
			}
			rhs = rhs.Convert(variableType);
			if (!flag)
			{
				return Compiler.CallSetVariable(Expression.Constant(this.VariablePath), rhs, null);
			}
			Expression expression = compiler.LocalVariablesParameter;
			foreach (PropertyInfo property in enumerable)
			{
				expression = Expression.Property(expression, property);
			}
			return Expression.Assign(expression, rhs);
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00135B50 File Offset: 0x00133D50
		internal Type GetVariableType(Compiler compiler, out IEnumerable<PropertyInfo> tupleAccessPath, out bool localInTuple)
		{
			localInTuple = (this._tupleIndex >= 0 && (compiler.Optimize || this._tupleIndex < 9));
			tupleAccessPath = null;
			Type result;
			if (localInTuple)
			{
				tupleAccessPath = MutableTuple.GetAccessPath(compiler.LocalVariablesTupleType, this._tupleIndex);
				result = tupleAccessPath.Last<PropertyInfo>().PropertyType;
			}
			else
			{
				result = typeof(object);
			}
			return result;
		}

		// Token: 0x04001D6A RID: 7530
		private int _tupleIndex = -1;
	}
}
