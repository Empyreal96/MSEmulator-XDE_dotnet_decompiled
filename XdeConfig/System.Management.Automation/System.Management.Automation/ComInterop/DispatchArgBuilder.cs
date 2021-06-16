using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7D RID: 2685
	internal class DispatchArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006AC7 RID: 27335 RVA: 0x002181C2 File Offset: 0x002163C2
		internal DispatchArgBuilder(Type parameterType) : base(parameterType)
		{
			this._isWrapper = (parameterType == typeof(DispatchWrapper));
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x002181E4 File Offset: 0x002163E4
		internal override Expression Marshal(Expression parameter)
		{
			parameter = base.Marshal(parameter);
			if (this._isWrapper)
			{
				parameter = Expression.Property(Helpers.Convert(parameter, typeof(DispatchWrapper)), typeof(DispatchWrapper).GetProperty("WrappedObject"));
			}
			return Helpers.Convert(parameter, typeof(object));
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x00218240 File Offset: 0x00216440
		internal override Expression MarshalToRef(Expression parameter)
		{
			parameter = this.Marshal(parameter);
			return Expression.Condition(Expression.Equal(parameter, Expression.Constant(null)), Expression.Constant(IntPtr.Zero), Expression.Call(typeof(Marshal).GetMethod("GetIDispatchForObject"), parameter));
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x00218290 File Offset: 0x00216490
		internal override Expression UnmarshalFromRef(Expression value)
		{
			Expression expression = Expression.Condition(Expression.Equal(value, Expression.Constant(IntPtr.Zero)), Expression.Constant(null), Expression.Call(typeof(Marshal).GetMethod("GetObjectForIUnknown"), value));
			if (this._isWrapper)
			{
				expression = Expression.New(typeof(DispatchWrapper).GetConstructor(new Type[]
				{
					typeof(object)
				}), new Expression[]
				{
					expression
				});
			}
			return base.UnmarshalFromRef(expression);
		}

		// Token: 0x04003325 RID: 13093
		private readonly bool _isWrapper;
	}
}
