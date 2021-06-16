using System;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7A RID: 2682
	internal class ConvertibleArgBuilder : ArgBuilder
	{
		// Token: 0x06006ABD RID: 27325 RVA: 0x002180A4 File Offset: 0x002162A4
		internal ConvertibleArgBuilder()
		{
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x002180AC File Offset: 0x002162AC
		internal override Expression Marshal(Expression parameter)
		{
			return Helpers.Convert(parameter, typeof(IConvertible));
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x002180BE File Offset: 0x002162BE
		internal override Expression MarshalToRef(Expression parameter)
		{
			throw Assert.Unreachable;
		}
	}
}
