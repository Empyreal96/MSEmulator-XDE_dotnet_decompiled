using System;
using System.Management.Automation.Language;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000895 RID: 2197
	[Serializable]
	internal class ParameterBindingValidationException : ParameterBindingException
	{
		// Token: 0x06005443 RID: 21571 RVA: 0x001BD3E8 File Offset: 0x001BB5E8
		internal ParameterBindingValidationException(ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x001BD40C File Offset: 0x001BB60C
		internal ParameterBindingValidationException(Exception innerException, ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(innerException, errorCategory, invocationInfo, errorPosition, parameterName, parameterType, typeSpecified, resourceString, errorId, args)
		{
			ValidationMetadataException ex = innerException as ValidationMetadataException;
			if (ex != null && ex.SwallowException)
			{
				this._swallowException = true;
			}
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x001BD449 File Offset: 0x001BB649
		protected ParameterBindingValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06005446 RID: 21574 RVA: 0x001BD453 File Offset: 0x001BB653
		internal bool SwallowException
		{
			get
			{
				return this._swallowException;
			}
		}

		// Token: 0x04002B31 RID: 11057
		private readonly bool _swallowException;
	}
}
