using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Language;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A80 RID: 2688
	internal class DispCallableMetaObject : DynamicMetaObject
	{
		// Token: 0x06006AD5 RID: 27349 RVA: 0x002183E1 File Offset: 0x002165E1
		internal DispCallableMetaObject(Expression expression, DispCallable callable) : base(expression, BindingRestrictions.Empty, callable)
		{
			this._callable = callable;
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x002183F7 File Offset: 0x002165F7
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			return this.BindGetOrInvoke(indexes, binder.CallInfo) ?? base.BindGetIndex(binder, indexes);
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x00218412 File Offset: 0x00216612
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			return this.BindGetOrInvoke(args, binder.CallInfo) ?? base.BindInvoke(binder, args);
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x00218430 File Offset: 0x00216630
		private DynamicMetaObject BindGetOrInvoke(DynamicMetaObject[] args, CallInfo callInfo)
		{
			IDispatchComObject dispatchComObject = this._callable.DispatchComObject;
			string memberName = this._callable.MemberName;
			ComMethodDesc method;
			if (dispatchComObject.TryGetMemberMethod(memberName, out method) || dispatchComObject.TryGetMemberMethodExplicit(memberName, out method))
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] isByRef = ComBinderHelpers.ProcessArgumentsForCom(method, ref args, temps, initTemps);
				return this.BindComInvoke(method, args, callInfo, isByRef, temps, initTemps);
			}
			return null;
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x00218498 File Offset: 0x00216698
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			IDispatchComObject dispatchComObject = this._callable.DispatchComObject;
			string memberName = this._callable.MemberName;
			bool holdsNull = value.Value == null && value.HasValue;
			ComMethodDesc method;
			if (dispatchComObject.TryGetPropertySetter(memberName, out method, value.LimitType, holdsNull) || dispatchComObject.TryGetPropertySetterExplicit(memberName, out method, value.LimitType, holdsNull))
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] array = ComBinderHelpers.ProcessArgumentsForCom(method, ref indexes, temps, initTemps);
				array = array.AddLast(false);
				DynamicMetaObject item = new DynamicMetaObject(value.CastOrConvertMethodArgument(value.LimitType, memberName, "SetIndex", temps, initTemps), value.Restrictions);
				DynamicMetaObject dynamicMetaObject = this.BindComInvoke(method, indexes.AddLast(item), binder.CallInfo, array, temps, initTemps);
				return new DynamicMetaObject(Expression.Block(dynamicMetaObject.Expression, Expression.Convert(value.Expression, typeof(object))), dynamicMetaObject.Restrictions);
			}
			return base.BindSetIndex(binder, indexes, value);
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x00218594 File Offset: 0x00216794
		private DynamicMetaObject BindComInvoke(ComMethodDesc method, DynamicMetaObject[] indexes, CallInfo callInfo, bool[] isByRef, List<ParameterExpression> temps, List<Expression> initTemps)
		{
			Expression expression = base.Expression;
			Expression expression2 = Helpers.Convert(expression, typeof(DispCallable));
			DynamicMetaObject dynamicMetaObject = new ComInvokeBinder(callInfo, indexes, isByRef, this.DispCallableRestrictions(), Expression.Constant(method), Expression.Property(expression2, typeof(DispCallable).GetProperty("DispatchObject")), method).Invoke();
			if (temps != null && temps.Any<ParameterExpression>())
			{
				Expression expression3 = dynamicMetaObject.Expression;
				Expression expression4 = Expression.Block(expression3.Type, temps, initTemps.Append(expression3));
				dynamicMetaObject = new DynamicMetaObject(expression4, dynamicMetaObject.Restrictions);
			}
			return dynamicMetaObject;
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x0021862C File Offset: 0x0021682C
		private BindingRestrictions DispCallableRestrictions()
		{
			Expression expression = base.Expression;
			BindingRestrictions typeRestriction = BindingRestrictions.GetTypeRestriction(expression, typeof(DispCallable));
			Expression expression2 = Helpers.Convert(expression, typeof(DispCallable));
			MemberExpression expr = Expression.Property(expression2, typeof(DispCallable).GetProperty("DispatchComObject"));
			MemberExpression left = Expression.Property(expression2, typeof(DispCallable).GetProperty("DispId"));
			BindingRestrictions restrictions = IDispatchMetaObject.IDispatchRestriction(expr, this._callable.DispatchComObject.ComTypeDesc);
			BindingRestrictions expressionRestriction = BindingRestrictions.GetExpressionRestriction(Expression.Equal(left, Expression.Constant(this._callable.DispId)));
			return typeRestriction.Merge(restrictions).Merge(expressionRestriction);
		}

		// Token: 0x04003329 RID: 13097
		private readonly DispCallable _callable;
	}
}
