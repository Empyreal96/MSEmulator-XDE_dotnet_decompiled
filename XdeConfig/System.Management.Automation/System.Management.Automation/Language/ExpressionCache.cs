using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x020005A0 RID: 1440
	internal static class ExpressionCache
	{
		// Token: 0x06003C9C RID: 15516 RVA: 0x00139FA0 File Offset: 0x001381A0
		internal static Expression Constant(int i)
		{
			if (i < -1 || i > 100)
			{
				return Expression.Constant(i);
			}
			Expression expression = ExpressionCache._intConstants[i + 1];
			if (expression == null)
			{
				expression = Expression.Constant(i);
				ExpressionCache._intConstants[i + 1] = expression;
			}
			return expression;
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x00139FE6 File Offset: 0x001381E6
		internal static Expression Constant(bool b)
		{
			if (!b)
			{
				return ExpressionCache.FalseConstant;
			}
			return ExpressionCache.TrueConstant;
		}

		// Token: 0x04001E86 RID: 7814
		internal static readonly Expression NullConstant = Expression.Constant(null);

		// Token: 0x04001E87 RID: 7815
		internal static readonly Expression NullExecutionContext = Expression.Constant(null, typeof(ExecutionContext));

		// Token: 0x04001E88 RID: 7816
		internal static readonly Expression NullPSObject = Expression.Constant(null, typeof(PSObject));

		// Token: 0x04001E89 RID: 7817
		internal static readonly Expression NullEnumerator = Expression.Constant(null, typeof(IEnumerator));

		// Token: 0x04001E8A RID: 7818
		internal static readonly Expression NullExtent = Expression.Constant(null, typeof(IScriptExtent));

		// Token: 0x04001E8B RID: 7819
		internal static readonly Expression NullTypeTable = Expression.Constant(null, typeof(TypeTable));

		// Token: 0x04001E8C RID: 7820
		internal static readonly Expression NullFormatProvider = Expression.Constant(null, typeof(IFormatProvider));

		// Token: 0x04001E8D RID: 7821
		internal static readonly Expression NullObjectArray = Expression.Constant(null, typeof(object[]));

		// Token: 0x04001E8E RID: 7822
		internal static readonly Expression AutomationNullConstant = Expression.Constant(AutomationNull.Value, typeof(object));

		// Token: 0x04001E8F RID: 7823
		internal static readonly Expression NullCommandRedirections = Expression.Constant(null, typeof(CommandRedirection[][]));

		// Token: 0x04001E90 RID: 7824
		internal static readonly Expression NullTypeArray = Expression.Constant(null, typeof(Type[]));

		// Token: 0x04001E91 RID: 7825
		internal static readonly Expression NullType = Expression.Constant(null, typeof(Type));

		// Token: 0x04001E92 RID: 7826
		internal static readonly Expression NullDelegateArray = Expression.Constant(null, typeof(Action<FunctionContext>[]));

		// Token: 0x04001E93 RID: 7827
		internal static readonly Expression NullPipe = Expression.Constant(new Pipe
		{
			NullPipe = true
		});

		// Token: 0x04001E94 RID: 7828
		internal static readonly Expression ConstEmptyString = Expression.Constant("");

		// Token: 0x04001E95 RID: 7829
		internal static readonly Expression CompareOptionsIgnoreCase = Expression.Constant(CompareOptions.IgnoreCase);

		// Token: 0x04001E96 RID: 7830
		internal static readonly Expression CompareOptionsNone = Expression.Constant(CompareOptions.None);

		// Token: 0x04001E97 RID: 7831
		internal static readonly Expression Ordinal = Expression.Constant(StringComparison.Ordinal);

		// Token: 0x04001E98 RID: 7832
		internal static readonly Expression InvariantCulture = Expression.Constant(CultureInfo.InvariantCulture);

		// Token: 0x04001E99 RID: 7833
		internal static readonly Expression CurrentCultureIgnoreCaseComparer = Expression.Constant(StringComparer.CurrentCultureIgnoreCase, typeof(StringComparer));

		// Token: 0x04001E9A RID: 7834
		internal static readonly Expression CatchAllType = Expression.Constant(typeof(ExceptionHandlingOps.CatchAll), typeof(Type));

		// Token: 0x04001E9B RID: 7835
		internal static readonly Expression Empty = Expression.Empty();

		// Token: 0x04001E9C RID: 7836
		internal static Expression GetExecutionContextFromTLS = Expression.Call(CachedReflectionInfo.LocalPipeline_GetExecutionContextFromTLS, new Expression[0]);

		// Token: 0x04001E9D RID: 7837
		internal static readonly Expression BoxedTrue = Expression.Field(null, typeof(Boxed).GetField("True", BindingFlags.Static | BindingFlags.NonPublic));

		// Token: 0x04001E9E RID: 7838
		internal static readonly Expression BoxedFalse = Expression.Field(null, typeof(Boxed).GetField("False", BindingFlags.Static | BindingFlags.NonPublic));

		// Token: 0x04001E9F RID: 7839
		private static readonly Expression[] _intConstants = new Expression[102];

		// Token: 0x04001EA0 RID: 7840
		internal static readonly Expression TrueConstant = Expression.Constant(true);

		// Token: 0x04001EA1 RID: 7841
		internal static readonly Expression FalseConstant = Expression.Constant(false);
	}
}
