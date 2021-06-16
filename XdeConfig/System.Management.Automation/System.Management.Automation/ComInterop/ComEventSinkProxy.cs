using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A5B RID: 2651
	internal sealed class ComEventSinkProxy : RealProxy
	{
		// Token: 0x060069DD RID: 27101 RVA: 0x00214D6D File Offset: 0x00212F6D
		private ComEventSinkProxy()
		{
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x00214D75 File Offset: 0x00212F75
		public ComEventSinkProxy(ComEventSink sink, Guid sinkIid) : base(typeof(ComEventSink))
		{
			this._sink = sink;
			this._sinkIid = sinkIid;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x00214D98 File Offset: 0x00212F98
		public override IntPtr SupportsInterface(ref Guid iid)
		{
			if (iid == this._sinkIid)
			{
				IntPtr zero = IntPtr.Zero;
				return Marshal.GetIDispatchForObject(this._sink);
			}
			return base.SupportsInterface(ref iid);
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x00214DD4 File Offset: 0x00212FD4
		public override IMessage Invoke(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage == null)
			{
				throw new NotSupportedException();
			}
			MethodInfo left = (MethodInfo)methodCallMessage.MethodBase;
			if (left == ComEventSinkProxy._methodInfoInvokeMember)
			{
				object ret = null;
				try
				{
					ret = ((IReflect)this._sink).InvokeMember(methodCallMessage.Args[0] as string, (BindingFlags)methodCallMessage.Args[1], methodCallMessage.Args[2] as Binder, null, methodCallMessage.Args[4] as object[], methodCallMessage.Args[5] as ParameterModifier[], methodCallMessage.Args[6] as CultureInfo, null);
				}
				catch (Exception ex)
				{
					return new ReturnMessage(ex.InnerException, methodCallMessage);
				}
				return new ReturnMessage(ret, methodCallMessage.Args, methodCallMessage.ArgCount, null, methodCallMessage);
			}
			return RemotingServices.ExecuteMessage(this._sink, methodCallMessage);
		}

		// Token: 0x040032AA RID: 12970
		private Guid _sinkIid;

		// Token: 0x040032AB RID: 12971
		private ComEventSink _sink;

		// Token: 0x040032AC RID: 12972
		private static readonly MethodInfo _methodInfoInvokeMember = typeof(ComEventSink).GetMethod("InvokeMember", BindingFlags.Instance | BindingFlags.Public);
	}
}
