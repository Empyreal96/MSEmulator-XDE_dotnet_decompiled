using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000914 RID: 2324
	internal class MshExpressionResult
	{
		// Token: 0x0600573E RID: 22334 RVA: 0x001C80D5 File Offset: 0x001C62D5
		internal MshExpressionResult(object res, MshExpression re, Exception e)
		{
			this._result = res;
			this._resolvedExpression = re;
			this._exception = e;
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x001C80F2 File Offset: 0x001C62F2
		internal object Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06005740 RID: 22336 RVA: 0x001C80FA File Offset: 0x001C62FA
		internal MshExpression ResolvedExpression
		{
			get
			{
				return this._resolvedExpression;
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x001C8102 File Offset: 0x001C6302
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x04002E75 RID: 11893
		private object _result;

		// Token: 0x04002E76 RID: 11894
		private MshExpression _resolvedExpression;

		// Token: 0x04002E77 RID: 11895
		private Exception _exception;
	}
}
