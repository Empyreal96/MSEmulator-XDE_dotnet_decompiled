using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A3 RID: 1187
	internal sealed class MshResolvedExpressionParameterAssociation
	{
		// Token: 0x06003507 RID: 13575 RVA: 0x0011FD1B File Offset: 0x0011DF1B
		internal MshResolvedExpressionParameterAssociation(MshParameter parameter, MshExpression expression)
		{
			if (expression == null)
			{
				throw PSTraceSource.NewArgumentNullException("expression");
			}
			this._originatingParameter = parameter;
			this._resolvedExpression = expression;
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003508 RID: 13576 RVA: 0x0011FD3F File Offset: 0x0011DF3F
		internal MshExpression ResolvedExpression
		{
			get
			{
				return this._resolvedExpression;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003509 RID: 13577 RVA: 0x0011FD47 File Offset: 0x0011DF47
		internal MshParameter OriginatingParameter
		{
			get
			{
				return this._originatingParameter;
			}
		}

		// Token: 0x04001B20 RID: 6944
		[TraceSource("MshResolvedExpressionParameterAssociation", "MshResolvedExpressionParameterAssociation")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("MshResolvedExpressionParameterAssociation", "MshResolvedExpressionParameterAssociation");

		// Token: 0x04001B21 RID: 6945
		private MshExpression _resolvedExpression;

		// Token: 0x04001B22 RID: 6946
		private MshParameter _originatingParameter;
	}
}
