using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Host
{
	// Token: 0x02000879 RID: 2169
	[Serializable]
	public class HostException : RuntimeException
	{
		// Token: 0x06005305 RID: 21253 RVA: 0x001B9E62 File Offset: 0x001B8062
		public HostException() : base(StringUtil.Format(HostInterfaceExceptionsStrings.DefaultCtorMessageTemplate, typeof(HostException).FullName))
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001B9E89 File Offset: 0x001B8089
		public HostException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001B9E98 File Offset: 0x001B8098
		public HostException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001B9EA8 File Offset: 0x001B80A8
		public HostException(string message, Exception innerException, string errorId, ErrorCategory errorCategory) : base(message, innerException)
		{
			base.SetErrorId(errorId);
			base.SetErrorCategory(errorCategory);
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x001B9EC1 File Offset: 0x001B80C1
		protected HostException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x001B9ECB File Offset: 0x001B80CB
		private void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
			base.SetErrorId(typeof(HostException).FullName);
		}
	}
}
