using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000170 RID: 368
	[Serializable]
	public class SetValueException : ExtendedTypeSystemException
	{
		// Token: 0x06001297 RID: 4759 RVA: 0x00074157 File Offset: 0x00072357
		public SetValueException() : base(typeof(SetValueException).FullName)
		{
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0007416E File Offset: 0x0007236E
		public SetValueException(string message) : base(message)
		{
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00074177 File Offset: 0x00072377
		public SetValueException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00074181 File Offset: 0x00072381
		internal SetValueException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0007418E File Offset: 0x0007238E
		protected SetValueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
