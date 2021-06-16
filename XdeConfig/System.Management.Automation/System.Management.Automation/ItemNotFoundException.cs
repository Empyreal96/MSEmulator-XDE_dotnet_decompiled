using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008AB RID: 2219
	[Serializable]
	public class ItemNotFoundException : SessionStateException
	{
		// Token: 0x060054BD RID: 21693 RVA: 0x001BF3C3 File Offset: 0x001BD5C3
		internal ItemNotFoundException(string path, string errorIdAndResourceId, string resourceStr) : base(path, SessionStateCategory.Drive, errorIdAndResourceId, resourceStr, ErrorCategory.ObjectNotFound, new object[0])
		{
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001BF3D7 File Offset: 0x001BD5D7
		public ItemNotFoundException()
		{
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001BF3DF File Offset: 0x001BD5DF
		public ItemNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x001BF3E8 File Offset: 0x001BD5E8
		public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x001BF3F2 File Offset: 0x001BD5F2
		protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
