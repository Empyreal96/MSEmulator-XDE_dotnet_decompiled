using System;
using System.Management.Automation.Language;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000896 RID: 2198
	[Serializable]
	internal class ParameterBindingArgumentTransformationException : ParameterBindingException
	{
		// Token: 0x06005447 RID: 21575 RVA: 0x001BD45C File Offset: 0x001BB65C
		internal ParameterBindingArgumentTransformationException(ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x001BD480 File Offset: 0x001BB680
		internal ParameterBindingArgumentTransformationException(Exception innerException, ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(innerException, errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x001BD4A4 File Offset: 0x001BB6A4
		protected ParameterBindingArgumentTransformationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
