using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008A7 RID: 2215
	[Serializable]
	public class SessionStateUnauthorizedAccessException : SessionStateException
	{
		// Token: 0x060054A8 RID: 21672 RVA: 0x001BF2D2 File Offset: 0x001BD4D2
		internal SessionStateUnauthorizedAccessException(string itemName, SessionStateCategory sessionStateCategory, string errorIdAndResourceId, string resourceStr) : base(itemName, sessionStateCategory, errorIdAndResourceId, resourceStr, ErrorCategory.WriteError, new object[0])
		{
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x001BF2E7 File Offset: 0x001BD4E7
		public SessionStateUnauthorizedAccessException()
		{
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x001BF2EF File Offset: 0x001BD4EF
		public SessionStateUnauthorizedAccessException(string message) : base(message)
		{
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001BF2F8 File Offset: 0x001BD4F8
		public SessionStateUnauthorizedAccessException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001BF302 File Offset: 0x001BD502
		protected SessionStateUnauthorizedAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
