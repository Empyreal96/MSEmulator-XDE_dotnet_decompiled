using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000871 RID: 2161
	[Serializable]
	public class RedirectedException : RuntimeException
	{
		// Token: 0x060052DD RID: 21213 RVA: 0x001B975C File Offset: 0x001B795C
		public RedirectedException()
		{
			base.SetErrorId("RedirectedException");
			base.SetErrorCategory(ErrorCategory.NotSpecified);
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x001B9776 File Offset: 0x001B7976
		public RedirectedException(string message) : base(message)
		{
			base.SetErrorId("RedirectedException");
			base.SetErrorCategory(ErrorCategory.NotSpecified);
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001B9791 File Offset: 0x001B7991
		public RedirectedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId("RedirectedException");
			base.SetErrorCategory(ErrorCategory.NotSpecified);
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001B97AD File Offset: 0x001B79AD
		protected RedirectedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
