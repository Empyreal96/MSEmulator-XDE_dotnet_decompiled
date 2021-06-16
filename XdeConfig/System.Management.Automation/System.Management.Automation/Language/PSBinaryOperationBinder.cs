using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x02000615 RID: 1557
	internal class PSBinaryOperationBinder : BinaryOperationBinder
	{
		// Token: 0x06004397 RID: 17303 RVA: 0x00163F34 File Offset: 0x00162134
		internal static PSBinaryOperationBinder Get(ExpressionType operation, bool ignoreCase = true, bool scalarCompare = false)
		{
			PSBinaryOperationBinder psbinaryOperationBinder;
			lock (PSBinaryOperationBinder._binderCache)
			{
				Tuple<ExpressionType, bool, bool> key = Tuple.Create<ExpressionType, bool, bool>(operation, ignoreCase, scalarCompare);
				if (!PSBinaryOperationBinder._binderCache.TryGetValue(key, out psbinaryOperationBinder))
				{
					psbinaryOperationBinder = new PSBinaryOperationBinder(operation, ignoreCase, scalarCompare);
					PSBinaryOperationBinder._binderCache.Add(key, psbinaryOperationBinder);
				}
			}
			return psbinaryOperationBinder;
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x00163F9C File Offset: 0x0016219C
		private PSBinaryOperationBinder(ExpressionType operation, bool ignoreCase, bool scalarCompare) : base(operation)
		{
			this._ignoreCase = ignoreCase;
			this._scalarCompare = scalarCompare;
			this._version = 0;
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x00163FBC File Offset: 0x001621BC
		private Func<object, object, bool> GetScalarCompareDelegate()
		{
			if (this._compareDelegate == null)
			{
				ParameterExpression parameterExpression;
				ParameterExpression parameterExpression2;
				Func<object, object, bool> value = Expression.Lambda<Func<object, object, bool>>(DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(base.Operation, this._ignoreCase, true), typeof(object), parameterExpression, parameterExpression2).Cast(typeof(bool)), new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile();
				Interlocked.CompareExchange<Func<object, object, bool>>(ref this._compareDelegate, value, null);
			}
			return this._compareDelegate;
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x00164060 File Offset: 0x00162260
		public override DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue || !arg.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[]
				{
					arg
				}).WriteToDebugLog(this);
			}
			if (((target.Value is PSObject && PSObject.Base(target.Value) != target.Value) || (arg.Value is PSObject && PSObject.Base(arg.Value) != arg.Value)) && (base.Operation != ExpressionType.Add || PSEnumerableBinder.IsEnumerable(target) == null))
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target,
					arg
				}).WriteToDebugLog(this);
			}
			ExpressionType operation = base.Operation;
			if (operation <= ExpressionType.Multiply)
			{
				switch (operation)
				{
				case ExpressionType.Add:
					return this.BinaryAdd(target, arg, errorSuggestion).WriteToDebugLog(this);
				case ExpressionType.AddChecked:
					break;
				case ExpressionType.And:
					return this.BinaryBitwiseAnd(target, arg, errorSuggestion).WriteToDebugLog(this);
				default:
					switch (operation)
					{
					case ExpressionType.Divide:
						return this.BinaryDivide(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.Equal:
						return this.CompareEQ(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.ExclusiveOr:
						return this.BinaryBitwiseXor(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.GreaterThan:
						return this.CompareGT(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.GreaterThanOrEqual:
						return this.CompareGE(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.LeftShift:
						return this.LeftShift(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.LessThan:
						return this.CompareLT(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.LessThanOrEqual:
						return this.CompareLE(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.Modulo:
						return this.BinaryRemainder(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.Multiply:
						return this.BinaryMultiply(target, arg, errorSuggestion).WriteToDebugLog(this);
					}
					break;
				}
			}
			else
			{
				switch (operation)
				{
				case ExpressionType.NotEqual:
					return this.CompareNE(target, arg, errorSuggestion).WriteToDebugLog(this);
				case ExpressionType.Or:
					return this.BinaryBitwiseOr(target, arg, errorSuggestion).WriteToDebugLog(this);
				default:
					switch (operation)
					{
					case ExpressionType.RightShift:
						return this.RightShift(target, arg, errorSuggestion).WriteToDebugLog(this);
					case ExpressionType.Subtract:
						return this.BinarySub(target, arg, errorSuggestion).WriteToDebugLog(this);
					}
					break;
				}
			}
			return (errorSuggestion ?? new DynamicMetaObject(Compiler.CreateThrow(typeof(object), typeof(PSNotImplementedException), new object[]
			{
				"Unimplemented operaton"
			}), target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}))).WriteToDebugLog(this);
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x001642E4 File Offset: 0x001624E4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSBinaryOperationBinder {0}{1} ver:{2}", new object[]
			{
				this.GetOperatorText(),
				this._scalarCompare ? " scalarOnly" : "",
				this._version
			});
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x00164338 File Offset: 0x00162538
		internal static void InvalidateCache()
		{
			lock (PSBinaryOperationBinder._binderCache)
			{
				foreach (PSBinaryOperationBinder psbinaryOperationBinder in PSBinaryOperationBinder._binderCache.Values)
				{
					psbinaryOperationBinder._version++;
				}
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x001643C0 File Offset: 0x001625C0
		private string GetOperatorText()
		{
			ExpressionType operation = base.Operation;
			if (operation <= ExpressionType.Multiply)
			{
				switch (operation)
				{
				case ExpressionType.Add:
					return TokenKind.Plus.Text();
				case ExpressionType.AddChecked:
					break;
				case ExpressionType.And:
					return TokenKind.Band.Text();
				default:
					switch (operation)
					{
					case ExpressionType.Divide:
						return TokenKind.Divide.Text();
					case ExpressionType.Equal:
						if (!this._ignoreCase)
						{
							return TokenKind.Ceq.Text();
						}
						return TokenKind.Ieq.Text();
					case ExpressionType.ExclusiveOr:
						return TokenKind.Bxor.Text();
					case ExpressionType.GreaterThan:
						if (!this._ignoreCase)
						{
							return TokenKind.Cgt.Text();
						}
						return TokenKind.Igt.Text();
					case ExpressionType.GreaterThanOrEqual:
						if (!this._ignoreCase)
						{
							return TokenKind.Cge.Text();
						}
						return TokenKind.Ige.Text();
					case ExpressionType.LeftShift:
						return TokenKind.Shl.Text();
					case ExpressionType.LessThan:
						if (!this._ignoreCase)
						{
							return TokenKind.Clt.Text();
						}
						return TokenKind.Ilt.Text();
					case ExpressionType.LessThanOrEqual:
						if (!this._ignoreCase)
						{
							return TokenKind.Cle.Text();
						}
						return TokenKind.Ile.Text();
					case ExpressionType.Modulo:
						return TokenKind.Rem.Text();
					case ExpressionType.Multiply:
						return TokenKind.Multiply.Text();
					}
					break;
				}
			}
			else
			{
				switch (operation)
				{
				case ExpressionType.NotEqual:
					if (!this._ignoreCase)
					{
						return TokenKind.Cne.Text();
					}
					return TokenKind.Ine.Text();
				case ExpressionType.Or:
					return TokenKind.Bor.Text();
				default:
					switch (operation)
					{
					case ExpressionType.RightShift:
						return TokenKind.Shr.Text();
					case ExpressionType.Subtract:
						return TokenKind.Minus.Text();
					}
					break;
				}
			}
			return "";
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x00164544 File Offset: 0x00162744
		private static DynamicMetaObject CallImplicitOp(string methodName, DynamicMetaObject target, DynamicMetaObject arg, string errorOperator, DynamicMetaObject errorSuggestion)
		{
			if (errorSuggestion != null && target.Value is DynamicObject)
			{
				return errorSuggestion;
			}
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ParserOps_ImplicitOp, target.Expression.Cast(typeof(object)), arg.Expression.Cast(typeof(object)), Expression.Constant(methodName), ExpressionCache.NullExtent, Expression.Constant(errorOperator)), target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}));
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x001645C4 File Offset: 0x001627C4
		private static bool IsValueNegative(object value, TypeCode typeCode)
		{
			switch (typeCode)
			{
			case TypeCode.SByte:
				return (sbyte)value < 0;
			case TypeCode.Int16:
				return (short)value < 0;
			case TypeCode.Int32:
				return (int)value < 0;
			case TypeCode.Int64:
				return (long)value < 0L;
			}
			return true;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x00164624 File Offset: 0x00162824
		private static Expression TypedZero(TypeCode typeCode)
		{
			switch (typeCode)
			{
			case TypeCode.SByte:
				return Expression.Constant(0);
			case TypeCode.Int16:
				return Expression.Constant(0);
			case TypeCode.Int32:
				return ExpressionCache.Constant(0);
			case TypeCode.Int64:
				return Expression.Constant(0L);
			}
			return null;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x00164688 File Offset: 0x00162888
		private static DynamicMetaObject FigureSignedUnsignedInt(DynamicMetaObject obj, TypeCode typeCode, TypeCode currentOpType, out Type opImplType, out Type argType, out bool shouldFallbackToDoubleInCaseOfOverflow)
		{
			opImplType = null;
			argType = null;
			shouldFallbackToDoubleInCaseOfOverflow = false;
			if (PSBinaryOperationBinder.IsValueNegative(obj.Value, typeCode))
			{
				switch (currentOpType)
				{
				case TypeCode.UInt32:
					opImplType = typeof(LongOps);
					argType = typeof(long);
					break;
				case TypeCode.UInt64:
					opImplType = typeof(DecimalOps);
					argType = typeof(decimal);
					shouldFallbackToDoubleInCaseOfOverflow = true;
					break;
				}
				return new DynamicMetaObject(obj.Expression, obj.PSGetTypeRestriction().Merge(BindingRestrictions.GetExpressionRestriction(Expression.LessThan(obj.Expression.Cast(obj.LimitType), PSBinaryOperationBinder.TypedZero(typeCode)))), obj.Value);
			}
			return new DynamicMetaObject(obj.Expression, obj.PSGetTypeRestriction().Merge(BindingRestrictions.GetExpressionRestriction(Expression.GreaterThanOrEqual(obj.Expression.Cast(obj.LimitType), PSBinaryOperationBinder.TypedZero(typeCode)))), obj.Value);
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x00164780 File Offset: 0x00162980
		private DynamicMetaObject BinaryNumericOp(string methodName, DynamicMetaObject target, DynamicMetaObject arg)
		{
			Type type = null;
			Type type2 = null;
			bool flag = false;
			TypeCode typeCode = LanguagePrimitives.GetTypeCode(target.LimitType);
			TypeCode typeCode2 = LanguagePrimitives.GetTypeCode(arg.LimitType);
			TypeCode typeCode3 = (typeCode >= typeCode2) ? typeCode : typeCode2;
			if (typeCode3 <= TypeCode.Int32)
			{
				type = typeof(IntOps);
				type2 = typeof(int);
			}
			else if (typeCode3 <= TypeCode.UInt32)
			{
				if (LanguagePrimitives.IsSignedInteger(typeCode))
				{
					target = PSBinaryOperationBinder.FigureSignedUnsignedInt(target, typeCode, typeCode3, out type, out type2, out flag);
				}
				else if (LanguagePrimitives.IsSignedInteger(typeCode2))
				{
					arg = PSBinaryOperationBinder.FigureSignedUnsignedInt(arg, typeCode2, typeCode3, out type, out type2, out flag);
				}
				if (type == null)
				{
					type = typeof(UIntOps);
					type2 = typeof(uint);
				}
			}
			else if (typeCode3 <= TypeCode.Int64)
			{
				type = typeof(LongOps);
				type2 = typeof(long);
			}
			else if (typeCode3 <= TypeCode.UInt64)
			{
				if (LanguagePrimitives.IsSignedInteger(typeCode))
				{
					target = PSBinaryOperationBinder.FigureSignedUnsignedInt(target, typeCode, typeCode3, out type, out type2, out flag);
				}
				else if (LanguagePrimitives.IsSignedInteger(typeCode2))
				{
					arg = PSBinaryOperationBinder.FigureSignedUnsignedInt(arg, typeCode2, typeCode3, out type, out type2, out flag);
				}
				if (type == null)
				{
					type = typeof(ULongOps);
					type2 = typeof(ulong);
				}
			}
			else if (typeCode3 == TypeCode.Decimal)
			{
				if (methodName.StartsWith("Compare", StringComparison.Ordinal))
				{
					if (LanguagePrimitives.IsFloating(typeCode))
					{
						return new DynamicMetaObject(Expression.Call(typeof(DecimalOps).GetMethod(methodName + "1", BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Cast(target.LimitType).Cast(typeof(double)), arg.Expression.Cast(arg.LimitType).Cast(typeof(decimal))), target.CombineRestrictions(new DynamicMetaObject[]
						{
							arg
						}));
					}
					if (LanguagePrimitives.IsFloating(typeCode2))
					{
						return new DynamicMetaObject(Expression.Call(typeof(DecimalOps).GetMethod(methodName + "2", BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Cast(target.LimitType).Cast(typeof(decimal)), arg.Expression.Cast(arg.LimitType).Cast(typeof(double))), target.CombineRestrictions(new DynamicMetaObject[]
						{
							arg
						}));
					}
				}
				type = typeof(DecimalOps);
				type2 = typeof(decimal);
			}
			else
			{
				type = typeof(DoubleOps);
				type2 = typeof(double);
			}
			Expression expression = Expression.Call(type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Cast(target.LimitType).Cast(type2), arg.Expression.Cast(arg.LimitType).Cast(type2));
			if (flag)
			{
				Type typeFromHandle = typeof(DoubleOps);
				Type typeFromHandle2 = typeof(double);
				Expression arg2 = Expression.Call(typeFromHandle.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Cast(target.LimitType).Cast(typeFromHandle2), arg.Expression.Cast(arg.LimitType).Cast(typeFromHandle2));
				ParameterExpression parameterExpression = Expression.Variable(typeof(RuntimeException), "psBinaryNumericOpException");
				CatchBlock catchBlock = Expression.Catch(parameterExpression, Expression.Block(Expression.IfThen(Expression.Not(Expression.TypeIs(Expression.Property(parameterExpression, "InnerException"), typeof(OverflowException))), Expression.Rethrow()), arg2));
				expression = Expression.TryCatch(expression, new CatchBlock[]
				{
					catchBlock
				});
			}
			if (target.LimitType.GetTypeInfo().IsEnum)
			{
				ExpressionType operation = base.Operation;
				switch (operation)
				{
				case ExpressionType.Equal:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					goto IL_3F6;
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Invoke:
				case ExpressionType.Lambda:
				case ExpressionType.LeftShift:
					break;
				default:
					if (operation == ExpressionType.NotEqual)
					{
						goto IL_3F6;
					}
					break;
				}
				expression = expression.Cast(target.LimitType).Cast(typeof(object));
			}
			IL_3F6:
			if (base.Operation == ExpressionType.Equal || base.Operation == ExpressionType.NotEqual)
			{
				expression = Expression.TryCatch(expression, new CatchBlock[]
				{
					Expression.Catch(typeof(InvalidCastException), (base.Operation == ExpressionType.Equal) ? ExpressionCache.BoxedFalse : ExpressionCache.BoxedTrue)
				});
			}
			return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}));
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x00164BF0 File Offset: 0x00162DF0
		private DynamicMetaObject BinaryNumericStringOp(DynamicMetaObject target, DynamicMetaObject arg)
		{
			List<ParameterExpression> variables = new List<ParameterExpression>();
			List<Expression> list = new List<Expression>();
			Expression arg2 = target.Expression;
			if (target.LimitType == typeof(string))
			{
				arg2 = PSBinaryOperationBinder.ConvertStringToNumber(target.Expression, arg.LimitType);
			}
			Expression arg3 = (arg.LimitType == typeof(string)) ? PSBinaryOperationBinder.ConvertStringToNumber(arg.Expression, target.LimitType) : arg.Expression;
			list.Add(DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(base.Operation, true, false), typeof(object), arg2, arg3));
			Expression expression = Expression.Block(variables, list);
			if (base.Operation == ExpressionType.Equal || base.Operation == ExpressionType.NotEqual)
			{
				expression = Expression.TryCatch(expression, new CatchBlock[]
				{
					Expression.Catch(typeof(InvalidCastException), (base.Operation == ExpressionType.Equal) ? ExpressionCache.BoxedFalse : ExpressionCache.BoxedTrue)
				});
			}
			return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}));
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x00164D03 File Offset: 0x00162F03
		internal static Expression ConvertStringToNumber(Expression expr, Type toType)
		{
			if (!toType.IsNumeric())
			{
				toType = typeof(int);
			}
			return Expression.Call(CachedReflectionInfo.Parser_ScanNumber, expr.Cast(typeof(string)), Expression.Constant(toType, typeof(Type)));
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x00164D44 File Offset: 0x00162F44
		private static DynamicMetaObject GetArgAsNumericOrPrimitive(DynamicMetaObject arg, Type targetType)
		{
			if (arg.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.Constant(0), arg.PSGetTypeRestriction(), 0);
			}
			bool flag = false;
			if (arg.LimitType.IsNumericOrPrimitive() && !arg.LimitType.GetTypeInfo().IsEnum)
			{
				if (!(targetType == typeof(decimal)) || !(arg.LimitType == typeof(bool)))
				{
					return arg;
				}
				flag = true;
			}
			bool debase;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(arg.Value, targetType, out debase);
			if (conversionData.Rank == ConversionRank.ImplicitCast || flag || arg.LimitType.GetTypeInfo().IsEnum)
			{
				return new DynamicMetaObject(PSConvertBinder.InvokeConverter(conversionData, arg.Expression, targetType, debase, ExpressionCache.InvariantCulture), arg.PSGetTypeRestriction());
			}
			return null;
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x00164E10 File Offset: 0x00163010
		private static Type GetBitwiseOpType(TypeCode opTypeCode)
		{
			Type typeFromHandle;
			if (opTypeCode <= TypeCode.Int32)
			{
				typeFromHandle = typeof(int);
			}
			else if (opTypeCode <= TypeCode.UInt32)
			{
				typeFromHandle = typeof(uint);
			}
			else if (opTypeCode <= TypeCode.Int64)
			{
				typeFromHandle = typeof(long);
			}
			else
			{
				typeFromHandle = typeof(ulong);
			}
			return typeFromHandle;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x00164E60 File Offset: 0x00163060
		private DynamicMetaObject BinaryAdd(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			if (target.Value == null)
			{
				return new DynamicMetaObject(arg.Expression.Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			if (target.LimitType.IsNumericOrPrimitive() && !(target.LimitType == typeof(char)))
			{
				DynamicMetaObject argAsNumericOrPrimitive = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(arg, target.LimitType);
				if (argAsNumericOrPrimitive != null)
				{
					return this.BinaryNumericOp("Add", target, argAsNumericOrPrimitive);
				}
				if (arg.LimitType == typeof(string))
				{
					return this.BinaryNumericStringOp(target, arg);
				}
			}
			Expression expression = null;
			if (target.LimitType == typeof(string))
			{
				expression = target.Expression.Cast(typeof(string));
			}
			else if (target.LimitType == typeof(char))
			{
				expression = Expression.New(CachedReflectionInfo.String_ctor_char_int, new Expression[]
				{
					target.Expression.Cast(typeof(char)),
					ExpressionCache.Constant(1)
				});
			}
			if (expression != null)
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.String_Concat_String, expression, PSToStringBinder.InvokeToString(ExpressionCache.GetExecutionContextFromTLS, arg.Expression)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject != null)
			{
				DynamicMetaObject dynamicMetaObject2 = PSEnumerableBinder.IsEnumerable(arg);
				Expression expression2;
				if (dynamicMetaObject2 != null)
				{
					expression2 = Expression.Call(CachedReflectionInfo.EnumerableOps_AddEnumerable, ExpressionCache.GetExecutionContextFromTLS, dynamicMetaObject.Expression.Cast(typeof(IEnumerator)), dynamicMetaObject2.Expression.Cast(typeof(IEnumerator)));
				}
				else
				{
					expression2 = Expression.Call(CachedReflectionInfo.EnumerableOps_AddObject, ExpressionCache.GetExecutionContextFromTLS, dynamicMetaObject.Expression.Cast(typeof(IEnumerator)), arg.Expression.Cast(typeof(object)));
				}
				return new DynamicMetaObject(expression2, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			if (!(target.Value is IDictionary))
			{
				return PSBinaryOperationBinder.CallImplicitOp("op_Addition", target, arg, "+", errorSuggestion);
			}
			if (arg.Value is IDictionary)
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.HashtableOps_Add, target.Expression.Cast(typeof(IDictionary)), arg.Expression.Cast(typeof(IDictionary))), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			return target.ThrowRuntimeError(new DynamicMetaObject[]
			{
				arg
			}, BindingRestrictions.Empty, "AddHashTableToNonHashTable", ParserStrings.AddHashTableToNonHashTable, new Expression[0]);
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x00165115 File Offset: 0x00163315
		private DynamicMetaObject BinarySub(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinarySubDivOrRem(target, arg, errorSuggestion, "Sub", "op_Subtraction", "-");
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x00165130 File Offset: 0x00163330
		private DynamicMetaObject BinaryMultiply(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			if (target.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.NullConstant, target.PSGetTypeRestriction());
			}
			if (target.LimitType.IsNumeric())
			{
				DynamicMetaObject argAsNumericOrPrimitive = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(arg, target.LimitType);
				if (argAsNumericOrPrimitive != null)
				{
					return this.BinaryNumericOp("Multiply", target, argAsNumericOrPrimitive);
				}
				if (arg.LimitType == typeof(string))
				{
					return this.BinaryNumericStringOp(target, arg);
				}
			}
			if (target.LimitType == typeof(string))
			{
				Expression arg2 = (arg.LimitType == typeof(string)) ? PSBinaryOperationBinder.ConvertStringToNumber(arg.Expression, typeof(int)).Convert(typeof(int)) : arg.CastOrConvert(typeof(int));
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.StringOps_Multiply, target.Expression.Cast(typeof(string)), arg2), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null)
			{
				return PSBinaryOperationBinder.CallImplicitOp("op_Multiply", target, arg, "*", errorSuggestion);
			}
			Expression arg3 = (arg.LimitType == typeof(string)) ? PSBinaryOperationBinder.ConvertStringToNumber(arg.Expression, typeof(int)).Convert(typeof(uint)) : arg.CastOrConvert(typeof(uint));
			if (target.LimitType.IsArray)
			{
				Type elementType = target.LimitType.GetElementType();
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_Multiply.MakeGenericMethod(new Type[]
				{
					elementType
				}), target.Expression.Cast(elementType.MakeArrayType()), arg3), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_Multiply, dynamicMetaObject.Expression, arg3), target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}));
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x00165344 File Offset: 0x00163544
		private DynamicMetaObject BinaryDivide(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinarySubDivOrRem(target, arg, errorSuggestion, "Divide", "op_Division", "/");
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0016535E File Offset: 0x0016355E
		private DynamicMetaObject BinaryRemainder(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinarySubDivOrRem(target, arg, errorSuggestion, "Remainder", "op_Modulus", "%");
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x00165378 File Offset: 0x00163578
		private DynamicMetaObject BinarySubDivOrRem(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion, string numericOpMethodName, string implicitOpMethodName, string errorOperatorText)
		{
			if (target.Value == null)
			{
				target = new DynamicMetaObject(ExpressionCache.Constant(0), target.PSGetTypeRestriction(), 0);
			}
			if (target.LimitType.IsNumericOrPrimitive())
			{
				DynamicMetaObject argAsNumericOrPrimitive = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(arg, target.LimitType);
				if (argAsNumericOrPrimitive != null)
				{
					return this.BinaryNumericOp(numericOpMethodName, target, argAsNumericOrPrimitive);
				}
				if (arg.LimitType == typeof(string))
				{
					return this.BinaryNumericStringOp(target, arg);
				}
			}
			if (target.LimitType == typeof(string))
			{
				return this.BinaryNumericStringOp(target, arg);
			}
			return PSBinaryOperationBinder.CallImplicitOp(implicitOpMethodName, target, arg, errorOperatorText, errorSuggestion);
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0016541C File Offset: 0x0016361C
		private DynamicMetaObject Shift(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion, string userOp, Func<Expression, Expression, Expression> exprGenerator)
		{
			if (target.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.Constant(0).Convert(typeof(object)), target.PSGetTypeRestriction());
			}
			if (target.LimitType == typeof(string) || arg.LimitType == typeof(string))
			{
				return this.BinaryNumericStringOp(target, arg);
			}
			TypeCode typeCode = LanguagePrimitives.GetTypeCode(target.LimitType);
			if (!target.LimitType.IsNumeric())
			{
				return PSBinaryOperationBinder.CallImplicitOp(userOp, target, arg, this.GetOperatorText(), errorSuggestion);
			}
			Type typeFromHandle = typeof(int);
			bool debase;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(arg.Value, typeFromHandle, out debase);
			if (conversionData.Rank == ConversionRank.None)
			{
				return PSConvertBinder.ThrowNoConversion(arg, typeof(int), this, this._version, new DynamicMetaObject[0]);
			}
			Expression expression = PSConvertBinder.InvokeConverter(conversionData, arg.Expression, typeFromHandle, debase, ExpressionCache.InvariantCulture);
			if (typeCode == TypeCode.Decimal || typeCode == TypeCode.Double || typeCode == TypeCode.Single)
			{
				Type type = (typeCode == TypeCode.Decimal) ? typeof(DecimalOps) : typeof(DoubleOps);
				Type type2 = (typeCode == TypeCode.Decimal) ? typeof(decimal) : typeof(double);
				string name = userOp.Substring(3);
				return new DynamicMetaObject(Expression.Call(type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Cast(type2), expression), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			Expression arg2 = target.Expression.Cast(target.LimitType);
			expression = Expression.And(expression, Expression.Constant((typeCode < TypeCode.Int64) ? 31 : 63, typeof(int)));
			return new DynamicMetaObject(exprGenerator(arg2, expression).Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			}));
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x00165604 File Offset: 0x00163804
		private DynamicMetaObject LeftShift(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.Shift(target, arg, errorSuggestion, "op_LeftShift", new Func<Expression, Expression, Expression>(Expression.LeftShift));
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x00165620 File Offset: 0x00163820
		private DynamicMetaObject RightShift(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.Shift(target, arg, errorSuggestion, "op_RightShift", new Func<Expression, Expression, Expression>(Expression.RightShift));
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0016563C File Offset: 0x0016383C
		private DynamicMetaObject BinaryBitwiseXor(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinaryBitwiseOp(target, arg, errorSuggestion, new Func<Expression, Expression, Expression>(Expression.ExclusiveOr), "op_ExclusiveOr", "-bxor", "BXor");
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x00165662 File Offset: 0x00163862
		private DynamicMetaObject BinaryBitwiseOr(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinaryBitwiseOp(target, arg, errorSuggestion, new Func<Expression, Expression, Expression>(Expression.Or), "op_BitwiseOr", "-bor", "BOr");
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x00165688 File Offset: 0x00163888
		private DynamicMetaObject BinaryBitwiseAnd(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			return this.BinaryBitwiseOp(target, arg, errorSuggestion, new Func<Expression, Expression, Expression>(Expression.And), "op_BitwiseAnd", "-band", "BAnd");
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x001656B0 File Offset: 0x001638B0
		private DynamicMetaObject BinaryBitwiseOp(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion, Func<Expression, Expression, Expression> exprGenerator, string implicitMethodName, string errorOperatorName, string methodName)
		{
			if (target.Value == null && arg.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.Constant(0).Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			Type type = target.LimitType.GetTypeInfo().IsEnum ? Enum.GetUnderlyingType(target.LimitType) : target.LimitType;
			Type type2 = arg.LimitType.GetTypeInfo().IsEnum ? Enum.GetUnderlyingType(arg.LimitType) : arg.LimitType;
			if (type.IsNumericOrPrimitive() || type2.IsNumericOrPrimitive())
			{
				TypeCode typeCode = LanguagePrimitives.GetTypeCode(type);
				TypeCode typeCode2 = LanguagePrimitives.GetTypeCode(type2);
				TypeCode typeCode3 = (typeCode >= typeCode2) ? typeCode : typeCode2;
				Type bitwiseOpType;
				DynamicMetaObject dynamicMetaObject;
				DynamicMetaObject dynamicMetaObject2;
				if (!type.IsNumericOrPrimitive())
				{
					bitwiseOpType = PSBinaryOperationBinder.GetBitwiseOpType(typeCode2);
					dynamicMetaObject = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(target, bitwiseOpType);
					dynamicMetaObject2 = arg;
				}
				else if (!type2.IsNumericOrPrimitive())
				{
					bitwiseOpType = PSBinaryOperationBinder.GetBitwiseOpType(typeCode);
					dynamicMetaObject = target;
					dynamicMetaObject2 = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(arg, bitwiseOpType);
				}
				else
				{
					dynamicMetaObject = target;
					dynamicMetaObject2 = arg;
				}
				if (typeCode3 == TypeCode.Decimal)
				{
					Type typeFromHandle = typeof(DecimalOps);
					Type typeFromHandle2 = typeof(decimal);
					return new DynamicMetaObject(Expression.Call(typeFromHandle.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic), dynamicMetaObject.Expression.Cast(dynamicMetaObject.LimitType).Convert(typeFromHandle2), dynamicMetaObject2.Expression.Cast(dynamicMetaObject2.LimitType).Convert(typeFromHandle2)), dynamicMetaObject.CombineRestrictions(new DynamicMetaObject[]
					{
						dynamicMetaObject2
					}));
				}
				if (typeCode3 == TypeCode.Double || typeCode3 == TypeCode.Single)
				{
					Type typeFromHandle = typeof(DoubleOps);
					Type typeFromHandle2 = typeof(double);
					return new DynamicMetaObject(Expression.Call(typeFromHandle.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic), dynamicMetaObject.Expression.Cast(dynamicMetaObject.LimitType).Convert(typeFromHandle2), dynamicMetaObject2.Expression.Cast(dynamicMetaObject2.LimitType).Convert(typeFromHandle2)), dynamicMetaObject.CombineRestrictions(new DynamicMetaObject[]
					{
						dynamicMetaObject2
					}));
				}
				bitwiseOpType = PSBinaryOperationBinder.GetBitwiseOpType((typeCode >= typeCode2) ? typeCode : typeCode2);
				if (dynamicMetaObject != null && dynamicMetaObject2 != null)
				{
					Expression expression = exprGenerator(dynamicMetaObject.Expression.Cast(target.LimitType).Cast(bitwiseOpType), dynamicMetaObject2.Expression.Cast(dynamicMetaObject2.LimitType).Cast(bitwiseOpType)).Cast(typeof(object));
					if (target.LimitType.GetTypeInfo().IsEnum)
					{
						expression = expression.Cast(target.LimitType).Cast(typeof(object));
					}
					return new DynamicMetaObject(expression, dynamicMetaObject.CombineRestrictions(new DynamicMetaObject[]
					{
						dynamicMetaObject2
					}));
				}
			}
			if (target.LimitType == typeof(string) || arg.LimitType == typeof(string))
			{
				return this.BinaryNumericStringOp(target, arg);
			}
			return PSBinaryOperationBinder.CallImplicitOp(implicitMethodName, target, arg, errorOperatorName, errorSuggestion);
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x001659B8 File Offset: 0x00163BB8
		private DynamicMetaObject CompareEQ(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			if (target.Value == null)
			{
				return new DynamicMetaObject((arg.Value == null) ? ExpressionCache.BoxedTrue : ExpressionCache.BoxedFalse, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && arg.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.BoxedFalse, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			return this.BinaryComparisonCommon(dynamicMetaObject, target, arg) ?? this.BinaryEqualityComparison(target, arg);
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x00165A40 File Offset: 0x00163C40
		private DynamicMetaObject CompareNE(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			if (target.Value == null)
			{
				return new DynamicMetaObject((arg.Value == null) ? ExpressionCache.BoxedFalse : ExpressionCache.BoxedTrue, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && arg.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.BoxedTrue, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			return this.BinaryComparisonCommon(dynamicMetaObject, target, arg) ?? this.BinaryEqualityComparison(target, arg);
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x00165AC8 File Offset: 0x00163CC8
		private DynamicMetaObject BinaryEqualityComparison(DynamicMetaObject target, DynamicMetaObject arg)
		{
			Func<Expression, Expression> func;
			if (base.Operation != ExpressionType.NotEqual)
			{
				func = ((Expression e) => e);
			}
			else
			{
				func = new Func<Expression, Expression>(Expression.Not);
			}
			Func<Expression, Expression> func2 = func;
			if (target.LimitType == typeof(string))
			{
				Expression left = target.Expression.Cast(typeof(string));
				Expression right = (!(arg.LimitType == typeof(string))) ? DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), arg.Expression, ExpressionCache.GetExecutionContextFromTLS) : arg.Expression.Cast(typeof(string));
				return new DynamicMetaObject(func2(Compiler.CallStringEquals(left, right, this._ignoreCase)).Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			if (target.LimitType == typeof(char) && this._ignoreCase)
			{
				if (arg.LimitType == typeof(char))
				{
					return new DynamicMetaObject(Expression.Call((base.Operation == ExpressionType.Equal) ? CachedReflectionInfo.CharOps_CompareIeq : CachedReflectionInfo.CharOps_CompareIne, target.Expression.Cast(typeof(char)), arg.Expression.Cast(typeof(char))), target.PSGetTypeRestriction().Merge(arg.PSGetTypeRestriction()));
				}
				if (arg.LimitType == typeof(string))
				{
					return new DynamicMetaObject(Expression.Call((base.Operation == ExpressionType.Equal) ? CachedReflectionInfo.CharOps_CompareStringIeq : CachedReflectionInfo.CharOps_CompareStringIne, target.Expression.Cast(typeof(char)), arg.Expression.Cast(typeof(string))), target.PSGetTypeRestriction().Merge(arg.PSGetTypeRestriction()));
				}
			}
			Expression expression = Expression.Call(target.Expression.Cast(typeof(object)), CachedReflectionInfo.Object_Equals, new Expression[]
			{
				arg.Expression.Cast(typeof(object))
			});
			Type limitType = target.LimitType;
			bool debase;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(arg.Value, limitType, out debase);
			if (conversionData.Rank == ConversionRank.Identity || conversionData.Rank == ConversionRank.Assignable || (conversionData.Rank == ConversionRank.NullToRef && limitType != typeof(PSReference)))
			{
				return new DynamicMetaObject(func2(expression).Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			});
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, limitType, this._version));
			if (conversionData.Rank == ConversionRank.None)
			{
				return new DynamicMetaObject(func2(expression).Cast(typeof(object)), bindingRestrictions);
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(bool));
			Expression right2 = Expression.Call(target.Expression.Cast(typeof(object)), CachedReflectionInfo.Object_Equals, new Expression[]
			{
				PSConvertBinder.InvokeConverter(conversionData, arg.Expression, limitType, debase, ExpressionCache.InvariantCulture).Cast(typeof(object))
			});
			BlockExpression expr = Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, expression),
				Expression.IfThen(Expression.Not(parameterExpression), Expression.TryCatch(Expression.Assign(parameterExpression, right2), new CatchBlock[]
				{
					Expression.Catch(typeof(InvalidCastException), Expression.Assign(parameterExpression, ExpressionCache.Constant(false)))
				})),
				func2(parameterExpression)
			});
			return new DynamicMetaObject(expr.Cast(typeof(object)), bindingRestrictions);
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x00165EEE File Offset: 0x001640EE
		private static Expression CompareWithZero(DynamicMetaObject target, Func<Expression, Expression, Expression> comparer)
		{
			return comparer(target.Expression.Cast(target.LimitType), ExpressionCache.Constant(0).Cast(target.LimitType)).Cast(typeof(object));
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x00165F38 File Offset: 0x00164138
		private DynamicMetaObject CompareLT(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && (target.Value == null || arg.Value == null))
			{
				Expression expression = target.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(target, new Func<Expression, Expression, Expression>(Expression.LessThan)) : (arg.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(arg, new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual)) : ((arg.Value != null) ? ExpressionCache.BoxedTrue : ExpressionCache.BoxedFalse));
				return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject result;
			if ((result = this.BinaryComparisonCommon(dynamicMetaObject, target, arg)) == null)
			{
				result = this.BinaryComparision(target, arg, (Expression e) => Expression.LessThan(e, ExpressionCache.Constant(0)));
			}
			return result;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x00166014 File Offset: 0x00164214
		private DynamicMetaObject CompareLE(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && (target.Value == null || arg.Value == null))
			{
				Expression expression = target.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(target, new Func<Expression, Expression, Expression>(Expression.LessThan)) : (arg.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(arg, new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual)) : ((target.Value != null) ? ExpressionCache.BoxedFalse : ExpressionCache.BoxedTrue));
				return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject result;
			if ((result = this.BinaryComparisonCommon(dynamicMetaObject, target, arg)) == null)
			{
				result = this.BinaryComparision(target, arg, (Expression e) => Expression.LessThanOrEqual(e, ExpressionCache.Constant(0)));
			}
			return result;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x001660F0 File Offset: 0x001642F0
		private DynamicMetaObject CompareGT(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && (target.Value == null || arg.Value == null))
			{
				Expression expression = target.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(target, new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual)) : (arg.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(arg, new Func<Expression, Expression, Expression>(Expression.LessThan)) : ((target.Value != null) ? ExpressionCache.BoxedTrue : ExpressionCache.BoxedFalse));
				return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject result;
			if ((result = this.BinaryComparisonCommon(dynamicMetaObject, target, arg)) == null)
			{
				result = this.BinaryComparision(target, arg, (Expression e) => Expression.GreaterThan(e, ExpressionCache.Constant(0)));
			}
			return result;
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x001661CC File Offset: 0x001643CC
		private DynamicMetaObject CompareGE(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null && (target.Value == null || arg.Value == null))
			{
				Expression expression = target.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(target, new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual)) : (arg.LimitType.IsNumeric() ? PSBinaryOperationBinder.CompareWithZero(arg, new Func<Expression, Expression, Expression>(Expression.LessThan)) : ((arg.Value != null) ? ExpressionCache.BoxedFalse : ExpressionCache.BoxedTrue));
				return new DynamicMetaObject(expression, target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			DynamicMetaObject result;
			if ((result = this.BinaryComparisonCommon(dynamicMetaObject, target, arg)) == null)
			{
				result = this.BinaryComparision(target, arg, (Expression e) => Expression.GreaterThanOrEqual(e, ExpressionCache.Constant(0)));
			}
			return result;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x00166298 File Offset: 0x00164498
		private DynamicMetaObject BinaryComparision(DynamicMetaObject target, DynamicMetaObject arg, Func<Expression, Expression> toResult)
		{
			if (target.LimitType == typeof(string))
			{
				Expression arg2 = target.Expression.Cast(typeof(string));
				Expression arg3 = (!(arg.LimitType == typeof(string))) ? DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), arg.Expression, ExpressionCache.GetExecutionContextFromTLS) : arg.Expression.Cast(typeof(string));
				MethodCallExpression arg4 = Expression.Call(CachedReflectionInfo.StringOps_Compare, arg2, arg3, ExpressionCache.InvariantCulture, this._ignoreCase ? ExpressionCache.CompareOptionsIgnoreCase : ExpressionCache.CompareOptionsNone);
				return new DynamicMetaObject(toResult(arg4).Cast(typeof(object)), target.CombineRestrictions(new DynamicMetaObject[]
				{
					arg
				}));
			}
			Type limitType = target.LimitType;
			bool flag;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(arg.Value, limitType, out flag);
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(new DynamicMetaObject[]
			{
				arg
			});
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, limitType, this._version));
			Expression expr;
			if (conversionData.Rank == ConversionRank.Identity || conversionData.Rank == ConversionRank.Assignable)
			{
				expr = arg.Expression;
			}
			else if (conversionData.Rank == ConversionRank.None)
			{
				Expression arg5 = flag ? Expression.Call(CachedReflectionInfo.PSObject_Base, arg.Expression) : arg.Expression.Cast(typeof(object));
				MethodCallExpression expression = Expression.Call(CachedReflectionInfo.LanguagePrimitives_GetInvalidCastMessages, arg5, Expression.Constant(limitType, typeof(Type)));
				expr = Compiler.ThrowRuntimeError("ComparisonFailure", ExtendedTypeSystem.ComparisonFailure, limitType, new Expression[]
				{
					DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), target.Expression, ExpressionCache.GetExecutionContextFromTLS),
					DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), arg.Expression, ExpressionCache.GetExecutionContextFromTLS),
					Expression.Property(expression, "Item2")
				});
			}
			else
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(InvalidCastException));
				expr = Expression.TryCatch(PSConvertBinder.InvokeConverter(conversionData, arg.Expression, limitType, flag, ExpressionCache.InvariantCulture), new CatchBlock[]
				{
					Expression.Catch(parameterExpression, Compiler.ThrowRuntimeErrorWithInnerException("ComparisonFailure", Expression.Constant(ExtendedTypeSystem.ComparisonFailure), parameterExpression, limitType, new Expression[]
					{
						DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), target.Expression, ExpressionCache.GetExecutionContextFromTLS),
						DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), arg.Expression, ExpressionCache.GetExecutionContextFromTLS),
						Expression.Property(parameterExpression, CachedReflectionInfo.Exception_Message)
					}))
				});
			}
			if (target.LimitType == arg.LimitType)
			{
				foreach (Type type in target.Value.GetType().GetInterfaces())
				{
					if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IComparable<>))
					{
						return new DynamicMetaObject(toResult(Expression.Call(Expression.Convert(target.Expression, type), type.GetMethod("CompareTo"), new Expression[]
						{
							expr.Cast(arg.LimitType)
						})).Cast(typeof(object)), bindingRestrictions);
					}
				}
			}
			if (target.Value is IComparable)
			{
				return new DynamicMetaObject(toResult(Expression.Call(target.Expression.Cast(typeof(IComparable)), CachedReflectionInfo.IComparable_CompareTo, new Expression[]
				{
					expr.Cast(typeof(object))
				})).Cast(typeof(object)), bindingRestrictions);
			}
			Expression ifFalse = Compiler.ThrowRuntimeError("NotIcomparable", ExtendedTypeSystem.NotIcomparable, this.ReturnType, new Expression[]
			{
				target.Expression
			});
			return new DynamicMetaObject(Expression.Condition(Expression.Call(target.Expression.Cast(typeof(object)), CachedReflectionInfo.Object_Equals, new Expression[]
			{
				arg.Expression.Cast(typeof(object))
			}), (base.Operation == ExpressionType.GreaterThanOrEqual || base.Operation == ExpressionType.LessThanOrEqual) ? ExpressionCache.BoxedTrue : ExpressionCache.BoxedFalse, ifFalse), bindingRestrictions);
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x00166744 File Offset: 0x00164944
		private DynamicMetaObject BinaryComparisonCommon(DynamicMetaObject targetAsEnumerator, DynamicMetaObject target, DynamicMetaObject arg)
		{
			if (targetAsEnumerator != null && !this._scalarCompare)
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_Compare, targetAsEnumerator.Expression, arg.Expression.Cast(typeof(object)), Expression.Constant(this.GetScalarCompareDelegate())), targetAsEnumerator.Restrictions.Merge(arg.PSGetTypeRestriction()));
			}
			if (target.LimitType.IsNumeric())
			{
				DynamicMetaObject argAsNumericOrPrimitive = PSBinaryOperationBinder.GetArgAsNumericOrPrimitive(arg, target.LimitType);
				if (argAsNumericOrPrimitive != null)
				{
					string methodName = null;
					ExpressionType operation = base.Operation;
					switch (operation)
					{
					case ExpressionType.Equal:
						methodName = "CompareEq";
						break;
					case ExpressionType.ExclusiveOr:
					case ExpressionType.Invoke:
					case ExpressionType.Lambda:
					case ExpressionType.LeftShift:
						break;
					case ExpressionType.GreaterThan:
						methodName = "CompareGt";
						break;
					case ExpressionType.GreaterThanOrEqual:
						methodName = "CompareGe";
						break;
					case ExpressionType.LessThan:
						methodName = "CompareLt";
						break;
					case ExpressionType.LessThanOrEqual:
						methodName = "CompareLe";
						break;
					default:
						if (operation == ExpressionType.NotEqual)
						{
							methodName = "CompareNe";
						}
						break;
					}
					return this.BinaryNumericOp(methodName, target, argAsNumericOrPrimitive);
				}
				if (arg.LimitType == typeof(string))
				{
					return this.BinaryNumericStringOp(target, arg);
				}
			}
			return null;
		}

		// Token: 0x040021B1 RID: 8625
		private static readonly Dictionary<Tuple<ExpressionType, bool, bool>, PSBinaryOperationBinder> _binderCache = new Dictionary<Tuple<ExpressionType, bool, bool>, PSBinaryOperationBinder>();

		// Token: 0x040021B2 RID: 8626
		private readonly bool _ignoreCase;

		// Token: 0x040021B3 RID: 8627
		private readonly bool _scalarCompare;

		// Token: 0x040021B4 RID: 8628
		internal int _version;

		// Token: 0x040021B5 RID: 8629
		private Func<object, object, bool> _compareDelegate;
	}
}
