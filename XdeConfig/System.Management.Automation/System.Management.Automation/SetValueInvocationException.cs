using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000171 RID: 369
	[Serializable]
	public class SetValueInvocationException : SetValueException
	{
		// Token: 0x0600129C RID: 4764 RVA: 0x00074198 File Offset: 0x00072398
		public SetValueInvocationException() : base(typeof(SetValueInvocationException).FullName)
		{
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000741AF File Offset: 0x000723AF
		public SetValueInvocationException(string message) : base(message)
		{
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x000741B8 File Offset: 0x000723B8
		public SetValueInvocationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000741C2 File Offset: 0x000723C2
		internal SetValueInvocationException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000741CF File Offset: 0x000723CF
		protected SetValueInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
