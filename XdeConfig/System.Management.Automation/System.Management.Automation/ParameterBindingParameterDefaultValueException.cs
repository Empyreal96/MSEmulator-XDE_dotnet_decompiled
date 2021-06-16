using System;
using System.Management.Automation.Language;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000897 RID: 2199
	[Serializable]
	internal class ParameterBindingParameterDefaultValueException : ParameterBindingException
	{
		// Token: 0x0600544A RID: 21578 RVA: 0x001BD4B0 File Offset: 0x001BB6B0
		internal ParameterBindingParameterDefaultValueException(ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x001BD4D4 File Offset: 0x001BB6D4
		internal ParameterBindingParameterDefaultValueException(Exception innerException, ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(innerException, errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x001BD4F8 File Offset: 0x001BB6F8
		protected ParameterBindingParameterDefaultValueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
