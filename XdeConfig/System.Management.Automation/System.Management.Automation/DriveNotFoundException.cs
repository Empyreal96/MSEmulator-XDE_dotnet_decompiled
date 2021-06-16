using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008AA RID: 2218
	[Serializable]
	public class DriveNotFoundException : SessionStateException
	{
		// Token: 0x060054B8 RID: 21688 RVA: 0x001BF38A File Offset: 0x001BD58A
		internal DriveNotFoundException(string itemName, string errorIdAndResourceId, string resourceStr) : base(itemName, SessionStateCategory.Drive, errorIdAndResourceId, resourceStr, ErrorCategory.ObjectNotFound, new object[0])
		{
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001BF39E File Offset: 0x001BD59E
		public DriveNotFoundException()
		{
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001BF3A6 File Offset: 0x001BD5A6
		public DriveNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001BF3AF File Offset: 0x001BD5AF
		public DriveNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x001BF3B9 File Offset: 0x001BD5B9
		protected DriveNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
