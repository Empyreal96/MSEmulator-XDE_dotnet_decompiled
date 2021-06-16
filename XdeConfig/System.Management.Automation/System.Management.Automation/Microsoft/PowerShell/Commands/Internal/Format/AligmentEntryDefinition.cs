using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004BC RID: 1212
	internal class AligmentEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x06003593 RID: 13715 RVA: 0x00124074 File Offset: 0x00122274
		internal AligmentEntryDefinition() : base("alignment", new Type[]
		{
			typeof(string)
		})
		{
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x001240A4 File Offset: 0x001222A4
		internal override object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			if (!originalParameterWasHashTable)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			string text = val as string;
			if (!string.IsNullOrEmpty(text))
			{
				int i = 0;
				while (i < AligmentEntryDefinition.legalValues.Length)
				{
					if (CommandParameterDefinition.FindPartialMatch(text, AligmentEntryDefinition.legalValues[i]))
					{
						if (i == 0)
						{
							return 1;
						}
						if (i == 1)
						{
							return 2;
						}
						return 3;
					}
					else
					{
						i++;
					}
				}
			}
			this.ProcessIllegalValue(text, invocationContext);
			return null;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x00124110 File Offset: 0x00122310
		private void ProcessIllegalValue(string s, TerminatingErrorContext invocationContext)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.IllegalAlignmentValueError, new object[]
			{
				s,
				base.KeyName,
				ParameterProcessor.CatenateStringArray(AligmentEntryDefinition.legalValues)
			});
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "AlignmentIllegalValue", msg);
		}

		// Token: 0x04001B62 RID: 7010
		private const string LeftAlign = "left";

		// Token: 0x04001B63 RID: 7011
		private const string CenterAlign = "center";

		// Token: 0x04001B64 RID: 7012
		private const string RightAlign = "right";

		// Token: 0x04001B65 RID: 7013
		private static readonly string[] legalValues = new string[]
		{
			"left",
			"center",
			"right"
		};
	}
}
