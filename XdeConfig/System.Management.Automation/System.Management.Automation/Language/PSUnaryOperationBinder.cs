using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x02000616 RID: 1558
	internal class PSUnaryOperationBinder : UnaryOperationBinder
	{
		// Token: 0x060043C4 RID: 17348 RVA: 0x00166868 File Offset: 0x00164A68
		internal static PSUnaryOperationBinder Get(ExpressionType operation)
		{
			if (operation <= ExpressionType.Not)
			{
				switch (operation)
				{
				case ExpressionType.Negate:
					if (PSUnaryOperationBinder._unaryMinus == null)
					{
						Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._unaryMinus, new PSUnaryOperationBinder(operation), null);
					}
					return PSUnaryOperationBinder._unaryMinus;
				case ExpressionType.UnaryPlus:
					if (PSUnaryOperationBinder._unaryPlusBinder == null)
					{
						Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._unaryPlusBinder, new PSUnaryOperationBinder(operation), null);
					}
					return PSUnaryOperationBinder._unaryPlusBinder;
				default:
					if (operation == ExpressionType.Not)
					{
						if (PSUnaryOperationBinder._notBinder == null)
						{
							Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._notBinder, new PSUnaryOperationBinder(operation), null);
						}
						return PSUnaryOperationBinder._notBinder;
					}
					break;
				}
			}
			else
			{
				if (operation == ExpressionType.Decrement)
				{
					if (PSUnaryOperationBinder._decrementBinder == null)
					{
						Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._decrementBinder, new PSUnaryOperationBinder(operation), null);
					}
					return PSUnaryOperationBinder._decrementBinder;
				}
				if (operation == ExpressionType.Increment)
				{
					if (PSUnaryOperationBinder._incrementBinder == null)
					{
						Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._incrementBinder, new PSUnaryOperationBinder(operation), null);
					}
					return PSUnaryOperationBinder._incrementBinder;
				}
				if (operation == ExpressionType.OnesComplement)
				{
					if (PSUnaryOperationBinder._bnotBinder == null)
					{
						Interlocked.CompareExchange<PSUnaryOperationBinder>(ref PSUnaryOperationBinder._bnotBinder, new PSUnaryOperationBinder(operation), null);
					}
					return PSUnaryOperationBinder._bnotBinder;
				}
			}
			throw new NotImplementedException("Unimplemented unary operation");
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x00166975 File Offset: 0x00164B75
		private PSUnaryOperationBinder(ExpressionType operation) : base(operation)
		{
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x00166980 File Offset: 0x00164B80
		public override DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				});
			}
			ExpressionType operation = base.Operation;
			if (operation <= ExpressionType.Not)
			{
				switch (operation)
				{
				case ExpressionType.Negate:
					return this.UnaryMinus(target, errorSuggestion).WriteToDebugLog(this);
				case ExpressionType.UnaryPlus:
					return this.UnaryPlus(target, errorSuggestion).WriteToDebugLog(this);
				default:
					if (operation == ExpressionType.Not)
					{
						return this.Not(target, errorSuggestion).WriteToDebugLog(this);
					}
					break;
				}
			}
			else
			{
				if (operation == ExpressionType.Decrement)
				{
					return this.IncrDecr(target, -1, errorSuggestion).WriteToDebugLog(this);
				}
				if (operation == ExpressionType.Increment)
				{
					return this.IncrDecr(target, 1, errorSuggestion).WriteToDebugLog(this);
				}
				if (operation == ExpressionType.OnesComplement)
				{
					return this.BNot(target, errorSuggestion).WriteToDebugLog(this);
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x00166A6C File Offset: 0x00164C6C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSUnaryOperationBinder {0}", new object[]
			{
				base.Operation
			});
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x00166AA0 File Offset: 0x00164CA0
		internal DynamicMetaObject Not(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			Expression expression = target.CastOrConvert(typeof(bool));
			return new DynamicMetaObject(Expression.Not(expression).Cast(typeof(object)), target.PSGetTypeRestriction());
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00166AF4 File Offset: 0x00164CF4
		internal DynamicMetaObject BNot(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				});
			}
			if (target.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.Constant(-1).Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			MethodInfo method = target.LimitType.GetMethod("op_OnesComplement", BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				target.LimitType
			}, null);
			if (method != null)
			{
				return new DynamicMetaObject(Expression.OnesComplement(target.Expression.Cast(target.LimitType), method).Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			if (target.LimitType == typeof(string))
			{
				return new DynamicMetaObject(DynamicExpression.Dynamic(this, this.ReturnType, PSBinaryOperationBinder.ConvertStringToNumber(target.Expression, typeof(int))), target.PSGetTypeRestriction());
			}
			Expression expression = null;
			if (!target.LimitType.IsNumeric())
			{
				Type typeFromHandle = typeof(int);
				bool debase;
				LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(target.Value, typeFromHandle, out debase);
				if (conversionData.Rank != ConversionRank.None)
				{
					expression = PSConvertBinder.InvokeConverter(conversionData, target.Expression, typeFromHandle, debase, ExpressionCache.InvariantCulture);
				}
				else
				{
					typeFromHandle = typeof(long);
					conversionData = LanguagePrimitives.FigureConversion(target.Value, typeFromHandle, out debase);
					if (conversionData.Rank != ConversionRank.None)
					{
						expression = PSConvertBinder.InvokeConverter(conversionData, target.Expression, typeFromHandle, debase, ExpressionCache.InvariantCulture);
					}
				}
			}
			else
			{
				TypeCode typeCode = LanguagePrimitives.GetTypeCode(target.LimitType);
				if (typeCode < TypeCode.Int32)
				{
					expression = (target.LimitType.GetTypeInfo().IsEnum ? target.Expression.Cast(Enum.GetUnderlyingType(target.LimitType)) : target.Expression.Cast(target.LimitType));
					expression = expression.Cast(typeof(int));
				}
				else
				{
					if (typeCode > TypeCode.UInt64)
					{
						Type type = (typeCode == TypeCode.Decimal) ? typeof(DecimalOps) : typeof(DoubleOps);
						Type type2 = (typeCode == TypeCode.Decimal) ? typeof(decimal) : typeof(double);
						return new DynamicMetaObject(Expression.Call(type.GetMethod("BNot", BindingFlags.Static | BindingFlags.NonPublic), target.Expression.Convert(type2)), target.PSGetTypeRestriction());
					}
					Type type3 = target.LimitType;
					if (type3.GetTypeInfo().IsEnum)
					{
						type3 = Enum.GetUnderlyingType(type3);
					}
					expression = target.Expression.Cast(type3);
				}
			}
			if (expression != null)
			{
				Expression expr = Expression.OnesComplement(expression);
				if (target.LimitType.GetTypeInfo().IsEnum)
				{
					expr = expr.Cast(target.LimitType);
				}
				return new DynamicMetaObject(expr.Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(target, typeof(int), this, -1, new DynamicMetaObject[0]);
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x00166E14 File Offset: 0x00165014
		private DynamicMetaObject UnaryPlus(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				});
			}
			if (target.LimitType.IsNumeric())
			{
				Expression expression = target.Expression.Cast(target.LimitType);
				if (target.LimitType == typeof(byte) || target.LimitType == typeof(sbyte))
				{
					expression = expression.Cast(typeof(int));
				}
				return new DynamicMetaObject(Expression.UnaryPlus(expression).Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			return new DynamicMetaObject(DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(ExpressionType.Add, true, false), typeof(object), ExpressionCache.Constant(0), target.Expression), target.PSGetTypeRestriction());
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x00166F18 File Offset: 0x00165118
		private DynamicMetaObject UnaryMinus(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				});
			}
			if (target.LimitType.IsNumeric())
			{
				Expression expression = target.Expression.Cast(target.LimitType);
				if (target.LimitType == typeof(byte) || target.LimitType == typeof(sbyte))
				{
					expression = expression.Cast(typeof(int));
				}
				return new DynamicMetaObject(Expression.Negate(expression).Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			return new DynamicMetaObject(DynamicExpression.Dynamic(PSBinaryOperationBinder.Get(ExpressionType.Subtract, true, false), typeof(object), ExpressionCache.Constant(0), target.Expression), target.PSGetTypeRestriction());
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0016701C File Offset: 0x0016521C
		private DynamicMetaObject IncrDecr(DynamicMetaObject target, int valueToAdd, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				});
			}
			if (target.Value == null)
			{
				return new DynamicMetaObject(ExpressionCache.Constant(valueToAdd).Cast(typeof(object)), target.PSGetTypeRestriction());
			}
			if (target.LimitType.IsNumeric())
			{
				DynamicMetaObject arg = new DynamicMetaObject(ExpressionCache.Constant(valueToAdd), BindingRestrictions.Empty, valueToAdd);
				DynamicMetaObject dynamicMetaObject = PSBinaryOperationBinder.Get(ExpressionType.Add, true, false).FallbackBinaryOperation(target, arg, errorSuggestion);
				return new DynamicMetaObject(dynamicMetaObject.Expression, target.PSGetTypeRestriction());
			}
			return errorSuggestion ?? target.ThrowRuntimeError(new DynamicMetaObject[0], BindingRestrictions.Empty, "OperatorRequiresNumber", ParserStrings.OperatorRequiresNumber, new Expression[]
			{
				Expression.Constant(((base.Operation == ExpressionType.Increment) ? TokenKind.PlusPlus : TokenKind.MinusMinus).Text()),
				Expression.Constant(target.LimitType, typeof(Type))
			});
		}

		// Token: 0x040021BB RID: 8635
		private static PSUnaryOperationBinder _notBinder;

		// Token: 0x040021BC RID: 8636
		private static PSUnaryOperationBinder _bnotBinder;

		// Token: 0x040021BD RID: 8637
		private static PSUnaryOperationBinder _unaryMinus;

		// Token: 0x040021BE RID: 8638
		private static PSUnaryOperationBinder _unaryPlusBinder;

		// Token: 0x040021BF RID: 8639
		private static PSUnaryOperationBinder _incrementBinder;

		// Token: 0x040021C0 RID: 8640
		private static PSUnaryOperationBinder _decrementBinder;
	}
}
