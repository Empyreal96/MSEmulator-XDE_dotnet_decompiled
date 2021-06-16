using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A4D RID: 2637
	internal abstract class ArgBuilder
	{
		// Token: 0x06006991 RID: 27025
		internal abstract Expression Marshal(Expression parameter);

		// Token: 0x06006992 RID: 27026 RVA: 0x00213FAC File Offset: 0x002121AC
		internal virtual Expression MarshalToRef(Expression parameter)
		{
			return this.Marshal(parameter);
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x00213FB5 File Offset: 0x002121B5
		internal virtual Expression UnmarshalFromRef(Expression newValue)
		{
			return newValue;
		}
	}
}
