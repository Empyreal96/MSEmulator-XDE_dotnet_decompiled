using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FB RID: 2299
	internal interface IMethodInvoker
	{
		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06005619 RID: 22041
		Delegate Invoker { get; }

		// Token: 0x0600561A RID: 22042
		object[] CreateInvokerArgs(Delegate methodToInvoke, object[] methodToInvokeArgs);
	}
}
