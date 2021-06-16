using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016B RID: 363
	[Serializable]
	public class MethodException : ExtendedTypeSystemException
	{
		// Token: 0x0600127E RID: 4734 RVA: 0x00074012 File Offset: 0x00072212
		public MethodException() : base(typeof(MethodException).FullName)
		{
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00074029 File Offset: 0x00072229
		public MethodException(string message) : base(message)
		{
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00074032 File Offset: 0x00072232
		public MethodException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0007403C File Offset: 0x0007223C
		internal MethodException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00074049 File Offset: 0x00072249
		protected MethodException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040007E3 RID: 2019
		internal const string MethodArgumentCountExceptionMsg = "MethodArgumentCountException";

		// Token: 0x040007E4 RID: 2020
		internal const string MethodAmbiguousExceptionMsg = "MethodAmbiguousException";

		// Token: 0x040007E5 RID: 2021
		internal const string MethodArgumentConversionExceptionMsg = "MethodArgumentConversionException";

		// Token: 0x040007E6 RID: 2022
		internal const string NonRefArgumentToRefParameterMsg = "NonRefArgumentToRefParameter";

		// Token: 0x040007E7 RID: 2023
		internal const string RefArgumentToNonRefParameterMsg = "RefArgumentToNonRefParameter";
	}
}
