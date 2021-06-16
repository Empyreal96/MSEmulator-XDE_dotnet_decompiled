using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004BD RID: 1213
	internal class WidthEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x06003597 RID: 13719 RVA: 0x0012418C File Offset: 0x0012238C
		internal WidthEntryDefinition() : base("width", new Type[]
		{
			typeof(int)
		})
		{
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x001241B9 File Offset: 0x001223B9
		internal override object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			if (!originalParameterWasHashTable)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			this.VerifyRange((int)val, invocationContext);
			return null;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x001241D4 File Offset: 0x001223D4
		private void VerifyRange(int width, TerminatingErrorContext invocationContext)
		{
			if (width <= 0)
			{
				string msg = StringUtil.Format(FormatAndOut_MshParameter.OutOfRangeWidthValueError, width, base.KeyName);
				ParameterProcessor.ThrowParameterBindingException(invocationContext, "WidthOutOfRange", msg);
			}
		}
	}
}
