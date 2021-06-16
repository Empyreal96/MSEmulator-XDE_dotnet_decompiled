using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A95 RID: 2709
	internal class VariantArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006BA7 RID: 27559 RVA: 0x0021C324 File Offset: 0x0021A524
		internal VariantArgBuilder(Type parameterType) : base(parameterType)
		{
			this._isWrapper = (parameterType == typeof(VariantWrapper));
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x0021C344 File Offset: 0x0021A544
		internal override Expression Marshal(Expression parameter)
		{
			if (this._isWrapper)
			{
				parameter = Expression.Property(Helpers.Convert(parameter, typeof(VariantWrapper)), typeof(VariantWrapper).GetProperty("WrappedObject"));
			}
			return Helpers.Convert(parameter, typeof(object));
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x0021C394 File Offset: 0x0021A594
		internal override Expression MarshalToRef(Expression parameter)
		{
			parameter = this.Marshal(parameter);
			return Expression.Call(typeof(UnsafeMethods).GetMethod("GetVariantForObject", BindingFlags.Static | BindingFlags.NonPublic), parameter);
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x0021C3BC File Offset: 0x0021A5BC
		internal override Expression UnmarshalFromRef(Expression value)
		{
			Expression expression = Expression.Call(typeof(UnsafeMethods).GetMethod("GetObjectForVariant"), value);
			if (this._isWrapper)
			{
				expression = Expression.New(typeof(VariantWrapper).GetConstructor(new Type[]
				{
					typeof(object)
				}), new Expression[]
				{
					expression
				});
			}
			return base.UnmarshalFromRef(expression);
		}

		// Token: 0x04003362 RID: 13154
		private readonly bool _isWrapper;
	}
}
