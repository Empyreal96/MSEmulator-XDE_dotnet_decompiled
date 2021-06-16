using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008A6 RID: 2214
	[Serializable]
	public class SessionStateOverflowException : SessionStateException
	{
		// Token: 0x060054A3 RID: 21667 RVA: 0x001BF29D File Offset: 0x001BD49D
		internal SessionStateOverflowException(string itemName, SessionStateCategory sessionStateCategory, string errorIdAndResourceId, string resourceStr, params object[] messageArgs) : base(itemName, sessionStateCategory, errorIdAndResourceId, resourceStr, ErrorCategory.InvalidOperation, messageArgs)
		{
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x001BF2AD File Offset: 0x001BD4AD
		public SessionStateOverflowException()
		{
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001BF2B5 File Offset: 0x001BD4B5
		public SessionStateOverflowException(string message) : base(message)
		{
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001BF2BE File Offset: 0x001BD4BE
		public SessionStateOverflowException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x001BF2C8 File Offset: 0x001BD4C8
		protected SessionStateOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
