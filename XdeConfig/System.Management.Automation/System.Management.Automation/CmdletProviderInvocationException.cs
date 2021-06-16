using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200086C RID: 2156
	[Serializable]
	public class CmdletProviderInvocationException : CmdletInvocationException
	{
		// Token: 0x060052BD RID: 21181 RVA: 0x001B9468 File Offset: 0x001B7668
		internal CmdletProviderInvocationException(ProviderInvocationException innerException, InvocationInfo myInvocation) : base(CmdletProviderInvocationException.GetInnerException(innerException), myInvocation)
		{
			if (innerException == null)
			{
				throw new ArgumentNullException("innerException");
			}
			this._providerInvocationException = innerException;
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x001B948C File Offset: 0x001B768C
		public CmdletProviderInvocationException()
		{
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x001B9494 File Offset: 0x001B7694
		protected CmdletProviderInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._providerInvocationException = (base.InnerException as ProviderInvocationException);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x001B94AF File Offset: 0x001B76AF
		public CmdletProviderInvocationException(string message) : base(message)
		{
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x001B94B8 File Offset: 0x001B76B8
		public CmdletProviderInvocationException(string message, Exception innerException) : base(message, innerException)
		{
			this._providerInvocationException = (innerException as ProviderInvocationException);
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x060052C2 RID: 21186 RVA: 0x001B94CE File Offset: 0x001B76CE
		public ProviderInvocationException ProviderInvocationException
		{
			get
			{
				return this._providerInvocationException;
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x060052C3 RID: 21187 RVA: 0x001B94D6 File Offset: 0x001B76D6
		public ProviderInfo ProviderInfo
		{
			get
			{
				if (this._providerInvocationException != null)
				{
					return this._providerInvocationException.ProviderInfo;
				}
				return null;
			}
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x001B94ED File Offset: 0x001B76ED
		private static Exception GetInnerException(Exception e)
		{
			if (e != null)
			{
				return e.InnerException;
			}
			return null;
		}

		// Token: 0x04002AAE RID: 10926
		[NonSerialized]
		private ProviderInvocationException _providerInvocationException;
	}
}
