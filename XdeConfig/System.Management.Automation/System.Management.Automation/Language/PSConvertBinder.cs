using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;
using Microsoft.PowerShell;

namespace System.Management.Automation.Language
{
	// Token: 0x02000617 RID: 1559
	internal class PSConvertBinder : ConvertBinder
	{
		// Token: 0x060043CD RID: 17357 RVA: 0x00167140 File Offset: 0x00165340
		public static PSConvertBinder Get(Type type)
		{
			PSConvertBinder psconvertBinder;
			lock (PSConvertBinder._binderCache)
			{
				if (!PSConvertBinder._binderCache.TryGetValue(type, out psconvertBinder))
				{
					psconvertBinder = new PSConvertBinder(type);
					PSConvertBinder._binderCache.Add(type, psconvertBinder);
				}
			}
			return psconvertBinder;
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0016719C File Offset: 0x0016539C
		private PSConvertBinder(Type type) : base(type, false)
		{
			this._version = 0;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x001671B0 File Offset: 0x001653B0
		public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]).WriteToDebugLog(this);
			}
			if (target.Value == AutomationNull.Value)
			{
				return new DynamicMetaObject(Expression.Default(base.Type), target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			Type type = base.Type;
			bool debase;
			LanguagePrimitives.ConversionData conversion = LanguagePrimitives.FigureConversion(target.Value, type, out debase);
			if (errorSuggestion != null && target.Value is DynamicObject)
			{
				return errorSuggestion.WriteToDebugLog(this);
			}
			BindingRestrictions bindingRestrictions = target.PSGetTypeRestriction();
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, type, this._version));
			return new DynamicMetaObject(PSConvertBinder.InvokeConverter(conversion, target.Expression, type, debase, ExpressionCache.InvariantCulture), bindingRestrictions).WriteToDebugLog(this);
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0016726C File Offset: 0x0016546C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSConvertBinder [{0}]  ver:{1}", new object[]
			{
				ToStringCodeMethods.Type(base.Type, true),
				this._version
			});
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x001672B0 File Offset: 0x001654B0
		internal static void InvalidateCache()
		{
			lock (PSConvertBinder._binderCache)
			{
				foreach (PSConvertBinder psconvertBinder in PSConvertBinder._binderCache.Values)
				{
					psconvertBinder._version++;
				}
			}
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x00167338 File Offset: 0x00165538
		internal static DynamicMetaObject ThrowNoConversion(DynamicMetaObject target, Type toType, DynamicMetaObjectBinder binder, int currentVersion, params DynamicMetaObject[] args)
		{
			Expression expression = Expression.Call(CachedReflectionInfo.LanguagePrimitives_ThrowInvalidCastException, target.Expression.Cast(typeof(object)), Expression.Constant(toType, typeof(Type)));
			if (!(binder.ReturnType == typeof(void)))
			{
				expression = Expression.Block(expression, Expression.Default(binder.ReturnType));
			}
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(args);
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(binder, toType, currentVersion));
			return new DynamicMetaObject(expression, bindingRestrictions);
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x001673C0 File Offset: 0x001655C0
		internal static Expression InvokeConverter(LanguagePrimitives.ConversionData conversion, Expression value, Type resultType, bool debase, Expression formatProvider)
		{
			Expression expression;
			if (conversion.Rank == ConversionRank.Identity || conversion.Rank == ConversionRank.Assignable)
			{
				expression = (debase ? Expression.Call(CachedReflectionInfo.PSObject_Base, value) : value);
			}
			else
			{
				Expression expression2;
				Expression expression3;
				if (debase)
				{
					expression2 = Expression.Call(CachedReflectionInfo.PSObject_Base, value);
					expression3 = value.Cast(typeof(PSObject));
				}
				else
				{
					expression2 = value.Cast(typeof(object));
					expression3 = ExpressionCache.NullPSObject;
				}
				expression = Expression.Call(Expression.Constant(conversion.Converter), conversion.Converter.GetType().GetMethod("Invoke"), new Expression[]
				{
					expression2,
					Expression.Constant(resultType, typeof(Type)),
					ExpressionCache.Constant(true),
					expression3,
					formatProvider,
					ExpressionCache.NullTypeTable
				});
			}
			if (expression.Type == resultType || resultType == typeof(LanguagePrimitives.InternalPSCustomObject))
			{
				return expression;
			}
			if (resultType.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(resultType) == null)
			{
				return Expression.Unbox(expression, resultType);
			}
			return Expression.Convert(expression, resultType);
		}

		// Token: 0x040021C1 RID: 8641
		private static readonly Dictionary<Type, PSConvertBinder> _binderCache = new Dictionary<Type, PSConvertBinder>();

		// Token: 0x040021C2 RID: 8642
		internal int _version;
	}
}
