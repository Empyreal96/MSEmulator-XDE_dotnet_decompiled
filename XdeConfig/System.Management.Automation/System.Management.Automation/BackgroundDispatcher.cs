using System;
using System.Diagnostics.Eventing;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020008D6 RID: 2262
	public class BackgroundDispatcher : IBackgroundDispatcher
	{
		// Token: 0x0600556D RID: 21869 RVA: 0x001C1F7F File Offset: 0x001C017F
		public BackgroundDispatcher(EventProvider transferProvider, EventDescriptor transferEvent) : this(new EtwActivityReverterMethodInvoker(new EtwEventCorrelator(transferProvider, transferEvent)))
		{
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x001C1F93 File Offset: 0x001C0193
		internal BackgroundDispatcher(IMethodInvoker etwActivityMethodInvoker)
		{
			if (etwActivityMethodInvoker == null)
			{
				throw new ArgumentNullException("etwActivityMethodInvoker");
			}
			this._etwActivityMethodInvoker = etwActivityMethodInvoker;
			this._invokerWaitCallback = new WaitCallback(this.DoInvoker);
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x001C1FC4 File Offset: 0x001C01C4
		private void DoInvoker(object invokerArgs)
		{
			object[] args = (object[])invokerArgs;
			this._etwActivityMethodInvoker.Invoker.DynamicInvoke(args);
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x001C1FEA File Offset: 0x001C01EA
		public bool QueueUserWorkItem(WaitCallback callback)
		{
			return this.QueueUserWorkItem(callback, null);
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x001C1FF4 File Offset: 0x001C01F4
		public bool QueueUserWorkItem(WaitCallback callback, object state)
		{
			object[] state2 = this._etwActivityMethodInvoker.CreateInvokerArgs(callback, new object[]
			{
				state
			});
			return ThreadPool.QueueUserWorkItem(this._invokerWaitCallback, state2);
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x001C2028 File Offset: 0x001C0228
		public IAsyncResult BeginInvoke(WaitCallback callback, object state, AsyncCallback completionCallback, object asyncState)
		{
			object[] state2 = this._etwActivityMethodInvoker.CreateInvokerArgs(callback, new object[]
			{
				state
			});
			return this._invokerWaitCallback.BeginInvoke(state2, completionCallback, asyncState);
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x001C205F File Offset: 0x001C025F
		public void EndInvoke(IAsyncResult asyncResult)
		{
			this._invokerWaitCallback.EndInvoke(asyncResult);
		}

		// Token: 0x04002CC2 RID: 11458
		private readonly IMethodInvoker _etwActivityMethodInvoker;

		// Token: 0x04002CC3 RID: 11459
		private readonly WaitCallback _invokerWaitCallback;
	}
}
