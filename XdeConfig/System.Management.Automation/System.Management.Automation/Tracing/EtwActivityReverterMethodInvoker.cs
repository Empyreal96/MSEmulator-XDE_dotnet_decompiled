using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FC RID: 2300
	internal class EtwActivityReverterMethodInvoker : IMethodInvoker
	{
		// Token: 0x0600561B RID: 22043 RVA: 0x001C3AFD File Offset: 0x001C1CFD
		public EtwActivityReverterMethodInvoker(IEtwEventCorrelator eventCorrelator)
		{
			if (eventCorrelator == null)
			{
				throw new ArgumentNullException("eventCorrelator");
			}
			this._eventCorrelator = eventCorrelator;
			this._invoker = new Func<Guid, Delegate, object[], object>(this.DoInvoke);
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x0600561C RID: 22044 RVA: 0x001C3B2C File Offset: 0x001C1D2C
		public Delegate Invoker
		{
			get
			{
				return this._invoker;
			}
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x001C3B34 File Offset: 0x001C1D34
		public object[] CreateInvokerArgs(Delegate methodToInvoke, object[] methodToInvokeArgs)
		{
			return new object[]
			{
				this._eventCorrelator.CurrentActivityId,
				methodToInvoke,
				methodToInvokeArgs
			};
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x001C3B68 File Offset: 0x001C1D68
		private object DoInvoke(Guid relatedActivityId, Delegate method, object[] methodArgs)
		{
			object result;
			using (this._eventCorrelator.StartActivity(relatedActivityId))
			{
				result = method.DynamicInvoke(methodArgs);
			}
			return result;
		}

		// Token: 0x04002DD2 RID: 11730
		private readonly IEtwEventCorrelator _eventCorrelator;

		// Token: 0x04002DD3 RID: 11731
		private readonly Func<Guid, Delegate, object[], object> _invoker;
	}
}
