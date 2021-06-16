using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A66 RID: 2662
	internal sealed class ComInvokeBinder
	{
		// Token: 0x060069FC RID: 27132 RVA: 0x00215238 File Offset: 0x00213438
		internal ComInvokeBinder(CallInfo callInfo, DynamicMetaObject[] args, bool[] isByRef, BindingRestrictions restrictions, Expression method, Expression dispatch, ComMethodDesc methodDesc)
		{
			this._method = method;
			this._dispatch = dispatch;
			this._methodDesc = methodDesc;
			this._callInfo = callInfo;
			this._args = args;
			this._isByRef = isByRef;
			this._restrictions = restrictions;
			this._instance = dispatch;
		}

		// Token: 0x17001D95 RID: 7573
		// (get) Token: 0x060069FD RID: 27133 RVA: 0x00215288 File Offset: 0x00213488
		private ParameterExpression DispatchObjectVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._dispatchObject, typeof(IDispatch), "dispatchObject");
			}
		}

		// Token: 0x17001D96 RID: 7574
		// (get) Token: 0x060069FE RID: 27134 RVA: 0x002152A4 File Offset: 0x002134A4
		private ParameterExpression DispatchPointerVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._dispatchPointer, typeof(IntPtr), "dispatchPointer");
			}
		}

		// Token: 0x17001D97 RID: 7575
		// (get) Token: 0x060069FF RID: 27135 RVA: 0x002152C0 File Offset: 0x002134C0
		private ParameterExpression DispIdVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._dispId, typeof(int), "dispId");
			}
		}

		// Token: 0x17001D98 RID: 7576
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x002152DC File Offset: 0x002134DC
		private ParameterExpression DispParamsVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._dispParams, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS), "dispParams");
			}
		}

		// Token: 0x17001D99 RID: 7577
		// (get) Token: 0x06006A01 RID: 27137 RVA: 0x002152F8 File Offset: 0x002134F8
		private ParameterExpression InvokeResultVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._invokeResult, typeof(Variant), "invokeResult");
			}
		}

		// Token: 0x17001D9A RID: 7578
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x00215314 File Offset: 0x00213514
		private ParameterExpression ReturnValueVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._returnValue, typeof(object), "returnValue");
			}
		}

		// Token: 0x17001D9B RID: 7579
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x00215330 File Offset: 0x00213530
		private ParameterExpression DispIdsOfKeywordArgsPinnedVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._dispIdsOfKeywordArgsPinned, typeof(GCHandle), "dispIdsOfKeywordArgsPinned");
			}
		}

		// Token: 0x17001D9C RID: 7580
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x0021534C File Offset: 0x0021354C
		private ParameterExpression PropertyPutDispIdVariable
		{
			get
			{
				return ComInvokeBinder.EnsureVariable(ref this._propertyPutDispId, typeof(int), "propertyPutDispId");
			}
		}

		// Token: 0x17001D9D RID: 7581
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x00215368 File Offset: 0x00213568
		private ParameterExpression ParamVariantsVariable
		{
			get
			{
				if (this._paramVariants == null)
				{
					this._paramVariants = Expression.Variable(VariantArray.GetStructType(this._args.Length), "paramVariants");
				}
				return this._paramVariants;
			}
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x00215398 File Offset: 0x00213598
		private static ParameterExpression EnsureVariable(ref ParameterExpression var, Type type, string name)
		{
			if (var != null)
			{
				return var;
			}
			ParameterExpression result;
			var = (result = Expression.Variable(type, name));
			return result;
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x002153B8 File Offset: 0x002135B8
		private static Type MarshalType(DynamicMetaObject mo, bool isByRef)
		{
			Type type = (mo.Value == null && mo.HasValue && !mo.LimitType.IsValueType) ? null : mo.LimitType;
			if (isByRef)
			{
				if (type == null)
				{
					type = mo.Expression.Type;
				}
				type = type.MakeByRefType();
			}
			return type;
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x0021540C File Offset: 0x0021360C
		internal DynamicMetaObject Invoke()
		{
			this._keywordArgNames = this._callInfo.ArgumentNames.ToArray<string>();
			this._totalExplicitArgs = this._args.Length;
			Type[] array = new Type[this._args.Length];
			for (int i = 0; i < this._args.Length; i++)
			{
				DynamicMetaObject mo = this._args[i];
				array[i] = ComInvokeBinder.MarshalType(mo, this._isByRef[i]);
			}
			this._varEnumSelector = new VarEnumSelector(array);
			return new DynamicMetaObject(this.CreateScope(this.MakeIDispatchInvokeTarget()), BindingRestrictions.Combine(this._args).Merge(this._restrictions));
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x002154AB File Offset: 0x002136AB
		private static void AddNotNull(List<ParameterExpression> list, ParameterExpression var)
		{
			if (var != null)
			{
				list.Add(var);
			}
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x002154B8 File Offset: 0x002136B8
		private Expression CreateScope(Expression expression)
		{
			List<ParameterExpression> list = new List<ParameterExpression>();
			ComInvokeBinder.AddNotNull(list, this._dispatchObject);
			ComInvokeBinder.AddNotNull(list, this._dispatchPointer);
			ComInvokeBinder.AddNotNull(list, this._dispId);
			ComInvokeBinder.AddNotNull(list, this._dispParams);
			ComInvokeBinder.AddNotNull(list, this._paramVariants);
			ComInvokeBinder.AddNotNull(list, this._invokeResult);
			ComInvokeBinder.AddNotNull(list, this._returnValue);
			ComInvokeBinder.AddNotNull(list, this._dispIdsOfKeywordArgsPinned);
			ComInvokeBinder.AddNotNull(list, this._propertyPutDispId);
			if (list.Count <= 0)
			{
				return expression;
			}
			return Expression.Block(list, new Expression[]
			{
				expression
			});
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x00215554 File Offset: 0x00213754
		private Expression GenerateTryBlock()
		{
			ParameterExpression parameterExpression = Expression.Variable(typeof(ExcepInfo), "excepInfo");
			ParameterExpression parameterExpression2 = Expression.Variable(typeof(uint), "argErr");
			ParameterExpression parameterExpression3 = Expression.Variable(typeof(int), "hresult");
			List<Expression> list = new List<Expression>();
			if (this._keywordArgNames.Length > 0)
			{
				string[] value = this._keywordArgNames.AddFirst(this._methodDesc.Name);
				list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("rgdispidNamedArgs")), Expression.Call(typeof(UnsafeMethods).GetMethod("GetIdsOfNamedParameters"), this.DispatchObjectVariable, Expression.Constant(value), this.DispIdVariable, this.DispIdsOfKeywordArgsPinnedVariable)));
			}
			Expression[] array = this.MakeArgumentExpressions();
			int num = this._varEnumSelector.VariantBuilders.Length - 1;
			int num2 = this._varEnumSelector.VariantBuilders.Length - this._keywordArgNames.Length;
			int i = 0;
			while (i < this._varEnumSelector.VariantBuilders.Length)
			{
				int field;
				if (i >= num2)
				{
					field = i - num2;
				}
				else
				{
					field = num;
				}
				VariantBuilder variantBuilder = this._varEnumSelector.VariantBuilders[i];
				Expression expression = variantBuilder.InitializeArgumentVariant(VariantArray.GetStructField(this.ParamVariantsVariable, field), array[i + 1]);
				if (expression != null)
				{
					list.Add(expression);
				}
				i++;
				num--;
			}
			System.Runtime.InteropServices.ComTypes.INVOKEKIND invokekind;
			if (this._methodDesc.IsPropertyPut)
			{
				if (this._methodDesc.IsPropertyPutRef)
				{
					invokekind = System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF;
				}
				else
				{
					invokekind = System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT;
				}
			}
			else
			{
				invokekind = (System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC | System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET);
			}
			MethodCallExpression right = Expression.Call(typeof(UnsafeMethods).GetMethod("IDispatchInvoke"), new Expression[]
			{
				this.DispatchPointerVariable,
				this.DispIdVariable,
				Expression.Constant(invokekind),
				this.DispParamsVariable,
				this.InvokeResultVariable,
				parameterExpression,
				parameterExpression2
			});
			Expression item = Expression.Assign(parameterExpression3, right);
			list.Add(item);
			List<Expression> list2 = new List<Expression>();
			foreach (Expression expression2 in array)
			{
				list2.Add(Expression.TypeAs(expression2, typeof(object)));
			}
			item = Expression.Call(typeof(ComRuntimeHelpers).GetMethod("CheckThrowException"), parameterExpression3, parameterExpression, Expression.Constant(this._methodDesc, typeof(ComMethodDesc)), Expression.NewArrayInit(typeof(object), list2), parameterExpression2);
			list.Add(item);
			Expression right2 = Expression.Call(this.InvokeResultVariable, typeof(Variant).GetMethod("ToObject"));
			VariantBuilder[] variantBuilders = this._varEnumSelector.VariantBuilders;
			Expression[] array3 = this.MakeArgumentExpressions();
			list.Add(Expression.Assign(this.ReturnValueVariable, right2));
			int k = 0;
			int num3 = variantBuilders.Length;
			while (k < num3)
			{
				Expression expression3 = variantBuilders[k].UpdateFromReturn(array3[k + 1]);
				if (expression3 != null)
				{
					list.Add(expression3);
				}
				k++;
			}
			list.Add(Expression.Empty());
			return Expression.Block(new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2,
				parameterExpression3
			}, list);
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x00215898 File Offset: 0x00213A98
		private Expression GenerateFinallyBlock()
		{
			List<Expression> list = new List<Expression>();
			list.Add(Expression.Call(typeof(UnsafeMethods).GetMethod("IUnknownRelease"), this.DispatchPointerVariable));
			int i = 0;
			int num = this._varEnumSelector.VariantBuilders.Length;
			while (i < num)
			{
				Expression expression = this._varEnumSelector.VariantBuilders[i].Clear();
				if (expression != null)
				{
					list.Add(expression);
				}
				i++;
			}
			list.Add(Expression.Call(this.InvokeResultVariable, typeof(Variant).GetMethod("Clear")));
			if (this._dispIdsOfKeywordArgsPinned != null)
			{
				list.Add(Expression.Call(this.DispIdsOfKeywordArgsPinnedVariable, typeof(GCHandle).GetMethod("Free")));
			}
			list.Add(Expression.Empty());
			return Expression.Block(list);
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x0021596C File Offset: 0x00213B6C
		private Expression MakeIDispatchInvokeTarget()
		{
			List<Expression> list = new List<Expression>();
			list.Add(Expression.Assign(this.DispIdVariable, Expression.Property(this._method, typeof(ComMethodDesc).GetProperty("DispId"))));
			if (this._totalExplicitArgs != 0)
			{
				list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("rgvarg")), Expression.Call(typeof(UnsafeMethods).GetMethod("ConvertVariantByrefToPtr"), VariantArray.GetStructField(this.ParamVariantsVariable, 0))));
			}
			list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("cArgs")), Expression.Constant(this._totalExplicitArgs)));
			if (this._methodDesc.IsPropertyPut)
			{
				list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("cNamedArgs")), Expression.Constant(1)));
				list.Add(Expression.Assign(this.PropertyPutDispIdVariable, Expression.Constant(-3)));
				list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("rgdispidNamedArgs")), Expression.Call(typeof(UnsafeMethods).GetMethod("ConvertInt32ByrefToPtr"), this.PropertyPutDispIdVariable)));
			}
			else
			{
				list.Add(Expression.Assign(Expression.Field(this.DispParamsVariable, typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).GetField("cNamedArgs")), Expression.Constant(this._keywordArgNames.Length)));
			}
			list.Add(Expression.Assign(this.DispatchObjectVariable, this._dispatch));
			list.Add(Expression.Assign(this.DispatchPointerVariable, Expression.Call(typeof(Marshal).GetMethod("GetIDispatchForObject"), this.DispatchObjectVariable)));
			Expression body = this.GenerateTryBlock();
			Expression @finally = this.GenerateFinallyBlock();
			list.Add(Expression.TryFinally(body, @finally));
			list.Add(this.ReturnValueVariable);
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			foreach (VariantBuilder variantBuilder in this._varEnumSelector.VariantBuilders)
			{
				if (variantBuilder.TempVariable != null)
				{
					list2.Add(variantBuilder.TempVariable);
				}
			}
			if (this._methodDesc.ReturnType == typeof(void))
			{
				list.Add(ExpressionCache.AutomationNullConstant);
			}
			return Expression.Block(list2, list);
		}

		// Token: 0x06006A0E RID: 27150 RVA: 0x00215C08 File Offset: 0x00213E08
		private Expression[] MakeArgumentExpressions()
		{
			int num = 0;
			Expression[] array;
			if (this._instance != null)
			{
				array = new Expression[this._args.Length + 1];
				array[num++] = this._instance;
			}
			else
			{
				array = new Expression[this._args.Length];
			}
			for (int i = 0; i < this._args.Length; i++)
			{
				array[num++] = this._args[i].Expression;
			}
			return array;
		}

		// Token: 0x040032CB RID: 13003
		private readonly ComMethodDesc _methodDesc;

		// Token: 0x040032CC RID: 13004
		private readonly Expression _method;

		// Token: 0x040032CD RID: 13005
		private readonly Expression _dispatch;

		// Token: 0x040032CE RID: 13006
		private readonly CallInfo _callInfo;

		// Token: 0x040032CF RID: 13007
		private readonly DynamicMetaObject[] _args;

		// Token: 0x040032D0 RID: 13008
		private readonly bool[] _isByRef;

		// Token: 0x040032D1 RID: 13009
		private readonly Expression _instance;

		// Token: 0x040032D2 RID: 13010
		private BindingRestrictions _restrictions;

		// Token: 0x040032D3 RID: 13011
		private VarEnumSelector _varEnumSelector;

		// Token: 0x040032D4 RID: 13012
		private string[] _keywordArgNames;

		// Token: 0x040032D5 RID: 13013
		private int _totalExplicitArgs;

		// Token: 0x040032D6 RID: 13014
		private ParameterExpression _dispatchObject;

		// Token: 0x040032D7 RID: 13015
		private ParameterExpression _dispatchPointer;

		// Token: 0x040032D8 RID: 13016
		private ParameterExpression _dispId;

		// Token: 0x040032D9 RID: 13017
		private ParameterExpression _dispParams;

		// Token: 0x040032DA RID: 13018
		private ParameterExpression _paramVariants;

		// Token: 0x040032DB RID: 13019
		private ParameterExpression _invokeResult;

		// Token: 0x040032DC RID: 13020
		private ParameterExpression _returnValue;

		// Token: 0x040032DD RID: 13021
		private ParameterExpression _dispIdsOfKeywordArgsPinned;

		// Token: 0x040032DE RID: 13022
		private ParameterExpression _propertyPutDispId;
	}
}
