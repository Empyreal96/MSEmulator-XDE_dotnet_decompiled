using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000869 RID: 2153
	[Serializable]
	public class ApplicationFailedException : RuntimeException
	{
		// Token: 0x060052AF RID: 21167 RVA: 0x001B925D File Offset: 0x001B745D
		protected ApplicationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x001B9267 File Offset: 0x001B7467
		public ApplicationFailedException()
		{
			base.SetErrorId("NativeCommandFailed");
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x001B9282 File Offset: 0x001B7482
		public ApplicationFailedException(string message) : base(message)
		{
			base.SetErrorId("NativeCommandFailed");
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x001B929E File Offset: 0x001B749E
		internal ApplicationFailedException(string message, string errorId) : base(message)
		{
			base.SetErrorId(errorId);
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x001B92B6 File Offset: 0x001B74B6
		internal ApplicationFailedException(string message, string errorId, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId(errorId);
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x001B92CF File Offset: 0x001B74CF
		public ApplicationFailedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId("NativeCommandFailed");
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x04002A87 RID: 10887
		private const string errorIdString = "NativeCommandFailed";
	}
}
