using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8F RID: 2703
	internal class UnknownArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006B49 RID: 27465 RVA: 0x0021A780 File Offset: 0x00218980
		internal UnknownArgBuilder(Type parameterType) : base(parameterType)
		{
			this._isWrapper = (parameterType == typeof(UnknownWrapper));
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x0021A7A0 File Offset: 0x002189A0
		internal override Expression Marshal(Expression parameter)
		{
			parameter = base.Marshal(parameter);
			if (this._isWrapper)
			{
				parameter = Expression.Property(Helpers.Convert(parameter, typeof(UnknownWrapper)), typeof(UnknownWrapper).GetProperty("WrappedObject"));
			}
			return Helpers.Convert(parameter, typeof(object));
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x0021A7FC File Offset: 0x002189FC
		internal override Expression MarshalToRef(Expression parameter)
		{
			parameter = this.Marshal(parameter);
			return Expression.Condition(Expression.Equal(parameter, Expression.Constant(null)), Expression.Constant(IntPtr.Zero), Expression.Call(typeof(Marshal).GetMethod("GetIUnknownForObject"), parameter));
		}

		// Token: 0x06006B4C RID: 27468 RVA: 0x0021A84C File Offset: 0x00218A4C
		internal override Expression UnmarshalFromRef(Expression value)
		{
			Expression expression = Expression.Condition(Expression.Equal(value, Expression.Constant(IntPtr.Zero)), Expression.Constant(null), Expression.Call(typeof(Marshal).GetMethod("GetObjectForIUnknown"), value));
			if (this._isWrapper)
			{
				expression = Expression.New(typeof(UnknownWrapper).GetConstructor(new Type[]
				{
					typeof(object)
				}), new Expression[]
				{
					expression
				});
			}
			return base.UnmarshalFromRef(expression);
		}

		// Token: 0x0400333F RID: 13119
		private readonly bool _isWrapper;
	}
}
