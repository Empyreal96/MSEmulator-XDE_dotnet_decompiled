using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8A RID: 2698
	internal class StringArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006B2C RID: 27436 RVA: 0x00219E45 File Offset: 0x00218045
		internal StringArgBuilder(Type parameterType) : base(parameterType)
		{
			this._isWrapper = (parameterType == typeof(BStrWrapper));
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x00219E64 File Offset: 0x00218064
		internal override Expression Marshal(Expression parameter)
		{
			parameter = base.Marshal(parameter);
			if (this._isWrapper)
			{
				parameter = Expression.Property(Helpers.Convert(parameter, typeof(BStrWrapper)), typeof(BStrWrapper).GetProperty("WrappedObject"));
			}
			return parameter;
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x00219EA3 File Offset: 0x002180A3
		internal override Expression MarshalToRef(Expression parameter)
		{
			parameter = this.Marshal(parameter);
			return Expression.Call(typeof(Marshal).GetMethod("StringToBSTR"), parameter);
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x00219EC8 File Offset: 0x002180C8
		internal override Expression UnmarshalFromRef(Expression value)
		{
			Expression expression = Expression.Condition(Expression.Equal(value, Expression.Constant(IntPtr.Zero)), Expression.Constant(null, typeof(string)), Expression.Call(typeof(Marshal).GetMethod("PtrToStringBSTR"), value));
			if (this._isWrapper)
			{
				expression = Expression.New(typeof(BStrWrapper).GetConstructor(new Type[]
				{
					typeof(string)
				}), new Expression[]
				{
					expression
				});
			}
			return base.UnmarshalFromRef(expression);
		}

		// Token: 0x04003339 RID: 13113
		private readonly bool _isWrapper;
	}
}
