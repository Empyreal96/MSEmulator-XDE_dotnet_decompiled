using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016F RID: 367
	[Serializable]
	public class GetValueInvocationException : GetValueException
	{
		// Token: 0x06001292 RID: 4754 RVA: 0x00074116 File Offset: 0x00072316
		public GetValueInvocationException() : base(typeof(GetValueInvocationException).FullName)
		{
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0007412D File Offset: 0x0007232D
		public GetValueInvocationException(string message) : base(message)
		{
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00074136 File Offset: 0x00072336
		public GetValueInvocationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00074140 File Offset: 0x00072340
		internal GetValueInvocationException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0007414D File Offset: 0x0007234D
		protected GetValueInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040007ED RID: 2029
		internal const string ExceptionWhenGettingMsg = "ExceptionWhenGetting";
	}
}
