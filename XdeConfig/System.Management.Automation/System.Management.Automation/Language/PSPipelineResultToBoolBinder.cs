using System;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x0200060A RID: 1546
	internal class PSPipelineResultToBoolBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004362 RID: 17250 RVA: 0x001627AC File Offset: 0x001609AC
		internal static PSPipelineResultToBoolBinder Get()
		{
			return PSPipelineResultToBoolBinder._binder;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x001627B3 File Offset: 0x001609B3
		private PSPipelineResultToBoolBinder()
		{
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06004364 RID: 17252 RVA: 0x001627BB File Offset: 0x001609BB
		public override Type ReturnType
		{
			get
			{
				return typeof(bool);
			}
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x001627C8 File Offset: 0x001609C8
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]).WriteToDebugLog(this);
			}
			IList list = target.Value as IList;
			Expression expression = target.Expression;
			if (!(typeof(IList) == expression.Type))
			{
				expression = Expression.Convert(expression, typeof(IList));
			}
			MemberExpression left = Expression.Property(Expression.Convert(expression, typeof(ICollection)), CachedReflectionInfo.ICollection_Count);
			Expression expression2;
			BindingRestrictions expressionRestriction;
			switch (list.Count)
			{
			case 0:
				expression2 = ExpressionCache.Constant(false);
				expressionRestriction = BindingRestrictions.GetExpressionRestriction(Expression.Equal(left, ExpressionCache.Constant(0)));
				break;
			case 1:
				expression2 = Expression.Call(expression, CachedReflectionInfo.IList_get_Item, new Expression[]
				{
					ExpressionCache.Constant(0)
				}).Convert(typeof(bool));
				expressionRestriction = BindingRestrictions.GetExpressionRestriction(Expression.Equal(left, ExpressionCache.Constant(1)));
				break;
			default:
				expression2 = ExpressionCache.Constant(true);
				expressionRestriction = BindingRestrictions.GetExpressionRestriction(Expression.GreaterThan(left, ExpressionCache.Constant(1)));
				break;
			}
			return new DynamicMetaObject(expression2, expressionRestriction).WriteToDebugLog(this);
		}

		// Token: 0x0400219B RID: 8603
		private static readonly PSPipelineResultToBoolBinder _binder = new PSPipelineResultToBoolBinder();
	}
}
