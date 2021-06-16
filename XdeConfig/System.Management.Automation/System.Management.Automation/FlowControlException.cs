using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200047D RID: 1149
	public abstract class FlowControlException : SystemException
	{
		// Token: 0x0600333A RID: 13114 RVA: 0x001181D3 File Offset: 0x001163D3
		internal FlowControlException()
		{
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x001181DB File Offset: 0x001163DB
		internal FlowControlException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
