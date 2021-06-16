using System;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A78 RID: 2680
	internal class ConversionArgBuilder : ArgBuilder
	{
		// Token: 0x06006AB7 RID: 27319 RVA: 0x00218033 File Offset: 0x00216233
		internal ConversionArgBuilder(Type parameterType, SimpleArgBuilder innerBuilder)
		{
			this._parameterType = parameterType;
			this._innerBuilder = innerBuilder;
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x00218049 File Offset: 0x00216249
		internal override Expression Marshal(Expression parameter)
		{
			return this._innerBuilder.Marshal(Helpers.Convert(parameter, this._parameterType));
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x00218062 File Offset: 0x00216262
		internal override Expression MarshalToRef(Expression parameter)
		{
			throw Assert.Unreachable;
		}

		// Token: 0x04003322 RID: 13090
		private SimpleArgBuilder _innerBuilder;

		// Token: 0x04003323 RID: 13091
		private Type _parameterType;
	}
}
