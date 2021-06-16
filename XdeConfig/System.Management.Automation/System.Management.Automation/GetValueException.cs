using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016D RID: 365
	[Serializable]
	public class GetValueException : ExtendedTypeSystemException
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00074094 File Offset: 0x00072294
		public GetValueException() : base(typeof(GetValueException).FullName)
		{
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000740AB File Offset: 0x000722AB
		public GetValueException(string message) : base(message)
		{
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x000740B4 File Offset: 0x000722B4
		public GetValueException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x000740BE File Offset: 0x000722BE
		internal GetValueException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x000740CB File Offset: 0x000722CB
		protected GetValueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040007EB RID: 2027
		internal const string GetWithoutGetterExceptionMsg = "GetWithoutGetterException";

		// Token: 0x040007EC RID: 2028
		internal const string WriteOnlyProperty = "WriteOnlyProperty";
	}
}
