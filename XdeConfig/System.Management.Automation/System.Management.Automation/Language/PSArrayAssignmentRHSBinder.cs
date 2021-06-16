using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000608 RID: 1544
	internal class PSArrayAssignmentRHSBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004356 RID: 17238 RVA: 0x00162318 File Offset: 0x00160518
		internal static PSArrayAssignmentRHSBinder Get(int i)
		{
			PSArrayAssignmentRHSBinder result;
			lock (PSArrayAssignmentRHSBinder._binders)
			{
				while (PSArrayAssignmentRHSBinder._binders.Count <= i)
				{
					PSArrayAssignmentRHSBinder._binders.Add(null);
				}
				PSArrayAssignmentRHSBinder psarrayAssignmentRHSBinder;
				if ((psarrayAssignmentRHSBinder = PSArrayAssignmentRHSBinder._binders[i]) == null)
				{
					psarrayAssignmentRHSBinder = (PSArrayAssignmentRHSBinder._binders[i] = new PSArrayAssignmentRHSBinder(i));
				}
				result = psarrayAssignmentRHSBinder;
			}
			return result;
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00162390 File Offset: 0x00160590
		private PSArrayAssignmentRHSBinder(int elements)
		{
			this._elements = elements;
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x001623A0 File Offset: 0x001605A0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "MultiAssignRHSBinder {0}", new object[]
			{
				this._elements
			});
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06004359 RID: 17241 RVA: 0x001623D2 File Offset: 0x001605D2
		public override Type ReturnType
		{
			get
			{
				return typeof(IList);
			}
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x001623E0 File Offset: 0x001605E0
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]).WriteToDebugLog(this);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				}).WriteToDebugLog(this);
			}
			IList list = target.Value as IList;
			if (list == null)
			{
				return new DynamicMetaObject(Expression.NewArrayInit(typeof(object), Enumerable.Repeat<Expression>(ExpressionCache.NullConstant, this._elements - 1).Prepend(target.Expression.Cast(typeof(object)))), target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			MemberExpression left = Expression.Property(target.Expression.Cast(typeof(ICollection)), CachedReflectionInfo.ICollection_Count);
			BindingRestrictions restrictions = target.PSGetTypeRestriction().Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(left, ExpressionCache.Constant(list.Count))));
			if (list.Count == this._elements)
			{
				return new DynamicMetaObject(target.Expression.Cast(typeof(IList)), restrictions).WriteToDebugLog(this);
			}
			Expression[] array = new Expression[this._elements];
			ParameterExpression parameterExpression = Expression.Variable(typeof(IList));
			if (list.Count < this._elements)
			{
				int i;
				for (i = 0; i < list.Count; i++)
				{
					array[i] = Expression.Call(parameterExpression, CachedReflectionInfo.IList_get_Item, new Expression[]
					{
						ExpressionCache.Constant(i)
					});
				}
				while (i < this._elements)
				{
					array[i] = ExpressionCache.NullConstant;
					i++;
				}
			}
			else
			{
				for (int i = 0; i < this._elements - 1; i++)
				{
					array[i] = Expression.Call(parameterExpression, CachedReflectionInfo.IList_get_Item, new Expression[]
					{
						ExpressionCache.Constant(i)
					});
				}
				array[this._elements - 1] = Expression.Call(CachedReflectionInfo.EnumerableOps_GetSlice, parameterExpression, ExpressionCache.Constant(this._elements - 1)).Cast(typeof(object));
			}
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, target.Expression.Cast(typeof(IList))),
				Expression.NewArrayInit(typeof(object), array)
			}), restrictions).WriteToDebugLog(this);
		}

		// Token: 0x04002198 RID: 8600
		private static readonly List<PSArrayAssignmentRHSBinder> _binders = new List<PSArrayAssignmentRHSBinder>();

		// Token: 0x04002199 RID: 8601
		private readonly int _elements;
	}
}
