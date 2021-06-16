using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004BF RID: 1215
	internal class FormatStringDefinition : HashtableEntryDefinition
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x00124248 File Offset: 0x00122448
		internal FormatStringDefinition() : base("formatString", new Type[]
		{
			typeof(string)
		})
		{
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00124278 File Offset: 0x00122478
		internal override object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			if (!originalParameterWasHashTable)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			string text = val as string;
			if (string.IsNullOrEmpty(text))
			{
				string msg = StringUtil.Format(FormatAndOut_MshParameter.EmptyFormatStringValueError, base.KeyName);
				ParameterProcessor.ThrowParameterBindingException(invocationContext, "FormatStringEmpty", msg);
			}
			return new FieldFormattingDirective
			{
				formatString = text
			};
		}
	}
}
