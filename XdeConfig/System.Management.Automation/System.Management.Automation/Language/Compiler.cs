using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Interpreter;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x020005A3 RID: 1443
	internal class Compiler : ICustomAstVisitor2, ICustomAstVisitor
	{
		// Token: 0x06003CA5 RID: 15525 RVA: 0x0013A3BC File Offset: 0x001385BC
		static Compiler()
		{
			Compiler._functionContext = Expression.Parameter(typeof(FunctionContext), "funcContext");
			Compiler._executionContextParameter = Expression.Variable(typeof(ExecutionContext), "context");
			Compiler._setDollarQuestionToTrue = Expression.Assign(Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_QuestionMarkVariableValue), ExpressionCache.TrueConstant);
			Compiler._callCheckForInterrupts = Expression.Call(CachedReflectionInfo.PipelineOps_CheckForInterrupts, Compiler._executionContextParameter);
			Compiler._getCurrentPipe = Expression.Field(Compiler._functionContext, CachedReflectionInfo.FunctionContext__outputPipe);
			Compiler._returnPipe = Expression.Variable(Compiler._getCurrentPipe.Type, "returnPipe");
			ParameterExpression parameterExpression = Expression.Variable(typeof(Exception), "exception");
			Compiler._catchFlowControl = Expression.Catch(typeof(FlowControlException), Expression.Rethrow());
			CatchBlock catchBlock = Expression.Catch(parameterExpression, Expression.Block(new Expression[]
			{
				Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_CheckActionPreference, Compiler._functionContext, parameterExpression)
			}));
			Compiler._stmtCatchHandlers = new CatchBlock[]
			{
				Compiler._catchFlowControl,
				catchBlock
			};
			Compiler._currentExceptionBeingHandled = Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_CurrentExceptionBeingHandled);
			for (int i = 0; i < SpecialVariables.AutomaticVariables.Length; i++)
			{
				Compiler.DottedLocalsNameIndexMap.Add(SpecialVariables.AutomaticVariables[i], i);
				Compiler.DottedScriptCmdletLocalsNameIndexMap.Add(SpecialVariables.AutomaticVariables[i], i);
			}
			for (int i = 0; i < SpecialVariables.PreferenceVariables.Length; i++)
			{
				Compiler.DottedScriptCmdletLocalsNameIndexMap.Add(SpecialVariables.PreferenceVariables[i], i + 9);
			}
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0013A5A8 File Offset: 0x001387A8
		private Compiler(List<IScriptExtent> sequencePoints)
		{
			this._sequencePoints = sequencePoints;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0013A5D0 File Offset: 0x001387D0
		internal Compiler()
		{
			this._sequencePoints = new List<IScriptExtent>();
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x0013A5FC File Offset: 0x001387FC
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x0013A604 File Offset: 0x00138804
		internal bool CompilingConstantExpression { get; set; }

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003CAA RID: 15530 RVA: 0x0013A60D File Offset: 0x0013880D
		// (set) Token: 0x06003CAB RID: 15531 RVA: 0x0013A615 File Offset: 0x00138815
		internal bool Optimize { get; private set; }

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06003CAC RID: 15532 RVA: 0x0013A61E File Offset: 0x0013881E
		// (set) Token: 0x06003CAD RID: 15533 RVA: 0x0013A626 File Offset: 0x00138826
		internal Type LocalVariablesTupleType { get; private set; }

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x0013A62F File Offset: 0x0013882F
		// (set) Token: 0x06003CAF RID: 15535 RVA: 0x0013A637 File Offset: 0x00138837
		internal ParameterExpression LocalVariablesParameter { get; private set; }

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x0013A640 File Offset: 0x00138840
		// (set) Token: 0x06003CB1 RID: 15537 RVA: 0x0013A648 File Offset: 0x00138848
		internal bool CompilingMemberFunction { get; set; }

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x0013A651 File Offset: 0x00138851
		// (set) Token: 0x06003CB3 RID: 15539 RVA: 0x0013A659 File Offset: 0x00138859
		internal SpecialMemberFunctionType SpecialMemberFunctionType { get; set; }

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x0013A662 File Offset: 0x00138862
		// (set) Token: 0x06003CB5 RID: 15541 RVA: 0x0013A66A File Offset: 0x0013886A
		internal Type MemberFunctionReturnType
		{
			get
			{
				return this._memberFunctionReturnType;
			}
			set
			{
				this._memberFunctionReturnType = value;
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x0013A673 File Offset: 0x00138873
		internal Expression Compile(Ast ast)
		{
			return (Expression)ast.Accept(this);
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x0013A684 File Offset: 0x00138884
		internal Expression CompileExpressionOperand(ExpressionAst exprAst)
		{
			Expression expression = this.Compile(exprAst);
			if (expression.Type == typeof(void))
			{
				expression = Expression.Block(expression, ExpressionCache.NullConstant);
			}
			return expression;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0013A6BD File Offset: 0x001388BD
		private IEnumerable<Expression> CompileInvocationArguments(IEnumerable<ExpressionAst> arguments)
		{
			if (arguments != null)
			{
				return arguments.Select(new Func<ExpressionAst, Expression>(this.CompileExpressionOperand));
			}
			return new Expression[0];
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x0013A6DC File Offset: 0x001388DC
		internal Expression ReduceAssignment(ISupportsAssignment left, TokenKind tokenKind, Expression right)
		{
			IAssignableValue assignableValue = left.GetAssignableValue();
			ExpressionType operation = ExpressionType.Extension;
			switch (tokenKind)
			{
			case TokenKind.Equals:
				return assignableValue.SetValue(this, right);
			case TokenKind.PlusEquals:
				operation = ExpressionType.Add;
				break;
			case TokenKind.MinusEquals:
				operation = ExpressionType.Subtract;
				break;
			case TokenKind.MultiplyEquals:
				operation = ExpressionType.Multiply;
				break;
			case TokenKind.DivideEquals:
				operation = ExpressionType.Divide;
				break;
			case TokenKind.RemainderEquals:
				operation = ExpressionType.Modulo;
				break;
			}
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			Expression value = assignableValue.GetValue(this, list, list2);
			list.Add(assignableValue.SetValue(this, DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(operation, true, false), typeof(object), value, right)));
			return Expression.Block(list2, list);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x0013A780 File Offset: 0x00138980
		internal Expression GetLocal(int tupleIndex)
		{
			Expression expression = this.LocalVariablesParameter;
			foreach (PropertyInfo property in MutableTuple.GetAccessPath(this.LocalVariablesTupleType, tupleIndex))
			{
				expression = Expression.Property(expression, property);
			}
			return expression;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0013A7DC File Offset: 0x001389DC
		internal static Expression CallGetVariable(Expression variablePath, VariableExpressionAst varAst)
		{
			return Expression.Call(CachedReflectionInfo.VariableOps_GetVariableValue, variablePath, Compiler._executionContextParameter, Expression.Constant(varAst).Cast(typeof(VariableExpressionAst)));
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x0013A803 File Offset: 0x00138A03
		internal static Expression CallSetVariable(Expression variablePath, Expression rhs, Expression attributes = null)
		{
			return Expression.Call(CachedReflectionInfo.VariableOps_SetVariableValue, variablePath, rhs.Cast(typeof(object)), Compiler._executionContextParameter, attributes ?? ExpressionCache.NullConstant.Cast(typeof(AttributeAst[])));
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0013A840 File Offset: 0x00138A40
		internal Expression GetAutomaticVariable(VariableExpressionAst varAst)
		{
			int tupleIndex = varAst.TupleIndex;
			Expression local = this.GetLocal(tupleIndex);
			Expression expression = Expression.Call(CachedReflectionInfo.VariableOps_GetAutomaticVariableValue, ExpressionCache.Constant(tupleIndex), Compiler._executionContextParameter, Expression.Constant(varAst)).Convert(local.Type);
			if (!this.Optimize)
			{
				return expression;
			}
			return Expression.Condition(Expression.Call(this.LocalVariablesParameter, CachedReflectionInfo.MutableTuple_IsValueSet, new Expression[]
			{
				ExpressionCache.Constant(tupleIndex)
			}), local, expression);
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0013A8B5 File Offset: 0x00138AB5
		internal static Expression CallStringEquals(Expression left, Expression right, bool ignoreCase)
		{
			return Expression.Call(CachedReflectionInfo.StringOps_Equals, left, right, ExpressionCache.InvariantCulture, ignoreCase ? ExpressionCache.CompareOptionsIgnoreCase : ExpressionCache.CompareOptionsNone);
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0013A8D7 File Offset: 0x00138AD7
		internal static Expression IsStrictMode(int version, Expression executionContext = null)
		{
			if (executionContext == null)
			{
				executionContext = ExpressionCache.NullExecutionContext;
			}
			return Expression.Call(CachedReflectionInfo.ExecutionContext_IsStrictVersion, executionContext, ExpressionCache.Constant(version));
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x0013A8F4 File Offset: 0x00138AF4
		internal Expression UpdatePosition(Ast ast)
		{
			this._sequencePoints.Add(ast.Extent);
			if (this._sequencePoints.Count != 1 || this._generatingWhileOrDoLoop)
			{
				return new UpdatePositionExpr(ast.Extent, this._sequencePoints.Count - 1, this._debugSymbolDocument, !this._compilingSingleExpression);
			}
			return ExpressionCache.Empty;
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0013A958 File Offset: 0x00138B58
		internal ParameterExpression NewTemp(Type type, string name)
		{
			return Expression.Variable(type, string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
			{
				name,
				this._tempCounter++
			}));
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0013A9A0 File Offset: 0x00138BA0
		internal static Type GetTypeConstraintForMethodResolution(ExpressionAst expr)
		{
			while (expr is ParenExpressionAst)
			{
				expr = ((ParenExpressionAst)expr).Pipeline.GetPureExpression();
			}
			ConvertExpressionAst convertExpressionAst = null;
			while (expr is AttributedExpressionAst)
			{
				if (expr is ConvertExpressionAst && !((ConvertExpressionAst)expr).IsRef())
				{
					convertExpressionAst = (ConvertExpressionAst)expr;
					break;
				}
				expr = ((AttributedExpressionAst)expr).Child;
			}
			if (convertExpressionAst != null)
			{
				return convertExpressionAst.Type.TypeName.GetReflectionType();
			}
			return null;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0013AA18 File Offset: 0x00138C18
		internal static PSMethodInvocationConstraints CombineTypeConstraintForMethodResolution(Type targetType, Type argType)
		{
			if (targetType == null && argType == null)
			{
				return null;
			}
			return new PSMethodInvocationConstraints(targetType, new Type[]
			{
				argType
			});
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0013AA4B File Offset: 0x00138C4B
		internal static PSMethodInvocationConstraints CombineTypeConstraintForMethodResolution(Type targetType, Type[] argTypes)
		{
			if (targetType == null && (argTypes == null || argTypes.Length == 0))
			{
				return null;
			}
			return new PSMethodInvocationConstraints(targetType, argTypes);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0013AA68 File Offset: 0x00138C68
		internal static Expression ConvertValue(ITypeName typeName, Expression expr)
		{
			Type reflectionType = typeName.GetReflectionType();
			if (!(reflectionType != null))
			{
				return DynamicExpression.Dynamic(PSDynamicConvertBinder.Get(), typeof(object), Expression.Call(CachedReflectionInfo.TypeOps_ResolveTypeName, Expression.Constant(typeName)), expr);
			}
			if (reflectionType == typeof(void))
			{
				return Expression.Block(typeof(void), new Expression[]
				{
					expr
				});
			}
			return expr.Convert(reflectionType);
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0013AAE0 File Offset: 0x00138CE0
		internal static Expression ConvertValue(Expression expr, List<AttributeBaseAst> conversions)
		{
			for (int i = 0; i < conversions.Count; i++)
			{
				AttributeBaseAst attributeBaseAst = conversions[i];
				if (attributeBaseAst is TypeConstraintAst)
				{
					expr = Compiler.ConvertValue(attributeBaseAst.TypeName, expr);
				}
			}
			return expr;
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x0013AB28 File Offset: 0x00138D28
		internal static RuntimeDefinedParameterDictionary GetParameterMetaData(ReadOnlyCollection<ParameterAst> parameters, bool automaticPositions, ref bool usesCmdletBinding)
		{
			RuntimeDefinedParameterDictionary runtimeDefinedParameterDictionary = new RuntimeDefinedParameterDictionary();
			List<RuntimeDefinedParameter> list = new List<RuntimeDefinedParameter>();
			bool flag = false;
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterAst parameterAst = parameters[i];
				RuntimeDefinedParameter runtimeDefinedParameter = Compiler.GetRuntimeDefinedParameter(parameterAst, ref flag, ref usesCmdletBinding);
				list.Add(runtimeDefinedParameter);
				runtimeDefinedParameterDictionary.Add(parameterAst.Name.VariablePath.UserPath, runtimeDefinedParameter);
			}
			int num = 0;
			if (automaticPositions && !flag)
			{
				for (int j = 0; j < list.Count; j++)
				{
					RuntimeDefinedParameter runtimeDefinedParameter2 = list[j];
					ParameterAttribute parameterAttribute = (ParameterAttribute)runtimeDefinedParameter2.Attributes.First((Attribute attr) => attr is ParameterAttribute);
					if (!(runtimeDefinedParameter2.ParameterType == typeof(SwitchParameter)))
					{
						parameterAttribute.Position = num++;
					}
				}
			}
			runtimeDefinedParameterDictionary.Data = list.ToArray();
			return runtimeDefinedParameterDictionary;
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x0013AC18 File Offset: 0x00138E18
		private static Delegate GetAttributeGenerator(CallInfo callInfo)
		{
			Delegate @delegate;
			lock (Compiler._attributeGeneratorCache)
			{
				if (!Compiler._attributeGeneratorCache.TryGetValue(callInfo, out @delegate))
				{
					PSAttributeGenerator binder = PSAttributeGenerator.Get(callInfo);
					ParameterExpression[] array = new ParameterExpression[callInfo.ArgumentCount + 1];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = Expression.Variable(typeof(object));
					}
					@delegate = Expression.Lambda(DynamicExpression.Dynamic(binder, typeof(object), array), array).Compile();
					Compiler._attributeGeneratorCache.Add(callInfo, @delegate);
				}
			}
			return @delegate;
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x0013ACCC File Offset: 0x00138ECC
		internal static Attribute GetAttribute(AttributeAst attributeAst)
		{
			int count = attributeAst.PositionalArguments.Count;
			string[] array = (from name in attributeAst.NamedArguments
			select name.ArgumentName).ToArray<string>();
			int num = count + array.Length;
			CallInfo callInfo = new CallInfo(num, array);
			object[] array2 = new object[num + 1];
			Type reflectionAttributeType = attributeAst.TypeName.GetReflectionAttributeType();
			if (reflectionAttributeType == null)
			{
				throw InterpreterError.NewInterpreterException(attributeAst, typeof(RuntimeException), attributeAst.Extent, "CustomAttributeTypeNotFound", ParserStrings.CustomAttributeTypeNotFound, new object[]
				{
					attributeAst.TypeName.FullName
				});
			}
			array2[0] = reflectionAttributeType;
			ConstantValueVisitor visitor = new ConstantValueVisitor
			{
				AttributeArgument = true
			};
			int num2 = 1;
			for (int i = 0; i < attributeAst.PositionalArguments.Count; i++)
			{
				ExpressionAst expressionAst = attributeAst.PositionalArguments[i];
				array2[num2++] = expressionAst.Accept(visitor);
			}
			for (int j = 0; j < attributeAst.NamedArguments.Count; j++)
			{
				NamedAttributeArgumentAst namedAttributeArgumentAst = attributeAst.NamedArguments[j];
				array2[num2++] = namedAttributeArgumentAst.Argument.Accept(visitor);
			}
			Attribute result;
			try
			{
				result = (Attribute)Compiler.GetAttributeGenerator(callInfo).DynamicInvoke(array2);
			}
			catch (TargetInvocationException ex)
			{
				Exception innerException = ex.InnerException;
				RuntimeException ex2 = innerException as RuntimeException;
				if (ex2 == null)
				{
					ex2 = InterpreterError.NewInterpreterExceptionWithInnerException(null, typeof(RuntimeException), attributeAst.Extent, "ExceptionConstructingAttribute", ExtendedTypeSystem.ExceptionConstructingAttribute, innerException, new object[]
					{
						innerException.Message,
						attributeAst.TypeName.FullName
					});
				}
				InterpreterError.UpdateExceptionErrorRecordPosition(ex2, attributeAst.Extent);
				throw ex2;
			}
			return result;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x0013AEB0 File Offset: 0x001390B0
		internal static Attribute GetAttribute(TypeConstraintAst typeConstraintAst)
		{
			Type type = null;
			ISupportsTypeCaching supportsTypeCaching = typeConstraintAst.TypeName as ISupportsTypeCaching;
			if (supportsTypeCaching != null)
			{
				type = supportsTypeCaching.CachedType;
			}
			if (type == null)
			{
				type = TypeOps.ResolveTypeName(typeConstraintAst.TypeName);
				if (supportsTypeCaching != null)
				{
					supportsTypeCaching.CachedType = type;
				}
			}
			return new ArgumentTypeConverterAttribute(new Type[]
			{
				type
			});
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x0013AF08 File Offset: 0x00139108
		private static RuntimeDefinedParameter GetRuntimeDefinedParameter(ParameterAst parameterAst, ref bool customParameterSet, ref bool usesCmdletBinding)
		{
			List<Attribute> list = new List<Attribute>();
			bool flag = false;
			for (int i = 0; i < parameterAst.Attributes.Count; i++)
			{
				AttributeBaseAst attributeBaseAst = parameterAst.Attributes[i];
				Attribute attribute = attributeBaseAst.GetAttribute();
				list.Add(attribute);
				ParameterAttribute parameterAttribute = attribute as ParameterAttribute;
				if (parameterAttribute != null)
				{
					flag = true;
					usesCmdletBinding = true;
					if (parameterAttribute.Position != -2147483648 || !parameterAttribute.ParameterSetName.Equals("__AllParameterSets", StringComparison.OrdinalIgnoreCase))
					{
						customParameterSet = true;
					}
				}
			}
			list.Reverse();
			if (!flag)
			{
				list.Insert(0, new ParameterAttribute());
			}
			RuntimeDefinedParameter runtimeDefinedParameter = new RuntimeDefinedParameter(parameterAst.Name.VariablePath.UserPath, parameterAst.StaticType, new Collection<Attribute>(list.ToArray()));
			object obj;
			if (parameterAst.DefaultValue != null)
			{
				object value;
				if (IsConstantValueVisitor.IsConstant(parameterAst.DefaultValue, out value, false, false))
				{
					runtimeDefinedParameter.Value = value;
				}
				else
				{
					runtimeDefinedParameter.Value = new Compiler.DefaultValueExpressionWrapper
					{
						Expression = parameterAst.DefaultValue
					};
				}
			}
			else if (Compiler.TryGetDefaultParameterValue(parameterAst.StaticType, out obj) && obj != null)
			{
				runtimeDefinedParameter.Value = obj;
			}
			return runtimeDefinedParameter;
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0013B028 File Offset: 0x00139228
		internal static bool TryGetDefaultParameterValue(Type type, out object value)
		{
			if (type == typeof(string))
			{
				value = string.Empty;
				return true;
			}
			if (type.GetTypeInfo().IsClass)
			{
				value = null;
				return true;
			}
			if (type == typeof(bool))
			{
				value = Boxed.False;
				return true;
			}
			if (type == typeof(SwitchParameter))
			{
				value = new SwitchParameter(false);
				return true;
			}
			if (LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(type)) && !type.GetTypeInfo().IsEnum)
			{
				value = 0;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x0013B0C8 File Offset: 0x001392C8
		internal void Compile(CompiledScriptBlockData scriptBlock, bool optimize)
		{
			IParameterMetadataProvider ast = scriptBlock.Ast;
			Ast ast2 = (Ast)ast;
			this.Optimize = optimize;
			this._compilingScriptCmdlet = scriptBlock.UsesCmdletBinding;
			string file = ast2.Extent.File;
			if (file != null)
			{
				this._debugSymbolDocument = Expression.SymbolDocument(file);
			}
			Tuple<Type, Dictionary<string, int>> tuple = VariableAnalysis.Analyze(ast, !optimize, this._compilingScriptCmdlet);
			this.LocalVariablesTupleType = tuple.Item1;
			Dictionary<string, int> item = tuple.Item2;
			if (!item.TryGetValue("switch", out this._switchTupleIndex))
			{
				this._switchTupleIndex = -2;
			}
			if (!item.TryGetValue("foreach", out this._foreachTupleIndex))
			{
				this._foreachTupleIndex = -2;
			}
			this.LocalVariablesParameter = Expression.Variable(this.LocalVariablesTupleType, "locals");
			FunctionMemberAst functionMemberAst = ast2 as FunctionMemberAst;
			if (functionMemberAst != null)
			{
				this.CompilingMemberFunction = true;
				this.MemberFunctionReturnType = functionMemberAst.GetReturnType();
				this._memberFunctionType = (TypeDefinitionAst)functionMemberAst.Parent;
				this.SpecialMemberFunctionType = SpecialMemberFunctionType.None;
				if (functionMemberAst.Name.Equals(this._memberFunctionType.Name, StringComparison.OrdinalIgnoreCase))
				{
					ReadOnlyCollection<ParameterAst> parameters = ((IParameterMetadataProvider)functionMemberAst.Body).Parameters;
					if (parameters == null || parameters.Count == 0)
					{
						this.SpecialMemberFunctionType = (functionMemberAst.IsStatic ? SpecialMemberFunctionType.StaticConstructor : SpecialMemberFunctionType.DefaultConstructor);
					}
				}
			}
			else
			{
				CompilerGeneratedMemberFunctionAst compilerGeneratedMemberFunctionAst = ast2 as CompilerGeneratedMemberFunctionAst;
				if (compilerGeneratedMemberFunctionAst != null)
				{
					this.CompilingMemberFunction = true;
					this.SpecialMemberFunctionType = compilerGeneratedMemberFunctionAst.Type;
					this.MemberFunctionReturnType = typeof(void);
					this._memberFunctionType = compilerGeneratedMemberFunctionAst.DefiningType;
				}
			}
			ast.Body.Accept(this);
			if (this._sequencePoints.Count == 0)
			{
				this._sequencePoints.Add(ast2.Extent);
			}
			if (optimize)
			{
				scriptBlock.DynamicParamBlockTree = this._dynamicParamBlockLambda;
				scriptBlock.BeginBlockTree = this._beginBlockLambda;
				scriptBlock.ProcessBlockTree = this._processBlockLambda;
				scriptBlock.EndBlockTree = this._endBlockLambda;
				scriptBlock.LocalsMutableTupleType = this.LocalVariablesTupleType;
				scriptBlock.NameToIndexMap = item;
			}
			else
			{
				scriptBlock.UnoptimizedDynamicParamBlockTree = this._dynamicParamBlockLambda;
				scriptBlock.UnoptimizedBeginBlockTree = this._beginBlockLambda;
				scriptBlock.UnoptimizedProcessBlockTree = this._processBlockLambda;
				scriptBlock.UnoptimizedEndBlockTree = this._endBlockLambda;
				scriptBlock.UnoptimizedLocalsMutableTupleType = this.LocalVariablesTupleType;
			}
			scriptBlock.CompileInterpretDecision = ((this._stmtCount > 300) ? CompileInterpretChoice.NeverCompile : CompileInterpretChoice.CompileOnDemand);
			if (scriptBlock.SequencePoints == null)
			{
				scriptBlock.SequencePoints = this._sequencePoints.ToArray();
			}
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0013B324 File Offset: 0x00139524
		internal static object GetExpressionValue(ExpressionAst expressionAst, bool isTrustedInput, ExecutionContext context, IDictionary usingValues = null)
		{
			return Compiler.GetExpressionValue(expressionAst, isTrustedInput, context, null, usingValues);
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0013B330 File Offset: 0x00139530
		internal static object GetExpressionValue(ExpressionAst expressionAst, bool isTrustedInput, ExecutionContext context, SessionStateInternal sessionStateInternal, IDictionary usingValues = null)
		{
			Func<FunctionContext, object> func = null;
			IScriptExtent[] array = null;
			Type type = null;
			return Compiler.GetExpressionValue(expressionAst, isTrustedInput, context, sessionStateInternal, usingValues, ref func, ref array, ref type);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0013B354 File Offset: 0x00139554
		private static object GetExpressionValue(ExpressionAst expressionAst, bool isTrustedInput, ExecutionContext context, SessionStateInternal sessionStateInternal, IDictionary usingValues, ref Func<FunctionContext, object> lambda, ref IScriptExtent[] sequencePoints, ref Type localsTupleType)
		{
			object result;
			if (IsConstantValueVisitor.IsConstant(expressionAst, out result, false, false))
			{
				return result;
			}
			if (!isTrustedInput)
			{
				return null;
			}
			VariableExpressionAst variableExpressionAst = expressionAst as VariableExpressionAst;
			if (variableExpressionAst != null)
			{
				return VariableOps.GetVariableValue(variableExpressionAst.VariablePath, context, variableExpressionAst);
			}
			if (lambda == null)
			{
				lambda = new Compiler().CompileSingleExpression(expressionAst, out sequencePoints, out localsTupleType);
			}
			SessionStateInternal engineSessionState = context.EngineSessionState;
			object result2;
			try
			{
				if (sessionStateInternal != null && context.EngineSessionState != sessionStateInternal)
				{
					context.EngineSessionState = sessionStateInternal;
				}
				List<object> list = new List<object>();
				Pipe outputPipe = new Pipe(list);
				try
				{
					FunctionContext functionContext = new FunctionContext
					{
						_sequencePoints = sequencePoints,
						_executionContext = context,
						_file = expressionAst.Extent.File,
						_outputPipe = outputPipe,
						_localsTuple = MutableTuple.MakeTuple(localsTupleType, Compiler.DottedLocalsNameIndexMap)
					};
					if (usingValues != null)
					{
						PSBoundParametersDictionary value = new PSBoundParametersDictionary
						{
							ImplicitUsingParameters = usingValues
						};
						functionContext._localsTuple.SetAutomaticVariable(AutomaticVariable.PSBoundParameters, value, context);
					}
					object obj = lambda(functionContext);
					if (obj == AutomationNull.Value)
					{
						result2 = ((list.Count == 0) ? null : PipelineOps.PipelineResult(list));
					}
					else
					{
						result2 = obj;
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
			catch (TerminateException)
			{
				throw;
			}
			catch (FlowControlException)
			{
				result2 = null;
			}
			finally
			{
				context.EngineSessionState = engineSessionState;
			}
			return result2;
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x0013B4C0 File Offset: 0x001396C0
		private Func<FunctionContext, object> CompileSingleExpression(ExpressionAst expressionAst, out IScriptExtent[] sequencePoints, out Type localsTupleType)
		{
			this.Optimize = false;
			this._compilingSingleExpression = true;
			Tuple<Type, Dictionary<string, int>> tuple = VariableAnalysis.AnalyzeExpression(expressionAst);
			Type item;
			localsTupleType = (item = tuple.Item1);
			this.LocalVariablesTupleType = item;
			this.LocalVariablesParameter = Expression.Variable(this.LocalVariablesTupleType, "locals");
			this._returnTarget = Expression.Label(typeof(object), "returnTarget");
			this._loopTargets.Clear();
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> temps = new List<ParameterExpression>
			{
				Compiler._executionContextParameter,
				this.LocalVariablesParameter
			};
			this.GenerateFunctionProlog(list, temps, null);
			this._sequencePoints.Add(expressionAst.Extent);
			list.Add(new UpdatePositionExpr(expressionAst.Extent, this._sequencePoints.Count - 1, this._debugSymbolDocument, true));
			Expression defaultValue = this.Compile(expressionAst).Cast(typeof(object));
			list.Add(Expression.Label(this._returnTarget, defaultValue));
			BlockExpression body = Expression.Block(new ParameterExpression[]
			{
				Compiler._executionContextParameter,
				this.LocalVariablesParameter
			}, list);
			ParameterExpression[] parameters = new ParameterExpression[]
			{
				Compiler._functionContext
			};
			sequencePoints = this._sequencePoints.ToArray();
			return Expression.Lambda<Func<FunctionContext, object>>(body, parameters).Compile();
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0013B618 File Offset: 0x00139818
		private Expression CaptureAstResults(Ast ast, Compiler.CaptureAstContext context, Compiler.MergeRedirectExprs generateRedirectExprs = null)
		{
			List<ParameterExpression> list = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			List<CatchBlock> list3 = new List<CatchBlock>();
			List<Expression> list4 = new List<Expression>();
			ParameterExpression parameterExpression = this.NewTemp(typeof(Pipe), "oldPipe");
			ParameterExpression parameterExpression2 = this.NewTemp(typeof(List<object>), "resultList");
			list.Add(parameterExpression2);
			list.Add(parameterExpression);
			list2.Add(Expression.Assign(parameterExpression, Compiler._getCurrentPipe));
			list2.Add(Expression.Assign(parameterExpression2, Expression.New(CachedReflectionInfo.ObjectList_ctor)));
			list2.Add(Expression.Assign(Compiler._getCurrentPipe, Expression.New(CachedReflectionInfo.Pipe_ctor, new Expression[]
			{
				parameterExpression2
			})));
			list2.Add(Expression.Call(parameterExpression, CachedReflectionInfo.Pipe_SetVariableListForTemporaryPipe, new Expression[]
			{
				Compiler._getCurrentPipe
			}));
			if (generateRedirectExprs != null)
			{
				generateRedirectExprs(list2, list4);
			}
			list2.Add(this.Compile(ast));
			Expression item;
			switch (context)
			{
			case Compiler.CaptureAstContext.Condition:
				item = DynamicExpression.Dynamic(PSPipelineResultToBoolBinder.Get(), typeof(bool), parameterExpression2);
				break;
			case Compiler.CaptureAstContext.Enumerable:
				item = parameterExpression2;
				break;
			case Compiler.CaptureAstContext.AssignmentWithResultPreservation:
			case Compiler.CaptureAstContext.AssignmentWithoutResultPreservation:
				item = Expression.Call(CachedReflectionInfo.PipelineOps_PipelineResult, parameterExpression2);
				if (context == Compiler.CaptureAstContext.AssignmentWithoutResultPreservation)
				{
					List<Expression> expressions = new List<Expression>
					{
						Expression.Call(CachedReflectionInfo.PipelineOps_ClearPipe, parameterExpression2),
						Expression.Rethrow(),
						Expression.Constant(null, typeof(object))
					};
					list3.Add(Expression.Catch(typeof(RuntimeException), Expression.Block(typeof(object), expressions)));
				}
				list4.Add(Expression.Call(CachedReflectionInfo.PipelineOps_FlushPipe, parameterExpression, parameterExpression2));
				break;
			default:
				throw new ArgumentOutOfRangeException("context");
			}
			list4.Add(Expression.Assign(Compiler._getCurrentPipe, parameterExpression));
			list2.Add(item);
			if (list3.Count > 0)
			{
				return Expression.Block(list.ToArray(), new Expression[]
				{
					Expression.TryCatchFinally(Expression.Block(list2), Expression.Block(list4), list3.ToArray())
				});
			}
			return Expression.Block(list.ToArray(), new Expression[]
			{
				Expression.TryFinally(Expression.Block(list2), Expression.Block(list4))
			});
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0013B864 File Offset: 0x00139A64
		private Expression CaptureStatementResultsHelper(StatementAst stmt, Compiler.CaptureAstContext context, Compiler.MergeRedirectExprs generateRedirectExprs)
		{
			CommandExpressionAst commandExpressionAst = stmt as CommandExpressionAst;
			if (commandExpressionAst != null)
			{
				if (commandExpressionAst.Redirections.Count > 0)
				{
					return this.GetRedirectedExpression(commandExpressionAst, true);
				}
				return this.Compile(commandExpressionAst.Expression);
			}
			else
			{
				AssignmentStatementAst assignmentStatementAst = stmt as AssignmentStatementAst;
				if (assignmentStatementAst != null)
				{
					Expression expression = this.Compile(assignmentStatementAst);
					if (stmt.Parent is StatementBlockAst)
					{
						expression = Expression.Block(expression, ExpressionCache.Empty);
					}
					return expression;
				}
				PipelineAst pipelineAst = stmt as PipelineAst;
				if (pipelineAst != null)
				{
					ExpressionAst pureExpression = pipelineAst.GetPureExpression();
					if (pureExpression != null)
					{
						return this.Compile(pureExpression);
					}
				}
				return this.CaptureAstResults(stmt, context, generateRedirectExprs);
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0013B900 File Offset: 0x00139B00
		private Expression CaptureStatementResults(StatementAst stmt, Compiler.CaptureAstContext context, Compiler.MergeRedirectExprs generateRedirectExprs = null)
		{
			Expression expression = this.CaptureStatementResultsHelper(stmt, context, generateRedirectExprs);
			if (context == Compiler.CaptureAstContext.Condition)
			{
				if (AstSearcher.FindFirst(stmt, (Ast ast) => ast is CommandAst, false) != null)
				{
					ParameterExpression parameterExpression = this.NewTemp(expression.Type, "condTmp");
					expression = Expression.Block(new ParameterExpression[]
					{
						parameterExpression
					}, new Expression[]
					{
						Expression.Assign(parameterExpression, expression),
						Compiler._setDollarQuestionToTrue,
						parameterExpression
					});
				}
			}
			return expression;
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0013B984 File Offset: 0x00139B84
		internal Expression CallAddPipe(Expression expr, Expression pipe)
		{
			if (!PSEnumerableBinder.IsStaticTypePossiblyEnumerable(expr.Type))
			{
				return Expression.Call(pipe, CachedReflectionInfo.Pipe_Add, new Expression[]
				{
					expr.Cast(typeof(object))
				});
			}
			return DynamicExpression.Dynamic(PSPipeWriterBinder.Get(), typeof(void), expr, pipe, Compiler._executionContextParameter);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0013B9E0 File Offset: 0x00139BE0
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return null;
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0013B9E3 File Offset: 0x00139BE3
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return ExpressionCache.Constant(1);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0013BA00 File Offset: 0x00139C00
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			FunctionDefinitionAst functionDefinitionAst = scriptBlockAst.Parent as FunctionDefinitionAst;
			string text = (functionDefinitionAst != null) ? functionDefinitionAst.Name : "<ScriptBlock>";
			ScriptBlockAst rootForDefiningTypes = (scriptBlockAst.Find((Ast ast) => ast is TypeDefinitionAst || ast is UsingStatementAst, true) != null) ? scriptBlockAst : null;
			if (scriptBlockAst.DynamicParamBlock != null)
			{
				this._dynamicParamBlockLambda = this.CompileNamedBlock(scriptBlockAst.DynamicParamBlock, text + "<DynamicParam>", rootForDefiningTypes);
				rootForDefiningTypes = null;
			}
			if (scriptBlockAst.BeginBlock != null)
			{
				this._beginBlockLambda = this.CompileNamedBlock(scriptBlockAst.BeginBlock, text + "<Begin>", rootForDefiningTypes);
				rootForDefiningTypes = null;
			}
			if (scriptBlockAst.ProcessBlock != null)
			{
				string funcName = text;
				if (!scriptBlockAst.ProcessBlock.Unnamed)
				{
					funcName = text + "<Process>";
				}
				this._processBlockLambda = this.CompileNamedBlock(scriptBlockAst.ProcessBlock, funcName, rootForDefiningTypes);
				rootForDefiningTypes = null;
			}
			if (scriptBlockAst.EndBlock != null)
			{
				if (!scriptBlockAst.EndBlock.Unnamed)
				{
					text += "<End>";
				}
				this._endBlockLambda = this.CompileNamedBlock(scriptBlockAst.EndBlock, text, rootForDefiningTypes);
			}
			return null;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0013BB14 File Offset: 0x00139D14
		private Expression<Action<FunctionContext>> CompileNamedBlock(NamedBlockAst namedBlockAst, string funcName, ScriptBlockAst rootForDefiningTypes)
		{
			IScriptExtent entryExtent = null;
			IScriptExtent exitExtent = null;
			if (namedBlockAst.Unnamed)
			{
				ScriptBlockAst scriptBlockAst = (ScriptBlockAst)namedBlockAst.Parent;
				if (scriptBlockAst.Parent != null && scriptBlockAst.Extent is InternalScriptExtent)
				{
					InternalScriptExtent internalScriptExtent = (InternalScriptExtent)scriptBlockAst.Extent;
					entryExtent = new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.StartOffset, internalScriptExtent.StartOffset + 1);
					exitExtent = new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.EndOffset - 1, internalScriptExtent.EndOffset);
				}
			}
			else
			{
				entryExtent = namedBlockAst.OpenCurlyExtent;
				exitExtent = namedBlockAst.CloseCurlyExtent;
			}
			return this.CompileSingleLambda(namedBlockAst.Statements, namedBlockAst.Traps, funcName, entryExtent, exitExtent, rootForDefiningTypes);
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x0013BBB4 File Offset: 0x00139DB4
		private Tuple<Action<FunctionContext>, Type> CompileTrap(TrapStatementAst trap)
		{
			Compiler compiler = new Compiler(this._sequencePoints)
			{
				_compilingTrap = true
			};
			string text = this._currentFunctionName + "<trap>";
			if (trap.TrapType != null)
			{
				text = text + "<" + trap.TrapType.TypeName.Name + ">";
			}
			Tuple<Type, Dictionary<string, int>> tuple = VariableAnalysis.AnalyzeTrap(trap);
			compiler.LocalVariablesTupleType = tuple.Item1;
			compiler.LocalVariablesParameter = Expression.Variable(compiler.LocalVariablesTupleType, "locals");
			Expression<Action<FunctionContext>> expression = compiler.CompileSingleLambda(trap.Body.Statements, trap.Body.Traps, text, null, null, null);
			return Tuple.Create<Action<FunctionContext>, Type>(expression.Compile(), compiler.LocalVariablesTupleType);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0013BCA0 File Offset: 0x00139EA0
		private Expression<Action<FunctionContext>> CompileSingleLambda(ReadOnlyCollection<StatementAst> statements, ReadOnlyCollection<TrapStatementAst> traps, string funcName, IScriptExtent entryExtent, IScriptExtent exitExtent, ScriptBlockAst rootForDefiningTypesAndUsings)
		{
			this._currentFunctionName = funcName;
			this._loopTargets.Clear();
			this._returnTarget = Expression.Label("returnTarget");
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			this.GenerateFunctionProlog(list, list2, entryExtent);
			if (rootForDefiningTypesAndUsings != null)
			{
				this.GenerateTypesAndUsings(rootForDefiningTypesAndUsings, list);
			}
			List<Expression> list3 = new List<Expression>();
			if (this.CompilingMemberFunction)
			{
				list2.Add(Compiler._returnPipe);
			}
			this.CompileStatementListWithTraps(statements, traps, list3, list2);
			list.AddRange(list3);
			list.Add(Expression.Label(this._returnTarget));
			this.GenerateFunctionEpilog(list, exitExtent);
			list2.Add(this.LocalVariablesParameter);
			Expression body = Expression.Block(list2, list);
			if (!this._compilingTrap)
			{
				if (traps == null || traps.Count <= 0)
				{
					if (!statements.Any((StatementAst stmt) => AstSearcher.Contains(stmt, (Ast ast) => ast is TrapStatementAst, false)))
					{
						goto IL_143;
					}
				}
				body = Expression.Block(new ParameterExpression[]
				{
					Compiler._executionContextParameter
				}, new Expression[]
				{
					Expression.TryCatchFinally(body, Expression.Call(Expression.Field(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_Debugger), CachedReflectionInfo.Debugger_ExitScriptFunction), new CatchBlock[]
					{
						Expression.Catch(typeof(ReturnException), ExpressionCache.Empty)
					})
				});
				goto IL_189;
			}
			IL_143:
			body = Expression.Block(new ParameterExpression[]
			{
				Compiler._executionContextParameter
			}, new Expression[]
			{
				Expression.TryFinally(body, Expression.Call(Expression.Field(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_Debugger), CachedReflectionInfo.Debugger_ExitScriptFunction))
			});
			IL_189:
			return Expression.Lambda<Action<FunctionContext>>(body, funcName, new ParameterExpression[]
			{
				Compiler._functionContext
			});
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0013BE7C File Offset: 0x0013A07C
		private void GenerateTypesAndUsings(ScriptBlockAst rootForDefiningTypesAndUsings, List<Expression> exprs)
		{
			if (rootForDefiningTypesAndUsings.Parent == null)
			{
				if (rootForDefiningTypesAndUsings.UsingStatements.Any<UsingStatementAst>())
				{
					bool allUsingsAreNamespaces = rootForDefiningTypesAndUsings.UsingStatements.All((UsingStatementAst us) => us.UsingStatementKind == UsingStatementKind.Namespace);
					Compiler.GenerateLoadUsings(rootForDefiningTypesAndUsings.UsingStatements, allUsingsAreNamespaces, exprs);
				}
				TypeDefinitionAst[] array = rootForDefiningTypesAndUsings.FindAll((Ast ast) => ast is TypeDefinitionAst, true).Cast<TypeDefinitionAst>().ToArray<TypeDefinitionAst>();
				if (array.Length > 0)
				{
					Assembly value = Compiler.DefinePowerShellTypes(rootForDefiningTypesAndUsings, array);
					exprs.Add(Expression.Call(CachedReflectionInfo.TypeOps_SetAssemblyDefiningPSTypes, Compiler._functionContext, Expression.Constant(value)));
				}
			}
			Dictionary<string, TypeDefinitionAst> dictionary = rootForDefiningTypesAndUsings.FindAll((Ast ast) => ast is TypeDefinitionAst, false).Cast<TypeDefinitionAst>().ToDictionary((TypeDefinitionAst type) => type.Name);
			if (dictionary.Count > 0)
			{
				exprs.Add(Expression.Call(CachedReflectionInfo.TypeOps_AddPowerShellTypesToTheScope, Expression.Constant(dictionary), Compiler._executionContextParameter));
			}
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x0013BFA0 File Offset: 0x0013A1A0
		internal static void GenerateLoadUsings(IEnumerable<UsingStatementAst> usingStatements, bool allUsingsAreNamespaces, List<Expression> exprs)
		{
			Dictionary<string, TypeDefinitionAst> dictionary = null;
			TypeResolutionState value;
			if (allUsingsAreNamespaces)
			{
				value = new TypeResolutionState(TypeOps.GetNamespacesForTypeResolutionState(usingStatements), null);
			}
			else
			{
				Assembly[] assemblies;
				dictionary = Compiler.LoadUsingsImpl(usingStatements, out assemblies);
				value = new TypeResolutionState(TypeOps.GetNamespacesForTypeResolutionState(usingStatements), assemblies);
			}
			exprs.Add(Expression.Call(CachedReflectionInfo.TypeOps_SetCurrentTypeResolutionState, Expression.Constant(value), Compiler._executionContextParameter));
			if (dictionary != null && dictionary.Count > 0)
			{
				exprs.Add(Expression.Call(CachedReflectionInfo.TypeOps_AddPowerShellTypesToTheScope, Expression.Constant(dictionary), Compiler._executionContextParameter));
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x0013C01C File Offset: 0x0013A21C
		internal static Assembly DefinePowerShellTypes(Ast rootForDefiningTypes, TypeDefinitionAst[] typeAsts)
		{
			if (typeAsts[0].Type != null)
			{
				foreach (TypeDefinitionAst typeDefinitionAst in typeAsts)
				{
					typeDefinitionAst.Type = null;
				}
			}
			Parser parser = new Parser();
			Assembly result = TypeDefiner.DefineTypes(parser, rootForDefiningTypes, typeAsts);
			if (parser.ErrorList.Count > 0)
			{
				foreach (TypeDefinitionAst typeDefinitionAst2 in typeAsts)
				{
					typeDefinitionAst2.Type = null;
				}
				throw new ParseException(parser.ErrorList.ToArray());
			}
			foreach (Type type in typeAsts[0].Type.GetTypeInfo().Assembly.GetTypes())
			{
				if (Regex.IsMatch(type.Name, "<.*_staticHelpers>"))
				{
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
					{
						((ScriptBlockMemberMethodWrapper)fieldInfo.GetValue(null)).InitAtRuntime();
					}
				}
			}
			return result;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x0013C12C File Offset: 0x0013A32C
		private static Dictionary<string, TypeDefinitionAst> LoadUsingsImpl(IEnumerable<UsingStatementAst> usingAsts, out Assembly[] assemblies)
		{
			List<Assembly> list = new List<Assembly>();
			Dictionary<string, TypeDefinitionAst> dictionary = new Dictionary<string, TypeDefinitionAst>(StringComparer.OrdinalIgnoreCase);
			foreach (UsingStatementAst usingStatementAst in usingAsts)
			{
				switch (usingStatementAst.UsingStatementKind)
				{
				case UsingStatementKind.Assembly:
					list.Add(Compiler.LoadAssembly(usingStatementAst.Name.Value, usingStatementAst.Extent.File));
					break;
				case UsingStatementKind.Module:
				{
					PSModuleInfo psmoduleInfo = Compiler.LoadModule(usingStatementAst.ModuleInfo);
					if (psmoduleInfo != null)
					{
						if (psmoduleInfo.ImplementingAssembly != null)
						{
							list.Add(psmoduleInfo.ImplementingAssembly);
						}
						ReadOnlyDictionary<string, TypeDefinitionAst> exportedTypeDefinitions = psmoduleInfo.GetExportedTypeDefinitions();
						Compiler.PopulateRuntimeTypes(usingStatementAst.ModuleInfo.GetExportedTypeDefinitions(), exportedTypeDefinitions);
						foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in exportedTypeDefinitions)
						{
							dictionary[SymbolResolver.GetModuleQualifiedName(psmoduleInfo.Name, keyValuePair.Key)] = keyValuePair.Value;
						}
					}
					break;
				}
				}
			}
			assemblies = list.ToArray();
			return dictionary;
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x0013C27C File Offset: 0x0013A47C
		private static void PopulateRuntimeTypes(ReadOnlyDictionary<string, TypeDefinitionAst> parseTimeTypes, ReadOnlyDictionary<string, TypeDefinitionAst> runtimeTypes)
		{
			foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in parseTimeTypes)
			{
				if (keyValuePair.Value.Type == null)
				{
					if (!runtimeTypes.ContainsKey(keyValuePair.Key))
					{
						throw InterpreterError.NewInterpreterException(keyValuePair.Value, typeof(RuntimeException), keyValuePair.Value.Extent, "TypeNotFound", ParserStrings.TypeNotFound, new object[]
						{
							keyValuePair.Value.Name
						});
					}
					keyValuePair.Value.Type = runtimeTypes[keyValuePair.Key].Type;
				}
			}
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x0013C348 File Offset: 0x0013A548
		private static Assembly LoadAssembly(string assemblyName, string scriptFileName)
		{
			string text = assemblyName;
			Assembly assembly = null;
			try
			{
				if (!string.IsNullOrEmpty(scriptFileName) && !Path.IsPathRooted(text))
				{
					text = Path.GetDirectoryName(scriptFileName) + "\\" + text;
				}
				if (!File.Exists(text))
				{
					GlobalAssemblyCache.ResolvePartialName(assemblyName, out text, null, null);
				}
				if (File.Exists(text))
				{
					assembly = ClrFacade.LoadFrom(text);
				}
			}
			catch
			{
			}
			if (assembly == null)
			{
				throw InterpreterError.NewInterpreterException(assemblyName, typeof(RuntimeException), null, "ErrorLoadingAssembly", ParserStrings.ErrorLoadingAssembly, new object[]
				{
					assemblyName
				});
			}
			return assembly;
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0013C3E4 File Offset: 0x0013A5E4
		private static PSModuleInfo LoadModule(PSModuleInfo originalModuleInfo)
		{
			string path = originalModuleInfo.Path;
			CmdletInfo commandInfo = new CmdletInfo("Import-Module", typeof(ImportModuleCommand));
			PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(commandInfo).AddParameter("Name", path).AddParameter("PassThru");
			Collection<PSModuleInfo> collection = powerShell.Invoke<PSModuleInfo>();
			if (powerShell.HadErrors)
			{
				ErrorRecord errorRecord = powerShell.Streams.Error[0];
				throw InterpreterError.NewInterpreterException(path, typeof(RuntimeException), null, errorRecord.FullyQualifiedErrorId, errorRecord.ToString(), new object[0]);
			}
			if (collection.Count == 1)
			{
				return collection[0];
			}
			return null;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0013C48C File Offset: 0x0013A68C
		private void GenerateFunctionProlog(List<Expression> exprs, List<ParameterExpression> temps, IScriptExtent entryExtent)
		{
			exprs.Add(Expression.Assign(Compiler._executionContextParameter, Expression.Field(Compiler._functionContext, CachedReflectionInfo.FunctionContext__executionContext)));
			exprs.Add(Expression.Assign(this.LocalVariablesParameter, Expression.Field(Compiler._functionContext, CachedReflectionInfo.FunctionContext__localsTuple).Cast(this.LocalVariablesTupleType)));
			if (this.CompilingMemberFunction)
			{
				exprs.Add(Expression.Assign(Compiler._returnPipe, Compiler._getCurrentPipe));
				exprs.Add(Expression.Assign(Compiler._getCurrentPipe, ExpressionCache.NullPipe));
				ParameterExpression parameterExpression = this.NewTemp(this._memberFunctionType.Type, "this");
				temps.Add(parameterExpression);
				exprs.Add(Expression.Assign(parameterExpression, this.GetLocal(Array.IndexOf<string>(SpecialVariables.AutomaticVariables, "this")).Cast(this._memberFunctionType.Type)));
				if (this.SpecialMemberFunctionType == SpecialMemberFunctionType.DefaultConstructor)
				{
					using (IEnumerator<PropertyMemberAst> enumerator = this._memberFunctionType.Members.OfType<PropertyMemberAst>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PropertyMemberAst propertyMemberAst = enumerator.Current;
							if (propertyMemberAst.InitialValue != null && !propertyMemberAst.IsStatic)
							{
								PropertyInfo property = this._memberFunctionType.Type.GetProperty(propertyMemberAst.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								exprs.Add(Expression.Assign(Expression.Property(parameterExpression, property), this.Compile(propertyMemberAst.InitialValue).Convert(property.PropertyType)));
							}
						}
						goto IL_200;
					}
				}
				if (this.SpecialMemberFunctionType == SpecialMemberFunctionType.StaticConstructor)
				{
					foreach (PropertyMemberAst propertyMemberAst2 in this._memberFunctionType.Members.OfType<PropertyMemberAst>())
					{
						if (propertyMemberAst2.InitialValue != null && propertyMemberAst2.IsStatic)
						{
							PropertyInfo property2 = this._memberFunctionType.Type.GetProperty(propertyMemberAst2.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							exprs.Add(Expression.Assign(Expression.Property(null, property2), this.Compile(propertyMemberAst2.InitialValue).Convert(property2.PropertyType)));
						}
					}
				}
			}
			IL_200:
			if (!this._compilingSingleExpression)
			{
				exprs.Add(Expression.Assign(Expression.Field(Compiler._functionContext, CachedReflectionInfo.FunctionContext__functionName), Expression.Constant(this._currentFunctionName)));
				if (entryExtent != null)
				{
					this._sequencePoints.Add(entryExtent);
					exprs.Add(new UpdatePositionExpr(entryExtent, this._sequencePoints.Count - 1, this._debugSymbolDocument, false));
				}
				exprs.Add(Expression.Call(Expression.Field(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_Debugger), CachedReflectionInfo.Debugger_EnterScriptFunction, new Expression[]
				{
					Compiler._functionContext
				}));
			}
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0013C748 File Offset: 0x0013A948
		private void GenerateFunctionEpilog(List<Expression> exprs, IScriptExtent exitExtent)
		{
			if (exitExtent != null)
			{
				exprs.Add(this.UpdatePosition(new SequencePointAst(exitExtent)));
			}
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x0013C75F File Offset: 0x0013A95F
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return null;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x0013C762 File Offset: 0x0013A962
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return null;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0013C765 File Offset: 0x0013A965
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return null;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0013C768 File Offset: 0x0013A968
		public object VisitParameter(ParameterAst parameterAst)
		{
			return null;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0013C76B File Offset: 0x0013A96B
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return null;
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0013C76E File Offset: 0x0013A96E
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return null;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0013C774 File Offset: 0x0013A974
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			this.CompileStatementListWithTraps(statementBlockAst.Statements, statementBlockAst.Traps, list, list2);
			if (list.Count == 0)
			{
				list.Add(ExpressionCache.Empty);
			}
			return Expression.Block(typeof(void), list2, list);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0013C7C8 File Offset: 0x0013A9C8
		private void CompileStatementListWithTraps(ReadOnlyCollection<StatementAst> statements, ReadOnlyCollection<TrapStatementAst> traps, List<Expression> exprs, List<ParameterExpression> temps)
		{
			if (statements.Count == 0)
			{
				exprs.Add(ExpressionCache.Empty);
				return;
			}
			List<Expression> list = exprs;
			Expression expression;
			ParameterExpression parameterExpression;
			ParameterExpression parameterExpression2;
			if (traps != null)
			{
				exprs = new List<Expression>();
				expression = Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_ExceptionHandlerInEnclosingStatementBlock);
				parameterExpression = this.NewTemp(typeof(bool), "oldActiveHandler");
				exprs.Add(Expression.Assign(parameterExpression, expression));
				exprs.Add(Expression.Assign(expression, ExpressionCache.Constant(true)));
				List<Expression> list2 = new List<Expression>();
				List<Action<FunctionContext>> list3 = new List<Action<FunctionContext>>();
				List<Type> list4 = new List<Type>();
				for (int i = 0; i < traps.Count; i++)
				{
					TrapStatementAst trapStatementAst = traps[i];
					list2.Add((trapStatementAst.TrapType != null) ? this.CompileTypeName(trapStatementAst.TrapType.TypeName) : ExpressionCache.CatchAllType);
					Tuple<Action<FunctionContext>, Type> tuple = this.CompileTrap(trapStatementAst);
					list3.Add(tuple.Item1);
					list4.Add(tuple.Item2);
				}
				exprs.Add(Expression.Call(Compiler._functionContext, CachedReflectionInfo.FunctionContext_PushTrapHandlers, Expression.NewArrayInit(typeof(Type), list2), Expression.Constant(list3.ToArray()), Expression.Constant(list4.ToArray())));
				parameterExpression2 = this.NewTemp(typeof(bool), "trapHandlersPushed");
				exprs.Add(Expression.Assign(parameterExpression2, ExpressionCache.Constant(true)));
				this._trapNestingCount++;
			}
			else
			{
				parameterExpression = null;
				expression = null;
				parameterExpression2 = null;
				if (this._trapNestingCount > 0)
				{
					exprs = new List<Expression>();
					exprs.Add(Expression.Call(Compiler._functionContext, CachedReflectionInfo.FunctionContext_PushTrapHandlers, ExpressionCache.NullTypeArray, ExpressionCache.NullDelegateArray, ExpressionCache.NullTypeArray));
					parameterExpression2 = this.NewTemp(typeof(bool), "trapHandlersPushed");
					exprs.Add(Expression.Assign(parameterExpression2, ExpressionCache.Constant(true)));
				}
			}
			this._stmtCount += statements.Count;
			if (statements.Count == 1)
			{
				List<Expression> list5 = new List<Expression>(3);
				this.CompileTrappableExpression(list5, statements[0]);
				list5.Add(ExpressionCache.Empty);
				TryExpression item = Expression.TryCatch(Expression.Block(list5), Compiler._stmtCatchHandlers);
				exprs.Add(item);
			}
			else
			{
				SwitchCase[] array = new SwitchCase[statements.Count + 1];
				LabelTarget[] array2 = new LabelTarget[statements.Count + 1];
				for (int j = 0; j <= statements.Count; j++)
				{
					array2[j] = Expression.Label();
					array[j] = Expression.SwitchCase(Expression.Goto(array2[j]), new Expression[]
					{
						ExpressionCache.Constant(j)
					});
				}
				ParameterExpression parameterExpression3 = Expression.Variable(typeof(int), "stmt");
				temps.Add(parameterExpression3);
				exprs.Add(Expression.Assign(parameterExpression3, ExpressionCache.Constant(0)));
				LabelTarget target = Expression.Label();
				exprs.Add(Expression.Label(target));
				List<Expression> list6 = new List<Expression>();
				list6.Add(Expression.Switch(parameterExpression3, array));
				for (int k = 0; k < statements.Count; k++)
				{
					list6.Add(Expression.Label(array2[k]));
					list6.Add(Expression.Assign(parameterExpression3, ExpressionCache.Constant(k + 1)));
					this.CompileTrappableExpression(list6, statements[k]);
				}
				list6.Add(ExpressionCache.Empty);
				ParameterExpression parameterExpression4 = Expression.Variable(typeof(Exception), "exception");
				MethodCallExpression arg = Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_CheckActionPreference, Compiler._functionContext, parameterExpression4);
				CatchBlock catchBlock = Expression.Catch(parameterExpression4, Expression.Block(arg, Expression.Goto(target)));
				TryExpression item2 = Expression.TryCatch(Expression.Block(list6), new CatchBlock[]
				{
					Compiler._catchFlowControl,
					catchBlock
				});
				exprs.Add(item2);
				exprs.Add(Expression.Label(array2[statements.Count]));
			}
			if (this._trapNestingCount > 0)
			{
				List<ParameterExpression> list7 = new List<ParameterExpression>();
				List<Expression> list8 = new List<Expression>();
				if (parameterExpression != null)
				{
					list7.Add(parameterExpression);
					list8.Add(Expression.Assign(expression, parameterExpression));
				}
				list7.Add(parameterExpression2);
				list8.Add(Expression.IfThen(parameterExpression2, Expression.Call(Compiler._functionContext, CachedReflectionInfo.FunctionContext_PopTrapHandlers)));
				list.Add(Expression.Block(list7, new Expression[]
				{
					Expression.TryFinally(Expression.Block(exprs), Expression.Block(list8))
				}));
			}
			if (traps != null)
			{
				this._trapNestingCount--;
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0013CC28 File Offset: 0x0013AE28
		private void CompileTrappableExpression(List<Expression> exprList, StatementAst stmt)
		{
			Expression item = this.Compile(stmt);
			exprList.Add(item);
			PipelineAst pipelineAst = stmt as PipelineAst;
			if (pipelineAst != null)
			{
				if (pipelineAst.PipelineElements.Count == 1 && pipelineAst.PipelineElements[0] is CommandExpressionAst)
				{
					exprList.Add(Compiler._setDollarQuestionToTrue);
					return;
				}
			}
			else
			{
				AssignmentStatementAst assignmentStatementAst = stmt as AssignmentStatementAst;
				if (assignmentStatementAst != null)
				{
					Ast ast = null;
					for (AssignmentStatementAst assignmentStatementAst2 = assignmentStatementAst; assignmentStatementAst2 != null; assignmentStatementAst2 = (ast as AssignmentStatementAst))
					{
						ast = assignmentStatementAst2.Right;
					}
					pipelineAst = (ast as PipelineAst);
					if (ast is CommandExpressionAst || (pipelineAst != null && pipelineAst.PipelineElements.Count == 1 && pipelineAst.PipelineElements[0] is CommandExpressionAst))
					{
						exprList.Add(Compiler._setDollarQuestionToTrue);
					}
				}
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0013CCE1 File Offset: 0x0013AEE1
		public object VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			return ExpressionCache.Empty;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0013CCE8 File Offset: 0x0013AEE8
		public object VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			return null;
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0013CCEB File Offset: 0x0013AEEB
		public object VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			return null;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0013CCF0 File Offset: 0x0013AEF0
		public object VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
		{
			Expression target = this.CompileExpressionOperand(baseCtorInvokeMemberExpressionAst.Expression);
			IEnumerable<Expression> args = this.CompileInvocationArguments(baseCtorInvokeMemberExpressionAst.Arguments);
			PSMethodInvocationConstraints invokeMemberConstraints = Compiler.GetInvokeMemberConstraints(baseCtorInvokeMemberExpressionAst);
			return this.InvokeBaseCtorMethod(invokeMemberConstraints, target, args);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0013CD27 File Offset: 0x0013AF27
		public object VisitUsingStatement(UsingStatementAst usingStatementAst)
		{
			return ExpressionCache.Empty;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0013CD2E File Offset: 0x0013AF2E
		public object VisitConfigurationDefinition(ConfigurationDefinitionAst configurationAst)
		{
			return this.VisitPipeline(configurationAst.GenerateSetItemPipelineAst());
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0013CD3C File Offset: 0x0013AF3C
		public object VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst)
		{
			if (dynamicKeywordAst.Keyword.MetaStatement)
			{
				return Expression.Empty();
			}
			return this.VisitPipeline(dynamicKeywordAst.GenerateCommandCallPipelineAst());
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x0013CD60 File Offset: 0x0013AF60
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			if (!functionDefinitionAst.IsWorkflow)
			{
				return Expression.Call(CachedReflectionInfo.FunctionOps_DefineFunction, Compiler._executionContextParameter, Expression.Constant(functionDefinitionAst), Expression.Constant(new ScriptBlockExpressionWrapper(functionDefinitionAst)));
			}
			if (this.generatedCallToDefineWorkflows)
			{
				return ExpressionCache.Empty;
			}
			Ast parent = functionDefinitionAst.Parent;
			while (!(parent is ScriptBlockAst))
			{
				parent = parent.Parent;
			}
			this.generatedCallToDefineWorkflows = true;
			return Expression.Call(CachedReflectionInfo.FunctionOps_DefineWorkflows, Compiler._executionContextParameter, Expression.Constant(parent, typeof(ScriptBlockAst)));
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0013CDE4 File Offset: 0x0013AFE4
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			int count = ifStmtAst.Clauses.Count;
			Tuple<BlockExpression, Expression>[] array = new Tuple<BlockExpression, Expression>[count];
			for (int i = 0; i < count; i++)
			{
				Tuple<PipelineBaseAst, StatementBlockAst> tuple = ifStmtAst.Clauses[i];
				BlockExpression item = Expression.Block(this.UpdatePosition(tuple.Item1), this.CaptureStatementResults(tuple.Item1, Compiler.CaptureAstContext.Condition, null).Convert(typeof(bool)));
				Expression item2 = this.Compile(tuple.Item2);
				array[i] = Tuple.Create<BlockExpression, Expression>(item, item2);
			}
			Expression expression = null;
			if (ifStmtAst.ElseClause != null)
			{
				expression = this.Compile(ifStmtAst.ElseClause);
			}
			Expression result = null;
			for (int j = count - 1; j >= 0; j--)
			{
				BlockExpression item3 = array[j].Item1;
				Expression item4 = array[j].Item2;
				if (expression != null)
				{
					expression = (result = Expression.IfThenElse(item3, item4, expression));
				}
				else
				{
					expression = (result = Expression.IfThen(item3, item4));
				}
			}
			return result;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0013CED1 File Offset: 0x0013B0D1
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			return null;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0013CED4 File Offset: 0x0013B0D4
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			return this.CompileAssignment(assignmentStatementAst, null);
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0013CEE0 File Offset: 0x0013B0E0
		private Expression CompileAssignment(AssignmentStatementAst assignmentStatementAst, Compiler.MergeRedirectExprs generateRedirectExprs = null)
		{
			ArrayLiteralAst arrayLiteralAst = assignmentStatementAst.Left as ArrayLiteralAst;
			ParenExpressionAst parenExpressionAst = assignmentStatementAst.Left as ParenExpressionAst;
			if (parenExpressionAst != null)
			{
				arrayLiteralAst = (parenExpressionAst.Pipeline.GetPureExpression() as ArrayLiteralAst);
			}
			Expression expression = this.CaptureStatementResults(assignmentStatementAst.Right, (arrayLiteralAst != null) ? Compiler.CaptureAstContext.Enumerable : Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, generateRedirectExprs);
			if (arrayLiteralAst != null)
			{
				expression = DynamicExpression.Dynamic(PSArrayAssignmentRHSBinder.Get(arrayLiteralAst.Elements.Count), typeof(IList), expression);
			}
			List<Expression> expressions = new List<Expression>
			{
				this.UpdatePosition(assignmentStatementAst),
				this.ReduceAssignment((ISupportsAssignment)assignmentStatementAst.Left, assignmentStatementAst.Operator, expression)
			};
			return Expression.Block(expressions);
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0013CFC8 File Offset: 0x0013B1C8
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			List<ParameterExpression> list = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			if (!(pipelineAst.Parent is AssignmentStatementAst) && !(pipelineAst.Parent is ParenExpressionAst))
			{
				list2.Add(this.UpdatePosition(pipelineAst));
			}
			ReadOnlyCollection<CommandBaseAst> pipelineElements = pipelineAst.PipelineElements;
			CommandExpressionAst commandExpressionAst = pipelineElements[0] as CommandExpressionAst;
			if (commandExpressionAst != null && pipelineElements.Count == 1)
			{
				if (commandExpressionAst.Redirections.Count > 0)
				{
					list2.Add(this.GetRedirectedExpression(commandExpressionAst, false));
				}
				else
				{
					list2.Add(this.Compile(commandExpressionAst));
				}
			}
			else
			{
				Expression expression;
				int i;
				int num;
				if (commandExpressionAst != null)
				{
					if (commandExpressionAst.Redirections.Count > 0)
					{
						expression = this.GetRedirectedExpression(commandExpressionAst, true);
					}
					else
					{
						expression = (this.GetRangeEnumerator(commandExpressionAst.Expression) ?? this.Compile(commandExpressionAst.Expression));
					}
					i = 1;
					num = pipelineElements.Count - 1;
				}
				else
				{
					expression = ExpressionCache.AutomationNullConstant;
					i = 0;
					num = pipelineElements.Count;
				}
				Expression[] array = new Expression[num];
				CommandBaseAst[] array2 = new CommandBaseAst[num];
				object[] array3 = new object[num];
				int num2 = 0;
				while (i < pipelineElements.Count)
				{
					CommandBaseAst commandBaseAst = pipelineElements[i];
					array[num2] = this.Compile(commandBaseAst);
					array3[num2] = this.GetCommandRedirections(commandBaseAst);
					array2[num2] = commandBaseAst;
					i++;
					num2++;
				}
				Expression expression2;
				if (array3.Any((object r) => r is Expression))
				{
					expression2 = Expression.NewArrayInit(typeof(CommandRedirection[]), from r in array3
					select (r as Expression) ?? Expression.Constant(r, typeof(CommandRedirection[])));
				}
				else if (array3.Any((object r) => r != null))
				{
					expression2 = Expression.Constant(array3.Map((object r) => r as CommandRedirection[]));
				}
				else
				{
					expression2 = ExpressionCache.NullCommandRedirections;
				}
				if (commandExpressionAst != null)
				{
					ParameterExpression parameterExpression = Expression.Variable(expression.Type);
					list.Add(parameterExpression);
					list2.Add(Expression.Assign(parameterExpression, expression));
					expression = parameterExpression;
				}
				Expression item = Expression.Call(CachedReflectionInfo.PipelineOps_InvokePipeline, new Expression[]
				{
					expression.Cast(typeof(object)),
					(commandExpressionAst != null) ? ExpressionCache.FalseConstant : ExpressionCache.TrueConstant,
					Expression.NewArrayInit(typeof(CommandParameterInternal[]), array),
					Expression.Constant(array2),
					expression2,
					Compiler._functionContext
				});
				list2.Add(item);
			}
			return Expression.Block(list, list2);
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0013D29C File Offset: 0x0013B49C
		private object GetCommandRedirections(CommandBaseAst command)
		{
			int count = command.Redirections.Count;
			if (count == 0)
			{
				return null;
			}
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = command.Redirections[i].Accept(this);
			}
			if (array.Any((object r) => r is Expression))
			{
				return Expression.NewArrayInit(typeof(CommandRedirection), from r in array
				select (r as Expression) ?? Expression.Constant(r));
			}
			return array.Map((object r) => (CommandRedirection)r);
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x0013D3C8 File Offset: 0x0013B5C8
		private Expression GetRedirectedExpression(CommandExpressionAst commandExpr, bool captureForInput)
		{
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> temps = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			bool flag = commandExpr.Redirections.Any((RedirectionAst r) => r is FileRedirectionAst && (r.FromStream == RedirectionStream.Output || r.FromStream == RedirectionStream.All));
			ParameterExpression parameterExpression = null;
			ParameterExpression parameterExpression2 = null;
			SubExpressionAst subExpressionAst = commandExpr.Expression as SubExpressionAst;
			if (subExpressionAst != null && captureForInput)
			{
				parameterExpression2 = this.NewTemp(typeof(Pipe), "oldPipe");
				parameterExpression = this.NewTemp(typeof(List<object>), "resultList");
				temps.Add(parameterExpression);
				temps.Add(parameterExpression2);
				list.Add(Expression.Assign(parameterExpression2, Compiler._getCurrentPipe));
				list.Add(Expression.Assign(parameterExpression, Expression.New(CachedReflectionInfo.ObjectList_ctor)));
				list.Add(Expression.Assign(Compiler._getCurrentPipe, Expression.New(CachedReflectionInfo.Pipe_ctor, new Expression[]
				{
					parameterExpression
				})));
				list.Add(Expression.Call(parameterExpression2, CachedReflectionInfo.Pipe_SetVariableListForTemporaryPipe, new Expression[]
				{
					Compiler._getCurrentPipe
				}));
			}
			foreach (FileRedirectionAst fileRedirectionAst in commandExpr.Redirections.OfType<FileRedirectionAst>())
			{
				object obj = this.VisitFileRedirection(fileRedirectionAst);
				ParameterExpression parameterExpression3 = this.NewTemp(typeof(Pipe[]), "savedPipes");
				temps.Add(parameterExpression3);
				ParameterExpression parameterExpression4 = this.NewTemp(typeof(FileRedirection), "fileRedirection");
				temps.Add(parameterExpression4);
				list.Add(Expression.Assign(parameterExpression4, (Expression)obj));
				list.Add(Expression.Assign(parameterExpression3, Expression.Call(parameterExpression4, CachedReflectionInfo.FileRedirection_BindForExpression, new Expression[]
				{
					Compiler._functionContext
				})));
				list2.Add(Expression.Call(parameterExpression4.Cast(typeof(CommandRedirection)), CachedReflectionInfo.CommandRedirection_UnbindForExpression, Compiler._functionContext, parameterExpression3));
				list2.Add(Expression.Call(parameterExpression4, CachedReflectionInfo.FileRedirection_Dispose));
			}
			Expression expression = null;
			ParenExpressionAst parenExpressionAst = commandExpr.Expression as ParenExpressionAst;
			if (parenExpressionAst != null)
			{
				AssignmentStatementAst assignmentStatementAst = parenExpressionAst.Pipeline as AssignmentStatementAst;
				if (assignmentStatementAst != null)
				{
					expression = this.CompileAssignment(assignmentStatementAst, delegate(List<Expression> mergeExprs, List<Expression> mergeFinallyExprs)
					{
						this.AddMergeRedirectionExpressions(commandExpr.Redirections, temps, mergeExprs, mergeFinallyExprs);
					});
				}
				else
				{
					bool flag2 = parenExpressionAst.ShouldPreserveOutputInCaseOfException();
					expression = this.CaptureAstResults(parenExpressionAst.Pipeline, flag2 ? Compiler.CaptureAstContext.AssignmentWithResultPreservation : Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, delegate(List<Expression> mergeExprs, List<Expression> mergeFinallyExprs)
					{
						this.AddMergeRedirectionExpressions(commandExpr.Redirections, temps, mergeExprs, mergeFinallyExprs);
					});
				}
			}
			else if (subExpressionAst != null)
			{
				this.AddMergeRedirectionExpressions(commandExpr.Redirections, temps, list, list2);
				list.Add(this.Compile(subExpressionAst.SubExpression));
				if (parameterExpression != null)
				{
					expression = Expression.Call(CachedReflectionInfo.PipelineOps_PipelineResult, parameterExpression);
				}
			}
			else
			{
				this.AddMergeRedirectionExpressions(commandExpr.Redirections, temps, list, list2);
				expression = this.Compile(commandExpr.Expression);
			}
			if (expression != null)
			{
				if (!flag && captureForInput)
				{
					list.Add(expression);
				}
				else
				{
					list.Add(this.CallAddPipe(expression, Compiler._getCurrentPipe));
					list.Add(ExpressionCache.AutomationNullConstant);
				}
			}
			if (parameterExpression2 != null)
			{
				list2.Add(Expression.Assign(Compiler._getCurrentPipe, parameterExpression2));
			}
			if (list2.Count != 0)
			{
				return Expression.Block(temps.ToArray(), new Expression[]
				{
					Expression.TryFinally(Expression.Block(list), Expression.Block(list2))
				});
			}
			return Expression.Block(temps.ToArray(), list);
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x0013D7C8 File Offset: 0x0013B9C8
		private void AddMergeRedirectionExpressions(ReadOnlyCollection<RedirectionAst> redirections, List<ParameterExpression> temps, List<Expression> exprs, List<Expression> finallyExprs)
		{
			foreach (MergingRedirectionAst mergingRedirectionAst in redirections.OfType<MergingRedirectionAst>())
			{
				ParameterExpression parameterExpression = this.NewTemp(typeof(Pipe[]), "savedPipes");
				temps.Add(parameterExpression);
				ConstantExpression constantExpression = Expression.Constant(this.VisitMergingRedirection(mergingRedirectionAst));
				exprs.Add(Expression.Assign(parameterExpression, Expression.Call(constantExpression, CachedReflectionInfo.MergingRedirection_BindForExpression, Compiler._executionContextParameter, Compiler._functionContext)));
				finallyExprs.Insert(0, Expression.Call(constantExpression.Cast(typeof(CommandRedirection)), CachedReflectionInfo.CommandRedirection_UnbindForExpression, Compiler._functionContext, parameterExpression));
			}
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x0013D884 File Offset: 0x0013BA84
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return new MergingRedirection(mergingRedirectionAst.FromStream, mergingRedirectionAst.ToStream);
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x0013D898 File Offset: 0x0013BA98
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			StringConstantExpressionAst stringConstantExpressionAst = fileRedirectionAst.Location as StringConstantExpressionAst;
			Expression expression;
			if (stringConstantExpressionAst != null)
			{
				expression = this.Compile(stringConstantExpressionAst);
			}
			else
			{
				expression = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), this.CompileExpressionOperand(fileRedirectionAst.Location), Compiler._executionContextParameter);
			}
			return Expression.New(CachedReflectionInfo.FileRedirection_ctor, new Expression[]
			{
				Expression.Constant(fileRedirectionAst.FromStream),
				ExpressionCache.Constant(fileRedirectionAst.Append),
				expression
			});
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0013D91C File Offset: 0x0013BB1C
		public object VisitCommand(CommandAst commandAst)
		{
			ReadOnlyCollection<CommandElementAst> commandElements = commandAst.CommandElements;
			Expression[] array = new Expression[commandElements.Count];
			for (int i = 0; i < commandElements.Count; i++)
			{
				CommandElementAst commandElementAst = commandElements[i];
				if (commandElementAst is CommandParameterAst)
				{
					array[i] = this.Compile(commandElementAst);
				}
				else
				{
					CommandElementAst commandElementAst2 = commandElementAst;
					bool b = false;
					UsingExpressionAst usingExpressionAst = commandElementAst as UsingExpressionAst;
					if (usingExpressionAst != null)
					{
						commandElementAst2 = usingExpressionAst.SubExpression;
					}
					VariableExpressionAst variableExpressionAst = commandElementAst2 as VariableExpressionAst;
					if (variableExpressionAst != null)
					{
						b = variableExpressionAst.Splatted;
					}
					bool b2 = this.ArgumentIsNotReallyArrayIfCommandIsNative(commandElementAst);
					array[i] = Expression.Call(CachedReflectionInfo.CommandParameterInternal_CreateArgument, Expression.Constant(commandElementAst.Extent), Expression.Convert(this.GetCommandArgumentExpression(commandElementAst), typeof(object)), ExpressionCache.Constant(b), ExpressionCache.Constant(b2));
				}
			}
			Expression expression = Expression.NewArrayInit(typeof(CommandParameterInternal), array);
			if (commandElements.Count == 2 && commandElements[1] is ParenExpressionAst)
			{
				ExpressionAst pureExpression = ((ParenExpressionAst)commandElements[1]).Pipeline.GetPureExpression();
				if (pureExpression is ArrayLiteralAst && commandElements[0].Extent.EndColumnNumber == commandElements[1].Extent.StartColumnNumber && commandElements[0].Extent.EndLineNumber == commandElements[1].Extent.StartLineNumber)
				{
					expression = Expression.Block(Expression.IfThen(Compiler.IsStrictMode(2, Compiler._executionContextParameter), Compiler.ThrowRuntimeError("StrictModeFunctionCallWithParens", ParserStrings.StrictModeFunctionCallWithParens, new Expression[0])), expression);
				}
			}
			return expression;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x0013DAB0 File Offset: 0x0013BCB0
		private Expression GetCommandArgumentExpression(CommandElementAst element)
		{
			ConstantExpressionAst constantExpressionAst = element as ConstantExpressionAst;
			if (constantExpressionAst != null && LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(constantExpressionAst.StaticType)))
			{
				string text = constantExpressionAst.Extent.Text;
				if (!text.Equals(constantExpressionAst.Value.ToString(), StringComparison.Ordinal))
				{
					return Expression.Constant(ParserOps.WrappedNumber(constantExpressionAst.Value, text));
				}
			}
			Expression expression = this.Compile(element);
			if (expression.Type == typeof(object[]))
			{
				expression = Expression.Call(CachedReflectionInfo.PipelineOps_CheckAutomationNullInCommandArgumentArray, expression);
			}
			else if (constantExpressionAst == null && expression.Type == typeof(object))
			{
				expression = Expression.Call(CachedReflectionInfo.PipelineOps_CheckAutomationNullInCommandArgument, expression);
			}
			return expression;
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0013DB60 File Offset: 0x0013BD60
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			ExpressionAst expression = commandExpressionAst.Expression;
			Expression expression2 = this.Compile(expression);
			UnaryExpressionAst unaryExpressionAst = expression as UnaryExpressionAst;
			if ((unaryExpressionAst != null && unaryExpressionAst.TokenKind.HasTrait(TokenFlags.PrefixOrPostfixOperator)) || expression2.Type == typeof(void))
			{
				return expression2;
			}
			return this.CallAddPipe(expression2, Compiler._getCurrentPipe);
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x0013DBC0 File Offset: 0x0013BDC0
		private bool ArgumentIsNotReallyArrayIfCommandIsNative(Ast arg)
		{
			ArrayLiteralAst arrayLiteralAst = arg as ArrayLiteralAst;
			if (arrayLiteralAst == null)
			{
				return false;
			}
			ExpressionAst expressionAst = arrayLiteralAst.Elements[0];
			for (int i = 1; i < arrayLiteralAst.Elements.Count; i++)
			{
				ExpressionAst expressionAst2 = arrayLiteralAst.Elements[i];
				if (expressionAst.Extent.EndOffset + 1 != expressionAst2.Extent.StartOffset)
				{
					return false;
				}
				expressionAst = expressionAst2;
			}
			return true;
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0013DC28 File Offset: 0x0013BE28
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			ExpressionAst argument = commandParameterAst.Argument;
			IScriptExtent errorPosition = commandParameterAst.ErrorPosition;
			if (argument != null)
			{
				bool b = errorPosition.EndLineNumber != argument.Extent.StartLineNumber || errorPosition.EndColumnNumber != argument.Extent.StartColumnNumber;
				bool b2 = this.ArgumentIsNotReallyArrayIfCommandIsNative(argument);
				return Expression.Call(CachedReflectionInfo.CommandParameterInternal_CreateParameterWithArgument, new Expression[]
				{
					Expression.Constant(errorPosition),
					Expression.Constant(commandParameterAst.ParameterName),
					Expression.Constant(errorPosition.Text),
					Expression.Constant(argument.Extent),
					Expression.Convert(this.GetCommandArgumentExpression(argument), typeof(object)),
					ExpressionCache.Constant(b),
					ExpressionCache.Constant(b2)
				});
			}
			return Expression.Call(CachedReflectionInfo.CommandParameterInternal_CreateParameter, Expression.Constant(errorPosition), Expression.Constant(commandParameterAst.ParameterName), Expression.Constant(errorPosition.Text));
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0013DD1E File Offset: 0x0013BF1E
		internal static Expression ThrowRuntimeError(string errorID, string resourceString, params Expression[] exceptionArgs)
		{
			return Compiler.ThrowRuntimeError(errorID, resourceString, typeof(object), exceptionArgs);
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x0013DD32 File Offset: 0x0013BF32
		internal static Expression ThrowRuntimeError(string errorID, string resourceString, Type throwResultType, params Expression[] exceptionArgs)
		{
			return Compiler.ThrowRuntimeError(typeof(RuntimeException), errorID, resourceString, throwResultType, exceptionArgs);
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0013DD5C File Offset: 0x0013BF5C
		internal static Expression ThrowRuntimeError(Type exceptionType, string errorID, string resourceString, Type throwResultType, params Expression[] exceptionArgs)
		{
			Expression expression;
			if (exceptionArgs == null)
			{
				expression = ExpressionCache.NullConstant;
			}
			else
			{
				expression = Expression.NewArrayInit(typeof(object), from e in exceptionArgs
				select e.Cast(typeof(object)));
			}
			Expression expression2 = expression;
			Expression[] arguments = new Expression[]
			{
				ExpressionCache.NullConstant,
				Expression.Constant(exceptionType, typeof(Type)),
				ExpressionCache.NullExtent,
				Expression.Constant(errorID),
				Expression.Constant(resourceString),
				expression2
			};
			return Expression.Throw(Expression.Call(CachedReflectionInfo.InterpreterError_NewInterpreterException, arguments), throwResultType);
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x0013DDFB File Offset: 0x0013BFFB
		internal static Expression ThrowRuntimeErrorWithInnerException(string errorID, string resourceString, Expression innerException, params Expression[] exceptionArgs)
		{
			return Compiler.ThrowRuntimeErrorWithInnerException(errorID, Expression.Constant(resourceString), innerException, typeof(object), exceptionArgs);
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x0013DE18 File Offset: 0x0013C018
		internal static Expression ThrowRuntimeErrorWithInnerException(string errorID, Expression resourceString, Expression innerException, Type throwResultType, params Expression[] exceptionArgs)
		{
			Expression expression = (exceptionArgs != null) ? Expression.NewArrayInit(typeof(object), exceptionArgs) : ExpressionCache.NullConstant;
			Expression[] arguments = new Expression[]
			{
				ExpressionCache.NullConstant,
				Expression.Constant(typeof(RuntimeException), typeof(Type)),
				ExpressionCache.NullExtent,
				Expression.Constant(errorID),
				resourceString,
				innerException,
				expression
			};
			return Expression.Throw(Expression.Call(CachedReflectionInfo.InterpreterError_NewInterpreterExceptionWithInnerException, arguments), throwResultType);
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x0013DEA0 File Offset: 0x0013C0A0
		internal static Expression CreateThrow(Type resultType, Type exception, Type[] exceptionArgTypes, params object[] exceptionArgs)
		{
			Expression[] array = new Expression[exceptionArgs.Length];
			for (int i = 0; i < exceptionArgs.Length; i++)
			{
				object value = exceptionArgs[i];
				Expression expression = Expression.Constant(value, exceptionArgTypes[i]);
				array[i] = expression;
			}
			ConstructorInfo constructor = exception.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, exceptionArgTypes, null);
			if (constructor == null)
			{
				throw new PSArgumentException("Type doesn't have constructor with a given signature");
			}
			return Expression.Throw(Expression.New(constructor, array), resultType);
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x0013DF08 File Offset: 0x0013C108
		internal static Expression CreateThrow(Type resultType, Type exception, params object[] exceptionArgs)
		{
			Type[] array = PSTypeExtensions.EmptyTypes;
			if (exceptionArgs != null)
			{
				array = new Type[exceptionArgs.Length];
				for (int i = 0; i < exceptionArgs.Length; i++)
				{
					array[i] = exceptionArgs[i].GetType();
				}
			}
			return Compiler.CreateThrow(resultType, exception, array, exceptionArgs);
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x0013DF94 File Offset: 0x0013C194
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			Compiler.AutomaticVarSaver automaticVarSaver = new Compiler.AutomaticVarSaver(this, SpecialVariables.UnderbarVarPath, 0);
			List<ParameterExpression> list = new List<ParameterExpression>();
			ParameterExpression parameterExpression = null;
			if (switchStatementAst.Default != null)
			{
				parameterExpression = this.NewTemp(typeof(bool), "skipDefault");
				list.Add(parameterExpression);
			}
			Action<List<Expression>, Expression> switchBodyGenerator = this.GetSwitchBodyGenerator(switchStatementAst, automaticVarSaver, parameterExpression);
			if ((switchStatementAst.Flags & SwitchFlags.File) != SwitchFlags.None)
			{
				List<Expression> list2 = new List<Expression>();
				ParameterExpression parameterExpression2 = this.NewTemp(typeof(string), "path");
				list.Add(parameterExpression2);
				list2.Add(this.UpdatePosition(switchStatementAst.Condition));
				DynamicExpression arg = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), this.CaptureStatementResults(switchStatementAst.Condition, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null), Compiler._executionContextParameter);
				list2.Add(Expression.Assign(parameterExpression2, Expression.Call(CachedReflectionInfo.SwitchOps_ResolveFilePath, Expression.Constant(switchStatementAst.Condition.Extent), arg, Compiler._executionContextParameter)));
				List<Expression> list3 = new List<Expression>();
				ParameterExpression parameterExpression3 = this.NewTemp(typeof(StreamReader), "streamReader");
				ParameterExpression line = this.NewTemp(typeof(string), "line");
				list.Add(parameterExpression3);
				list.Add(line);
				list3.Add(Expression.Assign(parameterExpression3, Expression.New(CachedReflectionInfo.StreamReader_ctor, new Expression[]
				{
					parameterExpression2
				})));
				BinaryExpression loopTest = Expression.NotEqual(Expression.Assign(line, Expression.Call(parameterExpression3, CachedReflectionInfo.StreamReader_ReadLine)).Cast(typeof(object)), ExpressionCache.NullConstant);
				list3.Add(automaticVarSaver.SaveAutomaticVar());
				list3.Add(this.GenerateWhileLoop(switchStatementAst.Label, () => loopTest, delegate(List<Expression> loopBody, LabelTarget breakTarget, LabelTarget continueTarget)
				{
					switchBodyGenerator(loopBody, line);
				}, null));
				BlockExpression blockExpression = Expression.Block(list3);
				BlockExpression @finally = Expression.Block(Expression.IfThen(Expression.NotEqual(parameterExpression3, ExpressionCache.NullConstant), Expression.Call(parameterExpression3.Cast(typeof(IDisposable)), CachedReflectionInfo.IDisposable_Dispose)), automaticVarSaver.RestoreAutomaticVar());
				ParameterExpression parameterExpression4 = this.NewTemp(typeof(Exception), "exception");
				BlockExpression body = Expression.Block(blockExpression.Type, new Expression[]
				{
					Expression.Call(CachedReflectionInfo.CommandProcessorBase_CheckForSevereException, parameterExpression4),
					Compiler.ThrowRuntimeErrorWithInnerException("FileReadError", ParserStrings.FileReadError, parameterExpression4, new Expression[]
					{
						Expression.Property(parameterExpression4, CachedReflectionInfo.Exception_Message)
					})
				});
				list2.Add(Expression.TryCatchFinally(blockExpression, @finally, new CatchBlock[]
				{
					Expression.Catch(typeof(FlowControlException), Expression.Rethrow(blockExpression.Type)),
					Expression.Catch(parameterExpression4, body)
				}));
				return Expression.Block(list.Concat(automaticVarSaver.GetTemps()), list2);
			}
			TryExpression tryExpression = Expression.TryFinally(Expression.Block(automaticVarSaver.SaveAutomaticVar(), this.GenerateIteratorStatement(SpecialVariables.switchVarPath, () => this.UpdatePosition(switchStatementAst.Condition), this._switchTupleIndex, switchStatementAst, switchBodyGenerator)), automaticVarSaver.RestoreAutomaticVar());
			return Expression.Block(list.Concat(automaticVarSaver.GetTemps()), new Expression[]
			{
				tryExpression
			});
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x0013E6A4 File Offset: 0x0013C8A4
		private Action<List<Expression>, Expression> GetSwitchBodyGenerator(SwitchStatementAst switchStatementAst, Compiler.AutomaticVarSaver avs, ParameterExpression skipDefault)
		{
			return delegate(List<Expression> exprs, Expression newValue)
			{
				PSSwitchClauseEvalBinder binder = PSSwitchClauseEvalBinder.Get(switchStatementAst.Flags);
				exprs.Add(avs.SetNewValue(newValue));
				if (skipDefault != null)
				{
					exprs.Add(Expression.Assign(skipDefault, ExpressionCache.Constant(false)));
				}
				IsConstantValueVisitor visitor = new IsConstantValueVisitor();
				ConstantValueVisitor visitor2 = new ConstantValueVisitor();
				int count = switchStatementAst.Clauses.Count;
				for (int i = 0; i < count; i++)
				{
					Tuple<ExpressionAst, StatementBlockAst> tuple = switchStatementAst.Clauses[i];
					object obj = ((bool)tuple.Item1.Accept(visitor)) ? tuple.Item1.Accept(visitor2) : null;
					Expression test;
					if (obj is ScriptBlock)
					{
						MethodCallExpression arg = Expression.Call(Expression.Constant(obj), CachedReflectionInfo.ScriptBlock_DoInvokeReturnAsIs, new Expression[]
						{
							ExpressionCache.Constant(true),
							Expression.Constant(ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe),
							this.GetLocal(0).Convert(typeof(object)),
							ExpressionCache.AutomationNullConstant,
							ExpressionCache.AutomationNullConstant,
							ExpressionCache.NullObjectArray
						});
						test = DynamicExpression.Dynamic(PSConvertBinder.Get(typeof(bool)), typeof(bool), arg);
					}
					else if (obj != null)
					{
						SwitchFlags flags = switchStatementAst.Flags;
						Expression expression = (obj is Regex || obj is WildcardPattern) ? Expression.Constant(obj) : DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), (obj is Type) ? Expression.Constant(obj, typeof(Type)) : Expression.Constant(obj), Compiler._executionContextParameter);
						Expression expression2 = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), this.GetLocal(0), Compiler._executionContextParameter);
						if ((flags & SwitchFlags.Regex) != SwitchFlags.None || obj is Regex)
						{
							test = Expression.Call(CachedReflectionInfo.SwitchOps_ConditionSatisfiedRegex, ExpressionCache.Constant((flags & SwitchFlags.CaseSensitive) != SwitchFlags.None), expression, Expression.Constant(tuple.Item1.Extent), expression2, Compiler._executionContextParameter);
						}
						else if ((flags & SwitchFlags.Wildcard) != SwitchFlags.None || obj is WildcardPattern)
						{
							test = Expression.Call(CachedReflectionInfo.SwitchOps_ConditionSatisfiedWildcard, ExpressionCache.Constant((flags & SwitchFlags.CaseSensitive) != SwitchFlags.None), expression, expression2, Compiler._executionContextParameter);
						}
						else
						{
							test = Compiler.CallStringEquals(expression, expression2, (flags & SwitchFlags.CaseSensitive) == SwitchFlags.None);
						}
					}
					else
					{
						Expression arg2 = this.Compile(tuple.Item1);
						test = DynamicExpression.Dynamic(binder, typeof(bool), arg2, this.GetLocal(0), Compiler._executionContextParameter);
					}
					exprs.Add(this.UpdatePosition(tuple.Item1));
					if (skipDefault != null)
					{
						exprs.Add(Expression.IfThen(test, Expression.Block(this.Compile(tuple.Item2), Expression.Assign(skipDefault, ExpressionCache.Constant(true)))));
					}
					else
					{
						exprs.Add(Expression.IfThen(test, this.Compile(tuple.Item2)));
					}
				}
				if (skipDefault != null)
				{
					exprs.Add(Expression.IfThen(Expression.Not(skipDefault), this.Compile(switchStatementAst.Default)));
				}
			};
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0013E6F8 File Offset: 0x0013C8F8
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			ParameterExpression parameterExpression = this.NewTemp(typeof(PSLanguageMode), "oldLanguageMode");
			MemberExpression memberExpression = Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_LanguageMode);
			List<Expression> list = new List<Expression>
			{
				Expression.Assign(parameterExpression, memberExpression),
				this.UpdatePosition(dataStatementAst),
				Expression.Call(CachedReflectionInfo.RestrictedLanguageChecker_EnsureUtilityModuleLoaded, Compiler._executionContextParameter)
			};
			if (dataStatementAst.CommandsAllowed.Count > 0)
			{
				list.Add(Expression.Call(CachedReflectionInfo.RestrictedLanguageChecker_CheckDataStatementLanguageModeAtRuntime, Expression.Constant(dataStatementAst), Compiler._executionContextParameter));
			}
			if (dataStatementAst.HasNonConstantAllowedCommand)
			{
				list.Add(Expression.Call(CachedReflectionInfo.RestrictedLanguageChecker_CheckDataStatementAstAtRuntime, Expression.Constant(dataStatementAst), Expression.NewArrayInit(typeof(string), from elem in dataStatementAst.CommandsAllowed
				select this.Compile(elem).Convert(typeof(string)))));
			}
			list.Add(Expression.Assign(memberExpression, Expression.Constant(PSLanguageMode.RestrictedLanguage)));
			Expression item = (dataStatementAst.Variable != null) ? this.CaptureAstResults(dataStatementAst.Body, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null).Cast(typeof(object)) : this.Compile(dataStatementAst.Body);
			list.Add(item);
			BlockExpression blockExpression = Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.TryFinally(Expression.Block(list), Expression.Assign(memberExpression, parameterExpression))
			});
			if (dataStatementAst.Variable == null)
			{
				return blockExpression;
			}
			if (dataStatementAst.TupleIndex >= 0)
			{
				return Expression.Assign(this.GetLocal(dataStatementAst.TupleIndex), blockExpression);
			}
			return Compiler.CallSetVariable(Expression.Constant(new VariablePath("local:" + dataStatementAst.Variable)), blockExpression, null);
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x0013E8B4 File Offset: 0x0013CAB4
		private static CatchBlock[] GenerateLoopBreakContinueCatchBlocks(string label, LabelTarget breakLabel, LabelTarget continueLabel)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(BreakException));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(ContinueException));
			return new CatchBlock[]
			{
				Expression.Catch(parameterExpression, Expression.IfThenElse(Expression.Call(parameterExpression, CachedReflectionInfo.LoopFlowException_MatchLabel, new Expression[]
				{
					Expression.Constant(label ?? "", typeof(string))
				}), Expression.Break(breakLabel), Expression.Rethrow())),
				Expression.Catch(parameterExpression2, Expression.IfThenElse(Expression.Call(parameterExpression2, CachedReflectionInfo.LoopFlowException_MatchLabel, new Expression[]
				{
					Expression.Constant(label ?? "", typeof(string))
				}), Expression.Continue(continueLabel), Expression.Rethrow()))
			};
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0013E97C File Offset: 0x0013CB7C
		private Expression GenerateWhileLoop(string loopLabel, Func<Expression> generateCondition, Action<List<Expression>, LabelTarget, LabelTarget> generateLoopBody, PipelineBaseAst continueAst = null)
		{
			int stmtCount = this._stmtCount;
			List<Expression> list = new List<Expression>();
			LabelTarget labelTarget = Expression.Label((!string.IsNullOrEmpty(loopLabel)) ? (loopLabel + "Continue") : "continue");
			LabelTarget labelTarget2 = Expression.Label((!string.IsNullOrEmpty(loopLabel)) ? (loopLabel + "Break") : "break");
			EnterLoopExpression enterLoopExpression = new EnterLoopExpression();
			LabelTarget target = (continueAst != null) ? Expression.Label((!string.IsNullOrEmpty(loopLabel)) ? (loopLabel + "LoopTop") : "looptop") : labelTarget;
			list.Add(Expression.Label(target));
			list.Add(enterLoopExpression);
			List<Expression> list2 = new List<Expression>();
			list2.Add(Compiler._callCheckForInterrupts);
			this._loopTargets.Add(new Compiler.LoopGotoTargets(loopLabel ?? "", labelTarget2, labelTarget));
			this._generatingWhileOrDoLoop = true;
			generateLoopBody(list2, labelTarget2, labelTarget);
			this._generatingWhileOrDoLoop = false;
			if (continueAst == null)
			{
				list2.Add(Expression.Goto(target));
			}
			this._loopTargets.RemoveAt(this._loopTargets.Count - 1);
			Expression expression = Expression.TryCatch(Expression.Block(list2), Compiler.GenerateLoopBreakContinueCatchBlocks(loopLabel, labelTarget2, labelTarget));
			if (continueAst != null)
			{
				List<Expression> list3 = new List<Expression>();
				list3.Add(expression);
				list3.Add(Expression.Label(labelTarget));
				if (continueAst.GetPureExpression() != null)
				{
					list3.Add(this.UpdatePosition(continueAst));
				}
				list3.Add(this.CaptureStatementResults(continueAst, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null));
				list3.Add(Expression.Goto(target));
				expression = Expression.Block(list3);
			}
			if (generateCondition != null)
			{
				list.Add(Expression.IfThen(generateCondition().Convert(typeof(bool)), expression));
			}
			else
			{
				list.Add(expression);
			}
			list.Add(Expression.Label(labelTarget2));
			enterLoopExpression.LoopStatementCount = this._stmtCount - stmtCount;
			return enterLoopExpression.Loop = new PowerShellLoopExpression(list);
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0013EB60 File Offset: 0x0013CD60
		private Expression GenerateDoLoop(LoopStatementAst loopStatement)
		{
			int stmtCount = this._stmtCount;
			string label = loopStatement.Label;
			List<Expression> list = new List<Expression>();
			LabelTarget target = Expression.Label((!string.IsNullOrEmpty(label)) ? label : null);
			LabelTarget labelTarget = Expression.Label((!string.IsNullOrEmpty(label)) ? (label + "Continue") : "continue");
			LabelTarget labelTarget2 = Expression.Label((!string.IsNullOrEmpty(label)) ? (label + "Break") : "break");
			EnterLoopExpression enterLoopExpression = new EnterLoopExpression();
			list.Add(Expression.Label(target));
			list.Add(enterLoopExpression);
			this._loopTargets.Add(new Compiler.LoopGotoTargets(label ?? "", labelTarget2, labelTarget));
			this._generatingWhileOrDoLoop = true;
			List<Expression> expressions = new List<Expression>
			{
				Compiler._callCheckForInterrupts,
				this.Compile(loopStatement.Body),
				ExpressionCache.Empty
			};
			this._generatingWhileOrDoLoop = false;
			this._loopTargets.RemoveAt(this._loopTargets.Count - 1);
			list.Add(Expression.TryCatch(Expression.Block(expressions), Compiler.GenerateLoopBreakContinueCatchBlocks(label, labelTarget2, labelTarget)));
			list.Add(Expression.Label(labelTarget));
			Expression expression = this.CaptureStatementResults(loopStatement.Condition, Compiler.CaptureAstContext.Condition, null).Convert(typeof(bool));
			if (loopStatement is DoUntilStatementAst)
			{
				expression = Expression.Not(expression);
			}
			list.Add(Expression.IfThen(expression, Expression.Goto(target)));
			list.Add(Expression.Label(labelTarget2));
			enterLoopExpression.LoopStatementCount = this._stmtCount - stmtCount;
			return enterLoopExpression.Loop = new PowerShellLoopExpression(list);
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0013ED34 File Offset: 0x0013CF34
		private Expression GenerateIteratorStatement(VariablePath iteratorVariablePath, Func<Expression> generateMoveNextUpdatePosition, int iteratorTupleIndex, LabeledStatementAst stmt, Action<List<Expression>, Expression> generateBody)
		{
			List<ParameterExpression> list = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			Compiler.AutomaticVarSaver automaticVarSaver = new Compiler.AutomaticVarSaver(this, iteratorVariablePath, iteratorTupleIndex);
			bool flag = stmt is ForEachStatementAst;
			list2.Add(automaticVarSaver.SaveAutomaticVar());
			ParameterExpression parameterExpression = this.NewTemp(typeof(object), "enumerable");
			list.Add(parameterExpression);
			if (flag)
			{
				list2.Add(this.UpdatePosition(stmt.Condition));
			}
			list2.Add(Expression.Assign(parameterExpression, this.GetRangeEnumerator(stmt.Condition.GetPureExpression()) ?? this.CaptureStatementResults(stmt.Condition, Compiler.CaptureAstContext.Enumerable, null).Convert(typeof(object))));
			ParameterExpression iteratorTemp = this.NewTemp(typeof(IEnumerator), iteratorVariablePath.UnqualifiedPath);
			list.Add(iteratorTemp);
			list2.Add(Expression.Assign(iteratorTemp, DynamicExpression.Dynamic(PSEnumerableBinder.Get(), typeof(IEnumerator), parameterExpression)));
			BinaryExpression test = flag ? Expression.AndAlso(Expression.Equal(iteratorTemp, ExpressionCache.NullConstant), Expression.NotEqual(parameterExpression, ExpressionCache.NullConstant)) : Expression.Equal(iteratorTemp, ExpressionCache.NullConstant);
			BinaryExpression ifTrue = Expression.Assign(iteratorTemp, Expression.Call(Expression.NewArrayInit(typeof(object), new Expression[]
			{
				Expression.Convert(parameterExpression, typeof(object))
			}), CachedReflectionInfo.IEnumerable_GetEnumerator));
			list2.Add(Expression.IfThen(test, ifTrue));
			list2.Add(automaticVarSaver.SetNewValue(iteratorTemp));
			BlockExpression moveNext = Expression.Block(generateMoveNextUpdatePosition(), Expression.Call(iteratorTemp, CachedReflectionInfo.IEnumerator_MoveNext));
			Expression expression = this.GenerateWhileLoop(stmt.Label, () => moveNext, delegate(List<Expression> loopBody, LabelTarget breakTarget, LabelTarget continueTarget)
			{
				generateBody(loopBody, Expression.Property(iteratorTemp, CachedReflectionInfo.IEnumerator_Current));
			}, null);
			if (flag)
			{
				list2.Add(Expression.IfThen(Expression.NotEqual(iteratorTemp, ExpressionCache.NullConstant), expression));
			}
			else
			{
				list2.Add(expression);
			}
			return Expression.Block(list.Concat(automaticVarSaver.GetTemps()), new Expression[]
			{
				Expression.TryFinally(Expression.Block(list2), automaticVarSaver.RestoreAutomaticVar())
			});
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0013EFEC File Offset: 0x0013D1EC
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			Action<List<Expression>, Expression> generateBody = delegate(List<Expression> exprs, Expression newValue)
			{
				exprs.Add(this.ReduceAssignment(forEachStatementAst.Variable, TokenKind.Equals, newValue));
				exprs.Add(this.Compile(forEachStatementAst.Body));
			};
			return this.GenerateIteratorStatement(SpecialVariables.foreachVarPath, () => this.UpdatePosition(forEachStatementAst.Variable), this._foreachTupleIndex, forEachStatementAst, generateBody);
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0013F040 File Offset: 0x0013D240
		private Expression GetRangeEnumerator(ExpressionAst condExpr)
		{
			Expression result = null;
			if (condExpr != null)
			{
				BinaryExpressionAst binaryExpressionAst = condExpr as BinaryExpressionAst;
				if (binaryExpressionAst != null && binaryExpressionAst.Operator == TokenKind.DotDot)
				{
					Expression expr = this.Compile(binaryExpressionAst.Left);
					Expression expr2 = this.Compile(binaryExpressionAst.Right);
					result = Expression.New(CachedReflectionInfo.RangeEnumerator_ctor, new Expression[]
					{
						expr.Convert(typeof(int)),
						expr2.Convert(typeof(int))
					});
				}
			}
			return result;
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x0013F0BE File Offset: 0x0013D2BE
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			return this.GenerateDoLoop(doWhileStatementAst);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x0013F0C7 File Offset: 0x0013D2C7
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			return this.GenerateDoLoop(doUntilStatementAst);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x0013F12C File Offset: 0x0013D32C
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			Expression expression = (forStatementAst.Initializer != null) ? this.CaptureStatementResults(forStatementAst.Initializer, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null) : null;
			Func<Expression> generateCondition = (forStatementAst.Condition != null) ? (() => Expression.Block(this.UpdatePosition(forStatementAst.Condition), this.CaptureStatementResults(forStatementAst.Condition, Compiler.CaptureAstContext.Condition, null))) : null;
			Expression expression2 = this.GenerateWhileLoop(forStatementAst.Label, generateCondition, delegate(List<Expression> loopBody, LabelTarget breakTarget, LabelTarget continueTarget)
			{
				loopBody.Add(this.Compile(forStatementAst.Body));
			}, forStatementAst.Iterator);
			if (expression != null)
			{
				return Expression.Block(expression, expression2);
			}
			return expression2;
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0013F220 File Offset: 0x0013D420
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			return this.GenerateWhileLoop(whileStatementAst.Label, () => Expression.Block(this.UpdatePosition(whileStatementAst.Condition), this.CaptureStatementResults(whileStatementAst.Condition, Compiler.CaptureAstContext.Condition, null)), delegate(List<Expression> loopBody, LabelTarget breakTarget, LabelTarget continueTarget)
			{
				loopBody.Add(this.Compile(whileStatementAst.Body));
			}, null);
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0013F26B File Offset: 0x0013D46B
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			return null;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0013F270 File Offset: 0x0013D470
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			List<ParameterExpression> list = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			List<Expression> list3 = new List<Expression>();
			ParameterExpression parameterExpression = this.NewTemp(typeof(bool), "oldActiveHandler");
			list.Add(parameterExpression);
			MemberExpression memberExpression = Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_ExceptionHandlerInEnclosingStatementBlock);
			list2.Add(Expression.Assign(parameterExpression, memberExpression));
			list2.Add(Expression.Assign(memberExpression, ExpressionCache.Constant(true)));
			list3.Add(Expression.Assign(memberExpression, parameterExpression));
			this.CompileStatementListWithTraps(tryStatementAst.Body.Statements, tryStatementAst.Body.Traps, list2, list);
			List<CatchBlock> list4 = new List<CatchBlock>();
			if (tryStatementAst.CatchClauses.Count == 1 && tryStatementAst.CatchClauses[0].IsCatchAll)
			{
				Compiler.AutomaticVarSaver automaticVarSaver = new Compiler.AutomaticVarSaver(this, SpecialVariables.UnderbarVarPath, 0);
				ParameterExpression parameterExpression2 = this.NewTemp(typeof(RuntimeException), "rte");
				ParameterExpression parameterExpression3 = this.NewTemp(typeof(RuntimeException), "oldrte");
				NewExpression newValue = Expression.New(CachedReflectionInfo.ErrorRecord__ctor, new Expression[]
				{
					Expression.Property(parameterExpression2, CachedReflectionInfo.RuntimeException_ErrorRecord),
					parameterExpression2
				});
				List<Expression> list5 = new List<Expression>
				{
					Expression.Assign(parameterExpression3, Compiler._currentExceptionBeingHandled),
					Expression.Assign(Compiler._currentExceptionBeingHandled, parameterExpression2),
					automaticVarSaver.SaveAutomaticVar(),
					automaticVarSaver.SetNewValue(newValue)
				};
				StatementBlockAst body = tryStatementAst.CatchClauses[0].Body;
				this.CompileStatementListWithTraps(body.Statements, body.Traps, list5, list);
				TryExpression tryExpression = Expression.TryFinally(Expression.Block(typeof(void), list5), Expression.Block(typeof(void), new Expression[]
				{
					automaticVarSaver.RestoreAutomaticVar(),
					Expression.Assign(Compiler._currentExceptionBeingHandled, parameterExpression3)
				}));
				list4.Add(Expression.Catch(typeof(PipelineStoppedException), Expression.Rethrow()));
				list4.Add(Expression.Catch(parameterExpression2, Expression.Block(automaticVarSaver.GetTemps().Append(parameterExpression3).ToArray<ParameterExpression>(), new Expression[]
				{
					tryExpression
				})));
			}
			else if (tryStatementAst.CatchClauses.Count > 0)
			{
				int num = 0;
				for (int i = 0; i < tryStatementAst.CatchClauses.Count; i++)
				{
					CatchClauseAst catchClauseAst = tryStatementAst.CatchClauses[i];
					num += Math.Max(catchClauseAst.CatchTypes.Count, 1);
				}
				Type[] array = new Type[num];
				Expression expression = Expression.Constant(array);
				List<Expression> list6 = new List<Expression>();
				List<SwitchCase> list7 = new List<SwitchCase>();
				int num2 = 0;
				int num3 = 0;
				ParameterExpression parameterExpression4 = Expression.Parameter(typeof(RuntimeException));
				for (int j = 0; j < tryStatementAst.CatchClauses.Count; j++)
				{
					CatchClauseAst catchClauseAst2 = tryStatementAst.CatchClauses[j];
					if (catchClauseAst2.IsCatchAll)
					{
						array[num3] = typeof(ExceptionHandlingOps.CatchAll);
					}
					else
					{
						for (int k = 0; k < catchClauseAst2.CatchTypes.Count; k++)
						{
							TypeConstraintAst typeConstraintAst = catchClauseAst2.CatchTypes[k];
							array[num3] = typeConstraintAst.TypeName.GetReflectionType();
							if (array[num3] == null)
							{
								IndexExpression left = Expression.ArrayAccess(expression, new Expression[]
								{
									ExpressionCache.Constant(num3)
								});
								list6.Add(Expression.IfThen(Expression.Equal(left, ExpressionCache.NullType), Expression.Assign(left, Expression.Call(CachedReflectionInfo.TypeOps_ResolveTypeName, Expression.Constant(typeConstraintAst.TypeName)))));
							}
							num3++;
						}
					}
					BlockExpression body2 = Expression.Block(typeof(void), new Expression[]
					{
						this.Compile(catchClauseAst2.Body)
					});
					if (catchClauseAst2.IsCatchAll)
					{
						list7.Add(Expression.SwitchCase(body2, new Expression[]
						{
							ExpressionCache.Constant(num2)
						}));
						num2++;
					}
					else
					{
						list7.Add(Expression.SwitchCase(body2, Enumerable.Range(num2, num2 + catchClauseAst2.CatchTypes.Count).Select(new Func<int, Expression>(ExpressionCache.Constant))));
						num2 += catchClauseAst2.CatchTypes.Count;
					}
				}
				if (list6.Count > 0)
				{
					expression = Expression.Block(list6.Append(expression));
				}
				Compiler.AutomaticVarSaver automaticVarSaver2 = new Compiler.AutomaticVarSaver(this, SpecialVariables.UnderbarVarPath, 0);
				MethodCallExpression switchValue = Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_FindMatchingHandler, this.LocalVariablesParameter, parameterExpression4, expression, Compiler._executionContextParameter);
				ParameterExpression parameterExpression5 = this.NewTemp(typeof(RuntimeException), "oldrte");
				TryExpression tryExpression2 = Expression.TryFinally(Expression.Block(typeof(void), new Expression[]
				{
					Expression.Assign(parameterExpression5, Compiler._currentExceptionBeingHandled),
					Expression.Assign(Compiler._currentExceptionBeingHandled, parameterExpression4),
					automaticVarSaver2.SaveAutomaticVar(),
					Expression.Switch(switchValue, Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_CheckActionPreference, Compiler._functionContext, parameterExpression4), list7.ToArray())
				}), Expression.Block(automaticVarSaver2.RestoreAutomaticVar(), Expression.Assign(Compiler._currentExceptionBeingHandled, parameterExpression5)));
				list4.Add(Expression.Catch(typeof(PipelineStoppedException), Expression.Rethrow()));
				list4.Add(Expression.Catch(parameterExpression4, Expression.Block(automaticVarSaver2.GetTemps().Append(parameterExpression5).ToArray<ParameterExpression>(), new Expression[]
				{
					tryExpression2
				})));
			}
			if (tryStatementAst.Finally != null)
			{
				ParameterExpression parameterExpression6 = this.NewTemp(typeof(bool), "oldIsStopping");
				list.Add(parameterExpression6);
				list3.Add(Expression.Assign(parameterExpression6, Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_SuspendStoppingPipeline, Compiler._executionContextParameter)));
				List<Expression> list8 = new List<Expression>();
				this.CompileStatementListWithTraps(tryStatementAst.Finally.Statements, tryStatementAst.Finally.Traps, list8, list);
				if (list8.Count == 0)
				{
					list8.Add(ExpressionCache.Empty);
				}
				list3.Add(Expression.Block(new Expression[]
				{
					Expression.TryFinally(Expression.Block(list8), Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_RestoreStoppingPipeline, Compiler._executionContextParameter, parameterExpression6))
				}));
			}
			if (list2[list2.Count - 1].Type != typeof(void))
			{
				list2.Add(ExpressionCache.Empty);
			}
			if (list4.Count > 0)
			{
				return Expression.Block(list.ToArray(), new Expression[]
				{
					Expression.TryCatchFinally(Expression.Block(list2), Expression.Block(list3), list4.ToArray())
				});
			}
			return Expression.Block(list.ToArray(), new Expression[]
			{
				Expression.TryFinally(Expression.Block(list2), Expression.Block(list3))
			});
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0013F994 File Offset: 0x0013DB94
		private Expression GenerateBreakOrContinue(Ast ast, ExpressionAst label, Func<Compiler.LoopGotoTargets, LabelTarget> fieldSelector, Func<LabelTarget, Expression> exprGenerator, ConstructorInfo nonLocalExceptionCtor)
		{
			LabelTarget labelTarget = null;
			Expression expression = null;
			if (label != null)
			{
				expression = this.Compile(label);
				if (this._loopTargets.Count > 0)
				{
					StringConstantExpressionAst labelStrAst = label as StringConstantExpressionAst;
					if (labelStrAst != null)
					{
						labelTarget = (from t in this._loopTargets
						where t.Label.Equals(labelStrAst.Value, StringComparison.OrdinalIgnoreCase)
						select fieldSelector(t)).LastOrDefault<LabelTarget>();
					}
				}
			}
			else if (this._loopTargets.Count > 0)
			{
				labelTarget = fieldSelector(this._loopTargets[this._loopTargets.Count - 1]);
			}
			Expression arg;
			if (labelTarget != null)
			{
				arg = exprGenerator(labelTarget);
			}
			else
			{
				expression = (expression ?? ExpressionCache.ConstEmptyString);
				arg = Expression.Throw(Expression.New(nonLocalExceptionCtor, new Expression[]
				{
					expression.Convert(typeof(string))
				}));
			}
			return Expression.Block(this.UpdatePosition(ast), arg);
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x0013FAC4 File Offset: 0x0013DCC4
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			return this.GenerateBreakOrContinue(breakStatementAst, breakStatementAst.Label, (Compiler.LoopGotoTargets lgt) => lgt.BreakLabel, new Func<LabelTarget, Expression>(Expression.Break), CachedReflectionInfo.BreakException_ctor);
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x0013FB09 File Offset: 0x0013DD09
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			return this.GenerateBreakOrContinue(continueStatementAst, continueStatementAst.Label, (Compiler.LoopGotoTargets lgt) => lgt.ContinueLabel, new Func<LabelTarget, Expression>(Expression.Continue), CachedReflectionInfo.ContinueException_ctor);
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x0013FB48 File Offset: 0x0013DD48
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			Expression expression;
			if (this._compilingTrap)
			{
				expression = Expression.Throw(Expression.New(CachedReflectionInfo.ReturnException_ctor, new Expression[]
				{
					ExpressionCache.AutomationNullConstant
				}));
			}
			else
			{
				expression = Expression.Return(this._returnTarget, (this._returnTarget.Type == typeof(object)) ? ExpressionCache.AutomationNullConstant : ExpressionCache.Empty);
			}
			if (returnStatementAst.Pipeline == null)
			{
				return expression;
			}
			PipelineBaseAst pipeline = returnStatementAst.Pipeline;
			AssignmentStatementAst assignmentStatementAst = pipeline as AssignmentStatementAst;
			Expression expression2;
			if (this.CompilingMemberFunction)
			{
				expression2 = this.CaptureStatementResults(returnStatementAst.Pipeline, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null);
				if (this.MemberFunctionReturnType != typeof(void))
				{
					expression2 = Expression.Call(Compiler._returnPipe, CachedReflectionInfo.Pipe_Add, new Expression[]
					{
						expression2.Convert(this.MemberFunctionReturnType).Cast(typeof(object))
					});
				}
				return Expression.Block(this.UpdatePosition(returnStatementAst.Pipeline), Expression.Assign(Compiler._getCurrentPipe, Compiler._returnPipe), expression2, expression);
			}
			expression2 = ((assignmentStatementAst != null) ? this.CallAddPipe(this.CompileAssignment(assignmentStatementAst, null), Compiler._getCurrentPipe) : this.Compile(pipeline));
			return Expression.Block(expression2, expression);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x0013FC88 File Offset: 0x0013DE88
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			Expression expr = (exitStatementAst.Pipeline != null) ? this.CaptureStatementResults(exitStatementAst.Pipeline, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null) : ExpressionCache.Constant(0);
			return Expression.Block(this.UpdatePosition(exitStatementAst), Expression.Throw(Expression.Call(CachedReflectionInfo.PipelineOps_GetExitException, expr.Convert(typeof(object))), typeof(void)));
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x0013FCEC File Offset: 0x0013DEEC
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			Expression expr = throwStatementAst.IsRethrow ? Compiler._currentExceptionBeingHandled : ((throwStatementAst.Pipeline == null) ? ExpressionCache.NullConstant : this.CaptureStatementResults(throwStatementAst.Pipeline, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null));
			return Expression.Block(this.UpdatePosition(throwStatementAst), Expression.Throw(Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_ConvertToException, expr.Convert(typeof(object)), Expression.Constant(throwStatementAst.Extent))));
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x0013FD5C File Offset: 0x0013DF5C
		public Expression GenerateCallContains(Expression lhs, Expression rhs, bool ignoreCase)
		{
			return Expression.Call(CachedReflectionInfo.ParserOps_ContainsOperatorCompiled, Compiler._executionContextParameter, Expression.Constant(CallSite<Func<CallSite, object, IEnumerator>>.Create(PSEnumerableBinder.Get())), Expression.Constant(CallSite<Func<CallSite, object, object, object>>.Create(PSBinaryOperationBinder.Get(ExpressionType.Equal, ignoreCase, true))), lhs.Cast(typeof(object)), rhs.Cast(typeof(object)));
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x0013FDBC File Offset: 0x0013DFBC
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			object value;
			if (!this.CompilingConstantExpression && IsConstantValueVisitor.IsConstant(binaryExpressionAst, out value, false, false))
			{
				return Expression.Constant(value);
			}
			Expression expression = this.CompileExpressionOperand(binaryExpressionAst.Left);
			Expression expression2 = this.CompileExpressionOperand(binaryExpressionAst.Right);
			switch (binaryExpressionAst.Operator)
			{
			case TokenKind.DotDot:
				return Expression.Call(CachedReflectionInfo.IntOps_Range, expression.Convert(typeof(int)), expression2.Convert(typeof(int)));
			case TokenKind.Multiply:
			{
				if (expression.Type == typeof(double) && expression2.Type == typeof(double))
				{
					return Expression.Multiply(expression, expression2);
				}
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Multiply, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Divide:
			{
				if (expression.Type == typeof(double) && expression2.Type == typeof(double))
				{
					return Expression.Divide(expression, expression2);
				}
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Divide, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Rem:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Modulo, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Plus:
			{
				if (expression.Type == typeof(double) && expression2.Type == typeof(double))
				{
					return Expression.Add(expression, expression2);
				}
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Add, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Minus:
			{
				if (expression.Type == typeof(double) && expression2.Type == typeof(double))
				{
					return Expression.Subtract(expression, expression2);
				}
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Subtract, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Format:
				if (expression.Type != typeof(string))
				{
					expression = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), expression, Compiler._executionContextParameter);
				}
				return Expression.Call(CachedReflectionInfo.StringOps_FormatOperator, expression, expression2.Cast(typeof(object)));
			case TokenKind.And:
				return Expression.AndAlso(expression.Convert(typeof(bool)), expression2.Convert(typeof(bool)));
			case TokenKind.Or:
				return Expression.OrElse(expression.Convert(typeof(bool)), expression2.Convert(typeof(bool)));
			case TokenKind.Xor:
				return Expression.NotEqual(expression.Convert(typeof(bool)), expression2.Convert(typeof(bool)));
			case TokenKind.Band:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.And, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Bor:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Or, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Bxor:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.ExclusiveOr, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Join:
				return Expression.Call(CachedReflectionInfo.ParserOps_JoinOperator, Compiler._executionContextParameter, Expression.Constant(binaryExpressionAst.ErrorPosition), expression.Cast(typeof(object)), expression2.Cast(typeof(object)));
			case TokenKind.Ieq:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Equal, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Ine:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.NotEqual, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Ige:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.GreaterThanOrEqual, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Igt:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.GreaterThan, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Ilt:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.LessThan, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Ile:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.LessThanOrEqual, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Ilike:
				return Expression.Call(CachedReflectionInfo.ParserOps_LikeOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(false),
					ExpressionCache.Constant(true)
				});
			case TokenKind.Inotlike:
				return Expression.Call(CachedReflectionInfo.ParserOps_LikeOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(true),
					ExpressionCache.Constant(true)
				});
			case TokenKind.Imatch:
				return Expression.Call(CachedReflectionInfo.ParserOps_MatchOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(false),
					ExpressionCache.Constant(true)
				});
			case TokenKind.Inotmatch:
				return Expression.Call(CachedReflectionInfo.ParserOps_MatchOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(true),
					ExpressionCache.Constant(true)
				});
			case TokenKind.Ireplace:
				return Expression.Call(CachedReflectionInfo.ParserOps_ReplaceOperator, Compiler._executionContextParameter, Expression.Constant(binaryExpressionAst.ErrorPosition), expression.Cast(typeof(object)), expression2.Cast(typeof(object)), ExpressionCache.Constant(true));
			case TokenKind.Icontains:
				return this.GenerateCallContains(expression, expression2, true);
			case TokenKind.Inotcontains:
				return Expression.Not(this.GenerateCallContains(expression, expression2, true));
			case TokenKind.Iin:
				return this.GenerateCallContains(expression2, expression, true);
			case TokenKind.Inotin:
				return Expression.Not(this.GenerateCallContains(expression2, expression, true));
			case TokenKind.Isplit:
				return Expression.Call(CachedReflectionInfo.ParserOps_SplitOperator, Compiler._executionContextParameter, Expression.Constant(binaryExpressionAst.ErrorPosition), expression.Cast(typeof(object)), expression2.Cast(typeof(object)), ExpressionCache.Constant(true));
			case TokenKind.Ceq:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.Equal, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Cne:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.NotEqual, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Cge:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.GreaterThanOrEqual, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Cgt:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.GreaterThan, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Clt:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.LessThan, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Cle:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.LessThanOrEqual, false, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Clike:
				return Expression.Call(CachedReflectionInfo.ParserOps_LikeOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(false),
					ExpressionCache.Constant(false)
				});
			case TokenKind.Cnotlike:
				return Expression.Call(CachedReflectionInfo.ParserOps_LikeOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(true),
					ExpressionCache.Constant(false)
				});
			case TokenKind.Cmatch:
				return Expression.Call(CachedReflectionInfo.ParserOps_MatchOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(false),
					ExpressionCache.Constant(false)
				});
			case TokenKind.Cnotmatch:
				return Expression.Call(CachedReflectionInfo.ParserOps_MatchOperator, new Expression[]
				{
					Compiler._executionContextParameter,
					Expression.Constant(binaryExpressionAst.ErrorPosition),
					expression.Cast(typeof(object)),
					expression2.Cast(typeof(object)),
					ExpressionCache.Constant(true),
					ExpressionCache.Constant(false)
				});
			case TokenKind.Creplace:
				return Expression.Call(CachedReflectionInfo.ParserOps_ReplaceOperator, Compiler._executionContextParameter, Expression.Constant(binaryExpressionAst.ErrorPosition), expression.Cast(typeof(object)), expression2.Cast(typeof(object)), ExpressionCache.Constant(false));
			case TokenKind.Ccontains:
				return this.GenerateCallContains(expression, expression2, false);
			case TokenKind.Cnotcontains:
				return Expression.Not(this.GenerateCallContains(expression, expression2, false));
			case TokenKind.Cin:
				return this.GenerateCallContains(expression2, expression, false);
			case TokenKind.Cnotin:
				return Expression.Not(this.GenerateCallContains(expression2, expression, false));
			case TokenKind.Csplit:
				return Expression.Call(CachedReflectionInfo.ParserOps_SplitOperator, Compiler._executionContextParameter, Expression.Constant(binaryExpressionAst.ErrorPosition), expression.Cast(typeof(object)), expression2.Cast(typeof(object)), ExpressionCache.Constant(false));
			case TokenKind.Is:
			case TokenKind.IsNot:
			{
				if (expression2 is ConstantExpression && expression2.Type == typeof(Type))
				{
					Type type = (Type)((ConstantExpression)expression2).Value;
					if (!(type == typeof(PSCustomObject)) && !(type == typeof(PSObject)))
					{
						expression = (expression.Type.GetTypeInfo().IsValueType ? expression : Expression.Call(CachedReflectionInfo.PSObject_Base, expression));
						if (binaryExpressionAst.Operator == TokenKind.Is)
						{
							return Expression.TypeIs(expression, type);
						}
						return Expression.Not(Expression.TypeIs(expression, type));
					}
				}
				Expression expression3 = Expression.Call(CachedReflectionInfo.TypeOps_IsInstance, expression.Cast(typeof(object)), expression2.Cast(typeof(object)));
				if (binaryExpressionAst.Operator == TokenKind.IsNot)
				{
					expression3 = Expression.Not(expression3);
				}
				return expression3;
			}
			case TokenKind.As:
				return Expression.Call(CachedReflectionInfo.TypeOps_AsOperator, expression.Cast(typeof(object)), expression2.Convert(typeof(Type)));
			case TokenKind.Shl:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.LeftShift, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			case TokenKind.Shr:
			{
				DynamicMetaObjectBinder binder = PSBinaryOperationBinder.Get(ExpressionType.RightShift, true, false);
				return DynamicExpression.Dynamic(binder, typeof(object), expression, expression2);
			}
			}
			throw new InvalidOperationException("Unknown token in binary operator.");
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x0014095C File Offset: 0x0013EB5C
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			object value;
			if (!this.CompilingConstantExpression && IsConstantValueVisitor.IsConstant(unaryExpressionAst, out value, false, false))
			{
				return Expression.Constant(value);
			}
			ExpressionAst child = unaryExpressionAst.Child;
			TokenKind tokenKind = unaryExpressionAst.TokenKind;
			if (tokenKind <= TokenKind.Bnot)
			{
				if (tokenKind <= TokenKind.Exclaim)
				{
					switch (tokenKind)
					{
					case TokenKind.MinusMinus:
						return this.CompileIncrementOrDecrement(child, -1, true);
					case TokenKind.PlusPlus:
						return this.CompileIncrementOrDecrement(child, 1, true);
					default:
						if (tokenKind != TokenKind.Exclaim)
						{
							goto IL_1BE;
						}
						break;
					}
				}
				else
				{
					switch (tokenKind)
					{
					case TokenKind.Plus:
						return DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(ExpressionType.Add, true, false), typeof(object), ExpressionCache.Constant(0), this.CompileExpressionOperand(child));
					case TokenKind.Minus:
						return DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(ExpressionType.Subtract, true, false), typeof(object), ExpressionCache.Constant(0), this.CompileExpressionOperand(child));
					default:
						switch (tokenKind)
						{
						case TokenKind.Not:
							break;
						case TokenKind.Bnot:
							return DynamicExpression.Dynamic(PSUnaryOperationBinder.Get(ExpressionType.OnesComplement), typeof(object), this.CompileExpressionOperand(child));
						default:
							goto IL_1BE;
						}
						break;
					}
				}
				return DynamicExpression.Dynamic(PSUnaryOperationBinder.Get(ExpressionType.Not), typeof(object), this.CompileExpressionOperand(child));
			}
			if (tokenKind <= TokenKind.Isplit)
			{
				if (tokenKind == TokenKind.Join)
				{
					return Expression.Call(CachedReflectionInfo.ParserOps_UnaryJoinOperator, Compiler._executionContextParameter, Expression.Constant(unaryExpressionAst.Extent), this.CompileExpressionOperand(child).Cast(typeof(object)));
				}
				if (tokenKind != TokenKind.Isplit)
				{
					goto IL_1BE;
				}
			}
			else if (tokenKind != TokenKind.Csplit)
			{
				switch (tokenKind)
				{
				case TokenKind.PostfixPlusPlus:
					return this.CompileIncrementOrDecrement(child, 1, false);
				case TokenKind.PostfixMinusMinus:
					return this.CompileIncrementOrDecrement(child, -1, false);
				default:
					goto IL_1BE;
				}
			}
			return Expression.Call(CachedReflectionInfo.ParserOps_UnarySplitOperator, Compiler._executionContextParameter, Expression.Constant(unaryExpressionAst.Extent), this.CompileExpressionOperand(child).Cast(typeof(object)));
			IL_1BE:
			throw new InvalidOperationException("Unknown token in unary operator.");
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00140B34 File Offset: 0x0013ED34
		private Expression CompileIncrementOrDecrement(ExpressionAst exprAst, int valueToAdd, bool prefix)
		{
			IAssignableValue assignableValue = ((ISupportsAssignment)exprAst).GetAssignableValue();
			List<ParameterExpression> list = new List<ParameterExpression>();
			List<Expression> list2 = new List<Expression>();
			Expression value = assignableValue.GetValue(this, list2, list);
			ParameterExpression parameterExpression;
			if (prefix)
			{
				DynamicExpression dynamicExpression = DynamicExpression.Dynamic(PSUnaryOperationBinder.Get((valueToAdd == 1) ? ExpressionType.Increment : ExpressionType.Decrement), typeof(object), value);
				parameterExpression = Expression.Parameter(dynamicExpression.Type);
				list2.Add(Expression.Assign(parameterExpression, dynamicExpression));
				list2.Add(assignableValue.SetValue(this, parameterExpression));
				list2.Add(parameterExpression);
			}
			else
			{
				parameterExpression = Expression.Parameter(value.Type);
				list2.Add(Expression.Assign(parameterExpression, value));
				DynamicExpression rhs = DynamicExpression.Dynamic(PSUnaryOperationBinder.Get((valueToAdd == 1) ? ExpressionType.Increment : ExpressionType.Decrement), typeof(object), parameterExpression);
				list2.Add(assignableValue.SetValue(this, rhs));
				if (parameterExpression.Type.GetTypeInfo().IsValueType)
				{
					list2.Add(parameterExpression);
				}
				else
				{
					list2.Add(Expression.Condition(Expression.Equal(parameterExpression, ExpressionCache.NullConstant), ExpressionCache.Constant(0).Cast(typeof(object)), parameterExpression));
				}
			}
			list.Add(parameterExpression);
			return Expression.Block(list, list2);
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00140C60 File Offset: 0x0013EE60
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			object value;
			if (!this.CompilingConstantExpression && IsConstantValueVisitor.IsConstant(convertExpressionAst, out value, false, false))
			{
				return Expression.Constant(value);
			}
			ITypeName typeName = convertExpressionAst.Type.TypeName;
			HashtableAst hashtableAst = convertExpressionAst.Child as HashtableAst;
			Expression expression = null;
			if (hashtableAst != null)
			{
				ParameterExpression parameterExpression = this.NewTemp(typeof(OrderedDictionary), "orderedDictionary");
				if (typeName.FullName.Equals("ordered", StringComparison.OrdinalIgnoreCase))
				{
					return Expression.Block(typeof(OrderedDictionary), new ParameterExpression[]
					{
						parameterExpression
					}, this.BuildHashtable(hashtableAst.KeyValuePairs, parameterExpression, true));
				}
				if (typeName.FullName.Equals("PSCustomObject", StringComparison.OrdinalIgnoreCase))
				{
					expression = Expression.Block(typeof(OrderedDictionary), new ParameterExpression[]
					{
						parameterExpression
					}, this.BuildHashtable(hashtableAst.KeyValuePairs, parameterExpression, true));
				}
			}
			if (convertExpressionAst.IsRef())
			{
				VariableExpressionAst variableExpressionAst = convertExpressionAst.Child as VariableExpressionAst;
				if (variableExpressionAst != null && variableExpressionAst.VariablePath.IsVariable && !variableExpressionAst.IsConstantVariable())
				{
					IEnumerable<PropertyInfo> enumerable;
					bool flag;
					Type variableType = variableExpressionAst.GetVariableType(this, out enumerable, out flag);
					return Expression.Call(CachedReflectionInfo.VariableOps_GetVariableAsRef, Expression.Constant(variableExpressionAst.VariablePath), Compiler._executionContextParameter, (variableType != null && variableType != typeof(object)) ? Expression.Constant(variableType, typeof(Type)) : ExpressionCache.NullType);
				}
			}
			if (expression == null)
			{
				expression = this.Compile(convertExpressionAst.Child);
			}
			if (typeName.FullName.Equals("PSCustomObject", StringComparison.OrdinalIgnoreCase))
			{
				return DynamicExpression.Dynamic(PSCustomObjectConverter.Get(), typeof(object), expression);
			}
			return Compiler.ConvertValue(typeName, expression);
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00140E15 File Offset: 0x0013F015
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return Expression.Constant(constantExpressionAst.Value);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00140E22 File Offset: 0x0013F022
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return Expression.Constant(stringConstantExpressionAst.Value);
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00140E30 File Offset: 0x0013F030
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			if (subExpressionAst.SubExpression.Statements.Count == 0)
			{
				return ExpressionCache.NullConstant;
			}
			bool flag = subExpressionAst.ShouldPreserveOutputInCaseOfException();
			return this.CaptureAstResults(subExpressionAst.SubExpression, flag ? Compiler.CaptureAstContext.AssignmentWithResultPreservation : Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null);
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00140E70 File Offset: 0x0013F070
		public object VisitUsingExpression(UsingExpressionAst usingExpression)
		{
			string usingExpressionKey = PsUtils.GetUsingExpressionKey(usingExpression);
			return Expression.Call(CachedReflectionInfo.VariableOps_GetUsingValue, this.LocalVariablesParameter, Expression.Constant(usingExpressionKey), ExpressionCache.Constant(usingExpression.RuntimeUsingIndex), Compiler._executionContextParameter);
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00140EAC File Offset: 0x0013F0AC
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			VariablePath variablePath = variableExpressionAst.VariablePath;
			if (variablePath.IsVariable)
			{
				if (variablePath.UnqualifiedPath.Equals("null", StringComparison.OrdinalIgnoreCase))
				{
					return ExpressionCache.NullConstant;
				}
				if (variablePath.UnqualifiedPath.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					return ExpressionCache.Constant(true);
				}
				if (variablePath.UnqualifiedPath.Equals("false", StringComparison.OrdinalIgnoreCase))
				{
					return ExpressionCache.Constant(false);
				}
			}
			int tupleIndex = variableExpressionAst.TupleIndex;
			if (variableExpressionAst.Automatic)
			{
				if (!variableExpressionAst.VariablePath.UnqualifiedPath.Equals("?", StringComparison.OrdinalIgnoreCase))
				{
					return this.GetAutomaticVariable(variableExpressionAst);
				}
				if (this.Optimize)
				{
					return Expression.Property(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_QuestionMarkVariableValue);
				}
				return Compiler.CallGetVariable(Expression.Constant(variableExpressionAst.VariablePath), variableExpressionAst);
			}
			else
			{
				if (tupleIndex < 0)
				{
					return Compiler.CallGetVariable(Expression.Constant(variableExpressionAst.VariablePath), variableExpressionAst);
				}
				return this.GetLocal(tupleIndex);
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x00140F8C File Offset: 0x0013F18C
		internal Expression CompileTypeName(ITypeName typeName)
		{
			Type type;
			try
			{
				type = typeName.GetReflectionType();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				type = null;
			}
			if (type != null)
			{
				return Expression.Constant(type, typeof(Type));
			}
			return Expression.Call(CachedReflectionInfo.TypeOps_ResolveTypeName, Expression.Constant(typeName));
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00140FE8 File Offset: 0x0013F1E8
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return this.CompileTypeName(typeExpressionAst.TypeName);
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00140FF8 File Offset: 0x0013F1F8
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			if (memberExpressionAst.Static && memberExpressionAst.Expression is TypeExpressionAst)
			{
				Type reflectionType = ((TypeExpressionAst)memberExpressionAst.Expression).TypeName.GetReflectionType();
				if (reflectionType != null && !reflectionType.GetTypeInfo().IsGenericTypeDefinition)
				{
					StringConstantExpressionAst stringConstantExpressionAst = memberExpressionAst.Member as StringConstantExpressionAst;
					if (stringConstantExpressionAst != null)
					{
						MemberInfo[] member = reflectionType.GetMember(stringConstantExpressionAst.Value, MemberTypes.Field | MemberTypes.Property, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (member.Length == 1)
						{
							PropertyInfo propertyInfo = member[0] as PropertyInfo;
							if (!(propertyInfo != null))
							{
								return Expression.Field(null, (FieldInfo)member[0]);
							}
							if (propertyInfo.CanRead)
							{
								return Expression.Property(null, propertyInfo);
							}
						}
					}
				}
			}
			Expression arg = this.CompileExpressionOperand(memberExpressionAst.Expression);
			StringConstantExpressionAst stringConstantExpressionAst2 = memberExpressionAst.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst2 != null)
			{
				string value = stringConstantExpressionAst2.Value;
				return DynamicExpression.Dynamic(PSGetMemberBinder.Get(value, this._memberFunctionType, memberExpressionAst.Static), typeof(object), arg);
			}
			Expression arg2 = this.Compile(memberExpressionAst.Member);
			return DynamicExpression.Dynamic(PSGetDynamicMemberBinder.Get(this._memberFunctionType, memberExpressionAst.Static), typeof(object), arg, arg2);
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00141124 File Offset: 0x0013F324
		internal static PSMethodInvocationConstraints GetInvokeMemberConstraints(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			ReadOnlyCollection<ExpressionAst> arguments = invokeMemberExpressionAst.Arguments;
			Type typeConstraintForMethodResolution = Compiler.GetTypeConstraintForMethodResolution(invokeMemberExpressionAst.Expression);
			return Compiler.CombineTypeConstraintForMethodResolution(typeConstraintForMethodResolution, (arguments != null) ? arguments.Select(new Func<ExpressionAst, Type>(Compiler.GetTypeConstraintForMethodResolution)).ToArray<Type>() : null);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00141168 File Offset: 0x0013F368
		internal static PSMethodInvocationConstraints GetInvokeMemberConstraints(BaseCtorInvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			Type targetType = null;
			ReadOnlyCollection<ExpressionAst> arguments = invokeMemberExpressionAst.Arguments;
			TypeDefinitionAst ancestorTypeDefinitionAst = Ast.GetAncestorTypeDefinitionAst(invokeMemberExpressionAst);
			if (ancestorTypeDefinitionAst != null)
			{
				targetType = ancestorTypeDefinitionAst.Type.GetTypeInfo().BaseType;
			}
			return Compiler.CombineTypeConstraintForMethodResolution(targetType, (arguments != null) ? arguments.Select(new Func<ExpressionAst, Type>(Compiler.GetTypeConstraintForMethodResolution)).ToArray<Type>() : null);
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x001411BC File Offset: 0x0013F3BC
		internal Expression InvokeMember(string name, PSMethodInvocationConstraints constraints, Expression target, IEnumerable<Expression> args, bool @static, bool propertySet)
		{
			CallInfo callInfo = new CallInfo(args.Count<Expression>(), new string[0]);
			Type classScope = (this._memberFunctionType != null) ? this._memberFunctionType.Type : null;
			CallSiteBinder binder = (name.Equals("new", StringComparison.OrdinalIgnoreCase) && @static) ? PSCreateInstanceBinder.Get(callInfo, constraints, true) : PSInvokeMemberBinder.Get(name, callInfo, @static, propertySet, constraints, classScope);
			return DynamicExpression.Dynamic(binder, typeof(object), args.Prepend(target));
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00141238 File Offset: 0x0013F438
		private Expression InvokeBaseCtorMethod(PSMethodInvocationConstraints constraints, Expression target, IEnumerable<Expression> args)
		{
			CallInfo callInfo = new CallInfo(args.Count<Expression>(), new string[0]);
			PSInvokeBaseCtorBinder binder = PSInvokeBaseCtorBinder.Get(callInfo, constraints);
			return DynamicExpression.Dynamic(binder, typeof(object), args.Prepend(target));
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00141278 File Offset: 0x0013F478
		internal Expression InvokeDynamicMember(Expression memberNameExpr, PSMethodInvocationConstraints constraints, Expression target, IEnumerable<Expression> args, bool @static, bool propertySet)
		{
			PSInvokeDynamicMemberBinder binder = PSInvokeDynamicMemberBinder.Get(new CallInfo(args.Count<Expression>(), new string[0]), this._memberFunctionType, @static, propertySet, constraints);
			return DynamicExpression.Dynamic(binder, typeof(object), args.Prepend(memberNameExpr).Prepend(target));
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001412C8 File Offset: 0x0013F4C8
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			PSMethodInvocationConstraints invokeMemberConstraints = Compiler.GetInvokeMemberConstraints(invokeMemberExpressionAst);
			Expression target = this.CompileExpressionOperand(invokeMemberExpressionAst.Expression);
			IEnumerable<Expression> args = this.CompileInvocationArguments(invokeMemberExpressionAst.Arguments);
			StringConstantExpressionAst stringConstantExpressionAst = invokeMemberExpressionAst.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				return this.InvokeMember(stringConstantExpressionAst.Value, invokeMemberConstraints, target, args, invokeMemberExpressionAst.Static, false);
			}
			Expression memberNameExpr = this.Compile(invokeMemberExpressionAst.Member);
			return this.InvokeDynamicMember(memberNameExpr, invokeMemberConstraints, target, args, invokeMemberExpressionAst.Static, false);
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0014133C File Offset: 0x0013F53C
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			Expression expression = this.CaptureAstResults(arrayExpressionAst.SubExpression, Compiler.CaptureAstContext.Enumerable, null);
			if (expression.Type.IsArray)
			{
				return expression;
			}
			if (expression.Type == typeof(List<object>))
			{
				return Expression.Call(expression, CachedReflectionInfo.ObjectList_ToArray);
			}
			if (expression.Type == typeof(object[]))
			{
				return expression;
			}
			if (expression.Type.GetTypeInfo().IsPrimitive)
			{
				return Expression.NewArrayInit(typeof(object), new Expression[]
				{
					expression.Cast(typeof(object))
				});
			}
			if (expression.Type == typeof(void))
			{
				return Expression.NewArrayInit(typeof(object), new Expression[0]);
			}
			return DynamicExpression.Dynamic(PSToObjectArrayBinder.Get(), typeof(object[]), expression);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x0014143A File Offset: 0x0013F63A
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			return Expression.NewArrayInit(typeof(object), from elem in arrayLiteralAst.Elements
			select this.Compile(elem).Cast(typeof(object)));
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x001416B8 File Offset: 0x0013F8B8
		private IEnumerable<Expression> BuildHashtable(ReadOnlyCollection<Tuple<ExpressionAst, StatementAst>> keyValuePairs, ParameterExpression temp, bool ordered)
		{
			yield return Expression.Assign(temp, Expression.New(ordered ? CachedReflectionInfo.OrderedDictionary_ctor : CachedReflectionInfo.Hashtable_ctor, new Expression[]
			{
				ExpressionCache.Constant(keyValuePairs.Count),
				ExpressionCache.CurrentCultureIgnoreCaseComparer.Cast(typeof(IEqualityComparer))
			}));
			for (int index = 0; index < keyValuePairs.Count; index++)
			{
				Tuple<ExpressionAst, StatementAst> keyValuePair = keyValuePairs[index];
				Expression key = Expression.Convert(this.Compile(keyValuePair.Item1), typeof(object));
				Expression value = Expression.Convert(this.CaptureStatementResults(keyValuePair.Item2, Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null), typeof(object));
				Expression errorExtent = Expression.Constant(keyValuePair.Item1.Extent);
				yield return Expression.Call(CachedReflectionInfo.HashtableOps_AddKeyValuePair, temp, key, value, errorExtent);
			}
			yield return temp;
			yield break;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x001416EC File Offset: 0x0013F8EC
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			ParameterExpression parameterExpression = this.NewTemp(typeof(Hashtable), "hashtable");
			return Expression.Block(typeof(Hashtable), new ParameterExpression[]
			{
				parameterExpression
			}, this.BuildHashtable(hashtableAst.KeyValuePairs, parameterExpression, false));
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00141738 File Offset: 0x0013F938
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return Expression.Call(Expression.Constant(new ScriptBlockExpressionWrapper(scriptBlockExpressionAst.ScriptBlock)), CachedReflectionInfo.ScriptBlockExpressionWrapper_GetScriptBlock, Compiler._executionContextParameter, ExpressionCache.Constant(false));
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00141760 File Offset: 0x0013F960
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			PipelineBaseAst pipeline = parenExpressionAst.Pipeline;
			AssignmentStatementAst assignmentStatementAst = pipeline as AssignmentStatementAst;
			if (assignmentStatementAst != null)
			{
				return this.CompileAssignment(assignmentStatementAst, null);
			}
			bool flag = parenExpressionAst.ShouldPreserveOutputInCaseOfException();
			return this.CaptureStatementResults(pipeline, flag ? Compiler.CaptureAstContext.AssignmentWithResultPreservation : Compiler.CaptureAstContext.AssignmentWithoutResultPreservation, null);
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x001417D0 File Offset: 0x0013F9D0
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			ConstantExpression arg = Expression.Constant(expandableStringExpressionAst.FormatExpression);
			ReadOnlyCollection<ExpressionAst> nestedExpressions = expandableStringExpressionAst.NestedExpressions;
			PSToStringBinder toStringBinder = PSToStringBinder.Get();
			NewArrayExpression arg2 = Expression.NewArrayInit(typeof(string), from e in nestedExpressions
			select DynamicExpression.Dynamic(toStringBinder, typeof(string), this.Compile(e), Compiler._executionContextParameter));
			return Expression.Call(CachedReflectionInfo.StringOps_FormatOperator, arg, arg2);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00141838 File Offset: 0x0013FA38
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			Expression expression = this.CompileExpressionOperand(indexExpressionAst.Target);
			ExpressionAst index = indexExpressionAst.Index;
			ArrayLiteralAst arrayLiteralAst = index as ArrayLiteralAst;
			PSMethodInvocationConstraints constraints = Compiler.CombineTypeConstraintForMethodResolution(Compiler.GetTypeConstraintForMethodResolution(indexExpressionAst.Target), Compiler.GetTypeConstraintForMethodResolution(index));
			if (arrayLiteralAst != null && arrayLiteralAst.Elements.Count > 1)
			{
				return DynamicExpression.Dynamic(PSGetIndexBinder.Get(arrayLiteralAst.Elements.Count, constraints, true), typeof(object), arrayLiteralAst.Elements.Select(new Func<ExpressionAst, Expression>(this.CompileExpressionOperand)).Prepend(expression));
			}
			return DynamicExpression.Dynamic(PSGetIndexBinder.Get(1, constraints, true), typeof(object), expression, this.CompileExpressionOperand(index));
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001418E6 File Offset: 0x0013FAE6
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			return attributedExpressionAst.Child.Accept(this);
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x001418F4 File Offset: 0x0013FAF4
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			return null;
		}

		// Token: 0x04001EAF RID: 7855
		internal static readonly ParameterExpression _executionContextParameter;

		// Token: 0x04001EB0 RID: 7856
		internal static readonly ParameterExpression _functionContext;

		// Token: 0x04001EB1 RID: 7857
		internal static readonly ParameterExpression _returnPipe;

		// Token: 0x04001EB2 RID: 7858
		private static readonly Expression _setDollarQuestionToTrue;

		// Token: 0x04001EB3 RID: 7859
		private static readonly Expression _callCheckForInterrupts;

		// Token: 0x04001EB4 RID: 7860
		private static readonly Expression _getCurrentPipe;

		// Token: 0x04001EB5 RID: 7861
		private static readonly Expression _currentExceptionBeingHandled;

		// Token: 0x04001EB6 RID: 7862
		private static readonly CatchBlock _catchFlowControl;

		// Token: 0x04001EB7 RID: 7863
		internal static readonly CatchBlock[] _stmtCatchHandlers;

		// Token: 0x04001EB8 RID: 7864
		internal static readonly Type DottedLocalsTupleType = MutableTuple.MakeTupleType(SpecialVariables.AutomaticVariableTypes);

		// Token: 0x04001EB9 RID: 7865
		internal static Type DottedScriptCmdletLocalsTupleType = MutableTuple.MakeTupleType(SpecialVariables.AutomaticVariableTypes.Concat(SpecialVariables.PreferenceVariableTypes).ToArray<Type>());

		// Token: 0x04001EBA RID: 7866
		internal static readonly Dictionary<string, int> DottedLocalsNameIndexMap = new Dictionary<string, int>(SpecialVariables.AutomaticVariableTypes.Length, StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001EBB RID: 7867
		internal static readonly Dictionary<string, int> DottedScriptCmdletLocalsNameIndexMap = new Dictionary<string, int>(SpecialVariables.AutomaticVariableTypes.Length + SpecialVariables.PreferenceVariableTypes.Length, StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001EBC RID: 7868
		private SymbolDocumentInfo _debugSymbolDocument;

		// Token: 0x04001EBD RID: 7869
		internal TypeDefinitionAst _memberFunctionType;

		// Token: 0x04001EBE RID: 7870
		private bool _compilingTrap;

		// Token: 0x04001EBF RID: 7871
		private bool _compilingSingleExpression;

		// Token: 0x04001EC0 RID: 7872
		private bool _compilingScriptCmdlet;

		// Token: 0x04001EC1 RID: 7873
		private string _currentFunctionName;

		// Token: 0x04001EC2 RID: 7874
		private int _switchTupleIndex = -1;

		// Token: 0x04001EC3 RID: 7875
		private int _foreachTupleIndex = -1;

		// Token: 0x04001EC4 RID: 7876
		private readonly List<IScriptExtent> _sequencePoints;

		// Token: 0x04001EC5 RID: 7877
		private int _stmtCount;

		// Token: 0x04001EC6 RID: 7878
		private Type _memberFunctionReturnType;

		// Token: 0x04001EC7 RID: 7879
		private int _tempCounter;

		// Token: 0x04001EC8 RID: 7880
		private static readonly Dictionary<CallInfo, Delegate> _attributeGeneratorCache = new Dictionary<CallInfo, Delegate>();

		// Token: 0x04001EC9 RID: 7881
		private LabelTarget _returnTarget;

		// Token: 0x04001ECA RID: 7882
		private Expression<Action<FunctionContext>> _dynamicParamBlockLambda;

		// Token: 0x04001ECB RID: 7883
		private Expression<Action<FunctionContext>> _beginBlockLambda;

		// Token: 0x04001ECC RID: 7884
		private Expression<Action<FunctionContext>> _processBlockLambda;

		// Token: 0x04001ECD RID: 7885
		private Expression<Action<FunctionContext>> _endBlockLambda;

		// Token: 0x04001ECE RID: 7886
		private readonly List<Compiler.LoopGotoTargets> _loopTargets = new List<Compiler.LoopGotoTargets>();

		// Token: 0x04001ECF RID: 7887
		private bool _generatingWhileOrDoLoop;

		// Token: 0x04001ED0 RID: 7888
		private int _trapNestingCount;

		// Token: 0x04001ED1 RID: 7889
		private bool generatedCallToDefineWorkflows;

		// Token: 0x020005A4 RID: 1444
		internal class DefaultValueExpressionWrapper
		{
			// Token: 0x17000D55 RID: 3413
			// (get) Token: 0x06003D55 RID: 15701 RVA: 0x001418F7 File Offset: 0x0013FAF7
			// (set) Token: 0x06003D56 RID: 15702 RVA: 0x001418FF File Offset: 0x0013FAFF
			internal ExpressionAst Expression { get; set; }

			// Token: 0x06003D57 RID: 15703 RVA: 0x00141908 File Offset: 0x0013FB08
			internal object GetValue(ExecutionContext context, SessionStateInternal sessionStateInternal, IDictionary usingValues = null)
			{
				object expressionValue;
				lock (this)
				{
					expressionValue = Compiler.GetExpressionValue(this.Expression, true, context, sessionStateInternal, usingValues, ref this._delegate, ref this._sequencePoints, ref this._localsTupleType);
				}
				return expressionValue;
			}

			// Token: 0x04001EED RID: 7917
			private Func<FunctionContext, object> _delegate;

			// Token: 0x04001EEE RID: 7918
			private IScriptExtent[] _sequencePoints;

			// Token: 0x04001EEF RID: 7919
			private Type _localsTupleType;
		}

		// Token: 0x020005A5 RID: 1445
		private class LoopGotoTargets
		{
			// Token: 0x06003D59 RID: 15705 RVA: 0x00141968 File Offset: 0x0013FB68
			internal LoopGotoTargets(string label, LabelTarget breakLabel, LabelTarget continueLabel)
			{
				this.Label = label;
				this.BreakLabel = breakLabel;
				this.ContinueLabel = continueLabel;
			}

			// Token: 0x17000D56 RID: 3414
			// (get) Token: 0x06003D5A RID: 15706 RVA: 0x00141985 File Offset: 0x0013FB85
			// (set) Token: 0x06003D5B RID: 15707 RVA: 0x0014198D File Offset: 0x0013FB8D
			internal string Label { get; private set; }

			// Token: 0x17000D57 RID: 3415
			// (get) Token: 0x06003D5C RID: 15708 RVA: 0x00141996 File Offset: 0x0013FB96
			// (set) Token: 0x06003D5D RID: 15709 RVA: 0x0014199E File Offset: 0x0013FB9E
			internal LabelTarget ContinueLabel { get; private set; }

			// Token: 0x17000D58 RID: 3416
			// (get) Token: 0x06003D5E RID: 15710 RVA: 0x001419A7 File Offset: 0x0013FBA7
			// (set) Token: 0x06003D5F RID: 15711 RVA: 0x001419AF File Offset: 0x0013FBAF
			internal LabelTarget BreakLabel { get; private set; }
		}

		// Token: 0x020005A6 RID: 1446
		private enum CaptureAstContext
		{
			// Token: 0x04001EF5 RID: 7925
			Condition,
			// Token: 0x04001EF6 RID: 7926
			Enumerable,
			// Token: 0x04001EF7 RID: 7927
			AssignmentWithResultPreservation,
			// Token: 0x04001EF8 RID: 7928
			AssignmentWithoutResultPreservation
		}

		// Token: 0x020005A7 RID: 1447
		// (Invoke) Token: 0x06003D61 RID: 15713
		private delegate void MergeRedirectExprs(List<Expression> exprs, List<Expression> finallyExprs);

		// Token: 0x020005A8 RID: 1448
		private class AutomaticVarSaver
		{
			// Token: 0x06003D64 RID: 15716 RVA: 0x001419B8 File Offset: 0x0013FBB8
			internal AutomaticVarSaver(Compiler compiler, VariablePath autoVarPath, int automaticVar)
			{
				this._compiler = compiler;
				this._autoVarPath = autoVarPath;
				this._automaticVar = automaticVar;
			}

			// Token: 0x06003D65 RID: 15717 RVA: 0x00141AA4 File Offset: 0x0013FCA4
			internal IEnumerable<ParameterExpression> GetTemps()
			{
				yield return this._oldValue;
				yield break;
			}

			// Token: 0x06003D66 RID: 15718 RVA: 0x00141AC4 File Offset: 0x0013FCC4
			internal Expression SaveAutomaticVar()
			{
				Expression expression = (this._automaticVar < 0) ? Compiler.CallGetVariable(Expression.Constant(this._autoVarPath), null) : this._compiler.GetLocal(this._automaticVar);
				this._oldValue = this._compiler.NewTemp(expression.Type, "old_" + this._autoVarPath.UnqualifiedPath);
				return Expression.Assign(this._oldValue, expression);
			}

			// Token: 0x06003D67 RID: 15719 RVA: 0x00141B37 File Offset: 0x0013FD37
			internal Expression SetNewValue(Expression newValue)
			{
				if (this._automaticVar < 0)
				{
					return Compiler.CallSetVariable(Expression.Constant(this._autoVarPath), newValue, null);
				}
				return Expression.Assign(this._compiler.GetLocal(this._automaticVar), newValue);
			}

			// Token: 0x06003D68 RID: 15720 RVA: 0x00141B6C File Offset: 0x0013FD6C
			internal Expression RestoreAutomaticVar()
			{
				if (this._automaticVar < 0)
				{
					return Compiler.CallSetVariable(Expression.Constant(this._autoVarPath), this._oldValue, null);
				}
				return Expression.Assign(this._compiler.GetLocal(this._automaticVar), this._oldValue);
			}

			// Token: 0x04001EF9 RID: 7929
			private readonly Compiler _compiler;

			// Token: 0x04001EFA RID: 7930
			private readonly int _automaticVar;

			// Token: 0x04001EFB RID: 7931
			private readonly VariablePath _autoVarPath;

			// Token: 0x04001EFC RID: 7932
			private ParameterExpression _oldValue;
		}
	}
}
