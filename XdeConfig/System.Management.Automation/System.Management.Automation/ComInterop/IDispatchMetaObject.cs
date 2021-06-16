using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A87 RID: 2695
	internal sealed class IDispatchMetaObject : ComFallbackMetaObject
	{
		// Token: 0x06006B19 RID: 27417 RVA: 0x0021971E File Offset: 0x0021791E
		internal IDispatchMetaObject(Expression expression, IDispatchComObject self) : base(expression, BindingRestrictions.Empty, self)
		{
			this._self = self;
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x00219734 File Offset: 0x00217934
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			ComMethodDesc comMethodDesc = null;
			ComBinder.ComInvokeMemberBinder comInvokeMemberBinder = binder as ComBinder.ComInvokeMemberBinder;
			if (comInvokeMemberBinder != null && comInvokeMemberBinder.IsPropertySet)
			{
				DynamicMetaObject dynamicMetaObject = args[args.Length - 1];
				bool holdsNull = dynamicMetaObject.Value == null && dynamicMetaObject.HasValue;
				if (!this._self.TryGetPropertySetter(binder.Name, out comMethodDesc, dynamicMetaObject.LimitType, holdsNull))
				{
					this._self.TryGetPropertySetterExplicit(binder.Name, out comMethodDesc, dynamicMetaObject.LimitType, holdsNull);
				}
			}
			if (comMethodDesc == null && !this._self.TryGetMemberMethod(binder.Name, out comMethodDesc))
			{
				this._self.TryGetMemberMethodExplicit(binder.Name, out comMethodDesc);
			}
			if (comMethodDesc != null)
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] isByRef = ComBinderHelpers.ProcessArgumentsForCom(comMethodDesc, ref args, temps, initTemps);
				return this.BindComInvoke(args, comMethodDesc, binder.CallInfo, isByRef, temps, initTemps);
			}
			return base.BindInvokeMember(binder, args);
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x00219810 File Offset: 0x00217A10
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			ComMethodDesc method;
			if (this._self.TryGetGetItem(out method))
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] isByRef = ComBinderHelpers.ProcessArgumentsForCom(method, ref args, temps, initTemps);
				return this.BindComInvoke(args, method, binder.CallInfo, isByRef, temps, initTemps);
			}
			return base.BindInvoke(binder, args);
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x00219860 File Offset: 0x00217A60
		private DynamicMetaObject BindComInvoke(DynamicMetaObject[] args, ComMethodDesc method, CallInfo callInfo, bool[] isByRef, List<ParameterExpression> temps, List<Expression> initTemps)
		{
			DynamicMetaObject dynamicMetaObject = new ComInvokeBinder(callInfo, args, isByRef, this.IDispatchRestriction(), Expression.Constant(method), Expression.Property(Helpers.Convert(base.Expression, typeof(IDispatchComObject)), typeof(IDispatchComObject).GetProperty("DispatchObject")), method).Invoke();
			if (temps != null && temps.Any<ParameterExpression>())
			{
				Expression expression = dynamicMetaObject.Expression;
				Expression expression2 = Expression.Block(expression.Type, temps, initTemps.Append(expression));
				dynamicMetaObject = new DynamicMetaObject(expression2, dynamicMetaObject.Restrictions);
			}
			return dynamicMetaObject;
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x002198F0 File Offset: 0x00217AF0
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			ComBinder.ComGetMemberBinder comGetMemberBinder = binder as ComBinder.ComGetMemberBinder;
			bool canReturnCallables = comGetMemberBinder != null && comGetMemberBinder._CanReturnCallables;
			ComMethodDesc comMethodDesc;
			if (this._self.TryGetMemberMethod(binder.Name, out comMethodDesc) && (comMethodDesc.InvokeKind & INVOKEKIND.INVOKE_PROPERTYGET) == INVOKEKIND.INVOKE_PROPERTYGET && comMethodDesc.ParamCount == 0)
			{
				return this.BindGetMember(comMethodDesc, canReturnCallables);
			}
			ComEventDesc @event;
			if (this._self.TryGetMemberEvent(binder.Name, out @event))
			{
				return this.BindEvent(@event);
			}
			if (this._self.TryGetMemberMethodExplicit(binder.Name, out comMethodDesc) && (comMethodDesc.InvokeKind & INVOKEKIND.INVOKE_PROPERTYGET) == INVOKEKIND.INVOKE_PROPERTYGET && comMethodDesc.ParamCount == 0)
			{
				return this.BindGetMember(comMethodDesc, canReturnCallables);
			}
			return base.BindGetMember(binder);
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x00219998 File Offset: 0x00217B98
		private DynamicMetaObject BindGetMember(ComMethodDesc method, bool canReturnCallables)
		{
			if (method.IsDataMember && method.ParamCount == 0)
			{
				return this.BindComInvoke(DynamicMetaObject.EmptyMetaObjects, method, new CallInfo(0, new string[0]), new bool[0], null, null);
			}
			if (!canReturnCallables)
			{
				return this.BindComInvoke(DynamicMetaObject.EmptyMetaObjects, method, new CallInfo(0, new string[0]), new bool[0], null, null);
			}
			return new DynamicMetaObject(Expression.Call(typeof(ComRuntimeHelpers).GetMethod("CreateDispCallable"), Helpers.Convert(base.Expression, typeof(IDispatchComObject)), Expression.Constant(method)), this.IDispatchRestriction());
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x00219A3C File Offset: 0x00217C3C
		private DynamicMetaObject BindEvent(ComEventDesc @event)
		{
			Expression expression = Expression.Call(typeof(ComRuntimeHelpers).GetMethod("CreateComEvent"), ComObject.RcwFromComObject(base.Expression), Expression.Constant(@event.sourceIID), Expression.Constant(@event.dispid));
			return new DynamicMetaObject(expression, this.IDispatchRestriction());
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x00219A9C File Offset: 0x00217C9C
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			ComMethodDesc method;
			if (this._self.TryGetGetItem(out method))
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] isByRef = ComBinderHelpers.ProcessArgumentsForCom(method, ref indexes, temps, initTemps);
				return this.BindComInvoke(indexes, method, binder.CallInfo, isByRef, temps, initTemps);
			}
			return base.BindGetIndex(binder, indexes);
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x00219AEC File Offset: 0x00217CEC
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			ComMethodDesc comMethodDesc;
			if (this._self.TryGetSetItem(out comMethodDesc))
			{
				List<ParameterExpression> temps = new List<ParameterExpression>();
				List<Expression> initTemps = new List<Expression>();
				bool[] array = ComBinderHelpers.ProcessArgumentsForCom(comMethodDesc, ref indexes, temps, initTemps);
				array = array.AddLast(false);
				DynamicMetaObject item = new DynamicMetaObject(value.CastOrConvertMethodArgument(value.LimitType, comMethodDesc.Name, "SetIndex", temps, initTemps), value.Restrictions);
				DynamicMetaObject dynamicMetaObject = this.BindComInvoke(indexes.AddLast(item), comMethodDesc, binder.CallInfo, array, temps, initTemps);
				return new DynamicMetaObject(Expression.Block(dynamicMetaObject.Expression, Expression.Convert(value.Expression, typeof(object))), dynamicMetaObject.Restrictions);
			}
			return base.BindSetIndex(binder, indexes, value);
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x00219BA1 File Offset: 0x00217DA1
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			DynamicMetaObject result;
			if ((result = this.TryPropertyPut(binder, value)) == null)
			{
				result = (this.TryEventHandlerNoop(binder, value) ?? base.BindSetMember(binder, value));
			}
			return result;
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x00219BC4 File Offset: 0x00217DC4
		private DynamicMetaObject TryPropertyPut(SetMemberBinder binder, DynamicMetaObject value)
		{
			bool holdsNull = value.Value == null && value.HasValue;
			ComMethodDesc comMethodDesc;
			if (this._self.TryGetPropertySetter(binder.Name, out comMethodDesc, value.LimitType, holdsNull) || this._self.TryGetPropertySetterExplicit(binder.Name, out comMethodDesc, value.LimitType, holdsNull))
			{
				BindingRestrictions restrictions = this.IDispatchRestriction();
				Expression dispatch = Expression.Property(Helpers.Convert(base.Expression, typeof(IDispatchComObject)), typeof(IDispatchComObject).GetProperty("DispatchObject"));
				CallInfo callInfo = new CallInfo(1, new string[0]);
				DynamicMetaObject[] args = new DynamicMetaObject[]
				{
					value
				};
				bool[] isByRef = new bool[1];
				DynamicMetaObject dynamicMetaObject = new ComInvokeBinder(callInfo, args, isByRef, restrictions, Expression.Constant(comMethodDesc), dispatch, comMethodDesc).Invoke();
				return new DynamicMetaObject(Expression.Block(dynamicMetaObject.Expression, Expression.Convert(value.Expression, typeof(object))), dynamicMetaObject.Restrictions);
			}
			return null;
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x00219CC0 File Offset: 0x00217EC0
		private DynamicMetaObject TryEventHandlerNoop(SetMemberBinder binder, DynamicMetaObject value)
		{
			ComEventDesc comEventDesc;
			if (this._self.TryGetMemberEvent(binder.Name, out comEventDesc) && value.LimitType == typeof(BoundDispEvent))
			{
				return new DynamicMetaObject(Expression.Constant(null), value.Restrictions.Merge(this.IDispatchRestriction()).Merge(BindingRestrictions.GetTypeRestriction(value.Expression, typeof(BoundDispEvent))));
			}
			return null;
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x00219D31 File Offset: 0x00217F31
		private BindingRestrictions IDispatchRestriction()
		{
			return IDispatchMetaObject.IDispatchRestriction(base.Expression, this._self.ComTypeDesc);
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x00219D4C File Offset: 0x00217F4C
		internal static BindingRestrictions IDispatchRestriction(Expression expr, ComTypeDesc typeDesc)
		{
			return BindingRestrictions.GetTypeRestriction(expr, typeof(IDispatchComObject)).Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Property(Helpers.Convert(expr, typeof(IDispatchComObject)), typeof(IDispatchComObject).GetProperty("ComTypeDesc")), Expression.Constant(typeDesc))));
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x00219DA7 File Offset: 0x00217FA7
		protected override ComUnwrappedMetaObject UnwrapSelf()
		{
			return new ComUnwrappedMetaObject(ComObject.RcwFromComObject(base.Expression), this.IDispatchRestriction(), this._self.RuntimeCallableWrapper);
		}

		// Token: 0x04003336 RID: 13110
		private readonly IDispatchComObject _self;
	}
}
