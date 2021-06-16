using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000482 RID: 1154
	public class ExitException : FlowControlException
	{
		// Token: 0x0600334E RID: 13134 RVA: 0x001182AA File Offset: 0x001164AA
		internal ExitException(object argument)
		{
			this.Argument = argument;
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x001182B9 File Offset: 0x001164B9
		// (set) Token: 0x06003350 RID: 13136 RVA: 0x001182C1 File Offset: 0x001164C1
		public object Argument { get; internal set; }

		// Token: 0x06003351 RID: 13137 RVA: 0x001182CA File Offset: 0x001164CA
		internal ExitException()
		{
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x001182D2 File Offset: 0x001164D2
		private ExitException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
