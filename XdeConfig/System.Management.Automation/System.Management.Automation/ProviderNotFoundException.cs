using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008A8 RID: 2216
	[Serializable]
	public class ProviderNotFoundException : SessionStateException
	{
		// Token: 0x060054AD RID: 21677 RVA: 0x001BF30C File Offset: 0x001BD50C
		internal ProviderNotFoundException(string itemName, SessionStateCategory sessionStateCategory, string errorIdAndResourceId, string resourceStr, params object[] messageArgs) : base(itemName, sessionStateCategory, errorIdAndResourceId, resourceStr, ErrorCategory.ObjectNotFound, messageArgs)
		{
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x001BF31D File Offset: 0x001BD51D
		public ProviderNotFoundException()
		{
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001BF325 File Offset: 0x001BD525
		public ProviderNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001BF32E File Offset: 0x001BD52E
		public ProviderNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x001BF338 File Offset: 0x001BD538
		protected ProviderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
