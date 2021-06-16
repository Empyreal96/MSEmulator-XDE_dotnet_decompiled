using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class ApplicationRecoveryException : ExternalException
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00005C08 File Offset: 0x00003E08
		public ApplicationRecoveryException()
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00005C10 File Offset: 0x00003E10
		public ApplicationRecoveryException(string message) : base(message)
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00005C19 File Offset: 0x00003E19
		public ApplicationRecoveryException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00005C23 File Offset: 0x00003E23
		public ApplicationRecoveryException(string message, int errorCode) : base(message, errorCode)
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00005C2D File Offset: 0x00003E2D
		protected ApplicationRecoveryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
