using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004C0 RID: 1216
	internal class BooleanEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x0600359D RID: 13725 RVA: 0x001242C8 File Offset: 0x001224C8
		internal BooleanEntryDefinition(string entryKey) : base(entryKey, null)
		{
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x001242D2 File Offset: 0x001224D2
		internal override object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			if (!originalParameterWasHashTable)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			return LanguagePrimitives.IsTrue(val);
		}
	}
}
