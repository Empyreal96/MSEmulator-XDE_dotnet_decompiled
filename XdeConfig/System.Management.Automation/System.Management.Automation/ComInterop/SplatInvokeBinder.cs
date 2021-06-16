using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A65 RID: 2661
	internal sealed class SplatInvokeBinder : CallSiteBinder
	{
		// Token: 0x060069F9 RID: 27129 RVA: 0x00215130 File Offset: 0x00213330
		public override Expression Bind(object[] args, ReadOnlyCollection<ParameterExpression> parameters, LabelTarget returnLabel)
		{
			int num = ((object[])args[1]).Length;
			ParameterExpression array = parameters[1];
			ReadOnlyCollectionBuilder<Expression> readOnlyCollectionBuilder = new ReadOnlyCollectionBuilder<Expression>(num + 1);
			Type[] array2 = new Type[num + 3];
			readOnlyCollectionBuilder.Add(parameters[0]);
			array2[0] = typeof(CallSite);
			array2[1] = typeof(object);
			for (int i = 0; i < num; i++)
			{
				readOnlyCollectionBuilder.Add(Expression.ArrayAccess(array, new Expression[]
				{
					Expression.Constant(i)
				}));
				array2[i + 2] = typeof(object).MakeByRefType();
			}
			array2[array2.Length - 1] = typeof(object);
			return Expression.IfThen(Expression.Equal(Expression.ArrayLength(array), Expression.Constant(num)), Expression.Return(returnLabel, Expression.MakeDynamic(Expression.GetDelegateType(array2), new ComInvokeAction(new CallInfo(num, new string[0])), readOnlyCollectionBuilder)));
		}

		// Token: 0x040032CA RID: 13002
		internal static readonly SplatInvokeBinder Instance = new SplatInvokeBinder();
	}
}
