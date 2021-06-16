using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016C RID: 364
	[Serializable]
	public class MethodInvocationException : MethodException
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x00074053 File Offset: 0x00072253
		public MethodInvocationException() : base(typeof(MethodInvocationException).FullName)
		{
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0007406A File Offset: 0x0007226A
		public MethodInvocationException(string message) : base(message)
		{
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00074073 File Offset: 0x00072273
		public MethodInvocationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0007407D File Offset: 0x0007227D
		internal MethodInvocationException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0007408A File Offset: 0x0007228A
		protected MethodInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040007E8 RID: 2024
		internal const string MethodInvocationExceptionMsg = "MethodInvocationException";

		// Token: 0x040007E9 RID: 2025
		internal const string CopyToInvocationExceptionMsg = "CopyToInvocationException";

		// Token: 0x040007EA RID: 2026
		internal const string WMIMethodInvocationException = "WMIMethodInvocationException";
	}
}
