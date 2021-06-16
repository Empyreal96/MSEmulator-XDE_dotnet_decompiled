using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000969 RID: 2409
	internal static class DisplayCondition
	{
		// Token: 0x06005859 RID: 22617 RVA: 0x001CBBE8 File Offset: 0x001C9DE8
		internal static bool Evaluate(PSObject obj, MshExpression ex, out MshExpressionResult expressionResult)
		{
			expressionResult = null;
			List<MshExpressionResult> values = ex.GetValues(obj);
			if (values.Count == 0)
			{
				return false;
			}
			if (values[0].Exception != null)
			{
				expressionResult = values[0];
				return false;
			}
			return LanguagePrimitives.IsTrue(values[0].Result);
		}
	}
}
